using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TIMS.Server;
using TIMSServerModel;

namespace TIMS.Forms.Maintenance
{
    public partial class ItemMaintenance : Form
    {
        bool alreadyChoseToCreateNewItem = false;
        bool changedProductLine = false;
        Item workingItem;

        public ItemMaintenance()
        {
            InitializeComponent();
            ClearAllItemFields();
        }

        private void SelectProductLine()
        {
            workingItem = Communication.RetrieveItem(itemNumberTB.Text, productLineComboBox.Text);
            PopulateItemInfoFields();
            saveItemButton.Enabled = true;
        }

        private void ClearAllItemFields()
        {
            workingItem = null;
            itemDescriptionTB.Text = string.Empty;
            saveItemButton.Enabled = false;

            productLineTBField.Text = string.Empty;
            itemNumberTBField.Text = string.Empty;
            itemNameTB.Text = string.Empty;
            descriptionTB.Text = string.Empty;
            supplierCB.Text = string.Empty;
            groupCodeTB.Text = string.Empty;
            velocityCodeCB.Text = string.Empty;
            velocityCodeCB.Items.Clear();
            prevYearVelocityCodeCB.Text = string.Empty;
            prevYearVelocityCodeCB.Items.Clear();
            standardPkgTB.Text = string.Empty;
            categoryCB.Text = string.Empty;
            categoryCB.Items.Clear();
            departmentCB.Text = string.Empty;
            departmentCB.Items.Clear();
            subDepartmentCB.Text = string.Empty;
            subDepartmentCB.Items.Clear();
            brandCB.Text = string.Empty;
            brandCB.Items.Clear();
            unitTB.Text = string.Empty;
            taxableCB.Checked = false;

            dateStockedTB.Text = string.Empty;
            lastReceiptTB.Text = string.Empty;
            minTB.Text = string.Empty;
            maxTB.Text = string.Empty;
            onHandTB.Text = string.Empty;
            wipQtyTB.Text = string.Empty;
            onOrderQtyTB.Text = string.Empty;
            onBackorderQtyTB.Text = string.Empty;
            daysOnBackOrderTB.Text = string.Empty;
            daysOnOrderTB.Text = string.Empty;

            listPriceTB.Text = string.Empty;
            redPriceTB.Text = string.Empty;
            yellowPriceTB.Text = string.Empty;
            greenPriceTB.Text = string.Empty;
            pinkPriceTB.Text = string.Empty;
            bluePriceTB.Text = string.Empty;
            costTB.Text = string.Empty;
            avgCostTB.Text = string.Empty;

            ageRestrictedCB.Checked = false;
            minimumAgeTB.Text = "";

            lastLabelDateTB.Text = string.Empty;
            lastLabelPriceTB.Text = string.Empty;
            locationsLB.Items.Clear();

            serializedCB.Checked = false;
            serialNumberTB.Text = string.Empty;
            serialNumbersLB.Items.Clear();
            removerSNbtn.Enabled = false;

            dateOfLastSaleTB.Text = string.Empty;

            itemNumberTB.Text = string.Empty;
            productLineComboBox.Items.Clear();
            productLineComboBox.Text = string.Empty;

            //Disable all fields
            itemDescriptionTB.Enabled = false;

            productLineTBField.Enabled = false;
            itemNumberTBField.Enabled = false;
            itemNameTB.Enabled = false;
            descriptionTB.Enabled = false;
            supplierCB.Enabled = false;
            groupCodeTB.Enabled = false;
            velocityCodeCB.Enabled = false;
            prevYearVelocityCodeCB.Enabled = false;
            standardPkgTB.Enabled = false;
            categoryCB.Enabled = false;
            brandCB.Enabled = false;
            departmentCB.Enabled = false;
            subDepartmentCB.Enabled = false;
            unitTB.Enabled = false;
            taxableCB.Enabled = false;

            dateStockedTB.Enabled = false;
            lastReceiptTB.Enabled = false;
            minTB.Enabled = false;
            maxTB.Enabled = false;
            onHandTB.Enabled = false;
            wipQtyTB.Enabled = false;
            onOrderQtyTB.Enabled = false;
            onBackorderQtyTB.Enabled = false;
            daysOnBackOrderTB.Enabled = false;
            daysOnOrderTB.Enabled = false;

            listPriceTB.Enabled = false;
            redPriceTB.Enabled = false;
            yellowPriceTB.Enabled = false;
            greenPriceTB.Enabled = false;
            pinkPriceTB.Enabled = false;
            bluePriceTB.Enabled = false;
            costTB.Enabled = false;
            avgCostTB.Enabled = false;

            ageRestrictedCB.Enabled = false;
            minimumAgeTB.Enabled = false;

            lastLabelDateTB.Enabled = false;
            lastLabelPriceTB.Enabled = false;

            serializedCB.Enabled = false;
            serialNumberTB.Enabled = false;
            removerSNbtn.Enabled = false;

            dateOfLastSaleTB.Enabled = false;
        }

