using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TIMS.Forms.Maintenance
{
    public partial class HTMLEditor : Form
    {
        public HTMLEditor()
        {
            InitializeComponent();
            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot");
            textBox1.Text = File.ReadAllText(Path.Combine(basePath, "index.html"));
            webBrowser1.DocumentText = ParseHTMLVariables(textBox1.Text, '#');
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            webBrowser1.DocumentText = ParseHTMLVariables(textBox1.Text, '#');
        }

        private string ParseHTMLVariables(string html, char delimiter)
        {
            string newHTML = "";
            string[] split = html.Split(delimiter);
            for (int i = 0; i != split.Length; i++)
            {
                if (split[i] != "")
                {
                    if (
                        (i > 0 && split[i - 1] == "") &&
                        (i + 1 != split.Length && split[i + 1] == ""))
                    {
                        newHTML += GetVariableValue(split[i]);
                        continue;
                    }

                    newHTML += split[i];
                    continue;
                }

                
            }

            return newHTML;
        }

        private string GetVariableValue(string var)
        {
            if (var == "ItemNumber")
                return var;
            else
                return "NULL";
        }
    }
}
