using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Drawing;

using PdfSharp.Drawing;

using TIMSServerModel;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace TIMS.Server
{
    class Communication
    {
        private static ChannelFactory<ITIMSServiceModel> channelFactory;
        private static ITIMSServiceModel proxy;
        private static AuthKey currentKey = new AuthKey();

        public static void SetEndpointAddress(string newAddress)
        {
            EndpointIdentity spn = EndpointIdentity.CreateSpnIdentity("TIMSServerEndpoint");
            Uri uri = new Uri(newAddress);
            var address = new EndpointAddress(uri, spn);
            channelFactory = new ChannelFactory<ITIMSServiceModel>("TIMSServerEndpoint", address);
            proxy = channelFactory.CreateChannel();
        }

        #region Employees
        public static string CheckEmployee(string input)
        {
            return proxy.CheckEmployee(input);
        }

        public static Employee Login(string user, byte[] pass) //AuthContainer'd
        {
            Employee e = proxy.Login(user, pass);
            if (e != null)
            {
                if (!e.key.Success)
                {
                    MessageBox.Show("Terminal is not registered to server. Please refer to the TIMS user manual or consult Revitacom help.");
                    return null;
                }
                currentKey = new AuthKey(e.key);
                currentKey.Regenerate();
            }
            return e;
        }

        public static Employee RetrieveEmployee(string employeeNumber)
        {
            return proxy.RetrieveEmployee(employeeNumber);
        }
        #endregion

        #region Items
        public static List<Item> CheckItemNumber(string itemNumber, bool connectionOpened)
        {
            AuthContainer<List<Item>> c = proxy.CheckItemNumber(itemNumber, connectionOpened, currentKey);
            if (c.Key.Success)
            {
                currentKey.Regenerate();
                return c.Data;
            }
            else
            {
                MessageBox.Show("Access Denied.");
                return null;
            }
        } //AuthContainer'd
        public static List<Item> CheckItemNumberFromSupplier(string itemNumber, string supplier)
        {
            AuthContainer<List<Item>> c = proxy.CheckItemNumberFromSupplier(itemNumber, supplier, currentKey);
            if (c.Key.Success)
            {
                currentKey.Regenerate();
                return c.Data;
            }
            else
            {
                MessageBox.Show("Access Denied.");
                return null;
            }
        } //AuthContainer'd
        public static List<Item> RetrieveItemsFromSupplier(string supplier)
        {
            AuthContainer<List<Item>> c = proxy.RetrieveItemsFromSupplier(supplier, currentKey);
            if (c.Key.Success)
            {
                currentKey.Regenerate();
                return c.Data;
            }
            else
            {
                MessageBox.Show("Access Denied.");
                return null;
            }
        } //AuthContainer'd
        public static List<Item> RetrieveItemsFromSupplierBelowMin(string supplier)
        {
            return proxy.RetrieveItemsFromSupplierBelowMin(supplier);
        }
        public static List<Item> RetrieveItemsFromSupplierBelowMax(string supplier)
        {
            return proxy.RetrieveItemsFromSupplierBelowMax(supplier);
        }
        public static List<InvoiceItem> RetrieveItemsFromSupplierSoldAfterLastOrderDate(string supplier)
        {
            return proxy.RetrieveItemsFromSupplierSoldAfterLastOrderDate(supplier);
        }
        public static List<string> RetrieveSuppliers()
        {
            return proxy.RetrieveSuppliers();
        }
        public static void AddSupplier(string supplier)
        {
            proxy.AddSupplier(supplier);
        }
        public static bool CheckProductLine(string productLine)
        {
            return proxy.CheckProductLine(productLine);
        }
        public static void AddProductLine(string productLine)
        {
            proxy.AddProductLine(productLine);
        }
        public static Item RetrieveItemFromBarcode(string scannedBarcode)
        {
            return proxy.RetrieveItemFromBarcode(scannedBarcode);
        }
        public static InvoiceItem RetrieveInvoiceItemFromBarcode(string scannedBarcode)
        {
            return proxy.RetrieveInvoiceItemFromBarcode(scannedBarcode);
        }
        public static Item RetrieveItem(string itemNumber, string productLine, bool connectionOpened = false)
        {
            return proxy.RetrieveItem(itemNumber, productLine, connectionOpened);
        }
        public static void UpdateItem(Item newItem)
        {
            proxy.UpdateItem(newItem);
        }
        public static List<string> RetrieveItemSerialNumbers(string productLine, string itemNumber)
        {
            return proxy.RetrieveItemSerialNumbers(productLine, itemNumber);
        }
        public static bool AddItem(Item item)
        {
            return proxy.AddItem(item);
        }
        public static List<Item> RetrieveLabelOutOfDateItems()
        {
            return proxy.RetrieveLabelOutOfDateItems();
        }

        public static List<Item> RetrieveItemsFromSubdepartment(string subdepartment, string parentDepartment)
        {
            AuthContainer<List<Item>> c = proxy.RetrieveItemsFromSubdepartment(subdepartment, parentDepartment, currentKey);
            if (c.Key.Success)
            {
                currentKey.Regenerate();
                return c.Data;
            }
            else
            {
                MessageBox.Show("Access Denied.");
                return null;
            }
        } //AuthContainer'd
        
        public static List<Item> SearchItemsByQuery(string query)
        {
            AuthContainer<List<Item>> c = proxy.SearchItemsByQuery(query, currentKey);
            if (c.Key.Success)
            {
                currentKey.Regenerate();
                return c.Data;
            }
            else
            {
                MessageBox.Show("Access Denied.");
                return null;
            }
        } //AuthContainer'd
        #endregion

        #region Categorization
        public static List<string> RetrieveProductBrands()
        {
            AuthContainer<List<string>> c = proxy.RetrieveProductBrands(currentKey);
            if (c.Key.Success)
            {
                currentKey.Regenerate();
                return c.Data;
            }
            else
            {
                MessageBox.Show("Access Denied.");
                return null;
            }
        } //AuthContainer'd
        public static List<string> RetrieveProductCategories()
        {
            AuthContainer<List<string>> c = proxy.RetrieveProductCategories(currentKey);
            if (c.Key.Success)
            {
                currentKey.Regenerate();
                return c.Data;
            }
            else
            {
                MessageBox.Show("Access Denied.");
                return null;
            }
        } //AuthContainer'd
        public static List<string> RetrieveProductDepartments()
        {
            AuthContainer<List<string>> c = proxy.RetrieveProductDepartments(currentKey);
            if (c.Key.Success)
            {
                currentKey.Regenerate();
                return c.Data;
            }
            else
            {
                MessageBox.Show("Access Denied.");
                return null;
            }
        } //AuthContainer'd
        public static List<string> RetrieveProductSubdepartments(string department)
        {
            AuthContainer<List<string>> c = proxy.RetrieveProductSubdepartments(department, currentKey);
            if (c.Key.Success)
            {
                currentKey.Regenerate();
                return c.Data;
            }
            else
            {
                MessageBox.Show("Access Denied.");
                return null;
            }
        } //AuthContainer'd

        public static void AddProductBrand(string brand)
        {
            AuthContainer<object> container = proxy.AddProductBrand(brand, currentKey);
            if (container.Key.Success)
                currentKey.Regenerate();
            else
                MessageBox.Show("Access Denied.");
        } //AuthContainer'd
        public static void AddProductCategory(string category)
        {
            AuthContainer<object> container = proxy.AddProductCategory(category, currentKey);
            if (container.Key.Success)
                currentKey.Regenerate();
            else
                MessageBox.Show("Access Denied.");
        } //AuthContainer'd
        public static void AddProductDepartment(string department)
        {
            AuthContainer<object> container = proxy.AddProductDepartment(department, currentKey);
            if (container.Key.Success)
                currentKey.Regenerate();
            else
                MessageBox.Show("Access Denied.");
        } //AuthContainer'd
        public static void AddProductSubdepartment(string parentDepartment, string subDepartment)
        {
            AuthContainer<object> container = proxy.AddProductSubdepartment(parentDepartment, subDepartment, currentKey);
            if (container.Key.Success)
                currentKey.Regenerate();
            else
                MessageBox.Show("Access Denied.");
        } //AuthContainer'd
        #endregion

        #region Customers

        public static Customer CheckCustomerNumber(string custNo)
        {
            if (custNo == string.Empty)
                return null;

            AuthContainer<Customer> c = proxy.CheckCustomerNumber(custNo, currentKey);
            if (c.Key.Success)
            {
                currentKey.Regenerate();
                return c.Data;
            }
            else
            {
                MessageBox.Show("Access Denied.");
                return null;
            }
        } //AuthContainer'd
        public static List<Customer> GetCustomers()
        {
            AuthContainer<List<Customer>> c = proxy.GetCustomers(currentKey);
            if (c.Key.Success)
            {
                currentKey.Regenerate();
                return c.Data;
            }
            else
            {
                MessageBox.Show("Access Denied.");
                return null;
            }
        } //AuthContainer'd
        public static void UpdateCustomer(Customer customer)
        {
            AuthContainer<object> container = proxy.UpdateCustomer(customer, currentKey);
            if (container.Key.Success)
                currentKey.Regenerate();
            else
                MessageBox.Show("Access Denied.");
        } //AuthContainer'd
        public static void AddCustomer(Customer customer)
        {
            AuthContainer<object> container = proxy.AddCustomer(customer, currentKey);
            if (container.Key.Success)
                currentKey.Regenerate();
            else
                MessageBox.Show("Access Denied.");
        } //AuthContainer'd

        #endregion

        #region Pricing Profiles

        public static List<PricingProfile> RetrievePricingProfiles()
        {
            AuthContainer<List<PricingProfile>> container = proxy.RetrievePricingProfiles(currentKey);
            if (container.Key.Success)
            {
                currentKey.Regenerate();
                return container.Data;
            }
            else
            {
                MessageBox.Show("Access Denied.");
                return null;
            }
        } //AuthContainer'd
        public static void UpdatePricingProfile(PricingProfile profile)
        {
            AuthContainer<object> container = proxy.UpdatePricingProfile(profile, currentKey);
            if (container.Key.Success)
                currentKey.Regenerate();
            else
                MessageBox.Show("Access Denied.");
        } //AuthContainer'd
        public static int RetrieveNextPricingProfileID()
        {
            AuthContainer<int> container = proxy.RetrieveNextPricingProfileID(currentKey);
            if (container.Key.Success)
            {
                currentKey.Regenerate();
                return container.Data;
            }
            else
            {
                MessageBox.Show("Access Denied.");
                return 0;
            }
        } //AuthContainer'd
        public static void AddPricingProfile(PricingProfile profile)
        {
            AuthContainer<object> container = proxy.AddPricingProfile(profile, currentKey);
            if (container.Key.Success)
                currentKey.Regenerate();
            else
                MessageBox.Show("Access Denied.");
        } //AuthContainer'd
        #endregion

        #region Invoices
        public static List<Invoice> RetrieveInvoicesByDateRange(DateTime startDate, DateTime endDate, bool connectionOpened = false)
        {
            return proxy.RetrieveInvoicesByDateRange(startDate, endDate, connectionOpened);
        }

        public static Invoice RetrieveInvoice(int invNumber)
        {
            return proxy.RetrieveInvoice(invNumber);
        }

        public static List<Invoice> RetrieveInvoicesByCriteria(string[] criteria)
        {
            return proxy.RetrieveInvoicesByCriteria(criteria);
        }

        public static List<Invoice> RetrieveSavedInvoices()
        {
            AuthContainer<List<Invoice>> c = proxy.RetrieveSavedInvoices(currentKey);
            if (c.Key.Success)
            {
                currentKey.Regenerate();
                return c.Data;
            }
            else
            {
                MessageBox.Show("Access Denied.");
                return null;
            }
        } //AuthContainer'd

        public static void DeleteSavedInvoice(Invoice inv)
        {
            AuthContainer<object> container = proxy.DeleteSavedInvoice(inv, currentKey);
            if (container.Key.Success)
                currentKey.Regenerate();
            else
                MessageBox.Show("Access Denied.");
        } //AuthContainer'd

        public static void SaveInvoice(Invoice inv)
        {
            proxy.SaveInvoice(inv);
        }

        public static int RetrieveNextInvoiceNumber()
        {
            return proxy.RetrieveNextInvoiceNumber();
        }
        #endregion

        #region Global Properties
        public static string RetrieveProperty(string key)
        {
            AuthContainer<string> container = proxy.RetrieveProperty(key, currentKey);
            if (container.Key.Success)
            {
                currentKey.Regenerate();
                return container.Data;
            }
            else
            {
                MessageBox.Show("Access Denied.");
                return "";
            }
        } //AuthContainer'd
        public static void SetImage(string key, Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, ImageFormat.Png);

            AuthContainer<object> container = proxy.SetImage(key, ms.ToArray(), currentKey);
            if (container.Key.Success)
                currentKey.Regenerate();
            else
                MessageBox.Show("Access Denied.");
        } //AuthContainer'd
        public static Image RetrieveImage(string key)
        {
            AuthContainer<byte[]> container = proxy.RetrieveImage(key, currentKey);
            if (container.Key.Success)
            {
                currentKey.Regenerate();

                MemoryStream ms = new MemoryStream(container.Data);
                try
                {
                    Image img = Image.FromStream(ms);
                    return img;
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                MessageBox.Show("Access Denied.");
                return null;
            }
        } //AuthContainer'd
        public static Image RetrieveCompanyLogo()
        {
            MemoryStream ms = new MemoryStream(proxy.RetrieveCompanyLogo());
            try
            {
                Image img = Image.FromStream(ms);
                return img;
            }
            catch
            {
                return null;
            }
        }
        public static Image RetrieveProductImage(string path) //AuthContainer'd
        {
            AuthContainer<byte[]> container = proxy.RetrieveProductImage(path, currentKey);
            if (container.Key.Success)
            {
                currentKey.Regenerate();

                MemoryStream ms = new MemoryStream(container.Data);
                try
                {
                    Image img = Image.FromStream(ms);
                    return img;
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                MessageBox.Show("Access Denied.");
                return null;
            }
        }

        #endregion

        #region Item Shortcut Menus
        public static List<ItemShortcutMenu> RetrieveShortcutMenus()
        {
            return proxy.RetrieveShortcutMenus();
        }
        #endregion

        #region Barcodes
        public static void AddBarcode(string itemnumber, string productline, string barcode, decimal quantity)
        {
            proxy.AddBarcode(itemnumber, productline, barcode, quantity);
        }

        public static List<string> RetrieveBarcode(Item item)
        {
            return proxy.RetrieveBarcode(item);
        }

        public static void UpdateBarcode(string barcode, InvoiceItem data)
        {
            AuthContainer<object> container = proxy.UpdateBarcode(barcode, data, currentKey);
            if (container.Key.Success)
                currentKey.Regenerate();
            else
                MessageBox.Show("Access Denied.");
        } //AuthContainer'd
        #endregion

        #region Reports
        public static List<string> RetrieveTableNames()
        {
            return proxy.RetrieveTableNames();
        }
        public static List<string> RetrieveTableHeaders(string table)
        {
            return proxy.RetrieveTableHeaders(table);
        }
        public static List<object> ReportQuery(string query, int columns)
        {
            return proxy.ReportQuery(query, columns);
        }
        public static void SaveReport(Report report)
        {
            proxy.SaveReport(report);
        }
        public static Report RetrieveReport(string shortcode)
        {
            return proxy.RetrieveReport(shortcode);
        }
        public static List<string> RetrieveAvailableReports()
        {
            return proxy.RetrieveAvailableReports();
        }
        #endregion

        #region POs and Checkins
        public static int RetrieveNextPONumber()
        {
            return proxy.RetrieveNextPONumber();
        }
        public static List<PurchaseOrder> RetrievePurchaseOrders()
        {
            return proxy.RetrievePurchaseOrders();
        }
        public static PurchaseOrder RetrievePurchaseOrder(int PONumber, bool connectionOpened = false)
        {
            return proxy.RetrievePurchaseOrder(PONumber, connectionOpened);
        }
        public static void SavePurchaseOrder(PurchaseOrder PO)
        {
            proxy.SavePurchaseOrder(PO);
        }
        public static void FinalizePurchaseOrder(PurchaseOrder PO)
        {
            proxy.FinalizePurchaseOrder(PO);
        }
        public static void DeletePurchaseOrder(int PONumber, bool connectionOpened = false)
        {
            proxy.DeletePurchaseOrder(PONumber, connectionOpened);
        }
        public static int RetrieveNextCheckinNumber()
        {
            return proxy.RetrieveNextCheckinNumber();
        }
        public static void SaveCheckin(Checkin checkin)
        {
            proxy.SaveCheckin(checkin);
        }
        public static List<Checkin> RetrieveCheckins()
        {
            return proxy.RetrieveCheckins();
        }
        public static Checkin RetrieveCheckin(int checkinNumber)
        {
            return proxy.RetrieveCheckin(checkinNumber);
        }
        public static void DeleteCheckin(int checkinNumber)
        {
            proxy.DeleteCheckin(checkinNumber);
        }
        public static void UpdateCheckinItem(CheckinItem item, int checkinNumber)
        {
            proxy.UpdateCheckinItem(item, checkinNumber);
        }
        #endregion

        #region Accounts
        public static List<Account> RetrieveAccounts()
        {
            return proxy.RetrieveAccounts();
        }
        public static List<Transaction> RetrieveAccountTransactions(string accountName)
        {
            return proxy.RetrieveAccountTransactions(accountName);
        }
        public static int RetrieveNextTransactionNumber()
        {
            return proxy.RetrieveNextTransactionNumber();
        }
        public static void SaveTransaction(Transaction t)
        {
            proxy.SaveTransaction(t);
        }
        public static void UpdateAccountBalance(int accountID, decimal newBalance)
        {
            proxy.UpdateAccountBalance(accountID, newBalance);
        }
        public static void VoidTransaction(int TransactionID)
        {
            AuthContainer<object> container = proxy.VoidTransaction(TransactionID, currentKey);
            if (container.Key.Success)
                currentKey.Regenerate();
            else
                MessageBox.Show("Access Denied.");
        } //AuthContainer'd
        #endregion

        #region Devices
        public static bool DeviceExists(string address)
        {
            return proxy.DeviceExists(address);
        }
        public static List<Device> RetrieveTerminals()
        {
            return proxy.RetrieveTerminals();
        }
        public static List<Device> RetrieveDevices()
        {
            return proxy.RetrieveDevices();
        }
        public static bool RegisterDevice(Device device)
        {
            return proxy.RegisterDevice(device);
        }
        public static bool DeleteDevice(Device device)
        {
            return proxy.DeleteDevice(device);
        }
        public static bool AssignDevice(Device terminal, Device device)
        {
            return proxy.AssignDevice(terminal, device);
        }
        public static bool RemoveDeviceAssignment(Device terminal, Device device)
        {
            return proxy.RemoveDeviceAssignment(terminal, device);
        }
        
        public static string TestIngenicoRequest(IngenicoRequest request)
        {
            try
            {
                AuthContainer<string> c = proxy.TestIngenicoRequest(request, currentKey);
                if (c.Key.Success)
                {
                    currentKey.Regenerate();
                    return c.Data;
                }
                else
                {
                    MessageBox.Show("Access Denied.");
                    return null;
                }
            }
            catch (TimeoutException)
            {
                currentKey.Regenerate();
                return "Timeout";
            }
        } //AuthContainer'd
        public static Payment InitiatePayment(decimal paymentAmount)
        {
            AuthContainer<Payment> c = proxy.InitiatePayment(paymentAmount, currentKey);
            if (c.Key.Success)
            {
                currentKey.Regenerate();
                return c.Data;
            }
            else
            {
                MessageBox.Show("Access Denied.");
                return null;
            }
        } //AuthContainer'd
        public static Payment InitiateRefund(decimal refundAmount)
        {
            AuthContainer<Payment> c = proxy.InitiateRefund(refundAmount, currentKey);
            if (c.Key.Success)
            {
                currentKey.Regenerate();
                return c.Data;
            }
            else
            {
                MessageBox.Show("Access Denied.");
                return null;
            }
        } //AuthContainer'd
        public static string RequestSignature()
        {
            try
            {
                AuthContainer<string> c = proxy.RequestSignature(currentKey);
                if (c.Key.Success)
                {
                    currentKey.Regenerate();
                    return c.Data;
                }
                else
                {
                    MessageBox.Show("Access Denied.");
                    return null;
                }
            }
            catch (TimeoutException)
            {
                return null;
            }
        } //AuthContainer'd

        public static void PrintReceipt(Invoice inv)
        {
            AuthContainer<object> container = proxy.PrintReceipt(inv, currentKey);
            if (container.Key.Success)
                currentKey.Regenerate();
            else
                MessageBox.Show("Access Denied.");
        } //AuthContainer'd
        #endregion

        #region Messages
        public static List<MailMessage> GetEmployeeMessages(string employee, bool justUnread = false)
        {
            AuthContainer<List<MailMessage>> c = proxy.GetEmployeeMessages(employee, currentKey, justUnread);
            if (c.Key.Success)
            {
                currentKey.Regenerate();
                foreach (MailMessage msg in c.Data)
                {
                    msg.Sender = RetrieveEmployee(msg.Sender.employeeNumber.ToString());
                    msg.Recipient = RetrieveEmployee(msg.Recipient.employeeNumber.ToString());
                }
                return c.Data;
            }
            else
            {
                MessageBox.Show("Access Denied.");
                return null;
            }
        }

        public static void SendMessage(List<MailMessage> messages)
        {
            AuthContainer<object> container = proxy.SendMessage(messages, currentKey);
            if (container.Key.Success)
                currentKey.Regenerate();
            else
                MessageBox.Show("Access Denied.");
        }
        #endregion
    }
}
