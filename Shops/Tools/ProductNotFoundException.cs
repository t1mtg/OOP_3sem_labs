namespace Shops.Tools
{
public class ProductNotFoundException : ShopException
{
    public ProductNotFoundException()
        : base("Product not found")
    {
    }
}
}