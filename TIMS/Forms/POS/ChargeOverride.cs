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
    public partial class ChargeOverride : Form
    {
        public ChargeOverride(Invoice inv)
        {
            InitializeComponent();
            label1.Text = "Charging the amount of " + inv.total.ToString("C") + "\nwill bring the account balance beyond the allowed credit limit of " + inv.customer.creditLimit.ToString("C");
            CancelButton = cancelButton;
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == Communication.RetrieveProperty("Charge Override Password"))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Invalid password.");
            }
        }
    }
}
