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

        public decimal BelowZeroLimit { get; }
        public DateTime LastDateTimeCommissionSubtracted { get; set; } = default;
        public decimal TempCommissionSum { get; set; }
        public decimal CreditCommission { get; set; }

        public void CalculateCommissions(CreditAccount creditAccount, DateTime dateTime)
        {
            if (creditAccount.LastDateTimeCommissionSubtracted == default)
            {
                creditAccount.LastDateTimeCommissionSubtracted = creditAccount.Transactions.First().DateTime;
            }

            for (int i = 0; i < AmountOfDays(dateTime); i++)
            {
                DateTime curDate = LastDateTimeCommissionSubtracted.AddDays(i);
                Transaction lastTransaction =
                    creditAccount.Transactions.LastOrDefault(transaction => transaction.DateTime <= curDate);
                if (lastTransaction == null) continue;
                if (lastTransaction.BalanceAfterTransaction < 0)
                {
                    creditAccount.TempCommissionSum += CreditCommission;
                }
            }

            creditAccount.LastDateTimeCommissionSubtracted = dateTime;
        }

        public void SubtractCommission(decimal commissionSum, DateTime dateTime)
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