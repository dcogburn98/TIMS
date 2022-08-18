using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel
{
    [DataContract]
    public class CheckinItem
    {
        [DataMember]
        public string itemNumber;
        [DataMember]
        public string productLine;
        [DataMember]
        public decimal shipped;
        [DataMember]
        public decimal received;
        [DataMember]
        public decimal damaged;
        [DataMember]
        public decimal ordered;

        public CheckinItem(string itemNumber, string productLine)
        {
            this.itemNumber = itemNumber;
            this.productLine = productLine;
            shipped = 0;
            received = 0;
            damaged = 0;
            ordered = 0;
        }
    }
}
