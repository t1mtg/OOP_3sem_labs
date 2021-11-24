using System;
using System.Collections.Generic;
using Banks.Accounts;

namespace Banks
{
    public class Client
    {
        public Client()
        {
            Id = Guid.NewGuid();
            Accounts = new List<Account>();
        }

        public Guid Id { get; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Passport { get; set; }
        public List<Account> Accounts { get; }
        public bool Verified => !string.IsNullOrEmpty(Address) && !string.IsNullOrEmpty(Passport);
        public static ClientBuilder Builder(string name) => new ClientBuilder().SetName(name);
        public static void GetUpdate(string message)
        {
            Console.WriteLine(message);
        }
    }
}