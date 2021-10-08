using System;
using System.Collections.Generic;
using System.Linq;
using Shops.Tools;

namespace Shops
{
    public class ShopService : IShopService
    {
        private readonly ProductRepository _productRepository;
        private readonly ShopRepository _shopRepository;

        public ShopService(ProductRepository productRepository, ShopRepository shopRepository)
        {
            _productRepository = productRepository;
            _shopRepository = shopRepository;
        }

        public Shop Create(string name, string address)
        {
            var shop = new Shop(name, address);
            _shopRepository.Add(shop);
            return shop;
        }

        public Product RegisterProduct(string name)
        {
            var product = new Product(name);
            _productRepository.AddProduct(product);
            return product;
        }

        public void SupplyShopWithProducts(Shop shop, List<ShopProduct> products)
        {
            foreach (ShopProduct product in products)
            {
                ShopProduct productFromShop =
                    shop.ShopProducts.FirstOrDefault(storeProduct => storeProduct.Id.Equals(product.Id));
                if (productFromShop == null)
                {
                    shop.AddProduct(product);
                }
                else
                {
                    productFromShop.Price = product.Price;
                    productFromShop.Count += product.Count;
                }
            }
        }

        public void Buy(Customer customer, Shop shop, Dictionary<Product, int> products)
        {
            int sum = 0;
            foreach ((Product key, int value) in products)
            {
                ShopProduct shopProduct = _shopRepository.FindShopProductById(key.Id);
                if (shopProduct == null)
                {
                    throw new ProductNotFoundException();
                }

                if (value > shopProduct.Count)
                {
                    throw new NotEnoughProductsInShopException();
                }

                sum += shopProduct.Price * value;
                if (sum > customer.Balance)
                {
                    throw new NotEnoughMoneyException();
                }

                shopProduct.Count -= value;
            }

            customer.Balance -= sum;
        }

        public Shop FindShopWithLowestProductPrice(Dictionary<Product, int> products)
        {
            var shopWithLowestPrice = new KeyValuePair<Shop, int>(null, int.MaxValue);
            int sum = 0;
            foreach (Shop shop in _shopRepository.AllShops())
            {
                foreach ((Product key, int value) in products)
                {
                    ShopProduct shopProduct = _shopRepository.FindShopProductById(key.Id);
                    if (shopProduct == null || shopProduct.Count < value)
                    {
                        break;
                    }

                    sum += shopProduct.Price * value;
                    if (sum < shopWithLowestPrice.Value)
                    {
                        shopWithLowestPrice = new KeyValuePair<Shop, int>(shop, sum);
                    }
                }
            }

            if (shopWithLowestPrice.Key == null)
            {
                throw new ShopNotFoundException();
            }

            return shopWithLowestPrice.Key;
        }

        public void ChangePrice(Shop shop, Product product, int newPrice)
        {
            ShopProduct shopProduct = _shopRepository.FindShopProductById(product.Id);
            if (shopProduct == null)
            {
                throw new ProductNotFoundException();
            }

            shopProduct.Price = newPrice;
        }
    }
}