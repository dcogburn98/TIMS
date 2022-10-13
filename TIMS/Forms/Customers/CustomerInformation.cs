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

namespace TIMS.Forms.Customers
{
    public partial class CustomerInformation : Form
    {
        public Customer currentCustomer;
        public bool customerEdited;

        public CustomerInformation()
        {
            InitializeComponent();
            customerEdited = false;

            dataGridView5.Rows[dataGridView5.Rows.Add()].Cells[0].Value = "Net Sales";
            dataGridView5.Rows[dataGridView5.Rows.Add()].Cells[0].Value = "Net Cost";
            dataGridView5.Rows[dataGridView5.Rows.Add()].Cells[0].Value = "Gross Profit";
            dataGridView5.Rows[dataGridView5.Rows.Add()].Cells[0].Value = "Returns";
            dataGridView5.Rows[dataGridView5.Rows.Add()].Cells[0].Value = "Returns % of Sale";
            dataGridView5.Rows[dataGridView5.Rows.Add()].Cells[0].Value = "Invoice Count";
            dataGridView5.Rows[dataGridView5.Rows.Add()].Cells[0].Value = "Net Delivered Sales";
            dataGridView5.Rows[dataGridView5.Rows.Add()].Cells[0].Value = "Net Delivered Cost";
            dataGridView5.Rows[dataGridView5.Rows.Add()].Cells[0].Value = "Delivered Gross Profit";
            dataGridView5.Rows[dataGridView5.Rows.Add()].Cells[0].Value = "Defective Returns";
            dataGridView5.Rows[dataGridView5.Rows.Add()].Cells[0].Value = "Un-Needed Returns";
            dataGridView5.Rows[dataGridView5.Rows.Add()].Cells[0].Value = "Special Order Sales";
            dataGridView5.Rows[dataGridView5.Rows.Add()].Cells[0].Value = "Net Special Order Cost";
            dataGridView5.Rows[dataGridView5.Rows.Add()].Cells[0].Value = "Special Order Gross Profit";

            accountNumberTB.Focus();
        }

