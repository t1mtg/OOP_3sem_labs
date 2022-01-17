using System;

namespace DTO.Exceptions
{
    public class WrongPageException : Exception
    {
        public WrongPageException(string message = "Wrong page number")
            : base(message)
        {
        }
    }
}