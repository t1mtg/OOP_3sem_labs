using System;
using Shops.Tools;

namespace Shops
{
    public class Customer
    {
        private int _balance;

        public Customer(int balance)
        {
            Id = Guid.NewGuid();
            _balance = balance;
        }

        public Guid Id { get; }

        public int Balance
        {
            get => _balance;

            set
            {
                if (value < 0)
                {
                    throw new NegativeBalanceException();
                }

                _balance = value;
            }
        }
    }
}