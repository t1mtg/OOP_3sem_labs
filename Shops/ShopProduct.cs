using System;

namespace Shops
{
    public class ShopProduct
    {
        public ShopProduct(Guid productId, int price, string name, int count)
        {
            ProductId = productId;
            Id = Guid.NewGuid();
            Price = price;
            Name = name;
            Count = count;
        }

        public Guid ProductId { get; }
        public Guid Id { get; }
        public int Price { get; set; }
        public string Name { get; }
        public int Count { get; set; }
    }
}