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

        private void AddBarcode()
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

        private void button1_Click(object sender, EventArgs e)
        {
            AddBarcode();
        }

        private void barcodeTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            barcodeTB.Text = barcodeTB.Text.Trim('@');
            
            InvoiceItem scannedItem = Communication.RetrieveInvoiceItemFromBarcode(barcodeTB.Text.Trim('@'));
            if (scannedItem != null)
            {
                productLineTB.Text = scannedItem.productLine;
                itemNumberTB.Text = scannedItem.itemNumber;
                qtyTB.Text = scannedItem.quantity.ToString();
                barcodeTB.SelectAll();
            }
            else
            {
                productLineTB.Text = "";
                itemNumberTB.Text = "";
                productLineTB.Focus();
                qtyTB.Text = "1";
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void productLineTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar))
            {
                e.KeyChar = char.ToUpper(e.KeyChar);
            }
        }

        private void itemNumberTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar))
            {
                e.KeyChar = char.ToUpper(e.KeyChar);
            }
        }

        private void qtyTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddBarcode();
            }
        }

        private void productLineTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                itemNumberTB.Focus();
            }
        }

        private void itemNumberTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                qtyTB.Focus();
            }
        }
    }
}
