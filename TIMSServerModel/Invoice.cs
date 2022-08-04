using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PdfSharp.Drawing;

namespace TIMSServerModel
{
    public class Invoice
    {
        public int invoiceNumber;
        public Customer customer;
        public Employee employee;

        public List<InvoiceItem> items;
        public decimal subtotal;
        public decimal taxableTotal;
        public decimal taxRate;
        public decimal taxAmount;
        public decimal total;
        public decimal cost;
        public decimal profit;
        public List<Payment> payments;
        public decimal totalPayments;

        public bool containsAgeRestrictedItem;
        public DateTime customerBirthdate;
        public string attentionLine = string.Empty;
        public string PONumber = string.Empty;
        public string invoiceMessage = string.Empty;

        public bool savedInvoice;
        public DateTime savedInvoiceTime;
        public DateTime invoiceCreationTime;
        public DateTime invoiceFinalizedTime;

        public bool finalized;
        public bool voided;

        public int invoicePages;
        public int currentPage;

        public Invoice()
        {
            items = new List<InvoiceItem>();
            payments = new List<Payment>();
        }
    }
}
