using System;
using Banks.Accounts;
using Banks.Exceptions;

namespace Banks.Transactions
{
    public class DebitTransferTransaction : Transaction
    {
        public DebitTransferTransaction(DateTime dateTime, decimal transactionSum, Account sourceAccount, Account destinationAccount)
            : base(dateTime, transactionSum, sourceAccount, destinationAccount)
        {
        }

        public override void Commit()
        {
            if (OperationCanceled)
            {
                throw new OperationIsAlreadyCancelledException();
            }

            if (!SourceAccount.Owner.Verified && TransactionSum > SourceAccount.UnverifiedLimit)
            {
                throw new AccountUnverifiedException();
            }

            SourceAccount.Balance -= TransactionSum;
            DestinationAccount.Balance += TransactionSum;
            BalanceAfterTransaction = SourceAccount.Balance;
            SourceAccount.Transactions.Add(this);
        }

        public override void Rollback()
        {
            if (OperationCanceled)
            {
                throw new OperationIsAlreadyCancelledException();
            }

            SourceAccount.Balance -= TransactionSum;
            BalanceAfterTransaction = SourceAccount.Balance;
            DestinationAccount.Balance += TransactionSum;
            SourceAccount.Transactions.Add(this);
        }
    }
}