        private void PopulateFields()
        {
            #region Account Information
            accountNumberTB.Text = currentCustomer.customerNumber;
            accountNameTB.Text = currentCustomer.customerName;
            customerNumber2TB.Text = currentCustomer.customerNumber;
            customerName2TB.Text = currentCustomer.customerName;
            physicalAddressTB.Text = currentCustomer.shippingAddress.Split(',')[0].Trim();
            physicalCityTB.Text = currentCustomer.shippingAddress.Split(',')[1].Trim();
            physicalStateTB.Text = currentCustomer.shippingAddress.Split(',')[2].Trim();
            physicalZipTB.Text = currentCustomer.shippingAddress.Split(',')[3].Trim();
            physicalCountryTB.Text = currentCustomer.shippingAddress.Split(',')[4].Trim();
            phoneNumberTB.Text = currentCustomer.phoneNumber;
            faxNumberTB.Text = currentCustomer.faxNumber;
            websiteTB.Text = currentCustomer.website;
            emailTB.Text = currentCustomer.email;
            salesRepTB.Text = currentCustomer.assignedRep;
            businessCategoryTB.Text = currentCustomer.businessCategory;
            invoiceNoteTB.Text = currentCustomer.invoiceMessage;

            dateAdded.Value = currentCustomer.dateAdded;
            dateLastPayment.Value = currentCustomer.dateOfLastROA;
            dateLasteSale.Value = currentCustomer.dateOfLastSale;
            languagePreferenceTB.Text = currentCustomer.preferredLanguage;
            foreach (string buyer in currentCustomer.authorizedBuyers)
                authorizedBuyersLB.Items.Add(buyer);
            #endregion

            #region Invoicing Information
            primaryTaxStatusCB.SelectedIndex = currentCustomer.primaryTaxStatus == "Exempt" ? 1 : 0;
            secondaryTaxStatusCB.SelectedIndex = currentCustomer.secondaryTaxStatus == "Exempt" ? 1 : 0;
            primaryTaxExemptNumberTB.Text = currentCustomer.primaryTaxExemptionNumber;
            secondaryTaxExemptNumberTB.Text = currentCustomer.secondaryTaxExemptionNumber;
            primaryTaxExemptExpiration.Value = currentCustomer.primaryTaxExemptionExpiration;
            secondaryTaxExemptExpiration.Value = currentCustomer.secondaryTaxExemptionExpiration;

            printCatalogNotesCB.Checked = currentCustomer.printCatalogNotes;
            printBalanceCB.Checked = currentCustomer.printBalance;
            emailInvoicesCB.Checked = currentCustomer.emailInvoices;
            allowBackordersCB.Checked = currentCustomer.allowBackorders;
            allowSpecialOrdersCB.Checked = currentCustomer.allowSpecialOrders;
            exemptFromSurchargesCB.Checked = currentCustomer.exemptFromInvoiceSurcharges;
            extraInvoiceCopiesTB.Text = currentCustomer.extraInvoiceCopies.ToString();
            poReqdOverAmountTB.Text = currentCustomer.PORequiredThresholdAmount.ToString();
            switch (currentCustomer.billingType)
            {
                case "Cash Only":
                    billingTypeCB.SelectedIndex = 0;
                    break;
                case "Cash Only(Include Mobile Payments)":
                    billingTypeCB.SelectedIndex = 1;
                    break;
                case "Charge Only":
                    billingTypeCB.SelectedIndex = 2;
                    break;
                case "Cash Or Charge":
                    billingTypeCB.SelectedIndex = 3;
                    break;
                case "Cash Or Charge(Include Mobile Payments)":
                    billingTypeCB.SelectedIndex = 4;
                    break;
            }

            defaultDeliverCB.Checked = currentCustomer.defaultToDeliver;
            travelTimeTB.Text = currentCustomer.travelTime.ToString();
            travelDistanceTB.Text = currentCustomer.travelDistance.ToString();
            minSaleFreeDeliveryTB.Text = currentCustomer.minimumSaleFreeDelivery.ToString();
            deliveryChargeTB.Text = currentCustomer.deliveryCharge.ToString();
            #endregion

            #region AR Information
            creditLimitTB.Text = currentCustomer.creditLimit.ToString();
            doNotChargeCB.Checked = currentCustomer.canCharge;
            switch (currentCustomer.statementType)
            {
                case "Balance Forward":
                    statementTypeCB.SelectedIndex = 0;
                    break;
                case "Open Item":
                    statementTypeCB.SelectedIndex = 1;
                    break;
            }
            extraStatementCopiesTB.Text = currentCustomer.extraStatementCopies.ToString();
            sendInvoicesEvery_DaysTB.Text = currentCustomer.sendInvoicesEvery_Days.ToString();
            sendAccountSummaryEvery_DaysTB.Text = currentCustomer.sendAccountSummaryEvery_Days.ToString();
            emailStatementsCB.Checked = currentCustomer.emailStatements;

            discountPercentTB.Text = currentCustomer.percentDiscount.ToString();
            discountDaysTB.Text = currentCustomer.paidForByDiscount.ToString();
            dueByTB.Text = currentCustomer.dueDate.ToString();

            if (currentCustomer.statementMailingAddress != string.Empty)
            {
                useDifferentAddressCB.Checked = true;
                mailingAddressTB.Text = currentCustomer.statementMailingAddress.Split(',')[0].Trim();
                mailingCityTB.Text = currentCustomer.statementMailingAddress.Split(',')[1].Trim();
                mailingStateTB.Text = currentCustomer.statementMailingAddress.Split(',')[2].Trim();
                mailingZipTB.Text = currentCustomer.statementMailingAddress.Split(',')[3].Trim();
                mailingCountryTB.Text = currentCustomer.statementMailingAddress.Split(',')[4].Trim();
            }
            else
                useDifferentAddressCB.Checked = false;

            lastPaymentAmountTB.Text = currentCustomer.lastPaymentAmount.ToString();
            lastPaymentDateTB.Text = currentCustomer.lastPaymentDate == DateTime.MinValue ? "" : currentCustomer.lastPaymentDate.ToString("MM/dd/yyyy");
            highestAmountOwedTB.Text = currentCustomer.highestAmountOwed.ToString();
            highestAmountOwedDateTB.Text = currentCustomer.highestAmountOwedDate == DateTime.MinValue ? "" : currentCustomer.highestAmountOwedDate.ToString("MM/dd/yyyy");
            highestAmountPaidTB.Text = currentCustomer.highestAmountPaid.ToString();
            highestAmountPaidDateTB.Text = currentCustomer.highestAmountPaidDate == DateTime.MinValue ? "" : currentCustomer.highestAmountPaidDate.ToString("MM/dd/yyyy");

            lastStatementAmount.Text = currentCustomer.lastStatementAmount.ToString();
            totalDueTB.Text = currentCustomer.totalDue.ToString();
            due30TB.Text = currentCustomer.due30.ToString();
            due60TB.Text = currentCustomer.due60.ToString();
            due90TB.Text = currentCustomer.due90.ToString();
            dueFurtherTB.Text = currentCustomer.furtherDue.ToString();
            serviceChargeTB.Text = currentCustomer.serviceCharge.ToString();
            accountBalanceTB.Text = currentCustomer.accountBalance.ToString();
            #endregion
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
                    if (MessageBox.Show("Account does not exist. Would you like to create it?", "Customer does not exist!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        MessageBox.Show("This doesn't work yet. The developer forgot about this part.\n\nIt's pretty important, how could he have overlooked this?\n\nWould you please send him a strongly worded message and tell him he's a terrible dev?");
                    }
                }
                else
                    PopulateFields();
            }
        }

        private void CustomerInformation_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (customerEdited)
            {
                if (MessageBox.Show("There are unsaved changes made to the current customer!\n\nThese changes will be discarded if you continue.", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void searchCustomerButton_Click(object sender, EventArgs e)
        {
            CustomerSearch search = new CustomerSearch();
            if (search.ShowDialog() == DialogResult.OK)
                currentCustomer = search.selectedCustomer;
            else
                return;

            PopulateFields();
        }
    }
}
