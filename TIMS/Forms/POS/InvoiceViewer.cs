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
                dataGridView1.Rows[row].Cells[1].Value = item.productLine;
                dataGridView1.Rows[row].Cells[2].Value = item.itemName;
                dataGridView1.Rows[row].Cells[3].Value = item.quantity;
                dataGridView1.Rows[row].Cells[4].Value = item.serialNumber;
                dataGridView1.Rows[row].Cells[5].Value = item.listPrice.ToString("C");
                dataGridView1.Rows[row].Cells[6].Value = item.price.ToString("C");
                dataGridView1.Rows[row].Cells[7].Value = item.total.ToString("C");
                dataGridView1.Rows[row].Cells[8].Value = item.taxed;
                dataGridView1.Rows[row].Cells[9].Value = item.codes;
            }
        }

        private void printButton_Click(object sender, EventArgs e)
        {
            ReportViewer viewer = new ReportViewer(inv);
            viewer.ShowDialog();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
