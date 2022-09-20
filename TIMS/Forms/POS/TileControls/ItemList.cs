using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TIMSServerModel;
using TIMS.Server;

namespace TIMS.Forms.POS.TileControls
{
    class ItemList
    {
        public List<InvoiceItem> items;
        public int visibleRows;
        
        public Rectangle bounds;
        public Color color;

        public bool mouseDown = false;
        public bool mouseInside = false;
        public bool needsRedraw = false;

        public ItemList(Rectangle bounds, int rows)
        {
            this.bounds = bounds;

            color = Color.White;
            needsRedraw = true;
            visibleRows = rows;
        }

        public void MouseDown()
        {

        }

        public void MouseUp()
        {

        }

        public void MouseEnter()
        {

        }

        public void MouseLeave()
        {

        }

        public void Draw(Graphics gfx)
        {
            gfx.FillRectangle(new SolidBrush(color), bounds);
            gfx.FillRectangle(Brushes.Red, bounds.X + 2, bounds.Y + visibleRows * 18, (bounds.Width / 2) - 4, bounds.Height - (visibleRows * 18));
        }
    }
}
