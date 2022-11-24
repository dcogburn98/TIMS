using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

using TIMSServerModel;
using TIMS.Server;

namespace TIMS.Forms.DeveloperTools
{
    public partial class IngenicoTest : Form
    {
        public IngenicoTest()
        {
            InitializeComponent();
            cardTypeBox.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cardTypeBox.SelectedIndex < 0)
                return;
            if (!Program.IsStringNumeric(cardNumberBox.Text) || cardNumberBox.Text == "")
                return;
            if (!Program.IsStringNumeric(cvvBox.Text) || cvvBox.Text == "")
                return;
            if (!Program.IsStringNumeric(expDateBox.Text) || expDateBox.Text == "")
                return;
            if (addressBox.Text == "")
                return;
            if (!Program.IsStringNumeric(zipBox.Text) || zipBox.Text == "")
                return;
            if (!decimal.TryParse(amountBox.Text, out decimal d) || amountBox.Text == "")
                return;

            XDocument xml = new XDocument(
                new XElement("DETAIL",
                    new XElement("TRAN_TYPE", "CCR1"),
                    new XElement("AMOUNT", Math.Round(d, 2).ToString("C")),
                    new XElement("ACCOUNT_NBR", cardNumberBox.Text),
                    new XElement("FIELD", new XAttribute("KEY", "AUTH_CARD_TYPE"), "V"),
                    new XElement("CVV2", cvvBox.Text),
                    new XElement("EXP_DATE", expDateBox.Text),
                    new XElement("ADDRESS", addressBox.Text),
                    new XElement("ZIP_CODE", zipBox.Text)));

            textBox6.Text = xml.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox8.Text = Communication.TestIngenicoRequest(new IngenicoRequest(textBox6.Text));
        }
    }
}
