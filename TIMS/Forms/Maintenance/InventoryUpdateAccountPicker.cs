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

namespace TIMS.Forms.Maintenance
{
    public partial class InventoryUpdateAccountPicker : Form
    {
        public Account selectedAccount;
        public InventoryUpdateAccountPicker(decimal difference, bool importing)
        {
            InitializeComponent();

            foreach (Account acct in Communication.RetrieveAccounts())
            {
                comboBox1.Items.Add(acct.Name);
            }
            if (difference > 0)
            {
                comboBox1.SelectedIndex = comboBox1.Items.IndexOf("Positive Adjustment");
            }
            else
            {
                comboBox1.SelectedIndex = comboBox1.Items.IndexOf("Negative Adjustment");
            }

            if (importing)
            {
                label1.Text = "The current items being imported will affect the inventory value.";
                button2.Visible = false;
            }
            else
            {
                CancelButton = button2;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
                return;

            selectedAccount = Communication.RetrieveAccounts().FirstOrDefault(el => el.Name == comboBox1.SelectedItem as string);
            if (selectedAccount == default(Account))
            {
                MessageBox.Show("Invalid account!");
                return;
            }

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
