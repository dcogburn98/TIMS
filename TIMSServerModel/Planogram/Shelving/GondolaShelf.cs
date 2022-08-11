using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel.Planogram.Shelving
{
    public class GondolaShelf
    {
        public enum ShelfWidths
        {
            in24,
            in36,
            in48
        }
        public ShelfWidths shelfWidth;

        public enum ShelfDepths
        {
            in12,
            in16,
            in18,
            in20,
            in24,
        }
        public ShelfDepths shelfDepth;

        public int uprightPoint;
        public int shelfTilt;

        public List<Item> shelfItems;
        public int locationCode;

        public GondolaShelf(Gondola parent)
        {
            shelfItems = new List<Item>();
            shelfWidth = parent.width;
            shelfDepth = ShelfDepths.in18;
            uprightPoint = 6;
            shelfTilt = 0;
            locationCode = 0;
        }
    
    
    }
}
