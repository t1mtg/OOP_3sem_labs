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

        public bool ExistsByName(string name)
        {
            return _shops.Any(shop => name.ToLower().Equals(shop.Name.ToLower()));
        }

        public ShopProduct FindShopProductById(Guid id)
        {
            return _shops.SelectMany(shop => shop.ShopProducts).FirstOrDefault(product => id.Equals(product.Id));
        }
    }
}