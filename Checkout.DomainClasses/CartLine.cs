using System;

namespace Checkout.DomainClasses
{
    public class CartLine
    {
        private int _quantity;

        public int CartLineId { get; set; }
        public Product Product { get; set; }
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (value <= 0)
                {
                    throw new InvalidOperationException("Invalid quantity");
                }
                _quantity = value;
            }
        }
    }
}