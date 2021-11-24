using System;

namespace Banks.Exceptions
{
    public class TransactionSumLessThanZeroException : Exception
    {
        public TransactionSumLessThanZeroException(string message = "Transaction sum < 0.")
            : base(message)
        {
        }
    }
}