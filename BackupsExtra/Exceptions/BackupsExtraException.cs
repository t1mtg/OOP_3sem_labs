using System;

namespace BackupsExtra
{
    public class BackupsExtraException : Exception
    {
        public BackupsExtraException(string message)
        {
            Console.WriteLine(message);
        }
    }
}