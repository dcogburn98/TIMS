using System;

namespace TIMS
{
    public class Item
    {
        public string productLine;
        public string itemNumber;
        public string itemName;
        public string longDescription;
        public string supplier;
        public int groupCode;
        public int velocityCode;
        public int previousYearVelocityCode;
        public int itemsPerContainer;
        public int standardPackage;
        public string category;
        public string SKU;

        public DateTime dateStocked;
        public DateTime dateLastReceipt;
        public decimal minimum;
        public decimal maximum;
        public decimal onHandQty;
        public decimal WIPQty;
        public decimal onOrderQty;
        public decimal onBackorderQty;
        public int daysOnOrder;
        public int daysOnBackorder;

        public decimal listPrice;
        public decimal redPrice;
        public decimal yellowPrice;
        public decimal greenPrice;
        public decimal pinkPrice;
        public decimal bluePrice;
        public decimal replacementCost;
        public decimal averageCost;
        public bool taxed;
        public bool ageRestricted;
        public int minimumAge;
        public bool serialized;

        public int locationCode;
    }
}
