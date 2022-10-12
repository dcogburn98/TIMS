using System;
using System.Windows.Forms;

using TIMSServerModel;

namespace TIMS
{
    public partial class AddCustomer : Form
    {
        public AddCustomer()
        {
            InitializeComponent();
            label6.Visible = false;
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            textBox3.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox1.Text.Length < 3)
            {
                label6.Text = "Please provide a valid customer name.";
                label6.Visible = true;
                return;
            }

            if (!int.TryParse(textBox2.Text, out int val))
            {
                label6.Text = "Please provide a valid customer number.";
                label6.Visible = true;
                return;
            }

            Customer customer = new Customer();
            customer.customerNumber = textBox2.Text; //Number
            customer.customerName = textBox1.Text; //Name
            customer.phoneNumber = textBox4.Text; //Phone
            string address = textBox5.Text + ", " + textBox6.Text + ", " + comboBox2.Text + ", " + comboBox1.Text;
            customer.billingAddress = address;
            customer.shippingAddress = address;
            customer.pricingProfile = comboBox1.Text; //Pricing Profile
            if (checkBox1.Checked) //Tax Exempt
                customer.primaryTaxStatus = "Exempt";
            else
                customer.primaryTaxStatus = "Non-Exempt";
            customer.primaryTaxExemptionNumber = textBox3.Text; //Tax Exempt Number

            //DatabaseHandler.AddCustomer(customer);

            Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBox3.Enabled = true;
            else
            {
                textBox3.Text = "";
                textBox3.Enabled = false;
            }
        }
    }
}
