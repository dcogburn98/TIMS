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
    public partial class ReportCreator : Form
    {
        public ReportCreator()
        {
            InitializeComponent();
            foreach (string table in DatabaseHandler.SqlRetrieveTableNames())
            {
                comboBox1.Items.Add(table);
            }
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            foreach (string item in DatabaseHandler.SqlRetrieveTableHeaders(comboBox1.Text))
            {
                comboBox2.Items.Add(item);
            }
        }
    }
}
