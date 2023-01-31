using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TIMSServerModel;
using TIMS.Server;

namespace TIMS.Forms.POS
{
    public partial class ProductCatalog : Form
    {
        Invoicing parentForm;
        List<Item> searchResults;

        public ProductCatalog(Invoicing invoiceForm)
        {
            InitializeComponent();

            parentForm = invoiceForm;

            foreach (string department in Communication.RetrieveProductDepartments().OrderBy(x=>x))
            {
                TreeNode departmentNode = treeView2.Nodes.Add(department);
                TreeNode tempNode = departmentNode.Nodes.Add("Loading...");
                tempNode.Name = "temp";
            }
        }

        private void Search()
        {
            treeView1.Nodes.Clear();
            TreeNode departmentsFilterNode = treeView1.Nodes.Add("Departments");
            departmentsFilterNode.Name = "departments";
            TreeNode categoriesFilterNode = treeView1.Nodes.Add("Categories");
            categoriesFilterNode.Name = "categories";
            TreeNode suppliersFilterNode = treeView1.Nodes.Add("Suppliers");
            suppliersFilterNode.Name = "suppliers";

            tabControl1.SelectedIndex = 1;
            dataGridView1.Rows.Clear();
            searchResults = Communication.SearchItemsByQuery(textBox1.Text);
            foreach (Item item in searchResults)
            {
                int row = dataGridView1.Rows.Add();
                dataGridView1.Rows[row].Cells[0].Value = item.productLine;
                dataGridView1.Rows[row].Cells[1].Value = item.itemNumber;
                dataGridView1.Rows[row].Cells[2].Value = item.itemName;
                dataGridView1.Rows[row].Cells[3].Value = item.onHandQty.ToString();
                dataGridView1.Rows[row].Cells[4].Value = parentForm.currentInvoice.customer.inStorePricingProfile.CalculatePrice(item).ToString("C");
                dataGridView1.Rows[row].Cells[5].Value = item.UPC;
                dataGridView1.Rows[row].Tag = item;
                if (departmentsFilterNode.Nodes.Find(item.department, false).Length == 0)
                {
                    departmentsFilterNode.Nodes.Add(item.department).Name = item.department;
                }
                if (categoriesFilterNode.Nodes.Find(item.category, false).Length == 0)
                {
                    categoriesFilterNode.Nodes.Add(item.category).Name = item.category;
                }
                if (suppliersFilterNode.Nodes.Find(item.supplier, false).Length == 0)
                {
                    suppliersFilterNode.Nodes.Add(item.supplier).Name = item.supplier;
                }
            }
        }

        private void treeView2_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Parent == null)
            {
                string department = e.Node.Text;
                foreach (string subdepartment in Communication.RetrieveProductSubdepartments(department).OrderBy(x => x))
                {
                    TreeNode subdepartmentNode = e.Node.Nodes.Add(subdepartment);
                    TreeNode tempNode = subdepartmentNode.Nodes.Add("Loading....");
                    tempNode.Name = "temp";
                }
                e.Node.Nodes.Remove(e.Node.Nodes.Find("temp", false)[0]);
            }
            else
            {
                string department = e.Node.Parent.Text;
                string subdepartment = e.Node.Text;
                foreach (Item item in Communication.RetrieveItemsFromSubdepartment(subdepartment, department).OrderBy(x => x.productLine).ThenBy(x => x.itemNumber))
                {
                    e.Node.Nodes.Add(
                        item.productLine + " " + item.itemNumber + " - " + item.itemName +
                        " (" + parentForm.currentInvoice.customer.inStorePricingProfile.CalculatePrice(item).ToString("C") + ")");
                }
                e.Node.Nodes.Remove(e.Node.Nodes.Find("temp", false)[0]);
            }
        }

        private void treeView2_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.Nodes.Clear();
            TreeNode tempnode = e.Node.Nodes.Add("Loading...");
            tempnode.Name = "temp";
        }

        private void treeView2_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node == null)
                return;
            if (e.Node.Nodes.Count > 0)
                return;

            string[] split = e.Node.Text.Split('-');
            string productLine = split[0].Trim().Split(' ')[0].Trim();
            string itemNumber = split[0].Trim().Split(' ')[1].Trim();
            parentForm.AddItemFromCatalog(new InvoiceItem(Communication.RetrieveItem(itemNumber, productLine)));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
                return;

            Search();
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1)
                return;

            parentForm.AddItemFromCatalog(new InvoiceItem(dataGridView1.SelectedRows[0].Tag as Item));
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || string.IsNullOrEmpty(textBox1.Text))
                return;

            Search();
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Parent == null)
            {
                if ((bool)e.Node.Tag == true)
                {

                }
            }
            else
            {
                string filter = e.Node.Text;
                string filterType = e.Node.Parent.Text;
                TreeNode parentNode = e.Node.Parent;
                parentNode.Nodes.Clear();
                parentNode.Text = parentNode.Text + " (" + filter + ")";
                parentNode.Tag = true;

                dataGridView1.Rows.Clear();
                foreach (Item item in searchResults)
                {
                    bool addItem = false;
                    switch (filterType.ToLower())
                    {
                        case "departments":
                            if (item.department == filter)
                                addItem = true;
                            break;
                        case "categories":
                            if (item.category == filter)
                                addItem = true;
                            break;
                        case "suppliers":
                            if (item.supplier == filter)
                                addItem = true;
                            break;
                    }
                    if (addItem)
                    {
                        int row = dataGridView1.Rows.Add();
                        dataGridView1.Rows[row].Cells[0].Value = item.productLine;
                        dataGridView1.Rows[row].Cells[1].Value = item.itemNumber;
                        dataGridView1.Rows[row].Cells[2].Value = item.itemName;
                        dataGridView1.Rows[row].Cells[3].Value = item.onHandQty.ToString();
                        dataGridView1.Rows[row].Cells[4].Value = parentForm.currentInvoice.customer.inStorePricingProfile.CalculatePrice(item).ToString("C");
                        dataGridView1.Rows[row].Cells[5].Value = item.UPC;
                        dataGridView1.Rows[row].Tag = item;
                    }
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1)
                return;

            ItemInformation info = new ItemInformation(dataGridView1.SelectedRows[0].Tag as Item);
            info.Show();
        }
    }
}
