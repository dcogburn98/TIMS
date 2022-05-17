using System;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Text;

namespace TIMS
{
    public partial class Login : Form
    {
        public string launch;
        public string useroremployeeno;
        public Login()
        {
            DatabaseHandler.InitializeDatabases();
            InitializeComponent();
            badLoginLabel.Visible = false;
            exceptionBox.Visible = false;
            badPasswordLabel.Visible = false;
            passwordBox.UseSystemPasswordChar = true;
            CancelButton = exitButton;
            loginButton.Enabled = false;
            passwordBox.Enabled = false;
            tempKeyBox.Enabled = false;
        }

        public void TryLogin()
        {
            SHA256 encrypt = SHA256.Create();
            encrypt.Initialize();
            byte[] hash = encrypt.ComputeHash(Encoding.UTF8.GetBytes(passwordBox.Text));
            Employee e = DatabaseHandler.SqlLogin(useroremployeeno, hash);
            if (e != null)
            {
                Program.currentEmployee = e;
                if (e.startupScreen == Employee.StartupScreens.Invoicing)
                    Program.LaunchInvoicing();
                else if (e.startupScreen == Employee.StartupScreens.EmployeeManagement)
                    Program.LaunchEmployee();
                else if (e.startupScreen == Employee.StartupScreens.Dashboard)
                    Program.LaunchAlternateFunctions();
            }
            else
            {
                badPasswordLabel.Show();
                passwordBox.Focus();
                passwordBox.SelectAll();
            }
            return;
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.OpenForms.Remove(this);
            Program.CheckOpenForms();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            TryLogin();
        }

        private void passwordBox_TextChanged(object sender, EventArgs e)
        {
            if (passwordBox.Text.Length > 0)
                loginButton.Enabled = true;
            else
                loginButton.Enabled = false;
        }

        private void passwordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && passwordBox.Text.Length > 0)
                TryLogin();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void usernameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                //Send a message to the server and wait for a response containing name of the employee
                //and the default startup screen for the employee. Server will return 0 if employee number does not exist
                if (!Program.IsStringAlphaNumeric(usernameBox.Text))
                {
                    badLoginLabel.Text = "Invalid username/employee number!";
                    badLoginLabel.Visible = true;
                    usernameBox.SelectAll();
                    return;
                }
                else
                {
                    string response = DatabaseHandler.SqlCheckEmployee(usernameBox.Text);
                    if (response == null)
                    {
                        badLoginLabel.Text = "Invalid username/employee number!";
                        badLoginLabel.Visible = true;
                        usernameBox.SelectAll();
                        return;
                    }
                    else
                    {
                        useroremployeeno = usernameBox.Text;
                        badLoginLabel.Text = response;
                        badLoginLabel.Visible = true;
                        passwordBox.Enabled = true;
                        tempKeyBox.Enabled = true;
                        passwordBox.Focus();
                    }
                }
            }
        }

        private void tempKeyBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && passwordBox.Text.Length > 0)
                TryLogin();
        }
    }
}
