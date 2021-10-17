using System;
using System.Collections.Generic;
using System.Linq;
using Shops.Tools;

namespace Shops
{
    public class Shop
    {
        private readonly List<ShopProduct> _products;

        public Shop(string name, string address)
        {
            Name = name;
            Address = address;
            _products = new List<ShopProduct>();
            ShopProducts = _products.AsReadOnly();
            Id = Guid.NewGuid();
        }

        public IReadOnlyList<ShopProduct> ShopProducts { get; }
        public Guid Id { get; }
        public string Address { get; }
        public string Name { get; }

        public void AddProduct(ShopProduct product)
        {
            if (GetShopProduct(product.ProductId) != null)
            {
                throw new ProductIsAlreadyInTheListException();
            }

            _products.Add(product);
        }

        public ShopProduct GetShopProduct(Guid id)
        {
            return ShopProducts.FirstOrDefault(shopProduct => shopProduct.ProductId.Equals(id));
        }
    }
}