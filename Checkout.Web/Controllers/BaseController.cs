using Microsoft.AspNetCore.Mvc;

namespace Checkout.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        public virtual OkObjectResult OkObjectResult(object value)
        {
            return new OkObjectResult(value);
        }
    }
}