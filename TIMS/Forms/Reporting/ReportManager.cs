using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TIMS.Server;
using TIMSServerModel;

namespace TIMS.Forms.Reporting
{
    public partial class ReportManager : Form
    {
        Report currentReport;
        public ReportManager()
        {
            InitializeComponent();

            foreach (string report in Communication.RetrieveAvailableReports())
                reportPickerCB.Items.Add(report);
        }

        private void reportPickerCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (reportPickerCB.SelectedIndex == -1)
                return;

            currentReport = Communication.RetrieveReport(reportPickerCB.Text.Split(' ')[0]);
            conditionsLB.Items.Clear();
            foreach (string condition in currentReport.Conditions)
                conditionsLB.Items.Add(condition);
            printButton.Enabled = true;
            resetButton.Enabled = true;
        }

        private void conditionsLB_DoubleClick(object sender, EventArgs e)
        {
            if (conditionsLB.SelectedIndex == -1)
                return;

            ReportConditionEditor editor = new ReportConditionEditor(conditionsLB.Items[conditionsLB.SelectedIndex].ToString());
            if (editor.ShowDialog() == DialogResult.OK)
            {
                currentReport.Conditions[currentReport.Conditions.IndexOf(conditionsLB.Items[conditionsLB.SelectedIndex].ToString())] = editor.condition;
                conditionsLB.Items.Clear();
                foreach (string condition in currentReport.Conditions)
                    conditionsLB.Items.Add(condition);
            }
        }

        private void printButton_Click(object sender, EventArgs e)
        {
            ReportViewer viewer = new ReportViewer(currentReport);
            viewer.ShowDialog();
        }
    }
}
