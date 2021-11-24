﻿using System;
using Banks.Accounts;
using Banks.Exceptions;

namespace Banks.Transactions
{
    public class DebitWithdrawTransaction : Transaction
    {
        public DebitWithdrawTransaction(DateTime dateTime, decimal transactionSum, Account sourceAccount)
            : base(dateTime, transactionSum, sourceAccount)
        {
        }

        public override void Commit()
        {
            if (OperationCanceled)
            {
                throw new OperationIsAlreadyCancelledException();
            }

            if (TransactionSum - SourceAccount.Balance > 0)
            {
                throw new InsufficientBalanceException();
            }

            if (!SourceAccount.Owner.Verified && TransactionSum > SourceAccount.UnverifiedLimit)
            {
                throw new AccountUnverifiedException();
            }

            SourceAccount.Balance -= TransactionSum;
            BalanceAfterTransaction = SourceAccount.Balance;
            SourceAccount.Transactions.Add(this);
        }

        public override void Rollback()
        {
            if (OperationCanceled)
            {
                throw new OperationIsAlreadyCancelledException();
            }

            SourceAccount.Balance += TransactionSum;
            BalanceAfterTransaction = SourceAccount.Balance;
            SourceAccount.Transactions.Add(this);
        }
    }
}