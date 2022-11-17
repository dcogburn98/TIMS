using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TIMSServerModel;
using TIMS.Server;

namespace TIMS.Forms.ServerRelations
{
    public partial class ServerConnectionSettings : Form
    {
        public ServerConnectionSettings()
        {
            InitializeComponent();

            textBox1.Text = Communication.RetrieveProperty("Server Relationship Key");
        }
    }
}
