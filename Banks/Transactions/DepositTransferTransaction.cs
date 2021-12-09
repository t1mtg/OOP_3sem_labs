using System;
using Banks.Accounts;
using Banks.Exceptions;

namespace Banks.Transactions
{
    public class DepositTransferTransaction : Transaction
    {
        public DepositTransferTransaction(DateTime dateTime, decimal transactionSum, Account sourceAccount, Account destinationAccount)
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

            if (SourceAccount is DepositAccount depositAccount && DateTime < depositAccount.DepositExpirationDate)
            {
                throw new DepositIsNotExpiredException();
            }

            SourceAccount.UpdateBalance(-TransactionSum);
            DestinationAccount.UpdateBalance(TransactionSum);
            BalanceAfterTransaction = SourceAccount.Balance;
            SourceAccount.Transactions.Add(this);
        }

        public override void Rollback()
        {
            if (OperationCanceled)
            {
                throw new OperationIsAlreadyCancelledException();
            }

            SourceAccount.UpdateBalance(-TransactionSum);
            BalanceAfterTransaction = SourceAccount.Balance;
            DestinationAccount.UpdateBalance(TransactionSum);
            SourceAccount.Transactions.Add(this);
        }
    }
}