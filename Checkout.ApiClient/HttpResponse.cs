using System.Net;

namespace Checkout.ApiClient
{
    public class HttpResponse<T>
    {
        public bool HasError { get { return Error != null; } }
        public HttpStatusCode HttpStatusCode { get; set; }
        public ResponseError Error { get; set; }
        public T Model { get; set; }

        public HttpResponse(T model)
        {
            this.Model = model;
        }
    }
}
