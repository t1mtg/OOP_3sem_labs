using System;

namespace Isu.Tools
{
    public class ReachMaxAmountException : IsuException
    {
        public ReachMaxAmountException()
            : base("Reached max amount of students in group")
        {
        }
    }
}