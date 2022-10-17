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

        public PricingProfileEditor(PricingProfile profile)
        {
            InitializeComponent();
            this.profile = profile;

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

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
