using Checkout.DataLayer.Repositories;
using Checkout.DomainClasses;
using Checkout.Web.Controllers;
using Checkout.Web.Dummies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Checkout.Tests
{
    public class CartControllerTests
    {
        [Fact]
        public void When_Post_IsCalled_With_CorrectData_Should_RespondWith_CreatedAtRoute()
        {
            Cart cart = new Cart();
            using (var controller = new CartController(new FakeProductRepository(), cart))
            {
                DummyOrderCreate newOrder = new DummyOrderCreate()
                {
                    ProductId = 1
                };

                // Act
                IActionResult actionResult = controller.Post(newOrder);
                var createdAtRouteResult = actionResult as CreatedAtRouteResult;

                // Assert
                Assert.NotNull(createdAtRouteResult);
                Assert.Equal(201, createdAtRouteResult.StatusCode);

                CartLine cartLine = createdAtRouteResult.Value as CartLine;
                Assert.NotNull(cartLine);
                Assert.Single(cart.Lines);
                Assert.Equal(1, cart.Lines.LastOrDefault().Product.ProductId);
                Assert.Equal("GetLineById", createdAtRouteResult.RouteName);
            }
        }

        [Fact]
        public void When_Post_IsCalled_With_Null_Should_RespondWith_BadRequest()
        {
            Cart cart = new Cart();
            using (var controller = new CartController(new FakeProductRepository(), cart))
            {
                // Act
                IActionResult actionResultNull = controller.Post(null);
                var badRequestResult1 = actionResultNull as BadRequestObjectResult;

                // Assert
                Assert.NotNull(badRequestResult1);
                Assert.Equal(400, badRequestResult1.StatusCode);
            }
        }

        [Fact]
        public void When_Post_IsCalled_And_ModelState_Is_Invalid_Should_RespondWith_BadRequest()
        {
            Cart cart = new Cart();
            using (var controller = new CartController(new FakeProductRepository(), cart))
            {
                DummyOrderCreate newOrder = new DummyOrderCreate()
                {
                    ProductId = 1
                };

                controller.ModelState.AddModelError("ProductId", "ProductId cannot be null");

                // Act
                IActionResult actionResultNull = controller.Post(newOrder);
                var badRequestResult1 = actionResultNull as BadRequestObjectResult;

                // Assert
                Assert.NotNull(badRequestResult1);
                Assert.Equal(400, badRequestResult1.StatusCode);
            }
        }

        [Fact]
        public void When_Post_IsCalled_And_ProductId_Is_Invalid_Should_RespondWith_BadRequest()
        {
            Cart cart = new Cart();
            using (var controller = new CartController(new FakeProductRepository(), cart))
            {
                DummyOrderCreate newOrder = new DummyOrderCreate()
                {
                    ProductId = 114
                };

                // Act
                IActionResult actionResultNull = controller.Post(newOrder);
                var badRequestResult1 = actionResultNull as BadRequestObjectResult;

                // Assert
                Assert.NotNull(badRequestResult1);
                Assert.Equal(400, badRequestResult1.StatusCode);
            }
        }

        [Fact]
        public void When_Delete_IsCalled_With_CorrectData_Should_RespondWith_NoContent()
        {
            Cart cart = new Cart();
            using (var controller = new CartController(new FakeProductRepository(), cart))
            {
                cart.AddItem(new Product() { ProductId = 1, Name = "Mouse" }, 1);
                cart.AddItem(new Product() { ProductId = 2, Name = "Keyboard" }, 1);

                // Act
                IActionResult actionResult = controller.Delete(new DummyOrderDelete() { ProductId = 1 });
                var noContentResult = actionResult as NoContentResult;

                // Assert
                Assert.NotNull(noContentResult);         
                Assert.Equal(204, noContentResult.StatusCode);
                Assert.Single(cart.Lines);
            }
        }

        [Fact]
        public void When_Delete_IsCalled_With_Null_Should_RespondWith_BadRequest()
        {
            Cart cart = new Cart();
            using (var controller = new CartController(new FakeProductRepository(), cart))
            {
                // Act
                IActionResult actionResultNull = controller.Delete(null);
                var badRequestResult1 = actionResultNull as BadRequestObjectResult;

                // Assert
                Assert.NotNull(badRequestResult1);
                Assert.Equal(400, badRequestResult1.StatusCode);
            }
        }

        [Fact]
        public void When_Delete_IsCalled_And_ModelState_Is_Invalid_Should_RespondWith_BadRequest()
        {
            Cart cart = new Cart();
            using (var controller = new CartController(new FakeProductRepository(), cart))
            {
                DummyOrderDelete deleteOrder = new DummyOrderDelete()
                {
                    ProductId = 1
                };

                controller.ModelState.AddModelError("ProductId", "ProductId cannot be null");

                // Act
                IActionResult actionResultNull = controller.Delete(deleteOrder);
                var badRequestResult1 = actionResultNull as BadRequestObjectResult;

                // Assert
                Assert.NotNull(badRequestResult1);
                Assert.Equal(400, badRequestResult1.StatusCode);
            }
        }

        [Fact]
        public void When_Delete_IsCalled_And_ProductId_Is_Invalid_Should_RespondWith_BadRequest()
        {
            Cart cart = new Cart();
            using (var controller = new CartController(new FakeProductRepository(), cart))
            {
                DummyOrderDelete deleteOrder = new DummyOrderDelete()
                {
                    ProductId = 114
                };

                // Act
                IActionResult actionResultNull = controller.Delete(deleteOrder);
                var badRequestResult1 = actionResultNull as BadRequestObjectResult;

                // Assert
                Assert.NotNull(badRequestResult1);
                Assert.Equal(400, badRequestResult1.StatusCode);
            }
        }

        [Fact]
        public void When_Clear_IsCalled_Should_RespondWith_NoContent()
        {
            Cart cart = new Cart();
            using (var controller = new CartController(new FakeProductRepository(), cart))
            {
                cart.AddItem(new Product() { ProductId = 1, Name = "Mouse" }, 1);
                cart.AddItem(new Product() { ProductId = 2, Name = "Keyboard" }, 1);
                
                // Act
                IActionResult actionResult = controller.Clear();
                var noContentResult = actionResult as NoContentResult;

                // Assert
                Assert.NotNull(noContentResult);
                Assert.Equal(204, noContentResult.StatusCode);
                Assert.Empty(cart.Lines);
            }
        }

        [Fact]
        public void When_GetLineById_IsCalled_With_RightId_Should_RespondWith_Ok()
        {
            Cart cart = new Cart();
            using (var controller = new CartController(new FakeProductRepository(), cart))
            {
                cart.AddItem(new Product() { ProductId = 1, Name = "Mouse" }, 1);
                cart.AddItem(new Product() { ProductId = 1, Name = "Keyboard" }, 1);
                cart.AddItem(new Product() { ProductId = 3, Name = "Monitor" }, 1);

                // Act
                IActionResult actionResult = controller.GetLineById(2);
                var okObjectResult = actionResult as OkObjectResult;

                // Assert
                Assert.NotNull(okObjectResult);
                Assert.Equal(200, okObjectResult.StatusCode);
                CartLine cartLine = okObjectResult.Value as CartLine;
                Assert.NotNull(cartLine);
            }
        }

        [Fact]
        public void When_GetLineById_IsCalled_With_WrongId_Should_RespondWith_NotFound()
        {
            Cart cart = new Cart();
            using (var controller = new CartController(new FakeProductRepository(), cart))
            {
                cart.AddItem(new Product() { ProductId = 1, Name = "Mouse" }, 1);
                cart.AddItem(new Product() { ProductId = 1, Name = "Keyboard" }, 1);
                cart.AddItem(new Product() { ProductId = 3, Name = "Monitor" }, 1);

                // Act
                IActionResult actionResult = controller.GetLineById(3);
                var notFoundResult = actionResult as NotFoundResult;

                // Assert
                Assert.NotNull(notFoundResult);
                Assert.Equal(404, notFoundResult.StatusCode);
            }
        }

        [Fact]
        public void When_Show_IsCalled_Should_RespondWith_Ok()
        {
            Cart cart = new Cart();
            using (var controller = new CartController(new FakeProductRepository(), cart))
            {
                cart.AddItem(new Product() { ProductId = 1, Name = "Mouse" }, 1);
                cart.AddItem(new Product() { ProductId = 1, Name = "Keyboard" }, 1);
                cart.AddItem(new Product() { ProductId = 3, Name = "Monitor" }, 1);

                // Act
                IActionResult actionResult = controller.Show();
                var okObjectResult = actionResult as OkObjectResult;

                // Assert
                Assert.NotNull(okObjectResult);
                Assert.Equal(200, okObjectResult.StatusCode);
                IEnumerable<CartLine> cartLines = okObjectResult.Value as IEnumerable<CartLine>;
                Assert.NotNull(cartLines);
                Assert.Equal(2, cartLines.Count());
            }
        }
    }
}
