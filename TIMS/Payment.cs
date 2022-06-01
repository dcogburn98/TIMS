﻿using System;

namespace TIMS
{
    public class Payment
    {
        public enum PaymentTypes
        {
            Cash,
            Check,
            PaymentCard,
            Charge,
            CashApp,
            Venmo,
            Paypal
        }
        public PaymentTypes paymentType;
        public decimal paymentAmount;
        public Guid ID = Guid.NewGuid();
    }
}
