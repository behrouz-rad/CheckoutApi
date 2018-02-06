using Checkout.Common.Functional;
using Checkout.DomainClasses;
using System.Collections.Generic;

namespace Checkout.DataLayer
{
    public interface IProductRepository
    {
        IList<Product> Products { get; }
        Maybe<Product> GetProductById(int id);
    }
}