using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel.Planogram
{
    [DataContract]
    public class Building
    {
        [DataMember]
        public Rectangle floor;
        [DataMember]
        public int squareFootage;
        public int ceilingHeight;
        

        public Building()
        {
            floor = new Rectangle(240, 240, 120, 120);
        }


    }
}
