using System;

namespace Banks.Exceptions
{
    public class DepositIsNotExpiredException : Exception
    {
        public DepositIsNotExpiredException(string message = "This operation cannot be completed: Deposit has not expired yet.")
            : base(message)
        {
        }
    }
}