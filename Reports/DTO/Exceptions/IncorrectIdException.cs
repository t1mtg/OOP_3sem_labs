using System;

namespace DTO.Exceptions
{
    public class IncorrectIdException : Exception
    {
        public IncorrectIdException(string message = "Incorrect ID")
            : base(message)
        {
        }
    }
}