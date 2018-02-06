using System.ComponentModel.DataAnnotations;

namespace Checkout.Web.Dummies
{
    public class DummyOrderCreate
    {
        [Required]
        public int? ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}