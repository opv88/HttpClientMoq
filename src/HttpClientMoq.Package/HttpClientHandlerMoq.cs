using HttpClientMoq.Package.Exceptions;
using HttpClientMoq.Package.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HttpClientMoq.Package
{
    public sealed class HttpClientHandlerMoq : HttpMessageHandler
    {
        private readonly IReadOnlyDictionary<HttpClientHandlerConditionMoq, HttpClientHandlerResponseMoq> _httpClientHandlerData;
        private readonly HttpClientHandlerStatistics _httpClientHandlerStatistics;

        internal HttpClientHandlerMoq(Dictionary<HttpClientHandlerConditionMoq, HttpClientHandlerResponseMoq> httpClientHandlerData)
        {
            _httpClientHandlerData = httpClientHandlerData
                ?? throw new ArgumentNullException(nameof(httpClientHandlerData));

            _httpClientHandlerStatistics = new HttpClientHandlerStatistics();
        }

        public HttpClientHandlerStatistics HttpClientHandlerStatistics => _httpClientHandlerStatistics;

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var absoluteUrl = request.RequestUri.AbsoluteUri;

            var handlerQuery = _httpClientHandlerData
                .Where(x => x.Key.Url == absoluteUrl
                && x.Key.HttpMethod == request.Method);

            if (request.Content != null)
            {
                var contentBody = await request.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(contentBody))
                {
                    handlerQuery = handlerQuery.Where(x => x.Key.Body == contentBody);
                }
            }

            var handler = handlerQuery.SingleOrDefault();

            if (handler.Key is null)
            {
                throw new HttpClientHandlerMoqException(HttpClientHandlerMoqErrorMessages.HandlerForRequestNotFound);
            }

            if (handler.Value.Exception != null)
            {
                throw handler.Value.Exception;
            }

            var httpResponseMessage = new HttpResponseMessage(handler.Value.HttpStatusCode);
            if (handler.Value.HttpContent != null)
            {
                httpResponseMessage.Content = handler.Value.HttpContent;
            }

            _httpClientHandlerStatistics.AddVisitedUrl(
                handler.Key,
                request.RequestUri.AbsoluteUri);

            if (request.Content != null)
            {
                _httpClientHandlerStatistics.AddSentContentBody(
                    handler.Key,
                    await request.Content.ReadAsStringAsync() ?? "Empty content");
            }

            return httpResponseMessage;
        }
    }
}
