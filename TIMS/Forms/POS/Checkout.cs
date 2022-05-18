using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TIMS.Forms
{
    public partial class Checkout : Form
    {
        Invoice invoice;
        bool containsAgeRestrictedItems;
        public bool finalized = false;
        public bool printed = false;

        public Checkout(Invoice invoice)
        {
            InitializeComponent();
            this.invoice = invoice;
            PopulateInvoiceInfo();
            invoice.payments = new List<Payment>();

            label12.Visible = false;
            label13.Visible = false;
            label14.Visible = false;
            paymentAmountTB.Visible = false;
            checkNumberTB.Visible = false;
            checkNameTB.Visible = false;
            acceptPaymentBtn.Visible = false;
            cancelPaymentBtn.Visible = false;

            paymentTypeLB.Items.Clear();
            paymentsLB.Items.Clear();

            remainingBalanceTB.Text = invoice.total.ToString("C");

            #region Add Payment Types to list box
            if (invoice.customer.availablePaymentTypes.Contains(Payment.PaymentTypes.Charge))
                paymentTypeLB.Items.Add("Charge");
            if (invoice.customer.availablePaymentTypes.Contains(Payment.PaymentTypes.Cash))
                paymentTypeLB.Items.Add("Cash");
            if (invoice.customer.availablePaymentTypes.Contains(Payment.PaymentTypes.Check))
                paymentTypeLB.Items.Add("Check");
            if (invoice.customer.availablePaymentTypes.Contains(Payment.PaymentTypes.PaymentCard))
                paymentTypeLB.Items.Add("Payment Card");
            if (invoice.customer.availablePaymentTypes.Contains(Payment.PaymentTypes.Paypal))
                paymentTypeLB.Items.Add("Paypal");
            if (invoice.customer.availablePaymentTypes.Contains(Payment.PaymentTypes.CashApp))
                paymentTypeLB.Items.Add("CashApp");
            if (invoice.customer.availablePaymentTypes.Contains(Payment.PaymentTypes.Venmo))
                paymentTypeLB.Items.Add("Venmo");
            #endregion
        }

        private void PopulateInvoiceInfo()
        {
            int itemCount = 0;
            containsAgeRestrictedItems = false;
            int minimumAge = 0;
            foreach (InvoiceItem item in invoice.items)
            {
                if (item.taxed)
                    invoice.taxableTotal += item.total;
                if (item.ageRestricted)
                {
                    containsAgeRestrictedItems = true;
                    if (minimumAge <= item.minimumAge)
                        minimumAge = item.minimumAge;
                }
                itemCount++;
            }

            invoice.taxRate = 0.1025f;
            invoice.taxAmount = invoice.taxableTotal * invoice.taxRate;
            invoice.total = (float)Math.Round(invoice.subtotal + invoice.taxAmount, 2);
            
            subtotalTB.Text = invoice.subtotal.ToString("C");
            itemCountTB.Text = itemCount.ToString();
            taxableTB.Text = invoice.taxableTotal.ToString("C");
            taxRateTB.Text = invoice.taxRate.ToString("P");
            taxAmtTB.Text = invoice.taxAmount.ToString("C");
            totalTB.Text = invoice.total.ToString("C");
            if (containsAgeRestrictedItems)
            {
                ageRestrictedTB.Text = "YES";
                customerBirthdaySelector.Enabled = true;
            }
            else
                ageRestrictedTB.Text = "NO";
            acceptBtn.Focus();
        }

        private void SelectPaymentType()
        {
            paymentTypeLB.Enabled = false;
            if ((string)paymentTypeLB.SelectedItem == "Cash" ||
                (string)paymentTypeLB.SelectedItem == "Payment Card" ||
                (string)paymentTypeLB.SelectedItem == "Paypal" ||
                (string)paymentTypeLB.SelectedItem == "CashApp" ||
                (string)paymentTypeLB.SelectedItem == "Venmo")
            {
                label12.Visible = true;
                paymentAmountTB.Visible = true;
                acceptPaymentBtn.Enabled = true;
                acceptPaymentBtn.Visible = true;
                cancelPaymentBtn.Visible = true;
                paymentAmountTB.Focus();
                paymentAmountTB.Text = (invoice.total - invoice.totalPayments).ToString();
                paymentAmountTB.SelectAll();
            }
            else if ((string)paymentTypeLB.SelectedItem == "Check")
            {
                label12.Visible = true;
                label13.Visible = true;
                label14.Visible = true;
                paymentAmountTB.Visible = true;
                checkNumberTB.Visible = true;
                checkNameTB.Visible = true;
                acceptPaymentBtn.Enabled = true;
                acceptPaymentBtn.Visible = true;
                paymentAmountTB.Focus();
                paymentAmountTB.Text = (invoice.total - invoice.totalPayments).ToString();
                paymentAmountTB.SelectAll();
            }
            else if ((string)paymentTypeLB.SelectedItem == "Charge")
            {
                if (invoice.payments.Count > 0)
                {
                    MessageBox.Show("You can only charge the total amount of the invoice to a customer!");
                }
                invoice.payments.Add(new Payment() { paymentAmount = invoice.total, paymentType = Payment.PaymentTypes.Charge });
                paymentsLB.Items.Add("Charge: " + invoice.total.ToString("C"));
                invoice.totalPayments = invoice.total;
                paymentTypeLB.Enabled = false;
                finalizeBtn.Enabled = true;
                finalizeBtn.Focus();
                remainingBalanceTB.Text = "$0.00";
            }
        }

        private void AddPayment()
        {
            float amt = float.Parse(paymentAmountTB.Text);
            switch ((string)paymentTypeLB.SelectedItem)
            {
                
                case "Cash":
                    paymentsLB.Items.Add("Cash: " + amt.ToString("C"));
                    invoice.payments.Add(new Payment() { paymentAmount = amt, paymentType = Payment.PaymentTypes.Cash });
                    invoice.totalPayments += amt;
                    break;
                case "Payment Card":
                    paymentsLB.Items.Add("Payment Card: " + amt.ToString("C"));
                    invoice.payments.Add(new Payment() { paymentAmount = amt, paymentType = Payment.PaymentTypes.PaymentCard });
                    invoice.totalPayments += amt;
                    break;
                case "Paypal":
                    paymentsLB.Items.Add("Paypal: " + amt.ToString("C"));
                    invoice.payments.Add(new Payment() { paymentAmount = amt, paymentType = Payment.PaymentTypes.Paypal });
                    invoice.totalPayments += amt;
                    break;
                case "CashApp":
                    paymentsLB.Items.Add("CashApp: " + amt.ToString("C"));
                    invoice.payments.Add(new Payment() { paymentAmount = amt, paymentType = Payment.PaymentTypes.CashApp });
                    invoice.totalPayments += amt;
                    break;
                case "Venmo":
                    paymentsLB.Items.Add("Venmo: " + amt.ToString("C"));
                    invoice.payments.Add(new Payment() { paymentAmount = amt, paymentType = Payment.PaymentTypes.Venmo });
                    invoice.totalPayments += amt;
                    break;
                case "Check":
                    paymentsLB.Items.Add("Check " + checkNumberTB.Text + ": " + amt.ToString("C"));
                    invoice.payments.Add(new Payment() { paymentAmount = amt, paymentType = Payment.PaymentTypes.Check });
                    invoice.totalPayments += amt;
                    break;
            }

            if (invoice.totalPayments >= invoice.total)
            {
                finalizeBtn.Enabled = true;
                changeDueLbl.Visible = true;
                changeDueLbl.Text = "Change Due: " + (invoice.totalPayments - invoice.total).ToString("C");
                finalizeBtn.Focus();
                paymentIndexTB.Enabled = false;
            }
            else
            {
                paymentTypeLB.Enabled = true;
                paymentTypeLB.ClearSelected();
                paymentIndexTB.Enabled = true;
                paymentIndexTB.Clear();
                paymentIndexTB.Focus();
            }

            paymentAmountTB.Visible = false;
            checkNumberTB.Visible = false;
            checkNameTB.Visible = false;
            cancelPaymentBtn.Visible = false;
            acceptPaymentBtn.Visible = false;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (invoice.totalPayments > 0)
            {
                MessageBox.Show("An invoice with payments cannot be edited!");
                return;
            }
            Close();
        }

        private void acceptBtn_Click(object sender, EventArgs e)
        {
            if (containsAgeRestrictedItems)
            {
                if ((DateTime.Today - customerBirthdaySelector.Value).Days >= 7670) //21 years including leap days
                {
                    customerBirthdaySelector.Enabled = false;
                    attentionTB.Enabled = false;
                    poTB.Enabled = false;
                    acceptBtn.Enabled = false;

                    invoice.attentionLine = attentionTB.Text;
                    invoice.PONumber = poTB.Text;
                    invoice.customerBirthdate = customerBirthdaySelector.Value;
                    invoice.containsAgeRestrictedItem = true;

                    paymentTypeLB.Enabled = true;
                    paymentIndexTB.Focus();
                }
                else
                {
                    MessageBox.Show("Customer is not old enough to purchase some items on the invoice!");
                    return;
                }
            }
            else
            {
                customerBirthdaySelector.Enabled = false;
                attentionTB.Enabled = false;
                poTB.Enabled = false;
                acceptBtn.Enabled = false;

                invoice.attentionLine = attentionTB.Text;
                invoice.PONumber = poTB.Text;

                paymentTypeLB.Enabled = true;
                paymentIndexTB.Focus();
            }
        }

        private void paymentTypeLB_DoubleClick(object sender, EventArgs e)
        {
            if (paymentTypeLB.SelectedIndex == -1)
                return;

            paymentTypeLB.Enabled = false;
            SelectPaymentType();

        }

        private void paymentIndexTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                if (int.Parse(paymentIndexTB.Text) > paymentTypeLB.Items.Count || int.Parse(paymentIndexTB.Text) == 0)
                {
                    paymentIndexTB.SelectAll();
                    return;
                }

                paymentTypeLB.SelectedIndex = int.Parse(paymentIndexTB.Text) - 1;
                SelectPaymentType();
            }

        }

        private void paymentIndexTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
                return;

            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }
        }

        private void paymentAmountTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void paymentAmountTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                cancelPaymentBtn_Click(sender, e);
                return;
            }
            if (e.KeyCode == Keys.Enter)
            {
                if (paymentAmountTB.Text == String.Empty || float.Parse(paymentAmountTB.Text) == 0 || paymentAmountTB.Text == ".")
                    return;

                if (checkNumberTB.Visible == true)
                {
                    checkNumberTB.Focus();
                }
                else
                    acceptPaymentBtn.Focus();
            }

        }

        private void acceptPaymentBtn_Click(object sender, EventArgs e)
        {
            if (paymentAmountTB.Text == String.Empty)
            {
                paymentAmountTB.Focus();
                return;
            }
            AddPayment();
        }

        private void cancelPaymentBtn_Click(object sender, EventArgs e)
        {
            paymentAmountTB.Visible = false;
            checkNumberTB.Visible = false;
            checkNameTB.Visible = false;
            cancelPaymentBtn.Visible = false;
            acceptPaymentBtn.Visible = false;

            paymentTypeLB.Enabled = true;
            paymentIndexTB.Focus();
            paymentIndexTB.Clear();
        }

        private void checkNumberTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void checkNumberTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                checkNameTB.Focus();
            }
        }

        private void checkNameTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                acceptPaymentBtn.Focus();
            }
        }

        private void paymentsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (paymentsLB.SelectedIndex == -1)
                deletePaymentBtn.Enabled = false;
            else
                deletePaymentBtn.Enabled = true;
        }

        private void deletePaymentBtn_Click(object sender, EventArgs e)
        {
            string[] parsedLine = paymentsLB.SelectedItem.ToString().Split(':');
            parsedLine[1] = parsedLine[1].Trim();
            parsedLine[1] = parsedLine[1].Substring(1);
            Payment.PaymentTypes type = Payment.PaymentTypes.Cash;
            if (parsedLine[0].Contains("Cash"))
                type = Payment.PaymentTypes.Cash;
            if (parsedLine[0].Contains("Payment Card"))
                type = Payment.PaymentTypes.PaymentCard;
            if (parsedLine[0].Contains("Paypal"))
                type = Payment.PaymentTypes.PaymentCard;
            if (parsedLine[0].Contains("CashApp"))
                type = Payment.PaymentTypes.CashApp;
            if (parsedLine[0].Contains("Venmo"))
                type = Payment.PaymentTypes.Venmo;
            if (parsedLine[0].Contains("Check"))
                type = Payment.PaymentTypes.Check;
            if (parsedLine[0].Contains("Charge"))
                type = Payment.PaymentTypes.Charge;

            invoice.payments.Remove(invoice.payments.Find(el => el.paymentAmount == float.Parse(parsedLine[1]) && el.paymentType == type));
            invoice.totalPayments -= float.Parse(parsedLine[1]);
            paymentsLB.Items.Remove(paymentsLB.SelectedItem);
            if (invoice.total >= invoice.totalPayments)
            {
                changeDueLbl.Visible = false;
                finalizeBtn.Enabled = false;
                paymentTypeLB.Enabled = true;
                paymentTypeLB.ClearSelected();
                paymentIndexTB.Enabled = true;
                paymentIndexTB.Clear();
                paymentIndexTB.Focus();
            }
            else
            {
                changeDueLbl.Text = "Change Due: " + (invoice.totalPayments - invoice.total).ToString("C");
                finalizeBtn.Focus();
            }
        }

        private void finalizeBtn_Click(object sender, EventArgs e)
        {
            paymentsLB.Enabled = false;
            finalizeBtn.Enabled = false;
            closeBtn.Enabled = true;
            closeBtn.Focus();
            returnInvoiceBtn.Enabled = false;
            finalized = true;
            saveInvoiceBtn.Enabled = false;

            invoice.finalized = true;
            invoice.invoiceFinalizedTime = DateTime.Now;
            invoice.invoiceNumber = DatabaseHandler.SqlRetrieveNextInvoiceNumber();

            DatabaseHandler.SqlSaveReleasedInvoice(invoice);
            ReportViewer viewer = new ReportViewer(invoice);
            viewer.ShowDialog();
        }
    }
}
