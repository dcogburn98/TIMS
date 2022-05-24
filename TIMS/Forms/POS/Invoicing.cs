using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TIMS.Forms
{
    public partial class Invoicing : Form
    {
        public Invoice currentInvoice;
        public List<Item> addingItems;
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

        public void CancelInvoice()
        {
            currentInvoice = null;
            addingItems = null;
            workingItem = null;

            itemNoTB.Clear();
            descriptionTB.Clear();
            qtyTB.Clear();
            priceCodeTB.Clear();
            productLineDropBox.Items.Clear();
            productLineDropBox.Text = "";
            priceTB.Clear();
            taxedCB.Checked = false;
            dataGridView1.Rows.Clear();

            productLineDropBox.Enabled = false;
            qtyTB.Enabled = false;
            taxedCB.Enabled = false;
            priceTB.Enabled = false;
            priceCodeTB.Enabled = false;
            acceptItemButton.Enabled = false;
            cancelItemButton.Visible = false;
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
            subtotalLabel.Text = "$0.00";

            customerNoTB.Clear();
            customerNoTB.Focus();

            currentState = State.NoCustomer;
        }

        private void AddCustomer()
        {
            Customer c = DatabaseHandler.SqlCheckCustomerNumber(customerNoTB.Text);
            Invoice newInvoice = new Invoice();

            if (c == null)
            {
                MessageBox.Show("Invalid customer number!");
                customerNoTB.SelectAll();
                return;
            }

            if (currentInvoice != null)
                if (currentInvoice.items.Count > 0)
                    newInvoice = currentInvoice;
                else
                    currentInvoice = newInvoice;
            else
                currentInvoice = newInvoice;

            currentInvoice.customer = c;
            currentInvoice.employee = Program.currentEmployee;
            currentInvoice.invoiceCreationTime = DateTime.Now;

            customerNoTB.Text = currentInvoice.customer.customerNumber + " " + currentInvoice.customer.customerName;
            EnableControls();
            itemNoTB.Focus();

            currentState = State.WaitingItemNumber;
        }

        private void EnterItemNumber()
        {
            if (itemNoTB.Text.Substring(0, 1) == "@")
            {
                InvoiceItem invItem = DatabaseHandler.SqlRetrieveBarcode(itemNoTB.Text);
                if (invItem == null)
                {
                    MessageBox.Show("Barcode does not exist!");
                }
                else
                {
                    workingItem = invItem;
                    workingItem.ID = Guid.NewGuid();

                    itemNoTB.Text = workingItem.itemNumber;
                    descriptionTB.Enabled = false;
                    descriptionTB.Text = workingItem.itemName;

                    qtyTB.Enabled = true;
                    qtyTB.Text = workingItem.quantity.ToString();
                    qtyTB.Focus();
                    qtyTB.SelectAll();

                    priceCodeTB.Enabled = true;
                    priceCodeTB.Text = "!";

                    priceTB.Enabled = true;
                    priceTB.Text = workingItem.price.ToString();

                    taxedCB.Enabled = true;
                    taxedCB.Checked = workingItem.taxed;

                    acceptItemButton.Enabled = true;

                    cancelItemButton.Enabled = true;
                    cancelItemButton.Visible = true;
                }
                return;
            }
            addingItems = DatabaseHandler.SqlCheckItemNumber(itemNoTB.Text, false);

            if (addingItems == null)
            {
                MessageBox.Show("Invalid Part Number!");
                itemNoTB.Clear();
                return;
            }
            else
            {
                productLineDropBox.Enabled = true;
                foreach (Item item in addingItems)
                    productLineDropBox.Items.Add(item.productLine);

                if (addingItems.Count == 1)
                {
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

        private void EditLineItem()
        {
            addingLine = true;

            workingItem.quantity = float.Parse(qtyTB.Text);
            workingItem.price = float.Parse(priceTB.Text);
            workingItem.total = (float)Math.Round(workingItem.quantity * workingItem.price, 2);
            workingItem.taxed = taxedCB.Checked;

            dataGridView1.SelectedRows[0].Cells[3].Value = workingItem.quantity;
            dataGridView1.SelectedRows[0].Cells[4].Value = workingItem.serialNumber;
            dataGridView1.SelectedRows[0].Cells[6].Value = workingItem.price.ToString("C");
            dataGridView1.SelectedRows[0].Cells[7].Value = workingItem.total.ToString("C");
            if (workingItem.taxed)
                dataGridView1.SelectedRows[0].Cells[8].Value = "Y";
            else
                dataGridView1.SelectedRows[0].Cells[8].Value = "N";
            dataGridView1.SelectedRows[0].Cells[9].Value = workingItem.codes;

            InvoiceItem j = currentInvoice.items.Find(el => el.ID == workingItem.ID);
            currentInvoice.items.Remove(j);
            currentInvoice.items.Add(workingItem);

            itemNoTB.Clear();
            itemNoTB.Enabled = true;
            productLineDropBox.Items.Clear();
            productLineDropBox.Text = "";
            productLineDropBox.Enabled = false;
            descriptionTB.Clear();
            descriptionTB.Enabled = false;
            qtyTB.Clear();
            qtyTB.Enabled = false;
            priceCodeTB.Clear();
            priceCodeTB.Enabled = false;
            priceTB.Clear();
            priceTB.Enabled = false;
            taxedCB.Checked = false;
            taxedCB.Enabled = false;
            pndTB.Clear();
            pndTB.Enabled = false;
            acceptItemButton.Enabled = false;
            cancelItemButton.Visible = false;

            itemNoTB.Focus();

            dataGridView1.ClearSelection();
            dataGridView1.Enabled = true;

            float total = 0.00f;
            foreach (InvoiceItem i in currentInvoice.items)
            {
                total += i.total;
            }
            currentInvoice.subtotal = total;
            subtotalLabel.Text = total.ToString("C");

            addingLine = false;
        }

        private void AddLineItem()
        {
            addingLine = true;

            workingItem.quantity = float.Parse(qtyTB.Text);
            workingItem.price = float.Parse(priceTB.Text);
            workingItem.taxed = taxedCB.Checked;
            workingItem.total = (float)Math.Round(workingItem.quantity * workingItem.price, 2);

            currentInvoice.items.Add(workingItem);
            int row = dataGridView1.Rows.Add();
            dataGridView1.Rows[row].Cells[0].Value = workingItem.itemNumber;
            dataGridView1.Rows[row].Cells[1].Value = workingItem.productLine;
            dataGridView1.Rows[row].Cells[2].Value = workingItem.itemName;
            dataGridView1.Rows[row].Cells[3].Value = workingItem.quantity;
            dataGridView1.Rows[row].Cells[4].Value = workingItem.serialNumber;
            dataGridView1.Rows[row].Cells[5].Value = workingItem.listPrice.ToString("C");
            dataGridView1.Rows[row].Cells[6].Value = workingItem.price.ToString("C");
            dataGridView1.Rows[row].Cells[7].Value = workingItem.total.ToString("C");
            if (workingItem.taxed)
                dataGridView1.Rows[row].Cells[8].Value = "Y";
            else
                dataGridView1.Rows[row].Cells[8].Value = "N";
            dataGridView1.Rows[row].Cells[9].Value = workingItem.codes;
            dataGridView1.Rows[row].Cells[10].Value = workingItem.ID;

            itemNoTB.Clear();
            productLineDropBox.Items.Clear();
            productLineDropBox.Text = "";
            productLineDropBox.Enabled = false;
            descriptionTB.Clear();
            descriptionTB.Enabled = false;
            qtyTB.Clear();
            qtyTB.Enabled = false;
            priceCodeTB.Clear();
            priceCodeTB.Enabled = false;
            priceTB.Clear();
            priceTB.Enabled = false;
            taxedCB.Checked = false;
            taxedCB.Enabled = false;
            pndTB.Clear();
            pndTB.Enabled = false;
            acceptItemButton.Enabled = false;
            cancelItemButton.Visible = false;

            itemNoTB.Focus();

            dataGridView1.ClearSelection();

            float total = 0.00f;
            foreach (InvoiceItem i in currentInvoice.items)
            {
                total += i.total;
            }
            currentInvoice.subtotal = total;
            subtotalLabel.Text = total.ToString("C");

            addingLine = false;
        }

        private void SelectProductLine()
        {
            string selectedLine = productLineDropBox.Text;
            workingItem = new InvoiceItem(addingItems.Find(el => el.productLine == selectedLine));
            workingItem.ID = Guid.NewGuid();

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
            taxedCB.Checked = workingItem.taxed;

            acceptItemButton.Enabled = true;

            cancelItemButton.Enabled = true;
            cancelItemButton.Visible = true;
        }

        private void Checkout()
        {
            Checkout checkoutForm = new Checkout(currentInvoice, this);
            checkoutForm.ShowDialog();
            if (currentInvoice.finalized)
            {
                CancelInvoice();
                customerNoTB.Focus();
            }
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
            if (e.KeyCode == Keys.F2)
            {
                if (checkoutButton.Enabled)
                    Checkout();
                return;
            }
            if (e.Modifiers == Keys.LShiftKey && e.KeyCode == Keys.Tab)
                extraFunctionsDropBox.Focus();

            if (e.KeyCode != Keys.Enter || itemNoTB.Text == "")
                return;

            EnterItemNumber();
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

            if (currentState == State.EditingLineItem)
                EditLineItem();
            else
                AddLineItem();
        }

        private void acceptItemButton_Click(object sender, EventArgs e)
        {
            if (currentState == State.EditingLineItem)
                EditLineItem();
            else
                AddLineItem();

            extraFunctionsDropBox.Enabled = true;
        }

        private void customerNoTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            AddCustomer();
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

            extraFunctionsDropBox.Enabled = false;
            currentState = State.EditingLineItem;
            workingItem = currentInvoice.items.Find(el => el.ID == (Guid)dataGridView1.SelectedRows[0].Cells[10].Value);

            itemNoTB.Text = workingItem.itemNumber;
            descriptionTB.Text = workingItem.itemName;
            qtyTB.Text = workingItem.quantity.ToString();
            priceCodeTB.Text = "!";
            priceTB.Text = workingItem.price.ToString();
            if (workingItem.taxed)
                taxedCB.Checked = true;
            else if (workingItem.taxed)
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
            Checkout();
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
                        productLineDropBox.SelectedIndex = i - 1;
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

            if (currentInvoice.items.Count == 0)
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
                workingItem = null;
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
                currentInvoice.items.Remove(currentInvoice.items.Find(el => el.ID == (Guid)dataGridView1.SelectedRows[0].Cells["guid"].Value));
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
                foreach (InvoiceItem i in currentInvoice.items)
                {
                    total += i.total;
                }
                subtotalLabel.Text = total.ToString("C");
                dataGridView1.Enabled = true;
                dataGridView1.ClearSelection();
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
