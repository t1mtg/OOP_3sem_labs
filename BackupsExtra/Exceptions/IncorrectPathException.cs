using System;

namespace BackupsExtra.Exceptions
{
    public class IncorrectPathException : Exception
    {
        public IncorrectPathException()
            : base("Incorrect path.")
        {
        }
    }
}