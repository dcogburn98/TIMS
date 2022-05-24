using System;
using System.Windows.Forms;
using TIMS.Forms.POS;

namespace TIMS.Forms
{
    public partial class OtherFunctions : Form
    {
        public OtherFunctions()
        {
            InitializeComponent();
        }

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

        private void reviewChangeTransactionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form reviewInvoices = Program.OpenForms.Find(el => el is ReviewInvoices);
            if (reviewInvoices == null)
                Program.OpenForm(new ReviewInvoices());
            else reviewInvoices.BringToFront();
        }
    }
}
