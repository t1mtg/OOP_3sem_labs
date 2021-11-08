namespace Shops.Tools
{
    public class NegativeBalanceException : ShopException
    {
        public NegativeBalanceException()
            : base("Negative balance")
        {
        }
    }
}