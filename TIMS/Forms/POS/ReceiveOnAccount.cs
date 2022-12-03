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

namespace TIMS.Forms.POS
{
    public partial class ReceiveOnAccount : Form
    {
        Customer currentCustomer;

        public ReceiveOnAccount()
        {
            InitializeComponent();
            paymentMethodCB.Items.AddRange(Enum.GetNames(typeof(Payment.PaymentTypes)));

            accountNumberTB.Focus();
        }

        private void accountNumberTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!Program.IsStringNumeric(accountNumberTB.Text))
                {
                    MessageBox.Show("Invalid customer number!");
                    return;
                }
                currentCustomer = Communication.CheckCustomerNumber(accountNumberTB.Text);
                if (currentCustomer == null)
                {
                    MessageBox.Show("Customer does not exist!");
                    return;
                }
                else
                {
                    accountNameTB.Text = currentCustomer.customerName;
                    PopulateFields();
                }
            }
        }

        private void PopulateFields()
        {
            nameLabel.Text = currentCustomer.customerName;
            address1Label.Text = currentCustomer.billingAddress.Split(',')[0].Trim();
            address2Label.Text = currentCustomer.billingAddress.Split(',')[1].Trim() + ", " + currentCustomer.billingAddress.Split(',')[2].Trim() + currentCustomer.billingAddress.Split(',')[3].Trim();
            phoneLabel.Text = currentCustomer.phoneNumber;
            creditLimit.Text = currentCustomer.creditLimit.ToString("C").Trim('$');
            creditAvailable.Text = (currentCustomer.creditLimit - currentCustomer.accountBalance).ToString("C").Trim('$');
            primaryTaxStatus.Text = currentCustomer.primaryTaxStatus;
            secondaryTaxStatus.Text = currentCustomer.secondaryTaxStatus;
            billingType.Text = currentCustomer.billingType;
            terms.Text = "TODO";
            lastStatementAmount.Text = currentCustomer.lastStatementAmount.ToString("C").Trim('$');
            totalDue.Text = currentCustomer.totalDue.ToString("C").Trim('$');
            due30.Text = currentCustomer.due30.ToString("C").Trim('$');
            due60.Text = currentCustomer.due60.ToString("C").Trim('$');
            due90.Text = currentCustomer.due90.ToString("C").Trim('$');
            dueFurther.Text = currentCustomer.furtherDue.ToString("C").Trim('$');
            serviceCharge.Text = currentCustomer.serviceCharge.ToString("C").Trim('$');
            lastPaymentAmount.Text = currentCustomer.lastPaymentAmount.ToString("C").Trim('$');
            lastPaymentDate.Text = currentCustomer.lastPaymentDate.ToString("MM/dd/yyyy");
            lateFee.Text = "TODO";
            currentMonthPayments.Text = "TODO";
            totalAccountBalance.Text = currentCustomer.accountBalance.ToString("C").Trim('$');
            maxDiscount.Text = "TODO";

            if (currentCustomer.statementType.ToUpper() == "OPEN ITEM")
            {
                invoicesLB.Enabled = true;
                invoiceNumber.Enabled = true;
                outstandingInvoicesButton.Enabled = true;
                addInvoiceButton.Enabled = true;
                removeInvoiceButton.Enabled = true;
            }
            else
            {
                invoicesLB.Enabled = false;
                invoiceNumber.Enabled = false;
                outstandingInvoicesButton.Enabled = false;
                addInvoiceButton.Enabled = false;
                removeInvoiceButton.Enabled = false;
            }

            
        }
    }
}
