using System;
using System.Collections.Generic;
using static System.Console;

namespace Checkout.ApiClient.Console
{
    public class Product
    {
        public int productId { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
    }

    public class RootObject
    {
        public int cartLineId { get; set; }
        public Product product { get; set; }
        public int quantity { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string endpointCUD = "http://localhost:58942/api/v1/cart";
            string endpointClear = "http://localhost:58942/api/v1/cart/clear";
            string endpointShowBasket = "http://localhost:58942/api/v1/cart/show";

            System.Threading.Thread.Sleep(7000);

            var createOrder1Result = new ApiHttpClient().PostRequest<RootObject>(endpointCUD, new { ProductId = 1 }).Result;

            var createOrder2Result = new ApiHttpClient().PostRequest<RootObject>(endpointCUD, new { ProductId = 1, Quantity = 2 }).Result;

            var createOrder3Result = new ApiHttpClient().PostRequest<RootObject>(endpointCUD, new { ProductId = 3 }).Result;

            var showBasket1 = new ApiHttpClient().GetRequest<List<RootObject>>(endpointShowBasket).Result.Model;

            ForegroundColor = ConsoleColor.Green;
            WriteLine($"There are {showBasket1.Count} items in the basket:");
            ForegroundColor = ConsoleColor.Gray;
            WriteLine(JsonHelper.ConvertToJsonString(showBasket1));
            WriteLine(Environment.NewLine);

            ForegroundColor = ConsoleColor.Green;
            WriteLine("Decreasing the quantity of Product 1 to 1:");
            var decreaseProductOrderQuantityResult = new ApiHttpClient().PostRequest<RootObject>(endpointCUD, new { ProductId = 1, Quantity = 1 }).Result;
            ForegroundColor = ConsoleColor.Gray;
            var showBasket2 = new ApiHttpClient().GetRequest<List<RootObject>>(endpointShowBasket).Result.Model;
            WriteLine(JsonHelper.ConvertToJsonString(showBasket2));
            WriteLine(Environment.NewLine);

            ForegroundColor = ConsoleColor.Green;
            WriteLine("Deleting Product 1:");
            var deleteProductResult = new ApiHttpClient().DeleteRequest<string>(endpointCUD, new { ProductId = 1 }).Result;
            ForegroundColor = ConsoleColor.Gray;
            var showBasket3 = new ApiHttpClient().GetRequest<List<RootObject>>(endpointShowBasket).Result.Model;
            WriteLine(JsonHelper.ConvertToJsonString(showBasket3));
            WriteLine(Environment.NewLine);

            ForegroundColor = ConsoleColor.Green;
            WriteLine("Clearing the basket:");
            var clearBasketResult = new ApiHttpClient().GetRequest<string>(endpointClear).Result;
            ForegroundColor = ConsoleColor.Gray;
            var showBasket4 = new ApiHttpClient().GetRequest<List<RootObject>>(endpointShowBasket).Result.Model;
            WriteLine(JsonHelper.ConvertToJsonString(showBasket4));

            Read();
        }
    }
}