        private void PopulateItemInfoFields()
        {
            supplierCB.Items.Clear();
            foreach (string supplier in Communication.RetrieveSuppliers())
            {
                supplierCB.Items.Add(supplier);
            }
            brandCB.Items.Clear();
            foreach (string brand in Communication.RetrieveProductBrands())
            {
                brandCB.Items.Add(brand);
            }
            categoryCB.Items.Clear();
            foreach (string category in Communication.RetrieveProductCategories())
            {
                categoryCB.Items.Add(category);
            }
            departmentCB.Items.Clear();
            foreach (string department in Communication.RetrieveProductDepartments())
            {
                departmentCB.Items.Add(department);
            }
            subDepartmentCB.Items.Clear();
            if (!string.IsNullOrEmpty(workingItem.department))
            {
                foreach (string subdepartment in Communication.RetrieveProductSubdepartments(workingItem.department))
                {
                    subDepartmentCB.Items.Add(subdepartment);
                }
            }

            itemNameTB.Focus();
            itemDescriptionTB.Text = workingItem.itemName;

            productLineTBField.Text = workingItem.productLine;
            itemNumberTBField.Text = workingItem.itemNumber;
            itemNameTB.Text = workingItem.itemName;
            supplierCB.Text = workingItem.supplier;
            groupCodeTB.Text = workingItem.groupCode.ToString();
            velocityCodeCB.Text = workingItem.velocityCode.ToString();
            prevYearVelocityCodeCB.Text = workingItem.previousYearVelocityCode.ToString();
            brandCB.Text = workingItem.brand;
            categoryCB.Text = workingItem.category;
            standardPkgTB.Text = workingItem.standardPackage.ToString();
            taxableCB.Checked = workingItem.taxed;
            descriptionTB.Text = workingItem.longDescription;
            departmentCB.Text = workingItem.department;
            subDepartmentCB.Text = workingItem.subDepartment;

            dateStockedTB.Text = workingItem.dateStocked.ToString("MM/dd/yyyy");
            lastReceiptTB.Text = workingItem.dateLastReceipt.ToString("MM/dd/yyyy");
            minTB.Text = workingItem.minimum.ToString();
            maxTB.Text = workingItem.maximum.ToString();
            onHandTB.Text = workingItem.onHandQty.ToString();
            wipQtyTB.Text = workingItem.WIPQty.ToString();
            onOrderQtyTB.Text = workingItem.onOrderQty.ToString();
            onBackorderQtyTB.Text = workingItem.onBackorderQty.ToString();
            daysOnOrderTB.Text = workingItem.daysOnOrder.ToString();
            daysOnBackOrderTB.Text = workingItem.daysOnBackorder.ToString();

            listPriceTB.Text = workingItem.listPrice.ToString();
            redPriceTB.Text = workingItem.redPrice.ToString();
            yellowPriceTB.Text = workingItem.yellowPrice.ToString();
            greenPriceTB.Text = workingItem.greenPrice.ToString();
            pinkPriceTB.Text = workingItem.pinkPrice.ToString();
            bluePriceTB.Text = workingItem.bluePrice.ToString();
            costTB.Text = workingItem.replacementCost.ToString();
            avgCostTB.Text = workingItem.averageCost.ToString();

            ageRestrictedCB.Checked = workingItem.ageRestricted;
            minimumAgeTB.Text = workingItem.minimumAge.ToString();

            //Enable all fields
            itemDescriptionTB.Enabled = true;

            productLineTBField.Enabled = true;
            itemNumberTBField.Enabled = true;
            itemNameTB.Enabled = true;
            descriptionTB.Enabled = true;
            supplierCB.Enabled = true;
            groupCodeTB.Enabled = true;
            velocityCodeCB.Enabled = true;
            prevYearVelocityCodeCB.Enabled = true;
            standardPkgTB.Enabled = true;
            categoryCB.Enabled = true;
            brandCB.Enabled = true;
            departmentCB.Enabled = true;
            subDepartmentCB.Enabled = true;
            unitTB.Enabled = true;
            taxableCB.Enabled = true;

            dateStockedTB.Enabled = true;
            lastReceiptTB.Enabled = true;
            minTB.Enabled = true;
            maxTB.Enabled = true;
            onHandTB.Enabled = true;
            wipQtyTB.Enabled = true;
            onOrderQtyTB.Enabled = true;
            onBackorderQtyTB.Enabled = true;
            daysOnBackOrderTB.Enabled = true;
            daysOnOrderTB.Enabled = true;

            listPriceTB.Enabled = true;
            redPriceTB.Enabled = true;
            yellowPriceTB.Enabled = true;
            greenPriceTB.Enabled = true;
            pinkPriceTB.Enabled = true;
            bluePriceTB.Enabled = true;
            costTB.Enabled = true;
            avgCostTB.Enabled = true;

            ageRestrictedCB.Enabled = true;
            minimumAgeTB.Enabled = true;

            lastLabelDateTB.Enabled = true;
            lastLabelPriceTB.Enabled = true;

            serializedCB.Enabled = true;
            serialNumberTB.Enabled = true;
            removerSNbtn.Enabled = true;

            dateOfLastSaleTB.Enabled = true;

            itemNumberTB.Enabled = true;
            productLineComboBox.Enabled = true;
        }

