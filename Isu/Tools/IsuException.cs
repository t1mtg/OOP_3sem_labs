using System;

namespace Isu.Tools
{
    public class IsuException : Exception
    {
        public IsuException()
        {
            Console.WriteLine("An error has occured");
        }

        public IsuException(string message)
            : base(message)
        {
            Console.WriteLine(message);
        }

        public IsuException(string message, Exception innerException)
            : base(message, innerException)
        {
            Console.WriteLine(InnerException + message);
        }
    }
}