using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TIMSServerModel.Planogram;

namespace TIMS.Forms.Planogram
{
    public partial class BuildingEditor : Form
    {
        public Building floorspace = new Building();
        bool isAnimatingScale = false;

        public BuildingEditor()
        {
            InitializeComponent();

            widthNumeric.Value = floorspace.floor.Width;
            depthNumeric.Value = floorspace.floor.Height;
            ceilingHeightNumeric.Value = floorspace.ceilingHeight;

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics gfx = e.Graphics;

            for (int v = 0; v < pictureBox1.Width; v += 20)
            {
                gfx.DrawLine(Pens.LightGray, 0, v, pictureBox1.Height, v);
            }
            for (int h = 0; h < pictureBox1.Height; h += 20)
            {
                gfx.DrawLine(Pens.LightGray, h, 0, h, pictureBox1.Height);
            }

            gfx.DrawRectangle(new Pen(Brushes.Black, 4), new Rectangle(new Point(0, 0), new Size(pictureBox1.Width, pictureBox1.Height)));

            int widthPixels = floorspace.floor.Width * 3;
            int depthPixels = floorspace.floor.Height * 3;

            gfx.DrawRectangle(new Pen(Brushes.Black, 4), 
                (pictureBox1.Width - widthPixels) / 2, 
                (pictureBox1.Height - depthPixels) / 2,
                widthPixels,
                depthPixels);

            if (isAnimatingScale)
                doAnimation();
        }

        private void widthNumeric_ValueChanged(object sender, EventArgs e)
        {
            floorspace.floor.Width = (int)widthNumeric.Value;
            pictureBox1.Refresh();
        }

        private void depthNumeric_ValueChanged(object sender, EventArgs e)
        {
            floorspace.floor.Height = (int)depthNumeric.Value;
            pictureBox1.Refresh();
        }

        private void ceilingHeightNumeric_ValueChanged(object sender, EventArgs e)
        {
            floorspace.ceilingHeight = (int)ceilingHeightNumeric.Value;
            pictureBox1.Refresh();
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            doAnimation();
        }

        private void doAnimation()
        {
            if (!isAnimatingScale)
                isAnimatingScale = true;

            if (floorspace.floor.Width*3 >= pictureBox1.Width || floorspace.floor.Height*3 >= pictureBox1.Height)
            {
                isAnimatingScale = false;
            }
            else
            {
                floorspace.floor.Width++;
                floorspace.floor.Height++;
            }

            pictureBox1.Invalidate();
        }
    }
}
