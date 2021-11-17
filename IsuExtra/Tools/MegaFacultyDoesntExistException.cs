using System;

namespace IsuExtra.Tools
{
    public class MegaFacultyDoesntExistException : Exception
    {
        public MegaFacultyDoesntExistException(string message = "Mega faculty does not exist")
            : base(message)
        {
        }
    }
}