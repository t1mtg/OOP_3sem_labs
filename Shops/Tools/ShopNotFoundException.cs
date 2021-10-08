namespace Shops.Tools
{
    public class ShopNotFoundException : ShopException
    {
        public ShopNotFoundException()
            : base("Shop not found")
        {
        }
    }
}