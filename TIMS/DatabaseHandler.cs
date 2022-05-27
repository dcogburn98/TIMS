using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace TIMS
{
    class DatabaseHandler
    {
        public static SQLiteConnection sqlite_conn;

        public static XDocument employeeDB = new XDocument();
        public static XDocument itemDB = new XDocument();
        public static XDocument customerDB = new XDocument();
        public static XDocument runningInvDB = new XDocument();
        public static XDocument savedInvDB = new XDocument();
        public static XDocument accountsDB = new XDocument();
        public static XDocument invoicesDB = new XDocument();
        public static string employeeDBLocation = "Employees.xml";
        public static string itemDBLocation = "Items.xml";
        public static string customerDBLocation = "Customers.xml";
        public static string runningInvDBLocation = "RunningInvoices.xml";
        public static string savedInvDBLocation = "SavedInvoices.xml";
        public static string accountsDBLocation = "Accounts.xml";
        public static string invoicesDBLocation = "Invoices.xml";

        public static void InitializeDatabases()
        {
            sqlite_conn = new SQLiteConnection("Data Source=database.db; Version = 3; New = True; Compress = True; ");
            OpenConnection();
            CloseConnection();

            #region XML Database Stuff
            /*
            #region Check if DB Files Exist
            if (!File.Exists(employeeDBLocation))
            {
                employeeDB = new XDocument(
                    new XElement("Employees"));
                employeeDB.Save(employeeDBLocation);
            }
            else
                employeeDB = XDocument.Load(employeeDBLocation);

            if (!File.Exists(itemDBLocation))
            {
                itemDB = new XDocument(
                    new XElement("Items"));
                itemDB.Save(itemDBLocation);
            }
            else
                itemDB = XDocument.Load(itemDBLocation);

            if (!File.Exists(customerDBLocation))
            {
                customerDB = new XDocument(
                    new XElement("Customers"));
                customerDB.Save(customerDBLocation);
            }
            else
                customerDB = XDocument.Load(customerDBLocation);

            if (!File.Exists(runningInvDBLocation))
            {
                runningInvDB = new XDocument(
                    new XElement("RunningInvoices"));
                runningInvDB.Save(runningInvDBLocation);
            }
            else
                runningInvDB = XDocument.Load(runningInvDBLocation);

            if (!File.Exists(savedInvDBLocation))
            {
                savedInvDB = new XDocument(
                    new XElement("SavedInvoices"));
                savedInvDB.Save(savedInvDBLocation);
            }
            else
                savedInvDB = XDocument.Load(savedInvDBLocation);

            if (!File.Exists(accountsDBLocation))
            {
                accountsDB = new XDocument(
                    new XElement("Accounts"));
                accountsDB.Save(accountsDBLocation);
            }
            else
                accountsDB = XDocument.Load(accountsDBLocation);

            if (!File.Exists(invoicesDBLocation))
            {
                invoicesDB = new XDocument(
                    new XElement("Invoices"));
                invoicesDB.Root.Add(new XElement("NextInvoice", 000001));
                invoicesDB.Save(invoicesDBLocation);
            }
            else
                invoicesDB = XDocument.Load(invoicesDBLocation);
            #endregion

            #region Default Employee Database Contents
            if (employeeDB.Root.Elements().FirstOrDefault(el => el.Element("Username").Value == "admin") == null)
            {
                employeeDB.Root.Add(
                new XElement("Employee",
                    new XElement("EmployeeName", "Administrator"),
                    new XElement("EmployeeNumber", "0"),
                    new XElement("Username", "admin"),
                    new XElement("Password", "admin"),
                    new XElement("PermissionsProfile", "Administrator"),
                    new XElement("StartupScreen", "Administrator")));
                employeeDB.Save(employeeDBLocation);
            }
            #endregion

            #region Default Account Database Contents
            if (accountsDB.Root.Elements().FirstOrDefault(el => el.Name == "Assets") == null)
                accountsDB.Root.Add(new XElement("Assets"));
            if (accountsDB.Root.Elements().FirstOrDefault(el => el.Name == "Equity") == null)
                accountsDB.Root.Add(new XElement("Equity"));
            if (accountsDB.Root.Elements().FirstOrDefault(el => el.Name == "Liabilities") == null)
                accountsDB.Root.Add(new XElement("Liabilities"));
            if (accountsDB.Root.Elements().FirstOrDefault(el => el.Name == "Income") == null)
                accountsDB.Root.Add(new XElement("Income"));
            if (accountsDB.Root.Elements().FirstOrDefault(el => el.Name == "Expenses") == null)
                accountsDB.Root.Add(new XElement("Expenses"));

            if (accountsDB.Root.Element("Assets").Elements().FirstOrDefault(el => el.Element("Name").Value == "Inventory") == null)
            {
                accountsDB.Root.Element("Assets").Add(
                new XElement("Account",
                    new XElement("Name", "Inventory"),
                    new XElement("Description", "Physical Inventory"),
                    new XElement("ID", "1000"),
                    new XElement("Balance", 0.00d)));
            }

            accountsDB.Save(accountsDBLocation);
            #endregion

            #region Default Customer Database Contents
            if (customerDB.Root.Elements().FirstOrDefault(el => el.Element("CustomerName").Value == "Cash Sale") == null)
            {
                customerDB.Root.Add(
                new XElement("Customer",
                    new XElement("CustomerName", "Cash Sale"),
                    new XElement("CustomerNumber", "0")));
                customerDB.Save(customerDBLocation);
            }
            #endregion

            #region Default Item Database Contents
            if (itemDB.Root.Elements().FirstOrDefault(el => el.Element("ItemName").Value == "Test Item") == null)
            {
                itemDB.Root.Add(
                new XElement("Item",
                    new XElement("ItemName", "Test Item"),
                    new XElement("ItemNumber", "TTT"),
                    new XElement("ProductLine", "TTT"),
                    new XElement("ItemPrice", 1.00f)));
                itemDB.Save(itemDBLocation);
            }
            if (itemDB.Root.Elements().FirstOrDefault(el => el.Element("ItemName").Value == "Shop Towels") == null)
            {
                itemDB.Root.Add(
                new XElement("Item",
                    new XElement("ItemName", "Shop Towels"),
                    new XElement("ItemNumber", "75130"),
                    new XElement("ProductLine", "NSE"),
                    new XElement("ItemPrice", 3.49f)));
                itemDB.Save(itemDBLocation);
            }
            if (itemDB.Root.Elements().FirstOrDefault(el => el.Element("ItemName").Value == "NAPA 5w-30 Conventional") == null)
            {
                itemDB.Root.Add(
                new XElement("Item",
                    new XElement("ItemName", "NAPA 5w-30 Conventional"),
                    new XElement("ItemNumber", "75130"),
                    new XElement("ProductLine", "NOL"),
                    new XElement("ItemPrice", 5.19f)));
                itemDB.Save(itemDBLocation);
            }
            #endregion
            */
            #endregion
        }

        #region XML Database Methods
        /*
        public void AddEmployee(string name, string employeeNumber, string[] permissions)
        {
            employeeDB.Root.Add(
                new XElement("Employee",
                    new XElement("Employee Name", "Administrator"),
                    new XElement("Username", "admin"),
                    new XElement("Password", "admin"),
                    new XElement("Permissions Profile", "Administrator"),
                    new XElement("Startup Screen", "Administrator")));
        }

        public static void AddCustomer(Customer customer)
        {
            customerDB.Root.Add(
                new XElement("Customer",
                    new XElement("CustomerNumber", customer.customerNumber),
                    new XElement("CustomerName", customer.customerName),
                    new XElement("Phone", customer.phoneNumber),
                    new XElement("Fax", customer.faxNumber),
                    new XElement("TaxExempt", customer.taxExempt, new XAttribute("TaxExemptionNumber", customer.taxExemptionNumber)),
                    new XElement("AvaiablePaymentTypes", customer.availablePaymentTypes),
                    new XElement("CanCharge", customer.canCharge),
                    new XElement("CreditLimit", customer.creditLimit),
                    new XElement("AccountBalance", customer.accountBalance),
                    new XElement("InvoiceMessage", customer.invoiceMessage),
                    new XElement("MailingAddress", customer.mailingAddress),
                    new XElement("ShippingAddress", customer.shippingAddress)));

        }

        public static void AddItem(string[] information)
        {
            itemDB.Root.Add(
                new XElement("Item",
                    new XElement("ItemNumber", information[0]),
                    new XElement("ItemDescription", information[1]),
                    new XElement("Supplier", information[2]),
                    new XElement("LocationCode", information[3]),
                    new XElement("Pricing",
                        new XElement("Red", information[2]),
                        new XElement("Blue", information[3]),
                        new XElement("Green", information[4]),
                        new XElement("Grey", information[5]),
                        new XElement("White", information[6]),
                        new XElement("Black", information[7]))));
        }

        public static XElement CheckEmployee(string userornumber)
        {
            XElement e = employeeDB.Root.Elements().FirstOrDefault(el => el.Element("Username").Value == userornumber);
            if (e == null)
                e = employeeDB.Root.Elements().FirstOrDefault(el => el.Element("EmployeeNumber").Value == userornumber);
            if (e == null)
                return new XElement("Invalid");
            else
                return e;
        }

        public static Employee Login(string userornumber, string password)
        {
            XElement e = employeeDB.Root.Elements().FirstOrDefault(el => el.Element("Username").Value == userornumber);
            if (e == null)
                e = employeeDB.Root.Elements().FirstOrDefault(el => el.Element("EmployeeNumber").Value == userornumber);
            if (e == null)
                return null;

            if (e.Element("Password").Value == password)
            {
                Employee em = new Employee(int.Parse(e.Element("EmployeeNumber").Value), e.Element("EmployeeName").Value);
                return em;
            }
            else
                return null;
        }

        public static List<Item> CheckItemNumber(string itemNo)
        {
            List<Item> invItems = new List<Item>();
            List<XElement> items = itemDB.Root.Elements().ToList().FindAll(el => el.Element("ItemNumber").Value == itemNo);
            foreach (XElement item in items)
            {
                Item i = new Item()
                {
                    itemNumber = item.Element("ItemNumber").Value,
                    productLine = item.Element("ProductLine").Value,
                    itemName = item.Element("ItemName").Value,
                    greenPrice = float.Parse(item.Element("ItemPrice").Value),
                    ageRestricted = bool.Parse(item.Element("AgeRestricted").Value),
                    taxed = item.Element("Taxed").Value == "True" ? true : false
                };
                if (item.Elements().FirstOrDefault(el => el.Name == "MinimumAge") != null)
                    i.minimumAge = int.Parse(item.Element("MinimumAge").Value);
                invItems.Add(i);
            }
            if (invItems.Count == 0)
                return null;
            else
                return invItems;
        }

        public static Customer CheckCustomerNumber(string custNo)
        {
            Customer cust = new Customer();
            var addresses = from address in customerDB.Root.Elements("Customer")
                            where address.Element("CustomerNumber").Value == custNo
                            select address;
            if (addresses.Count() < 1)
                return null;

            List<Payment.PaymentTypes> ptypes = new List<Payment.PaymentTypes>();
            string[] paymentTypes = addresses.First().Element("PaymentTypes").Value.Split(',');
            foreach (string p in paymentTypes)
            {
                switch (p)
                {
                    case "Cash":
                        ptypes.Add(Payment.PaymentTypes.Cash);
                        break;
                    case "Check":
                        ptypes.Add(Payment.PaymentTypes.Check);
                        break;
                    case "PaymentCard":
                        ptypes.Add(Payment.PaymentTypes.PaymentCard);
                        break;
                    case "CashApp":
                        ptypes.Add(Payment.PaymentTypes.CashApp);
                        break;
                    case "Venmo":
                        ptypes.Add(Payment.PaymentTypes.Venmo);
                        break;
                    case "Paypal":
                        ptypes.Add(Payment.PaymentTypes.Paypal);
                        break;
                    case "Charge":
                        ptypes.Add(Payment.PaymentTypes.Charge);
                        break;
                }
            }

            cust.customerNumber = addresses.First().Element("CustomerNumber").Value;
            cust.customerName = addresses.First().Element("CustomerName").Value;
            cust.mailingAddress = addresses.First().Element("MailingAddress").Value;
            cust.shippingAddress = addresses.First().Element("ShippingAddress").Value;
            //cust.availablePaymentTypes = ptypes.ToArray();

            return cust;
        }

        public static void SaveReleasedInvoice(Invoice inv)
        {
            //itemDB.Root.Add(
            //    new XElement("Invoice",
            //        new XElement("InvoiceNumber", inv.invoiceNumber),
            //        new XElement("CustomerNumber", inv.customer.customerNumber),
            //        new XElement("EmployeeNumber", inv.employee.employeeNumber),
            //        new XElement("Subtotal", inv.subtotal),
            //        new XElement("TaxableTotal", inv.taxableTotal),
            //        new XElement("TaxRate", inv.taxRate),
            //        new XElement("TaxAmount", inv.taxAmount),
            //        new XElement("Total", inv.total),
            //        new XElement("Payments"),
            //        new XElement("TotalPayments", inv.totalPayments),
            //        new XElement("ContainsAgeRestrictedItems", inv.containsAgeRestrictedItem),
            //        new XElement("Customer Birthdate", inv.customerBirthdate),
            //        new XElement("Attention", inv.attentionLine),
            //        new XElement("PONumber", inv.PONumber),
            //        new XElement("Finalized", inv.finalized),
            //        new XElement("Voided", inv.voided),
            //        new XElement("Items")));

            //foreach (InvoiceItem item in inv.items)
            //{
            //    XElement element = itemDB.Root.Elements().First(el => el.Element("InvoiceNumber").Value == inv.invoiceNumber.ToString()).Element("Items");

            //}
        }
        */
        #endregion



        //Used at login to verify a correct username or employee number has been supplied
        public static string SqlCheckEmployee(string input)
        {
            if (!int.TryParse(input, out int v))
                input = "'" + input + "'";
            string value = null;
            OpenConnection();
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

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

            CloseConnection();

            if (value == null)
                return null;
            else
                return value;
        }

        //Used at login to verify password is correct for specified username or employee number
        public static Employee SqlLogin(string user, byte[] pass)
        {
            //System.Threading.Thread.Sleep(1000); Uncomment before release
            Employee e = new Employee();

            if (!int.TryParse(user, out int v))
                user = "'" + user + "'";
            OpenConnection();

            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

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
                CloseConnection();
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

            CloseConnection();
            return e;
        }

        //Used when viewing invoices to retrieve employee information
        public static Employee SqlRetrieveEmployee(string employeeNumber)
        {
            Employee e = new Employee();
            OpenConnection();

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT * FROM EMPLOYEES WHERE EMPLOYEENUMBER = $NUMBER";
            SQLiteParameter p1 = new SQLiteParameter("$NUMBER", employeeNumber);
            command.Parameters.Add(p1);
            SQLiteDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                CloseConnection();
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

            CloseConnection();
            return e;
        }

        public static List<Item> SqlCheckItemNumber(string itemNumber, bool connectionOpened)
        {
            if (!connectionOpened)
                OpenConnection();

            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            sqlite_cmd.CommandText =
                "SELECT *" + " " +
                "FROM ITEMS" + " " +
                "WHERE ITEMNUMBER = $ITEM";

            SQLiteParameter itemParam = new SQLiteParameter("$ITEM", itemNumber);
            sqlite_cmd.Parameters.Add(itemParam);
            SQLiteDataReader reader = sqlite_cmd.ExecuteReader();

            List<Item> invItems = new List<Item>();

            while (reader.Read())
            {
                Item item = new Item();
                item.productLine = reader.GetString(0);
                item.itemNumber = reader.GetString(1);
                item.itemName = reader.GetString(2);
                item.longDescription = reader.GetString(3);
                item.supplier = reader.GetString(4);
                item.groupCode = reader.GetInt32(5);
                item.velocityCode = reader.GetInt32(6);
                item.previousYearVelocityCode = reader.GetInt32(7);
                item.itemsPerContainer = reader.GetInt32(8);
                item.standardPackage = reader.GetInt32(9);
                item.dateStocked = DateTime.Parse(reader.GetString(10));
                item.dateLastReceipt = DateTime.Parse(reader.GetString(11));
                item.minimum = reader.GetFloat(12);
                item.maximum = reader.GetFloat(13);
                item.onHandQty = reader.GetFloat(14);
                item.WIPQty = reader.GetFloat(15);
                item.onOrderQty = reader.GetFloat(16);
                item.onBackorderQty = reader.GetFloat(17);
                item.daysOnOrder = reader.GetInt32(18);
                item.daysOnBackorder = reader.GetInt32(19);
                item.listPrice = reader.GetFloat(20);
                item.redPrice = reader.GetFloat(21);
                item.yellowPrice = reader.GetFloat(22);
                item.greenPrice = reader.GetFloat(23);
                item.pinkPrice = reader.GetFloat(24);
                item.bluePrice = reader.GetFloat(25);
                item.replacementCost = reader.GetFloat(26);
                item.averageCost = reader.GetFloat(27);
                item.taxed = reader.GetBoolean(28);
                item.ageRestricted = reader.GetBoolean(29);
                item.minimumAge = reader.GetInt32(30);
                item.locationCode = reader.GetInt32(31);
                item.serialized = reader.GetBoolean(32);
                invItems.Add(item);
            }

            if (!connectionOpened)
                CloseConnection();
            if (invItems.Count == 0)
                return null;
            else
                return invItems;
        }

        public static bool SqlCheckProductLine(string productLine)
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

        public static Customer SqlCheckCustomerNumber(string custNo)
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

        public static void SqlSaveReleasedInvoice(Invoice inv)
        {
            OpenConnection();
            SQLiteCommand command = sqlite_conn.CreateCommand();

            #region Add General Invoice Information to INVOICES tables
            command.CommandText =
                "INSERT INTO INVOICES (" +

                "INVOICENUMBER,SUBTOTAL,TAXABLETOTAL,TAXRATE,TAXAMOUNT,TOTAL," +
                "TOTALPAYMENTS,AGERESTRICTED,CUSTOMERBIRTHDATE,ATTENTION,PO,MESSAGE," +
                "SAVEDINVOICE,SAVEDINVOICETIME,INVOICECREATIONTIME,INVOICEFINALIZEDTIME,FINALIZED,VOIDED,CUSTOMERNUMBER,EMPLOYEENUMBER) " +

                "VALUES ($INVOICENUMBER,$SUBTOTAL,$TAXABLETOTAL,$TAXRATE,$TAXAMOUNT,$TOTAL," +
                "$TOTALPAYMENTS,$AGERESTRICTED,$CUSTOMERBIRTHDATE,$ATTENTION,$PO,$MESSAGE," +
                "$SAVEDINVOICE,$SAVEDINVOICETIME,$INVOICECREATIONTIME,$INVOICEFINALIZEDTIME,$FINALIZED,$VOIDED,$CUSTOMERNUMBER,$EMPLOYEENUMBER)";

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

            command.ExecuteNonQuery();
            #endregion

            #region Add Invoice Item information to INVOICEITEMS table
            foreach (InvoiceItem item in inv.items)
            {
                command.CommandText =
                    "INSERT INTO INVOICEITEMS (" +

                    "INVOICENUMBER,ITEMNUMBER,PRODUCTLINE,ITEMDESCRIPTION,PRICE,LISTPRICE," +
                    "QUANTITY,TOTAL,PRICECODE,SERIALIZED,SERIALNUMBER,AGERESTRICTED," +
                    "MINIMUMAGE,TAXED,INVOICECODES,GUID) " +

                    "VALUES ($INVOICENUMBER,$ITEMNUMBER,$PRODUCTLINE,$ITEMDESCRIPTION,$PRICE,$LISTPRICE," +
                    "$QUANTITY,$TOTAL,$PRICECODE,$SERIALIZED,$SERIALNUMBER,$AGERESTRICTED," +
                    "$MINIMUMAGE,$TAXED,$INVOICECODES,$GUID)";

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

        public static int SqlRetrieveNextInvoiceNumber()
        {
            OpenConnection();
            SQLiteCommand command = sqlite_conn.CreateCommand();

            command.CommandText = "SELECT MAX(INVOICENUMBER) FROM INVOICES";

            SQLiteDataReader reader = command.ExecuteReader();

            int invNo = 0;
            while (reader.Read())
                invNo = reader.GetInt32(0) + 1;

            CloseConnection();
            return invNo;
        }

        public static string SqlRetrievePropertyString(string key)
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

        public static float SqlRetrievePropertyFloat(string key)
        {
            float property = 0;
            OpenConnection();

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText = "SELECT VALUE FROM GLOBALPROPERTIES WHERE KEY = $KEY";
            SQLiteParameter p1 = new SQLiteParameter("$KEY");
            p1.Value = key;
            command.Parameters.Add(p1);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                property = reader.GetFloat(0);
            }

            CloseConnection();
            return property;
        }

        public static int SqlRetrievePropertyInt(string key)
        {
            int property = 0;
            OpenConnection();

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText = "SELECT VALUE FROM GLOBALPROPERTIES WHERE KEY = $KEY";
            SQLiteParameter p1 = new SQLiteParameter("$KEY");
            p1.Value = key;
            command.Parameters.Add(p1);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                property = reader.GetInt32(0);
            }

            CloseConnection();
            return property;
        }

        public static Invoice SqlRetrieveInvoice(int invNumber)
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
                inv.subtotal = reader.GetFloat(1);
                inv.taxableTotal = reader.GetFloat(2);
                inv.taxRate = reader.GetFloat(3);
                inv.taxAmount = reader.GetFloat(4);
                inv.total = reader.GetFloat(5);
                inv.totalPayments = reader.GetFloat(6);
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
                    paymentAmount = reader.GetFloat(3),
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
                    price = reader.GetFloat(4),
                    listPrice = reader.GetFloat(5),
                    quantity = reader.GetFloat(6),
                    total = reader.GetFloat(7),
                    pricingCode = reader.GetString(8),
                    serializedItem = reader.GetBoolean(9),
                    //serialNumber = reader.GetString(10),
                    ageRestricted = reader.GetBoolean(11),
                    minimumAge = reader.GetInt32(12),
                    taxed = reader.GetBoolean(13),
                    codes = reader.GetString(14).Split(','),
                    ID = reader.GetGuid(15)
                };

                if (item.serializedItem)
                    item.serialNumber = reader.GetString(10);

                inv.items.Add(item);
            }

            CloseConnection();

            inv.customer = SqlCheckCustomerNumber(inv.customer.customerNumber);
            inv.employee = SqlRetrieveEmployee(inv.employee.employeeNumber.ToString());
            return inv;
        }

        public static InvoiceItem SqlRetrieveBarcode(string scannedBarcode)
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
                float d3 = reader.GetFloat(2);
                List<Item> itemMatches = SqlCheckItemNumber(d1, true);
                item = new InvoiceItem(itemMatches.Find(el => el.productLine.ToLower() == d2.ToLower()));
                item.quantity = d3;
            }

            CloseConnection();
            return item;
        }

        public static List<Invoice> SqlRetrieveInvoicesByCriteria(string[] criteria)
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
                invoices.Add(SqlRetrieveInvoice(number));
            }

            return invoices;
        }

        public static Item SqlRetrieveItem(string itemNumber, string productLine)
        {
            Item item = new Item();
            OpenConnection();

            SQLiteCommand command = sqlite_conn.CreateCommand();
            command.CommandText =
                "SELECT * FROM ITEMS WHERE (ITEMNUMBER = $ITEMNO AND PRODUCTLINE = $LINE)";
            SQLiteParameter p1 = new SQLiteParameter("$ITEMNO", itemNumber);
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
                item.productLine = reader.GetString(0);
                item.itemNumber = reader.GetString(1);
                item.itemName = reader.GetString(2);
                item.longDescription = reader.GetString(3);
                item.supplier = reader.GetString(4);
                item.groupCode = reader.GetInt32(5);
                item.velocityCode = reader.GetInt32(6);
                item.previousYearVelocityCode = reader.GetInt32(7);
                item.itemsPerContainer = reader.GetInt32(8);
                item.standardPackage = reader.GetInt32(9);
                item.dateStocked = DateTime.Parse(reader.GetString(10));
                item.dateLastReceipt = DateTime.Parse(reader.GetString(11));
                item.minimum = reader.GetFloat(12);
                item.maximum = reader.GetFloat(13);
                item.onHandQty = reader.GetFloat(14);
                item.WIPQty = reader.GetFloat(15);
                item.onOrderQty = reader.GetFloat(16);
                item.onBackorderQty = reader.GetFloat(17);
                item.daysOnOrder = reader.GetInt32(18);
                item.daysOnBackorder = reader.GetInt32(19);
                item.listPrice = reader.GetFloat(20);
                item.redPrice = reader.GetFloat(21);
                item.yellowPrice = reader.GetFloat(22);
                item.greenPrice = reader.GetFloat(23);
                item.pinkPrice = reader.GetFloat(24);
                item.bluePrice = reader.GetFloat(25);
                item.replacementCost = reader.GetFloat(26);
                item.averageCost = reader.GetFloat(27);
                item.taxed = reader.GetBoolean(28);
                item.ageRestricted = reader.GetBoolean(29);
                item.minimumAge = reader.GetInt32(30);
                item.locationCode = reader.GetInt32(31);
                item.serialized = reader.GetBoolean(32);
            }

            CloseConnection();
            return item;
        }

        public static void SqlUpdateItem(Item newItem)
        {
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
                "TAXED = $TAXED, AGERESTRICTED = $RESTRICTED, MINIMUMAGE = $MINAGE, LOCATIONCODE = $LOCATION " +
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

            command.ExecuteNonQuery();

            CloseConnection();
            return;
        }

        public static List<string> SqlRetrieveItemSerialNumbers(string productLine, string itemNumber)
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

        public static void OpenConnection()
        {
            sqlite_conn.Open();
        }

        public static void CloseConnection()
        {
            sqlite_conn.Close();
        }
    
    
    }
}
