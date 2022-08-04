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

            foreach (string sn in DatabaseHandler.SqlRetrieveItemSerialNumbers(invItem.productLine, invItem.itemNumber))
            {
                comboBox1.Items.Add(sn);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            invItem.serialNumber = comboBox1.Text;
            Close();
        }
    }
}
