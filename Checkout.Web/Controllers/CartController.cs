using Checkout.DataLayer;
using Checkout.DomainClasses;
using Checkout.Web.Dummies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;

namespace Checkout.Web.Controllers
{
    [Route("api/v1/[controller]")]
    public class CartController : BaseController
    {
        private readonly IProductRepository repository;
        private readonly Cart cart;

        public CartController(IProductRepository repo,
                              Cart cartService)
        {
            repository = repo;
            cart = cartService;
        }

        [HttpPost]
        public IActionResult Post([FromBody]DummyOrderCreate dummyProduct)
        {
            if (dummyProduct == null)
            {
                return BadRequest("Invalid product id");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = repository.GetProductById(dummyProduct.ProductId.Value);

            if (product.HasNoValue)
            {
                return BadRequest("No product found!");
            }

            var line = cart.AddItem(product.Value, dummyProduct.Quantity);

            return CreatedAtRoute("GetLineById", new { id = line.CartLineId }, line);
        }

        [HttpGet("{id:int}", Name = "GetLineById")]
        public IActionResult GetLineById(int id)
        {
            var line = cart.Lines.FirstOrDefault(x => x.CartLineId == id);

            return line != null ? new OkObjectResult(line) : (IActionResult)NotFound();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]DummyOrderDelete dummyProduct)
        {
            if (dummyProduct == null)
            {
                return BadRequest("Invalid product id");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = repository.GetProductById(dummyProduct.ProductId.Value);

            if (product.HasNoValue)
            {
                return BadRequest("No product found!");
            }

            cart.RemoveLine(product.Value);

            return NoContent();
        }

        [HttpGet("clear")]
        public IActionResult Clear()
        {
            cart.Clear();

            return NoContent();
        }

        [HttpGet("show")]
        public IActionResult Show()
        {
            return OkObjectResult(cart.Lines);
        }
    }
}