using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TIMS.Server;

namespace TIMS.Forms.Orders
{
    public partial class OrderSelection : Form
    {
        public string supplier = string.Empty;
        public string criteria = string.Empty;

        public OrderSelection()
        {
            InitializeComponent();
            CancelButton = button1;
            foreach (string supp in Communication.RetrieveSuppliers())
                comboBox1.Items.Add(supp);
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            supplier = comboBox1.Text;
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    criteria = "all";
                    break;
                case 1:
                    criteria = "none";
                    break;
                case 2:
                    criteria = "min";
                    break;
                case 3:
                    criteria = "max";
                    break;
                case 4:
                    criteria = "items sold";
                    break;
            }
            OrderCreator creator = new OrderCreator(supplier, criteria);
            creator.Show();
            //Close(); //Comment this line for testing, uncomment for release
        }
    }
}
