﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

using TIMS.Server;
using TIMSServerModel;

namespace TIMS.Forms.Orders
{
    public partial class OrderCreator : Form
    {
        public string supplier = string.Empty;
        public string criteria = string.Empty;
        public InvoiceItem workingItem;
        public decimal beforeEditValue = 0.0m;
        public int existingPO = 0;
        bool finalized = false;
        public OrderCreator(string supplier, string criteria)
        {
            InitializeComponent();
            CancelButton = button3;
            this.supplier = supplier;
            this.criteria = criteria;
            if (supplier == "Manual Order")
            {
                this.supplier = string.Empty;
                supplierLabel.Text = "Supplier: All";
            }
            else
                supplierLabel.Text = "Supplier: " + supplier;

            criteriaLabel.Text = "Pre-Fill Criteria: " + criteria;

            decimal totalCost = 0;
            decimal totalItems = 0;
            decimal totalRetail = 0;
            decimal totalPotentialProfit = 0;
            decimal averageMargin = 0;
            switch (criteria)
            {
                case "all":
                    if (supplier == "Manual Order")
                        break;
                    List<Item> itemsall = Communication.RetrieveItemsFromSupplier(supplier);
                    if (itemsall == null)
                        break;
                    foreach (Item item in itemsall)
                    {
                        workingItem = new InvoiceItem(item);
                        int row = dataGridView1.Rows.Add();
                        dataGridView1.Rows[row].Cells[0].Value = workingItem.itemNumber;
                        dataGridView1.Rows[row].Cells[1].Value = workingItem.productLine;
                        dataGridView1.Rows[row].Cells[2].Value = workingItem.itemName;
                        dataGridView1.Rows[row].Cells[3].Value = 1;
                        dataGridView1.Rows[row].Cells[4].Value = item.minimum;
                        dataGridView1.Rows[row].Cells[5].Value = item.maximum;
                        dataGridView1.Rows[row].Cells[6].Value = item.onHandQty;
                        dataGridView1.Rows[row].Cells[7].Value = workingItem.cost;
                        dataGridView1.Rows[row].Cells[8].Value = workingItem.price;
                        dataGridView1.Rows[row].Cells[9].Value = workingItem.cost;
                        dataGridView1.Rows[row].Cells[10].Value = workingItem.price;
                    }

                    foreach (DataGridViewRow roww in dataGridView1.Rows)
                    {
                        totalCost += decimal.Parse(roww.Cells[9].Value.ToString());
                        totalItems += decimal.Parse(roww.Cells[3].Value.ToString());
                        totalRetail += decimal.Parse(roww.Cells[10].Value.ToString());
                        averageMargin = totalRetail == 0 ? 0 : (totalRetail - totalCost) / totalRetail;
                    }
                    totalPotentialProfit = totalRetail - totalCost;
                    totalCostTB.Text = totalCost.ToString("C");
                    totalItemsTB.Text = totalItems.ToString();
                    totalRetailTB.Text = totalRetail.ToString("C");
                    potentialProfitTB.Text = totalPotentialProfit.ToString("C");
                    averageMarginTB.Text = averageMargin.ToString("P");
                    break;
                case "none":
                    break;
                case "min":
                    if (supplier == "Manual Order")
                        break;
                    List<Item> itemsmin = Communication.RetrieveItemsFromSupplierBelowMin(supplier);
                    if (itemsmin == null)
                        break;
                    foreach (Item item in itemsmin)
                    {
                        workingItem = new InvoiceItem(item);
                        int row = dataGridView1.Rows.Add();
                        dataGridView1.Rows[row].Cells[0].Value = workingItem.itemNumber;
                        dataGridView1.Rows[row].Cells[1].Value = workingItem.productLine;
                        dataGridView1.Rows[row].Cells[2].Value = workingItem.itemName;
                        dataGridView1.Rows[row].Cells[3].Value = item.onHandQty < 0 ? item.minimum : item.minimum - item.onHandQty;
                        dataGridView1.Rows[row].Cells[4].Value = item.minimum;
                        dataGridView1.Rows[row].Cells[5].Value = item.maximum;
                        dataGridView1.Rows[row].Cells[6].Value = item.onHandQty;
                        dataGridView1.Rows[row].Cells[7].Value = workingItem.cost;
                        dataGridView1.Rows[row].Cells[8].Value = workingItem.price;
                        dataGridView1.Rows[row].Cells[9].Value = workingItem.cost;
                        dataGridView1.Rows[row].Cells[10].Value = workingItem.price;
                    }

                    foreach (DataGridViewRow roww in dataGridView1.Rows)
                    {
                        totalCost += decimal.Parse(roww.Cells[9].Value.ToString());
                        totalItems += decimal.Parse(roww.Cells[3].Value.ToString());
                        totalRetail += decimal.Parse(roww.Cells[10].Value.ToString());
                        averageMargin = (totalRetail - totalCost) / totalRetail;
                    }
                    totalPotentialProfit = totalRetail - totalCost;
                    totalCostTB.Text = totalCost.ToString("C");
                    totalItemsTB.Text = totalItems.ToString();
                    totalRetailTB.Text = totalRetail.ToString("C");
                    potentialProfitTB.Text = totalPotentialProfit.ToString("C");
                    averageMarginTB.Text = averageMargin.ToString("P");
                    break;
                case "max":
                    if (supplier == "Manual Order")
                        break;
                    List<Item> itemsmax = Communication.RetrieveItemsFromSupplierBelowMax(supplier);
                    if (itemsmax == null)
                        break;
                    foreach (Item item in itemsmax)
                    {
                        workingItem = new InvoiceItem(item);
                        int row = dataGridView1.Rows.Add();
                        dataGridView1.Rows[row].Cells[0].Value = workingItem.itemNumber;
                        dataGridView1.Rows[row].Cells[1].Value = workingItem.productLine;
                        dataGridView1.Rows[row].Cells[2].Value = workingItem.itemName;
                        dataGridView1.Rows[row].Cells[3].Value = item.onHandQty < 0 ? item.maximum : item.maximum - item.onHandQty;
                        dataGridView1.Rows[row].Cells[4].Value = item.minimum;
                        dataGridView1.Rows[row].Cells[5].Value = item.maximum;
                        dataGridView1.Rows[row].Cells[6].Value = item.onHandQty;
                        dataGridView1.Rows[row].Cells[7].Value = workingItem.cost;
                        dataGridView1.Rows[row].Cells[8].Value = workingItem.price;
                        dataGridView1.Rows[row].Cells[9].Value = workingItem.cost;
                        dataGridView1.Rows[row].Cells[10].Value = workingItem.price;
                    }

                    foreach (DataGridViewRow roww in dataGridView1.Rows)
                    {
                        totalCost += decimal.Parse(roww.Cells[9].Value.ToString());
                        totalItems += decimal.Parse(roww.Cells[3].Value.ToString());
                        totalRetail += decimal.Parse(roww.Cells[10].Value.ToString());
                        averageMargin = (totalRetail - totalCost) / totalRetail;
                    }
                    totalPotentialProfit = totalRetail - totalCost;
                    totalCostTB.Text = totalCost.ToString("C");
                    totalItemsTB.Text = totalItems.ToString();
                    totalRetailTB.Text = totalRetail.ToString("C");
                    potentialProfitTB.Text = totalPotentialProfit.ToString("C");
                    averageMarginTB.Text = averageMargin.ToString("P");
                    break;
                case "items sold":
                    List<InvoiceItem> items = Communication.RetrieveItemsFromSupplierSoldAfterLastOrderDate(supplier);
                    if (items == null)
                        break;
                    foreach (InvoiceItem item in items)
                    {
                        workingItem = item;
                        Item iData = Communication.RetrieveItem(workingItem.itemNumber, workingItem.productLine);
                        bool broken = false;
                        foreach (DataGridViewRow roww in dataGridView1.Rows)
                        {
                            if (roww.Cells[0].Value.ToString() == item.itemNumber && roww.Cells[1].Value.ToString() == item.productLine)
                            {
                                decimal quantity = decimal.Parse(roww.Cells[3].Value.ToString());
                                quantity += item.quantity;
                                roww.Cells[3].Value = quantity;
                                roww.Cells[9].Value = iData.replacementCost * quantity;
                                roww.Cells[10].Value = iData.greenPrice * quantity;
                                broken = true;
                            }
                        }
                        if (broken)
                            continue;

                        int row = dataGridView1.Rows.Add();
                        dataGridView1.Rows[row].Cells[0].Value = workingItem.itemNumber;
                        dataGridView1.Rows[row].Cells[1].Value = workingItem.productLine;
                        dataGridView1.Rows[row].Cells[2].Value = workingItem.itemName;
                        dataGridView1.Rows[row].Cells[3].Value = item.quantity;
                        dataGridView1.Rows[row].Cells[4].Value = iData.minimum;
                        dataGridView1.Rows[row].Cells[5].Value = iData.maximum;
                        dataGridView1.Rows[row].Cells[6].Value = iData.onHandQty;
                        dataGridView1.Rows[row].Cells[7].Value = iData.replacementCost;
                        dataGridView1.Rows[row].Cells[8].Value = iData.greenPrice;
                        dataGridView1.Rows[row].Cells[9].Value = iData.replacementCost * item.quantity;
                        dataGridView1.Rows[row].Cells[10].Value = iData.greenPrice * item.quantity;
                    }

                    foreach (DataGridViewRow roww in dataGridView1.Rows)
                    {
                        totalCost += decimal.Parse(roww.Cells[9].Value.ToString());
                        totalItems += decimal.Parse(roww.Cells[3].Value.ToString());
                        totalRetail += decimal.Parse(roww.Cells[10].Value.ToString());
                        averageMargin = (totalRetail - totalCost) / totalRetail;
                    }
                    totalPotentialProfit = totalRetail - totalCost;
                    totalCostTB.Text = totalCost.ToString("C");
                    totalItemsTB.Text = totalItems.ToString();
                    totalRetailTB.Text = totalRetail.ToString("C");
                    potentialProfitTB.Text = totalPotentialProfit.ToString("C");
                    averageMarginTB.Text = averageMargin.ToString("P");
                    break;
                    break;
            }
        }

