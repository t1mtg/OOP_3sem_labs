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

        public override bool Equals(object obj)
            => obj is Product product && (Id == product.Id);

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
