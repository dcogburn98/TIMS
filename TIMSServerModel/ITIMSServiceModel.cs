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
        #region Employee and Login Functions
        [OperationContract]
        string CheckEmployee(string input);

        [OperationContract]
        Employee Login(string user, byte[] pass);

        [OperationContract]
        Employee RetrieveEmployee(string employeeNumber);
        #endregion

        #region Item retrieval and manipulation
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

        #region Page Rendering
        [OperationContract]
        void RenderInvoice(XGraphics gfx);
        #endregion
    }
}
