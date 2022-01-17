using System;

namespace Banks.Exceptions
{
    public class InsufficientBalanceException : Exception
    {
        public InsufficientBalanceException(string message = "This operation cannot be completed: insufficient balance.")
        {
        }
    }
}