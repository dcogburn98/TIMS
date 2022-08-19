﻿using System;
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

namespace TIMS.Forms.Orders
{
    public partial class CheckinPicker : Form
    {
        Checkin activeCheckin;
        string prevEdit = string.Empty;
        public CheckinPicker()
        {
            InitializeComponent();

            foreach (Checkin checkin in Communication.RetrieveCheckins())
            {
                comboBox1.Items.Add(checkin.checkinNumber.ToString());
            }
            comboBox1.SelectedIndex = 0;
            activeCheckin = Communication.RetrieveCheckin(int.Parse(comboBox1.SelectedItem.ToString()));

            PopulateGrid();
        }

        private void PopulateGrid()
        {
            dataGridView1.Rows.Clear();

            foreach (CheckinItem item in activeCheckin.items)
            {
                int row = dataGridView1.Rows.Add();
                dataGridView1.Rows[row].Cells[0].Value = item.productLine;
                dataGridView1.Rows[row].Cells[1].Value = item.itemNumber;
                dataGridView1.Rows[row].Cells[2].Value = Communication.RetrieveItem(item.itemNumber, item.productLine).itemName;
                dataGridView1.Rows[row].Cells[3].Value = item.ordered;
                dataGridView1.Rows[row].Cells[4].Value = item.shipped;
                dataGridView1.Rows[row].Cells[5].Value = item.received;
                dataGridView1.Rows[row].Cells[6].Value = item.damaged;
                string polist = string.Empty;
                List<PurchaseOrder> orders = activeCheckin.orders.FindAll(el => el.items.Find(ell => ell.itemNumber == item.itemNumber && ell.productLine == item.productLine) != null);
                foreach (PurchaseOrder order in orders)
                    polist += order.PONumber + ",";
                polist = polist.Trim(',');
                dataGridView1.Rows[row].Cells[7].Value = polist;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            activeCheckin = Communication.RetrieveCheckin(int.Parse(comboBox1.SelectedItem.ToString()));

            PopulateGrid();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex > 6 || e.ColumnIndex < 5)
            {
                e.Cancel = true;
                return;
            }
            prevEdit = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (!decimal.TryParse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out decimal _))
            {
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = prevEdit;
            }
        }
    }
}
