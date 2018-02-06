using System.ComponentModel.DataAnnotations;

namespace Checkout.Web.Dummies
{
    public class DummyOrderDelete
    {
        [Required]
        public int? ProductId { get; set; }
    }
}