using System;

namespace Shops
{
    public class Product
    {
        public Product(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; }

        public bool Equals(Product product)
        {
            if (product == null)
            {
                return false;
            }

            return this.Id == product.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Product)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}