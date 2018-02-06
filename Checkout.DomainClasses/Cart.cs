using System;
using System.Collections.Generic;
using System.Linq;

namespace Checkout.DomainClasses
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();
        public virtual CartLine AddItem(Product product, int quantity)
        {
            CartLine line = lineCollection.FirstOrDefault(p => p.Product.ProductId == product.ProductId);
            if (line == null)
            {
                lineCollection.Add(line = new CartLine
                {
                    CartLineId = lineCollection?.Any() == true ? lineCollection.Last().CartLineId + 1 : 1,
                    Product = product,
                    Quantity = quantity > 0 ? quantity : throw new InvalidOperationException("Invalid quantity")
                });
            }
            else
            {
                if (quantity <= 0)
                {
                    throw new InvalidOperationException("Invalid quantity");
                }

                line.Quantity = quantity;
            }

            return line;
        }

        public virtual void RemoveLine(Product product) => lineCollection.RemoveAll(l => l.Product.ProductId == product.ProductId);
        public virtual void Clear() => lineCollection.Clear();
        public virtual IEnumerable<CartLine> Lines => lineCollection;
    }
}