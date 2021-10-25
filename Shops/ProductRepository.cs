using System;
using System.Collections.Generic;
using System.Linq;
using Shops.Tools;

namespace Shops
{
    public class ProductRepository
    {
        private readonly List<Product> _products = new List<Product>();

        public List<Product> AllProducts()
        {
            return _products;
        }

        public void AddProduct(Product product)
        {
            if (GetProduct(product) != null)
            {
                throw new ProductIsAlreadyInTheListException();
            }

            _products.Add(product);
        }

        private Product GetProduct(Product product)
        {
            return _products.FirstOrDefault(product1 => product1.Equals(product));
        }
    }
}