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
        public float minimum;
        public float maximum;
        public float onHandQty;
        public float WIPQty;
        public float onOrderQty;
        public float onBackorderQty;
        public int daysOnOrder;
        public int daysOnBackorder;

        public float listPrice;
        public float redPrice;
        public float yellowPrice;
        public float greenPrice;
        public float pinkPrice;
        public float bluePrice;
        public float replacementCost;
        public float averageCost;
        public bool taxed;
        public bool ageRestricted;
        public int minimumAge;
        public bool serialized;

        public int locationCode;
    }
}
