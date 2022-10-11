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

namespace TIMS.Forms.Customers
{
    public partial class CustomerInformation : Form
    {
        public Customer currentCustomer;
        public bool customerEdited;

        public CustomerInformation()
        {
            InitializeComponent();
            customerEdited = false;

            dataGridView5.Rows[dataGridView5.Rows.Add()].Cells[0].Value = "Net Sales";
            dataGridView5.Rows[dataGridView5.Rows.Add()].Cells[0].Value = "Net Cost";
            dataGridView5.Rows[dataGridView5.Rows.Add()].Cells[0].Value = "Gross Profit";
            dataGridView5.Rows[dataGridView5.Rows.Add()].Cells[0].Value = "Returns";
            dataGridView5.Rows[dataGridView5.Rows.Add()].Cells[0].Value = "Returns % of Sale";
            dataGridView5.Rows[dataGridView5.Rows.Add()].Cells[0].Value = "Invoice Count";
            dataGridView5.Rows[dataGridView5.Rows.Add()].Cells[0].Value = "Net Delivered Sales";
            dataGridView5.Rows[dataGridView5.Rows.Add()].Cells[0].Value = "Net Delivered Cost";
            dataGridView5.Rows[dataGridView5.Rows.Add()].Cells[0].Value = "Delivered Gross Profit";
            dataGridView5.Rows[dataGridView5.Rows.Add()].Cells[0].Value = "Defective Returns";
            dataGridView5.Rows[dataGridView5.Rows.Add()].Cells[0].Value = "Un-Needed Returns";
            dataGridView5.Rows[dataGridView5.Rows.Add()].Cells[0].Value = "Special Order Sales";
            dataGridView5.Rows[dataGridView5.Rows.Add()].Cells[0].Value = "Net Special Order Cost";
            dataGridView5.Rows[dataGridView5.Rows.Add()].Cells[0].Value = "Special Order Gross Profit";

            accountNumberTB.Focus();
        }

        private void accountNumberTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!Program.IsStringNumeric(accountNumberTB.Text))
                {
                    MessageBox.Show("Invalid customer number!");
                    return;
                }
                currentCustomer = Communication.CheckCustomerNumber(accountNumberTB.Text);
                if (currentCustomer == null)
                {
                    if (MessageBox.Show("Account does not exist. Would you like to create it?", "Customer does not exist!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        MessageBox.Show("This doesn't work yet. The developer forgot about this part.\n\nIt's pretty important, how could he have overlooked this?\n\nWould you please send him a strongly worded message and tell him he's a terrible dev?");
                    }
                }
            }
        }

        private void CustomerInformation_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (customerEdited)
            {
                if (MessageBox.Show("There are unsaved changes made to the current customer!\n\nThese changes will be discarded if you continue.", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }
    }
}
