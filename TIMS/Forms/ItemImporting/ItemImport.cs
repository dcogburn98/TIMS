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

using TIMS.Forms.ItemImporting;
using TIMS.Forms.Maintenance;
using TIMS.Server;
using TIMSServerModel;

namespace TIMS.Forms
{
    public partial class ItemImport : Form
    {
        Item defaultItem;
        public ItemImport()
        {
            InitializeComponent();
            CancelButton = cancelButton;
            foreach (string header in Communication.RetrieveTableHeaders("ITEMS"))
                comboBox1.Items.Add(header);
            
            defaultItem = new Item()
            {
                itemNumber = "XXX",
                productLine = "XXX",
                itemName = "",
                longDescription = "",
                minimum = 0,
                maximum = 0,
                onHandQty = 0,
                WIPQty = 0,
                listPrice = 0,
                redPrice = 0,
                yellowPrice = 0,
                greenPrice = 0,
                pinkPrice = 0,
                bluePrice = 0,
                replacementCost = 0,
                averageCost = 0,
                minimumAge = 0,
                locationCode = 0,
                category = "Default",
                UPC = "",
                manufacturerNumber = "",
                groupCode = 0,
                velocityCode = 0,
                previousYearVelocityCode = 0,
                supplier = "Default",
                itemsPerContainer = 0,
                standardPackage = 0,
                onBackorderQty = 0,
                onOrderQty = 0,
                daysOnBackorder = 0,
                daysOnOrder = 0,
                dateStocked = DateTime.MinValue,
                dateLastReceipt = DateTime.MinValue,
                taxed = true,
                ageRestricted = false,
                serialized = false,
                brand = "Default",
                department = "Default",
                subDepartment = "Default"
            };
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
            List<string> remainingHeaders = Communication.RetrieveTableHeaders("ITEMS");
            comboBox1.Items.Clear();
            foreach (string header in remainingHeaders)
                comboBox1.Items.Add(header);
            using (TextFieldParser parser = new TextFieldParser("tempcsv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                int row = 0;
                List<int> skipped = new List<int>();
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    if (row == 0)
                    {
                        int fieldIndex = 0;
                        foreach (string header in fields)
                        {
                            if (!remainingHeaders.Contains(header))
                            {
                                ItemImportFieldEditor editor = new ItemImportFieldEditor(header, remainingHeaders);
                                editor.ShowDialog();
                                if (editor.selectedHeader != String.Empty)
                                {
                                    remainingHeaders.Remove(editor.selectedHeader);
                                    dataGridView1.Columns.Add(editor.selectedHeader, editor.selectedHeader);
                                    fieldIndex++;
                                }
                                else
                                {
                                    skipped.Add(fieldIndex);
                                    fieldIndex++;
                                }
                            }
                            else
                            {
                                //Automatically add the column to the fields intead of prompting the user for input on which table column it should apply to
                                remainingHeaders.Remove(header);
                                dataGridView1.Columns.Add(header, header);
                                fieldIndex++;
                            }
                        }
                    }
                    else
                    {
                        int gridRow = dataGridView1.Rows.Add();
                        int skipCount = 0;
                        for (int i = 0; i != fields.Length; i++)
                        {
                            if (!skipped.Contains(i))
                                dataGridView1.Rows[gridRow].Cells[i - skipCount].Value = fields[i];
                            else
                                skipCount++;
                        }
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
            MessageBox.Show("You can edit values for individual items in the table provided.\nPlease make sure values entered are suitable for that property type!");
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            bool calculateCheckDigit = false;
            if (MessageBox.Show("Should TIMS calculate the check digit for UPC's only containing 11 digits?", "Question", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                calculateCheckDigit = true;
            }

            List<string> availableHeaders = Communication.RetrieveTableHeaders("ITEMS");
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
            List<Item> itemsAdded = new List<Item>();
            List<DataGridViewRow> skippedRows = new List<DataGridViewRow>();
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = dataGridView1.Rows.Count;
            progressBar1.Step = 1;
            progressBar1.Style = ProgressBarStyle.Continuous;
            foreach (DataGridViewRow itemRow in dataGridView1.Rows)
            {
                Item workingItem = new Item()
                {
                    itemNumber = defaultItem.itemNumber,
                    productLine = defaultItem.productLine,
                    itemName = defaultItem.itemName,
                    longDescription = defaultItem.longDescription,
                    minimum = defaultItem.minimum,
                    maximum = defaultItem.maximum,
                    onHandQty = defaultItem.onHandQty,
                    WIPQty = defaultItem.WIPQty,
                    listPrice = defaultItem.listPrice,
                    redPrice = defaultItem.redPrice,
                    yellowPrice = defaultItem.yellowPrice,
                    greenPrice = defaultItem.greenPrice,
                    pinkPrice = defaultItem.pinkPrice,
                    bluePrice = defaultItem.bluePrice,
                    replacementCost = defaultItem.replacementCost,
                    averageCost = defaultItem.averageCost,
                    minimumAge = defaultItem.minimumAge,
                    locationCode = defaultItem.locationCode,
                    category = defaultItem.category,
                    UPC = defaultItem.UPC,
                    manufacturerNumber = defaultItem.manufacturerNumber,
                    groupCode = defaultItem.groupCode,
                    velocityCode = defaultItem.velocityCode,
                    previousYearVelocityCode = defaultItem.previousYearVelocityCode,
                    supplier = defaultItem.supplier,
                    itemsPerContainer = defaultItem.itemsPerContainer,
                    standardPackage = defaultItem.standardPackage,
                    onBackorderQty = defaultItem.onBackorderQty,
                    onOrderQty = defaultItem.onOrderQty,
                    daysOnBackorder = defaultItem.daysOnBackorder,
                    daysOnOrder = defaultItem.daysOnOrder,
                    dateStocked = defaultItem.dateStocked,
                    dateLastReceipt = defaultItem.dateLastReceipt,
                    taxed = defaultItem.taxed,
                    ageRestricted = defaultItem.ageRestricted,
                    serialized = defaultItem.serialized,
                    brand = defaultItem.brand,
                    department = defaultItem.department,
                    subDepartment = defaultItem.subDepartment
                };
                if (itemRow.Cells[0].Value == null)
                    continue;
                bool skipped = false;

                foreach (DataGridViewCell cell in itemRow.Cells)
                {
                    decimal d;
                    int i;
                    DateTime t;
                    string txt;
                    bool b;
                    switch (cell.OwningColumn.Name.ToLower())
                    {
                        #region String Operations
                        case "productline":
                            workingItem.productLine =
                                cell.Value.ToString() == "" ? defaultItem.productLine : cell.Value.ToString();
                            break;
                        case "itemnumber":
                            workingItem.itemNumber =
                                cell.Value.ToString() == "" ? defaultItem.itemNumber : cell.Value.ToString();
                            break;
                        case "itemname":
                            workingItem.itemName =
                                cell.Value.ToString() == "" ? defaultItem.itemName : cell.Value.ToString();
                            break;
                        case "longdescription":
                            workingItem.longDescription =
                                cell.Value.ToString() == "" ? defaultItem.longDescription : cell.Value.ToString();
                            break;
                        case "supplier":
                            workingItem.supplier =
                                cell.Value.ToString() == "" ? defaultItem.supplier : cell.Value.ToString();
                            break;
                        case "category":
                            workingItem.category =
                                cell.Value.ToString() == "" ? defaultItem.category : cell.Value.ToString();
                            break;
                        case "upc":
                            workingItem.UPC =
                                cell.Value.ToString() == "" ? defaultItem.UPC : cell.Value.ToString();
                            break;
                        case "manufacturernumber":
                            workingItem.manufacturerNumber =
                                cell.Value.ToString() == "" ? defaultItem.manufacturerNumber : cell.Value.ToString();
                            break;
                        #endregion

                        #region Decimal Operations

                        #region Price Operations
                        case "listprice":
                            if (decimal.TryParse(cell.Value.ToString().Trim('$'), out d) == false)
                            {
                                skipped = true;
                                break;
                            }
                            workingItem.listPrice = cell.Value.ToString() == "" ? defaultItem.listPrice : d;
                            break;
                        case "redprice":
                            if (decimal.TryParse(cell.Value.ToString().Trim('$'), out d) == false)
                            {
                                skipped = true;
                                break;
                            }
                            workingItem.redPrice = cell.Value.ToString() == "" ? defaultItem.redPrice : d;
                            break;
                        case "yellowprice":
                            if (decimal.TryParse(cell.Value.ToString().Trim('$'), out d) == false)
                            {
                                skipped = true;
                                break;
                            }
                            workingItem.yellowPrice = cell.Value.ToString() == "" ? defaultItem.yellowPrice : d;
                            break;
                        case "greenprice":
                            if (decimal.TryParse(cell.Value.ToString().Trim('$'), out d) == false)
                            {
                                skipped = true;
                                break;
                            }
                            workingItem.greenPrice = cell.Value.ToString() == "" ? defaultItem.greenPrice : d;
                            break;
                        case "pinkprice":
                            if (decimal.TryParse(cell.Value.ToString().Trim('$'), out d) == false)
                            {
                                skipped = true;
                                break;
                            }
                            workingItem.pinkPrice = cell.Value.ToString() == "" ? defaultItem.pinkPrice : d;
                            break;
                        case "blueprice":
                            if (decimal.TryParse(cell.Value.ToString().Trim('$'), out d) == false)
                            {
                                skipped = true;
                                break;
                            }
                            workingItem.bluePrice = cell.Value.ToString() == "" ? defaultItem.bluePrice : d;
                            break;
                        case "replacementcost":
                            if (decimal.TryParse(cell.Value.ToString().Trim('$'), out d) == false)
                            {
                                skipped = true;
                                break;
                            }
                            workingItem.replacementCost = cell.Value.ToString() == "" ? defaultItem.replacementCost : d;
                            break;
                        case "averagecost":
                            if (decimal.TryParse(cell.Value.ToString().Trim('$'), out d) == false)
                            {
                                skipped = true;
                                break;
                            }
                            workingItem.averageCost = cell.Value.ToString() == "" ? defaultItem.averageCost : d;
                            break;
                        case "lastlabelprice":
                            if (decimal.TryParse(cell.Value.ToString().Trim('$'), out d) == false)
                            {
                                skipped = true;
                                break;
                            }
                            workingItem.lastLabelPrice = cell.Value.ToString() == "" ? defaultItem.lastLabelPrice : d;
                            break;
                        case "lastsaleprice":
                            if (decimal.TryParse(cell.Value.ToString().Trim('$'), out d) == false)
                            {
                                skipped = true;
                                break;
                            }
                            workingItem.lastSalePrice = cell.Value.ToString() == "" ? defaultItem.lastSalePrice : d;
                            break;
                        #endregion

                        case "minimum":
                            if (decimal.TryParse(cell.Value.ToString(), out d) == false)
                            {
                                skipped = true;
                                break;
                            }
                            workingItem.minimum = cell.Value.ToString() == "" ? defaultItem.minimum : d;
                            break;
                        case "maximum":
                            if (decimal.TryParse(cell.Value.ToString(), out d) == false)
                            {
                                skipped = true;
                                break;
                            }
                            workingItem.maximum = cell.Value.ToString() == "" ? defaultItem.maximum : d;
                            break;
                        case "onhandquantity":
                            if (decimal.TryParse(cell.Value.ToString(), out d) == false)
                            {
                                skipped = true;
                                break;
                            }
                            workingItem.onHandQty = cell.Value.ToString() == "" ? defaultItem.onHandQty : d;
                            break;
                        case "wipquantity":
                            if (decimal.TryParse(cell.Value.ToString(), out d) == false)
                            {
                                skipped = true;
                                break;
                            }
                            workingItem.WIPQty = cell.Value.ToString() == "" ? defaultItem.WIPQty : d;
                            break;
                        case "onorderquantity":
                            if (decimal.TryParse(cell.Value.ToString(), out d) == false)
                            {
                                skipped = true;
                                break;
                            }
                            workingItem.onOrderQty = cell.Value.ToString() == "" ? defaultItem.onOrderQty : d;
                            break;
                        case "backorderquantity":
                            if (decimal.TryParse(cell.Value.ToString(), out d) == false)
                            {
                                skipped = true;
                                break;
                            }
                            workingItem.onBackorderQty = cell.Value.ToString() == "" ? defaultItem.onBackorderQty : d;
                            break;
                        
                        #endregion

                        #region Int Operations
                        case "groupcode":
                            if (int.TryParse(cell.Value.ToString(), out i) == false)
                            {
                                skipped = true;
                                break;
                            }
                            workingItem.groupCode =
                                cell.Value.ToString() == "" ? defaultItem.groupCode : i;
                            break;
                        case "velocitycode":
                            if (int.TryParse(cell.Value.ToString(), out i) == false)
                            {
                                skipped = true;
                                break;
                            }
                            workingItem.velocityCode =
                                cell.Value.ToString() == "" ? defaultItem.velocityCode : i;
                            break;
                        case "previousyearvelocitycode":
                            if (int.TryParse(cell.Value.ToString(), out i) == false)
                            {
                                skipped = true;
                                break;
                            }
                            workingItem.previousYearVelocityCode =
                                cell.Value.ToString() == "" ? defaultItem.previousYearVelocityCode : i;
                            break;
                        case "itemspercontainer":
                            if (int.TryParse(cell.Value.ToString(), out i) == false)
                            {
                                skipped = true;
                                break;
                            }
                            workingItem.itemsPerContainer = 
                                cell.Value.ToString() == "" ? defaultItem.itemsPerContainer : i;
                            break;
                        case "standardpackage":
                            if (int.TryParse(cell.Value.ToString(), out i) == false)
                            {
                                skipped = true;
                                break;
                            }
                            workingItem.standardPackage = 
                                cell.Value.ToString() == "" ? defaultItem.standardPackage : i;
                            break;
                        case "daysonorder":
                            if (int.TryParse(cell.Value.ToString(), out i) == false)
                            {
                                skipped = true;
                                break;
                            }
                            workingItem.daysOnOrder =
                                cell.Value.ToString() == "" ? defaultItem.daysOnOrder : i;
                            break;
                        case "daysonbackorder":
                            if (int.TryParse(cell.Value.ToString(), out i) == false)
                            {
                                skipped = true;
                                break;
                            }
                            workingItem.daysOnBackorder =
                                cell.Value.ToString() == "" ? defaultItem.daysOnBackorder : i;
                            break;
                        case "minimumage":
                            if (int.TryParse(cell.Value.ToString(), out i) == false)
                            {
                                skipped = true;
                                break;
                            }
                            workingItem.minimumAge =
                                cell.Value.ToString() == "" ? defaultItem.minimumAge : i;
                            break;
                        case "locationcode":
                            if (int.TryParse(cell.Value.ToString(), out i) == false)
                            {
                                skipped = true;
                                break;
                            }
                            workingItem.locationCode =
                                cell.Value.ToString() == "" ? defaultItem.locationCode : i;
                            break;
                        #endregion

                        #region Boolean Operations
                        case "taxed":
                            txt = cell.Value.ToString();
                            if (txt.ToLower() == "true" || txt == "1")
                                b = true;
                            else
                                b = false;
                            workingItem.taxed = cell.Value.ToString() == "" ? defaultItem.taxed : b;
                            break;
                        case "agerestricted":
                            txt = cell.Value.ToString();
                            if (txt.ToLower() == "true" || txt == "1")
                                b = true;
                            else
                                b = false;
                            workingItem.ageRestricted = cell.Value.ToString() == "" ? defaultItem.ageRestricted : b;
                            break;
                        case "serialized":
                            txt = cell.Value.ToString();
                            if (txt.ToLower() == "true" || txt == "1")
                                b = true;
                            else
                                b = false;
                            workingItem.serialized = cell.Value.ToString() == "" ? defaultItem.serialized : b;
                            break;
                        #endregion

                        #region Date Operations
                        case "datestocked":
                            if (DateTime.TryParse(cell.Value.ToString(), out t) == false)
                            {
                                skipped = true;
                                break;
                            }
                            workingItem.dateStocked =
                                cell.Value.ToString() == "" ? defaultItem.dateStocked : t;
                            break;
                        case "datelastreceipt":
                            if (DateTime.TryParse(cell.Value.ToString(), out t) == false)
                            {
                                skipped = true;
                                break;
                            }
                            workingItem.dateLastReceipt =
                                cell.Value.ToString() == "" ? defaultItem.dateLastReceipt : t;
                            break;
                        case "lastlabeldate":
                            if (DateTime.TryParse(cell.Value.ToString(), out t) == false)
                            {
                                skipped = true;
                                break;
                            }
                            workingItem.lastLabelDate =
                                cell.Value.ToString() == "" ? defaultItem.lastLabelDate : t;
                            break;
                        case "datelastsale":
                            if (DateTime.TryParse(cell.Value.ToString(), out t) == false)
                            {
                                skipped = true;
                                break;
                            }
                            workingItem.dateLastSale =
                                cell.Value.ToString() == "" ? defaultItem.dateLastSale : t;
                            break;
                        #endregion
                    }
                }

                if (skipped)
                {
                    skippedRows.Add(itemRow);
                    skipped = false;
                    rowsCompleted++;
                    progressBar1.PerformStep();
                    continue;
                }

                if (string.IsNullOrEmpty(workingItem.supplier))
                    workingItem.supplier = "Default";

                if (workingItem.UPC != "")
                {
                    if (Program.IsStringNumeric(workingItem.UPC) && workingItem.UPC.Length == 11 && calculateCheckDigit)
                        workingItem.UPC = UPCA.CalculateChecksumDigit(workingItem.UPC);
                    Communication.AddBarcode(workingItem.itemNumber, workingItem.productLine, workingItem.UPC, 1);
                }

                if (!Communication.CheckProductLine(workingItem.productLine.ToUpper()))
                    Communication.AddProductLine(workingItem.productLine.ToUpper());

                if (workingItem.supplier != null &&!Communication.RetrieveSuppliers().Contains(workingItem.supplier))
                    Communication.AddSupplier(workingItem.supplier);

                if (workingItem.category != null && !Communication.RetrieveProductCategories().Contains(workingItem.category))
                    Communication.AddProductCategory(workingItem.category);

                if (workingItem.department != null && !Communication.RetrieveProductDepartments().Contains(workingItem.department))
                    Communication.AddProductDepartment(workingItem.department);

                if (workingItem.department != null && workingItem.subDepartment != null && !Communication.RetrieveProductSubdepartments(workingItem.department).Contains(workingItem.subDepartment))
                    Communication.AddProductSubdepartment(workingItem.department, workingItem.subDepartment);

                if (Communication.RetrieveItem(workingItem.itemNumber, workingItem.productLine) == null)
                {
                    if (!Communication.AddItem(workingItem))
                        skippedRows.Add(itemRow);
                    else
                        itemsAdded.Add(workingItem);
                }
                else skippedRows.Add(itemRow);

                rowsCompleted++;
                progressBar1.PerformStep();
            }

            decimal inventoryAmtAdjustment = 0;
            foreach (Item item in itemsAdded)
                inventoryAmtAdjustment += item.onHandQty * item.replacementCost;

            if (inventoryAmtAdjustment != 0)
            {
                InventoryUpdateAccountPicker picker = new InventoryUpdateAccountPicker(inventoryAmtAdjustment, true);
                while (picker.ShowDialog() != DialogResult.OK)
                {
                    MessageBox.Show("You must select an account to offset the inventory value\nbeing reflected by this item import session!");
                }
                Account adjustmentAccount = picker.selectedAccount;
                if (inventoryAmtAdjustment > 0) //If the difference is positive, we will debit (increase) the inventory account
                {
                    Communication.SaveTransaction(new Transaction(1, adjustmentAccount.ID, inventoryAmtAdjustment)
                    {
                        transactionID = Communication.RetrieveNextTransactionNumber(),
                        memo = "Inventory amount adjustment for item import session"
                    });
                }
                else //, credit (decrease) the inventory account
                {
                    Communication.SaveTransaction(new Transaction(adjustmentAccount.ID, 1, Math.Abs(inventoryAmtAdjustment))
                    {
                        transactionID = Communication.RetrieveNextTransactionNumber(),
                        memo = "Inventory amount adjustment for item import session"
                    });
                }
            }

            MessageBox.Show("Items Added!\nItems skipped: " + skippedRows.Count + "\n\n" +
                "Skipped items will be displayed in the import window.");

            dataGridView1.Rows.Clear();
            foreach (DataGridViewRow row in skippedRows)
            {
                dataGridView1.Rows.Add(row);
            }

            progressBar1.Value = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
                return;

            dataGridView1.Columns.Add(comboBox1.Text, comboBox1.Text);
            comboBox1.Items.Remove(comboBox1.SelectedItem);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DefaultFieldValuesEditor editor = new DefaultFieldValuesEditor(defaultItem);
            if (editor.ShowDialog() == DialogResult.OK)
                defaultItem = editor.defaultItem;
        }
    }
}
