using System;
using System.Windows.Forms;

namespace TIMS.Forms
{
    public partial class GeneralJournal : Form
    {
        public GeneralJournal()
        {
            InitializeComponent();
        }

        private void GeneralJournal_ResizeEnd(object sender, EventArgs e)
        {
            dataGridView1.Width = Width - 40;
            dataGridView1.Height = Height - 120;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
