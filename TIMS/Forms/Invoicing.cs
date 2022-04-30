using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TIMS
{
    public partial class Invoicing : Form
    {
        public string[] currentEmployee;
        public string[] currentCustomer;
        public string[] currentSalesperson;

        public enum State
        {
            NoEmployee,
            NoCustomer,
            InvoiceIdle,
            AddingLineItem,
            EditingLineItem,
            CancellingInvoice,
            Startup,
        }
        public State currentState;

        public DataGridViewRow currentEditingRow;

        public Invoicing()
        {
            InitializeComponent();
            DatabaseHandler.InitializeDatabases();
            currentState = State.Startup;
            button14.Visible = false;
            listBox1.Items.Clear();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;
            int custNo;

            try { custNo = int.Parse(textBox1.Text); }
            catch 
            { 
                MessageBox.Show("Invalid employee number!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
                textBox1.SelectAll();  
                return; 
            }

            currentEmployee = DatabaseHandler.CheckEmployee(custNo);
            if (currentEmployee == null)
            {
                MessageBox.Show("Invalid employee number!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.SelectAll();
                return;
            }
            label2.Text = currentEmployee[0];
            textBox2.Enabled = true;
            textBox2.Focus();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.SelectedIndex == 1)
            {
                currentState = State.CancellingInvoice;
                dataGridView1.Rows.Clear();
                DisableAllControls();
            }
            comboBox4.SelectedIndex = 0;
        }

        private void EnableControls()
        {
            textBox2.Enabled = true;
            button1.Enabled = true;
            comboBox1.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button7.Enabled = true;
            textBox4.Enabled = true;
            button6.Enabled = true;
            textBox3.Enabled = true;
            dataGridView1.Enabled = true;
            textBox5.Enabled = true;
            comboBox4.Enabled = true;
            button12.Enabled = true;
            if (currentEmployee.Contains("all") || currentEmployee.Contains("msg"))
                button2.Enabled = true;
            if (currentEmployee.Contains("all") || currentEmployee.Contains("roa"))
                button3.Enabled = true;
        }
        private void DisableAllControls()
        {
            textBox2.Enabled = false;
            button1.Enabled = false;
            comboBox1.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button7.Enabled = false;
            textBox4.Enabled = false;
            button6.Enabled = false;
            textBox3.Enabled = false;
            dataGridView1.Enabled = false;
            textBox5.Enabled = false;
            comboBox4.Enabled = false;
            button12.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            listBox1.Items.Clear();
            textBox2.Clear();
            label2.Text = "";
            textBox1.Clear();
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.LShiftKey && e.KeyCode == Keys.Tab)
                comboBox4.Focus();

            if (e.KeyCode != Keys.Enter || textBox5.Text == "")
                return;

            currentState = State.AddingLineItem;
            comboBox4.Enabled = false;

            string[] item = DatabaseHandler.CheckItemNumber(textBox5.Text);
            if (item == null)
            {
                MessageBox.Show("Invalid Part Number!");
                textBox5.Clear();
                return;
            }
            textBox7.Enabled = true;
            textBox8.Enabled = true;
            textBox9.Enabled = true;
            textBox10.Enabled = true;
            checkBox1.Enabled = true;
            button8.Enabled = true;
            textBox8.Text = "1";
            textBox8.Focus();
            textBox7.Text = item[0];
            textBox10.Text = item[1];
            textBox5.ReadOnly = true;
            textBox7.ReadOnly = true;
            textBox9.Text = "!";
            checkBox1.Checked = true;
            button14.Visible = true;
            button14.Enabled = true;
            button14.Text = "Cancel";
        }

        private void textBox8_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button8.Focus();
        }

        private void button8_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            AddLineItem();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            AddLineItem();
        }

        private void AddLineItem()
        {
            int index = 0;
            if (currentState == State.AddingLineItem)
                index = dataGridView1.Rows.Add();
            else
                index = currentEditingRow.Index;

            dataGridView1.Rows[index].Cells[0].Value = textBox5.Text;
            dataGridView1.Rows[index].Cells[1].Value = textBox7.Text;
            dataGridView1.Rows[index].Cells[2].Value = textBox8.Text;
            dataGridView1.Rows[index].Cells[5].Value = textBox10.Text;
            dataGridView1.Rows[index].Cells[6].Value = (float.Parse(textBox10.Text) * float.Parse(textBox8.Text)).ToString();
            if (checkBox1.Checked)
                dataGridView1.Rows[index].Cells[7].Value = "Y";
            else
                dataGridView1.Rows[index].Cells[7].Value = "N";

            float subtotal = 0.0f;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                subtotal += float.Parse((string)row.Cells[6].Value);
            }
            label28.Text = "$" + subtotal.ToString();

            button14.Enabled = false;
            button14.Visible = false;
            button13.Enabled = false;
            button8.Enabled = false;
            checkBox1.Enabled = false;
            textBox10.Enabled = false;
            textBox10.Clear();
            textBox9.Enabled = false;
            textBox9.Clear();
            textBox8.Enabled = false;
            textBox8.Clear();
            textBox7.Enabled = false;
            textBox7.Clear();
            textBox5.Focus();
            textBox5.ReadOnly = false;
            textBox5.Clear();
            comboBox4.Enabled = true;

            dataGridView1.ClearSelection();

            if (currentState == State.EditingLineItem)
            {
                textBox5.Enabled = true;
                textBox5.ReadOnly = false;
                textBox5.Focus();
            }
            currentState = State.NoEmployee;
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            currentCustomer = DatabaseHandler.CheckCustomerNumber(textBox2.Text);
            if (currentCustomer == null)
            {
                MessageBox.Show("Invalid customer number!");
                textBox2.SelectAll();
                return;
            }

            textBox2.Text = currentCustomer[0] + " " + currentCustomer[1];
            EnableControls();
            textBox5.Focus();
        }

        private void dataGridView1_Paint(object sender, PaintEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
                button9.Enabled = true;
            else
                button9.Enabled = false;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (currentState == State.AddingLineItem)
            {
                button14.Enabled = false;
                button14.Visible = false;
                button13.Enabled = false;
                button8.Enabled = false;
                checkBox1.Enabled = false;
                textBox10.Enabled = false;
                textBox10.Clear();
                textBox9.Enabled = false;
                textBox9.Clear();
                textBox8.Enabled = false;
                textBox8.Clear();
                textBox7.Enabled = false;
                textBox7.Clear();
                textBox5.Focus();
                textBox5.ReadOnly = false;
                textBox5.Clear();
                currentState = State.NoEmployee;
            }
            if (currentState == State.EditingLineItem)
            {
                dataGridView1.Rows.Remove(currentEditingRow);
                button14.Enabled = false;
                button14.Visible = false;
                button13.Enabled = false;
                button8.Enabled = false;
                checkBox1.Enabled = false;
                textBox10.Enabled = false;
                textBox10.Clear();
                textBox9.Enabled = false;
                textBox9.Clear();
                textBox8.Enabled = false;
                textBox8.Clear();
                textBox7.Enabled = false;
                textBox7.Clear();
                textBox5.Enabled = true;
                textBox5.Focus();
                textBox5.ReadOnly = false;
                textBox5.Clear();

                float total = 0.00f;
                if (dataGridView1.Rows.Count == 0)
                    label28.Text = "$0.00";
                else
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        total += float.Parse((string)row.Cells[6].Value);
                    }
                    label28.Text = "$" + total.ToString();
                }
                currentState = State.NoEmployee;
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (currentState == State.AddingLineItem || currentState == State.EditingLineItem || currentState == State.CancellingInvoice)
                return;

            currentState = State.EditingLineItem;

            currentEditingRow = dataGridView1.SelectedRows[0];
            textBox5.Text = (string)currentEditingRow.Cells[0].Value;
            textBox7.Text = (string)currentEditingRow.Cells[1].Value;
            textBox8.Text = (string)currentEditingRow.Cells[2].Value;
            textBox9.Text = "!";
            textBox10.Text = (string)currentEditingRow.Cells[5].Value;
            if ((string)currentEditingRow.Cells[7].Value == "Y")
                checkBox1.Checked = true;
            else if ((string)currentEditingRow.Cells[7].Value == "N")
                checkBox1.Checked = false;

            textBox5.Enabled = false;
            textBox7.Enabled = false;
            textBox8.Enabled = true;
            textBox9.Enabled = true;
            textBox10.Enabled = true;
            checkBox1.Enabled = true;
            textBox8.Focus();
            textBox8.SelectAll();
            button8.Enabled = true;
            button14.Visible = true;
            button14.Enabled = true;
            button14.Text = "Delete";
        }

        private void addCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddCustomer addCustomerForm = new AddCustomer();
            addCustomerForm.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Checkout checkoutForm = new Checkout();
            checkoutForm.Show();
        }
    }
}
