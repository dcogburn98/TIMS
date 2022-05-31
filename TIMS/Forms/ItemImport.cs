using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace TIMS.Forms
{
    public partial class ItemImport : Form
    {
        public ItemImport()
        {
            InitializeComponent();
            CancelButton = cancelButton;
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "CSV Files (*.csv)|*.csv";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                csvPathTB.Text = fileDialog.FileName;
                populateButton.Enabled = true;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void populateButton_Click(object sender, EventArgs e)
        {
            //File.Copy(csvPathTB.Text, "tempcsv");
            //string csv = File.ReadAllText("tempcsv");
            //File.Delete("tempcsv");
            //string[] rows = csv.Split('\n');
            //string[] headers = rows[0].Split(',');
            //foreach (string header in headers)
            //{
            //    dataGridView1.Columns.Add(header, header);
            //}
            //for (int i = 1; i != rows.Length; i++)
            //{
            //    string[] cells = rows[i].Split(',');
            //    int row = dataGridView1.Rows.Add();
            //    for (int j = 0; j != cells.Length - 1; j++)
            //    {
            //        dataGridView1.Rows[row].Cells[j].Value = cells[j];
            //    }
            //}
            File.Copy(csvPathTB.Text, "tempcsv", true);
            using (TextFieldParser parser = new TextFieldParser("tempcsv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                int row = 0;
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    if (row == 0)
                        foreach (string header in fields)
                            dataGridView1.Columns.Add(header, header);
                    else
                    {
                        int gridRow = dataGridView1.Rows.Add();
                        for (int i = 0; i != fields.Length; i++)
                            dataGridView1.Rows[gridRow].Cells[i].Value = fields[i];
                    }
                    row++;
                }
            }
            File.Delete("tempcsv");
            importButton.Enabled = true;
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            List<string> availableHeaders = DatabaseHandler.SqlRetrieveTableHeaders("ITEMS");
            List<string> dataHeaders = new List<string>();

            foreach (string header in availableHeaders)
                if (dataGridView1.Columns.Contains(header.ToLower()))
                    dataHeaders.Add(header);

            foreach (string header in dataHeaders)
                availableHeaders.Remove(header);

            foreach (DataGridViewColumn header in dataGridView1.Columns)
                if (!dataHeaders.Contains(header.Name))
                {
                    ItemImportFieldEditor editor = new ItemImportFieldEditor(header.Name, availableHeaders);
                    editor.ShowDialog();
                    dataGridView1.Columns[header.Name].Name = editor.selectedHeader;
                    dataGridView1.Columns[header.Name].HeaderText = editor.selectedHeader;
                    dataHeaders.Add(editor.selectedHeader);
                    availableHeaders.Remove(editor.selectedHeader);
                }

            Item newItem = new Item();
            foreach (DataGridViewRow itemRow in dataGridView1.Rows)
            {
                foreach (DataGridViewCell cell in itemRow.Cells)
                {
                    if (cell.OwningColumn.Name.ToLower() == "productline")
                        newItem.productLine = cell.Value.ToString();
                    if (cell.OwningColumn.Name.ToLower() == "itemnumber")
                        newItem.itemNumber = cell.Value.ToString();
                    if (cell.OwningColumn.Name.ToLower() == "sku")
                        newItem.SKU = cell.Value.ToString();
                    if (cell.OwningColumn.Name.ToLower() == "itemname")
                        newItem.itemName = cell.Value.ToString();
                    if (cell.OwningColumn.Name.ToLower() == "longdescription")
                        newItem.longDescription = cell.Value.ToString();
                    if (cell.OwningColumn.Name.ToLower() == "category")
                        newItem.category = cell.Value.ToString();
                    if (cell.OwningColumn.Name.ToLower() == "onHandquantity")
                        newItem.onHandQty = float.Parse(cell.Value.ToString());
                    if (cell.OwningColumn.Name.ToLower() == "greenprice")
                        newItem.greenPrice = float.Parse(cell.Value.ToString());
                    if (cell.OwningColumn.Name.ToLower() == "replacementcost")
                        newItem.replacementCost = float.Parse(cell.Value.ToString());
                }
                if (newItem.supplier == null || newItem.supplier == string.Empty)
                    newItem.supplier = "Default";
                DatabaseHandler.SqlAddItem(newItem);
            }
            MessageBox.Show("Items Added!");
        }
    }
}
