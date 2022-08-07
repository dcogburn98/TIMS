using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel
{
    [DataContract]
    public class Checkin
    {
        [DataMember]
        public int checkinNumber;
        [DataMember]
        public List<CheckinItem> items;
        [DataMember]
        public List<PurchaseOrder> orders;

        public Checkin(int checkinNumber)
        {
            this.checkinNumber = checkinNumber;
            if (checkinNumber == 0)
                checkinNumber = 10001;
            items = new List<CheckinItem>();
            orders = new List<PurchaseOrder>();
        }
    }
}
