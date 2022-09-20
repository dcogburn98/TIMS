using System;
using System.Timers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TIMSServerModel;
using TIMS.Server;

using TIMS.Forms.POS.TileControls;

namespace TIMS.Forms.POS
{
    public partial class TileDisplay : Form
    {
        Bitmap image;
        TileLayout layout;
        public int TileWidth;
        public int TileHeight;

        public Invoice currentInvoice;

        public TileDisplay()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

            layout = new TileLayout();

            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.KeyPreview = true;
            KeyDown += pictureBox1_KeyDown;
            pictureBox1.MouseDown += PictureBox1_MouseDown;
            pictureBox1.MouseUp += PictureBox1_MouseUp;

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            image = new Bitmap(pictureBox1.Image);
            TileWidth = pictureBox1.Width / layout.HTileCount;
            TileHeight = (pictureBox1.Height - layout.MenuBarHeight) / layout.VTileCount;
            layout.itemButtons = new List<ItemButton>();
            for (int x = 0; x != layout.HTileCount; x++)
            {
                for (int y = 0; y != layout.VTileCount; y++)
                {
                    layout.itemButtons.Add(new ItemButton(new Rectangle((x * TileWidth) + 2, (y * TileHeight) + 2 + layout.MenuBarHeight, TileWidth - 4, TileHeight - 4)));
                }
            }
            using (Graphics gfx = Graphics.FromImage(image))
            {
                gfx.Clear(Color.DarkBlue);
            }
            pictureBox1.Invalidate();

            currentInvoice = new Invoice();
            //currentInvoice.customer = Communication.CheckCustomerNumber("0");
        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            foreach (ItemButton b in layout.itemButtons)
            {
                if (b.CheckWithinBounds(PointToClient(MousePosition))) b.MouseUp();
            }
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (ItemButton b in layout.itemButtons)
            {
                if (b.CheckWithinBounds(PointToClient(MousePosition))) b.MouseDown();
            }
        }

        private void pictureBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            e.Handled = true;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            foreach (ItemButton b in layout.itemButtons)
            {
                b.CheckWithinBounds(PointToClient(MousePosition));
            }

            using (Graphics gfx = Graphics.FromImage(image))
            {
                foreach (ItemButton button in layout.itemButtons.Where(el => el.needsRedraw))
                {
                    button.Draw(gfx);
                }

                pictureBox1.Image = image;
            }
        }
    }
}