        public OrderCreator(PurchaseOrder order)
        {
            InitializeComponent();
            CancelButton = button3;
            this.supplier = order.supplier;
            supplierLabel.Text = "Supplier: " + supplier;
            criteriaLabel.Visible = false;
            shippingCostTB.Text = order.shippingCost.ToString();

            decimal totalCost = 0;
            decimal totalItems = 0;
            decimal totalPotentialProfit = 0;
            decimal totalRetail = 0;
            decimal averageMargin = 0;
            foreach (InvoiceItem item in order.items)
            {
                Item i = Communication.RetrieveItem(item.itemNumber, item.productLine);
                int row = dataGridView1.Rows.Add();
                dataGridView1.Rows[row].Cells[0].Value = item.itemNumber;
                dataGridView1.Rows[row].Cells[1].Value = item.productLine;
                dataGridView1.Rows[row].Cells[2].Value = item.itemName;
                dataGridView1.Rows[row].Cells[3].Value = item.quantity;
                dataGridView1.Rows[row].Cells[4].Value = i.minimum;
                dataGridView1.Rows[row].Cells[5].Value = i.maximum;
                dataGridView1.Rows[row].Cells[6].Value = i.onHandQty;
                dataGridView1.Rows[row].Cells[7].Value = item.cost;
                dataGridView1.Rows[row].Cells[8].Value = item.price;
                dataGridView1.Rows[row].Cells[9].Value = item.cost * item.quantity;
                dataGridView1.Rows[row].Cells[10].Value = item.price * item.quantity;
            }
            foreach (DataGridViewRow roww in dataGridView1.Rows)
            {
                totalCost += decimal.Parse(roww.Cells[9].Value.ToString());
                totalItems += decimal.Parse(roww.Cells[3].Value.ToString());
                totalRetail += decimal.Parse(roww.Cells[10].Value.ToString());
                averageMargin = (totalRetail - totalCost) / totalRetail;
            }
            totalPotentialProfit = totalRetail - totalCost;
            totalCostTB.Text = totalCost.ToString("C");
            totalItemsTB.Text = totalItems.ToString();
            totalRetailTB.Text = totalRetail.ToString("C");
            potentialProfitTB.Text = totalPotentialProfit.ToString("C");
            averageMarginTB.Text = averageMargin.ToString("P");

            existingPO = order.PONumber;

            if (order.finalized)
            {
                finalized = true;
                finalizeButton.Enabled = false;
                saveOrderButton.Enabled = false;
                shippingCostTB.Enabled = false;
                itemNumberTB.Enabled = false;
                productLineCB.Enabled = false;
                qtyTB.Enabled = false;
                addItemButton.Enabled = false;
                clearItemButton.Enabled = false;
                deleteItemButton.Enabled = false;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            productLineCB.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count != 0 && !finalized)
            {
                if (MessageBox.Show("Are you sure you want to leave order editing?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    Close();
            }
            else
                Close();
        }

        private void productLineCB_Enter(object sender, EventArgs e)
        {
            if (itemNumberTB.Text == string.Empty)
                return;

            productLineCB.Items.Clear();
            List<Item> items;
            if (supplier == string.Empty)
                items = Communication.CheckItemNumber(itemNumberTB.Text, false);
            else
                items = Communication.CheckItemNumberFromSupplier(itemNumberTB.Text, supplier);

            if (items == null)
            {
                MessageBox.Show("Invalid item number!");
                itemNumberTB.Focus();
                itemNumberTB.SelectAll();
                return;
            }

            if (items.Count == 1)
            {
                productLineCB.Items.Add(items[0].productLine.ToUpper());
                productLineCB.SelectedIndex = 0;
                qtyTB.Focus();
                qtyTB.Text = "1";
                qtyTB.SelectAll();
            }
            else
            {
                foreach (Item i in items)
                {
                    productLineCB.Items.Add(i.productLine.ToUpper());
                }
                productLineCB.DroppedDown = true;
            }
            
        }

        private void qtyTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }

            // only allow one minus sign
            if ((e.KeyChar == '-') && ((sender as TextBox).Text.IndexOf('-') > -1))
            {
                e.Handled = true;
            }
        }

