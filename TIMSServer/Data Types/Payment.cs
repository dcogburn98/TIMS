using System;

namespace TIMSServer
{
    [Serializable]
    public class Payment
    {
        public enum PaymentTypes
        {
            Cash,
            Check,
            PaymentCard,
            Charge,
            CashApp,
            Venmo
        }
        public PaymentTypes paymentType;
        public float paymentAmount;
    }
}
