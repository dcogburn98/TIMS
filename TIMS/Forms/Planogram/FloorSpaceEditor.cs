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
    public partial class FloorSpaceEditor : Form
    {
        public FloorSpace floorspace = new FloorSpace();
        Graphics gfx;

        public FloorSpaceEditor()
        {
            InitializeComponent();

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            gfx = Graphics.FromImage(pictureBox1.Image);
            ClearViewer();
            PopulateProperties();
        }

        private void ClearViewer()
        {
            gfx.Clear(Color.GhostWhite);

            for (int v = 0; v < pictureBox1.Width; v += 20)
            {
                gfx.DrawLine(Pens.LightGray, 0, v, pictureBox1.Height, v);
            }
            for (int h = 0; h < pictureBox1.Height; h += 20)
            {
                gfx.DrawLine(Pens.LightGray, h, 0, h, pictureBox1.Height);
            }

            gfx.DrawRectangle(new Pen(Brushes.Black, 4), new Rectangle(new Point(0, 0), new Size(pictureBox1.Width, pictureBox1.Height)));

            gfx.DrawRectangle(new Pen(Brushes.Black, 4), floorspace.floor);
        }

        private bool CheckClickOnPoint(PointF clickPos)
        {
            #region Code for non-quadrilateral floor shapes
            //foreach (PointF point in floorspace.outline)
            //{
            //    if (clickPos.X >= point.X && clickPos.X <= point.X + 7 && clickPos.Y >= point.Y && clickPos.Y <= point.Y + 7)
            //    {
            //        pointSelected = true;
            //        selectedPoint = point;
            //        break;
            //    }
            //    else
            //        pointSelected = false;
            //}
            //return pointSelected;
            #endregion

            return false;
        }

        private void PopulateProperties()
        {
            listBox1.Items.Add("Floor space properties");
            listBox1.Items.Add("Length: " + ArchitecturalMath.FeetFromPoints(floorspace.floor.Height) + "'");
            listBox1.Items.Add("Width: " + ArchitecturalMath.FeetFromPoints(floorspace.floor.Width) + "'");
            listBox1.Items.Add("Floor Area: " +
                (ArchitecturalMath.FeetFromPoints(floorspace.floor.Width) *
                ArchitecturalMath.FeetFromPoints(floorspace.floor.Height)) + "sq ft");

            #region Code for non-quadrilateral floor shapes
            //listBox1.Items.Clear();
            //listBox1.Items.Add("Selected Point Properties:");
            //listBox1.Items.Add("{ X:" + selectedPoint.X + ", Y:" + selectedPoint.Y + " }");
            //listBox1.Items.Add("Connected Points");
            #endregion
        }

        private void FollowMouse(object sender, EventArgs e)
        {
            #region Code for non-quadrilateral floor shapes
            //selectedPoint = pictureBox1.PointToClient(Cursor.Position);
            //selectedPoint.X -= 3;
            //selectedPoint.Y -= 3;
            //pictureBox1.Refresh();
            #endregion
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            #region Code for non-quadrilateral floor shapes
            //ClearViewer();
            //foreach (PointF point in floorspace.outline)
            //{
            //    e.Graphics.FillEllipse(Brushes.DarkBlue, point.X, point.Y, 7, 7);
            //    e.Graphics.DrawLine(Pens.Black, point.X + 3, point.Y + 3,
            //        floorspace.outline[
            //            floorspace.outline.IndexOf(point) == floorspace.outline.Count - 1 ? 
            //            0 : 
            //            floorspace.outline.IndexOf(point) + 1].X + 3,
            //        floorspace.outline[
            //            floorspace.outline.IndexOf(point) == floorspace.outline.Count - 1 ? 
            //            0 : 
            //            floorspace.outline.IndexOf(point) + 1].Y + 3);
            //}

            //if (pointSelected)
            //    e.Graphics.FillEllipse(Brushes.Red, selectedPoint.X, selectedPoint.Y, 7, 7);

            ////pictureBox1.Refresh();
            #endregion
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            #region Code for non-quadrilateral floor shapes
            //if (comboBox1.SelectedIndex == -1)
            //    return;

            //if (comboBox1.SelectedItem.ToString() == "Add Point")
            //{
            //    floorspace.outline.Add(new PointF(e.X - 3, e.Y - 3));
            //    pointSelected = true;
            //    selectedPoint = new PointF(e.X - 3, e.Y - 3);
            //    PopulateSelectedPointProperties();
            //}
            //else if (comboBox1.SelectedItem.ToString() == "Select Point")
            //{
            //    if (!CheckClickOnPoint(e.Location))
            //    {
            //        listBox1.Items.Clear();
            //        return;
            //    }
            //    PopulateSelectedPointProperties();
            //}
            //else if (comboBox1.SelectedItem.ToString() == "Remove Point")
            //{
            //    if (CheckClickOnPoint(e.Location))
            //    {
            //        listBox1.Items.Clear();
            //        floorspace.outline.Remove(selectedPoint);
            //        pointSelected = false;
            //    }
            //}
            //else if (comboBox1.SelectedItem.ToString() == "Move Point")
            //{
            //    if (!CheckClickOnPoint(e.Location))
            //    {
            //        listBox1.Items.Clear();
            //        return;
            //    }
            //    PopulateSelectedPointProperties();
            //}

            //pictureBox1.Refresh();
            #endregion
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            #region Code for non-quadrilateral floor shapes
            //if (comboBox1.SelectedIndex == -1)
            //    return;

            //if (comboBox1.SelectedItem.ToString() == "Move Point")
            //{
            //    if (!CheckClickOnPoint(e.Location))
            //        return;
            //    pointToMove = selectedPoint;

            //    moveTimer = new Timer();
            //    moveTimer.Interval = 50;
            //    moveTimer.Tick += FollowMouse;
            //    moveTimer.Start();

            //    movingPoint = true;
            //}
            #endregion
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            #region Code for non-quadrilateral floor shapes
            //if (comboBox1.SelectedIndex == -1)
            //    return;

            //if (comboBox1.SelectedItem.ToString() == "Move Point")
            //{
            //    if (!movingPoint)
            //        return;

            //    moveTimer.Stop();


            //    PointF point = floorspace.outline[floorspace.outline.IndexOf(pointToMove)];
            //    point.X = selectedPoint.X;
            //    point.Y = selectedPoint.Y;
            //    floorspace.outline[floorspace.outline.IndexOf(pointToMove)] = point;
            //    pictureBox1.Refresh();

            //    movingPoint = false;
            #endregion
        }
    }
}
