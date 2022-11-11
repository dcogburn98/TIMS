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
    public partial class PricingProfiles : Form
    {
        public PricingProfiles()
        {
            InitializeComponent();

            foreach (PricingProfile profile in Communication.RetrievePricingProfiles())
            {
                int row = dataGridView1.Rows.Add();
                dataGridView1.Rows[row].Cells[0].Value = profile.ProfileID;
                dataGridView1.Rows[row].Cells[1].Value = profile.ProfileName;
                dataGridView1.Rows[row].Tag = profile;
            }

            CancelButton = button3;
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1)
                return;

            PricingProfileEditor editor = new PricingProfileEditor((dataGridView1.SelectedRows[0].Tag as PricingProfile));
            editor.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1)
                return;

            PricingProfileEditor editor = new PricingProfileEditor((dataGridView1.SelectedRows[0].Tag as PricingProfile));
            editor.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
