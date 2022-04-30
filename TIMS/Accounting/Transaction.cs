using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMS.Accounting
{
    class Transaction
    {
        public DateTime TransactionDateAndTime;
        public string Memo;
        public int Order;
        public List<object> SubTransactions;

        public void AddSubTransaction(Account DebitAccount, double DebitAmount, Account CreditAccount, double CreditAmount)
        {
            SubTransactions.Add(DebitAccount);
            SubTransactions.Add(DebitAmount);
            SubTransactions.Add(CreditAccount);
            SubTransactions.Add(CreditAmount);

            DebitAccount.Debit(DebitAmount);
            CreditAccount.Credit(CreditAmount);
        }
    }
}
