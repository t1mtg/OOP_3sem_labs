using System;

namespace IsuExtra.Tools
{
    public class ReachedMaxAmountOfOgnpException : Exception
    {
        public ReachedMaxAmountOfOgnpException(string message = "Reached max amount of ognps")
            : base(message)
        {
        }
    }
}