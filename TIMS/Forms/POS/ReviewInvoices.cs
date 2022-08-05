using System;
using System.Windows.Forms;
using System.Collections.Generic;

using TIMS.Server;
using TIMSServerModel;

namespace TIMS.Forms.POS
{
    public partial class ReviewInvoices : Form
    {
        List<Invoice> invoices = new List<Invoice>();

        public ReviewInvoices()
        {
            InitializeComponent();
            tabControl1.Controls.Remove(tabPage2);
            dateFrom.Value = DateTime.Now.Subtract(new TimeSpan(30, 0, 0, 0));
            formatPicker.DropDownStyle = ComboBoxStyle.DropDownList;
            formatPicker.SelectedIndex = 0;
        }

        private void HandleProductLine()
        {
            if (!Communication.CheckProductLine(productLineTB.Text))
            {
                productLineTB.Clear();
            }
            else
            {
                if (productLineLB.Items.Contains(productLineTB.Text))
                    productLineLB.Items.Remove(productLineTB.Text);
                else
                    productLineLB.Items.Add(productLineTB.Text);

                productLineTB.Clear();
                productLineAllCB.Checked = false;
            }
        }

        private void ExecuteSearch()
        {

        }

        #region Criteria Field Management Methods
        private void dateAllCB_CheckedChanged(object sender, EventArgs e)
        {
            if (dateAllCB.Checked)
            {
                dateFrom.Enabled = false;
                dateTo.Enabled = false;
            }
            else
            {
                dateFrom.Enabled = true;
                dateTo.Enabled = true;
            }
        }

        private void productLineTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            HandleProductLine();
        }

        private void productLineTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
                return;

            if (!char.IsLetter(e.KeyChar) || productLineTB.Text.Length == 3)
            {
                e.Handled = true;
                return;
            }
        }

        private void productLineTB_Leave(object sender, EventArgs e)
        {
            if (productLineTB.Text != String.Empty)
            {
                HandleProductLine();
                productLineTB.Focus();
            }
        }

        private void ReviewInvoices_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.OpenForms.Remove(this);
            Program.CheckOpenForms();
        }

        private void alternateFunctionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form otherFunctionsForm = Program.OpenForms.Find(el => el is OtherFunctions);
            if (otherFunctionsForm == null)
                Program.LaunchAlternateFunctions();
            else otherFunctionsForm.BringToFront();
        }

        private void invoicingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.LaunchInvoicing();
        }

        private void invoiceNoAllCB_CheckedChanged(object sender, EventArgs e)
        {
            if (invoiceNoAllCB.Checked)
            {
                invNoFrom.Clear();
                invNoTo.Clear();
                invoiceNoAllCB.Checked = true;
            }
        }

        private void invNoFrom_TextChanged(object sender, EventArgs e)
        {
            invoiceNoAllCB.Checked = false;
        }

        private void invNoTo_Enter(object sender, EventArgs e)
        {
            if (invNoTo.Text == string.Empty)
            {
                invNoTo.Text = invNoFrom.Text;
            }
        }

        private void customerNoAllCB_CheckedChanged(object sender, EventArgs e)
        {
            if (customerNoAllCB.Checked)
            {
                customerNoFrom.Clear();
                customerNoTo.Clear();
                customerNoAllCB.Checked = true;
            }
        }

        private void customerNoFrom_TextChanged(object sender, EventArgs e)
        {
            customerNoAllCB.Checked = false;
        }

        private void customerNoTo_Enter(object sender, EventArgs e)
        {
            if (customerNoTo.Text == string.Empty)
            {
                customerNoTo.Text = customerNoFrom.Text;
            }
        }

        private void itemNumberTB_TextChanged(object sender, EventArgs e)
        {
            itemNumberAllCB.Checked = false;

            if (itemNumberPrefixTB.Focused)
                return;
            itemNumberPrefixTB.Clear();
        }

        private void itemNumberPrefixTB_TextChanged(object sender, EventArgs e)
        {
            itemNumberAllCB.Checked = false;

            if (itemNumberTB.Focused)
                return;
            itemNumberTB.Clear();
        }

        private void itemNumberAllCB_CheckedChanged(object sender, EventArgs e)
        {
            if (itemNumberAllCB.Checked)
            {
                itemNumberTB.Clear();
                itemNumberPrefixTB.Clear();
                itemNumberAllCB.Checked = true;
            }
        }
        
        private void employeeNumberAllCB_CheckedChanged(object sender, EventArgs e)
        {
            if (employeeNumberAllCB.Checked)
            {
                employeeNoFrom.Clear();
                employeeNoTo.Clear();
                employeeNumberAllCB.Checked = true;
            }
        }

        private void employeeNoFrom_TextChanged(object sender, EventArgs e)
        {
            employeeNumberAllCB.Checked = false;
        }

        private void employeeNoTo_Enter(object sender, EventArgs e)
        {
            if (employeeNoTo.Text == string.Empty)
            {
                employeeNoTo.Text = employeeNoFrom.Text;
            }
        }

        private void POAllCB_CheckedChanged(object sender, EventArgs e)
        {
            if (POAllCB.Checked)
            {
                PONumberFrom.Clear();
                PONumberTo.Clear();
                POAllCB.Checked = true;
            }
        }

        private void PONumberFrom_TextChanged(object sender, EventArgs e)
        {
            POAllCB.Checked = false;
        }

        private void PONumberTo_Enter(object sender, EventArgs e)
        {
            if (PONumberTo.Text == string.Empty)
            {
                PONumberTo.Text = PONumberFrom.Text;
            }
        }

        private void totalAmountAllCB_CheckedChanged(object sender, EventArgs e)
        {
            if (totalAmountAllCB.Checked)
            {
                totalAmountFrom.Clear();
                totalAmountTo.Clear();
                totalAmountAllCB.Checked = true;
            }
        }

        private void totalAmountFrom_TextChanged(object sender, EventArgs e)
        {
            totalAmountAllCB.Checked = false;
        }

        private void totalAmountTo_Enter(object sender, EventArgs e)
        {
            if (totalAmountTo.Text == string.Empty)
            {
                totalAmountTo.Text = totalAmountFrom.Text;
            }
        }
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            //Check for errors in input fields

            invoices.Clear();
            dataGridView1.Rows.Clear();
            invoices = Communication.RetrieveInvoicesByCriteria(new string[] { "test" });
            if (tabControl1.TabPages.Count == 1)
                tabControl1.TabPages.Add(tabPage2);

            foreach (Invoice inv in invoices)
            {
                int row = dataGridView1.Rows.Add();
                dataGridView1.Rows[row].Cells[0].Value = inv.invoiceFinalizedTime.ToString("MM/dd/yyyy hh:mm tt");
                dataGridView1.Rows[row].Cells[1].Value = inv.invoiceNumber;
                dataGridView1.Rows[row].Cells[2].Value = inv.customer.customerNumber + " " + inv.customer.customerName;
                dataGridView1.Rows[row].Cells[3].Value = inv.employee.employeeNumber;
                dataGridView1.Rows[row].Cells[4].Value = inv.total.ToString("C");
                string payments = "";
                foreach (Payment p in inv.payments)
                    payments += p.paymentType.ToString() + ", ";
                payments = payments.Trim(' ');
                payments = payments.Trim(',');
                dataGridView1.Rows[row].Cells[5].Value = payments;
            }
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1)
                return;

            int invNumber = (int)dataGridView1.SelectedRows[0].Cells[1].Value;
            InvoiceViewer viewer = new InvoiceViewer(invoices, dataGridView1.SelectedRows[0].Index);
            Program.OpenForm(viewer);
        }
    }
}
