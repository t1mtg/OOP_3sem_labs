using System;

namespace DTO.Exceptions
{
    public class EmptyCommentException : Exception
    {
        public EmptyCommentException(string message = "Empty comment")
            : base(message)
        {
        }
    }
}