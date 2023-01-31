using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TIMSServerModel;
using TIMS.Server;

namespace TIMS.Forms.POS
{
    public partial class ItemInformation : Form
    {
        List<Image> itemImages = new List<Image>();
        int imageIndex = 0;

        public ItemInformation(Item item)
        {
            InitializeComponent();
            nextPictureButton.Enabled = false;
            prevPictureButton.Enabled = false;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            if (item.itemPicturePaths == null || item.itemPicturePaths.Count == 0)
            {
                pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Graphics gfx = Graphics.FromImage(pictureBox1.Image);
                gfx.DrawString("No Image Available", System.Drawing.SystemFonts.DialogFont, Brushes.Black, new PointF(20, 20));
            }
            else
            {
                foreach (string path in item.itemPicturePaths)
                {
                    itemImages.Add(Communication.RetrieveProductImage(path));
                }
                if (itemImages.Count > 1)
                    nextPictureButton.Enabled = true;

                pictureBox1.Image = itemImages[0];
            }

            descriptionBrowser.DocumentText = item.longDescription;
            itemNameTB.Text = item.itemName;
            brandLabel.Text = item.brand;
            categoryLabel.Text = item.category;
            departmentLabel.Text = item.department;
            subdepartmentLabel.Text = item.subDepartment;
        }

        private void prevPictureButton_Click(object sender, EventArgs e)
        {
            nextPictureButton.Enabled = true;
            imageIndex--;
            if (imageIndex == 0)
                prevPictureButton.Enabled = false;

            pictureBox1.Image = itemImages[imageIndex];
        }

        private void nextPictureButton_Click(object sender, EventArgs e)
        {
            prevPictureButton.Enabled = true;
            imageIndex++;
            if (itemImages.Count == imageIndex + 1)
                nextPictureButton.Enabled = false;

            pictureBox1.Image = itemImages[imageIndex];
        }
    }
}
