using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIMSServer
{
    [Serializable]
    public class InvoiceItem
    {
        public string itemNumber;
        public string description;
        public float price;
        public float listPrice;
        public float total;
        public int quantity;
        public string pricingCode;
        public bool serializedItem;
        public string serialNumber;
        public string taxed;
        public string[] codes;

        public InvoiceItem(string itemNumber, string itemDescription, float price, int quantity, string taxed,
             float listPrice = 0.00f, string pricingCode = "!", bool serializedItem = false,
             string serialNumber = "")
        {
            this.itemNumber = itemNumber;
            this.description = itemDescription;
            this.price = price;
            this.quantity = quantity;
            this.taxed = taxed;
            this.listPrice = listPrice;
            this.pricingCode = pricingCode;
            this.serializedItem = serializedItem;
            this.serialNumber = serialNumber;
        }

        public void AddCode(string code)
        {
            List<string> newCodes = codes.ToList();
            newCodes.Add(code);
            codes = newCodes.ToArray();
        }
    }
}
