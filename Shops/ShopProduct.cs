using System;

namespace Shops
{
    public class ShopProduct
    {
        public ShopProduct(Guid id, int price, string name, int count)
        {
            Id = id;
            Price = price;
            Name = name;
            Count = count;
        }

        public Guid Id { get; }
        public int Price { get; set; }
        public string Name { get; }
        public int Count { get; set; }
    }
}