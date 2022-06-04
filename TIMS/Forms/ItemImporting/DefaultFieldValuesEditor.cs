using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TIMS.Forms.ItemImporting
{
    public partial class DefaultFieldValuesEditor : Form
    {
        List<string> values;
        public DefaultFieldValuesEditor(List<string> values)
        {
            InitializeComponent();
            for (int i = 0; i != values.Count / 2; i++)
            {
                listBox1.Items.Add(values[i * 2] + " = " + values[(i * 2) + 1]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
