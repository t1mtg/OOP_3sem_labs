using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Accounts;
using Banks.Notifications;

namespace Banks
{
    public class Bank : IObservable
    {
        public Bank(string name, decimal debitPercent, int unverifiedLimit, int creditBelowZeroLimit, decimal creditCommission, InterestRates interestRates)
        {
            Accounts = new List<Account>();
            Id = Guid.NewGuid();
            Name = name;
            DebitPercent = debitPercent;
            UnverifiedLimit = unverifiedLimit;
            CreditBelowZeroLimit = creditBelowZeroLimit;
            CreditCommission = creditCommission;
            InterestRates = interestRates;
            Observers = new List<IObserver>();
        }

        public List<Account> Accounts { get; }
        public Guid Id { get; }
        public string Name { get; }
        public int CreditBelowZeroLimit { get; set; }
        public decimal CreditCommission { get; set; }
        public int UnverifiedLimit { get; set; }
        public decimal DebitPercent { get; set; }

        private InterestRates InterestRates { get; set; }
        private List<IObserver> Observers { get; set; }

        public DebitAccount AddNewDebitAccount(Client client)
        {
            var debitAccount = new DebitAccount(client, UnverifiedLimit);
            debitAccount.DebitPercent = DebitPercent;
            Accounts.Add(debitAccount);
            client.Accounts.Add(debitAccount);
            return debitAccount;
        }

        public DepositAccount AddNewDepositAccount(Client client, DateTime depositExpirationDate, int depositSum, InterestRates interestRates)
        {
            var depositAccount =
                new DepositAccount(client, UnverifiedLimit, depositExpirationDate, depositSum, interestRates);
            depositAccount.UpdateBalance(depositSum);
            Accounts.Add(depositAccount);
            client.Accounts.Add(depositAccount);
            return depositAccount;
        }

        public CreditAccount AddNewCreditAccount(Client client)
        {
            var creditAccount = new CreditAccount(client, UnverifiedLimit, CreditBelowZeroLimit, CreditCommission);
            Accounts.Add(creditAccount);
            client.Accounts.Add(creditAccount);
            return creditAccount;
        }

        public void ChangeCreditCommission(decimal newCommission)
        {
            CreditCommission = newCommission;
            foreach (CreditAccount creditAccount in Accounts.OfType<CreditAccount>())
            {
                creditAccount.CreditCommission = newCommission;
            }

            NotifySubscribers(new CreditCommissionNotification(newCommission));
        }

        public void ChangeUnverifiedLimit(int newLimit)
        {
            UnverifiedLimit = newLimit;
            foreach (Account account in Accounts)
            {
                account.UnverifiedLimit = newLimit;
            }

            NotifySubscribers(new UnverifiedLimitNotification(newLimit));
        }

        public void ChangeCreditBelowZeroLimit(int newLimit)
        {
            CreditBelowZeroLimit = newLimit;
            foreach (CreditAccount creditAccount in Accounts.OfType<CreditAccount>())
            {
                creditAccount.BelowZeroLimit = newLimit;
            }

            NotifySubscribers(new CreditBelowZeroNotification(newLimit));
        }

        public void ChangeInterestRates(InterestRates newInterestRates)
        {
            InterestRates = newInterestRates;
            foreach (DepositAccount depositAccount in Accounts.OfType<DepositAccount>())
            {
                depositAccount.InterestRates = newInterestRates;
            }

            NotifySubscribers(new InterestRateNotification(newInterestRates));
        }

        public void PayInterestsAndCommissions(DateTime dateTime)
        {
            foreach (Account account in Accounts)
            {
                account.PayPercents(dateTime);
            }
        }

        public void AddObserver(IObserver observer)
        {
            Observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            Observers.Remove(observer);
        }

        public void NotifySubscribers(INotification notification)
        {
            foreach (IObserver observer in Observers)
            {
                observer.Update(notification);
            }
        }
    }
}