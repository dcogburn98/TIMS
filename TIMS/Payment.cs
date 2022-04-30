using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMS
{
    class Payment
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
