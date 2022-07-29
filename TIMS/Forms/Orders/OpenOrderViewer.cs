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
    public partial class OpenOrderViewer : Form
    {
        List<PurchaseOrder> POs;
        public OpenOrderViewer()
        {
            InitializeComponent();
            POs = DatabaseHandler.SqlRetrievePurchaseOrders();
            foreach (PurchaseOrder order in POs)
            {
                int row = dataGridView1.Rows.Add();
                dataGridView1.Rows[row].Cells[0].Value = order.PONumber.ToString();
                dataGridView1.Rows[row].Cells[1].Value = order.totalCost.ToString();
                dataGridView1.Rows[row].Cells[2].Value = order.totalItems.ToString();
                dataGridView1.Rows[row].Cells[3].Value = order.assignedCheckin.ToString();
                dataGridView1.Rows[row].Cells[4].Value = order.supplier.ToString();
                dataGridView1.Rows[row].Cells[5].Value = order.finalized.ToString();
                dataGridView1.Rows[row].Cells[6].Value = order.shippingCost.ToString();
            }
            dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Ascending);
        }

        private void updatePOList()
        {
            dataGridView1.Rows.Clear();
            POs = DatabaseHandler.SqlRetrievePurchaseOrders();
            foreach (PurchaseOrder order in POs)
            {
                int row = dataGridView1.Rows.Add();
                dataGridView1.Rows[row].Cells[0].Value = order.PONumber.ToString();
                dataGridView1.Rows[row].Cells[1].Value = order.totalCost.ToString();
                dataGridView1.Rows[row].Cells[2].Value = order.totalItems.ToString();
                dataGridView1.Rows[row].Cells[3].Value = order.assignedCheckin.ToString();
                dataGridView1.Rows[row].Cells[4].Value = order.supplier.ToString();
                dataGridView1.Rows[row].Cells[5].Value = order.finalized.ToString();
                dataGridView1.Rows[row].Cells[6].Value = order.shippingCost.ToString();
            }
            dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Ascending);
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count < 1)
                return;

            PurchaseOrder order = POs.Find(el => el.PONumber == int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString()));
            OrderCreator viewer = new OrderCreator(order);
            if (viewer.ShowDialog() == DialogResult.OK)
                updatePOList();
        }
    }
}
