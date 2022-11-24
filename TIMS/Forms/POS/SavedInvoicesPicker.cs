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
    public partial class SavedInvoicesPicker : Form
    {
        public Invoice selectedInvoice;

        public SavedInvoicesPicker()
        {
            InitializeComponent();

            int sq = 0;
            List<Invoice> invoices = Communication.RetrieveSavedInvoices();
            if (invoices != null)
                foreach (Invoice inv in invoices)
                {
                    int row = dataGridView1.Rows.Add();
                    dataGridView1.Rows[row].Cells[0].Value = ++sq;
                    dataGridView1.Rows[row].Cells[1].Value = inv.customer.customerNumber;
                    dataGridView1.Rows[row].Cells[2].Value = inv.attentionLine;
                    dataGridView1.Rows[row].Cells[3].Value = inv.PONumber;
                    dataGridView1.Rows[row].Cells[4].Value = inv.employee.employeeNumber;
                    dataGridView1.Rows[row].Cells[7].Value = inv.invoiceCreationTime.ToString("MM/dd/yyyy");
                    dataGridView1.Rows[row].Cells[8].Value = inv.invoiceCreationTime.ToString("h:mm tt");
                    dataGridView1.Rows[row].Tag = inv;
                }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1)
                return;

            selectedInvoice = Communication.RetrieveInvoice((dataGridView1.SelectedRows[0].Tag as Invoice).invoiceNumber);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
