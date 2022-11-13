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

namespace TIMS.Forms.Customers
{
    public partial class PricingProfileEditor : Form
    {
        public PricingProfile profile;
        public bool newProfile = false;
        public bool profileEdited = false;

        public PricingProfileEditor(PricingProfile profile)
        {
            InitializeComponent();
            this.profile = profile;

            textBox2.Text = profile.ProfileID.ToString();
            textBox1.Text = profile.ProfileName;

            foreach (PricingProfileElement el in profile.Elements)
            {
                int row = dataGridView1.Rows.Add();
                (dataGridView1.Rows[row].Cells[5] as DataGridViewComboBoxCell).Items.AddRange("Cost", "Red", "Yellow", "Green", "Pink", "Blue");
                dataGridView1.Rows[row].Cells[0].Value = el.groupCode;
                dataGridView1.Rows[row].Cells[1].Value = el.department;
                dataGridView1.Rows[row].Cells[2].Value = el.subDepartment;
                dataGridView1.Rows[row].Cells[3].Value = el.productLine;
                dataGridView1.Rows[row].Cells[4].Value = el.itemNumber;
                dataGridView1.Rows[row].Cells[5].Value = Enum.GetName(typeof(PricingProfileElement.PriceSheets), el.priceSheet);
                dataGridView1.Rows[row].Cells[6].Value = el.margin.ToString();
                dataGridView1.Rows[row].Cells[7].Value = el.beginDate?? null;
                dataGridView1.Rows[row].Cells[8].Value = el.endDate?? null;
            }
        }

        public PricingProfileEditor()
        {
            InitializeComponent();
            newProfile = true;
            profile = new PricingProfile();
            profile.ProfileID = Communication.RetrieveNextPricingProfileID();

            textBox2.Text = profile.ProfileID.ToString();
            textBox1.Text = profile.ProfileName;
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            profileEdited = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int row = dataGridView1.Rows.Add();
            dataGridView1.Rows[row].Tag = new PricingProfileElement();
            (dataGridView1.Rows[row].Tag as PricingProfileElement).priceSheet = PricingProfileElement.PriceSheets.Green;
            profile.Elements.Add(dataGridView1.Rows[row].Tag as PricingProfileElement);
            (dataGridView1.Rows[row].Cells[5] as DataGridViewComboBoxCell).Items.AddRange("Cost", "Red", "Yellow", "Green", "Pink", "Blue");
            dataGridView1.Rows[row].Cells[5].Value = Enum.GetName(typeof(PricingProfileElement.PriceSheets), (dataGridView1.Rows[row].Tag as PricingProfileElement).priceSheet);
            profileEdited = true;
        }

        private void saveProfileButton_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty)
            {
                MessageBox.Show("Please enter a valid alphanumeric name for this pricing profile.");
                return;
            }
            profile.ProfileName = textBox1.Text;
            profile.ProfileID = int.Parse(textBox2.Text);
            int i = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                (row.Tag as PricingProfileElement).profileID = profile.ProfileID;

                #region Element error checking
                if (row.Cells[0].Value == null && row.Cells[1].Value == null && row.Cells[2].Value == null &&
                    row.Cells[3].Value == null && row.Cells[4].Value == null)
                {
                    MessageBox.Show("Row " + row.Index + " is invalid.\nElements must have at least one identifier type!");
                    return;
                }
                if (row.Cells[6].Value == null || !decimal.TryParse(row.Cells[6].Value?.ToString(), out decimal _))
                {
                    MessageBox.Show("Row " + row.Index + "is invalid.\nElements must have a specified margin!");
                    return;
                }
                if (row.Cells[7].Value != null)
                {
                    if (!DateTime.TryParse(row.Cells[7].Value.ToString(), out DateTime _))
                    {
                        MessageBox.Show("Invalid begin date on row " + row.Index + "!\nPlease enter a valid date for the begin date field.");
                        return;
                    }
                }
                if (row.Cells[8].Value != null)
                {
                    if (!DateTime.TryParse(row.Cells[8].Value.ToString(), out DateTime _))
                    {
                        MessageBox.Show("Invalid end date on row " + row.Index + "!\nPlease enter a valid date for the end date field.");
                        return;
                    }
                }
                #endregion

