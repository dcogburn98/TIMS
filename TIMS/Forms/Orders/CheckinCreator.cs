using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TIMS.Forms.Orders
{
    public partial class CheckinCreator : Form
    {
        public CheckinCreator()
        {
            InitializeComponent();
            foreach (PurchaseOrder order in DatabaseHandler.SqlRetrievePurchaseOrders())
            {
                if (order.finalized)
                {
                    int row = dataGridView1.Rows.Add();
                    dataGridView1.Rows[row].Cells[0].Value = order.PONumber;
                    dataGridView1.Rows[row].Cells[1].Value = order.totalCost;
                    dataGridView1.Rows[row].Cells[2].Value = order.totalItems;
                    dataGridView1.Rows[row].Cells[3].Value = order.assignedCheckin == 0 ? "" : order.assignedCheckin.ToString();
                    dataGridView1.Rows[row].Cells[4].Value = order.supplier;
                    dataGridView1.Rows[row].Cells[5].Value = order.finalized;
                    dataGridView1.Rows[row].Cells[6].Value = order.shippingCost;
                }
            }

            dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Ascending);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count < 1)
                return;

            DataGridViewRow row = dataGridView1.SelectedRows[0];
            dataGridView1.Rows.Remove(row);
            dataGridView2.Rows.Add(row);

            dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Ascending);
            dataGridView2.Sort(dataGridView2.Columns[0], ListSortDirection.Ascending);

            if (dataGridView2.Rows.Count < 1)
                saveButton.Enabled = false;
            else
                saveButton.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count < 1)
                return;

            DataGridViewRow row = dataGridView2.SelectedRows[0];
            dataGridView2.Rows.Remove(row);
            dataGridView1.Rows.Add(row);

            dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Ascending);
            dataGridView2.Sort(dataGridView2.Columns[0], ListSortDirection.Ascending);

            if (dataGridView2.Rows.Count < 1)
                saveButton.Enabled = false;
            else
                saveButton.Enabled = true;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count < 1)
                return;


        }
    }
}
