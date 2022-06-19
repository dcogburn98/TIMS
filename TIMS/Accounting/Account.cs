using System.Collections.Generic;
using System.Xml.Linq;

namespace TIMS.Accounting
{
    class Account
    {
        public static List<Account> Accounts = new List<Account>();

        public enum AccountTypes
        {
            Asset,
            Equity,
            Liability,
            Expense,
            Income
        }

        public AccountTypes Type;
        public string Name;
        public string Description;
        public int ID;
        public double Balance;

        public Account(string Name, string Description, int ID, double Balance)
        {
            this.Name = Name;
            this.Description = Description;
            this.ID = ID;
            this.Balance = Balance;
        }

        public Account()
        {
            //Allows database handler to create a blank account to edit
        }

        public void Debit(double amount)
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

        public void Credit(double amount)
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

        //public static void LoadAccounts()
        //{
        //    foreach (XElement acc in DatabaseHandler.accountsDB.Elements("Account"))
        //    {
        //        Account newAccount = new Account();
        //        newAccount.Name = acc.Value;
        //        newAccount.Description = acc.Element("Description").Value;
        //        newAccount.ID = int.Parse(acc.Element("ID").Value);
        //        string type = acc.Element("Type").Value;
        //        if (type == "Asset")
        //            newAccount.Type = AccountTypes.Asset;
        //        else if (type == "Equity")
        //            newAccount.Type = AccountTypes.Equity;
        //        else if (type == "Liability")
        //            newAccount.Type = AccountTypes.Liability;
        //        else if (type == "Income")
        //            newAccount.Type = AccountTypes.Income;
        //        else if (type == "Expense")
        //            newAccount.Type = AccountTypes.Expense;

        //        Accounts.Add(newAccount);
        //    }
        //}
    }
}
