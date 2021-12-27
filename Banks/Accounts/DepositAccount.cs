using System;
using System.Linq;
using Banks.Transactions;

namespace Banks.Accounts
{
    public class DepositAccount : Account
    {
        public DepositAccount(Client client, int unverifiedLimit, DateTime depositExpirationDate, int depositSum, InterestRates interestRates)
            : base(client, unverifiedLimit)
        {
            DepositExpirationDate = depositExpirationDate;
            DepositSum = depositSum;
            InterestRates = interestRates;
        }

        public decimal DepositSum { get; }
        public InterestRates InterestRates { get; set; }
        public DateTime LastDateTimeInterestAdded { get; set; } = default;
        public DateTime DepositExpirationDate { get; }

        public override void PayPercents(DateTime dateTime)
        {
            CalculatePercents(dateTime, CalculateDepositPercent(DepositSum));
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

        private decimal CalculateDepositPercent(decimal depositSum)
        {
            foreach ((int limit, decimal interestRateValue) in InterestRates.InterestRate.Where(tuple => depositSum < tuple.Limit))
            {
                return interestRateValue;
            }

            return InterestRates.InterestRate.Last().InterestRateValue;
        }

        private int AmountOfDays(DateTime dateTime)
        {
            TimeSpan interval = dateTime - LastDateTimeInterestAdded;
            return interval.Days;
        }
    }
}