using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using PdfSharp.Drawing;

namespace TIMSServerModel
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IEmployeeModel" in both code and config file together.
    [ServiceContract]
    public interface ITIMSServiceModel
    {
        #region Employees
        [OperationContract]
        string CheckEmployee(string input);
        [OperationContract]
        Employee Login(string user, byte[] pass);
        [OperationContract]
        Employee RetrieveEmployee(string employeeNumber);
        #endregion

        #region Items
        [OperationContract]
        AuthContainer<List<Item>> CheckItemNumber(string itemNumber, bool connectionOpened, AuthKey key);
        [OperationContract]
        AuthContainer<List<Item>> CheckItemNumberFromSupplier(string itemNumber, string supplier, AuthKey key);
        [OperationContract]
        AuthContainer<List<Item>> RetrieveItemsFromSupplier(string supplier, AuthKey key);
        [OperationContract]
        List<Item> RetrieveItemsFromSupplierBelowMin(string supplier);
        [OperationContract]
        List<Item> RetrieveItemsFromSupplierBelowMax(string supplier);
        [OperationContract]
        List<InvoiceItem> RetrieveItemsFromSupplierSoldAfterLastOrderDate(string supplier);
        [OperationContract]
        List<string> RetrieveSuppliers();
        [OperationContract]
        void AddSupplier(string supplier);
        [OperationContract]
        bool CheckProductLine(string productLine);
        [OperationContract]
        void AddProductLine(string productLine);
        [OperationContract]
        Item RetrieveItemFromBarcode(string scannedBarcode);
        [OperationContract]
        InvoiceItem RetrieveInvoiceItemFromBarcode(string scannedBarcode);
        [OperationContract]
        Item RetrieveItem(string itemNumber, string productLine, bool connectionOpened = false);
        [OperationContract]
        void UpdateItem(Item newItem, bool connectionOpened = false);
        [OperationContract]
        List<string> RetrieveItemSerialNumbers(string productLine, string itemNumber);
        [OperationContract]
        bool AddItem(Item item);
        [OperationContract]
        List<Item> RetrieveLabelOutOfDateItems();
        #endregion

        #region Categorization
        [OperationContract]
        AuthContainer<List<string>> RetrieveProductBrands(AuthKey key);
        [OperationContract]
        AuthContainer<List<string>> RetrieveProductCategories(AuthKey key);
        [OperationContract]
        AuthContainer<List<string>> RetrieveProductDepartments(AuthKey key);
        [OperationContract]
        AuthContainer<List<string>> RetrieveProductSubdepartments(string department, AuthKey key);
        [OperationContract]
        AuthContainer<object> AddProductBrand(string brand, AuthKey key);
        [OperationContract]
        AuthContainer<object> AddProductCategory(string category, AuthKey key);
        [OperationContract]
        AuthContainer<object> AddProductDepartment(string department, AuthKey key);
        [OperationContract]
        AuthContainer<object> AddProductSubdepartment(string parentDepartment, string subdepartment, AuthKey key);
        #endregion

        #region Invoices
        [OperationContract]
        List<Invoice> RetrieveInvoicesByDateRange(DateTime startDate, DateTime endDate, bool connectionOpened = false);
        [OperationContract]
        Invoice RetrieveInvoice(int invNumber);
        [OperationContract]
        List<Invoice> RetrieveInvoicesByCriteria(string[] criteria);
        [OperationContract]
        AuthContainer<List<Invoice>> RetrieveSavedInvoices(AuthKey key);
        [OperationContract]
        AuthContainer<object> DeleteSavedInvoice(Invoice inv, AuthKey key);
        [OperationContract]
        void SaveInvoice(Invoice inv);
        [OperationContract]
        int RetrieveNextInvoiceNumber();
        #endregion

        #region Customers
        [OperationContract]
        AuthContainer<Customer> CheckCustomerNumber(string custNo, AuthKey key);
        [OperationContract]
        AuthContainer<List<Customer>> GetCustomers(AuthKey key);
        [OperationContract]
        AuthContainer<object> UpdateCustomer(Customer c, AuthKey key);
        [OperationContract]
        AuthContainer<object> AddCustomer(Customer c, AuthKey key);
        #endregion

        #region Pricing Profiles
        [OperationContract]
        AuthContainer<List<PricingProfile>> RetrievePricingProfiles(AuthKey key);
        [OperationContract]
        AuthContainer<object> UpdatePricingProfile(PricingProfile profile, AuthKey key);
        [OperationContract]
        AuthContainer<int> RetrieveNextPricingProfileID(AuthKey key);
        [OperationContract]
        AuthContainer<object> AddPricingProfile(PricingProfile profile, AuthKey key);
        #endregion

        #region Global Properties
        [OperationContract]
        AuthContainer<string> RetrieveProperty(string key, AuthKey authkey);
        [OperationContract]
        AuthContainer<object> SetImage(string key, byte[] imgBytes, AuthKey authkey);
        [OperationContract]
        AuthContainer<byte[]> RetrieveImage(string key, AuthKey authkey);
        [OperationContract]
        byte[] RetrieveCompanyLogo();
        #endregion

        #region Item Shortcut Menus
        [OperationContract]
        List<ItemShortcutMenu> RetrieveShortcutMenus();
        #endregion

        #region Barcodes
        [OperationContract]
        void AddBarcode(string itemnumber, string productline, string barcode, decimal quantity);
        [OperationContract]
        List<string> RetrieveBarcode(Item item);
        [OperationContract]
        AuthContainer<object> UpdateBarcode(string barcode, InvoiceItem data, AuthKey key);
        #endregion

        #region Reports
        [OperationContract]
        List<string> RetrieveTableNames();
        [OperationContract]
        List<string> RetrieveTableHeaders(string table);
        [OperationContract]
        List<object> ReportQuery(string query, int columns);
        [OperationContract]
        void SaveReport(Report report);
        [OperationContract]
        Report RetrieveReport(string shortcode);
        [OperationContract]
        List<string> RetrieveAvailableReports();
        #endregion

        #region POs and Checkins
        [OperationContract]
        int RetrieveNextPONumber(bool connectionOpened = false);
        [OperationContract]
        List<PurchaseOrder> RetrievePurchaseOrders();
        [OperationContract]
        PurchaseOrder RetrievePurchaseOrder(int PONumber, bool connectionOpened = false);
        [OperationContract]
        void SavePurchaseOrder(PurchaseOrder PO);
        [OperationContract]
        void FinalizePurchaseOrder(PurchaseOrder PO);
        [OperationContract]
        void DeletePurchaseOrder(int PONumber, bool connectionOpened = false);
        [OperationContract]
        int RetrieveNextCheckinNumber();
        [OperationContract]
        void SaveCheckin(Checkin checkin);
        [OperationContract]
        List<Checkin> RetrieveCheckins();
        [OperationContract]
        Checkin RetrieveCheckin(int checkinNumber);
        [OperationContract]
        void DeleteCheckin(int checkinNumber);
        [OperationContract]
        void UpdateCheckinItem(CheckinItem item, int checkinNumber);
        #endregion

        #region Accounts
        [OperationContract]
        List<Account> RetrieveAccounts();
        [OperationContract]
        List<Transaction> RetrieveAccountTransactions(string accountName);
        [OperationContract]
        int RetrieveNextTransactionNumber();
        [OperationContract]
        void SaveTransaction(Transaction t);
        [OperationContract]
        void UpdateAccountBalance(int accountID, decimal newBalance);
        [OperationContract]
        AuthContainer<object> VoidTransaction(int TransactionID, AuthKey key);
        #endregion

        #region Devices
        [OperationContract]
        bool DeviceExists(string address);
        [OperationContract]
        List<Device> RetrieveTerminals();
        [OperationContract]
        List<Device> RetrieveDevices();
        [OperationContract]
        bool RegisterDevice(Device device);
        [OperationContract]
        bool DeleteDevice(Device device);
        [OperationContract]
        bool AssignDevice(Device terminal, Device device);
        [OperationContract]
        bool RemoveDeviceAssignment(Device terminal, Device device);
        [OperationContract]
        AuthContainer<string> TestIngenicoRequest(IngenicoRequest request, AuthKey key);
        [OperationContract]
        AuthContainer<Payment> InitiatePayment(decimal paymentAmount, AuthKey key);
        [OperationContract]
        AuthContainer<Payment> InitiateRefund(decimal refundAmount, AuthKey key);
        [OperationContract]
        AuthContainer<string> RequestSignature(AuthKey key);
        [OperationContract]
        AuthContainer<object> PrintReceipt(Invoice inv, AuthKey key);
        
        #endregion
    }
}
