using System;
using System.Collections.Generic;
using System.Windows.Forms;

using TIMS.Forms;
using TIMS.Forms.POS;
using TIMS.Forms.Reporting;

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

            #region Invoice Viewer Test Code
            //SHA256 encrypt = SHA256.Create();
            //byte[] hash = encrypt.ComputeHash(Encoding.UTF8.GetBytes("0"));
            //DatabaseHandler.InitializeDatabases();
            //Invoice inv = new Invoice();
            //inv.customer = DatabaseHandler.SqlCheckCustomerNumber("0");
            //inv.employee = DatabaseHandler.SqlLogin("0", hash);
            //inv.taxRate = 0.1025f;
            //for (int i = 0; i != 10; i++)
            //{
            //    inv.items.Add(new InvoiceItem(DatabaseHandler.SqlCheckItemNumber("75130", false).ToArray()[1])
            //    { quantity = 3, total = 10.50f });
            //}
            //for (int i = 0; i != 10; i++)
            //{
            //    inv.items.Add(new InvoiceItem(DatabaseHandler.SqlCheckItemNumber("75130", false).ToArray()[0])
            //    { quantity = 3, total = 10.50f });
            //}
            //for (int i = 0; i != 10; i++)
            //{
            //    inv.items.Add(new InvoiceItem(DatabaseHandler.SqlCheckItemNumber("MBR100", false).ToArray()[0])
            //    { quantity = 3, total = 10.50f });
            //    inv.containsAgeRestrictedItem = true;
            //}
            //for (int i = 0; i != 10; i++)
            //{
            //    inv.items.Add(new InvoiceItem(DatabaseHandler.SqlCheckItemNumber("75130", false).ToArray()[0])
            //    { quantity = 3, total = 10.50f });
            //}
            //foreach (InvoiceItem item in inv.items)
            //    inv.subtotal += item.total;
            //inv.taxAmount = inv.subtotal * inv.taxRate;
            //inv.total = inv.subtotal + inv.taxAmount;
            //inv.invoiceNumber = 2113095551;
            //inv.attentionLine = "Jason Ellis";
            //inv.PONumber = "Truck 451";
            //inv.invoiceMessage = "MUST SIGN INVOICE!!!!!!";
            //inv.invoiceFinalizedTime = DateTime.Now;
            //inv.payments.Add(new Payment() { ID = Guid.NewGuid(), paymentAmount = inv.total, paymentType = Payment.PaymentTypes.Cash });
            //InvoiceViewer v = new InvoiceViewer(inv);
            //Application.Run(v);
            #endregion
            #region Invoice Review Window Test Code
            //DatabaseHandler.InitializeDatabases();
            //ReviewInvoices rev = new ReviewInvoices();
            //Application.RegisterMessageLoop(new Application.MessageLoopCallback(CheckOpenForms));
            //Application.Run(rev);
            #endregion
            #region Report Manager Window Test Code
            //DatabaseHandler.InitializeDatabases();
            //ReportManager manager = new ReportManager();
            //Application.RegisterMessageLoop(new Application.MessageLoopCallback(CheckOpenForms));
            //Application.Run(manager);
            #endregion
            #region Bin Label Printer Test Code
            DatabaseHandler.InitializeDatabases();
            BinLabelPrinting printer = new BinLabelPrinting();
            Application.Run(printer);
            #endregion
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
