using System;
using System.Collections.Generic;
using Banks.Accounts;

namespace Banks
{
    public class Client : IObserver
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
        public List<INotification> Notifications { get; }
        public bool Verified => !string.IsNullOrEmpty(Address) && !string.IsNullOrEmpty(Passport);
        public static ClientBuilder Builder(string name) => new ClientBuilder().SetName(name);

        public void Update(INotification notification)
        {
            Notifications.Add(notification);
            notification.GetMessage();
        }
    }
}