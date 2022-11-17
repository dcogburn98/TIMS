using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TIMSServerModel;
using TIMS.Server;

namespace TIMS.Forms.Settings
{
    public partial class CompanyControls : Form
    {
        public CompanyControls()
        {
            InitializeComponent();

            Image img = Communication.RetrieveCompanyLogo();
            if (img != null)
                pictureBox1.Image = img;
        }

        private void CompanyControls_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.OpenForms.Remove(this);
            Program.CheckOpenForms();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Multiselect = false;
            file.Filter = "Image Files(*.PNG;*.BMP;*.JPG;*.GIF)|*.PNG;*.BMP;*.JPG;*.GIF";
            if (file.ShowDialog() != DialogResult.Cancel &&
                MessageBox.Show("This operation will overwrite your existing logo. Continue?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                physicalCityTB.Text = file.FileName;
            }

            //try
            //{
                Image img = Image.FromFile(file.FileName);
                if (img != null)
                    Communication.SetImage("Company Logo", img);

                pictureBox1.Image = Communication.RetrieveImage("Company Logo");
            //}
            //catch
            //{
            //    MessageBox.Show("Invalid image file!");
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
