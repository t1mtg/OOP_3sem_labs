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

        public bool ExistsByName(string name)
        {
            return _products.Any(product => name.ToLower().Equals(product.Name.ToLower()));
        }

        public void AddProduct(Product product)
        {
            if (GetProductById(product.Id) != null)
            {
                throw new ProductIsAlreadyInTheListException();
            }

            _products.Add(product);
        }

        private Product GetProductById(Guid id)
        {
            return _products.FirstOrDefault(product => product.Id.Equals(id));
        }
    }
}