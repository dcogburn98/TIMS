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
            foreach (string buyer in currentCustomer.authorizedBuyers.Split(','))
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

            #region Pricing Profiles
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            dataGridView4.Rows.Clear();

            foreach (PricingProfile profile in Communication.RetrievePricingProfiles())
            {
                int row = dataGridView2.Rows.Add();
                dataGridView2.Rows[row].Cells[0].Value = profile.ProfileID;
                dataGridView2.Rows[row].Cells[1].Value = profile.ProfileName;
                dataGridView2.Rows[row].Tag = profile;
            }

            defaultInstorePriceSheet.Text = Enum.GetName(typeof(PricingProfileElement.PriceSheets), currentCustomer.inStorePricingProfile.defaultPriceSheet);
            foreach (PricingProfile profile in currentCustomer.inStorePricingProfile.Profiles)
            {
                int row = dataGridView3.Rows.Add();
                dataGridView3.Rows[row].Cells[0].Value = profile.ProfileID;
                dataGridView3.Rows[row].Cells[1].Value = profile.ProfileName;
                dataGridView3.Rows[row].Tag = profile;
            }

            defaultOnlinePriceSheet.Text = Enum.GetName(typeof(PricingProfileElement.PriceSheets), currentCustomer.onlinePricingProfile.defaultPriceSheet);
            foreach (PricingProfile profile in currentCustomer.onlinePricingProfile.Profiles)
            {
                int row = dataGridView4.Rows.Add();
                dataGridView4.Rows[row].Cells[0].Value = profile.ProfileID;
                dataGridView4.Rows[row].Cells[1].Value = profile.ProfileName;
                dataGridView4.Rows[row].Tag = profile;
            }

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

        private void button12_Click(object sender, EventArgs e)
        {
            currentCustomer.customerName = customerName2TB.Text;
            currentCustomer.canCharge = !doNotChargeCB.Checked;
            currentCustomer.creditLimit = decimal.Parse(creditLimitTB.Text);
            currentCustomer.phoneNumber = phoneNumberTB.Text;
            currentCustomer.faxNumber = faxNumberTB.Text;
            currentCustomer.shippingAddress = physicalAddressTB.Text + ", " + physicalCityTB.Text + ", " + physicalStateTB.Text + ", " + physicalZipTB.Text + ", " + physicalCountryTB.Text;
            currentCustomer.invoiceMessage = invoiceNoteTB.Text;
            currentCustomer.website = websiteTB.Text;
            currentCustomer.email = emailTB.Text;
            currentCustomer.assignedRep = salesRepTB.Text;
            currentCustomer.businessCategory = businessCategoryTB.Text;
            currentCustomer.preferredLanguage = languagePreferenceTB.Text;

            string buyers = "";
            foreach (string item in authorizedBuyersLB.Items)
                buyers += item + ",";
            buyers.TrimEnd(',');
            currentCustomer.authorizedBuyers = buyers;

            currentCustomer.defaultTaxTable = defaultTaxTableCB.Text;
            currentCustomer.deliveryTaxTable = deliveryTaxTableCB.Text;
            currentCustomer.primaryTaxStatus = primaryTaxStatusCB.Text;
            currentCustomer.secondaryTaxStatus = secondaryTaxStatusCB.Text;
            currentCustomer.primaryTaxExemptionNumber = primaryTaxExemptNumberTB.Text;
            currentCustomer.secondaryTaxExemptionNumber = secondaryTaxExemptNumberTB.Text;
            currentCustomer.primaryTaxExemptionExpiration = primaryTaxExemptExpiration.Value;
            currentCustomer.secondaryTaxExemptionExpiration = secondaryTaxExemptExpiration.Value;
            currentCustomer.printCatalogNotes = printCatalogNotesCB.Checked;
            currentCustomer.printBalance = printBalanceCB.Checked;
            currentCustomer.emailInvoices = emailInvoicesCB.Checked;
            currentCustomer.allowBackorders = allowBackordersCB.Checked;
            currentCustomer.allowSpecialOrders = allowSpecialOrdersCB.Checked;
            currentCustomer.exemptFromInvoiceSurcharges = exemptFromSurchargesCB.Checked;
            currentCustomer.extraInvoiceCopies = int.Parse(extraInvoiceCopiesTB.Text);
            currentCustomer.PORequiredThresholdAmount = decimal.Parse(poReqdOverAmountTB.Text);
            currentCustomer.billingType = billingTypeCB.Text;
            currentCustomer.defaultToDeliver = defaultDeliverCB.Checked;
            currentCustomer.deliveryRoute = deliveryRouteCB.Text;
            currentCustomer.travelDistance = int.Parse(travelDistanceTB.Text);
            currentCustomer.travelTime = int.Parse(travelTimeTB.Text);
            currentCustomer.minimumSaleFreeDelivery = decimal.Parse(minSaleFreeDeliveryTB.Text);
            currentCustomer.deliveryCharge = decimal.Parse(deliveryChargeTB.Text);
            currentCustomer.statementType = statementTypeCB.Text;
            currentCustomer.percentDiscount = decimal.Parse(discountPercentTB.Text);
            currentCustomer.paidForByDiscount = int.Parse(discountDaysTB.Text);
            currentCustomer.dueDate = int.Parse(dueByTB.Text);
            currentCustomer.extraStatementCopies = int.Parse(extraStatementCopiesTB.Text);
            currentCustomer.sendInvoicesEvery_Days = int.Parse(sendInvoicesEvery_DaysTB.Text);
            currentCustomer.sendAccountSummaryEvery_Days = int.Parse(sendAccountSummaryEvery_DaysTB.Text);
            currentCustomer.emailStatements = emailStatementsCB.Checked;

            if (useDifferentAddressCB.Checked)
            {
                string addr = mailingAddressTB + ", " + mailingCityTB + ", " + mailingStateTB + ", " + mailingZipTB + ", " + mailingCountryTB;
                currentCustomer.statementMailingAddress = addr;
            }

            currentCustomer.enabledTIMSServerRelations = enableTIMSRelationsCB.Checked;
            currentCustomer.relationshipKey = relationshipKeyTB.Text;
            currentCustomer.automaticallySendMedia = autoMediaUpdatesCB.Checked;
            currentCustomer.automaticallySendPriceUpdates = autoPriceUpdatesCB.Checked;

            Communication.UpdateCustomer(currentCustomer);
            MessageBox.Show("Customer Information Updated");
        }

        private void addCustomerButton_Click(object sender, EventArgs e)
        {
            if (customerEdited)
                if (MessageBox.Show("Current customer has unsaved changes. Would you like to proceed WITHOUT saving?", "Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;

            AddCustomer adder = new AddCustomer();
            if (adder.ShowDialog() == DialogResult.Cancel)
                return;

            currentCustomer = adder.customer;
            Communication.AddCustomer(currentCustomer);
            MessageBox.Show("Customer successfully created!");
            PopulateFields();
        }
    }
}
