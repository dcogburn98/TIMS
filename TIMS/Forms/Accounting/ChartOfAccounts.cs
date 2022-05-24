using System.Windows.Forms;

namespace TIMS.Forms.Accounting
{
    public partial class ChartOfAccounts : Form
    {
        public ChartOfAccounts()
        {
            InitializeComponent();

            treeView1.Nodes.Add("Assets");
            treeView1.Nodes.Add("Equity");
            treeView1.Nodes.Add("Liabilities");
            treeView1.Nodes.Add("Income");
            treeView1.Nodes.Add("Expenses");
        }
    }
}
