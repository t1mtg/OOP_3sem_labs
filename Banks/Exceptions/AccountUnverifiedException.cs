using System;

namespace Banks.Exceptions
{
    public class AccountUnverifiedException : Exception
    {
        public AccountUnverifiedException(string message = "Unverified account cannot complete this operation.")
            : base(message)
        {
        }
    }
}