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

        public DateTime dateStocked;
        public DateTime dateLastReceipt;
        public int minimum;
        public int maximum;
        public int onHandQty;
        public int WIPQty;
        public int onOrderQty;
        public int onBackorderQty;
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

        public int locationCode;
    }
}
