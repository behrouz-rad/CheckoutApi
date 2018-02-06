using Checkout.DomainClasses;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;

namespace Checkout.Web.Services
{
    public class SessionCart : Cart
    {
        public static Cart GetCart(IServiceProvider services)
        {
            IMemoryCache cache = services.GetRequiredService<IMemoryCache>();
            SessionCart cart = cache.Get<SessionCart>("Cart") ?? new SessionCart();
            cart.Cache = cache;

            return cart;
        }

        [JsonIgnore]
        public IMemoryCache Cache { get; set; }

        public override CartLine AddItem(Product product, int quantity)
        {
            CartLine line = base.AddItem(product, quantity);

            Cache.Set<SessionCart>("Cart", this);

            return line;
        }

        public override void RemoveLine(Product product)
        {
            base.RemoveLine(product);
            Cache.Set<SessionCart>("Cart", this);
        }

        public override void Clear()
        {
            base.Clear();
            Cache.Remove("Cart");
        }
    }
}