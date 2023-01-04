using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Collections.Generic;
using System.Net;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Text;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml.Linq;

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
        internal static AuthKey BypassKey = new AuthKey("3ncrYqtEdbypa$$K3yF0rInt3rna1u$e");

        public static void Init()
        {
            Keys.Add(BypassKey);
            ReceiptPrinter.Init(new TIMSServiceModel());
        }

        private static AuthContainer<DataType> CheckAuthorization<DataType>(AuthKey key)
        {
            AuthContainer<DataType> container = new AuthContainer<DataType>();

            AuthKey localKey = Keys.Find(el => el.ID == key.ID);
            if (localKey == null || !localKey.Match(key))
            {
                container.Key = new AuthKey();
                container.Key.Success = false;
                return container;
            }
            else
            {
                //Console.WriteLine("Key Match");
                container.Key = key;
                container.Key.Success = true;
                localKey.Regenerate();
            }

            return container;
        }

        private static string GetClientAddress()
        {
            // creating object of service when request comes   
            OperationContext context = OperationContext.Current;
            //Getting Incoming Message details   
            MessageProperties prop = context.IncomingMessageProperties;
            //Getting client endpoint details from message header   
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            return endpoint.Address;
        }

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

            return value;
        }
        public Employee Login(string user, byte[] pass)
        {
            Console.WriteLine("Login Called for user: " + user);
            #region GetIP
            string addr = GetClientAddress();
            if (!DeviceExists(addr) && addr != "127.0.0.1")
            {
                Console.WriteLine("Terminal: " + addr + " is not registered with the server. Please refer to the user manual on how to register devices.");
                return new Employee() { key = new AuthKey() { Success = false } };
            }
            #endregion

            Employee e = new Employee();

            #region Authorization Initialization
            e.key = new AuthKey() { Success = true };
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
        
        public AuthContainer<List<Item>> CheckItemNumber(string itemNumber, bool connectionOpened, AuthKey key)
        {
            AuthContainer<List<Item>> container = CheckAuthorization<List<Item>>(key);
            if (!container.Key.Success)
                return container;
            container.Data = new List<Item>();

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
                container.Data.Add(item);
                //if (fixedPL == fixedIN)
                //    break;
            }

            if (!connectionOpened)
                Program.CloseConnection();
            //if (container.Data.Count == 0)
            //    return null;
            //else
                return container;
        }    
        public AuthContainer<List<Item>> CheckItemNumberFromSupplier(string itemNumber, string supplier, AuthKey key)
        {
            AuthContainer<List<Item>> container = CheckAuthorization<List<Item>>(key);
            if (!container.Key.Success)
                return container;
            container.Data = new List<Item>();

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
                container.Data.Add(item);
                //if (fixedPL == fixedIN)
                //    break;
            }

            Program.CloseConnection();
            //if (container.Data.Count == 0)
            //    return null;
            //else
                return container;
        }
        public AuthContainer<List<Item>> RetrieveItemsFromSupplier(string supplier, AuthKey key)
        {
            AuthContainer<List<Item>> container = CheckAuthorization<List<Item>>(key);
            if (!container.Key.Success)
                return container;
            container.Data = new List<Item>();

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
                container.Data.Add(item);
            }

            Program.CloseConnection();
            //if (container.Data.Count == 0)
            //    return null;
            //else
                return container;
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
                {
                    if (reader.IsDBNull(0))
                        lastOrderDate = DateTime.MinValue;
                    else
                        lastOrderDate = DateTime.Parse(reader.GetString(0));
                }

            CloseConnection();

            supplierItems = RetrieveItemsFromSupplier(supplier, BypassKey).Data;
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
            productLine = productLine.ToUpper();
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
            if (CheckProductLine(productLine))
                return;

            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "INSERT INTO PRODUCTLINES (PRODUCTLINE) VALUES ($LINE)";
            SqliteParameter p1 = new SqliteParameter("$LINE", productLine);
            command.Parameters.Add(p1);
            command.ExecuteNonQuery();

            CloseConnection();
        }
        
        public AuthContainer<List<Item>> SearchItemsByQuery(string query, AuthKey key)
        {
            AuthContainer<List<Item>> container = CheckAuthorization<List<Item>>(key);
            if (!container.Key.Success)
                return container;
            container.Data = new List<Item>();
            List<Item> results = new List<Item>();
            string[] elements = query.Split(' ');

            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.Parameters.Clear();
            command.CommandText = "SELECT PRODUCTLINE, ITEMNUMBER FROM ITEMS WHERE ITEMNUMBER LIKE $QUERY";
            command.Parameters.Add(new SqliteParameter("$QUERY", "%" + query + "%"));
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (results.Where(el => el.itemNumber == reader.GetString(1) && el.productLine == reader.GetString(0)).Count() <= 0)
                    results.Add(new Item() { productLine = reader.GetString(0), itemNumber = reader.GetString(1) });
            }
            reader.Close();

            command.CommandText = "SELECT PRODUCTLINE, ITEMNUMBER FROM ITEMS WHERE (";
            foreach (string el in elements)
            {
                string element = el.Replace('*', '%');
                command.CommandText += "ITEMNAME LIKE '%" + element + "%' AND ";
            }
            command.CommandText = command.CommandText.Trim().Trim('D').Trim('N').Trim('A').Trim() + ")";
            command.Parameters.Clear();
            
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (results.Where(el => el.itemNumber == reader.GetString(1) && el.productLine == reader.GetString(0)).Count() <= 0)
                    results.Add(new Item() { productLine = reader.GetString(0), itemNumber = reader.GetString(1) });
            }
            reader.Close();

            CloseConnection();
            if (results.Count > 0)
            {
                foreach (Item item in results)
                {
                    container.Data.Add(RetrieveItem(item.itemNumber, item.productLine));
                }
            }

            return container;
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
                item.UPC = reader.GetValue(34).ToString();
                item.lastLabelDate = reader.GetDateTime(35);
                item.lastLabelPrice = reader.GetDecimal(36);
                item.dateLastSale = reader.GetDateTime(37);
                item.manufacturerNumber = reader.GetString(38);
                item.lastSalePrice = reader.GetDecimal(39);
                item.brand = reader.GetString(40);
                item.department = reader.GetString(41);
                item.subDepartment = reader.GetString(42);
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
        public void UpdateItem(Item newItem, bool connectionOpened = false)
        {
            if (RetrieveItem(newItem.itemNumber, newItem.productLine, connectionOpened) == null)
            {
                AddItem(newItem);
                return;
            }

            if (!connectionOpened)
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
                "SERIALIZED = $SERIALIZED, CATEGORY = $CATEGORY, DATELASTSALE = $LASTSALEDATE, MANUFACTURERNUMBER = $MANUFACTURERNO, " +
                "BRAND = $BRAND, DEPARTMENT = $DEPARTMENT, SUBDEPARTMENT = $SUBDEPARTMENT, " + 
                "UPC = $SKU, LASTLABELDATE = $LABELDATE, LASTLABELPRICE = $LABELPRICE, LASTSALEPRICE = $LASTSALEPRICE " +
                "WHERE (ITEMNUMBER = $ITEMNUMBER AND PRODUCTLINE = $PRODUCTLINE)";

            command.Parameters.Add(new SqliteParameter("$ITEMNAME", newItem.itemName));
            command.Parameters.Add(new SqliteParameter("$LONGDESCRIPTION", newItem.longDescription));
            command.Parameters.Add(new SqliteParameter("$SUPPLIER", newItem.supplier));
            command.Parameters.Add(new SqliteParameter("$GROUPCODE", newItem.groupCode));
            command.Parameters.Add(new SqliteParameter("$VELOCITYCODE", newItem.velocityCode));
            command.Parameters.Add(new SqliteParameter("$PREVIOUSVELOCITYCODE", newItem.previousYearVelocityCode));
            command.Parameters.Add(new SqliteParameter("$ITEMSPERCONTAINER", newItem.itemsPerContainer));
            command.Parameters.Add(new SqliteParameter("$STANDARDPACKAGE", newItem.standardPackage));
            command.Parameters.Add(new SqliteParameter("$DATESTOCKED", newItem.dateStocked.ToString()));
            command.Parameters.Add(new SqliteParameter("$DATELASTRECEIPT", newItem.dateLastReceipt.ToString()));
            command.Parameters.Add(new SqliteParameter("$MIN", newItem.minimum));
            command.Parameters.Add(new SqliteParameter("$MAX", newItem.maximum));
            command.Parameters.Add(new SqliteParameter("$ONHANDQTY", newItem.onHandQty));
            command.Parameters.Add(new SqliteParameter("$WIPQTY", newItem.WIPQty));
            command.Parameters.Add(new SqliteParameter("$ONORDERQTY", newItem.onOrderQty));
            command.Parameters.Add(new SqliteParameter("$BACKORDERQTY", newItem.onBackorderQty));
            command.Parameters.Add(new SqliteParameter("$DAYSONORDER", newItem.daysOnOrder));
            command.Parameters.Add(new SqliteParameter("$DAYSONBACKORDER", newItem.daysOnBackorder));
            command.Parameters.Add(new SqliteParameter("$LIST", newItem.listPrice));
            command.Parameters.Add(new SqliteParameter("$RED", newItem.redPrice));
            command.Parameters.Add(new SqliteParameter("$YELLOW", newItem.yellowPrice));
            command.Parameters.Add(new SqliteParameter("$GREEN", newItem.greenPrice));
            command.Parameters.Add(new SqliteParameter("$PINK", newItem.pinkPrice));
            command.Parameters.Add(new SqliteParameter("$BLUE", newItem.bluePrice));
            command.Parameters.Add(new SqliteParameter("$COST", newItem.replacementCost));
            command.Parameters.Add(new SqliteParameter("$AVERAGECOST", newItem.averageCost));
            command.Parameters.Add(new SqliteParameter("$TAXED", newItem.taxed));
            command.Parameters.Add(new SqliteParameter("$RESTRICTED", newItem.ageRestricted));
            command.Parameters.Add(new SqliteParameter("$MINAGE", newItem.minimumAge));
            command.Parameters.Add(new SqliteParameter("$LOCATION", newItem.locationCode));
            command.Parameters.Add(new SqliteParameter("$ITEMNUMBER", newItem.itemNumber));
            command.Parameters.Add(new SqliteParameter("$PRODUCTLINE", newItem.productLine));
            command.Parameters.Add(new SqliteParameter("$SERIALIZED", newItem.serialized));
            command.Parameters.Add(new SqliteParameter("$CATEGORY", newItem.category));
            command.Parameters.Add(new SqliteParameter("$LASTSALEDATE", newItem.dateLastSale.ToString("MM/dd/yyyy")));
            command.Parameters.Add(new SqliteParameter("$MANUFACTURERNO", newItem.manufacturerNumber == null ? DBNull.Value : newItem.manufacturerNumber));
            command.Parameters.Add(new SqliteParameter("$SKU", newItem.UPC == null ? DBNull.Value : newItem.UPC));
            command.Parameters.Add(new SqliteParameter("$LABELDATE", newItem.lastLabelDate.ToString("MM/dd/yyyy")));
            command.Parameters.Add(new SqliteParameter("$LABELPRICE", newItem.lastLabelPrice));
            command.Parameters.Add(new SqliteParameter("$LASTSALEPRICE", newItem.lastSalePrice));
            command.Parameters.Add(new SqliteParameter("$BRAND", newItem.brand));
            command.Parameters.Add(new SqliteParameter("$DEPARTMENT", newItem.department));
            command.Parameters.Add(new SqliteParameter("$SUBDEPARTMENT", newItem.subDepartment));

            command.ExecuteNonQuery();

            if (!connectionOpened)
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
            if (!CheckProductLine(item.productLine))
            {
                AddProductLine(item.productLine);
            }

            if (RetrieveItem(item.itemNumber, item.productLine) != null)
                return false;

            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "INSERT INTO ITEMS (" +
                "PRODUCTLINE, ITEMNUMBER, ITEMNAME, LONGDESCRIPTION, SUPPLIER, GROUPCODE, VELOCITYCODE, PREVIOUSYEARVELOCITYCODE, " +
                "ITEMSPERCONTAINER, STANDARDPACKAGE, DATESTOCKED, DATELASTRECEIPT, MINIMUM, MAXIMUM, ONHANDQUANTITY, WIPQUANTITY, " +
                "ONORDERQUANTITY, BACKORDERQUANTITY, DAYSONORDER, DAYSONBACKORDER, LISTPRICE, REDPRICE, YELLOWPRICE, GREENPRICE, " +
                "PINKPRICE, BLUEPRICE, REPLACEMENTCOST, AVERAGECOST, TAXED, AGERESTRICTED, MINIMUMAGE, LOCATIONCODE, SERIALIZED, " +
                "CATEGORY, BRAND, DEPARTMENT, SUBDEPARTMENT, UPC, LASTLABELDATE, LASTLABELPRICE, DATELASTSALE, MANUFACTURERNUMBER, LASTSALEPRICE) " +

                "VALUES ($PRODUCTLINE, $ITEMNUMBER, $ITEMNAME, $DESCRIPTION, $SUPPLIER, $GROUP, $VELOCITY, $PREVIOUSVELOCITY, " +
                "$ITEMSPERCONTAINER, $STDPKG, $DATESTOCKED, $DATELASTRECEIPT, $MIN, $MAX, $ONHAND, $WIPQUANTITY, " +
                "$ONORDERQTY, $BACKORDERQTY, $DAYSONORDER, $DAYSONBACKORDER, $LIST, $RED, $YELLOW, $GREEN, " +
                "$PINK, $BLUE, $COST, $AVERAGECOST, $TAXED, $AGERESTRICTED, $MINAGE, $LOCATION, $SERIALIZED, $CATEGORY, " +
                "$BRAND, $DEPARTMENT, $SUBDEPARTMENT, $SKU, $LABELDATE, $LABELPRICE, $SALEDATE, $MANUFACTURERNUMBER, $SALEPRICE)";

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
            SqliteParameter p11 = new SqliteParameter("$DATESTOCKED", item.dateStocked.ToString("MM/dd/yyyy"));
            SqliteParameter p12 = new SqliteParameter("$DATELASTRECEIPT", item.dateLastReceipt.ToString("MM/dd/yyyy"));
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
            SqliteParameter p35 = new SqliteParameter("$SKU", item.UPC ?? "");
            SqliteParameter p36 = new SqliteParameter("$LABELDATE", item.lastLabelDate.ToString("MM/dd/yyyy"));
            SqliteParameter p37 = new SqliteParameter("$LABELPRICE", item.lastLabelPrice);
            SqliteParameter p38 = new SqliteParameter("$SALEDATE", item.dateLastSale.ToString("MM/dd/yyyy"));
            SqliteParameter p39 = new SqliteParameter("$MANUFACTURERNUMBER", item.manufacturerNumber == null ? DBNull.Value : item.manufacturerNumber);
            SqliteParameter p40 = new SqliteParameter("$SALEPRICE", item.lastSalePrice);
            SqliteParameter p41 = new SqliteParameter("$BRAND", item.brand);
            SqliteParameter p42 = new SqliteParameter("$DEPARTMENT", item.department);
            SqliteParameter p43 = new SqliteParameter("$SUBDEPARTMENT", item.subDepartment);

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
            command.Parameters.Add(p36);
            command.Parameters.Add(p37);
            command.Parameters.Add(p38);
            command.Parameters.Add(p39);
            command.Parameters.Add(p40);
            command.Parameters.Add(p41);
            command.Parameters.Add(p42);
            command.Parameters.Add(p43);

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
                "SELECT PRODUCTLINE, ITEMNUMBER FROM ITEMS WHERE LASTLABELPRICE != GREENPRICE OR LASTLABELDATE < $DATE";
            command.Parameters.Add(new SqliteParameter("$DATE", DateTime.Now));
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
        
        public AuthContainer<List<Item>> RetrieveItemsFromSubdepartment(string subdepartment, string parentDepartment, AuthKey key)
        {
            AuthContainer<List<Item>> container = CheckAuthorization<List<Item>>(key);
            if (!container.Key.Success)
                return container;
            container.Data = new List<Item>();

            OpenConnection();
            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText = "SELECT ITEMNUMBER, PRODUCTLINE FROM ITEMS WHERE (SUBDEPARTMENT = $SUB AND DEPARTMENT = $DEP)";
            command.Parameters.Add(new SqliteParameter("$SUB", subdepartment));
            command.Parameters.Add(new SqliteParameter("$DEP", parentDepartment));
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                return container;
            }
            List<Item> items = new List<Item>();
            while (reader.Read())
            {
                items.Add(new Item() { itemNumber = reader.GetString(0), productLine = reader.GetString(1) });
            }
            CloseConnection();

            foreach (Item item in items)
            {
                container.Data.Add(RetrieveItem(item.itemNumber, item.productLine));
            }
            return container;
        }
        
        
        #endregion

        #region Categorization

        public AuthContainer<List<string>> RetrieveProductBrands(AuthKey key)
        {
            AuthContainer<List<string>> container = CheckAuthorization<List<string>>(key);
            if (!container.Key.Success)
                return container;
            container.Data = new List<string>();

            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                @"SELECT * FROM BRANDS";
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                return container;
            }
            while (reader.Read())
            {
                container.Data.Add(reader.GetString(1));
            }

            CloseConnection();
            return container;
        }
        public AuthContainer<List<string>> RetrieveProductCategories(AuthKey key)
        {
            AuthContainer<List<string>> container = CheckAuthorization<List<string>>(key);
            if (!container.Key.Success)
                return container;
            container.Data = new List<string>();

            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                @"SELECT * FROM CATEGORIES";
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                return container;
            }
            while (reader.Read())
            {
                container.Data.Add(reader.GetString(1));
            }

            CloseConnection();
            return container;
        }
        public AuthContainer<List<string>> RetrieveProductDepartments(AuthKey key)
        {
            AuthContainer<List<string>> container = CheckAuthorization<List<string>>(key);
            if (!container.Key.Success)
                return container;
            container.Data = new List<string>();

            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                @"SELECT * FROM DEPARTMENTS";
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                return container;
            }
            while (reader.Read())
            {
                container.Data.Add(reader.GetString(1));
            }

            CloseConnection();
            return container;
        }
        public AuthContainer<List<string>> RetrieveProductSubdepartments(string department, AuthKey key)
        {
            AuthContainer<List<string>> container = CheckAuthorization<List<string>>(key);
            if (!container.Key.Success)
                return container;
            container.Data = new List<string>();

            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                @"SELECT * FROM SUBDEPARTMENTS WHERE PARENTDEPARTMENT = $DEPARTMENT";
            command.Parameters.Add(new SqliteParameter("$DEPARTMENT", department));
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                return container;
            }
            while (reader.Read())
            {
                container.Data.Add(reader.GetString(1));
            }

            CloseConnection();
            return container;
        }
        public AuthContainer<object> AddProductBrand(string brand, AuthKey key)
        {
            AuthContainer<object> container = CheckAuthorization<object>(key);
            if (!container.Key.Success)
                return container;
            container.Data = new object();

            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                @"INSERT INTO BRANDS (BRANDNAME) VALUES ($BRAND)";
            command.Parameters.Add(new SqliteParameter("$BRAND", brand));
            command.ExecuteNonQuery();

            CloseConnection();
            return container;
        }
        public AuthContainer<object> AddProductCategory(string category, AuthKey key)
        {
            AuthContainer<object> container = CheckAuthorization<object>(key);
            if (!container.Key.Success)
                return container;
            container.Data = new object();

            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                @"INSERT INTO CATEGORIES (CATEGORYNAME) VALUES ($CATEGORY)";
            command.Parameters.Add(new SqliteParameter("$CATEGORY", category));
            command.ExecuteNonQuery();

            CloseConnection();
            return container;
        }
        public AuthContainer<object> AddProductDepartment(string department, AuthKey key)
        {
            AuthContainer<object> container = CheckAuthorization<object>(key);
            if (!container.Key.Success)
                return container;
            container.Data = new object();

            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                @"INSERT INTO DEPARTMENTS (DEPARTMENT) VALUES ($DEPARTMENT)";
            command.Parameters.Add(new SqliteParameter("$DEPARTMENT", department));
            command.ExecuteNonQuery();

            CloseConnection();
            return container;
        }
        public AuthContainer<object> AddProductSubdepartment(string parentDepartment, string subdepartment, AuthKey key)
        {
            AuthContainer<object> container = CheckAuthorization<object>(key);
            if (!container.Key.Success)
                return container;
            container.Data = new object();

            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                @"INSERT INTO SUBDEPARTMENTS (DEPARTMENT, PARENTDEPARTMENT) VALUES ($SUBDEPARTMENT, $PARENT)";
            command.Parameters.Add(new SqliteParameter("$SUBDEPARTMENT", subdepartment));
            command.Parameters.Add(new SqliteParameter("$PARENT", parentDepartment));
            command.ExecuteNonQuery();

            CloseConnection();
            return container;
        }

        #endregion

        #region Customers

        public AuthContainer<Customer> CheckCustomerNumber(string custNo, AuthKey key)
        {
            AuthContainer<Customer> container = CheckAuthorization<Customer>(key);
            if (!container.Key.Success)
                return container;
            container.Data = new Customer();

            string[] inStoreProfiles = new string[0];
            string[] onlineStoreProfiles = new string[0];

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
                container.Data = null;
                return container;
            }

            while (reader.Read())
            {
                container.Data.customerName = reader.GetString(0);
                container.Data.customerNumber = reader.GetString(1);
                inStoreProfiles = reader.GetString(2) == "" ? null : reader.GetString(2).Split(',');
                onlineStoreProfiles = reader.GetString(3) == "" ? null : reader.GetString(3).Split(',');
                container.Data.inStorePricingProfile = new PricingProfileCollection();
                container.Data.onlinePricingProfile = new PricingProfileCollection();
                container.Data.inStorePricingProfile.defaultPriceSheet = (PricingProfileElement.PriceSheets)Enum.Parse(typeof(PricingProfileElement.PriceSheets), reader.GetString(4));
                container.Data.onlinePricingProfile.defaultPriceSheet = (PricingProfileElement.PriceSheets)Enum.Parse(typeof(PricingProfileElement.PriceSheets), reader.GetString(5));
                container.Data.canCharge = reader.GetInt32(6) != 0;
                container.Data.creditLimit = reader.GetDecimal(7);
                container.Data.accountBalance = reader.GetDecimal(8);
                container.Data.phoneNumber = reader.GetString(9);
                container.Data.faxNumber = reader.GetString(10);
                container.Data.billingAddress = reader.GetString(11);
                container.Data.shippingAddress = reader.GetString(12);
                container.Data.invoiceMessage = reader.GetString(13);
                container.Data.website = reader.GetString(14);
                container.Data.email = reader.GetString(15);
                container.Data.assignedRep = reader.GetString(16);
                container.Data.businessCategory = reader.GetString(17);
                container.Data.dateAdded = DateTime.Parse(reader.GetString(18));
                container.Data.dateOfLastSale = DateTime.Parse(reader.GetString(19));
                container.Data.dateOfLastROA = DateTime.Parse(reader.GetString(20));
                container.Data.preferredLanguage = reader.GetString(21);
                container.Data.authorizedBuyers = reader.GetString(22);
                container.Data.defaultTaxTable = reader.GetString(23);
                container.Data.deliveryTaxTable = reader.GetString(24);
                container.Data.primaryTaxStatus = reader.GetString(25);
                container.Data.secondaryTaxStatus = reader.GetString(26);
                container.Data.primaryTaxExemptionNumber = reader.GetString(27);
                container.Data.secondaryTaxExemptionNumber = reader.GetString(28);
                container.Data.primaryTaxExemptionExpiration = reader.GetString(29) == string.Empty ? DateTime.MinValue.AddYears(1970) : DateTime.Parse(reader.GetString(29));
                container.Data.secondaryTaxExemptionExpiration = reader.GetString(30) == string.Empty ? DateTime.MinValue.AddYears(1970) : DateTime.Parse(reader.GetString(30));
                container.Data.printCatalogNotes = reader.GetInt32(31) != 0;
                container.Data.printBalance = reader.GetInt32(32) != 0;
                container.Data.emailInvoices = reader.GetInt32(33) != 0;
                container.Data.allowBackorders = reader.GetInt32(34) != 0;
                container.Data.allowSpecialOrders = reader.GetInt32(35) != 0;
                container.Data.exemptFromInvoiceSurcharges = reader.GetInt32(36) != 0;
                container.Data.extraInvoiceCopies = reader.GetInt32(37);
                container.Data.PORequiredThresholdAmount = reader.GetDecimal(38);
                container.Data.billingType = reader.GetString(39);

                switch (reader.GetString(39))
                {
                    case ("Cash Only"):
                        container.Data.availablePaymentTypes.Add(Payment.PaymentTypes.Cash);
                        container.Data.availablePaymentTypes.Add(Payment.PaymentTypes.Check);
                        container.Data.availablePaymentTypes.Add(Payment.PaymentTypes.PaymentCard);
                        break;
                    case ("Cash Only(Include Mobile Payments)"):
                        container.Data.availablePaymentTypes.Add(Payment.PaymentTypes.Cash);
                        container.Data.availablePaymentTypes.Add(Payment.PaymentTypes.Check);
                        container.Data.availablePaymentTypes.Add(Payment.PaymentTypes.PaymentCard);
                        container.Data.availablePaymentTypes.Add(Payment.PaymentTypes.Venmo);
                        container.Data.availablePaymentTypes.Add(Payment.PaymentTypes.CashApp);
                        container.Data.availablePaymentTypes.Add(Payment.PaymentTypes.Paypal);
                        break;
                    case ("Charge Only"):
                        container.Data.availablePaymentTypes.Add(Payment.PaymentTypes.Charge);
                        break;
                    case ("Cash Or Charge"):
                        container.Data.availablePaymentTypes.Add(Payment.PaymentTypes.Cash);
                        container.Data.availablePaymentTypes.Add(Payment.PaymentTypes.Check);
                        container.Data.availablePaymentTypes.Add(Payment.PaymentTypes.PaymentCard);
                        container.Data.availablePaymentTypes.Add(Payment.PaymentTypes.Charge);
                        break;
                    case ("Cash Or Charge(Include Mobile Payments)"):
                        container.Data.availablePaymentTypes.Add(Payment.PaymentTypes.Cash);
                        container.Data.availablePaymentTypes.Add(Payment.PaymentTypes.Check);
                        container.Data.availablePaymentTypes.Add(Payment.PaymentTypes.PaymentCard);
                        container.Data.availablePaymentTypes.Add(Payment.PaymentTypes.Charge);
                        container.Data.availablePaymentTypes.Add(Payment.PaymentTypes.Venmo);
                        container.Data.availablePaymentTypes.Add(Payment.PaymentTypes.CashApp);
                        container.Data.availablePaymentTypes.Add(Payment.PaymentTypes.Paypal);
                        break;
                }

                container.Data.defaultToDeliver = reader.GetInt32(40) != 0;
                container.Data.deliveryRoute = reader.GetString(41);
                container.Data.travelTime = reader.GetInt32(42);
                container.Data.travelDistance = reader.GetInt32(43);
                container.Data.minimumSaleFreeDelivery = reader.GetDecimal(44);
                container.Data.deliveryCharge = reader.GetDecimal(45);
                container.Data.statementType = reader.GetString(46);
                container.Data.percentDiscount = reader.GetDecimal(47);
                container.Data.paidForByDiscount = reader.GetInt32(48);
                container.Data.dueDate = reader.GetInt32(49);
                container.Data.extraStatementCopies = reader.GetInt32(50);
                container.Data.sendInvoicesEvery_Days = reader.GetInt32(51);
                container.Data.sendAccountSummaryEvery_Days = reader.GetInt32(52);
                container.Data.emailStatements = reader.GetInt32(53) != 0;
                container.Data.statementMailingAddress = reader.GetString(54);
                container.Data.lastPaymentAmount = reader.GetDecimal(55);
                container.Data.lastPaymentDate = reader.GetString(56) == string.Empty ? DateTime.MinValue : DateTime.Parse(reader.GetString(56));
                container.Data.highestAmountOwed = reader.GetDecimal(57);
                container.Data.highestAmountOwedDate = reader.GetString(58) == string.Empty ? DateTime.MinValue : DateTime.Parse(reader.GetString(58));
                container.Data.highestAmountPaid = reader.GetDecimal(59);
                container.Data.highestAmountPaidDate = reader.GetString(60) == string.Empty ? DateTime.MinValue : DateTime.Parse(reader.GetString(60));
                container.Data.lastStatementAmount = reader.GetDecimal(61);
                container.Data.totalDue = reader.GetDecimal(62);
                container.Data.due30 = reader.GetDecimal(63);
                container.Data.due60 = reader.GetDecimal(64);
                container.Data.due90 = reader.GetDecimal(65);
                container.Data.furtherDue = reader.GetDecimal(66);
                container.Data.serviceCharge = reader.GetDecimal(67);
                container.Data.enabledTIMSServerRelations = reader.GetInt32(68) != 0;
                container.Data.relationshipKey = reader.GetString(69);
                container.Data.automaticallySendPriceUpdates = reader.GetInt32(70) != 0;
                container.Data.automaticallySendMedia = reader.GetInt32(71) != 0;
            }

            CloseConnection();

            List<PricingProfile> Profiles = RetrievePricingProfiles(BypassKey).Data;
            if (inStoreProfiles != null)
                foreach (string profile in inStoreProfiles)
                {
                    container.Data.inStorePricingProfile.Profiles.Add(Profiles.First(el => el.ProfileID.ToString().ToUpper() == profile));
                }
            if (onlineStoreProfiles != null)
                foreach (string profile in onlineStoreProfiles)
                {
                    container.Data.onlinePricingProfile.Profiles.Add(Profiles.First(el => el.ProfileID.ToString().ToUpper() == profile));
                }
            return container;
        }
        public AuthContainer<List<Customer>> GetCustomers(AuthKey key)
        {
            AuthContainer<List<Customer>> container = CheckAuthorization<List<Customer>>(key);
            if (!container.Key.Success)
                return container;
            container.Data = new List<Customer>();

            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT CustomerNumber FROM CUSTOMERS";
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                container.Data = null;
                return container;
            }

            List<string> CustomerNumbers = new List<string>();
            while (reader.Read())
            {
                CustomerNumbers.Add(reader.GetString(0));
            }
            CloseConnection();

            foreach (string number in CustomerNumbers)
            {
                container.Data.Add(CheckCustomerNumber(number, BypassKey).Data);
            }
            return container;
        }
        public AuthContainer<object> UpdateCustomer(Customer c, AuthKey key)
        {
            AuthContainer<object> container = CheckAuthorization<object>(key);
            if (!container.Key.Success)
                return container;

            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                @"UPDATE CUSTOMERS SET
                    CustomerName =                      $NAME,
                    InStorePricingProfile =             $INSTOREPROFILES,
                    OnlinePricingProfile =              $ONLINEPROFILES,
                    DefaultInStorePriceSheet =          $INSTOREPRICESHEET,
                    DefaultOnlinePriceSheet =           $ONLINEPRICESHEET,
                    CanCharge =                         $CANCHARGE,
                    CreditLimit =                       $CREDITLIMIT,
                    AccountBalance =                    $BALANCE,
                    PhoneNumber =                       $PHONE,
                    FaxNumber =                         $FAX,
                    BillingAddress =                    $BILLINGADDRESS,
                    ShippingAddress =                   $SHIPPINGADDRESS,
                    InvoiceMessage =                    $INVOICEMESSAGE,
                    Website =                           $WEBSITE,
                    Email =                             $EMAIL,
                    AssignedRep =                       $ASSIGNEDREP,
                    BusinessCategory =                  $CATEGORY,
                    DateAdded =                         $DATEADDED,
                    DateOfLastSale =                    $DATELASTSALE,
                    DateOfLastROA =                     $DATELASTROA,
                    PreferredLanguage =                 $LANGUAGE,
                    AuthorizedBuyers =                  $BUYERS,
                    DefaultTaxTable =                   $DEFAULTTAXTABLE,
                    DeliveryTaxTable =                  $DELIVERYTAXTABLE,
                    PrimaryTaxStatus =                  $PRIMARYTAXSTATUS,
                    SecondaryTaxStatus =                $SECONDARYTAXSTATUS,
                    PrimaryTaxExemptionNumber =         $PRIMARYTAXEXEMPTIONNUMBER,
                    SecondaryTaxExemptionNumber =       $SECONDARYTAXEXEMPTIONNUMBER,
                    PrimaryTaxExemptionExpiration =     $PRIMARYTAXEXEMPTEXPIRE,
                    SecondaryTaxExemptionExpiration =   $SECONDARYTAXEXEMPTEXPIRE,
                    PrintCatalogNotesOnInvoice =        $PRINTCATALOGNOTES,
                    PrintBalanceOnInvoice =             $PRINTBALANCE,
                    EmailInvoices =                     $EMAILINVOICES,
                    AllowBackorders =                   $ALLOWBACKORDERS,
                    AllowSpecialOrders =                $ALLOWSPECIALORDERS,
                    ExemptFromInvoiceSurcharges =       $EXEMPTSURCHARGES,
                    ExtraInvoiceCopies =                $EXTRAINVOICECOPIES,
                    PORequiredThresholdAmount =         $POREQDTHRESHOLD,
                    BillingType =                       $BILLINGTYPE,
                    DefaultToDeliver =                  $DEFAULTDELIVER,
                    DeliveryRoute =                     $DELIVERYROUTE,
                    TravelTime =                        $TRAVELTIME,
                    TravelDistance =                    $TRAVELDISTANCE,
                    MinimumSaleFreeDelivery =           $FREEDELIVERYMINIMUM,
                    DeliveryCharge =                    $DELIVERYCHARGE,
                    StatementType =                     $STATEMENTTYPE,
                    PercentDiscount =                   $PERCENTDISCOUNT,
                    PaidByForDiscount =                 $PAIDFORBYDISCOUNT,
                    DueDate =                           $DUEDATE,
                    ExtraStatementCopies =              $EXTRASTATEMENTCOPIES,
                    SendInvoicesEvery_Days =            $DAYSSENDINVOICES,
                    SendAccountSummaryEvery_Days =      $DAYSSENDACCOUNTSUMMARY,
                    EmailStatements =                   $EMAILSTATEMENTS,
                    StatementMailingAddress =           $STATEMENTMAILING,
                    LastPaymentAmount =                 $LASTPAYMENTAMOUNT,
                    LastPaymentDate =                   $LASTPAYMENTDATE,
                    HighestAmountOwed =                 $HIGHESTAMOUNTOWED,
                    HighestAmountOwedDate =             $HIGHESTAMOUNTOWEDDATE,
                    HighestAmountPaid =                 $HIGHESTAMOUNTPAID,
                    HighestAmountPaidDate =             $HIGHESTAMOUNTPAIDDATE,
                    LastStatementAmount =               $LASTSTATEMENTAMOUNT,
                    TotalDue =                          $TOTALDUE,
                    Due30Days =                         $DUE30,
                    Due60Days =                         $DUE60,
                    Due90Days =                         $DUE90,
                    FurtherDue =                        $FURTHERDUE,
                    ServiceCharge =                     $SERVICECHARGE,
                    EnableTIMSRelations =               $TIMSRELATIONS,
                    RelationshipKey =                   $RELATIONKEY,
                    AutomaticallySendPriceUpdates =     $AUTOPRICEUPDATES,
                    AutomaticallySendMedia =            $AUTOMEDIA 
                WHERE CUSTOMERNUMBER = $NUMBER";
            command.Parameters.Add(new SqliteParameter("$NAME", c.customerName));
            command.Parameters.Add(new SqliteParameter("$NUMBER", c.customerNumber));
            command.Parameters.Add(new SqliteParameter("$INSTOREPROFILES", c.inStorePricingProfile.ToString()));
            command.Parameters.Add(new SqliteParameter("$ONLINEPROFILES", c.onlinePricingProfile.ToString()));
            command.Parameters.Add(new SqliteParameter("$INSTOREPRICESHEET", c.inStorePricingProfile.defaultPriceSheet));
            command.Parameters.Add(new SqliteParameter("$ONLINEPRICESHEET", c.onlinePricingProfile.defaultPriceSheet));
            command.Parameters.Add(new SqliteParameter("$CANCHARGE", c.canCharge));
            command.Parameters.Add(new SqliteParameter("$CREDITLIMIT", c.creditLimit));
            command.Parameters.Add(new SqliteParameter("$BALANCE", c.accountBalance));
            command.Parameters.Add(new SqliteParameter("$PHONE", c.phoneNumber));
            command.Parameters.Add(new SqliteParameter("$FAX", c.faxNumber));
            command.Parameters.Add(new SqliteParameter("$BILLINGADDRESS", c.billingAddress));
            command.Parameters.Add(new SqliteParameter("$SHIPPINGADDRESS", c.shippingAddress));
            command.Parameters.Add(new SqliteParameter("$INVOICEMESSAGE", c.invoiceMessage));
            command.Parameters.Add(new SqliteParameter("$WEBSITE", c.website));
            command.Parameters.Add(new SqliteParameter("$EMAIL", c.email));
            command.Parameters.Add(new SqliteParameter("$ASSIGNEDREP", c.assignedRep));
            command.Parameters.Add(new SqliteParameter("$CATEGORY", c.businessCategory));
            command.Parameters.Add(new SqliteParameter("$DATEADDED", c.dateAdded));
            command.Parameters.Add(new SqliteParameter("$DATELASTSALE", c.dateOfLastSale));
            command.Parameters.Add(new SqliteParameter("$DATELASTROA", c.dateOfLastROA));
            command.Parameters.Add(new SqliteParameter("$LANGUAGE", c.preferredLanguage));
            command.Parameters.Add(new SqliteParameter("$BUYERS", c.authorizedBuyers));
            command.Parameters.Add(new SqliteParameter("$DEFAULTTAXTABLE", c.defaultTaxTable));
            command.Parameters.Add(new SqliteParameter("$DELIVERYTAXTABLE", c.deliveryTaxTable));
            command.Parameters.Add(new SqliteParameter("$PRIMARYTAXSTATUS", c.primaryTaxStatus));
            command.Parameters.Add(new SqliteParameter("$SECONDARYTAXSTATUS", c.secondaryTaxStatus));
            command.Parameters.Add(new SqliteParameter("$PRIMARYTAXEXEMPTIONNUMBER", c.primaryTaxExemptionNumber));
            command.Parameters.Add(new SqliteParameter("$SECONDARYTAXEXEMPTIONNUMBER", c.secondaryTaxExemptionNumber));
            command.Parameters.Add(new SqliteParameter("$PRIMARYTAXEXEMPTEXPIRE", c.primaryTaxExemptionExpiration));
            command.Parameters.Add(new SqliteParameter("$SECONDARYTAXEXEMPTEXPIRE", c.secondaryTaxExemptionExpiration));
            command.Parameters.Add(new SqliteParameter("$PRINTCATALOGNOTES", c.printCatalogNotes));
            command.Parameters.Add(new SqliteParameter("$PRINTBALANCE", c.printBalance));
            command.Parameters.Add(new SqliteParameter("$EMAILINVOICES", c.emailInvoices));
            command.Parameters.Add(new SqliteParameter("$ALLOWBACKORDERS", c.allowBackorders));
            command.Parameters.Add(new SqliteParameter("$ALLOWSPECIALORDERS", c.allowSpecialOrders));
            command.Parameters.Add(new SqliteParameter("$EXEMPTSURCHARGES", c.exemptFromInvoiceSurcharges));
            command.Parameters.Add(new SqliteParameter("$EXTRAINVOICECOPIES", c.extraInvoiceCopies));
            command.Parameters.Add(new SqliteParameter("$POREQDTHRESHOLD", c.PORequiredThresholdAmount));
            command.Parameters.Add(new SqliteParameter("$BILLINGTYPE", c.billingType));
            command.Parameters.Add(new SqliteParameter("$DEFAULTDELIVER", c.defaultToDeliver));
            command.Parameters.Add(new SqliteParameter("$DELIVERYROUTE", c.deliveryRoute));
            command.Parameters.Add(new SqliteParameter("$TRAVELTIME", c.travelTime));
            command.Parameters.Add(new SqliteParameter("$TRAVELDISTANCE", c.travelDistance));
            command.Parameters.Add(new SqliteParameter("$FREEDELIVERYMINIMUM", c.minimumSaleFreeDelivery));
            command.Parameters.Add(new SqliteParameter("$DELIVERYCHARGE", c.deliveryCharge));
            command.Parameters.Add(new SqliteParameter("$STATEMENTTYPE", c.statementType));
            command.Parameters.Add(new SqliteParameter("$PERCENTDISCOUNT", c.percentDiscount));
            command.Parameters.Add(new SqliteParameter("$PAIDFORBYDISCOUNT", c.paidForByDiscount));
            command.Parameters.Add(new SqliteParameter("$DUEDATE", c.dueDate));
            command.Parameters.Add(new SqliteParameter("$EXTRASTATEMENTCOPIES", c.extraStatementCopies));
            command.Parameters.Add(new SqliteParameter("$DAYSSENDINVOICES", c.sendInvoicesEvery_Days));
            command.Parameters.Add(new SqliteParameter("$DAYSSENDACCOUNTSUMMARY", c.sendAccountSummaryEvery_Days));
            command.Parameters.Add(new SqliteParameter("$EMAILSTATEMENTS", c.emailStatements));
            command.Parameters.Add(new SqliteParameter("$STATEMENTMAILING", c.statementMailingAddress));
            command.Parameters.Add(new SqliteParameter("$LASTPAYMENTAMOUNT", c.lastPaymentAmount));
            command.Parameters.Add(new SqliteParameter("$LASTPAYMENTDATE", c.lastPaymentDate));
            command.Parameters.Add(new SqliteParameter("$HIGHESTAMOUNTOWED", c.highestAmountOwed));
            command.Parameters.Add(new SqliteParameter("$HIGHESTAMOUNTOWEDDATE", c.highestAmountOwedDate));
            command.Parameters.Add(new SqliteParameter("$HIGHESTAMOUNTPAID", c.highestAmountPaid));
            command.Parameters.Add(new SqliteParameter("$HIGHESTAMOUNTPAIDDATE", c.highestAmountPaidDate));
            command.Parameters.Add(new SqliteParameter("$LASTSTATEMENTAMOUNT", c.lastStatementAmount));
            command.Parameters.Add(new SqliteParameter("$TOTALDUE", c.totalDue));
            command.Parameters.Add(new SqliteParameter("$DUE30", c.due30));
            command.Parameters.Add(new SqliteParameter("$DUE60", c.due60));
            command.Parameters.Add(new SqliteParameter("$DUE90", c.due90));
            command.Parameters.Add(new SqliteParameter("$FURTHERDUE", c.furtherDue));
            command.Parameters.Add(new SqliteParameter("$SERVICECHARGE", c.serviceCharge));
            command.Parameters.Add(new SqliteParameter("$TIMSRELATIONS", c.enabledTIMSServerRelations));
            command.Parameters.Add(new SqliteParameter("$RELATIONKEY", c.relationshipKey));
            command.Parameters.Add(new SqliteParameter("$AUTOPRICEUPDATES", c.automaticallySendPriceUpdates));
            command.Parameters.Add(new SqliteParameter("$AUTOMEDIA", c.automaticallySendMedia));
            command.ExecuteNonQuery();

            CloseConnection();
            return container;
        }
        public AuthContainer<object> AddCustomer(Customer c, AuthKey key)
        {
            AuthContainer<object> container = CheckAuthorization<object>(key);
            if (!container.Key.Success)
                return container;

            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                @"INSERT INTO CUSTOMERS (
                    CustomerName, CustomerNumber, InStorePricingProfile, OnlinePricingProfile, DefaultInStorePriceSheet, 
                    DefaultOnlinePriceSheet, CanCharge, CreditLimit, AccountBalance,
                    PhoneNumber, FaxNumber, BillingAddress, ShippingAddress, InvoiceMessage, Website,
                    Email, AssignedRep, BusinessCategory, DateAdded, DateOfLastSale, DateOfLastROA,
                    PreferredLanguage, AuthorizedBuyers, DefaultTaxTable, DeliveryTaxTable, PrimaryTaxStatus,
                    SecondaryTaxStatus, PrimaryTaxExemptionNumber, SecondaryTaxExemptionNumber, PrimaryTaxExemptionExpiration,
                    SecondaryTaxExemptionExpiration, PrintCatalogNotesOnInvoice, PrintBalanceOnInvoice, EmailInvoices,
                    AllowBackorders, AllowSpecialOrders, ExemptFromInvoiceSurcharges, ExtraInvoiceCopies,
                    PORequiredThresholdAmount, BillingType, DefaultToDeliver, DeliveryRoute, TravelTime,
                    TravelDistance, MinimumSaleFreeDelivery, DeliveryCharge, StatementType, PercentDiscount,
                    PaidByForDiscount, DueDate, ExtraStatementCopies, SendInvoicesEvery_Days, SendAccountSummaryEvery_Days,
                    EmailStatements, StatementMailingAddress, LastPaymentAmount, LastPaymentDate, HighestAmountOwed,
                    HighestAmountOwedDate, HighestAmountPaid, HighestAmountPaidDate, LastStatementAmount, TotalDue,
                    Due30Days, Due60Days, Due90Days, FurtherDue, ServiceCharge, EnableTIMSRelations,
                    RelationshipKey, AutomaticallySendPriceUpdates, AutomaticallySendMedia) 
                VALUES (
                    $NAME, $NUMBER, $INSTOREPROFILES, $ONLINEPROFILES, $INSTOREPRICESHEET, $ONLINEPRICESHEET, 
                    $CANCHARGE, $CREDITLIMIT, $BALANCE, $PHONE, $FAX, $BILLINGADDRESS, $SHIPPINGADDRESS, 
                    $INVOICEMESSAGE, $WEBSITE, $EMAIL, $ASSIGNEDREP, $CATEGORY, $DATEADDED, $DATELASTSALE, $DATELASTROA, $LANGUAGE, 
                    $BUYERS, $DEFAULTTAXTABLE, $DELIVERYTAXTABLE, $PRIMARYTAXSTATUS, $SECONDARYTAXSTATUS, $PRIMARYTAXEXEMPTIONNUMBER, 
                    $SECONDARYTAXEXEMPTIONNUMBER, $PRIMARYTAXEXEMPTEXPIRE, $SECONDARYTAXEXEMPTEXPIRE, $PRINTCATALOGNOTES, 
                    $PRINTBALANCE, $EMAILINVOICES, $ALLOWBACKORDERS, $ALLOWSPECIALORDERS, $EXEMPTSURCHARGES, $EXTRAINVOICECOPIES, 
                    $POREQDTHRESHOLD, $BILLINGTYPE, $DEFAULTDELIVER, $DELIVERYROUTE, $TRAVELTIME, $TRAVELDISTANCE, $FREEDELIVERYMINIMUM, 
                    $DELIVERYCHARGE, $STATEMENTTYPE, $PERCENTDISCOUNT, $PAIDFORBYDISCOUNT, $DUEDATE, $EXTRASTATEMENTCOPIES, 
                    $DAYSSENDINVOICES, $DAYSSENDACCOUNTSUMMARY, $EMAILSTATEMENTS, $STATEMENTMAILING, $LASTPAYMENTAMOUNT, 
                    $LASTPAYMENTDATE, $HIGHESTAMOUNTOWED, $HIGHESTAMOUNTOWEDDATE, $HIGHESTAMOUNTPAID, $HIGHESTAMOUNTPAIDDATE, 
                    $LASTSTATEMENTAMOUNT, $TOTALDUE, $DUE30, $DUE60, $DUE90, $FURTHERDUE, $SERVICECHARGE, 
                    $TIMSRELATIONS, $RELATIONKEY, $AUTOPRICEUPDATES, $AUTOMEDIA)";

            command.Parameters.Add(new SqliteParameter("$NAME", c.customerName));
            command.Parameters.Add(new SqliteParameter("$NUMBER", c.customerNumber));
            command.Parameters.Add(new SqliteParameter("$INSTOREPROFILES", c.inStorePricingProfile.ToString()));
            command.Parameters.Add(new SqliteParameter("$ONLINEPROFILES", c.onlinePricingProfile.ToString()));
            command.Parameters.Add(new SqliteParameter("$INSTOREPRICESHEET", c.inStorePricingProfile.defaultPriceSheet));
            command.Parameters.Add(new SqliteParameter("$ONLINEPRICESHEET", c.onlinePricingProfile.defaultPriceSheet));
            command.Parameters.Add(new SqliteParameter("$CANCHARGE", c.canCharge));
            command.Parameters.Add(new SqliteParameter("$CREDITLIMIT", c.creditLimit));
            command.Parameters.Add(new SqliteParameter("$BALANCE", c.accountBalance));
            command.Parameters.Add(new SqliteParameter("$PHONE", c.phoneNumber));
            command.Parameters.Add(new SqliteParameter("$FAX", c.faxNumber));
            command.Parameters.Add(new SqliteParameter("$BILLINGADDRESS", c.billingAddress));
            command.Parameters.Add(new SqliteParameter("$SHIPPINGADDRESS", c.shippingAddress));
            command.Parameters.Add(new SqliteParameter("$INVOICEMESSAGE", c.invoiceMessage));
            command.Parameters.Add(new SqliteParameter("$WEBSITE", c.website));
            command.Parameters.Add(new SqliteParameter("$EMAIL", c.email));
            command.Parameters.Add(new SqliteParameter("$ASSIGNEDREP", c.assignedRep));
            command.Parameters.Add(new SqliteParameter("$CATEGORY", c.businessCategory));
            command.Parameters.Add(new SqliteParameter("$DATEADDED", c.dateAdded));
            command.Parameters.Add(new SqliteParameter("$DATELASTSALE", c.dateOfLastSale));
            command.Parameters.Add(new SqliteParameter("$DATELASTROA", c.dateOfLastROA));
            command.Parameters.Add(new SqliteParameter("$LANGUAGE", c.preferredLanguage));
            command.Parameters.Add(new SqliteParameter("$BUYERS", c.authorizedBuyers));
            command.Parameters.Add(new SqliteParameter("$DEFAULTTAXTABLE", c.defaultTaxTable));
            command.Parameters.Add(new SqliteParameter("$DELIVERYTAXTABLE", c.deliveryTaxTable));
            command.Parameters.Add(new SqliteParameter("$PRIMARYTAXSTATUS", c.primaryTaxStatus));
            command.Parameters.Add(new SqliteParameter("$SECONDARYTAXSTATUS", c.secondaryTaxStatus));
            command.Parameters.Add(new SqliteParameter("$PRIMARYTAXEXEMPTIONNUMBER", c.primaryTaxExemptionNumber));
            command.Parameters.Add(new SqliteParameter("$SECONDARYTAXEXEMPTIONNUMBER", c.secondaryTaxExemptionNumber));
            command.Parameters.Add(new SqliteParameter("$PRIMARYTAXEXEMPTEXPIRE", c.primaryTaxExemptionExpiration));
            command.Parameters.Add(new SqliteParameter("$SECONDARYTAXEXEMPTEXPIRE", c.secondaryTaxExemptionExpiration));
            command.Parameters.Add(new SqliteParameter("$PRINTCATALOGNOTES", c.printCatalogNotes));
            command.Parameters.Add(new SqliteParameter("$PRINTBALANCE", c.printBalance));
            command.Parameters.Add(new SqliteParameter("$EMAILINVOICES", c.emailInvoices));
            command.Parameters.Add(new SqliteParameter("$ALLOWBACKORDERS", c.allowBackorders));
            command.Parameters.Add(new SqliteParameter("$ALLOWSPECIALORDERS", c.allowSpecialOrders));
            command.Parameters.Add(new SqliteParameter("$EXEMPTSURCHARGES", c.exemptFromInvoiceSurcharges));
            command.Parameters.Add(new SqliteParameter("$EXTRAINVOICECOPIES", c.extraInvoiceCopies));
            command.Parameters.Add(new SqliteParameter("$POREQDTHRESHOLD", c.PORequiredThresholdAmount));
            command.Parameters.Add(new SqliteParameter("$BILLINGTYPE", c.billingType));
            command.Parameters.Add(new SqliteParameter("$DEFAULTDELIVER", c.defaultToDeliver));
            command.Parameters.Add(new SqliteParameter("$DELIVERYROUTE", c.deliveryRoute));
            command.Parameters.Add(new SqliteParameter("$TRAVELTIME", c.travelTime));
            command.Parameters.Add(new SqliteParameter("$TRAVELDISTANCE", c.travelDistance));
            command.Parameters.Add(new SqliteParameter("$FREEDELIVERYMINIMUM", c.minimumSaleFreeDelivery));
            command.Parameters.Add(new SqliteParameter("$DELIVERYCHARGE", c.deliveryCharge));
            command.Parameters.Add(new SqliteParameter("$STATEMENTTYPE", c.statementType));
            command.Parameters.Add(new SqliteParameter("$PERCENTDISCOUNT", c.percentDiscount));
            command.Parameters.Add(new SqliteParameter("$PAIDFORBYDISCOUNT", c.paidForByDiscount));
            command.Parameters.Add(new SqliteParameter("$DUEDATE", c.dueDate));
            command.Parameters.Add(new SqliteParameter("$EXTRASTATEMENTCOPIES", c.extraStatementCopies));
            command.Parameters.Add(new SqliteParameter("$DAYSSENDINVOICES", c.sendInvoicesEvery_Days));
            command.Parameters.Add(new SqliteParameter("$DAYSSENDACCOUNTSUMMARY", c.sendAccountSummaryEvery_Days));
            command.Parameters.Add(new SqliteParameter("$EMAILSTATEMENTS", c.emailStatements));
            command.Parameters.Add(new SqliteParameter("$STATEMENTMAILING", c.statementMailingAddress));
            command.Parameters.Add(new SqliteParameter("$LASTPAYMENTAMOUNT", c.lastPaymentAmount));
            command.Parameters.Add(new SqliteParameter("$LASTPAYMENTDATE", c.lastPaymentDate));
            command.Parameters.Add(new SqliteParameter("$HIGHESTAMOUNTOWED", c.highestAmountOwed));
            command.Parameters.Add(new SqliteParameter("$HIGHESTAMOUNTOWEDDATE", c.highestAmountOwedDate));
            command.Parameters.Add(new SqliteParameter("$HIGHESTAMOUNTPAID", c.highestAmountPaid));
            command.Parameters.Add(new SqliteParameter("$HIGHESTAMOUNTPAIDDATE", c.highestAmountPaidDate));
            command.Parameters.Add(new SqliteParameter("$LASTSTATEMENTAMOUNT", c.lastStatementAmount));
            command.Parameters.Add(new SqliteParameter("$TOTALDUE", c.totalDue));
            command.Parameters.Add(new SqliteParameter("$DUE30", c.due30));
            command.Parameters.Add(new SqliteParameter("$DUE60", c.due60));
            command.Parameters.Add(new SqliteParameter("$DUE90", c.due90));
            command.Parameters.Add(new SqliteParameter("$FURTHERDUE", c.furtherDue));
            command.Parameters.Add(new SqliteParameter("$SERVICECHARGE", c.serviceCharge));
            command.Parameters.Add(new SqliteParameter("$TIMSRELATIONS", c.enabledTIMSServerRelations));
            command.Parameters.Add(new SqliteParameter("$RELATIONKEY", c.relationshipKey));
            command.Parameters.Add(new SqliteParameter("$AUTOPRICEUPDATES", c.automaticallySendPriceUpdates));
            command.Parameters.Add(new SqliteParameter("$AUTOMEDIA", c.automaticallySendMedia));
            command.ExecuteNonQuery();

            CloseConnection();
            return container;
        }

        #endregion

        #region Pricing Profiles
        public AuthContainer<List<PricingProfile>> RetrievePricingProfiles(AuthKey key)
        {
            AuthContainer<List<PricingProfile>> container = CheckAuthorization<List<PricingProfile>>(key);
            if (!container.Key.Success)
                return container;
            container.Data = new List<PricingProfile>();

            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT * FROM PRICINGPROFILES";
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                container.Data = null;
                return container;
            }
            while (reader.Read())
            {
                container.Data.Add(new PricingProfile()
                {
                    ProfileID = reader.GetInt32(0),
                    ProfileName = reader.GetString(1)
                });
            }
            reader.Close();
            command.CommandText = "SELECT * FROM PRICINGPROFILEELEMENTS WHERE PROFILEID = $PID";
            foreach (PricingProfile profile in container.Data)
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqliteParameter("$PID", profile.ProfileID));
                reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    reader.Close();
                    continue;
                }
                while (reader.Read())
                {
                    PricingProfileElement el = new PricingProfileElement();
                    el.elementID = reader.GetInt32(0);
                    el.profileID = reader.GetInt32(1);
                    el.priority = reader.GetInt32(2);
                    el.groupCode = reader.GetString(3);
                    el.department = reader.GetString(4);
                    el.subDepartment = reader.GetString(5);
                    el.productLine = reader.GetString(6);
                    el.itemNumber = reader.GetString(7);
                    el.priceSheet = (PricingProfileElement.PriceSheets)Enum.Parse(typeof(PricingProfileElement.PriceSheets), reader.GetString(8));
                    el.margin = reader.GetDecimal(9);
                    el.beginDate = DateTime.TryParse(reader.GetString(10), out DateTime i) ? (DateTime?)i : null;
                    el.endDate = DateTime.TryParse(reader.GetString(11), out DateTime j) ? (DateTime?)j : null;
                    container.Data.FirstOrDefault(ell => ell.ProfileID == profile.ProfileID).Elements.Add(el);
                }
                reader.Close();
            }

            CloseConnection();
            return container;
        }
        public AuthContainer<object> UpdatePricingProfile(PricingProfile profile, AuthKey key)
        {
            AuthContainer<object> container = CheckAuthorization<object>(key);
            if (!container.Key.Success)
                return container;

            OpenConnection();
            SqliteCommand command = sqlite_conn.CreateCommand();

            command.CommandText =
                "DELETE FROM PRICINGPROFILEELEMENTS WHERE PROFILEID = $PID";
            command.Parameters.Add(new SqliteParameter("$PID", profile.ProfileID));
            command.ExecuteNonQuery();

            command.CommandText =
                @"INSERT INTO PRICINGPROFILEELEMENTS (
                PROFILEID, PRIORITY, GROUPCODE, DEPARTMENT, SUBDEPARTMENT, PRODUCTLINE, 
                ITEMNUMBER, PRICESHEET, MARGIN, BEGINDATE, ENDDATE )
                VALUES 
                ($PID, $PRIORITY, $GROUP, $DEPARTMENT, $SUBDEPARTMENT, $PRODUCTLINE, 
                $ITEMNUMBER, $PRICESHEET, $MARGIN, $BEGIN, $END)";

            foreach (PricingProfileElement element in profile.Elements)
            {
                //Read on the null coalescing operator (??) and the null conditional operator (?.)
                command.Parameters.Clear();
                command.Parameters.Add(new SqliteParameter("$PID", element.profileID));
                command.Parameters.Add(new SqliteParameter("$PRIORITY", element.priority));
                command.Parameters.Add(new SqliteParameter("$GROUP", element.groupCode ?? String.Empty));
                command.Parameters.Add(new SqliteParameter("$DEPARTMENT", element.department ?? String.Empty));
                command.Parameters.Add(new SqliteParameter("$SUBDEPARTMENT", element.subDepartment ?? String.Empty));
                command.Parameters.Add(new SqliteParameter("$PRODUCTLINE", element.productLine ?? String.Empty));
                command.Parameters.Add(new SqliteParameter("$ITEMNUMBER", element.itemNumber ?? String.Empty));
                command.Parameters.Add(new SqliteParameter("$PRICESHEET", Enum.GetName(typeof(PricingProfileElement.PriceSheets), element.priceSheet)));
                command.Parameters.Add(new SqliteParameter("$MARGIN", element.margin));
                command.Parameters.Add(new SqliteParameter("$BEGIN", element.beginDate?.ToString() ?? String.Empty));
                command.Parameters.Add(new SqliteParameter("$END", element.endDate?.ToString() ?? String.Empty));
                command.ExecuteNonQuery();
            }

            CloseConnection();
            return container;
        }
        public AuthContainer<int> RetrieveNextPricingProfileID(AuthKey key)
        {
            AuthContainer<int> container = CheckAuthorization<int>(key);
            if (!container.Key.Success)
                return container;

            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                @"SELECT MAX(PROFILEID) FROM PRICINGPROFILES";
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                container.Data = 0;
                return container;
            }

            while (reader.Read())
            {
                if (!reader.IsDBNull(0))
                {
                    container.Data = reader.GetInt32(0) + 1;
                }
            }

            CloseConnection();
            return container;
        }
        public AuthContainer<object> AddPricingProfile(PricingProfile profile, AuthKey key)
        {
            AuthContainer<object> container = CheckAuthorization<object>(key);
            if (!container.Key.Success)
                return container;

            profile.ProfileID = RetrieveNextPricingProfileID(BypassKey).Data;
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                @"INSERT INTO PRICINGPROFILES (PROFILEID, PROFILENAME) VALUES ($PID, $PNAME)";
            command.Parameters.Add(new SqliteParameter("$PID", profile.ProfileID));
            command.Parameters.Add(new SqliteParameter("$PNAME", profile.ProfileName));
            command.ExecuteNonQuery();

            command.CommandText =
                @"INSERT INTO PRICINGPROFILEELEMENTS (
                PROFILEID, PRIORITY, GROUPCODE, DEPARTMENT, SUBDEPARTMENT, 
                PRODUCTLINE, ITEMNUMBER, PRICESHEET, MARGIN, BEGINDATE, ENDDATE) 
                VALUES (
                $PID, $PRIORITY, $GROUP, $DEPARTMENT, $SUBDEPARTMENT, 
                $LINE, $ITEMNO, $PRICESHEET, $MARGIN, $BEGIN, $END)";
            foreach (PricingProfileElement element in profile.Elements)
            {
                //Read on the null coalescing operator (??) and the null conditional operator (?.)
                command.Parameters.Clear();
                command.Parameters.Add(new SqliteParameter("$PID", profile.ProfileID));
                command.Parameters.Add(new SqliteParameter("$PRIORITY", element.priority));
                command.Parameters.Add(new SqliteParameter("$GROUP", element.groupCode ?? String.Empty));
                command.Parameters.Add(new SqliteParameter("$DEPARTMENT", element.department ?? String.Empty));
                command.Parameters.Add(new SqliteParameter("$SUBDEPARTMENT", element.subDepartment ?? String.Empty));
                command.Parameters.Add(new SqliteParameter("$LINE", element.productLine ?? String.Empty));
                command.Parameters.Add(new SqliteParameter("$ITEMNO", element.itemNumber ?? String.Empty));
                command.Parameters.Add(new SqliteParameter("$PRICESHEET", element.priceSheet));
                command.Parameters.Add(new SqliteParameter("$MARGIN", element.margin));
                command.Parameters.Add(new SqliteParameter("$BEGIN", element.beginDate?.ToString() ?? String.Empty));
                command.Parameters.Add(new SqliteParameter("$END", element.endDate?.ToString() ?? String.Empty));
                command.ExecuteNonQuery();
            }

            CloseConnection();

            return container;
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
                Payment p = new Payment();
                p.ID = reader.GetGuid(1);
                p.paymentAmount = reader.GetDecimal(3);
                p.paymentType = (Payment.PaymentTypes)Enum.Parse(typeof(Payment.PaymentTypes), reader.GetString(2));
                p.errorMessage = (Payment.CardReaderErrorMessages)Enum.Parse(typeof(Payment.CardReaderErrorMessages), reader.GetString(4));
                p.cardResponse = reader.IsDBNull(5) ? null : new IngenicoResponse(XDocument.Parse(reader.GetString(5)));
                p.cardRequest = reader.IsDBNull(6) ? null : new IngenicoRequest(reader.GetString(6));
                inv.payments.Add(p);
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

                if (item.serializedItem && !reader.IsDBNull(10))
                    item.serialNumber = reader.GetString(10);

                inv.items.Add(item);
            }

            CloseConnection();

            inv.customer = CheckCustomerNumber(inv.customer.customerNumber, BypassKey).Data;
            inv.employee = RetrieveEmployee(inv.employee.employeeNumber.ToString());
            return inv;
        }        
        public List<Invoice> RetrieveInvoicesByCriteria(string[] criteria)
        {
            List<Invoice> invoices = new List<Invoice>();
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText = "SELECT INVOICENUMBER FROM INVOICES WHERE SAVEDINVOICE = 0";
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
        public AuthContainer<List<Invoice>> RetrieveSavedInvoices(AuthKey key)
        {
            AuthContainer<List<Invoice>> container = CheckAuthorization<List<Invoice>>(key);
            if (!container.Key.Success)
                return container;
            container.Data = new List<Invoice>();

            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT * FROM INVOICES WHERE SAVEDINVOICE = \"1\"";
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                container.Data = null;
                return container;
            }

            while (reader.Read())
            {
                Invoice inv = new Invoice();
                inv.invoiceNumber = reader.GetInt32(0);
                inv.customer = new Customer() { customerNumber = reader.GetString(18)};
                inv.attentionLine = reader.GetString(9);
                inv.PONumber = reader.GetString(10);
                inv.employee = new Employee() { employeeNumber = reader.GetInt32(19) };
                inv.invoiceCreationTime = DateTime.Parse(reader.GetString(14));
                container.Data.Add(inv);
            }

            CloseConnection();
            foreach (Invoice inv in container.Data)
            {
                inv.employee = RetrieveEmployee(inv.employee.employeeNumber.ToString());
                inv.customer = CheckCustomerNumber(inv.customer.customerNumber, BypassKey).Data;
            }
            return container;
        }
        public AuthContainer<object> DeleteSavedInvoice(Invoice inv, AuthKey key)
        {
            AuthContainer<object> container = CheckAuthorization<object>(key);
            if (!container.Key.Success)
                return container;

            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                @"DELETE FROM INVOICES WHERE INVOICENUMBER = $INVNO";
            command.Parameters.Add(new SqliteParameter("$INVNO", inv.invoiceNumber));
            command.ExecuteNonQuery();

            command.CommandText =
                @"DELETE FROM INVOICEITEMS WHERE INVOICENUMBER = $INVNO";
            command.ExecuteNonQuery();

            foreach (InvoiceItem invitem in inv.items)
            {
                Item item = RetrieveItem(invitem.itemNumber, invitem.productLine);
                item.WIPQty -= invitem.quantity;
                UpdateItem(item);
            }

            CloseConnection();
            return container;
        }
        public void SaveInvoice(Invoice inv)
        {
            OpenConnection();
            bool previouslySaved = false;

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText = "SELECT INVOICENUMBER FROM INVOICES WHERE INVOICENUMBER = $INVNO";
            command.Parameters.Add(new SqliteParameter("$INVNO", inv.invoiceNumber));
            SqliteDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
                previouslySaved = true;
            reader.Close();

            #region Add General Invoice Information to INVOICES tables
            if (!previouslySaved)
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
            else
                command.CommandText =
                  @"UPDATE INVOICES SET 
                    INVOICENUMBER = $INVOICENUMBER, SUBTOTAL = $SUBTOTAL, TAXABLETOTAL = $TAXABLETOTAL, 
                    TAXRATE = $TAXRATE, TAXAMOUNT = $TAXAMOUNT, TOTAL = $TOTAL, TOTALPAYMENTS = $TOTALPAYMENTS, 
                    AGERESTRICTED = $AGERESTRICTED, CUSTOMERBIRTHDATE = $CUSTOMERBIRTHDATE, ATTENTION = $ATTENTION, 
                    PO = $PO, MESSAGE = $MESSAGE, SAVEDINVOICE = $SAVEDINVOICE, SAVEDINVOICETIME = $SAVEDINVOICETIME, 
                    INVOICECREATIONTIME = $INVOICECREATIONTIME, INVOICEFINALIZEDTIME = $INVOICEFINALIZEDTIME, 
                    FINALIZED = $FINALIZED, VOIDED = $VOIDED, CUSTOMERNUMBER = $CUSTOMERNUMBER, 
                    EMPLOYEENUMBER = $EMPLOYEENUMBER, COST = $COST, PROFIT = $PROFIT 
                    WHERE INVOICENUMBER = $INVOICENUMBER";

            command.Parameters.Add(new SqliteParameter("$INVOICENUMBER", inv.invoiceNumber));
            command.Parameters.Add(new SqliteParameter("$SUBTOTAL", inv.subtotal));
            command.Parameters.Add(new SqliteParameter("$TAXABLETOTAL", inv.taxableTotal));
            command.Parameters.Add(new SqliteParameter("$TAXRATE", inv.taxRate));
            command.Parameters.Add(new SqliteParameter("$TAXAMOUNT", inv.taxAmount));
            command.Parameters.Add(new SqliteParameter("$TOTAL", inv.total));
            command.Parameters.Add(new SqliteParameter("$TOTALPAYMENTS", inv.totalPayments));
            command.Parameters.Add(new SqliteParameter("$AGERESTRICTED", inv.containsAgeRestrictedItem));
            command.Parameters.Add(new SqliteParameter("$CUSTOMERBIRTHDATE", inv.customerBirthdate.ToString()));
            command.Parameters.Add(new SqliteParameter("$ATTENTION", inv.attentionLine));
            command.Parameters.Add(new SqliteParameter("$PO", inv.PONumber));
            command.Parameters.Add(new SqliteParameter("$MESSAGE", inv.invoiceMessage));
            command.Parameters.Add(new SqliteParameter("$SAVEDINVOICE", inv.savedInvoice));
            command.Parameters.Add(new SqliteParameter("$SAVEDINVOICETIME", inv.savedInvoiceTime.ToString()));
            command.Parameters.Add(new SqliteParameter("$INVOICECREATIONTIME", inv.invoiceCreationTime.ToString()));
            command.Parameters.Add(new SqliteParameter("$INVOICEFINALIZEDTIME", inv.invoiceFinalizedTime.ToString()));
            command.Parameters.Add(new SqliteParameter("$FINALIZED", inv.finalized));
            command.Parameters.Add(new SqliteParameter("$VOIDED", inv.voided));
            command.Parameters.Add(new SqliteParameter("$CUSTOMERNUMBER", inv.customer.customerNumber));
            command.Parameters.Add(new SqliteParameter("$EMPLOYEENUMBER", inv.employee.employeeNumber));
            command.Parameters.Add(new SqliteParameter("$COST", inv.cost));
            command.Parameters.Add(new SqliteParameter("$PROFIT", inv.profit));

            command.ExecuteNonQuery();
            #endregion

            #region Add Invoice Item information to INVOICEITEMS table
            foreach (InvoiceItem item in inv.items)
            {
                command.CommandText =
                    @"SELECT PRODUCTLINE, ITEMNUMBER, QUANTITY FROM INVOICEITEMS WHERE INVOICENUMBER = $INVOICENUMBER";
                command.Parameters.Clear();
                command.Parameters.Add(new SqliteParameter("$INVOICENUMBER", inv.invoiceNumber));
                reader = command.ExecuteReader();
                List<InvoiceItem> items = new List<InvoiceItem>();
                if (!reader.HasRows)
                    reader.Close();
                else
                {
                    while (reader.Read())
                    {
                        InvoiceItem itemm = new InvoiceItem();
                        itemm.productLine = reader.GetString(0);
                        itemm.itemNumber = reader.GetString(1);
                        itemm.quantity = reader.GetDecimal(2);
                        items.Add(itemm);
                    }
                }
                reader.Close();
                foreach (InvoiceItem itemm in items)
                {
                    Item itemmm = RetrieveItem(itemm.itemNumber, itemm.productLine, true);
                    itemmm.WIPQty -= itemm.quantity;
                    UpdateItem(itemmm, true);
                }

                command.CommandText =
                    @"DELETE FROM INVOICEITEMS WHERE INVOICENUMBER = $INVOICENUMBER";
                command.ExecuteNonQuery();

                command.CommandText =
                    "INSERT INTO INVOICEITEMS (" +

                    "INVOICENUMBER,ITEMNUMBER,PRODUCTLINE,ITEMDESCRIPTION,PRICE,LISTPRICE," +
                    "QUANTITY,TOTAL,PRICECODE,SERIALIZED,SERIALNUMBER,AGERESTRICTED," +
                    "MINIMUMAGE,TAXED,INVOICECODES,GUID,COST) " +

                    "VALUES ($INVOICENUMBER,$ITEMNUMBER,$PRODUCTLINE,$ITEMDESCRIPTION,$PRICE,$LISTPRICE," +
                    "$QUANTITY,$TOTAL,$PRICECODE,$SERIALIZED,$SERIALNUMBER,$AGERESTRICTED," +
                    "$MINIMUMAGE,$TAXED,$INVOICECODES,$GUID,$COST)";

                command.Parameters.Clear();

                command.Parameters.Add(new SqliteParameter("$INVOICENUMBER", inv.invoiceNumber));
                command.Parameters.Add(new SqliteParameter("$ITEMNUMBER", item.itemNumber));
                command.Parameters.Add(new SqliteParameter("$PRODUCTLINE", item.productLine));
                command.Parameters.Add(new SqliteParameter("$ITEMDESCRIPTION", item.itemName));
                command.Parameters.Add(new SqliteParameter("$PRICE", item.price));
                command.Parameters.Add(new SqliteParameter("$LISTPRICE", item.listPrice));
                command.Parameters.Add(new SqliteParameter("$QUANTITY", item.quantity));
                command.Parameters.Add(new SqliteParameter("$TOTAL", item.total));
                command.Parameters.Add(new SqliteParameter("$PRICECODE", item.pricingCode));
                command.Parameters.Add(new SqliteParameter("$SERIALIZED", item.serializedItem));
                command.Parameters.Add(new SqliteParameter("$SERIALNUMBER", string.IsNullOrEmpty(item.serialNumber) ? "" : item.serialNumber));
                command.Parameters.Add(new SqliteParameter("$AGERESTRICTED", item.ageRestricted));
                command.Parameters.Add(new SqliteParameter("$MINIMUMAGE", item.minimumAge));
                command.Parameters.Add(new SqliteParameter("$TAXED", item.taxed));
                string invCodes = string.Empty;
                if (item.codes != null)
                    foreach (string code in item.codes)
                        invCodes += code + ",";
                invCodes = invCodes.Trim(',');
                command.Parameters.Add(new SqliteParameter("$INVOICECODES", invCodes));
                command.Parameters.Add(new SqliteParameter("$GUID", item.ID));
                command.Parameters.Add(new SqliteParameter("$COST", item.cost));

                command.ExecuteNonQuery();

                command.CommandText =
                    @"UPDATE ITEMS SET DATELASTSALE = $DATE, LASTSALEPRICE = $PRICE WHERE (ITEMNUMBER = $ITEMNO AND PRODUCTLINE = $LINECODE)";

                command.Parameters.Clear();
                command.Parameters.Add(new SqliteParameter("$DATE", DateTime.Now.ToString("MM/dd/yyyy")));
                command.Parameters.Add(new SqliteParameter("$ITEMNO", item.itemNumber));
                command.Parameters.Add(new SqliteParameter("$LINECODE", item.productLine));
                command.Parameters.Add(new SqliteParameter("$PRICE", item.price));

                command.ExecuteNonQuery();
            }
            #endregion

            #region Add Invoice Payment Information to INVOICEPAYMENTS table
            foreach (Payment pay in inv.payments)
            {
                command.CommandText =
                    "INSERT INTO PAYMENTS (" +

                    "INVOICENUMBER,ID,PAYMENTTYPE,PAYMENTAMOUNT,CARDREADERERRORMESSAGE,INGENICORESPONSE,INGENICOREQUEST) " +

                    "VALUES ($INVOICENUMBER,$ID,$PAYMENTTYPE,$PAYMENTAMOUNT,$ERROR,$INGENICO,$REQ)";

                command.Parameters.Clear();

                command.Parameters.Add(new SqliteParameter("$INVOICENUMBER", inv.invoiceNumber));
                command.Parameters.Add(new SqliteParameter("$ID", pay.ID));
                command.Parameters.Add(new SqliteParameter("$PAYMENTTYPE", pay.paymentType.ToString()));
                command.Parameters.Add(new SqliteParameter("$PAYMENTAMOUNT", pay.paymentAmount));
                command.Parameters.Add(new SqliteParameter("$ERROR", Enum.GetName(typeof(Payment.CardReaderErrorMessages), pay.errorMessage)));
                command.Parameters.Add(new SqliteParameter("$INGENICO", pay.cardResponse == null ? DBNull.Value : pay.cardResponse.RawXMLResponse));
                command.Parameters.Add(new SqliteParameter("$REQ", pay.cardRequest == null ? DBNull.Value : pay.cardRequest.RawXMLRequest));
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
        
        #endregion

        #region Global Properties

        public AuthContainer<string> RetrieveProperty(string key, AuthKey authkey)
        {
            AuthContainer<string> container = CheckAuthorization<string>(authkey);
            if (!container.Key.Success)
                return container;
            container.Data = "";

            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText = "SELECT VALUE FROM GLOBALPROPERTIES WHERE KEY = $KEY";
            SqliteParameter p1 = new SqliteParameter("$KEY", key);
            p1.Value = key;
            command.Parameters.Add(p1);
            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                container.Data = reader.GetString(0);
            }

            CloseConnection();
            return container;
        }
        
        public AuthContainer<object> SetImage(string key, byte[] imgBytes, AuthKey authkey)
        {
            AuthContainer<object> container = CheckAuthorization<object>(authkey);
            if (!container.Key.Success)
                return container;

            bool exists = false;

            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                @"SELECT * FROM MEDIA WHERE KEY = $KEY AND MEDIATYPE = ""Image""";
            command.Parameters.Add(new SqliteParameter("$KEY", key));
            SqliteDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                exists = true;
            }
            reader.Close();

            if (exists)
                command.CommandText = @"UPDATE MEDIA SET MEDIATYPE = ""Image"", VALUE = $VALUE WHERE KEY = $KEY";
            else
                command.CommandText = @"INSERT INTO MEDIA (KEY, MEDIATYPE, VALUE) VALUES ($KEY, ""Image"", $VALUE)";

            command.Parameters.Clear();
            command.Parameters.Add(new SqliteParameter("$KEY", key));
            command.Parameters.Add(new SqliteParameter("$VALUE", imgBytes));

            command.ExecuteNonQuery();

            CloseConnection();
            return container;
        }
        public AuthContainer<byte[]> RetrieveImage(string key, AuthKey authkey)
        {
            AuthContainer<byte[]> container = CheckAuthorization<byte[]>(authkey);
            if (!container.Key.Success)
                return container;

            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                @"SELECT VALUE FROM MEDIA WHERE KEY = $KEY AND MEDIATYPE = ""Image""";
            command.Parameters.Add(new SqliteParameter("$KEY", key));
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                container.Message = "No value found for key " + key + ".";
                container.Data = null;
                return container;
            }

            byte[] imgBytes = new byte[1048576];
            while (reader.Read())
            {
                reader.GetBytes(0, 0, imgBytes, 0, 1048576);
            }
            container.Data = imgBytes;

            CloseConnection();
            return container;
        }
        public byte[] RetrieveCompanyLogo()
        {
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                @"SELECT VALUE FROM MEDIA WHERE KEY = ""Company Logo"" AND MEDIATYPE = ""Image""";
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                CloseConnection();
                return null;
            }

            byte[] imgBytes = new byte[1048576];
            while (reader.Read())
            {
                reader.GetBytes(0, 0, imgBytes, 0, 1048576);
            }

            CloseConnection();
            return imgBytes;
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
                List<Item> itemMatches = CheckItemNumber(d1, true, BypassKey).Data;
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
                barcodeData = scannedBarcode.Substring(1);
            else
                barcodeData = scannedBarcode;

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
                List<Item> itemMatches = CheckItemNumber(d1, true, BypassKey).Data;
                Item match = itemMatches.Find(el => el.productLine.ToLower() == d2.ToLower());
                if (match != null)
                    item = new InvoiceItem(itemMatches.Find(el => el.productLine.ToLower() == d2.ToLower()));
                else
                {
                    item = new InvoiceItem() { invalid = true, itemNumber = d1, productLine = d2, quantity = d3 };
                }
                item.quantity = d3;
            }

            CloseConnection();
            return item;
        }
        public AuthContainer<object> UpdateBarcode(string barcode, InvoiceItem data, AuthKey key)
        {
            AuthContainer<object> container = CheckAuthorization<object>(key);
            if (!container.Key.Success)
                return container;

            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
              @"UPDATE BARCODES SET 
                SCANNEDITEMNUMBER =     $ITEMNO, 
                SCANNEDPRODUCTLINE =    $PRODUCTLINE, 
                SCANNEDQUANTITY =       $QTY 
              WHERE BARCODEVALUE = $BARCODE";
            command.Parameters.Add(new SqliteParameter("$ITEMNO", data.itemNumber));
            command.Parameters.Add(new SqliteParameter("$PRODUCTLINE", data.productLine));
            command.Parameters.Add(new SqliteParameter("$QTY", data.quantity));
            command.Parameters.Add(new SqliteParameter("$BARCODE", barcode));
            command.ExecuteNonQuery();

            CloseConnection();
            return container;
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
                    if (!reader.IsDBNull(i))
                        data.Add(reader.GetValue(i));
                    else
                        data.Add(String.Empty);
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

            foreach (Account acct in accounts)
            {
                acct.Transactions = RetrieveAccountTransactions(acct.ID);
                acct.Transactions.Sort((d1, d2) => DateTime.Compare(d1.date, d2.date));
                foreach (Transaction t in acct.Transactions)
                {
                    if (acct.ID == t.creditAccount)
                    {
                        switch (acct.Type)
                        {
                            case Account.AccountTypes.Asset:
                            case Account.AccountTypes.Expense:
                                acct.Balance -= t.amount;
                                break;

                            case Account.AccountTypes.Equity:
                            case Account.AccountTypes.Liability:
                            case Account.AccountTypes.Income:
                                acct.Balance += t.amount;
                                break;
                        }
                    }
                    if (acct.ID == t.debitAccount)
                    {
                        switch (acct.Type)
                        {
                            case Account.AccountTypes.Asset:
                            case Account.AccountTypes.Expense:
                                acct.Balance += t.amount;
                                break;

                            case Account.AccountTypes.Equity:
                            case Account.AccountTypes.Liability:
                            case Account.AccountTypes.Income:
                                acct.Balance -= t.amount;
                                break;
                        }
                    }
                }
            }
            return accounts;
        }
        private List<Transaction> RetrieveAccountTransactions(int accountID)
        {
            List<Transaction> transactions = new List<Transaction>();
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
                    referenceNumber = reader.GetInt32(7),
                    _void = reader.GetBoolean(8),
                });
            }

            CloseConnection();
            return transactions;
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
                    referenceNumber = reader.GetInt32(7),
                    _void = reader.GetBoolean(8),
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
                "INSERT INTO ACCOUNTTRANSACTIONS (TRANSACTIONID, DATE, MEMO, CREDITACCOUNT, DEBITACCOUNT, AMOUNT, REFERENCENUMBER, VOID)" +
                "VALUES ($TID, $DATE, $MEMO, $CA, $DA, $AMT, $REF, $VOID)";
            command.Parameters.Add(new SqliteParameter("$TID", tID));
            command.Parameters.Add(new SqliteParameter("$DATE", t.date.ToString("MM/dd/yyyy h:mm tt")));
            command.Parameters.Add(new SqliteParameter("$MEMO", t.memo));
            command.Parameters.Add(new SqliteParameter("$CA", t.creditAccount));
            command.Parameters.Add(new SqliteParameter("$DA", t.debitAccount));
            command.Parameters.Add(new SqliteParameter("$AMT", t.amount));
            command.Parameters.Add(new SqliteParameter("$REF", t.referenceNumber));
            command.Parameters.Add(new SqliteParameter("$VOID", t._void));
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
        public AuthContainer<object> VoidTransaction(int TransactionID, AuthKey key)
        {
            AuthContainer<object> container = CheckAuthorization<object>(key);
            if (!container.Key.Success)
                return container;

            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText = "UPDATE ACCOUNTTRANSACTIONS SET (VOID = 1) WHERE (TRANSACTIONID = $TID)";
            command.Parameters.Add(new SqliteParameter("$TID", TransactionID));
            command.ExecuteNonQuery();

            CloseConnection();
            return container;
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
        public List<Device> RetrieveTerminals()
        {
            List<Device> devices = RetrieveDevices();
            List<Device> terms = new List<Device>();
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT NICKNAME, IPADDRESS, ID FROM DEVICES WHERE DEVICETYPE = 'TERMINAL'";
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Device term = new Device();
                term.Type = Device.DeviceType.Terminal;
                term.Nickname = reader.GetString(0);
                term.address = new IPEndPoint(IPAddress.Parse(reader.GetString(1)), 0);
                term.ID = reader.GetInt32(2);
                terms.Add(term);
            }
            reader.Close();

            foreach (Device term in terms)
            {
                command.Parameters.Clear();
                command.CommandText =
                    "SELECT DEVICEID FROM DEVICEASSIGNMENTS WHERE TERMINALID = $TERM";
                command.Parameters.Add(new SqliteParameter("$TERM", term.ID));
                reader = command.ExecuteReader();
                while (reader.Read())
                    term.AssignedDevices.Add(devices.First(el => el.ID == reader.GetInt32(0)));

                reader.Close();
            }

            CloseConnection();
            return terms;
        }
        public List<Device> RetrieveDevices()
        {
            List<Device> devices = new List<Device>();
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT NICKNAME, IPADDRESS, DEVICETYPE, ID FROM DEVICES WHERE DEVICETYPE != 'TERMINAL'";
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Device device = new Device();
                device.Nickname = reader.GetString(0);
                string[] split = reader.GetString(1).Split(':');
                device.address = new IPEndPoint(IPAddress.Parse(split[0]), int.Parse(split[1]));
                switch (reader.GetString(2).ToUpper())
                {
                    case ("THERMALPRINTER"):
                        device.Type = Device.DeviceType.ThermalPrinter;
                        break;
                    case ("PRINTER"):
                        device.Type = Device.DeviceType.ConventionalPrinter;
                        break;
                    case ("LINEDISPLAY"):
                        device.Type = Device.DeviceType.LineDisplay;
                        break;
                    case ("CARDREADER"):
                        device.Type = Device.DeviceType.CardReader;
                        break;
                    default:
                        device.Type = Device.DeviceType.Other;
                        break;
                }
                device.ID = reader.GetInt32(3);
                devices.Add(device);
            }

            CloseConnection();
            return devices;
        }
        public bool RegisterDevice(Device device)
        {
            if (DeviceExists(device.address.ToString()))
                return false;

            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "INSERT INTO DEVICES (DEVICETYPE, IPADDRESS, NICKNAME) VALUES ($TYPE, $ADDR, $NAME)";
            command.Parameters.Add(new SqliteParameter("$TYPE", Enum.GetName(typeof (Device.DeviceType), device.Type)));
            command.Parameters.Add(new SqliteParameter("$ADDR", device.address.ToString()));
            command.Parameters.Add(new SqliteParameter("$NAME", device.Nickname));
            if (command.ExecuteNonQuery() < 1)
            {
                CloseConnection();
                return false;
            }

            CloseConnection();

            if (device.Type == Device.DeviceType.ThermalPrinter)
                ReceiptPrinter.AddPrinter(device);

            return true;
        }
        public bool DeleteDevice(Device device)
        {
            if (!DeviceExists(device.address.ToString()))
                return false;

            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "DELETE FROM DEVICES WHERE NICKNAME = $NAME";
            command.Parameters.Add(new SqliteParameter("$NAME", device.Nickname));
            if (command.ExecuteNonQuery() < 1)
            {
                CloseConnection();
                return false;
            }

            CloseConnection();

            if (device.Type == Device.DeviceType.ThermalPrinter)
                ReceiptPrinter.RemovePrinter(device);
            return true;
        }
        
        public bool AssignDevice(Device terminal, Device device)
        {
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "INSERT INTO DEVICEASSIGNMENTS (DEVICEID, TERMINALID) VALUES ($DEVICE, $TERM)";
            command.Parameters.Add(new SqliteParameter("$DEVICE", device.ID));
            command.Parameters.Add(new SqliteParameter("$TERM", terminal.ID));
            if (command.ExecuteNonQuery() < 1)
            {
                CloseConnection();
                return false;
            }

            CloseConnection();
            return true;
        }
        public bool RemoveDeviceAssignment(Device terminal, Device device)
        {
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "DELETE FROM DEVICEASSIGNMENTS WHERE TERMINALID = $TERM AND DEVICEID = $DEVICE";
            command.Parameters.Add(new SqliteParameter("$TERM", terminal.ID));
            command.Parameters.Add(new SqliteParameter("$DEVICE", device.ID));
            if (command.ExecuteNonQuery() < 1)
            {
                CloseConnection();
                return false;
            }

            CloseConnection();
            return true;
        }

        public AuthContainer<string> TestIngenicoRequest(IngenicoRequest request, AuthKey key)
        {
            AuthContainer<string> container = CheckAuthorization<string>(key);
            if (!container.Key.Success)
                return container;

            IPAddress ip = IPAddress.Parse(GetClientAddress());
            List<Device> terminals = RetrieveTerminals();
            Device terminal = terminals.First(el => el.address.Address.Equals(ip));
            try
            {
                Device device = terminal.AssignedDevices.FirstOrDefault(el => el.Type == Device.DeviceType.CardReader);
                if (device == default(Device))
                {
                    container.Data = "NO ATTACHED DEVICE";
                    return container;
                }
                container.Data = Payments.Engine.SendRawRequest(device, request);
                return container;
            }
            catch
            {
                container.Data = "INVALID";
                return container;
            }
        }
        public AuthContainer<Payment> InitiatePayment(decimal paymentAmount, AuthKey key)
        {
            AuthContainer<Payment> container = CheckAuthorization<Payment>(key);
            if (!container.Key.Success)
                return container;

            IPAddress ip = IPAddress.Parse(GetClientAddress());
            List<Device> terminals = RetrieveTerminals();
            Device terminal = terminals.First(el => el.address.Address.Equals(ip));
            try
            {
                Device device = terminal.AssignedDevices.FirstOrDefault(el => el.Type == Device.DeviceType.CardReader);
                if (device == default(Device))
                {
                    container.Data = new Payment() { errorMessage = Payment.CardReaderErrorMessages.NoAttachedDevice };
                    return container;
                }
                container.Data = Payments.Engine.InitiatePayment(device, paymentAmount, 0, 0, 0);
                return container;
            }
            catch
            {
                container.Data = new Payment();
                container.Data.errorMessage = Payment.CardReaderErrorMessages.NoAttachedDevice;
                return container;
            }
        }
        public AuthContainer<Payment> InitiateRefund(decimal refundAmount, AuthKey key)
        {
            AuthContainer<Payment> container = CheckAuthorization<Payment>(key);
            if (!container.Key.Success)
                return container;

            IPAddress ip = IPAddress.Parse(GetClientAddress());
            List<Device> terminals = RetrieveTerminals();
            Device terminal = terminals.First(el => el.address.Address.Equals(ip));
            try
            {
                Device device = terminal.AssignedDevices.FirstOrDefault(el => el.Type == Device.DeviceType.CardReader);
                if (device == default(Device))
                {
                    container.Data = new Payment() { errorMessage = Payment.CardReaderErrorMessages.NoAttachedDevice };
                    return container;
                }
                container.Data = Payments.Engine.InitiateRefund(device, refundAmount);
                return container;
            }
            catch (TimeoutException)
            {
                container.Data = new Payment();
                container.Data.errorMessage = Payment.CardReaderErrorMessages.Timeout;
                return container;
            }
        }
        public AuthContainer<string> RequestSignature(AuthKey key)
        {
            AuthContainer<string> container = CheckAuthorization<string>(key);
            if (!container.Key.Success)
                return container;

            IPAddress ip = IPAddress.Parse(GetClientAddress());
            List<Device> terminals = RetrieveTerminals();
            Device terminal = terminals.First(el => el.address.Address.Equals(ip));
            try
            {
                Device device = terminal.AssignedDevices.FirstOrDefault(el => el.Type == Device.DeviceType.CardReader);
                if (device == default(Device))
                {
                    container.Data = "NO ATTACHED DEVICE";
                    return container;
                }
                container.Data = Payments.Engine.InitiateSignatureCapture(device);
                return container;
            }
            catch
            {
                container.Data = "INVALID";
                return container;
            }
        }
        
        public AuthContainer<object> PrintReceipt(Invoice inv, AuthKey key)
        {
            AuthContainer<object> container = CheckAuthorization<object>(key);
            if (!container.Key.Success)
                return container;

            IPAddress ip = IPAddress.Parse(GetClientAddress());
            List<Device> terminals = RetrieveTerminals();
            Device terminal = terminals.FirstOrDefault(el => el.address.Address.Equals(ip));
            try
            {
                Device device = terminal.AssignedDevices.FirstOrDefault(el => el.Type == Device.DeviceType.ThermalPrinter);
                if (device == default(Device))
                {
                    container.Data = "NO ATTACHED DEVICE";
                    return container;
                }
                ReceiptPrinter.PrintReceipt(inv, device);
                return container;
            }
            catch
            {
                container.Data = "INVALID";
                return container;
            }
        }
        #endregion


    }
}
