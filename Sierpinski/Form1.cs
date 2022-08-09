using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sierpinski
{
    public partial class Form1 : Form
    {
        Random rng = new Random();
        Timer timer = new Timer();

        List<PointF> initialPoints = new List<PointF>();
        PointF lastPoint;

        public Form1()
        {
            InitializeComponent();

            Graphics gfx = pictureBox1.CreateGraphics();

            initialPoints.Add(new PointF(0.5f * pictureBox1.Width, 0));
            initialPoints.Add(new PointF(0, pictureBox1.Height));
            initialPoints.Add(new PointF(pictureBox1.Width, pictureBox1.Height));

            timer.Tick += AddPoint;
            timer.Interval = 10;
            timer.Start();

            gfx.Clear(Color.Black);

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            pictureBox1.Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            using (Bitmap image = new Bitmap(pictureBox1.Image))
            {

                using (Graphics gfx = Graphics.FromImage(image))
                {
                    gfx.FillEllipse(Brushes.Black, initialPoints[0].X, initialPoints[0].Y, 2, 2);
                    gfx.FillEllipse(Brushes.Black, initialPoints[1].X, initialPoints[1].Y, 2, 2);
                    gfx.FillEllipse(Brushes.Black, initialPoints[2].X, initialPoints[2].Y, 2, 2);

                    PointF newPoint = CalculateMidpoint(lastPoint, initialPoints[rng.Next(0, 3)]);
                    gfx.FillEllipse(Brushes.Black, newPoint.X, newPoint.Y, 2, 2);
                    lastPoint = newPoint;
                }
            
            pictureBox1.Image = image;
            }
            //pictureBox1.Refresh();
        }
    
        private void AddPoint(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }

        private PointF CalculateMidpoint(PointF p1, PointF p2)
        {
            return new PointF((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
        }
    }
}
