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
    public partial class ReviewTodaysInvoices : Form
    {
        public ReviewTodaysInvoices()
        {
            InitializeComponent();

            List<Invoice> invoices = Communication.RetrieveInvoicesByDateRange(DateTime.Today, DateTime.Today.AddDays(1));
            if (invoices != null)
                foreach (Invoice inv in invoices)
                {
                    int row = dataGridView1.Rows.Add();
                    dataGridView1.Rows[row].Tag = inv;
                    dataGridView1.Rows[row].Cells[0].Value = inv.invoiceNumber.ToString();
                    dataGridView1.Rows[row].Cells[1].Value = inv.invoiceFinalizedTime.ToString("h:mm tt");
                    dataGridView1.Rows[row].Cells[2].Value = inv.employee.fullName;
                    dataGridView1.Rows[row].Cells[3].Value = inv.customer.customerName;
                    //dataGridView1.Rows[row].Cells[4].Value = inv.terminal;
                    dataGridView1.Rows[row].Cells[5].Value = inv.total.ToString("C");

                    if (inv.payments.Count > 1)
                        dataGridView1.Rows[row].Cells[6].Value = "MULTI";
                    else
                        dataGridView1.Rows[row].Cells[6].Value = Enum.GetName(typeof(Payment.PaymentTypes), inv.payments[0].paymentType);

                    dataGridView1.Rows[row].Cells[7].Value = inv.voided ? "Void" : "";
                }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            InvoiceViewer viewer = new InvoiceViewer(new List<Invoice>() { dataGridView1.Rows[e.RowIndex].Tag as Invoice }, 0);
            viewer.ShowDialog();
        }
    }
}
