using System;
using System.Windows.Forms;
using TIMS.Forms.POS;
using System.Collections.Generic;

using TIMS.Forms.Reporting;
using TIMS.Forms.Orders;
using TIMS.Forms.Settings;
using TIMS.Forms.Customers;
using TIMS.Forms.Maintenance;
using TIMS.Server;
using TIMSServerModel;


namespace TIMS.Forms
{
    public partial class OtherFunctions : Form
    {
        public OtherFunctions()
        {
            InitializeComponent();

            tabletInvoicingToolStripMenuItem.Visible = false;

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

        private void openPurchaseOrdersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenOrderViewer viewer = new OpenOrderViewer();
            viewer.Show();
        }

        private void createCheckInToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CheckinCreator creator = new CheckinCreator();
            if (creator.DialogResult != DialogResult.Cancel)
                creator.Show();
        }

        private void editPostToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CheckinPicker picker = new CheckinPicker();
            if (picker.DialogResult != DialogResult.Cancel)
                picker.Show();
        }

        private void deviceAssignmentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeviceAssignments assignments = new DeviceAssignments();
            assignments.Show();
        }

        private void receivePaymentOnAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReceiveOnAccount roa = new ReceiveOnAccount();
            roa.Show();
        }

        private void customerInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomerInformation ci = new CustomerInformation();
            ci.Show();
        }

        private void tabletInvoicingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.OpenForm(new TileDisplay());
        }

        private void pricingProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = Program.OpenForms.Find(el => el is PricingProfiles);
            if (form == null)
                Program.OpenForm(new PricingProfiles());
            else form.BringToFront();
        }

        private void itemMaintenanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = Program.OpenForms.Find(el => el is ItemMaintenance);
            if (form == null)
                Program.OpenForm(new ItemMaintenance());
            else form.BringToFront();
        }

        private void companyControlsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = Program.OpenForms.Find(el => el is CompanyControls);
            if (form == null)
                Program.OpenForm(new CompanyControls());
            else form.BringToFront();
        }
    }
}
