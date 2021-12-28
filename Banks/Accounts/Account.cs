using System;
using System.Collections.Generic;
using Banks.Transactions;

namespace Banks.Accounts
{
    public abstract class Account
    {
        protected Account(Client client, int unverifiedLimit)
        {
            Id = Guid.NewGuid();
            Owner = client;
            UnverifiedLimit = unverifiedLimit;
            Transactions = new List<Transaction>();
        }

        public Client Owner { get; }
        public decimal Balance { get; private set; }
        public Guid Id { get; }
        public bool ClientSubscribed { get; set; }
        public decimal TempInterestSum { get; set; }
        public int UnverifiedLimit { get; set; }
        public List<Transaction> Transactions { get; }

        public abstract void Transfer(DateTime dateTime, decimal transactionSum, Account destinationAccount);
        public abstract void Refill(DateTime dateTime, decimal transactionSum);
        public abstract void Withdraw(DateTime dateTime, decimal transactionSum);

        public void UpdateBalance(decimal transactionSum)
        {
            Balance += transactionSum;
        }

        public abstract void PayPercents(DateTime dateTime);
    }
}