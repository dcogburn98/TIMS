using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMS
{
    public class Checkin
    {
        public int checkinNumber;
        public List<CheckinItem> items;
        public List<PurchaseOrder> orders;

        public Checkin()
        {
            checkinNumber = DatabaseHandler.SqlRetrieveNextCheckinNumber();
            if (checkinNumber == 0)
                checkinNumber = 10001;
            items = new List<CheckinItem>();
            orders = new List<PurchaseOrder>();
        }
    }
}
