using System;
using System.Linq;
using Banks.Exceptions;
using Banks.Transactions;

namespace Banks.Accounts
{
    public class CreditAccount : Account
    {
        public CreditAccount(Client accountOwner, int unverifiedLimit, int belowZeroLimit, decimal commission)
            : base(accountOwner, unverifiedLimit)
        {
            BelowZeroLimit = belowZeroLimit;
            CreditCommission = commission;
        }

        public decimal BelowZeroLimit { get; set; }
        public DateTime LastDateTimeCommissionSubtracted { get; set; } = default;
        public decimal TempCommissionSum { get; set; }
        public decimal CreditCommission { get; set; }

        public override void Transfer(DateTime dateTime, decimal transactionSum, Account destinationAccount)
        {
            if (!Owner.Verified && transactionSum > UnverifiedLimit)
            {
                throw new AccountUnverifiedException();
            }

            if (Balance - transactionSum + BelowZeroLimit < 0)
            {
                throw new CreditLimitExceededException();
            }

            var transferTransaction = new TransferTransaction(dateTime, transactionSum, this, destinationAccount);
            transferTransaction.Commit();
        }

        public override void Refill(DateTime dateTime, decimal transactionSum)
        {
            if (transactionSum < 0)
            {
                throw new TransactionSumLessThanZeroException();
            }

            var refillTransaction = new RefillTransaction(dateTime, transactionSum, this);
            refillTransaction.Commit();
        }

        public override void Withdraw(DateTime dateTime, decimal transactionSum)
        {
            if (Owner.Verified && transactionSum > UnverifiedLimit)
            {
                throw new AccountUnverifiedException();
            }

            if (transactionSum - Balance + BelowZeroLimit < 0)
            {
                throw new CreditLimitExceededException();
            }

            var withdrawTransaction = new WithdrawTransaction(dateTime, transactionSum, this);
            withdrawTransaction.Commit();
        }

        public override void PayPercents(DateTime dateTime)
        {
            CalculateCommissions(dateTime);
            SubtractCommission(TempCommissionSum, dateTime);
            TempCommissionSum = decimal.Zero;
        }

        private void CalculateCommissions(DateTime dateTime)
        {
            if (LastDateTimeCommissionSubtracted == default)
            {
                LastDateTimeCommissionSubtracted = Transactions.First().DateTime;
            }

            for (int i = 0; i < AmountOfDays(dateTime); i++)
            {
                DateTime curDate = LastDateTimeCommissionSubtracted.AddDays(i);
                Transaction lastTransaction =
                    Transactions.LastOrDefault(transaction => transaction.DateTime <= curDate);
                if (lastTransaction == null) continue;
                if (lastTransaction.BalanceAfterTransaction < 0)
                {
                    TempCommissionSum += CreditCommission;
                }
            }

            LastDateTimeCommissionSubtracted = dateTime;
        }

        private void SubtractCommission(decimal commissionSum, DateTime dateTime)
        {
            Withdraw(dateTime, commissionSum);
        }

        private int AmountOfDays(DateTime dateTime)
        {
            TimeSpan interval = dateTime - LastDateTimeCommissionSubtracted;
            return interval.Days;
        }
    }
}