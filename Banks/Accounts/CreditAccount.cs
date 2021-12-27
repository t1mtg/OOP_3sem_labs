using System;
using System.Linq;
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
            var transaction = new CreditWithdrawTransaction(dateTime, commissionSum, this);
            transaction.Commit();
            Transactions.Add(transaction);
        }

        private int AmountOfDays(DateTime dateTime)
        {
            TimeSpan interval = dateTime - LastDateTimeCommissionSubtracted;
            return interval.Days;
        }
    }
}