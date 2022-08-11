﻿using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TIMSServerModel.Planogram.Shelving;

namespace TIMSServerModel.Planogram
{
    [DataContract]
    public class Building
    {
        [DataMember]
        public Rectangle floor;
        [DataMember]
        public int squareFootage;
        [DataMember]
        public int ceilingHeight;

        public List<GondolaRow> shelfRows;
        

        public Building()
        {
            floor = new Rectangle(240, 240, 120, 120);
            shelfRows = new List<GondolaRow>();
            //shelfRows.Add(new GondolaRow());
        }


    }
}
