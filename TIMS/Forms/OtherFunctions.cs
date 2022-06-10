﻿using System;
using System.Windows.Forms;
using TIMS.Forms.POS;
using System.Collections.Generic;

using TIMS.Forms.Reporting;
using TIMS.Forms.Orders;

namespace TIMS.Forms
{
    public partial class OtherFunctions : Form
    {
        Item workingItem;
        public OtherFunctions()
        {
            InitializeComponent();
            tabControl1.Selected += ChangeManagementTab;
        }

        private void SelectProductLine()
        {
            workingItem = DatabaseHandler.SqlRetrieveItem(itemNumberTB.Text, productLineComboBox.Text);
            PopulateItemInfoFields();
        }

        private void PopulateItemInfoFields()
        {
            itemDescriptionTB.Text = workingItem.itemName;

            productLineTBField.Text = workingItem.productLine;
            itemNumberTBField.Text = workingItem.itemNumber;
            itemNameTB.Text = workingItem.itemName;
            supplierCB.Text = workingItem.supplier;
            groupCodeTB.Text = workingItem.groupCode.ToString();
            velocityCodeCB.Text = workingItem.velocityCode.ToString();
            prevYearVelocityCodeCB.Text = workingItem.previousYearVelocityCode.ToString();
            categoryCB.Text = workingItem.category;
            standardPkgTB.Text = workingItem.standardPackage.ToString();
            taxableCB.Checked = workingItem.taxed;
            descriptionTB.Text = workingItem.longDescription;

            dateStockedTB.Text = workingItem.dateStocked.ToString("MM/dd/yyyy");
            lastReceiptTB.Text = workingItem.dateLastReceipt.ToString("MM/dd/yyyy");
            minTB.Text = workingItem.minimum.ToString();
            maxTB.Text = workingItem.maximum.ToString();
            onHandTB.Text = workingItem.onHandQty.ToString();
            wipQtyTB.Text = workingItem.WIPQty.ToString();
            onOrderQtyTB.Text = workingItem.onOrderQty.ToString();
            onBackorderQtyTB.Text = workingItem.onBackorderQty.ToString();
            daysOnOrderTB.Text = workingItem.daysOnOrder.ToString();
            daysOnBackOrderTB.Text = workingItem.daysOnBackorder.ToString();

            listPriceTB.Text = workingItem.listPrice.ToString();
            redPriceTB.Text = workingItem.redPrice.ToString();
            yellowPriceTB.Text = workingItem.yellowPrice.ToString();
            greenPriceTB.Text = workingItem.greenPrice.ToString();
            pinkPriceTB.Text = workingItem.pinkPrice.ToString();
            bluePriceTB.Text = workingItem.bluePrice.ToString();
            costTB.Text = workingItem.replacementCost.ToString();
            avgCostTB.Text = workingItem.averageCost.ToString();
        }

        private void ChangeManagementTab(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                itemNameTB.Focus();
            }
        }



        #region Toolbar Item Click Methods and Other Form Handlers
        
        private void orderOgToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void OtherFunctions_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.OpenForms.Remove(this);
            Program.CheckOpenForms();
        }

        private void generalJournalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GeneralJournal journal = new GeneralJournal();
            journal.Show();
        }

        private void chartOfAccountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Accounting.ChartOfAccounts COA = new Accounting.ChartOfAccounts();
            COA.Show();
        }

        private void invoicingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.OpenForm(new Invoicing());
        }

        private void reportCreatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportCreator creator = new ReportCreator();
            creator.Show();
        }

        private void reportManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportManager manager = new ReportManager();
            manager.Show();
        }

        private void massImportItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemImport import = new ItemImport();
            import.Show();
        }

        private void binLabelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BinLabelPrinting printing = new BinLabelPrinting();
            printing.Show();
        }

        private void createOrderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OrderSelection order = new OrderSelection();
            order.Show();
        }
        #endregion



        private void reviewChangeTransactionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form reviewInvoices = Program.OpenForms.Find(el => el is ReviewInvoices);
            if (reviewInvoices == null)
                Program.OpenForm(new ReviewInvoices());
            else reviewInvoices.BringToFront();
        }

        private void itemNumberTB_Leave(object sender, EventArgs e)
        {
            productLineComboBox.Items.Clear();
            List<Item> items = DatabaseHandler.SqlCheckItemNumber(itemNumberTB.Text, false);
            if (items == null)
                return;

            foreach (Item item in items)
            {
                productLineComboBox.Items.Add(item.productLine);
            }
            productLineComboBox.DroppedDown = true;
        }

        private void itemNumberTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                productLineComboBox.Focus();
            }
        }

        private void productLineComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || string.IsNullOrEmpty(productLineComboBox.Text))
            {
                return;
            }

            if (int.TryParse(productLineComboBox.Text, out int i))
            {
                if (i > productLineComboBox.Items.Count || i == 0)
                    return;
                else
                {
                    productLineComboBox.SelectedIndex = i - 1;
                    SelectProductLine();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            workingItem = null;
            itemDescriptionTB.Text = string.Empty;

            productLineTBField.Text = string.Empty;
            itemNumberTBField.Text = string.Empty;
            itemNameTB.Text = string.Empty;
            descriptionTB.Text = string.Empty;
            supplierCB.Text = string.Empty;
            groupCodeTB.Text = string.Empty;
            velocityCodeCB.Text = string.Empty;
            velocityCodeCB.Items.Clear();
            prevYearVelocityCodeCB.Text = string.Empty;
            prevYearVelocityCodeCB.Items.Clear();
            standardPkgTB.Text = string.Empty;
            categoryCB.Text = string.Empty;
            categoryCB.Items.Clear();
            departmentCB.Text = string.Empty;
            departmentCB.Items.Clear();
            subDepartmentCB.Text = string.Empty;
            subDepartmentCB.Items.Clear();
            unitTB.Text = string.Empty;
            taxableCB.Checked = false;

            dateStockedTB.Text = string.Empty;
            lastReceiptTB.Text = string.Empty;
            minTB.Text = string.Empty;
            maxTB.Text = string.Empty;
            onHandTB.Text = string.Empty;
            wipQtyTB.Text = string.Empty;
            onOrderQtyTB.Text = string.Empty;
            onBackorderQtyTB.Text = string.Empty;
            daysOnBackOrderTB.Text = string.Empty;
            daysOnOrderTB.Text = string.Empty;

            listPriceTB.Text = string.Empty;
            redPriceTB.Text = string.Empty;
            yellowPriceTB.Text = string.Empty;
            greenPriceTB.Text = string.Empty;
            pinkPriceTB.Text = string.Empty;
            bluePriceTB.Text = string.Empty;
            costTB.Text = string.Empty;
            avgCostTB.Text = string.Empty;

            lastLabelDateTB.Text = string.Empty;
            lastLabelPriceTB.Text = string.Empty;
            locationsLB.Items.Clear();

            serializedCB.Checked = false;
            serialNumberTB.Text = string.Empty;
            serialNumbersLB.Items.Clear();
            removerSNbtn.Enabled = false;

            dateOfLastSaleTB.Text = string.Empty;

        }
    }
}
