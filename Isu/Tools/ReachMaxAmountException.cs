using System;

namespace Isu.Tools
{
    public class ReachMaxAmountException : IsuException
    {
        public ReachMaxAmountException(string message = "Reached max amount of students in group")
            : base(message)
        {
        }
    }
}