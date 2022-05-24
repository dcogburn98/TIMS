using System;
using System.Windows.Forms;

namespace PermissionsTest
{
    public partial class Form1 : Form
    {
        public string username;
        public Form1()
        {
            InitializeComponent();
            username = string.Empty;
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (usernameBox.Text == string.Empty || passwordBox.Text == string.Empty)
                return;

            foreach (User u in User.Users)
            {
                if (u.username == usernameBox.Text)
                    username = u.username;
            }
            if (username == string.Empty)
                return;

            permissionsBox.Items.Clear();
            ulong[] permissions = Permission.ParsePermissions(User.Users.Find(el => el.username == username).permissions);
            foreach (ulong p in permissions)
            {
                permissionsBox.Items.Add(p);
            }
        }

        private void testButton1_Click(object sender, EventArgs e)
        {
            if (username == string.Empty)
                return;

            if (Permission.hasPermission(username, 0x01))
                MessageBox.Show("Success!");
            else
                MessageBox.Show("Access Denied.");
        }
    }
}
