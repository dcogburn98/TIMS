﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel
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