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
using TIMSServerModel;

namespace TIMS.Forms.Orders
{
    public partial class CheckinCreator : Form
    {
        public bool checkinSaved = false;
        public Checkin checkinObj;

        public CheckinCreator()
        {
            InitializeComponent();
            foreach (PurchaseOrder order in Communication.RetrievePurchaseOrders())
            {
                if (order.finalized)
                {
                    int row = dataGridView1.Rows.Add();
                    dataGridView1.Rows[row].Cells[0].Value = order.PONumber;
                    dataGridView1.Rows[row].Cells[1].Value = order.totalCost;
                    dataGridView1.Rows[row].Cells[2].Value = order.totalItems;
                    dataGridView1.Rows[row].Cells[3].Value = order.assignedCheckin == 0 ? "UNASSIGNED" : order.assignedCheckin.ToString();
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
            if (dataGridView1.SelectedRows[0].Cells[3].Value.ToString() != "UNASSIGNED")
            {
                MessageBox.Show("Cannot assign this PO to a checkin, it's already assigned to a checkin group!");
                return;
            }

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

            if (checkinObj == null)
                checkinObj = new Checkin(Communication.RetrieveNextCheckinNumber());
            checkinObj.items.Clear();
            checkinObj.orders.Clear();
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                checkinObj.orders.Add(Communication.RetrievePurchaseOrder(int.Parse(row.Cells[0].Value.ToString())));
            }
            foreach (PurchaseOrder order in checkinObj.orders)
            {
                foreach (InvoiceItem item in order.items)
                {
                    CheckinItem checkinItem = checkinObj.items.Find(el => el.itemNumber == item.itemNumber && el.productLine == item.productLine);
                    if (checkinItem != null)
                    {
                        checkinObj.items.Find(el => el.productLine == item.productLine && el.itemNumber == item.itemNumber).ordered += item.quantity;
                    }
                    else
                    {
                        checkinObj.items.Add(new CheckinItem(item.itemNumber, item.productLine) { ordered = item.quantity });
                    }
                }
            }

            if (!checkinSaved)
            {
                Communication.SaveCheckin(checkinObj);

                label1.Text = "Checkin Number: " + checkinObj.checkinNumber.ToString();
                checkinSaved = true;
            }
            else
            {
                Communication.DeleteCheckin(checkinObj.checkinNumber);
                Communication.SaveCheckin(checkinObj);
            }
        }
    }
}
