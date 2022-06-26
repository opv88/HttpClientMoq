using HttpClientMoq.Package.Exceptions;

namespace HttpClientMoq.Package.Models
{
    public sealed class HttpClientVisitedData
    {
        public HttpClientVisitedData(HttpClientHandlerConditionMoq httpClientHandlerConditionMoq)
        {
            Condition = httpClientHandlerConditionMoq;
            Url = string.Empty;
            ContentBody = string.Empty;
        }

        public HttpClientHandlerConditionMoq Condition { get; init; }

        public string Url { get; private set; }

        public string ContentBody { get; private set; }

        public void SetUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new HttpClientHandlerMoqException($"{nameof(url)} is empty.");
            }

            Url = url;
        }

        public void SetContentBody(string contentBody)
        {
            if (string.IsNullOrWhiteSpace(contentBody))
            {
                throw new HttpClientHandlerMoqException($"{nameof(contentBody)} is empty.");
            }

            ContentBody = contentBody;
        }
    }
}
