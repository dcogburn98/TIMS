using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel
{
    public class Transaction
    {
        public int ID;
        public DateTime date;
        public int transactionID;

        public int referenceNumber;
        public string memo;
        public decimal amount;
        public int creditAccount;
        public int debitAccount;
    }
}
