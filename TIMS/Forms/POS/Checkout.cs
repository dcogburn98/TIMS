using System;
using System.Collections.Generic;
using System.Windows.Forms;

using TIMS.Server;
using TIMSServerModel;

using TIMS.Forms.POS;

using PaymentEngine.xTransaction;

namespace TIMS.Forms
{
    public partial class Checkout : Form
    {
        Invoice invoice;
        Invoicing parentWindow;
        bool containsAgeRestrictedItems;
        public bool finalized = false;
        public bool printed = false;

        public Checkout(Invoice invoice, Invoicing parentWindow)
        {
            this.parentWindow = parentWindow;

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
            if (invoice.total != 0)
            {
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
            }
            else
            {
                paymentTypeLB.Items.Add("Cash");
            }
            #endregion
        }

        private void PopulateInvoiceInfo()
        {
            int itemCount = 0;
            containsAgeRestrictedItems = false;
            int minimumAge = 0;
            invoice.taxableTotal = 0;
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

            invoice.taxRate = 0.1025m;
            invoice.taxAmount = invoice.taxableTotal * invoice.taxRate;
            invoice.total = Math.Round(invoice.subtotal + invoice.taxAmount, 2);

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
                    paymentTypeLB.Enabled = true;
                    paymentTypeLB.ClearSelected();
                    paymentIndexTB.Enabled = true;
                    paymentIndexTB.Clear();
                    paymentIndexTB.Focus();
                    return;
                }
                if (invoice.customer.accountBalance + invoice.total > invoice.customer.creditLimit)
                {
                    ChargeOverride chargeOverride = new ChargeOverride(invoice);
                    if (chargeOverride.ShowDialog() != DialogResult.OK)
                    {
                        paymentTypeLB.Enabled = true;
                        paymentTypeLB.ClearSelected();
                        paymentIndexTB.Enabled = true;
                        paymentIndexTB.Clear();
                        paymentIndexTB.Focus();
                        return;
                    }
                }
                invoice.payments.Add(new Payment() { paymentAmount = invoice.total, paymentType = Payment.PaymentTypes.Charge });
                paymentsLB.Items.Add("Charge: " + invoice.total.ToString("C"));
                invoice.totalPayments = invoice.total;
                paymentTypeLB.Enabled = false;
                finalizeBtn.Enabled = true;
                finalizeBtn.Focus();
                remainingBalanceTB.Text = "$0.00";
            }
            else if ((string)paymentTypeLB.SelectedItem == "Payment Card")
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
        }

        private void AddPayment()
        {
            decimal amt = decimal.Parse(paymentAmountTB.Text);
            switch ((string)paymentTypeLB.SelectedItem)
            {

                case "Cash":
                    paymentsLB.Items.Add("Cash: " + amt.ToString("C"));
                    invoice.payments.Add(new Payment() { paymentAmount = amt, paymentType = Payment.PaymentTypes.Cash });
                    invoice.totalPayments += amt;
                    remainingBalanceTB.Text = (invoice.total - invoice.totalPayments).ToString("C");
                    break;
                case "Payment Card":
                    if (Communication.RetrieveProperty("Integrated Card Payments") == "1")
                    {
                        Payment payment;
                        if (amt > 0)
                            payment = Communication.InitiatePayment(amt);
                        else
                            payment = Communication.InitiateRefund(Math.Abs(amt));

                        if (payment.errorMessage != Payment.CardReaderErrorMessages.None)
                        {
                            if (payment.errorMessage == Payment.CardReaderErrorMessages.NoAttachedDevice)
                            {
                                paymentsLB.Items.Add("Payment Card: " + amt.ToString("C"));
                                invoice.payments.Add(new Payment() { paymentAmount = amt, paymentType = Payment.PaymentTypes.PaymentCard });
                                invoice.totalPayments += amt;
                                break;
                            }
                        }
                        if (payment.cardResponse.CapturedAmount == 0)
                        {
                            MessageBox.Show("An error has occurred.");
                            break;
                        }
                        else
                        {
                            if (payment.cardResponse.CapturedAmount != payment.cardResponse.RequestedAmount)
                                MessageBox.Show("A partial payment of " + payment.paymentAmount.ToString("C") + " was made with this card.\nPlease request remaining amount in a different form of payment");
                            
                            payment.paymentType = Payment.PaymentTypes.PaymentCard;
                            paymentsLB.Items.Add("Payment Card: " + payment.paymentAmount.ToString("C"));
                            invoice.payments.Add(payment);
                            invoice.totalPayments += payment.paymentAmount;
                        }
                    }
                    else
                    {
                        paymentsLB.Items.Add("Payment Card: " + amt.ToString("C"));
                        invoice.payments.Add(new Payment() { paymentAmount = amt, paymentType = Payment.PaymentTypes.PaymentCard });
                        invoice.totalPayments += amt;
                        remainingBalanceTB.Text = (invoice.total - invoice.totalPayments).ToString("C");
                    }
                    break;
                case "Paypal":
                    paymentsLB.Items.Add("Paypal: " + amt.ToString("C"));
                    invoice.payments.Add(new Payment() { paymentAmount = amt, paymentType = Payment.PaymentTypes.Paypal });
                    invoice.totalPayments += amt;
                    remainingBalanceTB.Text = (invoice.total - invoice.totalPayments).ToString("C");
                    break;
                case "CashApp":
                    paymentsLB.Items.Add("CashApp: " + amt.ToString("C"));
                    invoice.payments.Add(new Payment() { paymentAmount = amt, paymentType = Payment.PaymentTypes.CashApp });
                    invoice.totalPayments += amt;
                    remainingBalanceTB.Text = (invoice.total - invoice.totalPayments).ToString("C");
                    break;
                case "Venmo":
                    paymentsLB.Items.Add("Venmo: " + amt.ToString("C"));
                    invoice.payments.Add(new Payment() { paymentAmount = amt, paymentType = Payment.PaymentTypes.Venmo });
                    invoice.totalPayments += amt;
                    remainingBalanceTB.Text = (invoice.total - invoice.totalPayments).ToString("C");
                    break;
                case "Check":
                    paymentsLB.Items.Add("Check " + checkNumberTB.Text + ": " + amt.ToString("C"));
                    invoice.payments.Add(new Payment() { paymentAmount = amt, paymentType = Payment.PaymentTypes.Check });
                    invoice.totalPayments += amt;
                    remainingBalanceTB.Text = (invoice.total - invoice.totalPayments).ToString("C");
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
            DialogResult = DialogResult.Cancel;
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
                if (paymentIndexTB.Text == String.Empty || int.Parse(paymentIndexTB.Text) > paymentTypeLB.Items.Count || int.Parse(paymentIndexTB.Text) < 1)
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

            invoice.payments.Remove(invoice.payments.Find(el => el.paymentAmount == decimal.Parse(parsedLine[1]) && el.paymentType == type));
            invoice.totalPayments -= decimal.Parse(parsedLine[1]);
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
            CancelButton = closeBtn;

            paymentsLB.Enabled = false;
            finalizeBtn.Enabled = false;
            closeBtn.Enabled = true;
            closeBtn.Focus();
            returnInvoiceBtn.Enabled = false;
            finalized = true;
            saveInvoiceBtn.Enabled = false;

            foreach (InvoiceItem invItem in invoice.items)
            {
                Item newItem = Communication.RetrieveItem(invItem.itemNumber, invItem.productLine);
                newItem.onHandQty -= invItem.quantity;
                invItem.cost = newItem.replacementCost;
                invoice.cost += invItem.cost;
                if (invoice.savedInvoice)
                    newItem.WIPQty -= invItem.quantity;
                Communication.UpdateItem(newItem);
            }

            foreach (Payment p in invoice.payments)
            {
                if (p.paymentType == Payment.PaymentTypes.Charge)
                {
                    invoice.customer.accountBalance += p.paymentAmount;
                    Communication.UpdateCustomer(invoice.customer);
                }
            }

            invoice.finalized = true;
            invoice.savedInvoice = false;
            invoice.invoiceFinalizedTime = DateTime.Now;
            invoice.invoiceNumber = invoice.invoiceNumber == 0 ? Communication.RetrieveNextInvoiceNumber() : invoice.invoiceNumber;
            invoice.totalPayments = invoice.total;
            invoice.profit = invoice.subtotal - invoice.cost;
            Communication.SaveInvoice(invoice);

            #region Accounting Transactions

            List<Transaction> salesTransactions = new List<Transaction>();
            List<Transaction> salesTaxTransactions = new List<Transaction>();
            Transaction inventoryTransaction = null;

            if (invoice.total != 0)
                foreach (Payment p in invoice.payments)
                {
                    if (p.paymentType == Payment.PaymentTypes.PaymentCard)
                    {
                        if (p.paymentAmount > 0)
                        {
                            salesTransactions.Add(new Transaction(2, 10, p.paymentAmount - Math.Round((p.paymentAmount / invoice.total) * invoice.taxAmount, 2)) //2 - Checking Account  5 - Cash Sales Account
                            {
                                transactionID = Communication.RetrieveNextTransactionNumber(),
                                referenceNumber = invoice.invoiceNumber,
                                memo = "Subtotal transaction for card sale"
                            });
                            salesTaxTransactions.Add(new Transaction(2, 7, Math.Round((p.paymentAmount / invoice.total) * invoice.taxAmount, 2)) //2 - Checking Account  7 - Sales Tax Payable Account
                            {
                                transactionID = Communication.RetrieveNextTransactionNumber(),
                                referenceNumber = invoice.invoiceNumber,
                                memo = "Tax transaction for card sale"
                            });
                        }
                        else
                        {
                            salesTransactions.Add(new Transaction(10, 2, Math.Abs(p.paymentAmount) - Math.Abs(Math.Round((p.paymentAmount / invoice.total) * invoice.taxAmount, 2))) //2 - Checking Account  5 - Cash Sales Account
                            {
                                transactionID = Communication.RetrieveNextTransactionNumber(),
                                referenceNumber = invoice.invoiceNumber,
                                memo = "Refund subtotal for card sale"
                            });
                            salesTaxTransactions.Add(new Transaction(7, 2, Math.Abs(Math.Round((p.paymentAmount / invoice.total) * invoice.taxAmount, 2))) //2 - Checking Account  7 - Sales Tax Payable Account
                            {
                                transactionID = Communication.RetrieveNextTransactionNumber(),
                                referenceNumber = invoice.invoiceNumber,
                                memo = "Refund tax for card sale"
                            });
                        }
                    }
                    else if (p.paymentType == Payment.PaymentTypes.Charge)
                    {
                        if (p.paymentAmount > 0)
                        {
                            salesTransactions.Add(new Transaction(11, 6, p.paymentAmount - Math.Round((p.paymentAmount / invoice.total) * invoice.taxAmount, 2)) //11 - A/R Account  6 - Credit Sales Account
                            {
                                transactionID = Communication.RetrieveNextTransactionNumber(),
                                referenceNumber = invoice.invoiceNumber,
                                memo = "Subtotal transaction for charge sale"
                            });
                            salesTaxTransactions.Add(new Transaction(11, 7, Math.Round((p.paymentAmount / invoice.total) * invoice.taxAmount, 2)) //11 - A/R Account  7 - Sales Tax Payable Account
                            {
                                transactionID = Communication.RetrieveNextTransactionNumber(),
                                referenceNumber = invoice.invoiceNumber,
                                memo = "Tax transaction for charge sale"
                            });
                        }
                        else
                        {
                            salesTransactions.Add(new Transaction(6, 11, Math.Abs(p.paymentAmount) - Math.Abs(Math.Round((p.paymentAmount / invoice.total) * invoice.taxAmount, 2))) //11 - A/R Account  6 - Credit Sales Account
                            {
                                transactionID = Communication.RetrieveNextTransactionNumber(),
                                referenceNumber = invoice.invoiceNumber,
                                memo = "Credit subtotal for charge sale"
                            });
                            salesTaxTransactions.Add(new Transaction(7, 11, Math.Abs(Math.Round((p.paymentAmount / invoice.total) * invoice.taxAmount, 2))) //11 - A/R Account  7 - Sales Tax Payable Account
                            {
                                transactionID = Communication.RetrieveNextTransactionNumber(),
                                referenceNumber = invoice.invoiceNumber,
                                memo = "Tax credit for charge sale"
                            });
                        }
                    }
                    else
                    {
                        if (p.paymentAmount > 0)
                        {
                            salesTransactions.Add(new Transaction(14, 5, p.paymentAmount - Math.Round((p.paymentAmount / invoice.total) * invoice.taxAmount, 2)) //14 - Cash Drawer Account  5 - Cash Sales Account
                            {
                                transactionID = Communication.RetrieveNextTransactionNumber(),
                                referenceNumber = invoice.invoiceNumber,
                                memo = "Subtotal transaction for cash sale"
                            });
                            salesTaxTransactions.Add(new Transaction(14, 7, Math.Round((p.paymentAmount / invoice.total) * invoice.taxAmount, 2)) //14 - Cash Drawer Account  7 - Sales Tax Payable Account
                            {
                                transactionID = Communication.RetrieveNextTransactionNumber(),
                                referenceNumber = invoice.invoiceNumber,
                                memo = "Tax transaction for cash sale"
                            });
                        }
                        else
                        {
                            salesTransactions.Add(new Transaction(5, 14, Math.Abs(p.paymentAmount) - Math.Abs(Math.Round((p.paymentAmount / invoice.total) * invoice.taxAmount, 2))) //14 - Cash Drawer Account  5 - Cash Sales Account
                            {
                                transactionID = Communication.RetrieveNextTransactionNumber(),
                                referenceNumber = invoice.invoiceNumber,
                                memo = "Refund subtotal for cash sale"
                            });
                            salesTaxTransactions.Add(new Transaction(7, 14, Math.Abs(Math.Round((p.paymentAmount / invoice.total) * invoice.taxAmount, 2))) //14 - Cash Drawer Account  7 - Sales Tax Payable Account
                            {
                                transactionID = Communication.RetrieveNextTransactionNumber(),
                                referenceNumber = invoice.invoiceNumber,
                                memo = "Refund tax for cash sale"
                            });
                        }
                    }
                }

            if (invoice.cost < 0)
            {
                inventoryTransaction = new Transaction(8, 1, invoice.cost)
                {
                    transactionID = Communication.RetrieveNextTransactionNumber(),
                    referenceNumber = invoice.invoiceNumber,
                    memo = "Inventory transaction"
                };
            }
            else
            {
                inventoryTransaction = new Transaction(1, 8, invoice.cost)
                {
                    transactionID = Communication.RetrieveNextTransactionNumber(),
                    referenceNumber = invoice.invoiceNumber,
                    memo = "Inventory return transaction"
                };
            }

            if (((salesTaxTransactions.Count < 1 || salesTransactions.Count < 1) && invoice.total != 0) || 
                 (inventoryTransaction == null && invoice.cost != 0))
                MessageBox.Show("There was an error completing transactions for this sale in accounting. Please make sure to record those transactions in the ledger in order to keep records accurate.");
            else
            {
                if (invoice.total != 0)
                    foreach (Transaction t in salesTransactions)
                        Communication.SaveTransaction(t);

                if (invoice.total != 0)
                    foreach (Transaction t in salesTaxTransactions)
                        Communication.SaveTransaction(t);

                if (invoice.cost != 0)
                    Communication.SaveTransaction(inventoryTransaction);
            }

            #endregion

            Communication.PrintReceipt(invoice);
            ReportViewer viewer = new ReportViewer(invoice);
            viewer.ShowDialog();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
            //parentWindow.Focus();
            //parentWindow.CancelInvoice();
        }

        private void saveInvoiceBtn_Click(object sender, EventArgs e)
        {
            if (invoice.payments.Count > 0)
            {
                MessageBox.Show("An invoice with payments cannot be edited.\nPlease remove the payments to return to the invoicing screen.");
                return;
            }

            if (MessageBox.Show("Do you want to save this invoice for later?", "Save Invoice", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (InvoiceItem item in invoice.items)
                {
                    Item itemm = Communication.RetrieveItem(item.itemNumber, item.productLine);
                    itemm.WIPQty += item.quantity;
                    Communication.UpdateItem(itemm);
                }
                invoice.savedInvoice = true;
                invoice.savedInvoiceTime = DateTime.Now;
                invoice.attentionLine = attentionTB.Text;
                invoice.PONumber = poTB.Text;
                invoice.invoiceNumber = Communication.RetrieveNextInvoiceNumber();
                Communication.SaveInvoice(invoice);
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
