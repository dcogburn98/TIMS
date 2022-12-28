using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel
{
    public class Account
    {
        public enum AccountTypes
        {
            Asset,
            Equity,
            Liability,
            Expense,
            Income
        }

        public List<Transaction> Transactions;
        public AccountTypes Type;
        public string Name;
        public string Description;
        public int ID;
        public decimal Balance;

        public Account(string Name, string Description, int ID, decimal Balance)
        {
            this.Name = Name;
            this.Description = Description;
            this.ID = ID;
            this.Balance = Balance;
            this.Transactions = new List<Transaction>();
        }

        public Account()
        {
            //Allows database handler to create a blank account to edit
            this.Transactions = new List<Transaction>();
        }

        public void Debit(decimal amount)
        {
            if (Type == AccountTypes.Asset)
                Balance += amount;
            if (Type == AccountTypes.Expense)
                Balance += amount;
            if (Type == AccountTypes.Income)
                Balance -= amount;
            if (Type == AccountTypes.Liability)
                Balance -= amount;
            if (Type == AccountTypes.Equity)
                Balance -= amount;
        }

        public void Credit(decimal amount)
        {
            if (Type == AccountTypes.Asset)
                Balance -= amount;
            if (Type == AccountTypes.Expense)
                Balance -= amount;
            if (Type == AccountTypes.Income)
                Balance += amount;
            if (Type == AccountTypes.Liability)
                Balance += amount;
            if (Type == AccountTypes.Equity)
                Balance += amount;
        }
    }
}
