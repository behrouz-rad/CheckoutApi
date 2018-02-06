using Checkout.DataLayer;
using Checkout.DataLayer.Repositories;
using Checkout.DomainClasses;
using Checkout.Web.Middlewares;
using Checkout.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IProductRepository, FakeProductRepository>();

            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc();
            services.AddMemoryCache();          
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
           
            app.UseMvc();
        }
    }
}