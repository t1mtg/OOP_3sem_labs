using System.Collections.Generic;
using System.Linq;

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
            _products.Add(product);
        }

        public bool ExistsByName(string name)
        {
            return _products.Any(product => name.ToLower().Equals(product.Name.ToLower()));
        }
    }
}