using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using TIMS.Forms;

namespace TIMS
{
    static class Program
    {
        public static Guid guid = Guid.NewGuid();
        public static List<Form> OpenForms = new List<Form>();
        public static Employee currentEmployee;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Login login = new Login();
            //OpenForms.Add(login);
            //login.Show();
            //Application.RegisterMessageLoop(new Application.MessageLoopCallback(CheckOpenForms));
            //Application.Run();
            DatabaseHandler.InitializeDatabases();
            Invoice inv = new Invoice();
            inv.customer = DatabaseHandler.CheckCustomerNumber("0");
            inv.employee = DatabaseHandler.Login("0", "0");
            inv.subtotal = 3.95f;
            inv.taxRate = 0.1025f;
            inv.total = 4.49f;
            inv.items.Add(new InvoiceItem(DatabaseHandler.CheckItemNumber("75130").ToArray()[0]));
            inv.items.Add(new InvoiceItem(DatabaseHandler.CheckItemNumber("75130").ToArray()[1]));
            inv.items.Add(new InvoiceItem(DatabaseHandler.CheckItemNumber("75130").ToArray()[1]));
            inv.invoiceNumber = 75130;
            ReportViewer v = new ReportViewer(inv);
            Application.Run(v);
        }

        public static void LaunchInvoicing()
        {
            Invoicing inv = new Invoicing();
            OpenForms.Add(inv);
            inv.Show();

            Form login = OpenForms.Find(el => el is Login);
            if (login == null)
                return;
            OpenForms.Remove(login);
            login.Close();
        }

        public static void LaunchAlternateFunctions()
        {
            OtherFunctions of = new OtherFunctions();
            OpenForms.Add(of);
            of.Show();

            Form login = OpenForms.Find(el => el is Login);
            if (login == null)
                return;
            OpenForms.Remove(login);
            login.Close();
        }

        public static void LaunchEmployee()
        {
            //Launch into employee management window
        }

        public static void OpenForm(Form form)
        {
            OpenForms.Add(form);
            form.Show();
        }

        public static void CloseForm(Form form)
        {
            form.Close();
            OpenForms.Remove(form);
            CheckOpenForms();
        }

        public static bool CheckOpenForms()
        {
            if (OpenForms.Count == 0)
            {
                Environment.Exit(0);
            }
            return false;
        }
    
        public static bool IsStringNumeric(string input)
        {
            char[] a = input.ToCharArray();
            foreach (char b in a)
            {
                if (char.IsDigit(b))
                {
                    continue;
                }
                return false;
            }
            return true;
        }

        public static bool IsStringAlphaNumeric(string input)
        {
            char[] a = input.ToCharArray();
            foreach (char b in a)
            {
                if (char.IsLetterOrDigit(b))
                {
                    continue;
                }
                return false;
            }
            return true;
        }
    
        public static string FormatCurrency(float amount)
        {
            return amount.ToString("C");
        }
    }
}
