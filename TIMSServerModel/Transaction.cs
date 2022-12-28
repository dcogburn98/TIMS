using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TIMSServerModel
{
    [DataContract]
    public class Transaction
    {
        [DataMember]
        public int ID;
        [DataMember]
        public DateTime date;
        [DataMember]
        public int transactionID;

        [DataMember]
        public int referenceNumber;
        [DataMember]
        public string memo;
        [DataMember]
        public decimal amount;
        [DataMember]
        public int creditAccount;
        [DataMember]
        public int debitAccount;
        [DataMember]
        public bool _void;

        public Transaction(int debitAccount, int creditAccount, decimal amount)
        {
            this.creditAccount = creditAccount;
            this.debitAccount = debitAccount;
            this.amount = amount;
            date = DateTime.Now;
        }
    }
}
