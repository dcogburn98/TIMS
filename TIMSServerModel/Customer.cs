using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel
{
    public class Customer
    {
        public AuthKey key;

        public string customerName;
        public string customerNumber;
        public string pricingProfile;
        public List<Payment.PaymentTypes> availablePaymentTypes;
        public bool canCharge;
        public decimal creditLimit;
        public decimal accountBalance;
        public string phoneNumber;
        public string faxNumber;
        public string billingAddress;
        public string shippingAddress;
        public string invoiceMessage;
        public string website;
        public string email;
        public string assignedRep;
        public string businessCategory;
        public DateTime dateAdded;
        public DateTime dateOfLastSale;
        public DateTime dateOfLastROA;
        public string preferredLanguage;
        public List<string> authorizedBuyers;
        public string defaultTaxTable;
        public string deliveryTaxTable;
        public string primaryTaxStatus;
        public string secondaryTaxStatus;
        public string primaryTaxExemptionNumber;
        public string secondaryTaxExemptionNumber;
        public DateTime primaryTaxExemptionExpiration;
        public DateTime secondaryTaxExemptionExpiration;
        public bool printCatalogNotes;
        public bool printBalance;
        public bool emailInvoices;
        public bool allowBackorders;
        public bool allowSpecialOrders;
        public bool exemptFromInvoiceSurcharges;
        public int extraInvoiceCopies;
        public decimal PORequiredThresholdAmount;
        public string billingType;
        public bool defaultToDeliver;
        public string deliveryRoute;
        public int travelTime;
        public int travelDistance;
        public decimal minimumSaleFreeDelivery;
        public decimal deliveryCharge;
        public string statementType;
        public decimal percentDiscount;
        public int paidForByDiscount;
        public int dueDate;
        public int extraStatementCopies;
        public int sendInvoicesEvery_Days;
        public int sendAccountSummaryEvery_Days;
        public bool emailStatements;
        public string statementMailingAddress;
        public decimal lastPaymentAmount;
        public DateTime lastPaymentDate;
        public decimal highestAmountOwed;
        public DateTime highestAmountOwedDate;
        public decimal highestAmountPaid;
        public DateTime highestAmountPaidDate;
        public decimal lastStatementAmount;
        public decimal totalDue;
        public decimal due30;
        public decimal due60;
        public decimal due90;
        public decimal furtherDue;
        public decimal serviceCharge;
        public bool enabledTIMSServerRelations;
        public string relationshipKey;
        public bool automaticallySendPriceUpdates;
        public bool automaticallySendMedia;

        public Invoice[] currentSavedInvoices;
        public Invoice[] invoiceHistory;

        public Customer()
        {
            availablePaymentTypes = new List<Payment.PaymentTypes>();
        }
    }
}
