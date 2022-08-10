using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel.Planogram.Shelving
{
    public class GondolaShelf
    {
        public int shelfWidth;
        public int shelfDepth;
        public int shelfHeight;
        public bool baseShelf;
        public int shelfTilt;
        public List<Item> shelfItems;


        public int locationCode;

        public GondolaShelf(bool baseShelf)
        {
            this.baseShelf = baseShelf;
            shelfItems = new List<Item>();
        }
    
    
    }
}
