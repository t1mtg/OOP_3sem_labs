namespace Shops
{
    public class Order
    {
        public Order(Product product, int count)
        {
            Product = product;
            Count = count;
        }

        public Product Product { get; set; }
        public int Count { get; set; }
    }
}