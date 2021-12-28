using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Banks.Accounts;
using Banks.Exceptions;
using Banks.Transactions;
using NUnit.Framework;

namespace Banks.Tests
{
    public class BanksTest
    {
        private Bank _bank;
        private Client _client1;
        private Client _client2;
        private Account _debitAccount1;
        private Account _debitAccount2;
        private CentralBank _centralBank;

        [SetUp]
        public void Setup()
        {
            var interestRates = new InterestRates();
            interestRates.AddNewInterestRate(10000, 4);
            interestRates.AddNewInterestRate(20000, 5);
            interestRates.AddNewInterestRate(30000, 6);
            _centralBank = new CentralBank();
            _bank = _centralBank.RegisterBank("Gurman Bank", 1, 10000, 1488, 22, interestRates);
            _client1 = Client.Builder("Timofey Gurman").SetAddress("Vyazemskiy 5/7").GetClient();
            _client2 = Client.Builder("Ivan Gurman").SetAddress("Fedora Abramova 8").SetPassport("6969 696969").GetClient();
            _debitAccount1 = _bank.AddNewDebitAccount(_client1);
            _debitAccount2 = _bank.AddNewDebitAccount(_client2);
            _debitAccount1.Refill(DateTime.Now, 5000);
            _debitAccount2.Refill(DateTime.Now, 25000);
        }
        
        [Test]
        public void Refill()
        {
            /*var refillTransaction4 = new RefillTransaction(DateTime.Now, 19000, _debitAccount1);
            refillTransaction4.Commit();*/
            _debitAccount1.Refill(DateTime.Now, 19000);
            Assert.AreEqual(24000, _debitAccount1.Balance);
        }
        
        [Test]
        public void CancelRefill()
        {
            var refillTransaction1 = new RefillTransaction(DateTime.Now, 5000, _debitAccount1);
            refillTransaction1.Commit();
            Assert.AreEqual(10000, _debitAccount1.Balance);
            refillTransaction1.Rollback();
            Assert.AreEqual(5000, _debitAccount1.Balance);
        }
        
        [Test]
        public void Withdraw()
        {
            /*var refillTransaction = new RefillTransaction(DateTime.Now, 4321, _debitAccount1);
            refillTransaction.Commit();
            var debitWithdrawTransaction = new DebitWithdrawTransaction(DateTime.Now, 1234, _debitAccount1);
            debitWithdrawTransaction.Commit();*/
            _debitAccount1.Refill(DateTime.Now, 4321);
            _debitAccount1.Withdraw(DateTime.Now, 1234);
            Assert.AreEqual(8087, _debitAccount1.Balance);
            
        }
        [Test]
        public void Transfer()
        {
            /*var debitTransferTransaction = new DebitTransferTransaction(DateTime.Now,2000, _debitAccount1, _debitAccount2);
            debitTransferTransaction.Commit();*/
            _debitAccount1.Transfer(DateTime.Now, 2000, _debitAccount2);
            Assert.AreEqual(3000, _debitAccount1.Balance);
            Assert.AreEqual(27000, _debitAccount2.Balance);
        }

        [Test]
        public void SubtractCreditCommission()
        {
            Account creditAccount = _bank.AddNewCreditAccount(_client2);
            /*var refillTransaction = new RefillTransaction(DateTime.Now, 1000, creditAccount);
            refillTransaction.Commit();*/
            creditAccount.Refill(DateTime.Now, 1000);
            /*CreditWithdrawTransaction creditWithdrawTransaction = new(DateTime.Now, 1010, creditAccount);
            creditWithdrawTransaction.Commit();*/
            creditAccount.Withdraw(DateTime.Now, 1010);
            DateTime fourteenDaysPassed = DateTime.Now.AddDays(14);
            _centralBank.Notify(fourteenDaysPassed);
            Assert.AreEqual(-296, Math.Round(creditAccount.Balance));
        }
        
        [Test]
        public void AddInterests()
        {
            DateTime fourMonthsPassed = DateTime.Now.AddMonths(4);
            _centralBank.Notify(fourMonthsPassed);
            Assert.AreEqual(5017, Math.Round(_debitAccount1.Balance));
        }
        
    }
}