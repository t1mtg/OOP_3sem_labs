using System;

namespace Shops
{
    public class ShopProduct
    {
        public ShopProduct(Guid productId, int price, string name, int count)
        {
            ProductId = productId;
            Id = Guid.NewGuid();
            Price = price;
            Name = name;
            Count = count;
        }

        public Guid ProductId { get; }
        public Guid Id { get; }
        public int Price { get; set; }
        public string Name { get; }
        public int Count { get; set; }

        public static bool operator ==(Product product1, ShopProduct product2)
        {
            if (product1 == null || product2 == null)
            {
                return false;
            }

            return product1.Id.Equals(product2.ProductId);
        }

        public static bool operator !=(Product product1, ShopProduct product2)
        {
            return !(product1 == product2);
        }

        public bool Equals(ShopProduct other)
        {
            return this.Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ShopProduct)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}