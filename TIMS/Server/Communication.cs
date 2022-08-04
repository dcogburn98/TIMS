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

        #region Customers

        public static Customer CheckCustomerNumber(string custNo)
        {
            return proxy.CheckCustomerNumber(custNo);
        }

        #endregion

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

        public static string RetrievePropertyString(string key)
        {
            return proxy.RetrievePropertyString(key);
        }
    }
}
