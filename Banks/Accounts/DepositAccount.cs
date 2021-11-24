using System;
using System.Linq;
using Banks.Transactions;

namespace Banks.Accounts
{
    public class DepositAccount : Account
    {
        public DepositAccount(Client client, int unverifiedLimit, DateTime depositExpirationDate, int depositSum)
            : base(client, unverifiedLimit)
        {
            DepositExpirationDate = depositExpirationDate;
            DepositSum = depositSum;
        }

        public decimal DepositSum { get; set; }
        public DateTime LastDateTimeInterestAdded { get; set; } = default;
        public DateTime DepositExpirationDate { get; }

        public void CalculatePercents(DateTime dateTime, decimal percents)
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

        public void AddInterest(decimal interestSum, DateTime dateTime)
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
