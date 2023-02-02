using System.Xml.Linq;
using System.Windows.Forms;

using TIMS.Server;
using TIMSServerModel;

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
            }
            dataGridView1.Sort(dataGridView1.Columns[3], System.ComponentModel.ListSortDirection.Descending);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            MailMessage msg = (dataGridView1.Rows[e.RowIndex].Tag as MailMessage);
            XDocument html = new XDocument(
                new XElement("body",
                    new XElement("h1", msg.Subject),
                    new XElement("p", "Sender: " + msg.Sender.fullName),
                    new XElement("p", ""),
                    new XElement("p", "Message Body"),
                    new XElement("p", "--------------------------------------------")));
            
            tabControl1.SelectedTab = tabPage2;
            webBrowser1.DocumentText = html.ToString() + msg.MessageBody;
        }
    }
}
