﻿using System;
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
        public bool ageRestricted;
        public int minimumAge;
        public bool taxed;
        public string[] codes;
        public Guid ID;

        public InvoiceItem(string itemNumber, string itemName, float price, int quantity, bool taxed,
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
        
        public InvoiceItem(Item item)
        {
            itemNumber = item.itemNumber;
            productLine = item.productLine;
            itemName = item.itemName;
            longDescription = item.description;
            price = item.greenPrice;
            listPrice = item.listPrice;
            ageRestricted = item.ageRestricted;
            minimumAge = item.minimumAge;
            taxed = item.taxed;
        }

        public void AddCode(string code)
        {
            List<string> newCodes = codes.ToList();
            newCodes.Add(code);
            codes = newCodes.ToArray();
        }

    }
}
