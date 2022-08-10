using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel.Planogram.Shelving
{
    public class Gondola
    {
        public int width;
        public int height;
        public GondolaShelf baseShelf;
        public List<GondolaShelf> shelfPoints;
        public List<PegboardHook> hookPoints;

        public Gondola()
        {
            shelfPoints = new List<GondolaShelf>();
            hookPoints = new List<PegboardHook>();
        }
    }
}
