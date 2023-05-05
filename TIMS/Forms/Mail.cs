using System;
using System.Drawing;
using System.Xml.Linq;
using System.Windows.Forms;

using TIMS.Server;
using TIMSServerModel;
using System.Collections;

namespace TIMS
{
    public partial class Mail : Form
    {
        public Mail()
        {
            InitializeComponent();

            foreach (MailMessage msg in Communication.GetEmployeeMessages(Program.currentEmployee.employeeNumber.ToString()))
            {
                int row = dataGridView1.Rows.Add();
                dataGridView1.Rows[row].Tag = msg;
                dataGridView1.Rows[row].Cells[0].Value = msg.Read ? "Read" : "Unread";
                dataGridView1.Rows[row].Cells[1].Value = msg.Subject;
                dataGridView1.Rows[row].Cells[2].Value = msg.Sender.fullName;
                dataGridView1.Rows[row].Cells[3].Value = msg.SendDate.ToString("MM/dd/yyyy hh:mm tt");
                if (msg.Read) dataGridView1.Rows[row].DefaultCellStyle.BackColor = Color.LightGray;
            }
            dataGridView1.Sort(new RowComparer());
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
            dataGridView1.Rows[e.RowIndex].Cells[0].Value = "Read";
            MailMessage msg = (dataGridView1.Rows[e.RowIndex].Tag as MailMessage);
            Communication.ReadMessage(msg);
            XDocument html = new XDocument(
                new XElement("body",
                    new XElement("h1", msg.Subject),
                    new XElement("p", "Sender: " + msg.Sender.fullName),
                    new XElement("p", ""),
                    new XElement("h3", "Message Body"),
                    new XElement("p", "-------------------------------------------------------------")));
            
            tabControl1.SelectedTab = tabPage2;
            webBrowser1.DocumentText = html.ToString() + msg.MessageBody;
        }
    }

    class RowComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            DataGridViewRow row1 = (DataGridViewRow)x;
            DataGridViewRow row2 = (DataGridViewRow)y;
            return DateTime.Compare(DateTime.Parse(row2.Cells[3].Value.ToString()), DateTime.Parse(row1.Cells[3].Value.ToString()));
        }
    }
}
