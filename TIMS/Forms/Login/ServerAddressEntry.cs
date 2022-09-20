using System;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TIMS.Server;
using TIMSServerModel;

namespace TIMS.Forms.Login
{
    public partial class ServerAddressEntry : Form
    {
        public ServerAddressEntry()
        {
            InitializeComponent();
            CancelButton = button1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!IPAddress.TryParse(textBox1.Text, out IPAddress ip))
            {
                MessageBox.Show("Invalid IP Address. Please enter a correctly formed IP address.");
                textBox1.Focus();
                textBox1.SelectAll();
                return;
            }
            else
            {
                Communication.SetEndpointAddress("http://" + ip.ToString() + ":9999/endpoint");
                try
                {
                    Communication.CheckEmployee("12ascxd900xed");
                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch
                {
                    MessageBox.Show("No response received from server IP address. Please verify IP address is correct and try again.\nRefer to TIMS user manual for more information.");
                    return;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit TIMS?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DialogResult = DialogResult.Cancel;
            }
        }
    }
}
