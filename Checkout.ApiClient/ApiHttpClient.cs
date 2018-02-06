using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.ApiClient
{
    public class ApiHttpClient
    {
        private const string jsonContentType = "application/json";

        public Task<HttpResponse<T>> PostRequest<T>(string requestUri, object requestPayload = null)
        {
            var httpRequestMsg = new HttpRequestMessage(HttpMethod.Post, requestUri);
            var requestPayloadAsString = GetObjectAsString(requestPayload);

            httpRequestMsg.Content = new StringContent(requestPayloadAsString, Encoding.UTF8, jsonContentType);
            httpRequestMsg.Headers.Add("Accept", jsonContentType);

            return SendRequestAsync<T>(httpRequestMsg);
        }

        public Task<HttpResponse<T>> DeleteRequest<T>(string requestUri, object requestPayload = null)
        {
            var httpRequestMsg = new HttpRequestMessage(HttpMethod.Delete, requestUri);
            var requestPayloadAsString = GetObjectAsString(requestPayload);

            httpRequestMsg.Content = new StringContent(requestPayloadAsString, Encoding.UTF8, jsonContentType);
            httpRequestMsg.Headers.Add("Accept", jsonContentType);

            return SendRequestAsync<T>(httpRequestMsg);
        }

        public Task<HttpResponse<T>> GetRequest<T>(string requestUri)
        {
            var httpRequestMsg = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUri)
            };
            httpRequestMsg.Headers.Add("Accept", jsonContentType);

            return SendRequestAsync<T>(httpRequestMsg);
        }

        private string GetObjectAsString(object requestModel)
        {
            return ContentAdapter.ConvertToJsonString(requestModel);
        }

        private async Task<HttpResponse<T>> SendRequestAsync<T>(HttpRequestMessage request)
        {
            Task<HttpResponse<T>> response = null;
            HttpResponseMessage responseMessage = null;
            string responseAsString = null;
            string responseCode = null;

            HttpClient httpClient = new HttpClient();

            try
            {
                responseMessage = await httpClient.SendAsync(request).ConfigureAwait(false);

                responseCode = responseMessage.StatusCode.ToString();

                var responseContent = await responseMessage.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

                if (responseContent?.Length > 0)
                {
                    responseAsString = Encoding.UTF8.GetString(responseContent);
                }

                response = Task.FromResult(CreateHttpResponse<T>(responseAsString, responseMessage.StatusCode));
            }
            catch (Exception ex)
            {
                responseCode = "Exception" + ex.Message;

                throw;
            }
            finally
            {
                request.Dispose();
                httpClient?.Dispose();
            }

            return await response;
        }

        private HttpResponse<T> CreateHttpResponse<T>(string responseAsString, HttpStatusCode httpStatusCode)
        {
            if (httpStatusCode == HttpStatusCode.OK && responseAsString != null)
            {
                return new HttpResponse<T>(GetResponseAsObject<T>(responseAsString))
                {
                    HttpStatusCode = httpStatusCode
                };
            }
            else if (httpStatusCode == HttpStatusCode.Created && responseAsString != null)
            {
                return new HttpResponse<T>(GetResponseAsObject<T>(responseAsString))
                {
                    HttpStatusCode = httpStatusCode
                };
            }
            else if (httpStatusCode == HttpStatusCode.NoContent)
            {
                return new HttpResponse<T>(default(T))
                {
                    HttpStatusCode = httpStatusCode
                };
            }
            else if (responseAsString != null)
            {
                return new HttpResponse<T>(default(T))
                {
                    Error = GetResponseAsObject<ResponseError>(responseAsString),
                    HttpStatusCode = httpStatusCode
                };
            }

            return null;
        }

        private T GetResponseAsObject<T>(string responseAsString)
        {
            return ContentAdapter.JsonStringToObject<T>(responseAsString);
        }
    }
}
