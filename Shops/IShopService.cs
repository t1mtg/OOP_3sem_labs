using System.Collections.Generic;

namespace Shops
{
    public interface IShopService
    {
        Shop Create(string name, string address);
        Product RegisterProduct(string name);
        void SupplyShopWithProducts(Shop shop, List<ShopProduct> products);
        void Buy(Customer customer, Shop shop, Dictionary<Product, int> products);
        Shop FindShopWithLowestProductPrice(Dictionary<Product, int> products);
        void ChangePrice(Shop shop, Product product, int newPrice);
    }
}
