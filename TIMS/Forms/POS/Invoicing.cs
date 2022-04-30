using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TIMS.Forms
{
    public partial class Invoicing : Form
    {
        public Customer currentCustomer;
        public InvoiceItem currentEditingRow;
        public List<InvoiceItem> items;
        public List<InvoiceItem> addingItems;
        public InvoiceItem workingItem;

        bool addingLine;
        bool lineDeleted;

        public enum State
        {
            Startup,
            NoCustomer,
            WaitingItemNumber,
            WaitingProductLine,
            EditingLineItem,
            CancellingInvoice,
        }
        public State currentState;

        public Invoicing()
        {
            currentState = State.Startup;

            InitializeComponent();
            DatabaseHandler.InitializeDatabases();
            StartPosition = FormStartPosition.CenterScreen;
            items = new List<InvoiceItem>();

            cancelItemButton.Visible = false;
            productLineDropBox.Enabled = false;
            extraFunctionsDropBox.SelectedIndex = 0;
            customerInfoLB.Items.Clear();
            employeeNoTB.Text = Program.currentEmployee.employeeNumber.ToString();
            employeeNameLabel.Text = Program.currentEmployee.fullName;
            employeeNoTB.Enabled = false;
            customerNoTB.Enabled = true;
            customerNoTB.Focus();

            currentState = State.NoCustomer;
        }

        private void EnableControls()
        {
            customerNoTB.Enabled = true;
            customerSearchButton.Enabled = true;
            savedInvoiceButton.Enabled = true;
            catalogButton.Enabled = true;
            stockCheckButton.Enabled = true;
            invoiceAttentionTB.Enabled = true;
            openCoresButton.Enabled = true;
            customerNoteTB.Enabled = true;
            dataGridView1.Enabled = true;
            itemNoTB.Enabled = true;
            extraFunctionsDropBox.Enabled = true;
            enterBarcodeButton.Enabled = true;
            messagesButton.Enabled = true;
            roaButton.Enabled = true;
        }
        
        private void CancelInvoice()
        {
            currentCustomer = null;
            currentEditingRow = null;
            items.Clear();
            addingItems = null;
            workingItem = null;

            savedInvoiceButton.Enabled = true;
            catalogButton.Enabled = false;
            stockCheckButton.Enabled = false;
            invoiceAttentionTB.Enabled = false;
            openCoresButton.Enabled = false;
            customerNoteTB.Enabled = false;
            dataGridView1.Enabled = false;
            itemNoTB.Enabled = false;
            extraFunctionsDropBox.Enabled = false;
            enterBarcodeButton.Enabled = false;
            messagesButton.Enabled = false;
            roaButton.Enabled = false;
            customerInfoLB.Items.Clear();

            customerNoTB.Clear();
            customerNoTB.Focus();

            productLineDropBox.Items.Clear();
        }

        private void AddLineItem()
        {
            InvoiceItem newItem = workingItem;
            addingLine = true;
            int index;
            if (currentState == State.WaitingItemNumber)
            {
                index = dataGridView1.Rows.Add();

                newItem.itemName = descriptionTB.Text;
                newItem.quantity = float.Parse(qtyTB.Text);
                newItem.price = float.Parse(priceTB.Text);
                newItem.total = newItem.price * newItem.quantity;
                newItem.ID = Guid.NewGuid();
                items.Add(newItem);
            }
            else
            {
                index = dataGridView1.SelectedRows[0].Index;

                newItem.itemName = descriptionTB.Text;
                newItem.quantity = float.Parse(qtyTB.Text);
                newItem.price = float.Parse(priceTB.Text);
                newItem.total = newItem.price * newItem.quantity;
                newItem.ID = workingItem.ID;
                items.Remove(items.Find(el => el.ID == newItem.ID));
                items.Add(newItem);
            }

            dataGridView1.Rows[index].Cells[10].Value = newItem.ID;
            dataGridView1.Rows[index].Cells[0].Value = newItem.itemNumber;
            dataGridView1.Rows[index].Cells[1].Value = newItem.productLine;
            dataGridView1.Rows[index].Cells[2].Value = newItem.itemName;
            dataGridView1.Rows[index].Cells[3].Value = newItem.quantity;
            dataGridView1.Rows[index].Cells[6].Value = newItem.price.ToString("C");
            dataGridView1.Rows[index].Cells[7].Value = newItem.total.ToString("C");
            if (taxedCB.Checked)
                dataGridView1.Rows[index].Cells[8].Value = "Y";
            else
                dataGridView1.Rows[index].Cells[8].Value = "N";

            float subtotal = 0.0f;
            foreach (InvoiceItem i in items)
            {
                subtotal += i.total;
            }
            subtotalLabel.Text = subtotal.ToString("C");

            cancelItemButton.Enabled = false;
            cancelItemButton.Visible = false;
            changeSNButton.Enabled = false;
            acceptItemButton.Enabled = false;
            taxedCB.Enabled = false;
            priceTB.Enabled = false;
            priceTB.Clear();
            priceCodeTB.Enabled = false;
            priceCodeTB.Clear();
            qtyTB.Enabled = false;
            qtyTB.Clear();
            descriptionTB.Enabled = false;
            descriptionTB.Clear();
            itemNoTB.Focus();
            itemNoTB.ReadOnly = false;
            itemNoTB.Clear();
            extraFunctionsDropBox.Enabled = true;
            dataGridView1.Enabled = true;

            dataGridView1.ClearSelection();

            if (currentState == State.EditingLineItem)
            {
                itemNoTB.Enabled = true;
                itemNoTB.ReadOnly = false;
                itemNoTB.Focus();
            }

            currentState = State.WaitingItemNumber;
            addingLine = false;
        }

        private void SelectProductLine()
        {
            string selectedLine = productLineDropBox.Text;
            workingItem = addingItems.Find(el => el.productLine == selectedLine);

            descriptionTB.Enabled = false;
            descriptionTB.Text = workingItem.itemName;

            qtyTB.Enabled = true;
            qtyTB.Text = "1";
            qtyTB.Focus();
            qtyTB.SelectAll();

            priceCodeTB.Enabled = true;
            priceCodeTB.Text = "!";

            priceTB.Enabled = true;
            priceTB.Text = workingItem.price.ToString();

            taxedCB.Enabled = true;

            acceptItemButton.Enabled = true;

            cancelItemButton.Enabled = true;
            cancelItemButton.Visible = true;
        }

        private void Checkout()
        {

        }



        private void extraFunctionsDropBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (extraFunctionsDropBox.SelectedIndex == 1)
            {
                DialogResult ans = MessageBox.Show("Are you sure you want to cancel this invoice?", "Warning", MessageBoxButtons.YesNo);
                if (ans == DialogResult.No)
                {
                    extraFunctionsDropBox.SelectedIndex = 0;
                    return;
                }
                currentState = State.CancellingInvoice;
                dataGridView1.Rows.Clear();
                CancelInvoice();
            }
            extraFunctionsDropBox.SelectedIndex = 0;
        }

        private void itemNoTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.LShiftKey && e.KeyCode == Keys.Tab)
                extraFunctionsDropBox.Focus();

            if (e.KeyCode != Keys.Enter || itemNoTB.Text == "")
                return;

            currentState = State.WaitingItemNumber;
            //extraFunctionsDropBox.Enabled = false;

            addingItems = DatabaseHandler.CheckItemNumber(itemNoTB.Text);
            

            if (addingItems == null)
            {
                MessageBox.Show("Invalid Part Number!");
                itemNoTB.Clear();
                return;
            }
            else
            {
                productLineDropBox.Enabled = true;
                foreach (InvoiceItem item in addingItems)
                    productLineDropBox.Items.Add(item.productLine);

                if (addingItems.Count == 1)
                {
                    //descriptionTB.Enabled = true;
                    //qtyTB.Enabled = true;
                    //priceCodeTB.Enabled = true;
                    //priceTB.Enabled = true;
                    //taxedCB.Enabled = true;
                    //acceptItemButton.Enabled = true;
                    //qtyTB.Text = "1";
                    //qtyTB.Focus();
                    //descriptionTB.Text = addingItems.First().itemName;
                    //priceTB.Text = addingItems.First().price.ToString("0.00");
                    //itemNoTB.ReadOnly = true;
                    //descriptionTB.ReadOnly = true;
                    //priceCodeTB.Text = "!";
                    //taxedCB.Checked = true;
                    //cancelItemButton.Visible = true;
                    //cancelItemButton.Enabled = true;
                    //cancelItemButton.Text = "Cancel";
                    productLineDropBox.SelectedIndex = 0;
                    SelectProductLine();
                }
                else
                {
                    productLineDropBox.Enabled = true;
                    productLineDropBox.Focus();
                    productLineDropBox.DroppedDown = true;
                }
            }
            
        }

        private void qtyTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                acceptItemButton.Focus();
        }

        private void acceptItemButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            AddLineItem();
        }

        private void acceptItemButton_Click(object sender, EventArgs e)
        {
            AddLineItem();
        }

        private void customerNoTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            currentCustomer = DatabaseHandler.CheckCustomerNumber(customerNoTB.Text);
            if (currentCustomer == null)
            {
                MessageBox.Show("Invalid customer number!");
                customerNoTB.SelectAll();
                return;
            }

            customerNoTB.Text = currentCustomer.customerNumber + " " + currentCustomer.customerName;
            EnableControls();
            itemNoTB.Focus();
        }

        private void dataGridView1_Paint(object sender, PaintEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
                checkoutButton.Enabled = true;
            else
                checkoutButton.Enabled = false;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (addingLine)
                return;
            if (lineDeleted)
            {
                lineDeleted = false;
                return;
            }
            dataGridView1.Enabled = false;

            currentState = State.EditingLineItem;
            currentEditingRow = items.Find(el => el.ID == (Guid)dataGridView1.SelectedRows[0].Cells[10].Value);

            itemNoTB.Text =         currentEditingRow.itemNumber;
            descriptionTB.Text =    currentEditingRow.itemName;
            qtyTB.Text =            currentEditingRow.quantity.ToString();
            priceCodeTB.Text =      "!";
            priceTB.Text =          currentEditingRow.price.ToString();
            if (currentEditingRow.taxed == "Y")
                taxedCB.Checked = true;
            else if (currentEditingRow.taxed== "N")
                taxedCB.Checked = false;

            itemNoTB.Enabled = false;
            descriptionTB.Enabled = false;
            qtyTB.Enabled = true;
            priceCodeTB.Enabled = true;
            priceTB.Enabled = true;
            taxedCB.Enabled = true;
            qtyTB.Focus();
            qtyTB.SelectAll();
            acceptItemButton.Enabled = true;
            cancelItemButton.Visible = true;
            cancelItemButton.Enabled = true;
            cancelItemButton.Text = "Delete";
        }

        private void addCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddCustomer addCustomerForm = new AddCustomer();
            addCustomerForm.Show();
        }

        private void checkoutButton_Click(object sender, EventArgs e)
        {
            Checkout checkoutForm = new Checkout();
            checkoutForm.ShowDialog();
        }

        private void Invoicing_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.OpenForms.Remove(this);
            Program.CheckOpenForms();
        }

        private void editEmployeePermissionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PermissionsEditor pe = new PermissionsEditor();
            pe.ShowDialog();
        }

        private void alternateFunctionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form otherFunctionsForm = Program.OpenForms.Find(el => el is OtherFunctions);
            if (otherFunctionsForm == null)
                Program.LaunchAlternateFunctions();
            else otherFunctionsForm.BringToFront();
        }

        private void extraFunctionsDropBox_Enter(object sender, EventArgs e)
        {
            extraFunctionsDropBox.DroppedDown = true;
        }

        private void productLineDropBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab || e.KeyData == Keys.Tab)
            {
                if (!productLineDropBox.Items.Contains(productLineDropBox.Text))
                {
                    MessageBox.Show("No such item " + itemNoTB.Text + " exists in product line " + productLineDropBox.Text + "\n\n" + "Would you like to add it?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }
            }

            else if (e.KeyCode == Keys.Enter)
            {
                if (int.TryParse(productLineDropBox.Text, out int i))
                {
                    if (i > productLineDropBox.Items.Count || i == 0)
                        return;
                    else
                    {
                        productLineDropBox.SelectedIndex = i-1;
                        SelectProductLine();
                    }
                }
            }
            else
                return;
        }

        private void productLineDropBox_TextUpdate(object sender, EventArgs e)
        {
        }

        private void productLineDropBox_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void itemNoTB_Enter(object sender, EventArgs e)
        {
            itemNoTB.Clear();
            productLineDropBox.Items.Clear();
            productLineDropBox.Text = "";
            productLineDropBox.Enabled = false;
            descriptionTB.Clear();
            qtyTB.Clear();
            priceTB.Clear();
            priceCodeTB.Clear();
            taxedCB.Checked = false;
            acceptItemButton.Enabled = false;
            cancelItemButton.Visible = false;

            if (items.Count == 0)
            {
                checkoutButton.Enabled = false;
            }

            taxedCB.Enabled = false;
            qtyTB.Enabled = false;
            priceCodeTB.Enabled = false;
            priceTB.Enabled = false;
            itemNoTB.Enabled = true;
            currentState = State.WaitingItemNumber;
            itemNoTB.Focus();
        }

        private void cancelItemButton_Click(object sender, EventArgs e)
        {
            if (currentState == State.WaitingItemNumber)
            {
                cancelItemButton.Enabled = false;
                cancelItemButton.Visible = false;
                changeSNButton.Enabled = false;
                acceptItemButton.Enabled = false;
                taxedCB.Enabled = false;
                priceTB.Enabled = false;
                priceTB.Clear();
                priceCodeTB.Enabled = false;
                priceCodeTB.Clear();
                qtyTB.Enabled = false;
                qtyTB.Clear();
                descriptionTB.Enabled = false;
                descriptionTB.Clear();
                itemNoTB.Focus();
                itemNoTB.ReadOnly = false;
                itemNoTB.Clear();
            }
            if (currentState == State.EditingLineItem)
            {
                lineDeleted = true;
                items.Remove(items.Find(el => el.ID == (Guid)dataGridView1.SelectedRows[0].Cells["guid"].Value));
                dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
                cancelItemButton.Enabled = false;
                cancelItemButton.Visible = false;
                changeSNButton.Enabled = false;
                acceptItemButton.Enabled = false;
                taxedCB.Enabled = false;
                priceTB.Enabled = false;
                priceTB.Clear();
                priceCodeTB.Enabled = false;
                priceCodeTB.Clear();
                qtyTB.Enabled = false;
                qtyTB.Clear();
                descriptionTB.Enabled = false;
                descriptionTB.Clear();
                itemNoTB.Enabled = true;
                itemNoTB.Focus();
                itemNoTB.ReadOnly = false;
                itemNoTB.Clear();

                float total = 0.00f;
                foreach (InvoiceItem i in items)
                {
                    total += i.total;
                }
                subtotalLabel.Text = total.ToString("C");
                dataGridView1.Enabled = true;
            }
        }

        private void qtyTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void priceTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void priceTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                acceptItemButton.Focus();
        }
    }
}
