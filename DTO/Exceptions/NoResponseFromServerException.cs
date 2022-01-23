using System;

namespace DTO.Exceptions
{
    public class NoResponseFromServerException : Exception
    {
        public NoResponseFromServerException(string message = "No response from server")
            : base(message)
        {
        }
    }
}