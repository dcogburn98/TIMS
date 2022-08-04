using System;
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

        public string CheckEmployee(string input)
        {
            if (!int.TryParse(input, out int v))
                input = "'" + input + "'";
            string value = null;
            Program.OpenConnection();
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = Program.sqlite_conn.CreateCommand();

            sqlite_cmd.CommandText =
                "SELECT FULLNAME " +
                "FROM EMPLOYEES " +
                "WHERE USERNAME = " + input + " " +
                "OR EMPLOYEENUMBER = " + input;

            SQLiteDataReader rdr = sqlite_cmd.ExecuteReader();
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

    }
}
