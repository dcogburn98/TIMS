using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using PaymentEngine.xTransaction;
using System.IO;
using System.Text;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;

using TIMSServerModel;

namespace TIMSServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EmployeeModel" in both code and config file together.
    public class TIMSServiceModel : ITIMSServiceModel
    {
        private static SqliteConnection sqlite_conn = Program.sqlite_conn;
        private static void OpenConnection()
        {
            Program.OpenConnection();
        }
        private static void CloseConnection()
        {
            Program.CloseConnection();
        }

        private static List<AuthKey> Keys = new List<AuthKey>();
        private static AuthKey BypassKey = new AuthKey("3ncrYqtEdbypa$$K3yF0rInt3rna1u$e");

        #region Employees
        
        public string CheckEmployee(string input)
        {
            Console.WriteLine("Validating employee: " + input);
            if (!int.TryParse(input, out int v))
                input = "'" + input + "'";
            string value = null;
            Program.OpenConnection();
            SqliteCommand command = sqlite_conn.CreateCommand();

            command.CommandText =
                "SELECT FULLNAME " +
                "FROM EMPLOYEES " +
                "WHERE USERNAME = " + input + " " +
                "OR EMPLOYEENUMBER = " + input;

            SqliteDataReader rdr = command.ExecuteReader();
            while (rdr.Read())
            {
                value = $"{rdr.GetString(0)}";
            }

            Program.CloseConnection();

            if (value == null)
                return null;
            else
                return value;
        }
        public Employee Login(string user, byte[] pass)
        {
            Console.WriteLine("Login Called for user: " + user);
            #region GetIP
            string addr = "";
            if (!DeviceExists(addr))
            {
                Console.WriteLine("Terminal (" + addr + ") being used to login for user \"" + user + "\" is not currently enrolled in the system. Please type \"accept\" + [Device Nickname] to enroll this terminal, otherwise type anything else.");
                
                
                string input = Console.ReadLine();
                if (input.Split(' ')[0].ToLower() == "accept")
                {
                    if (input.Split(' ').Length < 2)
                    {
                        Console.WriteLine("Please specify a nickname for this terminal: ");
                        input = input + " " + Console.ReadLine();
                    }
                    Console.WriteLine("Enrolling device...");
                    //AddTerminal(addr, input.Split(' ')[1]);
                    Console.WriteLine("Device Enrolled. Logging in.");
                }
                else
                {
                    Console.WriteLine("Device enrollment rejected. Please verify the origin of this connection, it could be malicious.");
                    //if (!File.Exists("LoginAttempts.log")) { File.Create("LoginAttempts.log").Close(); }
                    FileStream stream = File.Open("loginAttempts.log", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, (int)stream.Length);
                    string text = Encoding.ASCII.GetString(buffer);
                    byte[] data = Encoding.ASCII.GetBytes(text + "[" + DateTime.Now.ToString() + "] " + addr + "\n");
                    stream.Write(new byte[0], 0, 0);
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                    return null;
                }
            }
            #endregion
            //System.Threading.Thread.Sleep(1000); Uncomment before release
            Employee e = new Employee();

            #region Authorization Initialization
            e.key = new AuthKey();
            AuthKey copy = new AuthKey(e.key);
            Keys.Add(copy);
            #endregion

            if (!int.TryParse(user, out int v))
                user = "'" + user + "'";
            Program.OpenConnection();

            SqliteCommand sqlite_cmd;
            sqlite_cmd = Program.sqlite_conn.CreateCommand();

            sqlite_cmd.CommandText =
                "SELECT EMPLOYEENUMBER, FULLNAME, USERNAME, POSITION, BIRTHDATE, HIREDATE, TERMINATIONDATE, PERMISSIONS, STARTUPSCREEN, COMMISSIONED, COMMISSIONRATE, WAGED, HOURLYWAGE, PAYPERIOD, PASSWORDHASH" + " " +
                "FROM EMPLOYEES" + " " +
                "WHERE (USERNAME = " + user + " " +
                "OR EMPLOYEENUMBER = " + user + ") " +
                "AND PASSWORDHASH = $pass";

            SqliteParameter hash = new SqliteParameter("$pass", System.Data.DbType.Binary)
            {
                Value = pass
            };
            sqlite_cmd.Parameters.Add(hash);

            SqliteDataReader rdr = sqlite_cmd.ExecuteReader(System.Data.CommandBehavior.Default);
            if (!rdr.HasRows)
            {
                Program.CloseConnection();
                return null;
            }
            while (rdr.Read())
            {
                e.employeeNumber = rdr.GetInt32(0);
                e.fullName = rdr.GetString(1);
                e.username = rdr.GetString(2);
                e.position = rdr.GetString(3);
                DateTime.TryParse(rdr.GetString(4), out e.birthDate);
                DateTime.TryParse(rdr.GetString(5), out e.hireDate);
                DateTime.TryParse(rdr.GetString(6), out e.terminationDate);
                if (rdr.GetString(7) == "ALL")
                    foreach (string p in Enum.GetNames(typeof(Employee.EmployeePermissions)))
                        e.employeePermissions.Add((Employee.EmployeePermissions)Enum.Parse(typeof(Employee.EmployeePermissions), p));
                else
                    foreach (string p in rdr.GetString(7).Split(','))
                        e.employeePermissions.Add((Employee.EmployeePermissions)Enum.Parse(typeof(Employee.EmployeePermissions), p));
                switch (rdr.GetInt32(8))
                {
                    case 0:
                        e.startupScreen = Employee.StartupScreens.Dashboard;
                        break;
                    case 1:
                        e.startupScreen = Employee.StartupScreens.Inbox;
                        break;
                    case 2:
                        e.startupScreen = Employee.StartupScreens.EmployeeManagement;
                        break;
                    case 3:
                        e.startupScreen = Employee.StartupScreens.InventoryManagement;
                        break;
                    case 4:
                        e.startupScreen = Employee.StartupScreens.Invoicing;
                        break;
                    default:
                        e.startupScreen = Employee.StartupScreens.Inbox;
                        break;
                }
            }

            Program.CloseConnection();
            Keys.Find(el => el.ID == e.key.ID).Regenerate();
            return e;
        }
        public Employee RetrieveEmployee(string employeeNumber)
        {
            Console.WriteLine("Information retrieved for employee: " + employeeNumber);
            Employee e = new Employee();
            Program.OpenConnection();

            SqliteCommand command = Program.sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT * FROM EMPLOYEES WHERE EMPLOYEENUMBER = $NUMBER";
            SqliteParameter p1 = new SqliteParameter("$NUMBER", employeeNumber);
            command.Parameters.Add(p1);
            SqliteDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                Program.CloseConnection();
                return null;
            }

            while (reader.Read())
            {
                e.employeeNumber = reader.GetInt32(0);
                e.fullName = reader.GetString(1);
                e.username = reader.GetString(2);
                e.position = reader.GetString(3);
                e.birthDate = DateTime.TryParse(reader.GetString(4), out DateTime n) ? n : DateTime.MinValue;
                e.hireDate = DateTime.TryParse(reader.GetString(5), out DateTime o) ? o : DateTime.MinValue;
                e.terminationDate = DateTime.TryParse(reader.GetString(6), out DateTime p) ? p : DateTime.MinValue;
            }

            Program.CloseConnection();
            return e;
        }
        
        #endregion

        #region Items
        
        public List<Item> CheckItemNumber(string itemNumber, bool connectionOpened, AuthKey key)
        {
            List<Item> invItems = new List<Item>();

            #region Authorization Check
            AuthKey localKey = Keys.Find(el => el.ID == key.ID);
            if (!localKey.Match(key))
            {
                Item i = new Item();
                i.key = new AuthKey();
                i.key.Success = false;
                invItems.Add(i);
                return invItems;
            }
            else
            {
                Console.WriteLine("Key Match");
                Item i = new Item();
                i.key = key;
                i.key.Success = true;
                invItems.Add(i);
                localKey.Regenerate();
            }
            #endregion

            string fixedIN = string.Empty;
            foreach (char c in itemNumber)
            {
                if (char.IsLetterOrDigit(c))
                    fixedIN += c;
            }
            fixedIN = fixedIN.ToUpper();

            if (!connectionOpened)
                Program.OpenConnection();

            SqliteCommand sqlite_cmd;
            sqlite_cmd = Program.sqlite_conn.CreateCommand();

            sqlite_cmd.CommandText =
                "SELECT *" + " " +
                "FROM ITEMS" + " " +
                "WHERE ITEMNUMBER LIKE $ITEM";

            SqliteParameter itemParam = new SqliteParameter("$ITEM", fixedIN[0] + "%" + fixedIN[fixedIN.Length - 1]);
            sqlite_cmd.Parameters.Add(itemParam);
            SqliteDataReader reader = sqlite_cmd.ExecuteReader();

            while (reader.Read())
            {
                Item item = new Item();
                item.productLine = reader.GetString(0);
                item.itemNumber = reader.GetString(1);
                string fixedPL = string.Empty;
                foreach (char c in item.itemNumber)
                    if (char.IsLetterOrDigit(c))
                        fixedPL += c;
                fixedPL = fixedPL.ToUpper();
                if (fixedPL != fixedIN)
                    continue;

                item.itemName = reader.GetString(2);
                item.longDescription = reader.GetValue(3).ToString();
                item.supplier = reader.GetString(4);
                item.groupCode = reader.GetInt32(5);
                item.velocityCode = reader.GetInt32(6);
                item.previousYearVelocityCode = reader.GetInt32(7);
                item.itemsPerContainer = reader.GetInt32(8);
                item.standardPackage = reader.GetInt32(9);
                item.dateStocked = DateTime.Parse(reader.GetString(10));
                item.dateLastReceipt = DateTime.Parse(reader.GetString(11));
                item.minimum = reader.GetDecimal(12);
                item.maximum = reader.GetDecimal(13);
                item.onHandQty = reader.GetDecimal(14);
                item.WIPQty = reader.GetDecimal(15);
                item.onOrderQty = reader.GetDecimal(16);
                item.onBackorderQty = reader.GetDecimal(17);
                item.daysOnOrder = reader.GetInt32(18);
                item.daysOnBackorder = reader.GetInt32(19);
                item.listPrice = reader.GetDecimal(20);
                item.redPrice = reader.GetDecimal(21);
                item.yellowPrice = reader.GetDecimal(22);
                item.greenPrice = reader.GetDecimal(23);
                item.pinkPrice = reader.GetDecimal(24);
                item.bluePrice = reader.GetDecimal(25);
                item.replacementCost = reader.GetDecimal(26);
                item.averageCost = reader.GetDecimal(27);
                item.taxed = reader.GetBoolean(28);
                item.ageRestricted = reader.GetBoolean(29);
                item.minimumAge = reader.GetInt32(30);
                item.locationCode = reader.GetInt32(31);
                item.serialized = reader.GetBoolean(32);
                invItems.Add(item);
                //if (fixedPL == fixedIN)
                //    break;
            }

            if (!connectionOpened)
                Program.CloseConnection();
            if (invItems.Count == 0)
                return null;
            else
                return invItems;
        }    
        public List<Item> CheckItemNumberFromSupplier(string itemNumber, string supplier, AuthKey key)
        {
            List<Item> invItems = new List<Item>();

            #region Authorization Check
            AuthKey localKey = Keys.Find(el => el.ID == key.ID);
            if (!localKey.Match(key))
            {
                Item i = new Item();
                i.key = new AuthKey();
                i.key.Success = false;
                invItems.Add(i);
                return invItems;
            }
            else
            {
                Console.WriteLine("Key Match");
                Item i = new Item();
                i.key = key;
                i.key.Success = true;
                invItems.Add(i);
                localKey.Regenerate();
            }
            #endregion

            string fixedIN = string.Empty;
            foreach (char c in itemNumber)
            {
                if (char.IsLetterOrDigit(c))
                    fixedIN += c;
            }
            fixedIN = fixedIN.ToUpper();

            Program.OpenConnection();
            SqliteCommand sqlite_cmd;
            sqlite_cmd = Program.sqlite_conn.CreateCommand();

            sqlite_cmd.CommandText =
                "SELECT *" + " " +
                "FROM ITEMS" + " " +
                "WHERE ITEMNUMBER LIKE $ITEM AND SUPPLIER == $SUPPLIER";

            SqliteParameter itemParam = new SqliteParameter("$ITEM", fixedIN[0] + "%" + fixedIN[fixedIN.Length - 1]);
            SqliteParameter supplierParam = new SqliteParameter("$SUPPLIER", supplier);
            sqlite_cmd.Parameters.Add(itemParam);
            sqlite_cmd.Parameters.Add(supplierParam);
            SqliteDataReader reader = sqlite_cmd.ExecuteReader();

            while (reader.Read())
            {
                Item item = new Item();
                item.productLine = reader.GetString(0);
                item.itemNumber = reader.GetString(1);
                string fixedPL = string.Empty;
                foreach (char c in item.itemNumber)
                    if (char.IsLetterOrDigit(c))
                        fixedPL += c;
                fixedPL = fixedPL.ToUpper();
                if (fixedPL != fixedIN)
                    continue;

                item.itemName = reader.GetString(2);
                item.longDescription = reader.GetValue(3).ToString();
                item.supplier = reader.GetString(4);
                item.groupCode = reader.GetInt32(5);
                item.velocityCode = reader.GetInt32(6);
                item.previousYearVelocityCode = reader.GetInt32(7);
                item.itemsPerContainer = reader.GetInt32(8);
                item.standardPackage = reader.GetInt32(9);
                item.dateStocked = DateTime.Parse(reader.GetString(10));
                item.dateLastReceipt = DateTime.Parse(reader.GetString(11));
                item.minimum = reader.GetDecimal(12);
                item.maximum = reader.GetDecimal(13);
                item.onHandQty = reader.GetDecimal(14);
                item.WIPQty = reader.GetDecimal(15);
                item.onOrderQty = reader.GetDecimal(16);
                item.onBackorderQty = reader.GetDecimal(17);
                item.daysOnOrder = reader.GetInt32(18);
                item.daysOnBackorder = reader.GetInt32(19);
                item.listPrice = reader.GetDecimal(20);
                item.redPrice = reader.GetDecimal(21);
                item.yellowPrice = reader.GetDecimal(22);
                item.greenPrice = reader.GetDecimal(23);
                item.pinkPrice = reader.GetDecimal(24);
                item.bluePrice = reader.GetDecimal(25);
                item.replacementCost = reader.GetDecimal(26);
                item.averageCost = reader.GetDecimal(27);
                item.taxed = reader.GetBoolean(28);
                item.ageRestricted = reader.GetBoolean(29);
                item.minimumAge = reader.GetInt32(30);
                item.locationCode = reader.GetInt32(31);
                item.serialized = reader.GetBoolean(32);
                invItems.Add(item);
                //if (fixedPL == fixedIN)
                //    break;
            }

            Program.CloseConnection();
            if (invItems.Count == 0)
                return null;
            else
                return invItems;
        }
        public List<Item> RetrieveItemsFromSupplier(string supplier, AuthKey key)
        {
            List<Item> invItems = new List<Item>();

            #region Authorization Check
            AuthKey localKey = Keys.Find(el => el.ID == key.ID);
            if (!localKey.Match(key))
            {
                Item i = new Item();
                i.key = new AuthKey();
                i.key.Success = false;
                invItems.Add(i);
                return invItems;
            }
            else
            {
                Console.WriteLine("Key Match");
                Item i = new Item();
                i.key = key;
                i.key.Success = true;
                invItems.Add(i);
                localKey.Regenerate();
            }
            #endregion

            Program.OpenConnection();
            SqliteCommand sqlite_cmd;
            sqlite_cmd = Program.sqlite_conn.CreateCommand();

            sqlite_cmd.CommandText =
              @"SELECT * 
                FROM ITEMS 
                WHERE SUPPLIER == $SUPPLIER";

            SqliteParameter supplierParam = new SqliteParameter("$SUPPLIER", supplier);
            sqlite_cmd.Parameters.Add(supplierParam);
            SqliteDataReader reader = sqlite_cmd.ExecuteReader();

            while (reader.Read())
            {
                Item item = new Item();
                item.productLine = reader.GetString(0);
                item.itemNumber = reader.GetString(1);
                item.itemName = reader.GetString(2);
                item.longDescription = reader.GetValue(3).ToString();
                item.supplier = reader.GetString(4);
                item.groupCode = reader.GetInt32(5);
                item.velocityCode = reader.GetInt32(6);
                item.previousYearVelocityCode = reader.GetInt32(7);
                item.itemsPerContainer = reader.GetInt32(8);
                item.standardPackage = reader.GetInt32(9);
                item.dateStocked = DateTime.Parse(reader.GetString(10));
                item.dateLastReceipt = DateTime.Parse(reader.GetString(11));
                item.minimum = reader.GetDecimal(12);
                item.maximum = reader.GetDecimal(13);
                item.onHandQty = reader.GetDecimal(14);
                item.WIPQty = reader.GetDecimal(15);
                item.onOrderQty = reader.GetDecimal(16);
                item.onBackorderQty = reader.GetDecimal(17);
                item.daysOnOrder = reader.GetInt32(18);
                item.daysOnBackorder = reader.GetInt32(19);
                item.listPrice = reader.GetDecimal(20);
                item.redPrice = reader.GetDecimal(21);
                item.yellowPrice = reader.GetDecimal(22);
                item.greenPrice = reader.GetDecimal(23);
                item.pinkPrice = reader.GetDecimal(24);
                item.bluePrice = reader.GetDecimal(25);
                item.replacementCost = reader.GetDecimal(26);
                item.averageCost = reader.GetDecimal(27);
                item.taxed = reader.GetBoolean(28);
                item.ageRestricted = reader.GetBoolean(29);
                item.minimumAge = reader.GetInt32(30);
                item.locationCode = reader.GetInt32(31);
                item.serialized = reader.GetBoolean(32);
                invItems.Add(item);
            }

            Program.CloseConnection();
            if (invItems.Count == 0)
                return null;
            else
                return invItems;
        }
        public List<Item> RetrieveItemsFromSupplierBelowMin(string supplier)
        {
            OpenConnection();
            SqliteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            sqlite_cmd.CommandText =
                "SELECT *" + " " +
                "FROM ITEMS" + " " +
                "WHERE SUPPLIER == $SUPPLIER AND ONHANDQUANTITY < MINIMUM";

            SqliteParameter supplierParam = new SqliteParameter("$SUPPLIER", supplier);
            sqlite_cmd.Parameters.Add(supplierParam);
            SqliteDataReader reader = sqlite_cmd.ExecuteReader();

            List<Item> invItems = new List<Item>();

            while (reader.Read())
            {
                Item item = new Item();
                item.productLine = reader.GetString(0);
                item.itemNumber = reader.GetString(1);
                item.itemName = reader.GetString(2);
                item.longDescription = reader.GetValue(3).ToString();
                item.supplier = reader.GetString(4);
                item.groupCode = reader.GetInt32(5);
                item.velocityCode = reader.GetInt32(6);
                item.previousYearVelocityCode = reader.GetInt32(7);
                item.itemsPerContainer = reader.GetInt32(8);
                item.standardPackage = reader.GetInt32(9);
                item.dateStocked = DateTime.Parse(reader.GetString(10));
                item.dateLastReceipt = DateTime.Parse(reader.GetString(11));
                item.minimum = reader.GetDecimal(12);
                item.maximum = reader.GetDecimal(13);
                item.onHandQty = reader.GetDecimal(14);
                item.WIPQty = reader.GetDecimal(15);
                item.onOrderQty = reader.GetDecimal(16);
                item.onBackorderQty = reader.GetDecimal(17);
                item.daysOnOrder = reader.GetInt32(18);
                item.daysOnBackorder = reader.GetInt32(19);
                item.listPrice = reader.GetDecimal(20);
                item.redPrice = reader.GetDecimal(21);
                item.yellowPrice = reader.GetDecimal(22);
                item.greenPrice = reader.GetDecimal(23);
                item.pinkPrice = reader.GetDecimal(24);
                item.bluePrice = reader.GetDecimal(25);
                item.replacementCost = reader.GetDecimal(26);
                item.averageCost = reader.GetDecimal(27);
                item.taxed = reader.GetBoolean(28);
                item.ageRestricted = reader.GetBoolean(29);
                item.minimumAge = reader.GetInt32(30);
                item.locationCode = reader.GetInt32(31);
                item.serialized = reader.GetBoolean(32);
                invItems.Add(item);
            }

            CloseConnection();
            if (invItems.Count == 0)
                return null;
            else
                return invItems;
        }  
        public List<Item> RetrieveItemsFromSupplierBelowMax(string supplier)
        {
            OpenConnection();
            SqliteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            sqlite_cmd.CommandText =
                "SELECT *" + " " +
                "FROM ITEMS" + " " +
                "WHERE SUPPLIER == $SUPPLIER AND ONHANDQUANTITY < MAXIMUM";

            SqliteParameter supplierParam = new SqliteParameter("$SUPPLIER", supplier);
            sqlite_cmd.Parameters.Add(supplierParam);
            SqliteDataReader reader = sqlite_cmd.ExecuteReader();

            List<Item> invItems = new List<Item>();

            while (reader.Read())
            {
                Item item = new Item();
                item.productLine = reader.GetString(0);
                item.itemNumber = reader.GetString(1);
                item.itemName = reader.GetString(2);
                item.longDescription = reader.GetValue(3).ToString();
                item.supplier = reader.GetString(4);
                item.groupCode = reader.GetInt32(5);
                item.velocityCode = reader.GetInt32(6);
                item.previousYearVelocityCode = reader.GetInt32(7);
                item.itemsPerContainer = reader.GetInt32(8);
                item.standardPackage = reader.GetInt32(9);
                item.dateStocked = DateTime.Parse(reader.GetString(10));
                item.dateLastReceipt = DateTime.Parse(reader.GetString(11));
                item.minimum = reader.GetDecimal(12);
                item.maximum = reader.GetDecimal(13);
                item.onHandQty = reader.GetDecimal(14);
                item.WIPQty = reader.GetDecimal(15);
                item.onOrderQty = reader.GetDecimal(16);
                item.onBackorderQty = reader.GetDecimal(17);
                item.daysOnOrder = reader.GetInt32(18);
                item.daysOnBackorder = reader.GetInt32(19);
                item.listPrice = reader.GetDecimal(20);
                item.redPrice = reader.GetDecimal(21);
                item.yellowPrice = reader.GetDecimal(22);
                item.greenPrice = reader.GetDecimal(23);
                item.pinkPrice = reader.GetDecimal(24);
                item.bluePrice = reader.GetDecimal(25);
                item.replacementCost = reader.GetDecimal(26);
                item.averageCost = reader.GetDecimal(27);
                item.taxed = reader.GetBoolean(28);
                item.ageRestricted = reader.GetBoolean(29);
                item.minimumAge = reader.GetInt32(30);
                item.locationCode = reader.GetInt32(31);
                item.serialized = reader.GetBoolean(32);
                invItems.Add(item);
            }

            CloseConnection();
            if (invItems.Count == 0)
                return null;
            else
                return invItems;
        }
        public List<InvoiceItem> RetrieveItemsFromSupplierSoldAfterLastOrderDate(string supplier)
        {
            List<InvoiceItem> items = new List<InvoiceItem>();
            List<Item> supplierItems = new List<Item>();
            DateTime lastOrderDate = DateTime.MinValue;

            OpenConnection();

            //Retrieve last order date for supplier
            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT LASTORDERDATE FROM SUPPLIERS WHERE SUPPLIER = $SUPPLIER";
            SqliteParameter p1 = new SqliteParameter("$SUPPLIER", supplier);
            command.Parameters.Add(p1);
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
                lastOrderDate = DateTime.MinValue;
            else
                while (reader.Read())
                    lastOrderDate = DateTime.Parse(reader.GetString(0));

            CloseConnection();

            supplierItems = RetrieveItemsFromSupplier(supplier, BypassKey);
            List<Invoice> invoices = RetrieveInvoicesByDateRange(lastOrderDate, DateTime.Now);

            foreach (Invoice inv in invoices)
                foreach (InvoiceItem item in inv.items)
                {
                    if (supplierItems.Find(el => el.productLine == item.productLine && el.itemNumber == item.itemNumber) != null)
                        items.Add(item);
                }

            return items;
        }
        public List<string> RetrieveSuppliers()
        {
            List<string> suppliers = new List<string>();
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT SUPPLIER FROM SUPPLIERS";
            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
                suppliers.Add(reader.GetString(0));

            suppliers.Sort();

            CloseConnection();
            return suppliers;
        }
        public void AddSupplier(string supplier)
        {
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "INSERT INTO SUPPLIERS (SUPPLIER) VALUES ($SUPPLIER)";
            SqliteParameter p1 = new SqliteParameter("$SUPPLIER", supplier);
            command.Parameters.Add(p1);
            command.ExecuteNonQuery();

            CloseConnection();
        }
        public bool CheckProductLine(string productLine)
        {
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT * FROM PRODUCTLINES WHERE PRODUCTLINE = $LINE";
            SqliteParameter p1 = new SqliteParameter("$LINE", productLine);
            command.Parameters.Add(p1);
            SqliteDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                CloseConnection();
                return false;
            }
            else
            {
                CloseConnection();
                return true;
            }
        }
        public void AddProductLine(string productLine)
        {
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "INSERT INTO PRODUCTLINES (PRODUCTLINE) VALUES ($LINE)";
            SqliteParameter p1 = new SqliteParameter("$LINE", productLine);
            command.Parameters.Add(p1);
            command.ExecuteNonQuery();

            CloseConnection();
        }
        public Item RetrieveItemFromBarcode(string scannedBarcode)
        {
            OpenConnection();
            Item item = null;
            string barcodeType = "UPCA";
            string barcodeData = String.Empty;

            if (scannedBarcode.Substring(0, 1) == "@")
            {
                if (scannedBarcode.Substring(1, 1).ToLower() == "c")
                {
                    barcodeType = "UPCA";
                    barcodeData = scannedBarcode.Substring(2);
                }
            }
            else
            {
                barcodeData = scannedBarcode;
            }

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT SCANNEDITEMNUMBER,SCANNEDPRODUCTLINE,SCANNEDQUANTITY FROM BARCODES " +
                "WHERE (BARCODETYPE = $TYPE AND BARCODEVALUE = $VALUE)";
            SqliteParameter p1 = new SqliteParameter("$TYPE", barcodeType);
            SqliteParameter p2 = new SqliteParameter("$VALUE", barcodeData);
            command.Parameters.Add(p1);
            command.Parameters.Add(p2);

            SqliteDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                CloseConnection();
                return null;
            }

            while (reader.Read())
            {
                string d1 = reader.GetString(0);
                string d2 = reader.GetString(1);
                decimal d3 = reader.GetDecimal(2);
                List<Item> itemMatches = CheckItemNumber(d1, true, BypassKey);
                item = itemMatches.Find(el => el.productLine.ToLower() == d2.ToLower());
            }

            CloseConnection();
            return item;
        }
        public InvoiceItem RetrieveInvoiceItemFromBarcode(string scannedBarcode)
        {
            OpenConnection();
            InvoiceItem item = null;
            string barcodeType = "UPCA";
            string barcodeData = String.Empty;

            if (scannedBarcode.Substring(0, 1) == "@")
            {
                if (scannedBarcode.Substring(1, 1).ToLower() == "c")
                {
                    barcodeType = "UPCA";
                    barcodeData = scannedBarcode.Substring(2);
                }
            }
            else
            {
                barcodeData = scannedBarcode;
            }

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT SCANNEDITEMNUMBER,SCANNEDPRODUCTLINE,SCANNEDQUANTITY FROM BARCODES " +
                "WHERE (BARCODETYPE = $TYPE AND BARCODEVALUE = $VALUE)";
            SqliteParameter p1 = new SqliteParameter("$TYPE", barcodeType);
            SqliteParameter p2 = new SqliteParameter("$VALUE", barcodeData);
            command.Parameters.Add(p1);
            command.Parameters.Add(p2);

            SqliteDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                CloseConnection();
                return null;
            }

            while (reader.Read())
            {
                string d1 = reader.GetString(0);
                string d2 = reader.GetString(1);
                decimal d3 = reader.GetDecimal(2);
                List<Item> itemMatches = CheckItemNumber(d1, true, BypassKey);
                item = new InvoiceItem(itemMatches.Find(el => el.productLine.ToLower() == d2.ToLower()));
            }

            CloseConnection();
            return item;
        }
        public Item RetrieveItem(string itemNumber, string productLine, bool connectionOpened = false)
        {
            string fixedIN = string.Empty;
            foreach (char c in itemNumber)
            {
                if (char.IsLetterOrDigit(c))
                    fixedIN += c;
            }
            fixedIN = fixedIN.ToUpper();

            Item item = new Item();
            if (!connectionOpened)
                OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT * FROM ITEMS WHERE (ITEMNUMBER LIKE $ITEMNO AND PRODUCTLINE = $LINE)";
            SqliteParameter p1 = new SqliteParameter("$ITEMNO", fixedIN[0] + "%" + fixedIN[fixedIN.Length - 1]);
            SqliteParameter p2 = new SqliteParameter("$LINE", productLine);
            command.Parameters.Add(p1);
            command.Parameters.Add(p2);
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                if (!connectionOpened)
                    CloseConnection();
                return null;
            }

            while (reader.Read())
            {
                item.productLine = reader.GetString(0);
                item.itemNumber = reader.GetString(1);
                string fixedPL = string.Empty;
                foreach (char c in item.itemNumber)
                    if (char.IsLetterOrDigit(c))
                        fixedPL += c;
                fixedPL = fixedPL.ToUpper();
                if (fixedPL != fixedIN)
                    continue;

                item.itemName = reader.GetValue(2).ToString();
                item.longDescription = reader.GetValue(3).ToString();
                item.supplier = reader.GetValue(4).ToString();
                item.groupCode = reader.GetInt32(5);
                item.velocityCode = reader.GetInt32(6);
                item.previousYearVelocityCode = reader.GetInt32(7);
                item.itemsPerContainer = reader.GetInt32(8);
                item.standardPackage = reader.GetInt32(9);
                item.dateStocked = DateTime.Parse(reader.GetString(10));
                item.dateLastReceipt = DateTime.Parse(reader.GetString(11));
                item.minimum = reader.GetDecimal(12);
                item.maximum = reader.GetDecimal(13);
                item.onHandQty = reader.GetDecimal(14);
                item.WIPQty = reader.GetDecimal(15);
                item.onOrderQty = reader.GetDecimal(16);
                item.onBackorderQty = reader.GetDecimal(17);
                item.daysOnOrder = reader.GetInt32(18);
                item.daysOnBackorder = reader.GetInt32(19);
                item.listPrice = reader.GetDecimal(20);
                item.redPrice = reader.GetDecimal(21);
                item.yellowPrice = reader.GetDecimal(22);
                item.greenPrice = reader.GetDecimal(23);
                item.pinkPrice = reader.GetDecimal(24);
                item.bluePrice = reader.GetDecimal(25);
                item.replacementCost = reader.GetDecimal(26);
                item.averageCost = reader.GetDecimal(27);
                item.taxed = reader.GetBoolean(28);
                item.ageRestricted = reader.GetBoolean(29);
                item.minimumAge = reader.GetInt32(30);
                item.locationCode = reader.GetInt32(31);
                item.serialized = reader.GetBoolean(32);
                item.category = reader.GetValue(33).ToString();
                item.SKU = reader.GetValue(34).ToString();
                if (fixedPL == fixedIN)
                    break;
            }
            reader.Close();
            reader.Dispose();

            if (!connectionOpened)
                CloseConnection();

            string fixedPLE = string.Empty;
            foreach (char c in item.itemNumber)
                if (char.IsLetterOrDigit(c))
                    fixedPLE += c;
            fixedPLE = fixedPLE.ToUpper();
            if (fixedPLE != fixedIN)
                return null;

            return item;
        }
        public void UpdateItem(Item newItem)
        {
            if (RetrieveItem(newItem.itemNumber, newItem.productLine) == null)
            {
                AddItem(newItem);
                return;
            }
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "UPDATE ITEMS SET " +
                "ITEMNAME = $ITEMNAME, LONGDESCRIPTION = $LONGDESCRIPTION, " +
                "SUPPLIER = $SUPPLIER, GROUPCODE = $GROUPCODE, " +
                "VELOCITYCODE = $VELOCITYCODE, PREVIOUSYEARVELOCITYCODE = $PREVIOUSVELOCITYCODE, " +
                "ITEMSPERCONTAINER = $ITEMSPERCONTAINER, STANDARDPACKAGE = $STANDARDPACKAGE, " +
                "DATESTOCKED = $DATESTOCKED, DATELASTRECEIPT = $DATELASTRECEIPT, " +
                "MINIMUM = $MIN, MAXIMUM = $MAX, ONHANDQUANTITY = $ONHANDQTY, WIPQUANTITY = $WIPQTY, " +
                "ONORDERQUANTITY = $ONORDERQTY, BACKORDERQUANTITY = $BACKORDERQTY, " +
                "DAYSONORDER = $DAYSONORDER, DAYSONBACKORDER = $DAYSONBACKORDER, LISTPRICE = $LIST, " +
                "REDPRICE = $RED, YELLOWPRICE = $YELLOW, GREENPRICE = $GREEN, PINKPRICE = $PINK, " +
                "BLUEPRICE = $BLUE, REPLACEMENTCOST = $COST, AVERAGECOST = $AVERAGECOST, " +
                "TAXED = $TAXED, AGERESTRICTED = $RESTRICTED, MINIMUMAGE = $MINAGE, LOCATIONCODE = $LOCATION, " +
                "SERIALIZED = $SERIALIZED, CATEGORY = $CATEGORY, SKU = $SKU " +
                "WHERE (ITEMNUMBER = $ITEMNUMBER AND PRODUCTLINE = $PRODUCTLINE)";

            SqliteParameter p1 = new SqliteParameter("$ITEMNAME", newItem.itemName);
            SqliteParameter p2 = new SqliteParameter("$LONGDESCRIPTION", newItem.longDescription);
            SqliteParameter p3 = new SqliteParameter("$SUPPLIER", newItem.supplier);
            SqliteParameter p4 = new SqliteParameter("$GROUPCODE", newItem.groupCode);
            SqliteParameter p5 = new SqliteParameter("$VELOCITYCODE", newItem.velocityCode);
            SqliteParameter p6 = new SqliteParameter("$PREVIOUSVELOCITYCODE", newItem.previousYearVelocityCode);
            SqliteParameter p7 = new SqliteParameter("$ITEMSPERCONTAINER", newItem.itemsPerContainer);
            SqliteParameter p8 = new SqliteParameter("$STANDARDPACKAGE", newItem.standardPackage);
            SqliteParameter p9 = new SqliteParameter("$DATESTOCKED", newItem.dateStocked.ToString());
            SqliteParameter p10 = new SqliteParameter("$DATELASTRECEIPT", newItem.dateLastReceipt.ToString());
            SqliteParameter p11 = new SqliteParameter("$MIN", newItem.minimum);
            SqliteParameter p12 = new SqliteParameter("$MAX", newItem.maximum);
            SqliteParameter p13 = new SqliteParameter("$ONHANDQTY", newItem.onHandQty);
            SqliteParameter p14 = new SqliteParameter("$WIPQTY", newItem.WIPQty);
            SqliteParameter p15 = new SqliteParameter("$ONORDERQTY", newItem.onOrderQty);
            SqliteParameter p16 = new SqliteParameter("$BACKORDERQTY", newItem.onBackorderQty);
            SqliteParameter p17 = new SqliteParameter("$DAYSONORDER", newItem.daysOnOrder);
            SqliteParameter p18 = new SqliteParameter("$DAYSONBACKORDER", newItem.daysOnBackorder);
            SqliteParameter p19 = new SqliteParameter("$LIST", newItem.listPrice);
            SqliteParameter p20 = new SqliteParameter("$RED", newItem.redPrice);
            SqliteParameter p21 = new SqliteParameter("$YELLOW", newItem.yellowPrice);
            SqliteParameter p22 = new SqliteParameter("$GREEN", newItem.greenPrice);
            SqliteParameter p23 = new SqliteParameter("$PINK", newItem.pinkPrice);
            SqliteParameter p24 = new SqliteParameter("$BLUE", newItem.bluePrice);
            SqliteParameter p25 = new SqliteParameter("$COST", newItem.replacementCost);
            SqliteParameter p26 = new SqliteParameter("$AVERAGECOST", newItem.averageCost);
            SqliteParameter p27 = new SqliteParameter("$TAXED", newItem.taxed);
            SqliteParameter p28 = new SqliteParameter("$RESTRICTED", newItem.ageRestricted);
            SqliteParameter p29 = new SqliteParameter("$MINAGE", newItem.minimumAge);
            SqliteParameter p30 = new SqliteParameter("$LOCATION", newItem.locationCode);
            SqliteParameter p31 = new SqliteParameter("$ITEMNUMBER", newItem.itemNumber);
            SqliteParameter p32 = new SqliteParameter("$PRODUCTLINE", newItem.productLine);
            SqliteParameter p33 = new SqliteParameter("$SERIALIZED", newItem.serialized);
            SqliteParameter p34 = new SqliteParameter("$CATEGORY", newItem.category);
            SqliteParameter p35 = new SqliteParameter("$SKU", newItem.SKU);

            command.Parameters.Add(p1);
            command.Parameters.Add(p2);
            command.Parameters.Add(p3);
            command.Parameters.Add(p4);
            command.Parameters.Add(p5);
            command.Parameters.Add(p6);
            command.Parameters.Add(p7);
            command.Parameters.Add(p8);
            command.Parameters.Add(p9);
            command.Parameters.Add(p10);
            command.Parameters.Add(p11);
            command.Parameters.Add(p12);
            command.Parameters.Add(p13);
            command.Parameters.Add(p14);
            command.Parameters.Add(p15);
            command.Parameters.Add(p16);
            command.Parameters.Add(p17);
            command.Parameters.Add(p18);
            command.Parameters.Add(p19);
            command.Parameters.Add(p20);
            command.Parameters.Add(p21);
            command.Parameters.Add(p22);
            command.Parameters.Add(p23);
            command.Parameters.Add(p24);
            command.Parameters.Add(p25);
            command.Parameters.Add(p26);
            command.Parameters.Add(p27);
            command.Parameters.Add(p28);
            command.Parameters.Add(p29);
            command.Parameters.Add(p30);
            command.Parameters.Add(p31);
            command.Parameters.Add(p32);
            command.Parameters.Add(p33);
            command.Parameters.Add(p34);
            command.Parameters.Add(p35);

            command.ExecuteNonQuery();

            CloseConnection();
            return;
        }
        public List<string> RetrieveItemSerialNumbers(string productLine, string itemNumber)
        {
            List<string> serialNumbers = new List<string>();
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT SERIALNUMBER FROM SERIALNUMBERS WHERE ITEMNUMBER = $ITEM AND PRODUCTLINE = $LINE";
            SqliteParameter p1 = new SqliteParameter("$ITEM", itemNumber);
            SqliteParameter p2 = new SqliteParameter("$LINE", productLine);
            command.Parameters.Add(p1);
            command.Parameters.Add(p2);
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                return null;
            }

            while (reader.Read())
            {
                serialNumbers.Add(reader.GetString(0));
            }

            CloseConnection();
            return serialNumbers;
        }
        public bool AddItem(Item item)
        {
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "INSERT INTO ITEMS (" +
                "PRODUCTLINE, ITEMNUMBER, ITEMNAME, LONGDESCRIPTION, SUPPLIER, GROUPCODE, VELOCITYCODE, PREVIOUSYEARVELOCITYCODE, " +
                "ITEMSPERCONTAINER, STANDARDPACKAGE, DATESTOCKED, DATELASTRECEIPT, MINIMUM, MAXIMUM, ONHANDQUANTITY, WIPQUANTITY, " +
                "ONORDERQUANTITY, BACKORDERQUANTITY, DAYSONORDER, DAYSONBACKORDER, LISTPRICE, REDPRICE, YELLOWPRICE, GREENPRICE, " +
                "PINKPRICE, BLUEPRICE, REPLACEMENTCOST, AVERAGECOST, TAXED, AGERESTRICTED, MINIMUMAGE, LOCATIONCODE, SERIALIZED, " +
                "CATEGORY, SKU) " +
                "VALUES ($PRODUCTLINE, $ITEMNUMBER, $ITEMNAME, $DESCRIPTION, $SUPPLIER, $GROUP, $VELOCITY, $PREVIOUSVELOCITY, " +
                "$ITEMSPERCONTAINER, $STDPKG, $DATESTOCKED, $DATELASTRECEIPT, $MIN, $MAX, $ONHAND, $WIPQUANTITY, " +
                "$ONORDERQTY, $BACKORDERQTY, $DAYSONORDER, $DAYSONBACKORDER, $LIST, $RED, $YELLOW, $GREEN, " +
                "$PINK, $BLUE, $COST, $AVERAGECOST, $TAXED, $AGERESTRICTED, $MINAGE, $LOCATION, $SERIALIZED, $CATEGORY, $SKU)";

            SqliteParameter p1 = new SqliteParameter("$PRODUCTLINE", item.productLine);
            SqliteParameter p2 = new SqliteParameter("$ITEMNUMBER", item.itemNumber);
            SqliteParameter p3 = new SqliteParameter("$ITEMNAME", item.itemName);
            SqliteParameter p4 = new SqliteParameter("$DESCRIPTION", item.longDescription);
            SqliteParameter p5 = new SqliteParameter("$SUPPLIER", item.supplier);
            SqliteParameter p6 = new SqliteParameter("$GROUP", item.groupCode);
            SqliteParameter p7 = new SqliteParameter("$VELOCITY", item.velocityCode);
            SqliteParameter p8 = new SqliteParameter("$PREVIOUSVELOCITY", item.previousYearVelocityCode);
            SqliteParameter p9 = new SqliteParameter("$ITEMSPERCONTAINER", item.itemsPerContainer);
            SqliteParameter p10 = new SqliteParameter("$STDPKG", item.standardPackage);
            SqliteParameter p11 = new SqliteParameter("$DATESTOCKED", item.dateStocked.ToString());
            SqliteParameter p12 = new SqliteParameter("$DATELASTRECEIPT", item.dateLastReceipt.ToString());
            SqliteParameter p13 = new SqliteParameter("$MIN", item.minimum);
            SqliteParameter p14 = new SqliteParameter("$MAX", item.maximum);
            SqliteParameter p15 = new SqliteParameter("$ONHAND", item.onHandQty);
            SqliteParameter p16 = new SqliteParameter("$WIPQUANTITY", item.WIPQty);
            SqliteParameter p17 = new SqliteParameter("$ONORDERQTY", item.onOrderQty);
            SqliteParameter p18 = new SqliteParameter("$BACKORDERQTY", item.onBackorderQty);
            SqliteParameter p19 = new SqliteParameter("$DAYSONORDER", item.daysOnOrder);
            SqliteParameter p20 = new SqliteParameter("$DAYSONBACKORDER", item.daysOnBackorder);
            SqliteParameter p21 = new SqliteParameter("$LIST", item.listPrice);
            SqliteParameter p22 = new SqliteParameter("$RED", item.redPrice);
            SqliteParameter p23 = new SqliteParameter("$YELLOW", item.yellowPrice);
            SqliteParameter p24 = new SqliteParameter("$GREEN", item.greenPrice);
            SqliteParameter p25 = new SqliteParameter("$PINK", item.pinkPrice);
            SqliteParameter p26 = new SqliteParameter("$BLUE", item.bluePrice);
            SqliteParameter p27 = new SqliteParameter("$COST", item.replacementCost);
            SqliteParameter p28 = new SqliteParameter("$AVERAGECOST", item.averageCost);
            SqliteParameter p29 = new SqliteParameter("$TAXED", item.taxed);
            SqliteParameter p30 = new SqliteParameter("$AGERESTRICTED", item.ageRestricted);
            SqliteParameter p31 = new SqliteParameter("$MINAGE", item.minimumAge);
            SqliteParameter p32 = new SqliteParameter("$LOCATION", item.locationCode);
            SqliteParameter p33 = new SqliteParameter("$SERIALIZED", item.serialized);
            SqliteParameter p34 = new SqliteParameter("$CATEGORY", item.category);
            SqliteParameter p35 = new SqliteParameter("$SKU", item.SKU);

            command.Parameters.Add(p1);
            command.Parameters.Add(p2);
            command.Parameters.Add(p3);
            command.Parameters.Add(p4);
            command.Parameters.Add(p5);
            command.Parameters.Add(p6);
            command.Parameters.Add(p7);
            command.Parameters.Add(p8);
            command.Parameters.Add(p9);
            command.Parameters.Add(p10);
            command.Parameters.Add(p11);
            command.Parameters.Add(p12);
            command.Parameters.Add(p13);
            command.Parameters.Add(p14);
            command.Parameters.Add(p15);
            command.Parameters.Add(p16);
            command.Parameters.Add(p17);
            command.Parameters.Add(p18);
            command.Parameters.Add(p19);
            command.Parameters.Add(p20);
            command.Parameters.Add(p21);
            command.Parameters.Add(p22);
            command.Parameters.Add(p23);
            command.Parameters.Add(p24);
            command.Parameters.Add(p25);
            command.Parameters.Add(p26);
            command.Parameters.Add(p27);
            command.Parameters.Add(p28);
            command.Parameters.Add(p29);
            command.Parameters.Add(p30);
            command.Parameters.Add(p31);
            command.Parameters.Add(p32);
            command.Parameters.Add(p33);
            command.Parameters.Add(p34);
            command.Parameters.Add(p35);

            command.ExecuteNonQuery();

            CloseConnection();
            return true;
        }
        public List<Item> RetrieveLabelOutOfDateItems()
        {
            List<Item> items = new List<Item>();
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT PRODUCTLINE, ITEMNUMBER FROM ITEMS WHERE LASTLABELPRICE != GREENPRICE";
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                return null;
            }

            while (reader.Read())
            {
                string pl = reader.GetString(0);
                string itemno = reader.GetString(1);
                items.Add(RetrieveItem(itemno, pl, true));
            }

            CloseConnection();
            return items;
        }
        

        #endregion

        #region Customers

        public Customer CheckCustomerNumber(string custNo, AuthKey key)
        {
            Customer cust = new Customer();
            #region Authorization Check
            AuthKey localKey = Keys.Find(el => el.ID == key.ID);
            if (!localKey.Match(key))
            {
                cust.key = new AuthKey();
                cust.key.Success = false;
                return cust;
            }
            else
            {
                Console.WriteLine("Key Match");
                cust.key = key;
                cust.key.Success = true;
                localKey.Regenerate();
            }
            #endregion

            
            OpenConnection();

            SqliteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            sqlite_cmd.CommandText =
                "SELECT *" + " " +
                "FROM CUSTOMERS" + " " +
                "WHERE CUSTOMERNUMBER = $CUSTNO";

            SqliteParameter itemParam = new SqliteParameter("$CUSTNO", custNo);
            sqlite_cmd.Parameters.Add(itemParam);
            SqliteDataReader reader = sqlite_cmd.ExecuteReader();

            if (!reader.HasRows)
            {
                CloseConnection();
                return null;
            }

            while (reader.Read())
            {
                cust.availablePaymentTypes = new List<Payment.PaymentTypes>();
                string[] paymentTypes = reader.GetString(5).Split(',');
                foreach (string p in paymentTypes)
                {
                    switch (p)
                    {
                        case "Cash":
                            cust.availablePaymentTypes.Add(Payment.PaymentTypes.Cash);
                            break;
                        case "Check":
                            cust.availablePaymentTypes.Add(Payment.PaymentTypes.Check);
                            break;
                        case "PaymentCard":
                            cust.availablePaymentTypes.Add(Payment.PaymentTypes.PaymentCard);
                            break;
                        case "CashApp":
                            cust.availablePaymentTypes.Add(Payment.PaymentTypes.CashApp);
                            break;
                        case "Venmo":
                            cust.availablePaymentTypes.Add(Payment.PaymentTypes.Venmo);
                            break;
                        case "Paypal":
                            cust.availablePaymentTypes.Add(Payment.PaymentTypes.Paypal);
                            break;
                        case "Charge":
                            cust.availablePaymentTypes.Add(Payment.PaymentTypes.Charge);
                            break;
                    }
                }

                cust.customerNumber = reader.GetInt32(1).ToString();
                cust.customerName = reader.GetString(0);
                cust.mailingAddress = reader.GetString(11);
                cust.shippingAddress = reader.GetString(12);
            }

            CloseConnection();
            return cust;
        }
        
        #endregion

        #region Invoices

        public List<Invoice> RetrieveInvoicesByDateRange(DateTime startDate, DateTime endDate, bool connectionOpened = false)
        {
            List<Invoice> invoices = new List<Invoice>();
            if (!connectionOpened)
                OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT INVOICENUMBER FROM INVOICES WHERE INVOICEFINALIZEDTIME > $STARTTIME AND INVOICEFINALIZEDTIME < $ENDTIME";
            SqliteParameter p1 = new SqliteParameter("$STARTTIME", startDate.ToString());
            SqliteParameter p2 = new SqliteParameter("$ENDTIME", endDate.AddDays(1).ToString());
            command.Parameters.Add(p1);
            command.Parameters.Add(p2);
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                if (!connectionOpened)
                    CloseConnection();
                return null;
            }

            List<int> invNumbers = new List<int>();

            while (reader.Read())
            {
                invNumbers.Add(reader.GetInt32(0));
            }

            CloseConnection();

            foreach (int number in invNumbers)
            {
                invoices.Add(RetrieveInvoice(number));
            }

            if (!connectionOpened)
                CloseConnection();
            return invoices;
        }
        public Invoice RetrieveInvoice(int invNumber)
        {
            Invoice inv = new Invoice();
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();

            command.CommandText =
                "SELECT * FROM INVOICES WHERE INVOICENUMBER = $INVOICENUMBER";

            SqliteParameter p1 = new SqliteParameter("$INVOICENUMBER", invNumber);
            command.Parameters.Add(p1);
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                return null;
            }

            while (reader.Read())
            {
                inv.invoiceNumber = reader.GetInt32(0);
                inv.subtotal = reader.GetDecimal(1);
                inv.taxableTotal = reader.GetDecimal(2);
                inv.taxRate = reader.GetDecimal(3);
                inv.taxAmount = reader.GetDecimal(4);
                inv.total = reader.GetDecimal(5);
                inv.totalPayments = reader.GetDecimal(6);
                inv.containsAgeRestrictedItem = reader.GetBoolean(7);
                inv.customerBirthdate = DateTime.Parse(reader.GetString(8));
                inv.attentionLine = reader.GetString(9);
                inv.PONumber = reader.GetString(10);
                inv.invoiceMessage = reader.GetString(11);
                inv.savedInvoice = reader.GetBoolean(12);
                inv.savedInvoiceTime = DateTime.Parse(reader.GetString(13));
                inv.invoiceCreationTime = DateTime.Parse(reader.GetString(14));
                inv.invoiceFinalizedTime = DateTime.Parse(reader.GetString(15));
                inv.finalized = reader.GetBoolean(16);
                inv.voided = reader.GetBoolean(17);
                inv.customer = new Customer() { customerNumber = reader.GetInt32(18).ToString() };
                inv.employee = new Employee() { employeeNumber = reader.GetInt32(19) };
                //TODO: FINISH ADDING GENERAL INVOICE DATA FROM DATABASE
            }

            reader.Close();
            command.CommandText =
                "SELECT * FROM PAYMENTS WHERE INVOICENUMBER = $INVOICENUMBER";
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                inv.payments.Add(new Payment()
                {
                    paymentAmount = reader.GetDecimal(3),
                    paymentType = (Payment.PaymentTypes)Enum.Parse(typeof(Payment.PaymentTypes), reader.GetString(2))
                });
            }

            reader.Close();
            command.CommandText =
                "SELECT * FROM INVOICEITEMS WHERE INVOICENUMBER = $INVOICENUMBER";
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                InvoiceItem item = new InvoiceItem()
                {
                    itemNumber = reader.GetString(1),
                    productLine = reader.GetString(2),
                    itemName = reader.GetString(3),
                    price = reader.GetDecimal(4),
                    listPrice = reader.GetDecimal(5),
                    quantity = reader.GetDecimal(6),
                    total = reader.GetDecimal(7),
                    pricingCode = reader.GetString(8),
                    serializedItem = reader.GetBoolean(9),
                    //serialNumber = reader.GetString(10),
                    ageRestricted = reader.GetBoolean(11),
                    minimumAge = reader.GetInt32(12),
                    taxed = reader.GetBoolean(13),
                    codes = reader.GetString(14).Split(','),
                    ID = reader.GetGuid(15),
                    cost = reader.GetDecimal(16)
                };

                if (item.serializedItem)
                    item.serialNumber = reader.GetString(10);

                inv.items.Add(item);
            }

            CloseConnection();

            inv.customer = CheckCustomerNumber(inv.customer.customerNumber, BypassKey);
            inv.employee = RetrieveEmployee(inv.employee.employeeNumber.ToString());
            return inv;
        }        
        public List<Invoice> RetrieveInvoicesByCriteria(string[] criteria)
        {
            List<Invoice> invoices = new List<Invoice>();
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText = "SELECT INVOICENUMBER FROM INVOICES";
            SqliteDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                CloseConnection();
                return null;
            }

            List<int> invNumbers = new List<int>();

            while (reader.Read())
            {
                invNumbers.Add(reader.GetInt32(0));
            }

            CloseConnection();

            foreach (int number in invNumbers)
            {
                invoices.Add(RetrieveInvoice(number));
            }

            return invoices;
        }
        public void SaveReleasedInvoice(Invoice inv)
        {
            OpenConnection();
            SqliteCommand command = sqlite_conn.CreateCommand();

            #region Add General Invoice Information to INVOICES tables
            command.CommandText =
                "INSERT INTO INVOICES (" +

                "INVOICENUMBER,SUBTOTAL,TAXABLETOTAL,TAXRATE,TAXAMOUNT,TOTAL," +
                "TOTALPAYMENTS,AGERESTRICTED,CUSTOMERBIRTHDATE,ATTENTION,PO,MESSAGE," +
                "SAVEDINVOICE,SAVEDINVOICETIME,INVOICECREATIONTIME,INVOICEFINALIZEDTIME,FINALIZED,VOIDED,CUSTOMERNUMBER,EMPLOYEENUMBER, " +
                "COST, PROFIT) " +

                "VALUES ($INVOICENUMBER,$SUBTOTAL,$TAXABLETOTAL,$TAXRATE,$TAXAMOUNT,$TOTAL," +
                "$TOTALPAYMENTS,$AGERESTRICTED,$CUSTOMERBIRTHDATE,$ATTENTION,$PO,$MESSAGE," +
                "$SAVEDINVOICE,$SAVEDINVOICETIME,$INVOICECREATIONTIME,$INVOICEFINALIZEDTIME,$FINALIZED,$VOIDED,$CUSTOMERNUMBER,$EMPLOYEENUMBER, " +
                "$COST, $PROFIT)";

            SqliteParameter p1 = new SqliteParameter("$INVOICENUMBER", inv.invoiceNumber);
            SqliteParameter p2 = new SqliteParameter("$SUBTOTAL", inv.subtotal);
            SqliteParameter p3 = new SqliteParameter("$TAXABLETOTAL", inv.taxableTotal);
            SqliteParameter p4 = new SqliteParameter("$TAXRATE", inv.taxRate);
            SqliteParameter p5 = new SqliteParameter("$TAXAMOUNT", inv.taxAmount);
            SqliteParameter p6 = new SqliteParameter("$TOTAL", inv.total);
            SqliteParameter p7 = new SqliteParameter("$TOTALPAYMENTS", inv.totalPayments);
            SqliteParameter p8 = new SqliteParameter("$AGERESTRICTED", inv.containsAgeRestrictedItem);
            SqliteParameter p9 = new SqliteParameter("$CUSTOMERBIRTHDATE", inv.customerBirthdate.ToString());
            SqliteParameter p10 = new SqliteParameter("$ATTENTION", inv.attentionLine);
            SqliteParameter p11 = new SqliteParameter("$PO", inv.PONumber);
            SqliteParameter p12 = new SqliteParameter("$MESSAGE", inv.invoiceMessage);
            SqliteParameter p13 = new SqliteParameter("$SAVEDINVOICE", inv.savedInvoice);
            SqliteParameter p14 = new SqliteParameter("$SAVEDINVOICETIME", inv.savedInvoiceTime.ToString());
            SqliteParameter p15 = new SqliteParameter("$INVOICECREATIONTIME", inv.invoiceCreationTime.ToString());
            SqliteParameter p16 = new SqliteParameter("$INVOICEFINALIZEDTIME", inv.invoiceFinalizedTime.ToString());
            SqliteParameter p17 = new SqliteParameter("$FINALIZED", inv.finalized);
            SqliteParameter p18 = new SqliteParameter("$VOIDED", inv.voided);
            SqliteParameter p19 = new SqliteParameter("$CUSTOMERNUMBER", inv.customer.customerNumber);
            SqliteParameter p20 = new SqliteParameter("$EMPLOYEENUMBER", inv.employee.employeeNumber);
            SqliteParameter p21 = new SqliteParameter("$COST", inv.cost);
            SqliteParameter p22 = new SqliteParameter("$PROFIT", inv.profit);


            command.Parameters.Add(p1);
            command.Parameters.Add(p2);
            command.Parameters.Add(p3);
            command.Parameters.Add(p4);
            command.Parameters.Add(p5);
            command.Parameters.Add(p6);
            command.Parameters.Add(p7);
            command.Parameters.Add(p8);
            command.Parameters.Add(p9);
            command.Parameters.Add(p10);
            command.Parameters.Add(p11);
            command.Parameters.Add(p12);
            command.Parameters.Add(p13);
            command.Parameters.Add(p14);
            command.Parameters.Add(p15);
            command.Parameters.Add(p16);
            command.Parameters.Add(p17);
            command.Parameters.Add(p18);
            command.Parameters.Add(p19);
            command.Parameters.Add(p20);
            command.Parameters.Add(p21);
            command.Parameters.Add(p22);

            command.ExecuteNonQuery();
            #endregion

            #region Add Invoice Item information to INVOICEITEMS table
            foreach (InvoiceItem item in inv.items)
            {
                command.CommandText =
                    "INSERT INTO INVOICEITEMS (" +

                    "INVOICENUMBER,ITEMNUMBER,PRODUCTLINE,ITEMDESCRIPTION,PRICE,LISTPRICE," +
                    "QUANTITY,TOTAL,PRICECODE,SERIALIZED,SERIALNUMBER,AGERESTRICTED," +
                    "MINIMUMAGE,TAXED,INVOICECODES,GUID,COST) " +

                    "VALUES ($INVOICENUMBER,$ITEMNUMBER,$PRODUCTLINE,$ITEMDESCRIPTION,$PRICE,$LISTPRICE," +
                    "$QUANTITY,$TOTAL,$PRICECODE,$SERIALIZED,$SERIALNUMBER,$AGERESTRICTED," +
                    "$MINIMUMAGE,$TAXED,$INVOICECODES,$GUID,$COST)";

                SqliteParameter pp1 = new SqliteParameter("$INVOICENUMBER", inv.invoiceNumber);
                SqliteParameter pp2 = new SqliteParameter("$ITEMNUMBER", item.itemNumber);
                SqliteParameter pp3 = new SqliteParameter("$PRODUCTLINE", item.productLine);
                SqliteParameter pp4 = new SqliteParameter("$ITEMDESCRIPTION", item.itemName);
                SqliteParameter pp5 = new SqliteParameter("$PRICE", item.price);
                SqliteParameter pp6 = new SqliteParameter("$LISTPRICE", item.listPrice);
                SqliteParameter pp7 = new SqliteParameter("$QUANTITY", item.quantity);
                SqliteParameter pp8 = new SqliteParameter("$TOTAL", item.total);
                SqliteParameter pp9 = new SqliteParameter("$PRICECODE", item.pricingCode);
                SqliteParameter pp10 = new SqliteParameter("$SERIALIZED", item.serializedItem);
                SqliteParameter pp11 = new SqliteParameter("$SERIALNUMBER", item.serialNumber);
                SqliteParameter pp12 = new SqliteParameter("$AGERESTRICTED", item.ageRestricted);
                SqliteParameter pp13 = new SqliteParameter("$MINIMUMAGE", item.minimumAge);
                SqliteParameter pp14 = new SqliteParameter("$TAXED", item.taxed);
                string invCodes = string.Empty;
                if (item.codes != null)
                    foreach (string code in item.codes)
                        invCodes += code + ",";
                invCodes = invCodes.Trim(',');
                SqliteParameter pp15 = new SqliteParameter("$INVOICECODES", invCodes);
                SqliteParameter pp16 = new SqliteParameter("$GUID", item.ID);
                SqliteParameter pp17 = new SqliteParameter("$GUID", item.ID);

                command.Parameters.Add(pp1);
                command.Parameters.Add(pp2);
                command.Parameters.Add(pp3);
                command.Parameters.Add(pp4);
                command.Parameters.Add(pp5);
                command.Parameters.Add(pp6);
                command.Parameters.Add(pp7);
                command.Parameters.Add(pp8);
                command.Parameters.Add(pp9);
                command.Parameters.Add(pp10);
                command.Parameters.Add(pp11);
                command.Parameters.Add(pp12);
                command.Parameters.Add(pp13);
                command.Parameters.Add(pp14);
                command.Parameters.Add(pp15);
                command.Parameters.Add(pp16);
                command.Parameters.Add(pp17);

                command.ExecuteNonQuery();
            }
            #endregion

            #region Add Invoice Payment Information to INVOICEPAYMENTS table
            foreach (Payment pay in inv.payments)
            {
                command.CommandText =
                    "INSERT INTO PAYMENTS (" +

                    "INVOICENUMBER,ID,PAYMENTTYPE,PAYMENTAMOUNT) " +

                    "VALUES ($INVOICENUMBER,$ID,$PAYMENTTYPE,$PAYMENTAMOUNT)";

                SqliteParameter ppp1 = new SqliteParameter("$INVOICENUMBER", inv.invoiceNumber);
                SqliteParameter ppp2 = new SqliteParameter("$ID", pay.ID);
                SqliteParameter ppp3 = new SqliteParameter("$PAYMENTTYPE", pay.paymentType.ToString());
                SqliteParameter ppp4 = new SqliteParameter("$PAYMENTAMOUNT", pay.paymentAmount);

                command.Parameters.Add(ppp1);
                command.Parameters.Add(ppp2);
                command.Parameters.Add(ppp3);
                command.Parameters.Add(ppp4);

                command.ExecuteNonQuery();
            }
            #endregion

            CloseConnection();
            return;
        }
        public int RetrieveNextInvoiceNumber()
        {
            int invNo = 100000;
            OpenConnection();
            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText = "SELECT MAX(INVOICENUMBER) FROM INVOICES";
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                return invNo;
            }

            while (reader.Read())
            {
                if (!reader.IsDBNull(0))
                {
                    invNo = reader.GetInt32(0) + 1;
                }
            }

            CloseConnection();
            return invNo;
        }
        public Request InitiatePayment(Invoice inv, decimal paymentAmount)
        {
            return PaymentCard.ProcessOutOfScopeAsync(inv, paymentAmount);
            //Payment payment = new Payment();
            //payment.cardResponse = PaymentCard.ProcessOutOfScopeAsync(inv, paymentAmount);
            //payment.paymentType = Payment.PaymentTypes.PaymentCard;
            //payment.paymentAmount = decimal.Parse(payment.cardResponse.xAuthAmount == String.Empty ? "0" : payment.cardResponse.xAuthAmount);
            //return payment;
        }
        
        #endregion

        #region Global Properties

        public string RetrievePropertyString(string key)
        {
            string property = String.Empty;
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText = "SELECT VALUE FROM GLOBALPROPERTIES WHERE KEY = $KEY";
            SqliteParameter p1 = new SqliteParameter("$KEY", key);
            p1.Value = key;
            command.Parameters.Add(p1);
            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                property = reader.GetString(0);
            }

            CloseConnection();
            return property;
        }

        #endregion

        #region Shortcut Menus
        public List<ItemShortcutMenu> RetrieveShortcutMenus()
        {
            List<ItemShortcutMenu> menus = new List<ItemShortcutMenu>();
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT * FROM SHORTCUTMENUS";
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                return null;
            }

            while (reader.Read())
            {
                string[] itemsInMenu = reader.GetString(2).Split(',');
                List<Item> items = new List<Item>();
                foreach (string s in itemsInMenu)
                    items.Add(RetrieveItem(s.Split(':')[1], s.Split(':')[0], true));

                ItemShortcutMenu menu = new ItemShortcutMenu();
                menu.menuName = reader.GetString(1);
                menu.menuItems = items;
                menu.parentMenu = reader.GetString(3);
                menus.Add(menu);
            }

            CloseConnection();
            return menus;
        }
        #endregion

        #region Barcodes
        public void AddBarcode(string itemnumber, string productline, string barcode, decimal quantity)
        {
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "INSERT INTO BARCODES ( BARCODETYPE, BARCODEVALUE, SCANNEDITEMNUMBER, SCANNEDPRODUCTLINE, SCANNEDQUANTITY) " +
                "VALUES ($TYPE, $VALUE, $ITEMNUMBER, $PRODUCTLINE, $QUANTITY)";
            SqliteParameter p1 = new SqliteParameter("$TYPE", "UPCA");
            SqliteParameter p2 = new SqliteParameter("$VALUE", barcode);
            SqliteParameter p3 = new SqliteParameter("$ITEMNUMBER", itemnumber);
            SqliteParameter p4 = new SqliteParameter("$PRODUCTLINE", productline);
            SqliteParameter p5 = new SqliteParameter("$QUANTITY", quantity);
            command.Parameters.Add(p1);
            command.Parameters.Add(p2);
            command.Parameters.Add(p3);
            command.Parameters.Add(p4);
            command.Parameters.Add(p5);

            command.ExecuteNonQuery();

            CloseConnection();
        }
        public List<string> RetrieveBarcode(Item item)
        {
            List<string> barcodes = new List<string>();
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT BARCODEVALUE FROM BARCODES WHERE SCANNEDITEMNUMBER = $ITEMNUMBER AND SCANNEDPRODUCTLINE = $LINE";
            SqliteParameter p1 = new SqliteParameter("$ITEMNUMBER", item.itemNumber);
            SqliteParameter p2 = new SqliteParameter("$LINE", item.productLine);
            command.Parameters.Add(p1);
            command.Parameters.Add(p2);
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                return null;
            }

            while (reader.Read())
            {
                barcodes.Add(reader.GetString(0));
            }

            CloseConnection();
            return barcodes;
        }
        #endregion

        #region Reports
        public List<string> RetrieveTableNames()
        {
            List<string> tables = new List<string>();
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT NAME FROM sqlite_master WHERE TYPE = 'table'";
            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                tables.Add(reader.GetString(0));
            }

            CloseConnection();
            return tables;
        }
        public List<string> RetrieveTableHeaders(string table)
        {
            List<string> headers = new List<string>();
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                $"PRAGMA table_info({table})";
            //SqliteParameter p1 = new SqliteParameter("$TABLE", table);
            //command.Parameters.Add(p1);
            SqliteDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                CloseConnection();
                return null;
            }

            while (reader.Read())
                headers.Add(reader.GetString(1));

            CloseConnection();
            return headers;
        }
        public List<object> ReportQuery(string query, int columns)
        {
            List<object> data = new List<object>();
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText = query;
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i != columns; i++)
                {
                    data.Add(reader.GetValue(i));
                }
            }

            CloseConnection();
            return data;
        }
        public void SaveReport(Report report)
        {
            OpenConnection();

            string conditions = string.Empty;
            string fields = string.Empty;
            string totals = string.Empty;

            foreach (string condition in report.Conditions)
                conditions += condition + "|";
            conditions = conditions.Trim('|');
            foreach (string field in report.Fields)
                fields += field + "|";
            fields = fields.Trim('|');
            foreach (string total in report.Totals)
                totals += total + "|";
            totals = totals.Trim('|');

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "INSERT INTO REPORTS " +
                "(REPORTNAME,REPORTSHORTCODE,DATASOURCE,CONDITIONS,FIELDS,TOTALS) " +
                "VALUES ($REPORTNAME,$REPORTSHORTCODE,$DATASOURCE,$CONDITIONS,$FIELDS,$TOTALS)";

            SqliteParameter p1 = new SqliteParameter("$REPORTNAME", report.ReportName);
            SqliteParameter p2 = new SqliteParameter("$REPORTSHORTCODE", report.ReportShortcode);
            SqliteParameter p3 = new SqliteParameter("$DATASOURCE", report.DataSource);
            SqliteParameter p4 = new SqliteParameter("$CONDITIONS", conditions);
            SqliteParameter p5 = new SqliteParameter("$FIELDS", fields);
            SqliteParameter p6 = new SqliteParameter("$TOTALS", totals);

            command.Parameters.Add(p1);
            command.Parameters.Add(p2);
            command.Parameters.Add(p3);
            command.Parameters.Add(p4);
            command.Parameters.Add(p5);
            command.Parameters.Add(p6);

            command.ExecuteNonQuery();

            CloseConnection();
            return;
        }
        public Report RetrieveReport(string shortcode)
        {
            Report report;
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT * FROM REPORTS WHERE REPORTSHORTCODE = $CODE";
            SqliteParameter p1 = new SqliteParameter("$CODE", shortcode);
            command.Parameters.Add(p1);
            SqliteDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                CloseConnection();
                return null;
            }

            string reportName = string.Empty;
            string dataSource = string.Empty;
            List<string> conditions = new List<string>();
            List<string> fields = new List<string>();
            List<string> totals = new List<string>();

            while (reader.Read())
            {
                reportName = reader.GetString(0);
                dataSource = reader.GetString(2);
                foreach (string condition in reader.GetString(3).Split('|'))
                    conditions.Add(condition);
                foreach (string field in reader.GetString(4).Split('|'))
                    fields.Add(field);
                if (reader.GetString(5) != string.Empty)
                {
                    foreach (string total in reader.GetString(5).Split('|'))
                        totals.Add(total);
                }
            }

            CloseConnection();
            report = new Report(fields, dataSource, conditions, totals) { ReportName = reportName, ReportShortcode = shortcode };
            report.tableheaders = RetrieveTableHeaders(report.DataSource);
            report.GenerateQuery();
            return report;
        }
        public List<string> RetrieveAvailableReports()
        {
            List<string> reports = new List<string>();
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT REPORTSHORTCODE, REPORTNAME FROM REPORTS";
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                reports.Add(reader.GetString(0) + " " + reader.GetString(1));
            }

            CloseConnection();
            return reports;
        }
        #endregion

        #region POs and Checkins
        public int RetrieveNextPONumber(bool connectionOpened = false)
        {
            int PONumber = 0;
            if (!connectionOpened)
                OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT MAX(PONUMBER) FROM PURCHASEORDERS";
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                if (!connectionOpened)
                    CloseConnection();
                return PONumber;
            }

            while (reader.Read())
            {
                if (reader.IsDBNull(0))
                {
                    if (!connectionOpened)
                        CloseConnection();
                    return PONumber;
                }
                PONumber = reader.GetInt32(0) + 1;
            }

            if (!connectionOpened)
                CloseConnection();
            return PONumber;
        }
        public List<PurchaseOrder> RetrievePurchaseOrders()
        {
            List<int> poNumbers = new List<int>();
            List<PurchaseOrder> order = new List<PurchaseOrder>();
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT PONUMBER FROM PURCHASEORDERS";
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                return null;
            }
            while (reader.Read())
                poNumbers.Add(reader.GetInt32(0));
            CloseConnection();

            foreach (int po in poNumbers)
                order.Add(RetrievePurchaseOrder(po));
            return order;
        }
        public PurchaseOrder RetrievePurchaseOrder(int PONumber, bool connectionOpened = false)
        {
            PurchaseOrder po = new PurchaseOrder("NONE", RetrieveNextPONumber(connectionOpened));
            if (!connectionOpened)
                OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT * FROM PURCHASEORDERS WHERE PONUMBER = $PONUMBER";
            SqliteParameter p1 = new SqliteParameter("$PONUMBER", PONumber);
            command.Parameters.Add(p1);
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                if (!connectionOpened)
                    CloseConnection();
                return null;
            }
            while (reader.Read())
            {
                po.PONumber = reader.GetInt32(0);
                po.totalCost = reader.GetDecimal(1);
                po.totalItems = reader.GetDecimal(2);
                po.assignedCheckin = reader.GetInt32(3);
                po.supplier = reader.GetString(4);
                po.finalized = reader.GetBoolean(5);
                po.shippingCost = reader.GetDecimal(6);
            }
            reader.Close();

            command.CommandText =
                "SELECT * FROM PURCHASEORDERITEMS WHERE PONUMBER = $PONUMBER";
            reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                if (!connectionOpened)
                    CloseConnection();
                return null;
            }
            while (reader.Read())
            {
                InvoiceItem i = new InvoiceItem();
                i.itemNumber = reader.GetString(2);
                i.productLine = reader.GetString(3);
                i.quantity = reader.GetDecimal(4);
                i.cost = reader.GetDecimal(5);
                i.price = reader.GetDecimal(6);
                po.items.Add(i);
            }

            if (!connectionOpened)
                CloseConnection();
            return po;
        }
        public void SavePurchaseOrder(PurchaseOrder PO)
        {
            bool exists = false;
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT * FROM PURCHASEORDERS WHERE PONUMBER = $PO";
            SqliteParameter p8 = new SqliteParameter("$PO", PO.PONumber);
            command.Parameters.Add(p8);
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
                exists = true;
            reader.Close();
            command.Parameters.Clear();

            if (exists)
                DeletePurchaseOrder(PO.PONumber, true);

            command.CommandText =
                "INSERT INTO PURCHASEORDERS (" +
                "PONUMBER, TOTALCOST, TOTALITEMS, ASSIGNEDCHECKIN, SUPPLIER, FINALIZED, SHIPPINGCOST) " +
                "VALUES ($PONUMBER, $COST, $TOTALITEMS, $CHECKIN, $SUPPLIER, $FINALIZED, $SHIPPING)";
            SqliteParameter p1 = new SqliteParameter("$PONUMBER", PO.PONumber);
            SqliteParameter p2 = new SqliteParameter("$COST", PO.totalCost);
            SqliteParameter p3 = new SqliteParameter("$TOTALITEMS", PO.totalItems);
            SqliteParameter p4 = new SqliteParameter("$CHECKIN", PO.assignedCheckin);
            SqliteParameter p5 = new SqliteParameter("$SUPPLIER", PO.supplier);
            SqliteParameter p6 = new SqliteParameter("$FINALIZED", PO.finalized);
            SqliteParameter p7 = new SqliteParameter("$SHIPPING", PO.shippingCost);
            command.Parameters.Add(p1);
            command.Parameters.Add(p2);
            command.Parameters.Add(p3);
            command.Parameters.Add(p4);
            command.Parameters.Add(p5);
            command.Parameters.Add(p6);
            command.Parameters.Add(p7);

            command.ExecuteNonQuery();

            command.CommandText =
                "INSERT INTO PURCHASEORDERITEMS (" +
                "PONUMBER, ID, ITEMNUMBER, PRODUCTLINE, QUANTITY, COST, PRICE) " +
                "VALUES ($PONUMBER, $ID, $ITEMNUMBER, $PRODUCTLINE, $QTY, $COST, $PRICE)";

            foreach (InvoiceItem item in PO.items)
            {
                p1 = new SqliteParameter("$PONUMBER", PO.PONumber);
                p2 = new SqliteParameter("$ID", Guid.NewGuid());
                p3 = new SqliteParameter("$ITEMNUMBER", item.itemNumber);
                p4 = new SqliteParameter("$PRODUCTLINE", item.productLine);
                p5 = new SqliteParameter("$QTY", item.quantity);
                p6 = new SqliteParameter("$COST", item.cost);
                p7 = new SqliteParameter("$PRICE", item.price);
                command.Parameters.Clear();
                command.Parameters.Add(p1);
                command.Parameters.Add(p2);
                command.Parameters.Add(p3);
                command.Parameters.Add(p4);
                command.Parameters.Add(p5);
                command.Parameters.Add(p6);
                command.Parameters.Add(p7);
                command.ExecuteNonQuery();
            }

            CloseConnection();
        }
        public void FinalizePurchaseOrder(PurchaseOrder PO)
        {
            DeletePurchaseOrder(PO.PONumber);

            SavePurchaseOrder(PO);
        }
        public void DeletePurchaseOrder(int PONumber, bool connectionOpened = false)
        {
            if (!connectionOpened)
                OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "DELETE FROM PURCHASEORDERS WHERE PONUMBER = $PO";
            SqliteParameter p1 = new SqliteParameter("$PO", PONumber);
            command.Parameters.Add(p1);

            command.ExecuteNonQuery();

            command.CommandText =
                "DELETE FROM PURCHASEORDERITEMS WHERE PONUMBER = $PO";
            command.ExecuteNonQuery();

            if (!connectionOpened)
                CloseConnection();
        }
        public int RetrieveNextCheckinNumber()
        {
            int checkinNumber = 10000;
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT MAX(CHECKINNUMBER) FROM CHECKINS";
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                return checkinNumber;
            }

            while (reader.Read())
            {
                if (reader.IsDBNull(0))
                {
                    CloseConnection();
                    return checkinNumber;
                }
                checkinNumber = reader.GetInt32(0) + 1;
            }

            CloseConnection();
            return checkinNumber;
        }
        public void SaveCheckin(Checkin checkin)
        {
            string POs = string.Empty;
            foreach (PurchaseOrder po in checkin.orders)
            {
                POs += po.PONumber.ToString() + ",";
            }
            POs = POs.Trim(',');

            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "INSERT INTO CHECKINS (CHECKINNUMBER, PONUMBERS) " +
                "VALUES ($CHECKINNUMBER, $PONUMBERS)";
            SqliteParameter p1 = new SqliteParameter("$CHECKINNUMBER", checkin.checkinNumber);
            SqliteParameter p2 = new SqliteParameter("$PONUMBERS", POs);
            command.Parameters.Add(p1);
            command.Parameters.Add(p2);

            command.ExecuteNonQuery();

            command.CommandText =
                "INSERT INTO CHECKINITEMS (CHECKINNUMBER, PRODUCTLINE, ITEMNUMBER, ORDEREDQTY, SHIPPEDQTY, RECEIVEDQTY, DAMAGEDQTY) " +
                "VALUES ($CHECKINNUMBER, $LINECODE, $ITEMNUMBER, $ORDERED, $SHIPPED, $RECEIVED, $DAMAGED)";
            
            foreach (CheckinItem item in checkin.items)
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqliteParameter("$CHECKINNUMBER", checkin.checkinNumber));
                command.Parameters.Add(new SqliteParameter("$LINECODE", item.productLine));
                command.Parameters.Add(new SqliteParameter("$ITEMNUMBER", item.itemNumber));
                command.Parameters.Add(new SqliteParameter("$ORDERED", item.ordered));
                command.Parameters.Add(new SqliteParameter("$SHIPPED", item.shipped));
                command.Parameters.Add(new SqliteParameter("$RECEIVED", item.received));
                command.Parameters.Add(new SqliteParameter("$DAMAGED", item.damaged));
                command.ExecuteNonQuery();
            }

            foreach (PurchaseOrder order in checkin.orders)
            {
                command.CommandText =
                    "UPDATE PURCHASEORDERS SET ASSIGNEDCHECKIN = $CHECKIN WHERE PONUMBER = $ORDER";
                command.Parameters.Clear();
                command.Parameters.Add(new SqliteParameter("$CHECKIN", checkin.checkinNumber));
                command.Parameters.Add(new SqliteParameter("$ORDER", order.PONumber));
                command.ExecuteNonQuery();
            }

            CloseConnection();
            return;
        }
        public List<Checkin> RetrieveCheckins()
        {
            List<int> checkinNumbers = new List<int>();
            List<Checkin> checkins = new List<Checkin>();
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT CHECKINNUMBER FROM CHECKINS";
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                return checkins;
            }
            while (reader.Read())
                checkinNumbers.Add(reader.GetInt32(0));
            CloseConnection();

            foreach (int checkinNumber in checkinNumbers)
                checkins.Add(RetrieveCheckin(checkinNumber));
            return checkins;
        }
        public Checkin RetrieveCheckin(int checkinNumber)
        {
            Checkin checkin = new Checkin(RetrieveNextCheckinNumber());
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT * FROM CHECKINS WHERE CHECKINNUMBER = $CHECKINNUMBER";
            SqliteParameter p1 = new SqliteParameter("$CHECKINNUMBER", checkinNumber);
            command.Parameters.Add(p1);
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                return null;
            }
            while (reader.Read())
            {
                checkin.checkinNumber = reader.GetInt32(0);
                foreach (string orderNo in reader.GetString(1).Split(','))
                {
                    PurchaseOrder number = RetrievePurchaseOrder(int.Parse(orderNo), true);
                    checkin.orders.Add(number);
                }
            }
            reader.Close();

            command.CommandText =
                "SELECT * FROM CHECKINITEMS WHERE CHECKINNUMBER = $CHECKINNUMBER";
            reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                return null;
            }
            while (reader.Read())
            {
                CheckinItem item = new CheckinItem(reader.GetString(2), reader.GetString(1));
                item.ordered = reader.GetDecimal(3);
                item.shipped = reader.GetDecimal(4);
                item.received = reader.GetDecimal(5);
                item.damaged = reader.GetDecimal(6);
                checkin.items.Add(item);
            }

            CloseConnection();
            return checkin;
        }
        public void DeleteCheckin(int checkinNumber)
        {
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "DELETE FROM CHECKINS WHERE CHECKINNUMBER = $CHECKIN";
            command.Parameters.Add(new SqliteParameter("$CHECKIN", checkinNumber));
            command.ExecuteNonQuery();

            command.CommandText =
                "DELETE FROM CHECKINITEMS WHERE CHECKINNUMBER = $CHECKIN";
            command.Parameters.Clear();
            command.Parameters.Add(new SqliteParameter("$CHECKIN", checkinNumber));
            command.ExecuteNonQuery();

            command.CommandText =
                "UPDATE PURCHASEORDERS SET ASSIGNEDCHECKIN = 0 WHERE ASSIGNEDCHECKIN = $CHECKIN";
            command.Parameters.Clear();
            command.Parameters.Add(new SqliteParameter("$CHECKIN", checkinNumber));
            command.ExecuteNonQuery();

            CloseConnection();
            return;
        }
        public void UpdateCheckinItem(CheckinItem item, int checkinNumber)
        {
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "UPDATE CHECKINITEMS SET ORDEREDQTY = $ORDERED, SHIPPEDQTY = $SHIPPED, RECEIVEDQTY = $RECEIVED, DAMAGEDQTY = $DAMAGED " +
                "WHERE (CHECKINNUMBER = $CHECKIN AND ITEMNUMBER = $ITEMNO AND PRODUCTLINE = $LINE)";
            command.Parameters.Add(new SqliteParameter("$ORDERED", item.ordered));
            command.Parameters.Add(new SqliteParameter("$SHIPPED", item.shipped));
            command.Parameters.Add(new SqliteParameter("$RECEIVED", item.received));
            command.Parameters.Add(new SqliteParameter("$DAMAGED", item.damaged));
            command.Parameters.Add(new SqliteParameter("$CHECKIN", checkinNumber));
            command.Parameters.Add(new SqliteParameter("$ITEMNO", item.itemNumber));
            command.Parameters.Add(new SqliteParameter("$LINE", item.productLine));
            command.ExecuteNonQuery();

            CloseConnection();
        }
        #endregion

        #region Accounts
        public List<Account> RetrieveAccounts()
        {
            List<Account> accounts = new List<Account>();
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT * FROM ACCOUNTS";
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                return null;
            }

            while (reader.Read())
            {
                Account acct = new Account();
                acct.ID = reader.GetInt32(0);
                Enum.TryParse(reader.GetString(1), out acct.Type);
                acct.Name = reader.GetString(2);
                acct.Description = reader.GetString(3);
                acct.Balance = reader.GetDecimal(4);
                accounts.Add(acct);
            }
            CloseConnection();
            return accounts;
        }
        public List<Transaction> RetrieveAccountTransactions(string accountName)
        {
            List<Transaction> transactions = new List<Transaction>();
            int accountID = RetrieveAccounts().Find(el => el.Name == accountName).ID;
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT * FROM ACCOUNTTRANSACTIONS WHERE CREDITACCOUNT = $ACCT OR DEBITACCOUNT = $ACCT";
            command.Parameters.Add(new SqliteParameter("$ACCT", accountID));
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                return transactions;
            }

            while (reader.Read())
            {
                int creditAccount = reader.GetInt32(4);
                int debitAccount = reader.GetInt32(5);
                decimal amount = reader.GetDecimal(6);
                transactions.Add(new Transaction(debitAccount, creditAccount, amount)
                {
                    ID = reader.GetInt32(0),
                    transactionID = reader.GetInt32(1),
                    date = DateTime.Parse(reader.GetString(2)),
                    memo = reader.GetString(3),
                    referenceNumber = reader.GetInt32(7)
                });
            }

            CloseConnection();
            return transactions;
        }
        public int RetrieveNextTransactionNumber()
        {
            int tNo = 0;
            OpenConnection();
            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT MAX(TRANSACTIONID) FROM ACCOUNTTRANSACTIONS";
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                return tNo;
            }

            while (reader.Read())
            {
                if (!reader.IsDBNull(0))
                    tNo = reader.GetInt32(0) + 1;
            }

            CloseConnection();
            return tNo + 1;
        }
        public void SaveTransaction(Transaction t)
        {
            int tID = RetrieveNextTransactionNumber();
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "INSERT INTO ACCOUNTTRANSACTIONS (TRANSACTIONID, DATE, MEMO, CREDITACCOUNT, DEBITACCOUNT, AMOUNT, REFERENCENUMBER)" +
                "VALUES ($TID, $DATE, $MEMO, $CA, $DA, $AMT, $REF)";
            command.Parameters.Add(new SqliteParameter("$TID", tID));
            command.Parameters.Add(new SqliteParameter("$DATE", t.date.ToString("MM/dd/yyyy")));
            command.Parameters.Add(new SqliteParameter("$MEMO", t.memo));
            command.Parameters.Add(new SqliteParameter("$CA", t.creditAccount));
            command.Parameters.Add(new SqliteParameter("$DA", t.debitAccount));
            command.Parameters.Add(new SqliteParameter("$AMT", t.amount));
            command.Parameters.Add(new SqliteParameter("$REF", t.referenceNumber));
            command.ExecuteNonQuery();

            CloseConnection();
        }
        public void UpdateAccountBalance(int accountID, decimal newBalance)
        {
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "UPDATE ACCOUNTS SET CURRENTBALANCE = $BAL WHERE ID = $ACCT";
            command.Parameters.Add(new SqliteParameter("$BAL", newBalance));
            command.Parameters.Add(new SqliteParameter("$ACCT", accountID));
            command.ExecuteNonQuery();

            CloseConnection();
        }
        #endregion

        #region Devices
        public bool DeviceExists(string address)
        {
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT * FROM DEVICES WHERE IPADDRESS = $ADDR";
            command.Parameters.Add(new SqliteParameter("$ADDR", address));
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                return false;
            }

            CloseConnection();
            return true;
        }
        public List<string> RetrieveTerminals()
        {
            List<string> terms = new List<string>();
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT NICKNAME FROM DEVICES WHERE DEVICETYPE = 'TERMINAL'";
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                terms.Add(reader.GetString(0));

            CloseConnection();
            return terms;
        }

        #endregion

        #region Misc
        private string GetClientAddress()
        {
            // creating object of service when request comes   
            OperationContext context = OperationContext.Current;
            //Getting Incoming Message details   
            MessageProperties prop = context.IncomingMessageProperties;
            //Getting client endpoint details from message header   
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            return endpoint.Address;
        }
        #endregion
    }
}
