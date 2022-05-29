using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TIMS.Forms.Reporting
{
    public partial class ReportConditionEditor : Form
    {
        public string condition = string.Empty;
        public ReportConditionEditor(string condition)
        {
            InitializeComponent();
            this.condition = condition;
            CancelButton = button1;
            textBox1.Text = condition.Split(' ')[0];
            textBox2.Text = condition.Split(' ')[1];
            textBox3.Text = condition.Split(' ')[2];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            condition = textBox1.Text + " " + textBox2.Text + " " + textBox3.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
