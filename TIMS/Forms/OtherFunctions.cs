using System;
using System.Windows.Forms;
using TIMS.Forms.POS;
using System.Collections.Generic;

namespace TIMS.Forms
{
    public partial class OtherFunctions : Form
    {
        Item workingItem;
        public OtherFunctions()
        {
            InitializeComponent();
            tabPage1.GotFocus += TabPage1_GotFocus;
        }

        private void TabPage1_GotFocus(object sender, EventArgs e)
        {
            itemNumberTB.Focus();
            itemNumberTB.SelectAll();
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
            standardPkgTB.Text = workingItem.standardPackage.ToString();
            taxableCB.Checked = workingItem.taxed;
        }

        #region Toolbar Items
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
            descriptionTB.Text = string.Empty;
            supplierCB.Text = string.Empty;
            groupCodeTB.Text = string.Empty;
            velocityCodeCB.Text = string.Empty;
            prevYearVelocityCodeCB.Text = string.Empty;
            standardPkgTB.Text = string.Empty;
            taxableCB.Checked = false;
        }

        private void reportCreatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportCreator creator = new ReportCreator();
            creator.Show();
        }
    }
}
