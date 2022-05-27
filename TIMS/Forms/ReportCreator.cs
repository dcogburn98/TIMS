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
    }
}
