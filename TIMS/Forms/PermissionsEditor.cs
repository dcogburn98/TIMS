using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TIMS.Forms
{
    public partial class PermissionsEditor : Form
    {
        private bool suppressNextCheckEvent = false;
        private bool isDoubleClick = false;

        public PermissionsEditor()
        {
            InitializeComponent();
            permissionsViewer.CheckBoxes = true;
            permissionsViewer.Nodes.Clear();
            TreeNode root = permissionsViewer.Nodes.Add("All Permissions");
            foreach (Permission p in EmployeePermissions.Permissions)
            {
                root.Nodes.Add(p.name);
            }
        }

        private void permissionsViewer_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void permissionsViewer_DoubleClick(object sender, EventArgs e)
        {
            isDoubleClick = true;
        }

        private void permissionsViewer_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            //permissionsViewer_DoubleClick(sender, e);
        }

        private void permissionsViewer_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (isDoubleClick && e.Action == TreeViewAction.Collapse)
            {
                e.Cancel = true;
                isDoubleClick = false;
            }
        }
        private void permissionsViewer_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (isDoubleClick && e.Action == TreeViewAction.Expand)
            {
                e.Cancel = true;
                isDoubleClick = false;
            }
        }
        private void permissionsViewer_MouseDown(object sender, MouseEventArgs e)
        {
            isDoubleClick = e.Clicks > 1;
        }

        private void permissionsViewer_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (suppressNextCheckEvent)
                return;

            //Check or uncheck all children of the node
            suppressNextCheckEvent = true;
            List<TreeNode> changedNodes = new List<TreeNode>();
            if (e.Node.Nodes.Count > 0)
                foreach (TreeNode t in e.Node.Nodes)
                    t.Checked = e.Node.Checked;
            suppressNextCheckEvent = false;

            //If the node has no parent, it must be root so there are no further checks to perform
            if (e.Node.Parent == null)
                return;

            //Check to see if all sibling nodes are checked or not
            bool allSiblingsChecked = false;
            foreach (TreeNode t in e.Node.Parent.Nodes)
                if (t.Checked)
                    allSiblingsChecked = true;
                else
                {
                    allSiblingsChecked = false;
                    break;
                }

            //If all siblings are checked, check the parent as well
            suppressNextCheckEvent = true;   
            if (allSiblingsChecked)
                e.Node.Parent.Checked = true;
            else
                e.Node.Parent.Checked = false;
            suppressNextCheckEvent = false;
        }
    }
}
