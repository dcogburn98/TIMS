using System;

namespace TIMSServer
{
    [Serializable]
    public class Customer
    {
        public string customerName;
        public string customerNumber;
        public bool taxExempt;
        public string taxExemptionNumber;
        public string pricingProfile;

        public Payment.PaymentTypes[] availablePaymentTypes;
        public bool canCharge;
        public float creditLimit;
        public float accountBalance;

        public string phoneNumber;
        public string faxNumber;
        public string customerStreetAddress;
        public string customerCity;
        public string customerState;
        public string customerZip;

        public string invoiceMessage;
    }
}
