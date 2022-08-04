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

namespace TIMS.Forms
{
    public partial class BinLabelPrinting : Form
    {
        BarcodeSheet sheet;
        public BinLabelPrinting()
        {
            InitializeComponent();
            CancelButton = button1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<Item> LabelItems = DatabaseHandler.SqlRetrieveLabelOutOfDateItems();
            sheet = new BarcodeSheet(LabelItems);
            ReportViewer viewer = new ReportViewer(sheet);
            viewer.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
                return;


        }
    }
}
