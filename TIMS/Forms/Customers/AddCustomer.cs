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
    public partial class AddCustomer : Form
    {
        public Customer customer;

        public AddCustomer()
        {
            InitializeComponent();

            customerCopyCB.Items.Add("NEW CUSTOMER");
            List<Customer> availableCustomers = Communication.GetCustomers();
            availableCustomers.OrderBy(el => int.Parse(el.customerNumber));
            foreach (Customer c in availableCustomers)
                customerCopyCB.Items.Add(c);
            customerCopyCB.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Program.IsStringNumeric(customerNumberTB.Text) || customerNumberTB.Text == string.Empty)
            {
                MessageBox.Show("Please enter a valid customer number!\n(Only numbers allowed)");
                return;
            }

            if (Communication.CheckCustomerNumber(customerNumberTB.Text) != null)
            {
                MessageBox.Show("Customer number is already in use!\nPlease enter a unique customer number!");
                return;
            }

            if (customerNameTB.Text == string.Empty)
            {
                MessageBox.Show("Please enter a valid name for customer!");
                return;
            }

            if ((customerCopyCB.SelectedItem as string) != "NEW CUSTOMER")
            {
                customer = Communication.CheckCustomerNumber((customerCopyCB.SelectedItem as Customer).customerNumber);
            }
            else
            {
                customer = Communication.CheckCustomerNumber("9999");
            }

            customer.customerNumber = customerNumberTB.Text;
            customer.customerName = customerNameTB.Text;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
