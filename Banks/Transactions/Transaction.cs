using System;
using Banks.Accounts;

namespace Banks.Transactions
{
    public abstract class Transaction
    {
        protected Transaction(DateTime dateTime, decimal transactionSum, Account sourceAccount, Account destinationAccount = null)
        {
            Id = Guid.NewGuid();
            DateTime = dateTime;
            SourceAccount = sourceAccount;
            DestinationAccount = destinationAccount;
            TransactionSum = transactionSum;
        }

        public decimal BalanceAfterTransaction { get; set; }
        public Guid Id { get; }
        public decimal TransactionSum { get; set; }
        public DateTime DateTime { get; }
        public Account SourceAccount { get; }
        public Account DestinationAccount { get; set; }

        protected bool OperationCanceled { get; set; } = false;

        public abstract void Commit();
        public abstract void Rollback();
    }
}