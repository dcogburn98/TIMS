﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMS
{
    class Item
    {
        public string lineCode;
        public string itemNumber;
        public string description;
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

        public int locationCode;
    }
}
