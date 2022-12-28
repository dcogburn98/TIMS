using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using TIMSServerModel;
using TIMS.Server;

namespace TIMS.Forms.Accounting
{
    public partial class ChartOfAccounts : Form
    {
        List<Account> accounts = Communication.RetrieveAccounts();
        TreeNode assetsNode;
        TreeNode equityNode;
        TreeNode liabilitiesNode;
        TreeNode incomeNode;
        TreeNode expensesNode;

        public ChartOfAccounts()
        {
            InitializeComponent();

            PopulateTree();
        }

        private void PopulateTree()
        {
            assetsNode = treeView1.Nodes.Add("Assets");
            equityNode = treeView1.Nodes.Add("Equity");
            liabilitiesNode = treeView1.Nodes.Add("Liabilities");
            incomeNode = treeView1.Nodes.Add("Income");
            expensesNode = treeView1.Nodes.Add("Expenses");

            foreach (Account account in accounts.FindAll(el => el.Type == Account.AccountTypes.Asset))
            {
                TreeNode node = assetsNode.Nodes.Add(account.Name + " (" + account.Balance.ToString("C") + ")");
                node.Tag = account;
                node.Name = account.Name;
                if (account.Balance < 0)
                    node.ForeColor = Color.Red;
            }
            foreach (Account account in accounts.FindAll(el => el.Type == Account.AccountTypes.Equity))
            {
                TreeNode node = equityNode.Nodes.Add(account.Name + " (" + account.Balance.ToString("C") + ")");
                node.Tag = account;
                node.Name = account.Name;
                if (account.Balance < 0)
                    node.ForeColor = Color.Red;
            }
            foreach (Account account in accounts.FindAll(el => el.Type == Account.AccountTypes.Liability))
            {
                TreeNode node = liabilitiesNode.Nodes.Add(account.Name + " (" + account.Balance.ToString("C") + ")");
                node.Tag = account;
                node.Name = account.Name;
                if (account.Balance < 0)
                    node.ForeColor = Color.Red;
            }
            foreach (Account account in accounts.FindAll(el => el.Type == Account.AccountTypes.Income))
            {
                TreeNode node = incomeNode.Nodes.Add(account.Name + " (" + account.Balance.ToString("C") + ")");
                node.Tag = account;
                node.Name = account.Name;
                if (account.Balance < 0)
                    node.ForeColor = Color.Red;
            }
            foreach (Account account in accounts.FindAll(el => el.Type == Account.AccountTypes.Expense))
            {
                TreeNode node = expensesNode.Nodes.Add(account.Name + " (" + account.Balance.ToString("C") + ")");
                node.Tag = account;
                node.Name = account.Name;
                if (account.Balance < 0)
                    node.ForeColor = Color.Red;
            }
        }

        private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (treeView1.SelectedNode == null || treeView1.SelectedNode.Tag == null)
                return;

            bool assetsExpanded = false;
            bool equityExpanded = false;
            bool liabilitiesExpanded = false;
            bool incomeExpanded = false;
            bool expensesExpanded = false;
            if (assetsNode.IsExpanded)
                assetsExpanded = true;
            if (equityNode.IsExpanded)
                equityExpanded = true;
            if (liabilitiesNode.IsExpanded)
                liabilitiesExpanded = true;
            if (incomeNode.IsExpanded)
                incomeExpanded = true;
            if (expensesNode.IsExpanded)
                expensesExpanded = true;

            TreeNode selected = treeView1.SelectedNode;
            GeneralJournal journalView = new GeneralJournal((Account)treeView1.SelectedNode.Tag);
            journalView.ShowDialog();
            RefreshAccounts();

            if (assetsExpanded)
                assetsNode.Expand();
            if (equityExpanded)
                equityNode.Expand();
            if (liabilitiesExpanded)
                liabilitiesNode.Expand();
            if (incomeExpanded)
                incomeNode.Expand();
            if (expensesExpanded)
                expensesNode.Expand();
            treeView1.SelectedNode = treeView1.Nodes.Find(selected.Name, true)[0];
        }
    
        private void RefreshAccounts()
        {
            accounts = Communication.RetrieveAccounts();

            treeView1.Nodes.Clear();
            PopulateTree();
        }
    }
}
