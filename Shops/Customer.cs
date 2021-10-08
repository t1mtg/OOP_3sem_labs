using System;

namespace Shops
{
    public class Customer
    {
        public Customer(int balance)
        {
            Id = Guid.NewGuid();
            Balance = balance;
        }

        public Guid Id { get; set; }
        public int Balance { get; set; }
    }
}