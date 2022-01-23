using System;

namespace DTO.Exceptions
{
    public class IncorrectNameException : Exception
    {
        public IncorrectNameException(string message = "Please provide correct name")
            : base(message)
        {
        }
    }
}