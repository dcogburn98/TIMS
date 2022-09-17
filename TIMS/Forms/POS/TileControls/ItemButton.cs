using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TIMSServerModel;

namespace TIMS.Forms.POS.TileControls
{
    public class ItemButton
    {
        public InvoiceItem item;
        public Rectangle bounds;
        public Color color;
        public Color unclickedColor;
        public Color clickedColor;
        public Color hoverColor;

        public bool mouseDown = false;
        public bool mouseInside = false;
        public bool needsRedraw = false;


        public ItemButton(Rectangle Bounds)
        {
            System.Threading.Thread.Sleep(1);
            Random RNG = new Random();
            bounds = Bounds;
            unclickedColor = Color.FromArgb(RNG.Next(0, 255), RNG.Next(0, 255), RNG.Next(0, 255));
            color = unclickedColor;

            int r;
            int g;
            int b;

            #region clickedColor Maker
            if (color.R > 230)
                r = color.R - 20;
            else
                r = color.R + 20;

            if (color.G > 230)
                g = color.G - 20;
            else
                g = color.G + 20;

            if (color.B > 230)
                b = color.B - 20;
            else
                b = color.B + 20;

            clickedColor = Color.FromArgb(r, g, b);
            #endregion
            #region hoverColor Maker
            if (clickedColor.R > 230)
                r = clickedColor.R - 20;
            else
                r = clickedColor.R + 20;

            if (clickedColor.G > 230)
                g = clickedColor.G - 20;
            else
                g = clickedColor.G + 20;

            if (clickedColor.B > 230)
                b = clickedColor.B - 20;
            else
                b = clickedColor.B + 20;

            hoverColor = Color.FromArgb(r, g, b);
            #endregion

            needsRedraw = true;
        }

        public bool CheckWithinBounds(Point p)
        {
            if (p.X > bounds.X && p.X < bounds.X + bounds.Width && p.Y > bounds.Y && p.Y < bounds.Y + bounds.Height)
            {
                if (!mouseInside)
                {
                    MouseEnter();
                }
                return true;
            }
            else
            {
                if (mouseDown)
                    mouseDown = false;
                if (mouseInside)
                    MouseLeave();
                return false;
            }
        }

        public void MouseDown()
        {
            mouseDown = true;
            color = clickedColor;
            needsRedraw = true;
        }

        public void MouseUp()
        {
            if (!mouseDown)
                return;

            mouseDown = false;
            color = hoverColor;
            needsRedraw = true;
        }

        public void MouseEnter()
        {
            mouseInside = true;
            color = hoverColor;
            needsRedraw = true;
        }

        public void MouseLeave()
        {
            mouseDown = false;
            mouseInside = false;
            color = unclickedColor;
            needsRedraw = true;
        }
    
        public void Draw(Graphics gfx)
        {
            gfx.FillRectangle(new SolidBrush(color), bounds);
            needsRedraw = false;
        }
    }
}
