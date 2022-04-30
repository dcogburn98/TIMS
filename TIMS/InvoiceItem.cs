using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMS
{
    /// <summary>
    ///// Represents a line item in an invoice
    /// </summary>
    public class InvoiceItem
    {
        public string itemNumber;
        public string productLine;
        public string itemName;
        public string longDescription;
        public float price;
        public float listPrice;
        public float total;
        public float quantity;
        public string pricingCode;
        public bool serializedItem;
        public string serialNumber;
        public string taxed;
        public string[] codes;
        public Guid ID;

        public InvoiceItem(string itemNumber, string itemName, float price, int quantity, string taxed,
             float listPrice = 0.00f, string pricingCode = "!", bool serializedItem = false,
             string serialNumber = "")
        {
            this.itemNumber = itemNumber;
            this.itemName = itemName;
            this.price = price;
            this.quantity = quantity;
            this.taxed = taxed;
            this.listPrice = listPrice;
            this.pricingCode = pricingCode;
            this.serializedItem = serializedItem;
            this.serialNumber = serialNumber;
        }

        public InvoiceItem()
        {

        }

        public void AddCode(string code)
        {
            List<string> newCodes = codes.ToList();
            newCodes.Add(code);
            codes = newCodes.ToArray();
        }
    }
}
