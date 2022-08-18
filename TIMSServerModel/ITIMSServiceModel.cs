using System;
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
        List<Item> CheckItemNumber(string itemNumber, bool connectionOpened);
        [OperationContract]
        List<Item> CheckItemNumberFromSupplier(string itemNumber, string supplier);
        [OperationContract]
        List<Item> RetrieveItemsFromSupplier(string supplier);
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
        void UpdateItem(Item newItem);
        [OperationContract]
        List<string> RetrieveItemSerialNumbers(string productLine, string itemNumber);
        [OperationContract]
        bool AddItem(Item item);
        [OperationContract]
        List<Item> RetrieveLabelOutOfDateItems();
        [OperationContract]
        void SaveReleasedInvoice(Invoice inv);
        [OperationContract]
        int RetrieveNextInvoiceNumber();
        #endregion

        #region Invoices
        [OperationContract]
        List<Invoice> RetrieveInvoicesByDateRange(DateTime startDate, DateTime endDate, bool connectionOpened = false);
        [OperationContract]
        Invoice RetrieveInvoice(int invNumber);
        [OperationContract]
        List<Invoice> RetrieveInvoicesByCriteria(string[] criteria);
        #endregion

        #region Customers
        [OperationContract]
        Customer CheckCustomerNumber(string custNo);
        #endregion

        #region Global Properties
        [OperationContract]
        string RetrievePropertyString(string key);
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
        int RetrieveNextPONumber();
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
        #endregion
    }
}
