using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Banks.Accounts;
using Banks.Transactions;

namespace Banks
{
    internal static class Program
    {
        private static void Main()
        {
            var interestRates = new List<InterestRate>()
            {
                new InterestRate(10000, 4),
                new InterestRate(20000, 5),
                new InterestRate(30000, 6),
            };
            Client client = Client.Builder("Timofey Gurman").SetAddress("Вяземский пер. 5/7").SetPassport("1631 754754")
                .GetClient();
            Client brother = Client.Builder("Ivan Gurman").SetAddress("Вяземский пер. 5/7").SetPassport("1631 228228")
                .GetClient();
            var centralBank = new CentralBank();
            Bank bank = centralBank.RegisterBank("Gurman Bank", 1, 10000, 1488, 22, interestRates);
            bank.AddNewDebitAccount(brother);
            Console.WriteLine($"Welcome to Gurman Bank, {client.Name}");
            while (true)
            {
                Console.WriteLine("Choose an option: \n" +
                                  "1. Check your accounts and balance \n" +
                                  "2. Create new Debit Account \n" +
                                  "3. Create new Credit Account \n" +
                                  "4. Create new Deposit Account \n" +
                                  "5. Choose the account to Refill/Withdraw/Transfer \n" +
                                  "6. Exit \n");
                switch (Console.ReadLine())
                {
                    case "1":
                        foreach (Account account in bank.Accounts.Where(account => account.Owner.Equals(client)))
                        {
                            string accountType = account switch
                            {
                                DebitAccount => "Debit",
                                CreditAccount => "Credit",
                                DepositAccount => "Deposit",
                                _ => null
                            };

                            Console.WriteLine("======================");
                            Console.WriteLine($"Account type: {accountType}");
                            Console.WriteLine($"Balance: {account.Balance.ToString(CultureInfo.InvariantCulture)}");
                        }

                        Console.WriteLine("======================");
                        Console.WriteLine("Choose an option: \n" +
                                          "1. Return to the main menu \n" +
                                          "2. Exit \n");
                        switch (Console.ReadLine())
                        {
                            case "1":
                                break;
                            case "2":
                                return;
                        }

                        break;
                    case "2":
                        bank.AddNewDebitAccount(client);
                        Console.WriteLine("Account successfully created. \n");
                        Console.WriteLine("Choose an option: \n" +
                                          "1. Return to the main menu \n" +
                                          "2. Exit \n");
                        switch (Console.ReadLine())
                        {
                            case "1":
                                break;
                            case "2":
                                return;
                        }

                        break;
                    case "3":
                        bank.AddNewCreditAccount(client);
                        Console.WriteLine("Account successfully created. \n");
                        Console.WriteLine("Choose an option: \n" +
                                          "1. Return to the main menu \n" +
                                          "2. Exit \n");
                        switch (Console.ReadLine())
                        {
                            case "1":
                                break;
                            case "2":
                                return;
                        }

                        break;
                    case "4":
                        DateTime depositExpirationTime = DateTime.Now;
                        Console.WriteLine("Please choose the term of the deposit: \n" +
                                          "1. 6 months \n" +
                                          "2. 1 year \n" +
                                          "3. 3 years \n" +
                                          "4. 5 years \n");
                        depositExpirationTime = Console.ReadLine() switch
                        {
                            "1" => depositExpirationTime.AddMonths(6),
                            "2" => depositExpirationTime.AddYears(1),
                            "3" => depositExpirationTime.AddYears(3),
                            "4" => depositExpirationTime.AddYears(5),
                            _ => depositExpirationTime
                        };
                        if (depositExpirationTime - DateTime.Now < TimeSpan.FromDays(170))
                        {
                            Console.WriteLine("Please choose the correct term.");
                            break;
                        }

                        Console.WriteLine("Please enter Deposit Sum");
                        int depositSum = int.Parse(Console.ReadLine());
                        bank.AddNewDepositAccount(client, depositExpirationTime, depositSum);
                        Console.WriteLine("Account successfully created. \n" +
                                          $"Expiration date: {depositExpirationTime.Date} \n" +
                                          $"Deposit Sum = {depositSum}");
                        Console.WriteLine("Choose an option: \n" +
                                          "1. Return to the main menu \n" +
                                          "2. Exit \n");
                        switch (Console.ReadLine())
                        {
                            case "1":
                                break;
                            case "2":
                                return;
                        }

                        break;
                    case "5":
                        int cnt = 1;
                        string accType = null;
                        var clientAccounts =
                            bank.Accounts.Where(account => account.Owner.Equals(client)).ToList();
                        foreach (Account account in clientAccounts)
                        {
                            accType = account switch
                            {
                                DebitAccount => "Debit",
                                CreditAccount => "Credit",
                                DepositAccount => "Deposit",
                                _ => null
                            };

                            Console.WriteLine("======================");
                            Console.WriteLine($"{cnt}. Account type: {accType}");
                            Console.WriteLine($"Balance: {account.Balance.ToString(CultureInfo.InvariantCulture)}");
                            cnt++;
                        }

                        if (cnt == 1)
                        {
                            Console.WriteLine("Please open a new account first.");
                            break;
                        }

                        Account chosenAccount = clientAccounts[int.Parse(Console.ReadLine()) - 1];
                        Console.WriteLine("Please choose the operation: \n" +
                                          "1. Refill \n" +
                                          "2. Withdraw \n" +
                                          "3. Transfer \n");
                        switch (Console.ReadLine())
                        {
                            case "1":
                                Console.WriteLine("Please enter the amount of money to refill");
                                int transactionSum = int.Parse(Console.ReadLine());
                                var refillTransaction =
                                    new RefillTransaction(DateTime.Now, transactionSum, chosenAccount);
                                refillTransaction.Commit();
                                Console.WriteLine(
                                    $"Account successfully refilled. Current balance: {chosenAccount.Balance}\n");
                                Console.WriteLine("Choose an option: \n" +
                                                  "1. Return to the main menu \n" +
                                                  "2. Exit \n");
                                switch (Console.ReadLine())
                                {
                                    case "1":
                                        break;
                                    case "2":
                                        return;
                                }

                                break;
                            case "2":
                                Console.WriteLine("Please enter the amount of money to withdraw:");
                                int trSum = int.Parse(Console.ReadLine());
                                switch (accType)
                                {
                                    case "Debit":
                                        var debitWithdrawTransaction =
                                            new DebitWithdrawTransaction(DateTime.Now, trSum, chosenAccount);
                                        debitWithdrawTransaction.Commit();
                                        break;
                                    case "Deposit":
                                        var depositWithdrawTransaction =
                                            new DepositWithdrawTransaction(DateTime.Now, trSum, chosenAccount);
                                        depositWithdrawTransaction.Commit();
                                        break;
                                    case "Withdraw":
                                        var creditWithdrawTransaction =
                                            new CreditWithdrawTransaction(DateTime.Now, trSum, chosenAccount);
                                        creditWithdrawTransaction.Commit();
                                        break;
                                }

                                Console.WriteLine(
                                    $"The withdrawal was successful. Current balance: {chosenAccount.Balance}\n");
                                Console.WriteLine("Choose an option: \n" +
                                                  "1. Return to the main menu \n" +
                                                  "2. Exit \n");
                                switch (Console.ReadLine())
                                {
                                    case "1":
                                        break;
                                    case "2":
                                        return;
                                }

                                break;
                            case "3":
                                Console.WriteLine("Please enter the amount of money to transfer:");
                                int tranSum = int.Parse(Console.ReadLine());
                                Console.WriteLine("Please choose the client to transfer to:");
                                var clients = bank.Accounts.Select(account => account.Owner).ToList();
                                clients = clients.Distinct().ToList();
                                int counter = 1;
                                foreach (Client cl in clients)
                                {
                                    Console.WriteLine($"{counter}. {cl.Name}");
                                    counter++;
                                }

                                Account chosenDestinationAccount =
                                    clients[int.Parse(Console.ReadLine()) - 1].Accounts.First();
                                switch (accType)
                                {
                                    case "Debit":
                                        var debitWithdrawTransaction =
                                            new DebitTransferTransaction(DateTime.Now, tranSum, chosenAccount, chosenDestinationAccount);
                                        debitWithdrawTransaction.Commit();
                                        break;
                                    case "Deposit":
                                        var depositWithdrawTransaction =
                                            new DepositTransferTransaction(DateTime.Now, tranSum, chosenAccount, chosenDestinationAccount);
                                        depositWithdrawTransaction.Commit();
                                        break;
                                    case "Credit":
                                        var creditWithdrawTransaction =
                                            new CreditTransferTransaction(DateTime.Now, tranSum, chosenAccount, chosenDestinationAccount);
                                        creditWithdrawTransaction.Commit();
                                        break;
                                }

                                Console.WriteLine(
                                    $"The transfer was successful. Current balance: {chosenAccount.Balance}\n");
                                Console.WriteLine("Choose an option: \n" +
                                                  "1. Return to the main menu \n" +
                                                  "2. Exit \n");
                                switch (Console.ReadLine())
                                {
                                    case "1":
                                        break;
                                    case "2":
                                        return;
                                }

                                break;
                        }

                        break;
                    case "6":
                        return;
                }
            }
        }
    }
}
