using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel
{
    public class InvoiceItem
    {
        public string itemNumber;
        public string productLine;
        public string itemName;
        public string longDescription;
        public decimal price;
        public decimal listPrice;
        public decimal total;
        public decimal cost;
        public decimal quantity;
        public string pricingCode = "!";
        public bool serializedItem;
        public string serialNumber;
        public bool ageRestricted;
        public int minimumAge;
        public bool taxed;
        public string[] codes;
        public Guid ID;

        public InvoiceItem()
        {

        }

        public InvoiceItem(Item item)
        {
            itemNumber = item.itemNumber;
            productLine = item.productLine;
            itemName = item.itemName;
            longDescription = item.longDescription;
            price = item.greenPrice;
            listPrice = item.listPrice;
            ageRestricted = item.ageRestricted;
            minimumAge = item.minimumAge;
            taxed = item.taxed;
            serializedItem = item.serialized;
            cost = item.replacementCost;
        }

        public void AddCode(string code)
        {
            List<string> newCodes = codes.ToList();
            newCodes.Add(code);
            codes = newCodes.ToArray();
        }

    }
}
