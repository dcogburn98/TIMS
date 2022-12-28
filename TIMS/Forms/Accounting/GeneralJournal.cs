using System;
using System.Windows.Forms;
using System.Collections.Generic;

using TIMSServerModel;
using TIMS.Server;

namespace TIMS.Forms
{
    public partial class GeneralJournal : Form
    {
        public string preEditValue = string.Empty;
        public List<Account> accounts = new List<Account>();
        public Account thisAccount;
        public bool addingRows = false;
        public bool rowAdded = false;

        public GeneralJournal(Account viewingAccount)
        {
            InitializeComponent();
            CancelButton = button1;
            Text = viewingAccount.Name + " Journal View";
            button3.Visible = false;
            accounts = Communication.RetrieveAccounts();
            thisAccount = viewingAccount;

            addingRows = true;
            thisAccount.Transactions.Sort((x, y) => DateTime.Compare(y.date, x.date));

            foreach (Transaction t in thisAccount.Transactions)
            {
                string transactionAccount =
                    t.creditAccount == viewingAccount.ID ?
                    accounts.Find(el => el.ID == t.debitAccount).Name :
                    accounts.Find(el => el.ID == t.creditAccount).Name;
                int row = dataGridView1.Rows.Add();
                foreach (Account acct in accounts)
                {
                    if (acct.Name == thisAccount.Name)
                        continue;
                    (dataGridView1.Rows[row].Cells[3] as DataGridViewComboBoxCell).Items.Add(acct.Name);
                }
                dataGridView1.Rows[row].Cells[0].Value = t.date.ToString("MM/dd/yyyy h:mm tt");
                dataGridView1.Rows[row].Cells[1].Value = t.referenceNumber;
                dataGridView1.Rows[row].Cells[2].Value = t.memo;
                dataGridView1.Rows[row].Cells[3].Value = transactionAccount;
                dataGridView1.Rows[row].Cells[4].Value =
                    t.creditAccount == viewingAccount.ID ?
                    0 : t.amount;
                dataGridView1.Rows[row].Cells[5].Value =
                    t.debitAccount == viewingAccount.ID ?
                    0 : t.amount;
                dataGridView1.Rows[row].Tag = t;
            }
            dataGridView1.Sort(new RowComparer());

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (thisAccount.Type == TIMSServerModel.Account.AccountTypes.Asset || thisAccount.Type == TIMSServerModel.Account.AccountTypes.Expense)
                {
                    if (row.Index == 0)
                    {
                        if (thisAccount.ID == (dataGridView1.Rows[row.Index].Tag as Transaction).debitAccount)
                            dataGridView1.Rows[row.Index].Cells[6].Value = (dataGridView1.Rows[row.Index].Tag as Transaction).amount.ToString();
                        else
                            dataGridView1.Rows[row.Index].Cells[6].Value = (0 - (dataGridView1.Rows[row.Index].Tag as Transaction).amount).ToString();
                    }
                    else
                    {
                        if (thisAccount.ID == (dataGridView1.Rows[row.Index].Tag as Transaction).debitAccount)
                            dataGridView1.Rows[row.Index].Cells[6].Value = (decimal.Parse(dataGridView1.Rows[row.Index - 1].Cells[6].Value.ToString()) + (dataGridView1.Rows[row.Index].Tag as Transaction).amount).ToString();
                        else
                            dataGridView1.Rows[row.Index].Cells[6].Value = (decimal.Parse(dataGridView1.Rows[row.Index - 1].Cells[6].Value.ToString()) - (dataGridView1.Rows[row.Index].Tag as Transaction).amount).ToString();
                    }
                }
                else
                {
                    if (row.Index == 0)
                    {
                        if (thisAccount.ID == (dataGridView1.Rows[row.Index].Tag as Transaction).creditAccount)
                            dataGridView1.Rows[row.Index].Cells[6].Value = (dataGridView1.Rows[row.Index].Tag as Transaction).amount.ToString();
                        else
                            dataGridView1.Rows[row.Index].Cells[6].Value = (0 - (dataGridView1.Rows[row.Index].Tag as Transaction).amount).ToString();
                    }
                    else
                    {
                        if (thisAccount.ID == (dataGridView1.Rows[row.Index].Tag as Transaction).creditAccount)
                            dataGridView1.Rows[row.Index].Cells[6].Value = (decimal.Parse(dataGridView1.Rows[row.Index - 1].Cells[6].Value.ToString()) + (dataGridView1.Rows[row.Index].Tag as Transaction).amount).ToString();
                        else
                            dataGridView1.Rows[row.Index].Cells[6].Value = (decimal.Parse(dataGridView1.Rows[row.Index - 1].Cells[6].Value.ToString()) - (dataGridView1.Rows[row.Index].Tag as Transaction).amount).ToString();
                    }
                }
            }
            addingRows = false;

            AddRow();
        }

        public GeneralJournal()
        {
            InitializeComponent();
            CancelButton = button1;
            accounts = Communication.RetrieveAccounts();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 6)
                e.Cancel = true;

