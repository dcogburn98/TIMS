using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIMSServerModel.Planogram;

namespace TIMSServerModel
{
    public class Item
    {
        //public AuthKey key;
        public string productLine;
        public string itemNumber;
        public string itemName;
        public string manufacturerNumber;
        public string longDescription;
        public string supplier;
        public int groupCode;
        public int velocityCode;
        public int previousYearVelocityCode;
        public int itemsPerContainer;
        public int standardPackage;
        public string department;
        public string subDepartment;
        public string category;
        public string UPC;
        public string SKU;

        public DateTime lastLabelDate;
        public DateTime dateLastSale;
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
        
        public decimal calculatedPrice;
        public decimal listPrice;
        public decimal redPrice;
        public decimal yellowPrice;
        public decimal greenPrice;
        public decimal pinkPrice;
        public decimal bluePrice;
        public decimal replacementCost;
        public decimal averageCost;
        public decimal lastSalePrice;
        public decimal lastLabelPrice;

        public bool taxed;
        public bool ageRestricted;
        public int minimumAge;
        public bool serialized;

        public int locationCode;

        public List<string> itemPicturePaths = new List<string>();
        public string modelPath;
        public ModelSize modelSize;
        public bool hangingItem;
        public string brand;
    }
}
