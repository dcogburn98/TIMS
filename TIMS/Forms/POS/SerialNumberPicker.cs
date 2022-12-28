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
using TIMSServerModel;

namespace TIMS.Forms.POS
{
    public partial class SerialNumberPicker : Form
    {
        InvoiceItem invItem;
        public SerialNumberPicker(InvoiceItem invItem)
        {
            InitializeComponent();

            this.invItem = invItem;
            if (!invItem.serializedItem)
                Close();

            List<string> SerialNumbers = Communication.RetrieveItemSerialNumbers(invItem.productLine, invItem.itemNumber);
            if (SerialNumbers != null)
                foreach (string sn in Communication.RetrieveItemSerialNumbers(invItem.productLine, invItem.itemNumber))
                {
                    comboBox1.Items.Add(sn);
                }
            else
            {
                if (MessageBox.Show("No serial numbers are available for this item. Add to invoice anyway?", "Question", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    invItem.serialNumber = "NO SERIAL NUMBER";
                    Close();
                }
                else
                {
                    invItem.serialNumber = "FALSE";
                    Close();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            invItem.serialNumber = comboBox1.Text;
            Close();
        }
    }
}
