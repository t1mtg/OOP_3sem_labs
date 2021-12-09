using System;
using Banks.Accounts;
using Banks.Exceptions;

namespace Banks.Transactions
{
    public class CreditWithdrawTransaction : Transaction
    {
        public CreditWithdrawTransaction(DateTime dateTime, decimal transactionSum, Account sourceAccount)
            : base(dateTime, transactionSum, sourceAccount)
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
            BalanceAfterTransaction = SourceAccount.Balance;
            SourceAccount.Transactions.Add(this);
        }
    }
}