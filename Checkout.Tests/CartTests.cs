using Checkout.DomainClasses;
using System.Linq;
using Xunit;

namespace Checkout.Tests
{
    public class CartTests
    {
        [Fact]
        public void Can_Add_New_Lines()
        {   
            Product product1 = new Product { ProductId = 1, Name = "Product1" };
            Product product2 = new Product { ProductId = 2, Name = "Product2" };
           
            Cart target = new Cart();
           
            target.AddItem(product1, 1);
            target.AddItem(product2, 1);
            CartLine[] results = target.Lines.ToArray();
           
            Assert.Equal(2, results.Length);
            Assert.Equal(product1, results[0].Product);
            Assert.Equal(product2, results[1].Product);
        }

        [Fact]
        public void Can_Change_Quantity_For_Existing_Lines()
        {        
            Product product1 = new Product { ProductId = 1, Name = "Product1" };
            Product product2 = new Product { ProductId = 2, Name = "Product2" };
           
            Cart target = new Cart();
            
            target.AddItem(product1, 1);
            target.AddItem(product2, 1);
            target.AddItem(product1, 10); // changing the quantity
            CartLine[] results = target.Lines.OrderBy(c => c.Product.ProductId).ToArray();
           
            Assert.Equal(2, results.Length);
            Assert.Equal(10, results[0].Quantity);
            Assert.Equal(1, results[1].Quantity);
        }

        [Fact]
        public void Can_Remove_Line()
        {           
            Product product1 = new Product { ProductId = 1, Name = "Product1" };
            Product product2 = new Product { ProductId = 2, Name = "Product2" };
            Product product3 = new Product { ProductId = 3, Name = "Product3" };
            
            Cart target = new Cart();
           
            target.AddItem(product1, 1);
            target.AddItem(product2, 3);
            target.AddItem(product3, 5);
            target.AddItem(product2, 1);
           
            target.RemoveLine(product2);
          
            Assert.Empty(target.Lines.Where(c => c.Product == product2));
            Assert.Equal(2, target.Lines.Count());
        }

        [Fact]
        public void Can_Clear_Contents()
        {          
            Product product1 = new Product { ProductId = 1, Name = "Product1", Price = 100M };
            Product product2 = new Product { ProductId = 2, Name = "Product2", Price = 50M };
            
            Cart target = new Cart();
          
            target.AddItem(product1, 1);
            target.AddItem(product2, 1);
            
            target.Clear();
            
            Assert.Empty(target.Lines);
        }
    }
}