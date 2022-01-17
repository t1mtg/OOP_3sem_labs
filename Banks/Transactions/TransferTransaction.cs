﻿using System;
using Banks.Accounts;
using Banks.Exceptions;

namespace Banks.Transactions
{
    public class TransferTransaction : Transaction
    {
        public TransferTransaction(DateTime dateTime, decimal transactionSum, Account sourceAccount, Account destinationAccount)
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

            if (SourceAccount is CreditAccount creditAccount &&
                SourceAccount.Balance - TransactionSum + creditAccount.BelowZeroLimit < 0)
            {
                throw new CreditLimitExceededException();
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

            SourceAccount.UpdateBalance(TransactionSum);
            DestinationAccount.UpdateBalance(-TransactionSum);
            BalanceAfterTransaction = SourceAccount.Balance;
            SourceAccount.Transactions.Add(this);
        }
    }
}