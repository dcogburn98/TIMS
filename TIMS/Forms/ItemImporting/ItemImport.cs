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
            foreach (string header in DatabaseHandler.SqlRetrieveTableHeaders("ITEMS"))
                comboBox1.Items.Add(header);
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
            File.Copy(csvPathTB.Text, "tempcsv", true);
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            List<string> remainingHeaders = DatabaseHandler.SqlRetrieveTableHeaders("ITEMS");
            comboBox1.Items.Clear();
            foreach (string header in remainingHeaders)
                comboBox1.Items.Add(header);
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
                        {
                            if (!remainingHeaders.Contains(header))
                            {
                                ItemImportFieldEditor editor = new ItemImportFieldEditor(header, remainingHeaders);
                                editor.ShowDialog();
                                remainingHeaders.Remove(editor.selectedHeader);
                                dataGridView1.Columns.Add(editor.selectedHeader, editor.selectedHeader);
                            }
                            else
                            {
                                remainingHeaders.Remove(header);
                                dataGridView1.Columns.Add(header, header);
                            }
                        }
                    else
                    {
                        int gridRow = dataGridView1.Rows.Add();
                        for (int i = 0; i != fields.Length; i++)
                            dataGridView1.Rows[gridRow].Cells[i].Value = fields[i];
                    }
                    row++;
                }
            }
            foreach (DataGridViewColumn col in dataGridView1.Columns)
                comboBox1.Items.Remove(col.Name);
            comboBox1.Enabled = true;
            button1.Enabled = true;
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

            int rowsCompleted = 0;
            List<DataGridViewRow> skippedRows = new List<DataGridViewRow>();
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = dataGridView1.Rows.Count;
            progressBar1.Step = 1;
            progressBar1.Style = ProgressBarStyle.Continuous;
            foreach (DataGridViewRow itemRow in dataGridView1.Rows)
            {
                Item newItem = new Item() {
                    productLine = "XXX",
                    itemNumber = "XXX",
                    ageRestricted = false,
                    minimumAge = 0,
                    minimum = 0,
                    maximum = 0,
                    averageCost = 0,
                    replacementCost = 0,
                    bluePrice = 0,
                    greenPrice = 0,
                    listPrice = 0,
                    pinkPrice = 0,
                    redPrice = 0,
                    yellowPrice = 0,
                    previousYearVelocityCode = 0,
                    velocityCode = 0,
                    itemsPerContainer = 0,
                    standardPackage = 0,
                    WIPQty = 0,
                    category = "Default",
                    dateLastReceipt = DateTime.Now,
                    dateStocked = DateTime.Now,
                    locationCode = 0,
                    longDescription = "",
                    daysOnBackorder = 0,
                    daysOnOrder = 0,
                    groupCode = 0,
                    onBackorderQty = 0,
                    onHandQty = 0,
                    onOrderQty = 0,
                    itemName = "XXX",
                    serialized = false,
                    SKU = "",
                    supplier = "Default",
                    taxed = true
                };
                if (itemRow.Cells[0].Value == null)
                    continue;
                //System.Threading.Thread.Sleep(50);
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
                        newItem.onHandQty = decimal.Parse(cell.Value.ToString());
                    if (cell.OwningColumn.Name.ToLower() == "greenprice")
                        newItem.greenPrice = decimal.Parse(cell.Value.ToString());
                    if (cell.OwningColumn.Name.ToLower() == "replacementcost")
                        newItem.replacementCost = decimal.Parse(cell.Value.ToString());
                }
                if (newItem.supplier == null || newItem.supplier == string.Empty)
                    newItem.supplier = "Default";
                if (DatabaseHandler.SqlRetrieveItem(newItem.itemNumber, newItem.productLine) == null)
                {
                    if (!DatabaseHandler.SqlAddItem(newItem))
                        skippedRows.Add(itemRow);
                }
                else skippedRows.Add(itemRow);

                rowsCompleted++;
                progressBar1.PerformStep();
            }
            MessageBox.Show("Items Added!\nItems skipped: " + skippedRows.Count);

            dataGridView1.Rows.Clear();
            foreach (DataGridViewRow row in skippedRows)
            {
                dataGridView1.Rows.Add(row);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
                return;

            dataGridView1.Columns.Add(comboBox1.Text, comboBox1.Text);
            comboBox1.Items.Remove(comboBox1.SelectedItem);
        }
    }
}
