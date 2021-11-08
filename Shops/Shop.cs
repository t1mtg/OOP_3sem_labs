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

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Shop)obj);
        }

        public bool Equals(Shop other)
        {
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public void AddProduct(ShopProduct product)
        {
            if (GetShopProduct(product) != null)
            {
                throw new ProductIsAlreadyInTheListException();
            }

            _products.Add(product);
        }

        public ShopProduct GetShopProduct(ShopProduct product)
        {
            return ShopProducts.FirstOrDefault(product.Equals);
        }

        public ShopProduct GetShopProduct(Product product)
        {
            return ShopProducts.FirstOrDefault(shopProduct => product == shopProduct);
        }
    }
}