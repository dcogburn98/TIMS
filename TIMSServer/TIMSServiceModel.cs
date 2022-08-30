﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data.SQLite;

using TIMSServerModel;

namespace TIMSServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EmployeeModel" in both code and config file together.
    public class TIMSServiceModel : ITIMSServiceModel
    {
        private static SQLiteConnection sqlite_conn = Program.sqlite_conn;

        private static void OpenConnection()
        {
            Program.OpenConnection();
        }

        private static void CloseConnection()
        {
            Program.CloseConnection();
        }

        #region Employees
        
        public string CheckEmployee(string input)
        {
            if (!int.TryParse(input, out int v))
                input = "'" + input + "'";
            string value = null;
            Program.OpenConnection();
            SQLiteCommand command = sqlite_conn.CreateCommand();

            command.CommandText =
                "SELECT FULLNAME " +
                "FROM EMPLOYEES " +
                "WHERE USERNAME = " + input + " " +
                "OR EMPLOYEENUMBER = " + input;

            SQLiteDataReader rdr = command.ExecuteReader();
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
            Console.WriteLine("Login Called");
            //System.Threading.Thread.Sleep(1000); Uncomment before release
            Employee e = new Employee();

            if (!int.TryParse(user, out int v))
                user = "'" + user + "'";
            Program.OpenConnection();

            SQLiteCommand sqlite_cmd;
            sqlite_cmd = Program.sqlite_conn.CreateCommand();

            sqlite_cmd.CommandText =
                "SELECT EMPLOYEENUMBER, FULLNAME, USERNAME, POSITION, BIRTHDATE, HIREDATE, TERMINATIONDATE, PERMISSIONS, STARTUPSCREEN, COMMISSIONED, COMMISSIONRATE, WAGED, HOURLYWAGE, PAYPERIOD, PASSWORDHASH" + " " +
                "FROM EMPLOYEES" + " " +
                "WHERE (USERNAME = " + user + " " +
                "OR EMPLOYEENUMBER = " + user + ") " +
                "AND PASSWORDHASH = $pass";

            SQLiteParameter hash = new SQLiteParameter("$pass", System.Data.DbType.Binary)
            {
                Value = pass
            };
            sqlite_cmd.Parameters.Add(hash);

            SQLiteDataReader rdr = sqlite_cmd.ExecuteReader(System.Data.CommandBehavior.Default);
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
            return e;
        }
        public Employee RetrieveEmployee(string employeeNumber)
        {
            Employee e = new Employee();
            Program.OpenConnection();

            SQLiteCommand command = Program.sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT * FROM EMPLOYEES WHERE EMPLOYEENUMBER = $NUMBER";
            SQLiteParameter p1 = new SQLiteParameter("$NUMBER", employeeNumber);
            command.Parameters.Add(p1);
            SQLiteDataReader reader = command.ExecuteReader();

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
        
        public List<Item> CheckItemNumber(string itemNumber, bool connectionOpened)
        {
            string fixedIN = string.Empty;
            foreach (char c in itemNumber)
            {
                if (char.IsLetterOrDigit(c))
                    fixedIN += c;
            }
            fixedIN = fixedIN.ToUpper();

            if (!connectionOpened)
                Program.OpenConnection();

            SQLiteCommand sqlite_cmd;
            sqlite_cmd = Program.sqlite_conn.CreateCommand();

            sqlite_cmd.CommandText =
                "SELECT *" + " " +
                "FROM ITEMS" + " " +
                "WHERE ITEMNUMBER LIKE $ITEM";

            SQLiteParameter itemParam = new SQLiteParameter("$ITEM", fixedIN[0] + "%" + fixedIN[fixedIN.Length - 1]);
            sqlite_cmd.Parameters.Add(itemParam);
            SQLiteDataReader reader = sqlite_cmd.ExecuteReader();

            List<Item> invItems = new List<Item>();

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
        public List<Item> CheckItemNumberFromSupplier(string itemNumber, string supplier)
        {
            string fixedIN = string.Empty;
            foreach (char c in itemNumber)
            {
                if (char.IsLetterOrDigit(c))
                    fixedIN += c;
            }
            fixedIN = fixedIN.ToUpper();

            Program.OpenConnection();
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = Program.sqlite_conn.CreateCommand();

            sqlite_cmd.CommandText =
                "SELECT *" + " " +
                "FROM ITEMS" + " " +
                "WHERE ITEMNUMBER LIKE $ITEM AND SUPPLIER == $SUPPLIER";

            SQLiteParameter itemParam = new SQLiteParameter("$ITEM", fixedIN[0] + "%" + fixedIN[fixedIN.Length - 1]);
            SQLiteParameter supplierParam = new SQLiteParameter("$SUPPLIER", supplier);
            sqlite_cmd.Parameters.Add(itemParam);
            sqlite_cmd.Parameters.Add(supplierParam);
            SQLiteDataReader reader = sqlite_cmd.ExecuteReader();

            List<Item> invItems = new List<Item>();

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
        public List<Item> RetrieveItemsFromSupplier(string supplier)
        {
            Program.OpenConnection();
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = Program.sqlite_conn.CreateCommand();

            sqlite_cmd.CommandText =
                "SELECT *" + " " +
                "FROM ITEMS" + " " +
                "WHERE SUPPLIER == $SUPPLIER";

            SQLiteParameter supplierParam = new SQLiteParameter("$SUPPLIER", supplier);
            sqlite_cmd.Parameters.Add(supplierParam);
            SQLiteDataReader reader = sqlite_cmd.ExecuteReader();

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

            Program.CloseConnection();
            if (invItems.Count == 0)
                return null;
            else
                return invItems;
        }
        public List<Item> RetrieveItemsFromSupplierBelowMin(string supplier)
        {
            OpenConnection();
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            sqlite_cmd.CommandText =
                "SELECT *" + " " +
                "FROM ITEMS" + " " +
                "WHERE SUPPLIER == $SUPPLIER AND ONHANDQUANTITY < MINIMUM";

            SQLiteParameter supplierParam = new SQLiteParameter("$SUPPLIER", supplier);
            sqlite_cmd.Parameters.Add(supplierParam);
            SQLiteDataReader reader = sqlite_cmd.ExecuteReader();

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
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            sqlite_cmd.CommandText =
                "SELECT *" + " " +
                "FROM ITEMS" + " " +
                "WHERE SUPPLIER == $SUPPLIER AND ONHANDQUANTITY < MAXIMUM";

            SQLiteParameter supplierParam = new SQLiteParameter("$SUPPLIER", supplier);
            sqlite_cmd.Parameters.Add(supplierParam);
            SQLiteDataReader reader = sqlite_cmd.ExecuteReader();

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
            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT LASTORDERDATE FROM SUPPLIERS WHERE SUPPLIER = $SUPPLIER";
            SQLiteParameter p1 = new SQLiteParameter("$SUPPLIER", supplier);
            command.Parameters.Add(p1);
            SQLiteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
                lastOrderDate = DateTime.MinValue;
            else
                while (reader.Read())
                    lastOrderDate = DateTime.Parse(reader.GetString(0));

            CloseConnection();

            supplierItems = RetrieveItemsFromSupplier(supplier);
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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT SUPPLIER FROM SUPPLIERS";
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
                suppliers.Add(reader.GetString(0));

            suppliers.Sort();

            CloseConnection();
            return suppliers;
        }
        public void AddSupplier(string supplier)
        {
            OpenConnection();

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "INSERT INTO SUPPLIERS (SUPPLIER) VALUES ($SUPPLIER)";
            SQLiteParameter p1 = new SQLiteParameter("$SUPPLIER", supplier);
            command.Parameters.Add(p1);
            command.ExecuteNonQuery();

            CloseConnection();
        }
        public bool CheckProductLine(string productLine)
        {
            OpenConnection();

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT * FROM PRODUCTLINES WHERE PRODUCTLINE = $LINE";
            SQLiteParameter p1 = new SQLiteParameter("$LINE", productLine);
            command.Parameters.Add(p1);
            SQLiteDataReader reader = command.ExecuteReader();

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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "INSERT INTO PRODUCTLINES (PRODUCTLINE) VALUES ($LINE)";
            SQLiteParameter p1 = new SQLiteParameter("$LINE", productLine);
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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT SCANNEDITEMNUMBER,SCANNEDPRODUCTLINE,SCANNEDQUANTITY FROM BARCODES " +
                "WHERE (BARCODETYPE = $TYPE AND BARCODEVALUE = $VALUE)";
            SQLiteParameter p1 = new SQLiteParameter("$TYPE", barcodeType);
            SQLiteParameter p2 = new SQLiteParameter("$VALUE", barcodeData);
            command.Parameters.Add(p1);
            command.Parameters.Add(p2);

            SQLiteDataReader reader = command.ExecuteReader();

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
                List<Item> itemMatches = CheckItemNumber(d1, true);
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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT SCANNEDITEMNUMBER,SCANNEDPRODUCTLINE,SCANNEDQUANTITY FROM BARCODES " +
                "WHERE (BARCODETYPE = $TYPE AND BARCODEVALUE = $VALUE)";
            SQLiteParameter p1 = new SQLiteParameter("$TYPE", barcodeType);
            SQLiteParameter p2 = new SQLiteParameter("$VALUE", barcodeData);
            command.Parameters.Add(p1);
            command.Parameters.Add(p2);

            SQLiteDataReader reader = command.ExecuteReader();

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
                List<Item> itemMatches = CheckItemNumber(d1, true);
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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT * FROM ITEMS WHERE (ITEMNUMBER LIKE $ITEMNO AND PRODUCTLINE = $LINE)";
            SQLiteParameter p1 = new SQLiteParameter("$ITEMNO", fixedIN[0] + "%" + fixedIN[fixedIN.Length - 1]);
            SQLiteParameter p2 = new SQLiteParameter("$LINE", productLine);
            command.Parameters.Add(p1);
            command.Parameters.Add(p2);
            SQLiteDataReader reader = command.ExecuteReader();
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

            SQLiteCommand command = sqlite_conn.CreateCommand();
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

            SQLiteParameter p1 = new SQLiteParameter("$ITEMNAME", newItem.itemName);
            SQLiteParameter p2 = new SQLiteParameter("$LONGDESCRIPTION", newItem.longDescription);
            SQLiteParameter p3 = new SQLiteParameter("$SUPPLIER", newItem.supplier);
            SQLiteParameter p4 = new SQLiteParameter("$GROUPCODE", newItem.groupCode);
            SQLiteParameter p5 = new SQLiteParameter("$VELOCITYCODE", newItem.velocityCode);
            SQLiteParameter p6 = new SQLiteParameter("$PREVIOUSVELOCITYCODE", newItem.previousYearVelocityCode);
            SQLiteParameter p7 = new SQLiteParameter("$ITEMSPERCONTAINER", newItem.itemsPerContainer);
            SQLiteParameter p8 = new SQLiteParameter("$STANDARDPACKAGE", newItem.standardPackage);
            SQLiteParameter p9 = new SQLiteParameter("$DATESTOCKED", newItem.dateStocked.ToString());
            SQLiteParameter p10 = new SQLiteParameter("$DATELASTRECEIPT", newItem.dateLastReceipt.ToString());
            SQLiteParameter p11 = new SQLiteParameter("$MIN", newItem.minimum);
            SQLiteParameter p12 = new SQLiteParameter("$MAX", newItem.maximum);
            SQLiteParameter p13 = new SQLiteParameter("$ONHANDQTY", newItem.onHandQty);
            SQLiteParameter p14 = new SQLiteParameter("$WIPQTY", newItem.WIPQty);
            SQLiteParameter p15 = new SQLiteParameter("$ONORDERQTY", newItem.onOrderQty);
            SQLiteParameter p16 = new SQLiteParameter("$BACKORDERQTY", newItem.onBackorderQty);
            SQLiteParameter p17 = new SQLiteParameter("$DAYSONORDER", newItem.daysOnOrder);
            SQLiteParameter p18 = new SQLiteParameter("$DAYSONBACKORDER", newItem.daysOnBackorder);
            SQLiteParameter p19 = new SQLiteParameter("$LIST", newItem.listPrice);
            SQLiteParameter p20 = new SQLiteParameter("$RED", newItem.redPrice);
            SQLiteParameter p21 = new SQLiteParameter("$YELLOW", newItem.yellowPrice);
            SQLiteParameter p22 = new SQLiteParameter("$GREEN", newItem.greenPrice);
            SQLiteParameter p23 = new SQLiteParameter("$PINK", newItem.pinkPrice);
            SQLiteParameter p24 = new SQLiteParameter("$BLUE", newItem.bluePrice);
            SQLiteParameter p25 = new SQLiteParameter("$COST", newItem.replacementCost);
            SQLiteParameter p26 = new SQLiteParameter("$AVERAGECOST", newItem.averageCost);
            SQLiteParameter p27 = new SQLiteParameter("$TAXED", newItem.taxed);
            SQLiteParameter p28 = new SQLiteParameter("$RESTRICTED", newItem.ageRestricted);
            SQLiteParameter p29 = new SQLiteParameter("$MINAGE", newItem.minimumAge);
            SQLiteParameter p30 = new SQLiteParameter("$LOCATION", newItem.locationCode);
            SQLiteParameter p31 = new SQLiteParameter("$ITEMNUMBER", newItem.itemNumber);
            SQLiteParameter p32 = new SQLiteParameter("$PRODUCTLINE", newItem.productLine);
            SQLiteParameter p33 = new SQLiteParameter("$SERIALIZED", newItem.serialized);
            SQLiteParameter p34 = new SQLiteParameter("$CATEGORY", newItem.category);
            SQLiteParameter p35 = new SQLiteParameter("$SKU", newItem.SKU);

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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT SERIALNUMBER FROM SERIALNUMBERS WHERE ITEMNUMBER = $ITEM AND PRODUCTLINE = $LINE";
            SQLiteParameter p1 = new SQLiteParameter("$ITEM", itemNumber);
            SQLiteParameter p2 = new SQLiteParameter("$LINE", productLine);
            command.Parameters.Add(p1);
            command.Parameters.Add(p2);
            SQLiteDataReader reader = command.ExecuteReader();
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

            SQLiteCommand command = sqlite_conn.CreateCommand();
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

            SQLiteParameter p1 = new SQLiteParameter("$PRODUCTLINE", item.productLine);
            SQLiteParameter p2 = new SQLiteParameter("$ITEMNUMBER", item.itemNumber);
            SQLiteParameter p3 = new SQLiteParameter("$ITEMNAME", item.itemName);
            SQLiteParameter p4 = new SQLiteParameter("$DESCRIPTION", item.longDescription);
            SQLiteParameter p5 = new SQLiteParameter("$SUPPLIER", item.supplier);
            SQLiteParameter p6 = new SQLiteParameter("$GROUP", item.groupCode);
            SQLiteParameter p7 = new SQLiteParameter("$VELOCITY", item.velocityCode);
            SQLiteParameter p8 = new SQLiteParameter("$PREVIOUSVELOCITY", item.previousYearVelocityCode);
            SQLiteParameter p9 = new SQLiteParameter("$ITEMSPERCONTAINER", item.itemsPerContainer);
            SQLiteParameter p10 = new SQLiteParameter("$STDPKG", item.standardPackage);
            SQLiteParameter p11 = new SQLiteParameter("$DATESTOCKED", item.dateStocked.ToString());
            SQLiteParameter p12 = new SQLiteParameter("$DATELASTRECEIPT", item.dateLastReceipt.ToString());
            SQLiteParameter p13 = new SQLiteParameter("$MIN", item.minimum);
            SQLiteParameter p14 = new SQLiteParameter("$MAX", item.maximum);
            SQLiteParameter p15 = new SQLiteParameter("$ONHAND", item.onHandQty);
            SQLiteParameter p16 = new SQLiteParameter("$WIPQUANTITY", item.WIPQty);
            SQLiteParameter p17 = new SQLiteParameter("$ONORDERQTY", item.onOrderQty);
            SQLiteParameter p18 = new SQLiteParameter("$BACKORDERQTY", item.onBackorderQty);
            SQLiteParameter p19 = new SQLiteParameter("$DAYSONORDER", item.daysOnOrder);
            SQLiteParameter p20 = new SQLiteParameter("$DAYSONBACKORDER", item.daysOnBackorder);
            SQLiteParameter p21 = new SQLiteParameter("$LIST", item.listPrice);
            SQLiteParameter p22 = new SQLiteParameter("$RED", item.redPrice);
            SQLiteParameter p23 = new SQLiteParameter("$YELLOW", item.yellowPrice);
            SQLiteParameter p24 = new SQLiteParameter("$GREEN", item.greenPrice);
            SQLiteParameter p25 = new SQLiteParameter("$PINK", item.pinkPrice);
            SQLiteParameter p26 = new SQLiteParameter("$BLUE", item.bluePrice);
            SQLiteParameter p27 = new SQLiteParameter("$COST", item.replacementCost);
            SQLiteParameter p28 = new SQLiteParameter("$AVERAGECOST", item.averageCost);
            SQLiteParameter p29 = new SQLiteParameter("$TAXED", item.taxed);
            SQLiteParameter p30 = new SQLiteParameter("$AGERESTRICTED", item.ageRestricted);
            SQLiteParameter p31 = new SQLiteParameter("$MINAGE", item.minimumAge);
            SQLiteParameter p32 = new SQLiteParameter("$LOCATION", item.locationCode);
            SQLiteParameter p33 = new SQLiteParameter("$SERIALIZED", item.serialized);
            SQLiteParameter p34 = new SQLiteParameter("$CATEGORY", item.category);
            SQLiteParameter p35 = new SQLiteParameter("$SKU", item.SKU);

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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT PRODUCTLINE, ITEMNUMBER FROM ITEMS WHERE LASTLABELPRICE != GREENPRICE";
            SQLiteDataReader reader = command.ExecuteReader();
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
        public void SaveReleasedInvoice(Invoice inv)
        {
            OpenConnection();
            SQLiteCommand command = sqlite_conn.CreateCommand();

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

            SQLiteParameter p1 = new SQLiteParameter("$INVOICENUMBER", inv.invoiceNumber);
            SQLiteParameter p2 = new SQLiteParameter("$SUBTOTAL", inv.subtotal);
            SQLiteParameter p3 = new SQLiteParameter("$TAXABLETOTAL", inv.taxableTotal);
            SQLiteParameter p4 = new SQLiteParameter("$TAXRATE", inv.taxRate);
            SQLiteParameter p5 = new SQLiteParameter("$TAXAMOUNT", inv.taxAmount);
            SQLiteParameter p6 = new SQLiteParameter("$TOTAL", inv.total);
            SQLiteParameter p7 = new SQLiteParameter("$TOTALPAYMENTS", inv.totalPayments);
            SQLiteParameter p8 = new SQLiteParameter("$AGERESTRICTED", inv.containsAgeRestrictedItem);
            SQLiteParameter p9 = new SQLiteParameter("$CUSTOMERBIRTHDATE", inv.customerBirthdate.ToString());
            SQLiteParameter p10 = new SQLiteParameter("$ATTENTION", inv.attentionLine);
            SQLiteParameter p11 = new SQLiteParameter("$PO", inv.PONumber);
            SQLiteParameter p12 = new SQLiteParameter("$MESSAGE", inv.invoiceMessage);
            SQLiteParameter p13 = new SQLiteParameter("$SAVEDINVOICE", inv.savedInvoice);
            SQLiteParameter p14 = new SQLiteParameter("$SAVEDINVOICETIME", inv.savedInvoiceTime.ToString());
            SQLiteParameter p15 = new SQLiteParameter("$INVOICECREATIONTIME", inv.invoiceCreationTime.ToString());
            SQLiteParameter p16 = new SQLiteParameter("$INVOICEFINALIZEDTIME", inv.invoiceFinalizedTime.ToString());
            SQLiteParameter p17 = new SQLiteParameter("$FINALIZED", inv.finalized);
            SQLiteParameter p18 = new SQLiteParameter("$VOIDED", inv.voided);
            SQLiteParameter p19 = new SQLiteParameter("$CUSTOMERNUMBER", inv.customer.customerNumber);
            SQLiteParameter p20 = new SQLiteParameter("$EMPLOYEENUMBER", inv.employee.employeeNumber);
            SQLiteParameter p21 = new SQLiteParameter("$COST", inv.cost);
            SQLiteParameter p22 = new SQLiteParameter("$PROFIT", inv.profit);


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

                SQLiteParameter pp1 = new SQLiteParameter("$INVOICENUMBER", inv.invoiceNumber);
                SQLiteParameter pp2 = new SQLiteParameter("$ITEMNUMBER", item.itemNumber);
                SQLiteParameter pp3 = new SQLiteParameter("$PRODUCTLINE", item.productLine);
                SQLiteParameter pp4 = new SQLiteParameter("$ITEMDESCRIPTION", item.itemName);
                SQLiteParameter pp5 = new SQLiteParameter("$PRICE", item.price);
                SQLiteParameter pp6 = new SQLiteParameter("$LISTPRICE", item.listPrice);
                SQLiteParameter pp7 = new SQLiteParameter("$QUANTITY", item.quantity);
                SQLiteParameter pp8 = new SQLiteParameter("$TOTAL", item.total);
                SQLiteParameter pp9 = new SQLiteParameter("$PRICECODE", item.pricingCode);
                SQLiteParameter pp10 = new SQLiteParameter("$SERIALIZED", item.serializedItem);
                SQLiteParameter pp11 = new SQLiteParameter("$SERIALNUMBER", item.serialNumber);
                SQLiteParameter pp12 = new SQLiteParameter("$AGERESTRICTED", item.ageRestricted);
                SQLiteParameter pp13 = new SQLiteParameter("$MINIMUMAGE", item.minimumAge);
                SQLiteParameter pp14 = new SQLiteParameter("$TAXED", item.taxed);
                string invCodes = string.Empty;
                if (item.codes != null)
                    foreach (string code in item.codes)
                        invCodes += code + ",";
                invCodes = invCodes.Trim(',');
                SQLiteParameter pp15 = new SQLiteParameter("$INVOICECODES", invCodes);
                SQLiteParameter pp16 = new SQLiteParameter("$GUID", item.ID);
                SQLiteParameter pp17 = new SQLiteParameter("$GUID", item.ID);

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

                SQLiteParameter ppp1 = new SQLiteParameter("$INVOICENUMBER", inv.invoiceNumber);
                SQLiteParameter ppp2 = new SQLiteParameter("$ID", pay.ID);
                SQLiteParameter ppp3 = new SQLiteParameter("$PAYMENTTYPE", pay.paymentType.ToString());
                SQLiteParameter ppp4 = new SQLiteParameter("$PAYMENTAMOUNT", pay.paymentAmount);

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
            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText = "SELECT MAX(INVOICENUMBER) FROM INVOICES";
            SQLiteDataReader reader = command.ExecuteReader();
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

        #region Customers

        public Customer CheckCustomerNumber(string custNo)
        {
            Customer cust = new Customer();
            OpenConnection();

            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            sqlite_cmd.CommandText =
                "SELECT *" + " " +
                "FROM CUSTOMERS" + " " +
                "WHERE CUSTOMERNUMBER = $CUSTNO";

            SQLiteParameter itemParam = new SQLiteParameter("$CUSTNO", custNo);
            sqlite_cmd.Parameters.Add(itemParam);
            SQLiteDataReader reader = sqlite_cmd.ExecuteReader();

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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT INVOICENUMBER FROM INVOICES WHERE INVOICEFINALIZEDTIME > $STARTTIME AND INVOICEFINALIZEDTIME < $ENDTIME";
            SQLiteParameter p1 = new SQLiteParameter("$STARTTIME", startDate.ToString());
            SQLiteParameter p2 = new SQLiteParameter("$ENDTIME", endDate.AddDays(1).ToString());
            command.Parameters.Add(p1);
            command.Parameters.Add(p2);
            SQLiteDataReader reader = command.ExecuteReader();
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

            SQLiteCommand command = sqlite_conn.CreateCommand();

            command.CommandText =
                "SELECT * FROM INVOICES WHERE INVOICENUMBER = $INVOICENUMBER";

            SQLiteParameter p1 = new SQLiteParameter("$INVOICENUMBER", invNumber);
            command.Parameters.Add(p1);
            SQLiteDataReader reader = command.ExecuteReader();
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

            inv.customer = CheckCustomerNumber(inv.customer.customerNumber);
            inv.employee = RetrieveEmployee(inv.employee.employeeNumber.ToString());
            return inv;
        }        
        public List<Invoice> RetrieveInvoicesByCriteria(string[] criteria)
        {
            List<Invoice> invoices = new List<Invoice>();
            OpenConnection();

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText = "SELECT INVOICENUMBER FROM INVOICES";
            SQLiteDataReader reader = command.ExecuteReader();

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
        
        #endregion

        #region Global Properties
        
        public string RetrievePropertyString(string key)
        {
            string property = String.Empty;
            OpenConnection();

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText = "SELECT VALUE FROM GLOBALPROPERTIES WHERE KEY = $KEY";
            SQLiteParameter p1 = new SQLiteParameter("$KEY");
            p1.Value = key;
            command.Parameters.Add(p1);
            SQLiteDataReader reader = command.ExecuteReader();

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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT * FROM SHORTCUTMENUS";
            SQLiteDataReader reader = command.ExecuteReader();
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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "INSERT INTO BARCODES ( BARCODETYPE, BARCODEVALUE, SCANNEDITEMNUMBER, SCANNEDPRODUCTLINE, SCANNEDQUANTITY) " +
                "VALUES ($TYPE, $VALUE, $ITEMNUMBER, $PRODUCTLINE, $QUANTITY)";
            SQLiteParameter p1 = new SQLiteParameter("$TYPE", "UPCA");
            SQLiteParameter p2 = new SQLiteParameter("$VALUE", barcode);
            SQLiteParameter p3 = new SQLiteParameter("$ITEMNUMBER", itemnumber);
            SQLiteParameter p4 = new SQLiteParameter("$PRODUCTLINE", productline);
            SQLiteParameter p5 = new SQLiteParameter("$QUANTITY", quantity);
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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT BARCODEVALUE FROM BARCODES WHERE SCANNEDITEMNUMBER = $ITEMNUMBER AND SCANNEDPRODUCTLINE = $LINE";
            SQLiteParameter p1 = new SQLiteParameter("$ITEMNUMBER", item.itemNumber);
            SQLiteParameter p2 = new SQLiteParameter("$LINE", item.productLine);
            command.Parameters.Add(p1);
            command.Parameters.Add(p2);
            SQLiteDataReader reader = command.ExecuteReader();
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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT NAME FROM sqlite_master WHERE TYPE = 'table'";
            SQLiteDataReader reader = command.ExecuteReader();

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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                $"PRAGMA table_info({table})";
            //SQLiteParameter p1 = new SQLiteParameter("$TABLE", table);
            //command.Parameters.Add(p1);
            SQLiteDataReader reader = command.ExecuteReader();

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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText = query;
            SQLiteDataReader reader = command.ExecuteReader();
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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "INSERT INTO REPORTS " +
                "(REPORTNAME,REPORTSHORTCODE,DATASOURCE,CONDITIONS,FIELDS,TOTALS) " +
                "VALUES ($REPORTNAME,$REPORTSHORTCODE,$DATASOURCE,$CONDITIONS,$FIELDS,$TOTALS)";

            SQLiteParameter p1 = new SQLiteParameter("$REPORTNAME", report.ReportName);
            SQLiteParameter p2 = new SQLiteParameter("$REPORTSHORTCODE", report.ReportShortcode);
            SQLiteParameter p3 = new SQLiteParameter("$DATASOURCE", report.DataSource);
            SQLiteParameter p4 = new SQLiteParameter("$CONDITIONS", conditions);
            SQLiteParameter p5 = new SQLiteParameter("$FIELDS", fields);
            SQLiteParameter p6 = new SQLiteParameter("$TOTALS", totals);

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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT * FROM REPORTS WHERE REPORTSHORTCODE = $CODE";
            SQLiteParameter p1 = new SQLiteParameter("$CODE", shortcode);
            command.Parameters.Add(p1);
            SQLiteDataReader reader = command.ExecuteReader();

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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT REPORTSHORTCODE, REPORTNAME FROM REPORTS";
            SQLiteDataReader reader = command.ExecuteReader();
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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT MAX(PONUMBER) FROM PURCHASEORDERS";
            SQLiteDataReader reader = command.ExecuteReader();
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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT PONUMBER FROM PURCHASEORDERS";
            SQLiteDataReader reader = command.ExecuteReader();
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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT * FROM PURCHASEORDERS WHERE PONUMBER = $PONUMBER";
            SQLiteParameter p1 = new SQLiteParameter("$PONUMBER", PONumber);
            command.Parameters.Add(p1);
            SQLiteDataReader reader = command.ExecuteReader();
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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT * FROM PURCHASEORDERS WHERE PONUMBER = $PO";
            SQLiteParameter p8 = new SQLiteParameter("$PO", PO.PONumber);
            command.Parameters.Add(p8);
            SQLiteDataReader reader = command.ExecuteReader();
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
            SQLiteParameter p1 = new SQLiteParameter("$PONUMBER", PO.PONumber);
            SQLiteParameter p2 = new SQLiteParameter("$COST", PO.totalCost);
            SQLiteParameter p3 = new SQLiteParameter("$TOTALITEMS", PO.totalItems);
            SQLiteParameter p4 = new SQLiteParameter("$CHECKIN", PO.assignedCheckin);
            SQLiteParameter p5 = new SQLiteParameter("$SUPPLIER", PO.supplier);
            SQLiteParameter p6 = new SQLiteParameter("$FINALIZED", PO.finalized);
            SQLiteParameter p7 = new SQLiteParameter("$SHIPPING", PO.shippingCost);
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
                p1 = new SQLiteParameter("$PONUMBER", PO.PONumber);
                p2 = new SQLiteParameter("$ID", Guid.NewGuid());
                p3 = new SQLiteParameter("$ITEMNUMBER", item.itemNumber);
                p4 = new SQLiteParameter("$PRODUCTLINE", item.productLine);
                p5 = new SQLiteParameter("$QTY", item.quantity);
                p6 = new SQLiteParameter("$COST", item.cost);
                p7 = new SQLiteParameter("$PRICE", item.price);
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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "DELETE FROM PURCHASEORDERS WHERE PONUMBER = $PO";
            SQLiteParameter p1 = new SQLiteParameter("$PO", PONumber);
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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT MAX(CHECKINNUMBER) FROM CHECKINS";
            SQLiteDataReader reader = command.ExecuteReader();
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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "INSERT INTO CHECKINS (CHECKINNUMBER, PONUMBERS) " +
                "VALUES ($CHECKINNUMBER, $PONUMBERS)";
            SQLiteParameter p1 = new SQLiteParameter("$CHECKINNUMBER", checkin.checkinNumber);
            SQLiteParameter p2 = new SQLiteParameter("$PONUMBERS", POs);
            command.Parameters.Add(p1);
            command.Parameters.Add(p2);

            command.ExecuteNonQuery();

            command.CommandText =
                "INSERT INTO CHECKINITEMS (CHECKINNUMBER, PRODUCTLINE, ITEMNUMBER, ORDEREDQTY, SHIPPEDQTY, RECEIVEDQTY, DAMAGEDQTY) " +
                "VALUES ($CHECKINNUMBER, $LINECODE, $ITEMNUMBER, $ORDERED, $SHIPPED, $RECEIVED, $DAMAGED)";
            
            foreach (CheckinItem item in checkin.items)
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SQLiteParameter("$CHECKINNUMBER", checkin.checkinNumber));
                command.Parameters.Add(new SQLiteParameter("$LINECODE", item.productLine));
                command.Parameters.Add(new SQLiteParameter("$ITEMNUMBER", item.itemNumber));
                command.Parameters.Add(new SQLiteParameter("$ORDERED", item.ordered));
                command.Parameters.Add(new SQLiteParameter("$SHIPPED", item.shipped));
                command.Parameters.Add(new SQLiteParameter("$RECEIVED", item.received));
                command.Parameters.Add(new SQLiteParameter("$DAMAGED", item.damaged));
                command.ExecuteNonQuery();
            }

            foreach (PurchaseOrder order in checkin.orders)
            {
                command.CommandText =
                    "UPDATE PURCHASEORDERS SET ASSIGNEDCHECKIN = $CHECKIN WHERE PONUMBER = $ORDER";
                command.Parameters.Clear();
                command.Parameters.Add(new SQLiteParameter("$CHECKIN", checkin.checkinNumber));
                command.Parameters.Add(new SQLiteParameter("$ORDER", order.PONumber));
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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT CHECKINNUMBER FROM CHECKINS";
            SQLiteDataReader reader = command.ExecuteReader();
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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT * FROM CHECKINS WHERE CHECKINNUMBER = $CHECKINNUMBER";
            SQLiteParameter p1 = new SQLiteParameter("$CHECKINNUMBER", checkinNumber);
            command.Parameters.Add(p1);
            SQLiteDataReader reader = command.ExecuteReader();
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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "DELETE FROM CHECKINS WHERE CHECKINNUMBER = $CHECKIN";
            command.Parameters.Add(new SQLiteParameter("$CHECKIN", checkinNumber));
            command.ExecuteNonQuery();

            command.CommandText =
                "DELETE FROM CHECKINITEMS WHERE CHECKINNUMBER = $CHECKIN";
            command.Parameters.Clear();
            command.Parameters.Add(new SQLiteParameter("$CHECKIN", checkinNumber));
            command.ExecuteNonQuery();

            command.CommandText =
                "UPDATE PURCHASEORDERS SET ASSIGNEDCHECKIN = 0 WHERE ASSIGNEDCHECKIN = $CHECKIN";
            command.Parameters.Clear();
            command.Parameters.Add(new SQLiteParameter("$CHECKIN", checkinNumber));
            command.ExecuteNonQuery();

            CloseConnection();
            return;
        }
        public void UpdateCheckinItem(CheckinItem item, int checkinNumber)
        {
            OpenConnection();

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "UPDATE CHECKINITEMS SET ORDEREDQTY = $ORDERED, SHIPPEDQTY = $SHIPPED, RECEIVEDQTY = $RECEIVED, DAMAGEDQTY = $DAMAGED " +
                "WHERE (CHECKINNUMBER = $CHECKIN AND ITEMNUMBER = $ITEMNO AND PRODUCTLINE = $LINE)";
            command.Parameters.Add(new SQLiteParameter("$ORDERED", item.ordered));
            command.Parameters.Add(new SQLiteParameter("$SHIPPED", item.shipped));
            command.Parameters.Add(new SQLiteParameter("$RECEIVED", item.received));
            command.Parameters.Add(new SQLiteParameter("$DAMAGED", item.damaged));
            command.Parameters.Add(new SQLiteParameter("$CHECKIN", checkinNumber));
            command.Parameters.Add(new SQLiteParameter("$ITEMNO", item.itemNumber));
            command.Parameters.Add(new SQLiteParameter("$LINE", item.productLine));
            command.ExecuteNonQuery();

            CloseConnection();
        }
        #endregion

        #region Accounts
        public List<Account> RetrieveAccounts()
        {
            List<Account> accounts = new List<Account>();
            OpenConnection();

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT * FROM ACCOUNTS";
            SQLiteDataReader reader = command.ExecuteReader();
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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT * FROM ACCOUNTTRANSACTIONS WHERE CREDITACCOUNT = $ACCT OR DEBITACCOUNT = $ACCT";
            command.Parameters.Add(new SQLiteParameter("$ACCT", accountID));
            SQLiteDataReader reader = command.ExecuteReader();
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
            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT MAX(TRANSACTIONID) FROM ACCOUNTTRANSACTIONS";
            SQLiteDataReader reader = command.ExecuteReader();
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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "INSERT INTO ACCOUNTTRANSACTIONS (TRANSACTIONID, DATE, MEMO, CREDITACCOUNT, DEBITACCOUNT, AMOUNT, REFERENCENUMBER)" +
                "VALUES ($TID, $DATE, $MEMO, $CA, $DA, $AMT, $REF)";
            command.Parameters.Add(new SQLiteParameter("$TID", tID));
            command.Parameters.Add(new SQLiteParameter("$DATE", t.date.ToString("MM/dd/yyyy")));
            command.Parameters.Add(new SQLiteParameter("$MEMO", t.memo));
            command.Parameters.Add(new SQLiteParameter("$CA", t.creditAccount));
            command.Parameters.Add(new SQLiteParameter("$DA", t.debitAccount));
            command.Parameters.Add(new SQLiteParameter("$AMT", t.amount));
            command.Parameters.Add(new SQLiteParameter("$REF", t.referenceNumber));
            command.ExecuteNonQuery();

            CloseConnection();
        }
        public void UpdateAccountBalance(int accountID, decimal newBalance)
        {
            OpenConnection();

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "UPDATE ACCOUNTS SET CURRENTBALANCE = $BAL WHERE ID = $ACCT";
            command.Parameters.Add(new SQLiteParameter("$BAL", newBalance));
            command.Parameters.Add(new SQLiteParameter("$ACCT", accountID));
            command.ExecuteNonQuery();

            CloseConnection();
        }
        #endregion
    }
}
