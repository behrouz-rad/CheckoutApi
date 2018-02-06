using Newtonsoft.Json;

namespace Checkout.ApiClient.Console
{
    public static class JsonHelper
    {
        public static string ConvertToJsonString(object model)
        {
            return JsonConvert.SerializeObject(model);
        }
    }
}