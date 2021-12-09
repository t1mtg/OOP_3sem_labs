using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Accounts;

namespace Banks
{
    public class Bank
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
        }

        public List<Account> Accounts { get; }
        public Guid Id { get; }
        public string Name { get; }
        public int CreditBelowZeroLimit { get; set; }
        public decimal CreditCommission { get; set; }
        public int UnverifiedLimit { get; set; }
        public decimal DebitPercent { get; set; }

        private InterestRates InterestRates { get; set; }

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
            var depositAccount = new DepositAccount(client, UnverifiedLimit, depositExpirationDate, depositSum, interestRates);
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

        public void ChangeDebitInterestRate(decimal newInterestRate)
        {
            DebitPercent = newInterestRate;
            Accounts.Where(account => account.ClientSubscribed && account is DebitAccount)
                .Select(account => account.Owner)
                .ToList()
                .ForEach(_ => Client.GetUpdate($"Debit interest rate changed. New value: {newInterestRate}"));
        }

        public void ChangeCreditCommission(decimal newCommission)
        {
            CreditCommission = newCommission;
            Accounts.Where(account => account.ClientSubscribed && account is CreditAccount)
                .Select(account => account.Owner)
                .ToList()
                .ForEach(_ => Client.GetUpdate($"Credit commission changed. New value: {newCommission}"));
        }

        public void ChangeUnverifiedLimit(int newLimit)
        {
            Accounts.Where(account => account.ClientSubscribed && !account.Owner.Verified)
                .Select(account => account.Owner)
                .ToList()
                .ForEach(_ => Client.GetUpdate($"Unverified limit changed. New value: {newLimit}"));
        }

        public void ChangeCreditBelowZeroLimit(int newLimit)
        {
            CreditBelowZeroLimit = newLimit;
            Accounts.Where(account => account.ClientSubscribed && account is CreditAccount)
                .Select(account => account.Owner)
                .ToList()
                .ForEach(_ => Client.GetUpdate($"Credit below zero changed. New value: {newLimit}"));
        }

        public void ChangeInterestRates(InterestRates newInterestRates)
        {
            InterestRates = newInterestRates;
            Accounts.Where(account => account.ClientSubscribed && account is CreditAccount)
                .Select(account => account.Owner)
                .ToList()
                .ForEach(_ => Client.GetUpdate($"Interest rates changed."));
        }

        public void PayInterestsAndCommissions(DateTime dateTime)
        {
            foreach (Account account in Accounts)
            {
                account.PayPercents(dateTime);
            }
        }
    }
}