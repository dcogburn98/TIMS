using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TIMS.Forms.Orders
{
    public partial class OrderCreator : Form
    {
        public string supplier = string.Empty;
        public string criteria = string.Empty;
        public InvoiceItem workingItem;
        public OrderCreator(string supplier, string criteria)
        {
            InitializeComponent();
            CancelButton = button3;
            this.supplier = supplier;
            this.criteria = criteria;
            if (supplier == "Manual Order")
            {
                this.supplier = string.Empty;
                supplierLabel.Text = "Supplier: All";
            }
            else
                supplierLabel.Text = "Supplier: " + supplier;

            criteriaLabel.Text = "Criteria: " + criteria;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            productLineCB.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count != 0)
                if (MessageBox.Show("Are you sure you want to leave order editing?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    Close();
        }

        private void productLineCB_Enter(object sender, EventArgs e)
        {
            productLineCB.Items.Clear();
            List<Item> items;
            if (supplier == string.Empty)
                items = DatabaseHandler.SqlCheckItemNumber(itemNumberTB.Text, false);
            else
                items = DatabaseHandler.SqlCheckItemNumber(itemNumberTB.Text, supplier);

            if (items == null)
            {
                MessageBox.Show("Invalid item number!");
                itemNumberTB.Focus();
                itemNumberTB.SelectAll();
                return;
            }

            if (items.Count == 1)
            {
                productLineCB.Items.Add(items[0].productLine.ToUpper());
                productLineCB.SelectedIndex = 0;
                qtyTB.Focus();
                qtyTB.Text = "1";
                qtyTB.SelectAll();
            }
            else
            {
                foreach (Item i in items)
                {
                    productLineCB.Items.Add(i.productLine.ToUpper());
                }
                productLineCB.DroppedDown = true;
            }
            
        }

        private void qtyTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }

            // only allow one minus sign
            if ((e.KeyChar == '-') && ((sender as TextBox).Text.IndexOf('-') > -1))
            {
                e.Handled = true;
            }
        }

        private void productLineCB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
                return;

            if (int.Parse(e.KeyChar.ToString()) > productLineCB.Items.Count || int.Parse(e.KeyChar.ToString()) == 0)
                return;

            productLineCB.SelectedIndex = int.Parse(e.KeyChar.ToString()) - 1;
        }

        private void productLineCB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || productLineCB.SelectedIndex == -1)
                return;

            qtyTB.Focus();
        }

        private void addItemButton_Click(object sender, EventArgs e)
        {
            if (productLineCB.SelectedIndex == -1 || productLineCB.Items.Count < 1)
            {
                MessageBox.Show("Invalid item!");
                return;
            }

            workingItem = new InvoiceItem(DatabaseHandler.SqlRetrieveItem(itemNumberTB.Text, productLineCB.Text));

            int row = dataGridView1.Rows.Add();
            dataGridView1.Rows[row].Cells[0].Value = workingItem.itemNumber;
            dataGridView1.Rows[row].Cells[1].Value = workingItem.productLine;
        }
    }
}
