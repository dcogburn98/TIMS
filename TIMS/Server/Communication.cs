using System;
using System.Collections.Generic;
using System.ServiceModel;

using PdfSharp.Drawing;

using TIMSServerModel;

namespace TIMS.Server
{
    class Communication
    {
        private static ChannelFactory<ITIMSServiceModel> channelFactory = new
            ChannelFactory<ITIMSServiceModel>("TIMSServerEndpoint");

        private static ITIMSServiceModel proxy = channelFactory.CreateChannel();

        public static void ChangeEndpointAddress(EndpointAddress newAddress)
        {
            channelFactory = new ChannelFactory<ITIMSServiceModel>("TIMSServerEndpoint", newAddress);
        }


        #region Employees
        public static string CheckEmployee(string input)
        {
            return proxy.CheckEmployee(input);
        }

        public static Employee Login(string user, byte[] pass)
        {
            return proxy.Login(user, pass);
        }

        public static Employee RetrieveEmployee(string employeeNumber)
        {
            return proxy.RetrieveEmployee(employeeNumber);
        }
        #endregion

        #region Items
        public static List<Item> CheckItemNumber(string itemNumber, bool connectionOpened)
        {
            return proxy.CheckItemNumber(itemNumber, connectionOpened);
        }

        public static List<Item> CheckItemNumber(string itemNumber, string supplier)
        {
            return proxy.CheckItemNumberFromSupplier(itemNumber, supplier);
        }

        public static List<Item> RetrieveItemsFromSupplier(string supplier)
        {
            return proxy.RetrieveItemsFromSupplier(supplier);
        }

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
        public static void SaveReleasedInvoice(Invoice inv)
        {
            proxy.SaveReleasedInvoice(inv);
        }
        public static int RetrieveNextInvoiceNumber()
        {
            return proxy.RetrieveNextInvoiceNumber();
        }

        #endregion

        #region Customers

        public static Customer CheckCustomerNumber(string custNo)
        {
            return proxy.CheckCustomerNumber(custNo);
        }

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
        #endregion

        #region Global Properties
        public static string RetrievePropertyString(string key)
        {
            return proxy.RetrievePropertyString(key);
        }
        #endregion

        #region Item Shortcut Menus
        internal static List<ItemShortcutMenu> RetrieveShortcutMenus()
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
        public static void SaveTransaction(Transaction t)
        {
            proxy.SaveTransaction(t);
        }
        #endregion
    }
}
