using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel.Planogram.Shelving
{
    public class Gondola
    {
        public enum ShelfHeights
        {
            ft5,
            ft6,
            ft8
        }
        public ShelfHeights shelfHeight;

        public GondolaShelf.ShelfWidths width;
        public GondolaShelf baseShelfLeftSide;
        public GondolaShelf baseShelfRightSide;
        public List<GondolaShelf> shelfPointsLeftSide;
        public List<GondolaShelf> shelfPointsRightSide;
        public List<PegboardHook> hookPointsLeftSide;
        public List<PegboardHook> hookPointsRightSide;

        public Point origin;

        public Gondola(Point position)
        {
            shelfPointsLeftSide = new List<GondolaShelf>();
            shelfPointsRightSide = new List<GondolaShelf>();
            hookPointsLeftSide = new List<PegboardHook>();
            hookPointsRightSide = new List<PegboardHook>();
            shelfHeight = ShelfHeights.ft6;
            width = GondolaShelf.ShelfWidths.in48;
            baseShelfLeftSide = new GondolaShelf(this);
            baseShelfRightSide = new GondolaShelf(this);
            shelfPointsLeftSide.Add(new GondolaShelf(this) { uprightPoint = 18 });
            shelfPointsLeftSide.Add(new GondolaShelf(this) { uprightPoint = 30 });
            shelfPointsLeftSide.Add(new GondolaShelf(this) { uprightPoint = 48 });
            shelfPointsLeftSide.Add(new GondolaShelf(this) { uprightPoint = 60 });
            shelfPointsLeftSide.Add(new GondolaShelf(this) { uprightPoint = 72 });
            shelfPointsRightSide.Add(new GondolaShelf(this) { uprightPoint = 18 });
            shelfPointsRightSide.Add(new GondolaShelf(this) { uprightPoint = 30 });
            shelfPointsRightSide.Add(new GondolaShelf(this) { uprightPoint = 48 });
            shelfPointsRightSide.Add(new GondolaShelf(this) { uprightPoint = 60 });
            shelfPointsRightSide.Add(new GondolaShelf(this) { uprightPoint = 72 });
            origin = position;
        }
    }
}
