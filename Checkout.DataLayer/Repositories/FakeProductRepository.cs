using Checkout.Common.Functional;
using Checkout.DomainClasses;
using System.Collections.Generic;
using System.Linq;

namespace Checkout.DataLayer.Repositories
{
    public class FakeProductRepository : IProductRepository
    {
        public IList<Product> Products => new List<Product> {
                new Product { ProductId=1, Name = "Mouse", Price = 50 },
                new Product { ProductId=2, Name = "Keyboard", Price = 70 },
                new Product { ProductId=3, Name = "Mobile", Price = 1000 },
                new Product { ProductId=4, Name = "Flash", Price = 5 },
                new Product { ProductId=5, Name = "Laptop", Price = 1500 }
        };

        public Maybe<Product> GetProductById(int id)
        {
            return (Maybe<Product>)Products.FirstOrDefault(p => p.ProductId == id);
        }
    }
}