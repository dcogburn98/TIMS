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

namespace TIMS.Forms.Maintenance
{
    public partial class BarcodeMaintenance : Form
    {
        public BarcodeMaintenance()
        {
            InitializeComponent();

            CancelButton = closeButton;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(barcodeTB.Text))
            {
                MessageBox.Show("Invalid barcode! Please enter or scan a valid barcode to be assigned to.");
                return;
            }
            if (string.IsNullOrEmpty(productLineTB.Text) || !Communication.CheckProductLine(productLineTB.Text))
            {
                MessageBox.Show("Invalid product line! Please enter a valid product line.");
                return;
            }
            if (string.IsNullOrEmpty(itemNumberTB.Text))
            {
                MessageBox.Show("Invalid product number! Please enter a valid item number.");
                return;
            }
            if (string.IsNullOrEmpty(qtyTB.Text) || !decimal.TryParse(qtyTB.Text, out decimal qty))
            {
                MessageBox.Show("Invalid quantity! Please enter a valid quantity.");
                return;
            }

            if (Communication.RetrieveItemFromBarcode(barcodeTB.Text) != null) //If the item exists already in the TIMS database
            {
                InvoiceItem barcode = new InvoiceItem() { itemNumber = itemNumberTB.Text, productLine = productLineTB.Text, quantity = decimal.Parse(qtyTB.Text) };
                Communication.UpdateBarcode(barcodeTB.Text, barcode);
                MessageBox.Show("Barcode updated!");
            }
            else
            {
                Communication.AddBarcode(itemNumberTB.Text, productLineTB.Text, barcodeTB.Text, decimal.Parse(qtyTB.Text));
                MessageBox.Show("Barcode added to the database!");
            }
        }

        private void barcodeTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }
            

            InvoiceItem scannedItem = Communication.RetrieveInvoiceItemFromBarcode(barcodeTB.Text.Trim('@'));
            if (scannedItem != null)
            {
                productLineTB.Text = scannedItem.productLine;
                itemNumberTB.Text = scannedItem.itemNumber;
                qtyTB.Text = scannedItem.quantity.ToString();
            }
            else
            {
                productLineTB.Focus();
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
