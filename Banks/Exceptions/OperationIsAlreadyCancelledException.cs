using System;

namespace Banks.Exceptions
{
    public class OperationIsAlreadyCancelledException : Exception
    {
        public OperationIsAlreadyCancelledException(string message = "Operation is already cancelled.")
            : base(message)
        {
        }
    }
}