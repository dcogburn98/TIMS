using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Data.SQLite;
using System.Security.Cryptography;

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
            #endregion
        }

        #region XML Database Methods
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
        #endregion
        
        
        
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

        public static Employee SqlLogin(string user, byte[] pass)
        {
            
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

        public static List<Item> SqlCheckItemNumber(string itemNumber)
        {
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
                Item i = new Item()
                {
                    productLine = reader.GetString(0),
                    itemNumber = reader.GetString(1),
                    itemName = reader.GetString(2),
                    greenPrice = reader.GetFloat(23),
                    ageRestricted = reader.GetBoolean(29),
                    taxed = reader.GetBoolean(28),
                    minimumAge = reader.GetInt32(30)
                };
                invItems.Add(i);
            }

            CloseConnection();
            if (invItems.Count == 0)
                return null;
            else
                return invItems;
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
                string test = reader.GetString(5);
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
