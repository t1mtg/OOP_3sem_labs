using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Shops.Tools;

namespace Shops.Tests
{
    public class ShopServiceTest
    {
        private ShopService _shopService;

        [SetUp]
        public void Setup()
        {
            var productRepository = new ProductRepository();
            var shopRepository = new ShopRepository();
            _shopService = new ShopService(productRepository, shopRepository);
        }

        [Test]
        public void SupplyStoreWithProducts()
        {
            Shop shop1 = _shopService.Create("Diksi", "Kamennoostrovkiy pr. 24");
            Product product1 = _shopService.RegisterProduct("Coca-Cola");
            Product product2 = _shopService.RegisterProduct("Pringles Paprika");
            Product product3 = _shopService.RegisterProduct("Bounty");
            var storeProducts = new List<ShopProduct>()
            {
                new ShopProduct(product1.Id, 70, product1.Name, 10),
                new ShopProduct(product2.Id, 120, product2.Name, 20),
                new ShopProduct(product3.Id, 50, product3.Name, 100)
            };
            _shopService.SupplyShopWithProducts(shop1, storeProducts);
            Assert.AreEqual(3, shop1.ShopProducts.Count);
            Assert.AreEqual("Coca-Cola", shop1.ShopProducts
                .First(product => product1.Id.Equals(product.Id)).Name);
            Assert.AreEqual(120, shop1.ShopProducts
                .First(product => product2.Id.Equals(product.Id)).Price);
            Assert.AreEqual(100, shop1.ShopProducts
                .First(product => product3.Id.Equals(product.Id)).Count);
        }

        [Test]
        public void BuyProductException()
        {
            Shop shop1 = _shopService.Create("Diksi", "Kamennoostrovkiy pr. 24");
            Product product1 = _shopService.RegisterProduct("Coca-Cola");
            Product product2 = _shopService.RegisterProduct("Pringles Paprika");
            Product product3 = _shopService.RegisterProduct("Bounty");
            var shopProducts = new List<ShopProduct>()
            {
                new ShopProduct(product1.Id, 70, product1.Name, 10),
                new ShopProduct(product2.Id, 120, product2.Name, 20),
            };
            _shopService.SupplyShopWithProducts(shop1, shopProducts);
            var customer1 = new Customer(1000);
            var cart1 = new Dictionary<Product, int>()
            {
                {product3, 2}
            };
            Assert.Catch<ProductNotFoundException>(() =>
            {
                _shopService.Buy(customer1, shop1, cart1);
            });

            var customer2 = new Customer(100);
            var cart2 = new Dictionary<Product, int>()
            {
                {product1, 5},
                {product2, 4}
            };
            Assert.Catch<NotEnoughMoneyException>(() =>
            {
                _shopService.Buy(customer2, shop1, cart2);
            });

            var customer3 = new Customer(5000);
            var cart3 = new Dictionary<Product, int>()
            {
                {product1, 9},
                {product2, 21}
            };
            Assert.Catch<NotEnoughProductsInShopException>(() =>
            {
                _shopService.Buy(customer3, shop1, cart3);
            });
        }

        [Test]
        public void ChangePrice_PriceChanged()
        {
            Shop shop = _shopService.Create("Diksi", "Kamennoostrovkiy pr. 24");
            Product product = _shopService.RegisterProduct("Chebupizza");
            var shopProducts = new List<ShopProduct>
            {
                new ShopProduct(product.Id, 80, product.Name, 5)
            };
            _shopService.SupplyShopWithProducts(shop, shopProducts);
            _shopService.ChangePrice(shop, product, 60);

            Assert.AreEqual(60,
                shop.ShopProducts
                    .First(shopProduct => product.Id.Equals(shopProduct.Id)).Price);
        }

        [Test]
        public void FindShopWithTheLowestProductPrice()
        {
            Shop shop1 = _shopService.Create("Diksi", "Kamennoostrovkiy pr. 24");
            Shop shop2 = _shopService.Create("Real", "Kamennoostrovkiy pr. 59");
            Shop shop3 = _shopService.Create("Lenta", "Chkalovskiy pr. 50");
            Product product = _shopService.RegisterProduct("Chebupizza");
            var shopProducts1 = new List<ShopProduct>
            {
                new ShopProduct(product.Id, 110, product.Name, 50)
            };
            var shopProducts2 = new List<ShopProduct>
            {
                new ShopProduct(product.Id, 120, product.Name, 350)
            };
            var shopProducts3 = new List<ShopProduct>
            {
                new ShopProduct(product.Id, 100, product.Name, 29)
            };
            _shopService.SupplyShopWithProducts(shop1, shopProducts1);
            _shopService.SupplyShopWithProducts(shop2, shopProducts2);
            _shopService.SupplyShopWithProducts(shop3, shopProducts3);
            var cart = new Dictionary<Product, int>()
            {
                {product, 40}
            };
            Assert.AreEqual(shop1.Id, _shopService.FindShopWithLowestProductPrice(cart).Id);
        }

        [Test]
        public void FindShopWithTheLowestProductPriceException()
        {
            Shop shop1 = _shopService.Create("Diksi", "Kamennoostrovkiy pr. 24");
            Shop shop2 = _shopService.Create("Real", "Kamennoostrovkiy pr. 59");
            Product product = _shopService.RegisterProduct("Chebupizza");
            var shopProducts1 = new List<ShopProduct>
            {
                new ShopProduct(product.Id, 110, product.Name, 50)
            };
            var shopProducts2 = new List<ShopProduct>
            {
                new ShopProduct(product.Id, 120, product.Name, 40)
            };
            _shopService.SupplyShopWithProducts(shop1, shopProducts1);
            _shopService.SupplyShopWithProducts(shop2, shopProducts2);
            var cart = new Dictionary<Product, int>()
            {
                {product, 60}
            };
            Assert.Catch<ShopNotFoundException>(() =>
            {
                _shopService.FindShopWithLowestProductPrice(cart);
            });
        }

        [Test]
        public void Buy_CountAndBalanceChanged()
        {
            Shop shop1 = _shopService.Create("Diksi", "Kamennoostrovkiy pr. 24");
            Product product = _shopService.RegisterProduct("Chebupizza");
            var shopProducts1 = new List<ShopProduct>
            {
                new ShopProduct(product.Id, 110, product.Name, 50)
            };
            _shopService.SupplyShopWithProducts(shop1, shopProducts1);
            var customer = new Customer(1000);
            var cart = new Dictionary<Product, int>()
            {
                {product, 5}
            };
            _shopService.Buy(customer, shop1, cart);
            Assert.AreEqual(450, customer.Balance);
            Assert.AreEqual(45, shop1.ShopProducts
                .First(shopProduct => shopProduct.Id.Equals(product.Id)).Count);
        }
        
    }
}