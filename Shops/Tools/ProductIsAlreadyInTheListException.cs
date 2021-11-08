namespace Shops.Tools
{
    public class ProductIsAlreadyInTheListException : ShopException
    {
        public ProductIsAlreadyInTheListException()
            : base("Product is already in the list")
        {
        }
    }
}