using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel.Planogram.Shelving
{
    public class GondolaRow
    {
        public List<Gondola> gondolas;

        public enum Orientation
        {
            Lengthwise,
            Widthwise
        }
        public Orientation orientation;
        public Point origin;

        public GondolaRow(Point position)
        {
            gondolas = new List<Gondola>();
            origin = position;
            gondolas.Add(new Gondola(origin));
            orientation = Orientation.Widthwise;
        }

        public Gondola AddGondola()
        {
            Point point = new Point(0, 0);
            if (orientation == Orientation.Widthwise)
                point.X -= 1;
            else
                point.Y += 1;
            Gondola g = new Gondola(point);
            gondolas.Add(g);
            return g;
        }
    }
}
