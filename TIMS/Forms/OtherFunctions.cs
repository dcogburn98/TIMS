using System;
using System.Windows.Forms;
using TIMS.Forms.POS;
using System.Collections.Generic;

using TIMS.Forms.Reporting;
using TIMS.Forms.Orders;
using TIMS.Server;
using TIMSServerModel;

namespace TIMS.Forms
{
    public partial class OtherFunctions : Form
    {
        Item workingItem;
        public OtherFunctions()
        {
            InitializeComponent();
            tabControl1.Selected += ChangeManagementTab;
            foreach (string supplier in Communication.RetrieveSuppliers())
            {
                supplierCB.Items.Add(supplier);
            }
        }

        private void SelectProductLine()
        {
            workingItem = Communication.RetrieveItem(itemNumberTB.Text, productLineComboBox.Text);
            PopulateItemInfoFields();
        }

        private void ClearAllItemFields()
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

            itemNumberTB.Text = string.Empty;
            productLineComboBox.Items.Clear();
            productLineComboBox.Text = string.Empty;
        }

        private void PopulateItemInfoFields()
        {
            itemNameTB.Focus();
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


        #region Item Maintenance Group Methods
        private void reviewChangeTransactionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form reviewInvoices = Program.OpenForms.Find(el => el is ReviewInvoices);
            if (reviewInvoices == null)
                Program.OpenForm(new ReviewInvoices());
            else reviewInvoices.BringToFront();
        }

        private void itemNumberTB_Leave(object sender, EventArgs e)
        {
            if (itemNumberTB.Text == "")
                return;

            productLineComboBox.Items.Clear();
            List<Item> items = Communication.CheckItemNumber(itemNumberTB.Text, false);
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
            else
            {
                if (productLineComboBox.Items.Contains(productLineComboBox.Text))
                {
                    productLineComboBox.SelectedIndex = productLineComboBox.Items.IndexOf(productLineComboBox.Text);
                    SelectProductLine();
                }
                else
                {
                    if (MessageBox.Show(@"Item number """ + itemNumberTB.Text + @""" does not exist in product line " + productLineComboBox.Text + "!\nWould you like to add it?", "Add product?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        workingItem = new Item() { itemNumber = itemNumberTB.Text, productLine = productLineComboBox.Text};
                        PopulateItemInfoFields();
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (ageRestrictedCB.Checked && minimumAgeTB.Text == "")
            {
                MessageBox.Show("Minimum age is required for age restricted items!");
                minimumAgeTB.Focus();
                return;
            }

            Item newItem = new Item();
            newItem.productLine = productLineTBField.Text == string.Empty ? "XXX" : productLineTBField.Text;
            newItem.itemNumber = itemNumberTBField.Text == string.Empty ? "XXX" : itemNumberTBField.Text;
            newItem.itemName = itemNameTB.Text;
            newItem.supplier = supplierCB.Text;
            newItem.groupCode = int.TryParse(groupCodeTB.Text, out int i) == false ? 0 : i;
            newItem.velocityCode = int.TryParse(velocityCodeCB.Text, out i) == false ? 0 : i;
            newItem.previousYearVelocityCode = int.TryParse(prevYearVelocityCodeCB.Text, out i) == false ? 0 : i;
            newItem.category = categoryCB.Text;
            newItem.standardPackage = int.TryParse(standardPkgTB.Text, out i) == false ? 0 : i;
            newItem.taxed = taxableCB.Checked;
            newItem.dateStocked = DateTime.Parse(dateStockedTB.Text);
            newItem.dateLastReceipt = DateTime.Parse(lastReceiptTB.Text);
            newItem.minimum = decimal.TryParse(minTB.Text, out decimal j) == false ? 0 : j;
            newItem.maximum = decimal.TryParse(maxTB.Text, out j) == false ? 0 : j;
            newItem.onHandQty = decimal.TryParse(onHandTB.Text, out j) == false ? 0 : j;
            newItem.WIPQty = decimal.TryParse(wipQtyTB.Text, out j) == false ? 0 : j;
            newItem.onOrderQty = decimal.TryParse(onOrderQtyTB.Text, out j) == false ? 0 : j;
            newItem.onBackorderQty = decimal.TryParse(onBackorderQtyTB.Text, out j) == false ? 0 : j;
            newItem.daysOnOrder = int.TryParse(daysOnOrderTB.Text, out i) == false ? 0 : i;
            newItem.daysOnBackorder = int.TryParse(daysOnBackOrderTB.Text, out i) == false ? 0 : i;
            newItem.listPrice = decimal.TryParse(listPriceTB.Text, out j) == false ? 0 : j;
            newItem.redPrice = decimal.TryParse(redPriceTB.Text, out j) == false ? 0 : j;
            newItem.yellowPrice = decimal.TryParse(yellowPriceTB.Text, out j) == false ? 0 : j;
            newItem.greenPrice = decimal.TryParse(greenPriceTB.Text, out j) == false ? 0 : j;
            newItem.pinkPrice = decimal.TryParse(pinkPriceTB.Text, out j) == false ? 0 : j;
            newItem.bluePrice = decimal.TryParse(bluePriceTB.Text, out j) == false ? 0 : j;
            newItem.replacementCost = decimal.TryParse(costTB.Text, out j) == false ? 0 : j;
            newItem.averageCost = decimal.TryParse(avgCostTB.Text, out j) == false ? 0 : j;
            newItem.ageRestricted = ageRestrictedCB.Checked;
            newItem.minimumAge = int.TryParse(minimumAgeTB.Text, out i) == false ? 0 : i;
            newItem.longDescription = descriptionTB.Text;
            newItem.serialized = serializedCB.Checked;

            Communication.UpdateItem(newItem);
            MessageBox.Show("Item updated!");
        }
        
        private void clearItemButton_Click(object sender, EventArgs e)
        {
            ClearAllItemFields();
            itemNumberTB.Focus();
        }
        #endregion

        private void productLineComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar))
            {
                e.KeyChar = char.ToUpper(e.KeyChar);
            }
        }

        private void openPurchaseOrdersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenOrderViewer viewer = new OpenOrderViewer();
            viewer.Show();
        }

        private void createCheckInToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CheckinCreator creator = new CheckinCreator();
            creator.Show();
        }
    }
}
