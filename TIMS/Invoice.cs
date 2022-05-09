using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMS
{
    public class Invoice
    {
        public int invoiceNumber;
        public Customer customer;
        public Employee employee;

        public List<InvoiceItem> items;
        public float subtotal;
        public float taxRate;
        public float taxAmount;
        public float total;
        public Payment[] payments;

        public bool containsAgeRestrictedItem;
        public DateTime customerBirthdate;
        public string attentionLine;
        public string PONumber;
        public string invoiceMessage;

        public bool savedInvoice;
        public DateTime savedInvoiceTime;

        public Invoice()
        {
            items = new List<InvoiceItem>();
        }
    }
}
