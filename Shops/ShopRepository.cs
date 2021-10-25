using System;
using System.Collections.Generic;
using System.Linq;

namespace Shops
{
    public class ShopRepository
    {
        private readonly List<Shop> _shops = new List<Shop>();

        public IReadOnlyList<Shop> AllShops()
        {
            return _shops;
        }

        public void Add(Shop shop)
        {
            _shops.Add(shop);
        }

        public ShopProduct FindShopProduct(ShopProduct shopProduct)
        {
            return _shops.SelectMany(shop => shop.ShopProducts).FirstOrDefault(product => product.Equals(shopProduct));
        }
    }
}