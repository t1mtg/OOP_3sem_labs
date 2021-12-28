using System;
using System.Linq;
using Banks.Exceptions;
using Banks.Transactions;

namespace Banks.Accounts
{
    public class DebitAccount : Account
    {
        public DebitAccount(Client client, int transferLimitForUnverified)
            : base(client, transferLimitForUnverified)
        {
        }

        public decimal DebitPercent { get; set; }
        private DateTime LastDateTimeInterestAdded { get; set; } = default;
        public override void Transfer(DateTime dateTime, decimal transactionSum, Account destinationAccount)
        {
            if (!Owner.Verified && transactionSum > UnverifiedLimit)
            {
                throw new AccountUnverifiedException();
            }

            if (Balance - transactionSum < 0)
            {
                throw new InsufficientBalanceException();
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

            if (transactionSum - Balance > 0)
            {
                throw new InsufficientBalanceException();
            }

            var withdrawTransaction = new WithdrawTransaction(dateTime, transactionSum, this);
            withdrawTransaction.Commit();
        }

        public override void PayPercents(DateTime dateTime)
        {
            CalculatePercents(dateTime, DebitPercent);
            AddInterest(TempInterestSum, dateTime);
            TempInterestSum = decimal.Zero;
        }

        private void CalculatePercents(DateTime dateTime, decimal percents)
        {
            if (LastDateTimeInterestAdded == default)
            {
                LastDateTimeInterestAdded = Transactions.First().DateTime;
            }

            for (int i = 0; i < AmountOfDays(dateTime); i++)
            {
                DateTime curDate = LastDateTimeInterestAdded.AddDays(i);
                Transaction lastTransaction =
                    Transactions.LastOrDefault(transaction => transaction.DateTime.Date <= curDate);
                if (lastTransaction == null) continue;
                decimal curBalance = lastTransaction.BalanceAfterTransaction;
                TempInterestSum += curBalance * (percents / 365) / 100;
            }

            LastDateTimeInterestAdded = dateTime;
        }

        private void AddInterest(decimal interestSum, DateTime dateTime)
        {
            var transaction = new RefillTransaction(dateTime, interestSum, this);
            transaction.Commit();
            Transactions.Add(transaction);
        }

        private int AmountOfDays(DateTime dateTime)
        {
            TimeSpan interval = dateTime - LastDateTimeInterestAdded;
            return interval.Days;
        }
    }
}