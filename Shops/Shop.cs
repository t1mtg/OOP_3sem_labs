using System;
using System.Collections.Generic;

namespace Shops
{
    public class Shop
    {
        public Shop(string name, string address)
        {
            Name = name;
            Address = address;
            Products = new List<ShopProduct>();
            ShopProducts = Products.AsReadOnly();
            Id = Guid.NewGuid();
        }

        public IReadOnlyList<ShopProduct> ShopProducts { get; }
        public Guid Id { get; }
        public string Address { get; }
        public string Name { get; }
        private List<ShopProduct> Products { get; }

        public void AddProduct(ShopProduct product)
        {
            Products.Add(product);
        }
    }
}