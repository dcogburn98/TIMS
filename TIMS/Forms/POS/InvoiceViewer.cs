using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace TIMS.Forms.POS
{
    public partial class InvoiceViewer : Form
    {
        Invoice inv;
        public InvoiceViewer(Invoice inv)
        {
            InitializeComponent();
            CancelButton = closeButton;

            this.inv = inv;
            foreach (InvoiceItem item in inv.items)
            {
                int row = dataGridView1.Rows.Add();

                dataGridView1.Rows[row].Cells[0].Value = item.itemNumber;
                dataGridView1.Rows[row].Cells[1].Value = item.productLine;
                dataGridView1.Rows[row].Cells[2].Value = item.itemName;
                dataGridView1.Rows[row].Cells[3].Value = item.quantity;
                dataGridView1.Rows[row].Cells[4].Value = item.serialNumber;
                dataGridView1.Rows[row].Cells[5].Value = item.listPrice.ToString("C");
                dataGridView1.Rows[row].Cells[6].Value = item.price.ToString("C");
                dataGridView1.Rows[row].Cells[7].Value = item.total.ToString("C");
                dataGridView1.Rows[row].Cells[8].Value = item.taxed;
                dataGridView1.Rows[row].Cells[9].Value = item.codes;
            }
            foreach (Payment pay in inv.payments)
            {
                paymentsListBox.Items.Add(pay.paymentType + ": " + pay.paymentAmount.ToString("C"));
            }

            #region Invoice Information Section
            invNoLabel.Text = inv.invoiceNumber.ToString();
            dateLabel.Text = inv.invoiceFinalizedTime.ToString("MM/dd/yyyy");
            timeLabel.Text = inv.invoiceFinalizedTime.ToString("hh:mm tt");
            subtotalLabel.Text = inv.subtotal.ToString("C");
            taxableTotalLabel.Text = inv.taxableTotal.ToString("C");
            taxRateLabel.Text = inv.taxRate.ToString("P");
            taxAmountLabel.Text = inv.taxAmount.ToString("C");
            totalLabel.Text = inv.total.ToString("C");

            if (inv.payments.Count > 1)
                paymentTypeLabel.Text = "MULTIPLE";
            else
                paymentTypeLabel.Text = inv.payments[0].paymentType + " (" + inv.payments[0].paymentAmount.ToString("C") + ")";

            if (inv.containsAgeRestrictedItem)
                ageRestrictedLabel.Text = "True (" + inv.items.OrderByDescending(el => el.minimumAge).FirstOrDefault().minimumAge + ")";
            else
                ageRestrictedLabel.Text = "False";

            attentionLabel.Text = inv.attentionLine;
            poLabel.Text = inv.PONumber;
            messageLabel.Text = inv.invoiceMessage;
            voidLabel.Text = inv.voided.ToString();
            #endregion

            #region Customer Information Section
            customerNoLabel.Text = inv.customer.customerNumber;
            customerNameLabel.Text = inv.customer.customerName;
            if (inv.customer.taxExempt)
                taxExemptionLabel.Text = "True (" + inv.customer.taxExemptionNumber + ")";
            else
                taxExemptionLabel.Text = "False";
            pricingProfileLabel.Text = inv.customer.pricingProfile;
            if (inv.customer.canCharge)
            {
                allowChargingLabel.Text = "True";
                creditLimitLabel.Text = inv.customer.creditLimit.ToString("C");
                newBalanceLabel.Text = inv.customer.accountBalance.ToString("C");
            }
            else
            {
                allowChargingLabel.Text = "False";
                creditLimitLabel.Text = 0.00f.ToString("C");
                newBalanceLabel.Text = inv.customer.accountBalance.ToString("C");
            }
            if (inv.containsAgeRestrictedItem)
                birthdateLabel.Text = inv.customerBirthdate.ToString("MM/dd/yyyy");
            else
                birthdateLabel.Text = "N/A";
            contactPhoneLabel.Text = inv.customer.phoneNumber;
            faxLabel.Text = inv.customer.faxNumber;
            string[] mailing = inv.customer.mailingAddress.Split(',');
            mailingAddressLn1Label.Text = mailing[0].Trim();
            mailingAddressLn2Label.Text = mailing[1].Trim() + "," + mailing[2] + mailing[3] + "," + mailing[4];
            string[] shipping = inv.customer.shippingAddress.Split(',');
            shippingAddressLn1Label.Text = shipping[0].Trim();
            shippingAddressLn2Label.Text = shipping[1].Trim() + "," + shipping[2] + shipping[3] + "," + shipping[4];

            #endregion

            employeeNumberLabel.Text = inv.employee.employeeNumber.ToString();
            employeeNameLabel.Text = inv.employee.fullName;
            employeePosLabel.Text = inv.employee.position;
        }

        private void printButton_Click(object sender, EventArgs e)
        {
            ReportViewer viewer = new ReportViewer(inv);
            viewer.ShowDialog();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
