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
        public static string employeeDBLocation = "C:/Users/Blake/Dropbox/TIMS/Database/Employees.xml";
        public static string itemDBLocation = "C:/Users/Blake/Dropbox/TIMS/Database/Items.xml";
        public static string customerDBLocation = "C:/Users/Blake/Dropbox/TIMS/Database/Customers.xml";
        public static string runningInvDBLocation = "C:/Users/Blake/Dropbox/TIMS/Database/RunningInvoices.xml";
        public static string savedInvDBLocation = "C:/Users/Blake/Dropbox/TIMS/Database/SavedInvoices.xml";

        public static void InitializeDatabases()
        {
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
        }

        public void AddEmployee(string name, string employeeNumber, string[] permissions)
        {
            
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
        
        public static string[] CheckEmployee(int employeeNo)
        {
            if (employeeNo == 1)
                return new string[] { "Blake Cogburn", "all" };
            else if (employeeNo == 2)
                return new string[] { "Shea Cogburn", "msg", "inv" };
            else
                return null;
        }

        public static string[] CheckItemNumber(string itemNo)
        {
            if (itemNo == "75130")
                return new string[] { "Shop Towels", "3.49", "srl" };
            else if (itemNo == "4321")
                return new string[] { "test Item 2", "19.79" };
            else
                return null;
        }

        public static string[] CheckCustomerNumber(string custNo)
        {
            string[] cust = new string[10];
            var addresses = from address in customerDB.Root.Elements("Customer")
                            where address.Element("CustomerNumber").Value == custNo
                            select address;
            if (addresses.Count() < 1)
                return null;
            cust[0] = addresses.First().Element("CustomerNumber").Value;
            cust[1] = addresses.First().Element("CustomerName").Value;
            cust[2] = addresses.First().Element("Phone").Value;
            cust[3] = addresses.First().Element("Address").Element("Street").Value;
            cust[4] = addresses.First().Element("Address").Element("City").Value;
            cust[5] = addresses.First().Element("Address").Element("State").Value;
            cust[6] = addresses.First().Element("Address").Element("Zip").Value;
            cust[7] = addresses.First().Element("PriceProfile").Value;
            cust[8] = addresses.First().Element("TaxExempt").Value;
            cust[9] = addresses.First().Element("TaxExemptionNumber").Value;

            return cust;
        }
    }
}
