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
        Report report;
        string selectedDatasource;
        public ReportCreator()
        {
            InitializeComponent();
            foreach (string table in DatabaseHandler.SqlRetrieveTableNames())
            {
                dataSourceCB.Items.Add(table);
            }
        }

        private void dataSourceCB_Leave(object sender, EventArgs e)
        {
            if (selectedDatasource == dataSourceCB.Text)
                return;

            selectedDatasource = dataSourceCB.Text;
            conditionsLB.Items.Clear();

            conditionLeftComparatorCB.Items.Clear();
            conditionRightComparatorCB.Items.Clear();
            conditionLeftComparatorCB.Text = string.Empty;
            conditionRightComparatorCB.Text = string.Empty;

            foreach (string item in DatabaseHandler.SqlRetrieveTableHeaders(dataSourceCB.Text))
            {
                conditionLeftComparatorCB.Items.Add(item);
                conditionRightComparatorCB.Items.Add(item);
                fieldsCB.Items.Add(item);
                totalCB.Items.Add(item);
            }
        }
        
        private void addConditionButton_Click(object sender, EventArgs e)
        {
            if (conditionRightComparatorCB.Text == string.Empty || conditionOperatorCB.Text == string.Empty || conditionLeftComparatorCB.Text == string.Empty)
                return;

            string right = conditionRightComparatorCB.Text;
            string left = conditionLeftComparatorCB.Text;
            string op = conditionOperatorCB.Text;

            foreach (char c in right)
                if (!char.IsLetterOrDigit(c))
                    return;

            conditionsLB.Items.Add($"{left} {op} {right}");
            conditionLeftComparatorCB.SelectedIndex = -1;
            conditionRightComparatorCB.SelectedIndex = -1;
            conditionRightComparatorCB.Text = String.Empty;
            conditionOperatorCB.SelectedIndex = -1;
            if (fieldsLB.Items.Count > 0)
                viewQueryButton.Enabled = true;
        }

        private void addFieldButton_Click(object sender, EventArgs e)
        {
            if (fieldsCB.SelectedIndex < 0)
                return;

            fieldsLB.Items.Add(fieldsCB.SelectedItem.ToString());
            if (conditionsLB.Items.Count > 0)
                viewQueryButton.Enabled = true;
        }

        private void addTotalButton_Click(object sender, EventArgs e)
        {
            if (totalCB.SelectedIndex < 0)
                return;

            totalLB.Items.Add(totalCB.SelectedItem.ToString());
        }

        private void previewReportButton_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            List<string> fields = new List<string>();
            List<string> conditions = new List<string>();
            List<string> totals = new List<string>();
            foreach (string field in fieldsLB.Items)
                fields.Add(field);
            foreach (string condition in conditionsLB.Items)
                conditions.Add(condition);
            foreach (string total in totalLB.Items)
                totals.Add(total);

            report = new Report(fields, dataSourceCB.Text, conditions, totals);
            report.ExecuteReport();

            foreach (string field in fieldsLB.Items)
            {
                dataGridView1.Columns.Add(field, field);
            }
            foreach (object o in report.Results)
            {
                int row = dataGridView1.Rows.Add();
                for (int i = 0; i != dataGridView1.Columns.Count; i++)
                {
                    dataGridView1.Rows[row].Cells[i].Value = o;
                }
            }
        }

        private void conditionsLB_DoubleClick(object sender, EventArgs e)
        {
            if (conditionsLB.SelectedIndex == -1)
                return;

            conditionsLB.Items.RemoveAt(conditionsLB.SelectedIndex);
            if (conditionsLB.Items.Count == 0)
                viewQueryButton.Enabled = false;
        }

        private void viewQueryButton_Click(object sender, EventArgs e)
        {
            List<string> fields = new List<string>();
            List<string> conditions = new List<string>();
            List<string> totals = new List<string>();
            foreach (string field in fieldsLB.Items)
                fields.Add(field);
            foreach (string condition in conditionsLB.Items)
                conditions.Add(condition);
            foreach (string total in totalLB.Items)
                totals.Add(total);
                
            report = new Report(fields, dataSourceCB.Text, conditions, totals);
            MessageBox.Show(report.Query);
        }

        private void fieldsLB_DoubleClick(object sender, EventArgs e)
        {
            if (fieldsLB.SelectedIndex == -1)
                return;

            fieldsLB.Items.RemoveAt(fieldsLB.SelectedIndex);
            if (fieldsLB.Items.Count == 0)
                viewQueryButton.Enabled = false;
        }
    }
}
