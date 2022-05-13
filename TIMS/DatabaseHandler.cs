using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace TIMS
{
    class DatabaseHandler
    {
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
        }

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
                    new XElement("Address",
                        new XElement("Street", customer.customerStreetAddress),
                        new XElement("City", customer.customerCity),
                        new XElement("State", customer.customerState),
                        new XElement("ZipCode", customer.customerZip))));
                    
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
            cust.availablePaymentTypes = ptypes.ToArray();

            return cust;
        }
    }
}
