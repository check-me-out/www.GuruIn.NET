using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebFramework.App_Start
{
    public class WebApiMessageHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Call the inner handler.
            var response = base.SendAsync(request, cancellationToken);

            if (response.Result.StatusCode == HttpStatusCode.OK)
            {
                var ok = response.Result.Content.ReadAsStringAsync();
                var okResponse = new HttpResponseMessage(HttpStatusCode.OK)
                                            {
                                                Content = new StringContent(ok.Result),
                                                RequestMessage = request,
                                                ReasonPhrase = "OK"
                                            };

                return Task<HttpResponseMessage>.Factory.StartNew(() => okResponse);
            }
            else if (response.Result.StatusCode == HttpStatusCode.NotFound)
            {
                var notFoundResponse = new HttpResponseMessage(HttpStatusCode.NotFound)
                                            {
                                                Content = new StringContent("Unable to find \"" + request.RequestUri + "\" resource. Please check your URI."),
                                                RequestMessage = request,
                                                ReasonPhrase = "Resource Not Found"
                                            };

                return Task<HttpResponseMessage>.Factory.StartNew(() => notFoundResponse);
            }
            else if (response.Result.StatusCode == HttpStatusCode.InternalServerError)
            {
                var exceptionDetails = response.Result.Content.ReadAsStringAsync();
                var errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                                            {
                                                Content = new StringContent(exceptionDetails.Result),
                                                RequestMessage = request,
                                                ReasonPhrase = "Internal Server Error"
                                            };

                return Task<HttpResponseMessage>.Factory.StartNew(() => errorResponse);
            }
            else
            {
                return response;
            }
        }
    }
}