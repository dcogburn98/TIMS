using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TIMS.Server;

namespace TIMS.Forms.Settings
{
    public partial class DeviceAssignments : Form
    {
        public DeviceAssignments()
        {
            InitializeComponent();

            
        }

        private void addReceiptPrinter_Click(object sender, EventArgs e)
        {
            AddDevice adder = new AddDevice("Receipt Printer");
            if (adder.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Device added.");
            }
            else
            {
                MessageBox.Show("Device NOT added.");
            }
        }
    }
}
