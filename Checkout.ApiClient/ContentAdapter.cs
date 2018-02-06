using Newtonsoft.Json;

namespace Checkout.ApiClient
{
    public static class ContentAdapter
    {
        public static string ConvertToJsonString(object model)
        {
            return  JsonConvert.SerializeObject(model);
        }

        public static T JsonStringToObject<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}