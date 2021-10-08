namespace Shops.Tools
{
    public class NotEnoughProductsInShopException : ShopException
    {
        public NotEnoughProductsInShopException()
            : base("Not enough products in the shop")
        {
        }
    }
}