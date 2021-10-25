using System;
using System.Collections.Generic;
using System.Linq;
using Shops.Tools;

namespace Shops
{
    public class ShopService
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
                    shop.ShopProducts.FirstOrDefault(storeProduct => storeProduct.Equals(product));
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

        public void Buy(Customer customer, Shop shop, List<Order> cart)
        {
            int sum = 0;
            var productsToDelete = new List<Order>();
            foreach (Order product in cart)
            {
                ShopProduct shopProduct = shop.GetShopProduct(product.Product);
                if (shopProduct == null)
                {
                    throw new ProductNotFoundException();
                }

                if (product.Count > shopProduct.Count)
                {
                    throw new NotEnoughProductsInShopException();
                }

                sum += shopProduct.Price * product.Count;
                if (sum > customer.Balance)
                {
                    throw new NotEnoughMoneyException();
                }

                productsToDelete.Add(product);
            }

            foreach (Order product in productsToDelete)
            {
                ShopProduct shopProduct = shop.GetShopProduct(product.Product);
                shopProduct.Count -= product.Count;
            }

            customer.Balance -= sum;
        }

        public Shop FindShopWithLowestProductPrice(List<Order> cart)
        {
            var shopWithLowestPrice = new KeyValuePair<Shop, int>(null, int.MaxValue);
            int sum = 0;
            foreach (Shop shop in _shopRepository.AllShops())
            {
                foreach (Order product in cart)
                {
                    ShopProduct shopProduct = shop.GetShopProduct(product.Product);
                    if (shopProduct == null || shopProduct.Count < product.Count)
                    {
                        break;
                    }

                    sum += shopProduct.Price * product.Count;
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
            ShopProduct shopProduct = shop.GetShopProduct(product);
            if (shopProduct == null)
            {
                throw new ProductNotFoundException();
            }

            shopProduct.Price = newPrice;
        }
    }
}