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

namespace TIMS.Forms.POS
{
    public partial class ReceiveOnAccount : Form
    {
        Customer currentCustomer;

        public ReceiveOnAccount()
        {
            InitializeComponent();

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
                    MessageBox.Show("Customer does not exist!");
                    return;
                }
                else
                {
                    accountNameTB.Text = currentCustomer.customerName;
                }
            }
        }
    }
}
