using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TIMS.Accounting;

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