            preEditValue = (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value ?? string.Empty).ToString();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                if (!DateTime.TryParse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out _))
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = preEditValue;
                }
            }
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //AddRow();
        }

        private void AddRow()
        {
            int newRowIndex = dataGridView1.Rows.Add();
            DataGridViewRow newRow = dataGridView1.Rows[newRowIndex];
            newRow.Cells[0].Value = DateTime.Now.ToString("MM/dd/yyyy h:mm tt");
            foreach (Account acct in accounts)
            {
                if (acct.Name == thisAccount.Name)
                    continue;
                (newRow.Cells[3] as DataGridViewComboBoxCell).Items.Add(acct.Name);
            }
            rowAdded = true;
            //return newRowIndex;
        }

        private bool CheckValidTransactionRow(int row)
        {
            if (DateTime.TryParse(dataGridView1.Rows[row].Cells[0].Value.ToString(), out DateTime _))
            {
                if (!int.TryParse(dataGridView1.Rows[row].Cells[1].Value?.ToString(), out int _))
                    dataGridView1.Rows[row].Cells[1].Value = 0;

                if (dataGridView1.Rows[row].Cells[3].Value != null)
                {
                    if (dataGridView1.Rows[row].Cells[4].Value != null && dataGridView1.Rows[row].Cells[5].Value != null)
                    {
                        if (!decimal.TryParse(dataGridView1.Rows[row].Cells[4].Value.ToString(), out decimal debitAmt))
                        {
                            MessageBox.Show("Invalid value in the debit column!");
                            return false;
                        }
                        else if (!decimal.TryParse(dataGridView1.Rows[row].Cells[5].Value.ToString(), out decimal creditAmt))
                        {
                            MessageBox.Show("Invalid value in the credit column!");
                            return false;
                        }
                        else
                        {
                            if (debitAmt > creditAmt)
                            {
                                dataGridView1.Rows[row].Cells[4].Value = debitAmt - creditAmt;
                                dataGridView1.Rows[row].Cells[5].Value = null;
                            }
                            else if (creditAmt > debitAmt)
                            {
                                dataGridView1.Rows[row].Cells[5].Value = creditAmt - debitAmt;
                                dataGridView1.Rows[row].Cells[4].Value = null;
                            }
                            else
                            {
                                dataGridView1.Rows[row].Cells[5].Value = null;
                                dataGridView1.Rows[row].Cells[4].Value = null;
                            }
                        }
                    }

                    if (dataGridView1.Rows[row].Cells[4].Value == null && dataGridView1.Rows[row].Cells[5].Value == null)
                    {
                        MessageBox.Show("Please enter a value for this transaction!");
                        return false;
                    }

                    if (dataGridView1.Rows[row].Cells[4].Value == null)
                    {
                        if (decimal.TryParse(dataGridView1.Rows[row].Cells[5].Value.ToString(), out decimal _))
                        {
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("Invalid amount!");
                            dataGridView1.Rows[row].Cells[5].Value = null;
                            return false;
                        }
                    }
                    else if (dataGridView1.Rows[row].Cells[5].Value == null)
                    {
                        if (decimal.TryParse(dataGridView1.Rows[row].Cells[4].Value.ToString(), out decimal _))
                        {
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("Invalid amount!");
                            dataGridView1.Rows[row].Cells[4].Value = null;
                            return false;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select an account to transact from!");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Invalid date for transaction!");
                return false;
            }
            return false;
        }

        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex < 0 || addingRows)
            //    return;

            //if (e.ColumnIndex == 5)
            //{
            //    if (CheckValidTransactionRow(e.RowIndex))
            //        button3.Visible = true;
            //    else
            //        button3.Visible = false;
            //}
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1)
                return;
            if (dataGridView1.SelectedRows[0].Index == dataGridView1.Rows.Count - 1)
            {
                int debitAccount =
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[4].Value == null ?
                    accounts.Find(el => el.Name == dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[3].Value.ToString()).ID :
                    thisAccount.ID;
                int creditAccount =
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[5].Value == null ?
                    accounts.Find(el => el.Name == dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[3].Value.ToString()).ID :
                    thisAccount.ID;
                decimal amount =
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[4].Value == null ?
                    decimal.Parse(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[5].Value.ToString()) :
                    decimal.Parse(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[4].Value.ToString());
                Transaction t = new Transaction(debitAccount, creditAccount, amount);
                t.referenceNumber = int.Parse(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1].Value.ToString());
                t.memo = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[2].Value.ToString();
                Communication.SaveTransaction(t);
                AddRow();
                button3.Visible = false;
            }
            else
            {

            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || addingRows)
                return;

            if (e.ColumnIndex == 6)
            {
                if (dataGridView1.Rows[e.RowIndex].Tag == null)
                {
                    if (CheckValidTransactionRow(e.RowIndex))
                    {
                        int debitAccount =
                            dataGridView1.Rows[e.RowIndex].Cells[4].Value == null ?
                            accounts.Find(el => el.Name == dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString()).ID :
                            thisAccount.ID;
                        int creditAccount =
                            dataGridView1.Rows[e.RowIndex].Cells[5].Value == null ?
                            accounts.Find(el => el.Name == dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString()).ID :
                            thisAccount.ID;
                        decimal amount =
                            dataGridView1.Rows[e.RowIndex].Cells[4].Value == null ?
                            decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString()) :
                            decimal.Parse(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());

                        Transaction t = new Transaction(debitAccount, creditAccount, amount);
                        t.referenceNumber = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                        t.memo = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                        t.date = DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                        Communication.SaveTransaction(t);
                        dataGridView1.Rows[e.RowIndex].Tag = t;
                        thisAccount.Transactions.Add(t);
                        thisAccount.Transactions.Sort((x, y) => DateTime.Compare(y.date, x.date));

                        BeginInvoke(new MethodInvoker(SortRows));

                        BeginInvoke(new MethodInvoker(AddRow));
                        button3.Visible = false;
                    }
                }
                else
                {

                }
            }
        }

        private void SortRows()
        {
            dataGridView1.Sort(new RowComparer());
        }
    }

    class RowComparer : System.Collections.IComparer
    {
        public int Compare(object x, object y)
        {
            DataGridViewRow row1 = (DataGridViewRow)x;
            DataGridViewRow row2 = (DataGridViewRow)y;
            return DateTime.Compare(DateTime.Parse(row1.Cells[0].Value.ToString()), DateTime.Parse(row2.Cells[0].Value.ToString()));
        }
    }
}
