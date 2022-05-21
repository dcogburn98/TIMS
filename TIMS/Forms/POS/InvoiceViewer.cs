using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TIMS.Forms.POS
{
    public partial class InvoiceViewer : Form
    {
        Invoice inv;
        public InvoiceViewer(Invoice inv)
        {
            InitializeComponent();
            CancelButton = closeButton;

            this.inv = inv;
            foreach (InvoiceItem item in inv.items)
            {
                int row = dataGridView1.Rows.Add();

                dataGridView1.Rows[row].Cells[0].Value = item.itemNumber;
            }
        }

        private void printButton_Click(object sender, EventArgs e)
        {
            ReportViewer viewer = new ReportViewer(inv);
        }
    }
}
