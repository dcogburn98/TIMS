using System.Windows.Forms;
using System.Collections.Generic;

using TIMSServerModel;
using TIMS.Server;

namespace TIMS.Forms.Accounting
{
    public partial class ChartOfAccounts : Form
    {
        List<Account> accounts = Communication.RetrieveAccounts();

        public ChartOfAccounts()
        {
            InitializeComponent();

            TreeNode assetsNode = treeView1.Nodes.Add("Assets");
            TreeNode equityNode = treeView1.Nodes.Add("Equity");
            TreeNode liabilitiesNode = treeView1.Nodes.Add("Liabilities");
            TreeNode incomeNode = treeView1.Nodes.Add("Income");
            TreeNode expensesNode = treeView1.Nodes.Add("Expenses");

            accounts = Communication.RetrieveAccounts();

            foreach (Account account in accounts.FindAll(el => el.Type == Account.AccountTypes.Asset))
            {
                TreeNode node = assetsNode.Nodes.Add(account.Name + " (" + account.Balance.ToString("C") + ")");
                node.Tag = account;
            }
            foreach (Account account in accounts.FindAll(el => el.Type == Account.AccountTypes.Equity))
            {
                TreeNode node = equityNode.Nodes.Add(account.Name + " (" + account.Balance.ToString("C") + ")");
                node.Tag = account;
            }
            foreach (Account account in accounts.FindAll(el => el.Type == Account.AccountTypes.Liability))
            {
                TreeNode node = liabilitiesNode.Nodes.Add(account.Name + " (" + account.Balance.ToString("C") + ")");
                node.Tag = account;
            }
            foreach (Account account in accounts.FindAll(el => el.Type == Account.AccountTypes.Income))
            {
                TreeNode node = incomeNode.Nodes.Add(account.Name + " (" + account.Balance.ToString("C") + ")");
                node.Tag = account;
            }
            foreach (Account account in accounts.FindAll(el => el.Type == Account.AccountTypes.Expense))
            {
                TreeNode node = expensesNode.Nodes.Add(account.Name + " (" + account.Balance.ToString("C") + ")");
                node.Tag = account;
            }
        }

        private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (treeView1.SelectedNode == null || treeView1.SelectedNode.Tag == null)
                return;

            GeneralJournal journalView = new GeneralJournal((Account)treeView1.SelectedNode.Tag);
            journalView.ShowDialog();
        }
    }
}