                if (row.Cells[0].Value != null)
                {
                    if (!Program.IsStringNumeric(row.Cells[0].Value.ToString()))
                    {
                        MessageBox.Show("Please enter a valid numeric group code on row " + row.Index + ".");
                        return;
                    }
                    else
                    {
                        (row.Tag as PricingProfileElement).groupCode = row.Cells[0].Value.ToString();
                    }
                }

                if (row.Cells[1].Value != null)
                {
                    (row.Tag as PricingProfileElement).department = row.Cells[1].Value.ToString();
                }

                if (row.Cells[2].Value != null)
                {
                    (row.Tag as PricingProfileElement).subDepartment = row.Cells[2].Value.ToString();
                }

                if (row.Cells[3].Value != null)
                {
                    if (row.Cells[3].Value.ToString().All(Char.IsLetter))
                    {
                        if (Communication.CheckProductLine(row.Cells[3].Value.ToString()))
                        {
                            (row.Tag as PricingProfileElement).productLine = row.Cells[3].Value.ToString().ToUpper();
                        }
                        else
                        {
                            MessageBox.Show("Product line \"" + row.Cells[3].Value.ToString().ToUpper() + "\" on row " + row.Index + " does not exist!");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Product line \"" + row.Cells[3].Value.ToString().ToUpper() + "\" on row " + row.Index + " is not valid!");
                        return;
                    }
                }

                if (row.Cells[4].Value != null)
                {
                    if (!Program.IsStringAlphaNumeric(row.Cells[4].Value.ToString()))
                    {
                        MessageBox.Show("Please enter a valid item number or item prefix on row " + row.Index + ".");
                        return;
                    }
                    else
                    {
                        (row.Tag as PricingProfileElement).itemNumber = row.Cells[4].Value.ToString();
                    }
                }

                (row.Tag as PricingProfileElement).priceSheet = (PricingProfileElement.PriceSheets)Enum.Parse(typeof(PricingProfileElement.PriceSheets), row.Cells[5].Value.ToString());

                (row.Tag as PricingProfileElement).margin = decimal.Parse(row.Cells[6].Value.ToString());

                if (row.Cells[7].Value != null)
                {
                    if (DateTime.TryParse(row.Cells[7].Value.ToString(), out DateTime beginDate))
                    {
                        (row.Tag as PricingProfileElement).beginDate = beginDate;
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid begin date on row " + row.Index + ".");
                        return;
                    }
                }

                if (row.Cells[8].Value != null)
                {
                    if (DateTime.TryParse(row.Cells[8].Value.ToString(), out DateTime endDate))
                    {
                        (row.Tag as PricingProfileElement).endDate = endDate;
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid end date on row " + row.Index + ".");
                        return;
                    }
                }

                (row.Tag as PricingProfileElement).priority = i;
                i++;

                profile.Elements.Add((row.Tag as PricingProfileElement));
            }

            if (newProfile)
            {
                Communication.AddPricingProfile(profile);
                newProfile = false;
            }
            else
                Communication.UpdatePricingProfile(profile);

            MessageBox.Show("Profile successfully saved!");

            profileEdited = false;
        }

        private void deleteElementButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1)
                return;

            dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
            profileEdited = true;
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count < 1)
                return;

            if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageBox.Show("How did you select more than one row?\nThis is an error that needs to be looked into.\nPlease contact the developer.");
                return;
            }

            deleteElementButton.Enabled = true;

            if (dataGridView1.Rows.Count > 1)
            {
                priorityUpButton.Enabled = true;
                priorityDownButton.Enabled = true;
            }

            if (dataGridView1.SelectedRows[0].Index == 0)
            {
                priorityUpButton.Enabled = false;
            }

            if (dataGridView1.SelectedRows[0].Index == dataGridView1.Rows.Count - 1)
            {
                priorityDownButton.Enabled = false;
            }
        }

        private void priorityUpButton_Click(object sender, EventArgs e)
        {

        }

        private void PricingProfileEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (profileEdited)
            {
                if (MessageBox.Show("There are changes pending saving for this profile.\nAre you sure you want to exit without saving?", "Warning!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    return;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
