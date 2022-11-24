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
    public partial class ItemImportFieldEditor : Form
    {
        public string selectedHeader = string.Empty;

        public ItemImportFieldEditor(string CSVColumn, List<string> availableHeaders)
        {
            InitializeComponent();
            csvHeaderTB.Text = CSVColumn;
            foreach (string header in availableHeaders)
                itemHeaderCB.Items.Add(header);
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            if (itemHeaderCB.SelectedIndex == -1)
                return;

            selectedHeader = itemHeaderCB.Text;
            Close();
        }

        private void skipButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
