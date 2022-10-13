using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

using TIMSServerModel;
using TIMS.Server;

namespace TIMS.Forms
{
    public partial class CustomerSearch : Form
    {
        public List<Customer> availableCustomers;
        public Customer selectedCustomer;

        public CustomerSearch()
        {
            InitializeComponent();

            availableCustomers = new List<Customer>();
            comboBox1.SelectedIndex = 1;
        }

        private void search()
        {
            if (textBox1.Text == string.Empty)
            {
                MessageBox.Show("Please enter a search term.");
                textBox1.Focus();
                return;
            }

            dataGridView1.Rows.Clear();
            availableCustomers.Clear();

            foreach (Customer c in Communication.GetCustomers())
                availableCustomers.Add(c);

            switch (comboBox1.SelectedItem)
            {
                case "Customer Number":
                    if (radioButton1.Checked)
                    {
                        foreach (Customer c in availableCustomers.Where(el => el.customerNumber.ToUpper().Contains(textBox1.Text.ToUpper())))
                        {
                            int row = dataGridView1.Rows.Add();
                            dataGridView1.Rows[row].Cells[0].Value = c.customerName;
                            dataGridView1.Rows[row].Cells[1].Value = c.customerNumber;
                            dataGridView1.Rows[row].Cells[2].Value = c.phoneNumber;
                            dataGridView1.Rows[row].Cells[3].Value = c.billingAddress;
                            dataGridView1.Rows[row].Tag = c;
                        }
                    }
                    else if (radioButton2.Checked)
                    {
                        throw new NotImplementedException();
                    }
                    else if (radioButton3.Checked)
                    {
                        throw new NotImplementedException();
                    }
                    break;
                case "Customer Name":
                    if (radioButton1.Checked)
                    {
                        foreach (Customer c in availableCustomers.Where(el => el.customerName.ToUpper().Contains(textBox1.Text.ToUpper())))
                        {
                            int row = dataGridView1.Rows.Add();
                            dataGridView1.Rows[row].Cells[0].Value = c.customerName;
                            dataGridView1.Rows[row].Cells[1].Value = c.customerNumber;
                            dataGridView1.Rows[row].Cells[2].Value = c.phoneNumber;
                            dataGridView1.Rows[row].Cells[3].Value = c.billingAddress;
                            dataGridView1.Rows[row].Tag = c;
                        }
                    }
                    else if (radioButton2.Checked)
                    {
                        throw new NotImplementedException();
                    }
                    else if (radioButton3.Checked)
                    {
                        throw new NotImplementedException();
                    }
                    break;
                case "Customer Address":
                    if (radioButton1.Checked)
                    {
                        foreach (Customer c in availableCustomers.Where(el => el.billingAddress.ToUpper().Contains(textBox1.Text.ToUpper())))
                        {
                            int row = dataGridView1.Rows.Add();
                            dataGridView1.Rows[row].Cells[0].Value = c.customerName;
                            dataGridView1.Rows[row].Cells[1].Value = c.customerNumber;
                            dataGridView1.Rows[row].Cells[2].Value = c.phoneNumber;
                            dataGridView1.Rows[row].Cells[3].Value = c.billingAddress;
                            dataGridView1.Rows[row].Tag = c;
                        }
                    }
                    else if (radioButton2.Checked)
                    {
                        throw new NotImplementedException();
                    }
                    else if (radioButton3.Checked)
                    {
                        throw new NotImplementedException();
                    }
                    break;
                case "Customer Phone Number":
                    if (radioButton1.Checked)
                    {
                        foreach (Customer c in availableCustomers.Where(el => el.phoneNumber.ToUpper().Contains(textBox1.Text.ToUpper())))
                        {
                            int row = dataGridView1.Rows.Add();
                            dataGridView1.Rows[row].Cells[0].Value = c.customerName;
                            dataGridView1.Rows[row].Cells[1].Value = c.customerNumber;
                            dataGridView1.Rows[row].Cells[2].Value = c.phoneNumber;
                            dataGridView1.Rows[row].Cells[3].Value = c.billingAddress;
                            dataGridView1.Rows[row].Tag = c;
                        }
                    }
                    else if (radioButton2.Checked)
                    {
                        throw new NotImplementedException();
                    }
                    else if (radioButton3.Checked)
                    {
                        throw new NotImplementedException();
                    }
                    break;
            }

            if (dataGridView1.Rows.Count < 1)
            {
                textBox1.Focus();
                textBox1.SelectAll();
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            search();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                search();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count < 1 || dataGridView1.SelectedRows.Count > 1)
                return;

            selectedCustomer = (dataGridView1.SelectedRows[0].Tag as Customer);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CustomerSearch_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (selectedCustomer == null)
                DialogResult = DialogResult.Cancel;
        }
    }
}
