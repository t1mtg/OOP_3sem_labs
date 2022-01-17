using System;

namespace Banks.Exceptions
{
    public class CreditLimitExceededException : Exception
    {
        public CreditLimitExceededException(string message = "This operation cannot be completed: Credit Limit Exceeded.")
        {
        }
    }
}