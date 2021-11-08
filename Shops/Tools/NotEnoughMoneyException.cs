namespace Shops.Tools
{
    public class NotEnoughMoneyException : ShopException
    {
        public NotEnoughMoneyException()
            : base("Not enough money to buy those products")
        {
        }
    }
}