        private void productLineComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar))
            {
                changedProductLine = true;
                e.KeyChar = char.ToUpper(e.KeyChar);
            }
        }

        private void itemNumberTB_Leave(object sender, EventArgs e)
        {
            if (itemNumberTB.Text == "")
                return;

            productLineComboBox.Items.Clear();
            List<Item> items = Communication.CheckItemNumber(itemNumberTB.Text, false);
            if (items == null)
                return;

            foreach (Item item in items)
            {
                productLineComboBox.Items.Add(item.productLine);
            }

            //if (productLineComboBox.Items.Count == 1)
            //{
            //    productLineComboBox.SelectedIndex = 0;
            //    SelectProductLine();
            //    itemNameTB.Focus();
            //    return;
            //}

            productLineComboBox.DroppedDown = true;
        }

        private void itemNumberTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                productLineComboBox.Focus();
            }
        }

        private void productLineComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                itemNumberTB.Focus();
        }

        private void saveItemButton_Click(object sender, EventArgs e)
        {
            if (workingItem == null)
            {
                return;
            }
            if (ageRestrictedCB.Checked && minimumAgeTB.Text == "")
            {
                MessageBox.Show("Minimum age is required for age restricted items!");
                minimumAgeTB.Focus();
                return;
            }

            Item newItem = new Item();
            newItem.productLine = productLineTBField.Text == string.Empty ? "XXX" : productLineTBField.Text;
            newItem.itemNumber = itemNumberTBField.Text == string.Empty ? "XXX" : itemNumberTBField.Text;
            newItem.itemName = itemNameTB.Text;
            newItem.supplier = supplierCB.Text;
            newItem.groupCode = int.TryParse(groupCodeTB.Text, out int i) == false ? 0 : i;
            newItem.velocityCode = int.TryParse(velocityCodeCB.Text, out i) == false ? 0 : i;
            newItem.previousYearVelocityCode = int.TryParse(prevYearVelocityCodeCB.Text, out i) == false ? 0 : i;
            newItem.category = categoryCB.Text;
            newItem.department = departmentCB.Text;
            newItem.subDepartment = subDepartmentCB.Text;
            newItem.brand = brandCB.Text;
            newItem.standardPackage = int.TryParse(standardPkgTB.Text, out i) == false ? 0 : i;
            newItem.taxed = taxableCB.Checked;
            newItem.dateStocked = DateTime.TryParse(dateStockedTB.Text, out DateTime date) == false ? DateTime.MinValue : date;
            newItem.dateLastReceipt = DateTime.TryParse(lastReceiptTB.Text, out date) == false ? DateTime.MinValue : date;
            newItem.minimum = decimal.TryParse(minTB.Text, out decimal j) == false ? 0 : j;
            newItem.maximum = decimal.TryParse(maxTB.Text, out j) == false ? 0 : j;
            newItem.onHandQty = decimal.TryParse(onHandTB.Text, out j) == false ? 0 : j;
            newItem.WIPQty = decimal.TryParse(wipQtyTB.Text, out j) == false ? 0 : j;
            newItem.onOrderQty = decimal.TryParse(onOrderQtyTB.Text, out j) == false ? 0 : j;
            newItem.onBackorderQty = decimal.TryParse(onBackorderQtyTB.Text, out j) == false ? 0 : j;
            newItem.daysOnOrder = int.TryParse(daysOnOrderTB.Text, out i) == false ? 0 : i;
            newItem.daysOnBackorder = int.TryParse(daysOnBackOrderTB.Text, out i) == false ? 0 : i;
            newItem.listPrice = decimal.TryParse(listPriceTB.Text, out j) == false ? 0 : j;
            newItem.redPrice = decimal.TryParse(redPriceTB.Text, out j) == false ? 0 : j;
            newItem.yellowPrice = decimal.TryParse(yellowPriceTB.Text, out j) == false ? 0 : j;
            newItem.greenPrice = decimal.TryParse(greenPriceTB.Text, out j) == false ? 0 : j;
            newItem.pinkPrice = decimal.TryParse(pinkPriceTB.Text, out j) == false ? 0 : j;
            newItem.bluePrice = decimal.TryParse(bluePriceTB.Text, out j) == false ? 0 : j;
            newItem.replacementCost = decimal.TryParse(costTB.Text, out j) == false ? 0 : j;
            newItem.averageCost = decimal.TryParse(avgCostTB.Text, out j) == false ? 0 : j;
            newItem.ageRestricted = ageRestrictedCB.Checked;
            newItem.minimumAge = int.TryParse(minimumAgeTB.Text, out i) == false ? 0 : i;
            newItem.longDescription = descriptionTB.Text;
            newItem.serialized = serializedCB.Checked;

            Item oldItem = Communication.RetrieveItem(itemNumberTBField.Text == string.Empty ? "XXX" : itemNumberTBField.Text, productLineTBField.Text == string.Empty ? "XXX" : productLineTBField.Text);
            decimal difference;
            if (oldItem == null)
            {
                newItem.manufacturerNumber = "";
                difference = newItem.onHandQty * newItem.replacementCost;
                if (difference != 0)
                {
                    InventoryUpdateAccountPicker picker = new InventoryUpdateAccountPicker(difference, false);
                    if (picker.ShowDialog() == DialogResult.OK)
                    {
                        Account adjustmentAccount = picker.selectedAccount;
                        if (difference > 0) //If the difference is positive, we will debit (increase) the inventory account
                        {
                            Communication.SaveTransaction(new Transaction(1, adjustmentAccount.ID, difference)
                            {
                                transactionID = Communication.RetrieveNextTransactionNumber(),
                                memo = "Inventory amount adjustment for item \"" + newItem.productLine + " " + newItem.itemNumber + "\""
                            });
                        }
                        else //, credit (decrease) the inventory account
                        {
                            Communication.SaveTransaction(new Transaction(adjustmentAccount.ID, 1, Math.Abs(difference))
                            {
                                transactionID = Communication.RetrieveNextTransactionNumber(),
                                memo = "Inventory amount adjustment for item \"" + newItem.productLine + " " + newItem.itemNumber + "\""
                            });
                        }
                    }
                    else
                        return;
                }
            }
            else
            {
                newItem.manufacturerNumber = oldItem.manufacturerNumber;
                difference = (newItem.onHandQty * newItem.replacementCost) - (oldItem.onHandQty * oldItem.replacementCost);
                if (difference != 0)
                {
                    InventoryUpdateAccountPicker picker = new InventoryUpdateAccountPicker(difference, false);
                    if (picker.ShowDialog() == DialogResult.OK)
                    {
                        Account adjustmentAccount = picker.selectedAccount;
                        if (difference > 0) //If the difference is positive, we will debit (increase) the inventory account
                        {
                            Communication.SaveTransaction(new Transaction(1, adjustmentAccount.ID, difference)
                            {
                                transactionID = Communication.RetrieveNextTransactionNumber(),
                                memo = "Inventory amount adjustment for item \"" + newItem.productLine + " " + newItem.itemNumber + "\""
                            });
                        }
                        else //, credit (decrease) the inventory account
                        {
                            Communication.SaveTransaction(new Transaction(adjustmentAccount.ID, 1, Math.Abs(difference))
                            {
                                transactionID = Communication.RetrieveNextTransactionNumber(),
                                memo = "Inventory amount adjustment for item \"" + newItem.productLine + " " + newItem.itemNumber + "\""
                            });
                        }
                    }
                    else
                        return;
                }
            }

            if (!string.IsNullOrEmpty(newItem.category) && !Communication.RetrieveProductCategories().Contains(newItem.category))
            {
                Communication.AddProductCategory(newItem.category);
            }
            if (!string.IsNullOrEmpty(newItem.department) && !Communication.RetrieveProductDepartments().Contains(newItem.department))
            {
                Communication.AddProductDepartment(newItem.department);
            }
            if (!string.IsNullOrEmpty(newItem.brand) && !Communication.RetrieveProductBrands().Contains(newItem.brand))
            {
                Communication.AddProductBrand(newItem.brand);
            }
            if (!string.IsNullOrEmpty(newItem.department) && !string.IsNullOrEmpty(newItem.subDepartment) && !Communication.RetrieveProductSubdepartments(newItem.department).Contains(newItem.subDepartment))
            {
                Communication.AddProductSubdepartment(newItem.department, newItem.subDepartment);
            }

            Communication.UpdateItem(newItem);
            MessageBox.Show("Item updated!");
        }

        private void clearItemButton_Click(object sender, EventArgs e)
        {
            ClearAllItemFields();
            itemNumberTB.Focus();
            alreadyChoseToCreateNewItem = false;
        }

        private void ItemMaintenance_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.OpenForms.Remove(this);
            Program.CheckOpenForms();
        }

        private void productLineComboBox_Leave(object sender, EventArgs e)
        {
            if ((!changedProductLine && workingItem != null) || alreadyChoseToCreateNewItem || productLineComboBox.Text == workingItem?.productLine.ToUpper() || productLineComboBox.Text == "")
            {
                alreadyChoseToCreateNewItem = false;
                return;
            }

            if (int.TryParse(productLineComboBox.Text, out int i))
            {
                if (i > productLineComboBox.Items.Count || i == 0)
                    return;
                else
                {
                    productLineComboBox.SelectedIndex = i - 1;
                    SelectProductLine();
                }
            }
            else
            {
                if (productLineComboBox.Items.Contains(productLineComboBox.Text))
                {
                    productLineComboBox.SelectedIndex = productLineComboBox.Items.IndexOf(productLineComboBox.Text);
                    SelectProductLine();
                }
                else
                {
                    string pl = productLineComboBox.Text;
                    string itemNo = itemNumberTB.Text;
                    if (MessageBox.Show(@"Item number """ + itemNumberTB.Text + @""" does not exist in product line " + productLineComboBox.Text + "!\nWould you like to add it?", "Add product?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        alreadyChoseToCreateNewItem = true;
                        workingItem = new Item() { itemNumber = itemNo, productLine = pl, taxed = true };
                        PopulateItemInfoFields();
                        productLineComboBox.Text = pl;
                        saveItemButton.Enabled = true;
                    }
                }
            }
        }

        private void departmentCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void departmentCB_TextChanged(object sender, EventArgs e)
        {
            subDepartmentCB.Text = "";
            subDepartmentCB.Items.Clear();

            if (departmentCB.SelectedIndex == -1)
                return;

            foreach (string subdepartment in Communication.RetrieveProductSubdepartments(departmentCB.SelectedItem as string))
            {
                subDepartmentCB.Items.Add(subdepartment);
            }
        }
    }
}