        private void productLineCB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
                return;

            if (int.Parse(e.KeyChar.ToString()) > productLineCB.Items.Count || int.Parse(e.KeyChar.ToString()) == 0)
                return;

            productLineCB.SelectedIndex = int.Parse(e.KeyChar.ToString()) - 1;
        }

        private void productLineCB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || productLineCB.SelectedIndex == -1)
                return;

            qtyTB.Focus();
        }

        private void addItemButton_Click(object sender, EventArgs e)
        {
            if (productLineCB.SelectedIndex == -1 || productLineCB.Items.Count < 1)
            {
                MessageBox.Show("Invalid item!");
                return;
            }

            Item item = Communication.RetrieveItem(itemNumberTB.Text, productLineCB.Text);
            workingItem = new InvoiceItem(item);
            workingItem.quantity = decimal.Parse(qtyTB.Text);

            bool broken = false;
            foreach (DataGridViewRow roww in dataGridView1.Rows)
            {
                if (roww.Cells[0].Value.ToString() == workingItem.itemNumber && roww.Cells[1].Value.ToString() == workingItem.productLine)
                {
                    decimal qty = decimal.Parse(roww.Cells[3].Value.ToString()) + workingItem.quantity;
                    roww.Cells[3].Value = qty;
                    roww.Cells[9].Value = workingItem.cost * qty;
                    roww.Cells[10].Value = workingItem.price * qty;
                    broken = true;
                }
            }

            if (!broken)
            {
                int row = dataGridView1.Rows.Add();
                dataGridView1.Rows[row].Cells[0].Value = workingItem.itemNumber;
                dataGridView1.Rows[row].Cells[1].Value = workingItem.productLine;
                dataGridView1.Rows[row].Cells[2].Value = workingItem.itemName;
                dataGridView1.Rows[row].Cells[3].Value = workingItem.quantity;
                dataGridView1.Rows[row].Cells[4].Value = item.minimum;
                dataGridView1.Rows[row].Cells[5].Value = item.maximum;
                dataGridView1.Rows[row].Cells[6].Value = item.onHandQty;
                dataGridView1.Rows[row].Cells[7].Value = workingItem.cost;
                dataGridView1.Rows[row].Cells[8].Value = workingItem.price;
                dataGridView1.Rows[row].Cells[9].Value = workingItem.cost * workingItem.quantity;
                dataGridView1.Rows[row].Cells[10].Value = workingItem.price * workingItem.quantity;
            }

            decimal totalCost = 0;
            decimal totalItems = 0;
            foreach (DataGridViewRow roww in dataGridView1.Rows)
            {
                totalCost += decimal.Parse(roww.Cells[9].Value.ToString());
                totalItems += decimal.Parse(roww.Cells[3].Value.ToString());
            }
            totalCostTB.Text = totalCost.ToString("C");
            totalItemsTB.Text = totalItems.ToString();

            itemNumberTB.Text = "";
            productLineCB.Items.Clear();
            qtyTB.Text = "";
            itemNumberTB.Focus();
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex < 3 || e.ColumnIndex > 8 || dataGridView1.SelectionMode == DataGridViewSelectionMode.FullRowSelect)
            {
                e.Cancel = true;
                return;
            }

            beforeEditValue = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (!decimal.TryParse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out decimal d))
            {
                MessageBox.Show("Invalid quantity");
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = beforeEditValue;
                return;
            }

            //if (decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()) == 0)
            //{
            //    MessageBox.Show("Cannot use 0 as a quantity for order!\nUse 'Delete Item' button if removal was intended."); 
            //    return;
            //}

            Item item = Communication.RetrieveItem(
                dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(),
                dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
            workingItem = new InvoiceItem(item);

            workingItem.quantity = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
            workingItem.cost = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString());
            workingItem.price = decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString());

            dataGridView1.Rows[e.RowIndex].Cells[9].Value = workingItem.cost * workingItem.quantity;
            dataGridView1.Rows[e.RowIndex].Cells[10].Value = workingItem.price * workingItem.quantity;

            decimal totalCost = 0;
            decimal totalItems = 0;
            decimal totalRetail = 0;
            decimal totalPotentialProfit;
            decimal averageMargin;
            foreach (DataGridViewRow roww in dataGridView1.Rows)
            {
                totalCost += decimal.Parse(roww.Cells[9].Value.ToString());
                totalItems += decimal.Parse(roww.Cells[3].Value.ToString());
                totalRetail += decimal.Parse(roww.Cells[10].Value.ToString());
            }
            if (totalRetail != 0)
                averageMargin = (totalRetail - totalCost) / totalRetail;
            else
                averageMargin = 0;

            totalPotentialProfit = totalRetail - totalCost;
            totalCostTB.Text = totalCost.ToString("C");
            totalItemsTB.Text = totalItems.ToString();
            totalRetailTB.Text = totalRetail.ToString("C");
            potentialProfitTB.Text = totalPotentialProfit.ToString("C");
            averageMarginTB.Text = averageMargin.ToString("P");
        }

        private void clearItemButton_Click(object sender, EventArgs e)
        {
            itemNumberTB.Text = "";
            productLineCB.Items.Clear();
            qtyTB.Text = "";
            itemNumberTB.Focus();
        }

        private void qtyTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            addItemButton.Focus();
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            finalizeButton.Enabled = true;
            saveOrderButton.Enabled = true;
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (dataGridView1.Rows.Count < 1)
            {
                finalizeButton.Enabled = false;
                saveOrderButton.Enabled = false;
            }
            decimal totalCost = 0;
            decimal totalItems = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                totalCost += decimal.Parse(row.Cells[9].Value.ToString());
                totalItems += decimal.Parse(row.Cells[3].Value.ToString());
            }
            totalCostTB.Text = totalCost.ToString("C");
            totalItemsTB.Text = totalItems.ToString();
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!finalized)
                deleteItemButton.Enabled = true;
        }

        private void deleteItemButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count < 1)
                return;

            dataGridView1.Rows.Remove(dataGridView1.CurrentCell.OwningRow);
            if (dataGridView1.Rows.Count < 1)
                deleteItemButton.Enabled = false;
        }

        private void saveOrderButton_Click(object sender, EventArgs e)
        {
            PurchaseOrder order = new PurchaseOrder(supplier, Communication.RetrieveNextPONumber());
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (decimal.Parse(row.Cells[3].Value.ToString()) == 0)
                    continue;

                order.items.Add(new InvoiceItem(Communication.RetrieveItem(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString()))
                {
                    quantity = decimal.Parse(row.Cells[3].Value.ToString()),
                    cost = decimal.Parse(row.Cells[7].Value.ToString()),
                    total = decimal.Parse(row.Cells[3].Value.ToString()) * decimal.Parse(row.Cells[7].Value.ToString())
                });
                order.totalCost += decimal.Parse(row.Cells[3].Value.ToString()) * decimal.Parse(row.Cells[7].Value.ToString());
                order.totalItems++;
            }
            order.shippingCost = decimal.TryParse(shippingCostTB.Text, out decimal d) == false ? 0 : d;

            if (existingPO == 0)
                Communication.SavePurchaseOrder(order);
            else
            {
                order.PONumber = existingPO;
                Communication.SavePurchaseOrder(order);
            }
            MessageBox.Show("Order saved with order number: " + order.PONumber);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }

            // only allow one minus sign
            if ((e.KeyChar == '-') && ((sender as TextBox).Text.IndexOf('-') > -1))
            {
                e.Handled = true;
            }
        }

        private void finalizeButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to finalize this purchase order?\nYou will not be able to edit items it contains.", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                PurchaseOrder order = new PurchaseOrder(supplier, Communication.RetrieveNextPONumber());
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (decimal.Parse(row.Cells[3].Value.ToString()) == 0)
                        continue;

                    order.items.Add(new InvoiceItem(Communication.RetrieveItem(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString()))
                    {
                        quantity = decimal.Parse(row.Cells[3].Value.ToString()),
                        cost = decimal.Parse(row.Cells[7].Value.ToString()),
                        total = decimal.Parse(row.Cells[3].Value.ToString()) * decimal.Parse(row.Cells[7].Value.ToString())
                    });
                    order.totalCost += decimal.Parse(row.Cells[3].Value.ToString()) * decimal.Parse(row.Cells[7].Value.ToString());
                    order.totalItems++;
                }
                order.finalized = true;
                order.shippingCost = decimal.TryParse(shippingCostTB.Text, out decimal d) == false ? 0 : d;

                if (existingPO == 0)
                    Communication.SavePurchaseOrder(order);
                else
                {
                    order.PONumber = existingPO;
                    Communication.FinalizePurchaseOrder(order);
                }

                ReportViewer viewer = new ReportViewer(order);
                viewer.ShowDialog();
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
