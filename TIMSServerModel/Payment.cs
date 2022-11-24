using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel
{
    [DataContract]
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
        [DataMember]
        public PaymentTypes paymentType;

        public enum CardReaderErrorMessages
        {
            None,
            NoAttachedDevice,
            CustomerCancelled,
            Declined,
            PartialAuthorization
        }
        [DataMember]
        public CardReaderErrorMessages errorMessage = CardReaderErrorMessages.None;

        [DataMember]
        public decimal paymentAmount;
        [DataMember]
        public IngenicoResponse cardResponse;

        [DataMember]
        public Guid ID = Guid.NewGuid();
    }
}
