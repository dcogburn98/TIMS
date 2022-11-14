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
        }

        private void CompanyControls_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.OpenForms.Remove(this);
            Program.CheckOpenForms();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Image Files|*.png,*.jpg";
            if (file.ShowDialog() != DialogResult.Cancel)
            {
                textBox1.Text = file.FileName;
            }

            Image img = null;
            try
            {
                img = Image.FromFile(Path.GetFileName(file.FileName));
            }
            catch
            {
                MessageBox.Show("Invalid image file!");
            }

            if (img != null)
                Communication.SetImage("Company Logo", img);

            pictureBox1.Image = Communication.RetrieveImage("Company Logo");
        }
    }
}
