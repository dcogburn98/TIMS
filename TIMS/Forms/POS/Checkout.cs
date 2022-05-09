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
    public partial class Checkout : Form
    {
        Invoice invoice;
        public Checkout(Invoice invoice)
        {
            InitializeComponent();
            this.invoice = invoice;
            PopulateInvoiceInfo();
        }

        private void PopulateInvoiceInfo()
        {
            subtotalTB.Text = invoice.subtotal.ToString("C");

            float taxableTotal = 0;
            foreach (InvoiceItem item in invoice.items)
            {
                if (item.taxed)
                    taxableTotal += item.total;
            }

            taxableTB.Text = taxableTotal.ToString("C");
            taxRateTB.Text = 10.25f.ToString();
            taxAmtTB.Text = (taxableTotal * 0.1025f).ToString("C");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
