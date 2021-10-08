using System;

namespace Shops.Tools
{
    public class ShopException : Exception
    {
        public ShopException()
        {
            Console.WriteLine("An error has occured");
        }

        public ShopException(string message)
            : base(message)
        {
            Console.WriteLine(message);
        }

        public ShopException(string message, Exception innerException)
            : base(message, innerException)
        {
            Console.WriteLine(InnerException + message);
        }
    }
}