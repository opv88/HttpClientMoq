using HttpClientMoq.Package.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace HttpClientMoq.Package
{
    public sealed class HttpClientHandlerMoqBuilder
    {
        private Dictionary<HttpClientHandlerConditionMoq, HttpClientHandlerResponseMoq> _httpClientHandlerData;

        public HttpClientHandlerMoqBuilder()
        {
            _httpClientHandlerData = new Dictionary<HttpClientHandlerConditionMoq, HttpClientHandlerResponseMoq>();
        }

        public HttpClientHandlerMoqBuilder(
            string url,
            string responseStringData) : this(
                new HttpClientHandlerConditionMoq(url),
                new HttpClientHandlerResponseMoq(HttpStatusCode.OK, new StringContent(responseStringData)))
        {
        }

        public HttpClientHandlerMoqBuilder(
            string url,
            HttpStatusCode httpStatusCode,
            string responseStringData): this(
                new HttpClientHandlerConditionMoq(url),
                new HttpClientHandlerResponseMoq(httpStatusCode, new StringContent(responseStringData)))
        {
        }

        public HttpClientHandlerMoqBuilder(
            string url,
            HttpStatusCode httpStatusCode,
            HttpContent httpContent): this (
                new HttpClientHandlerConditionMoq(url),
                new HttpClientHandlerResponseMoq(httpStatusCode, httpContent))
        {
        }

        public HttpClientHandlerMoqBuilder(
            HttpMethod httpMethod,
            string url,
            HttpStatusCode httpStatusCode,
            string responseStringData) : this(
                new HttpClientHandlerConditionMoq(url, string.Empty, httpMethod),
                new HttpClientHandlerResponseMoq(httpStatusCode, new StringContent(responseStringData)))
        {
        }

        public HttpClientHandlerMoqBuilder(
            HttpMethod httpMethod,
            string url,
            HttpStatusCode httpStatusCode,
            HttpContent httpContent): this(
                new HttpClientHandlerConditionMoq(url, string.Empty, httpMethod),
                new HttpClientHandlerResponseMoq(httpStatusCode, httpContent))
        {
        }

        public HttpClientHandlerMoqBuilder(
            HttpMethod httpMethod,
            string url,
            string body,
            HttpStatusCode httpStatusCode,
            string responseStringData): this(
                new HttpClientHandlerConditionMoq(url, body, httpMethod),
                new HttpClientHandlerResponseMoq(httpStatusCode, new StringContent(responseStringData)))
        {
        }

        public HttpClientHandlerMoqBuilder(
            HttpMethod httpMethod,
            string url,
            string body,
            HttpStatusCode httpStatusCode,
            HttpContent httpContent): this(
                new HttpClientHandlerConditionMoq(url, body, httpMethod),
                new HttpClientHandlerResponseMoq(httpStatusCode, httpContent))
        {
        }

        public HttpClientHandlerMoqBuilder(
            HttpClientHandlerConditionMoq httpClientHandlerConditionMoq,
            HttpClientHandlerResponseMoq httpClientHandlerResponseMoq)
        {
            _httpClientHandlerData = new Dictionary<HttpClientHandlerConditionMoq, HttpClientHandlerResponseMoq>();
            _ = SetupRequest(httpClientHandlerConditionMoq, httpClientHandlerResponseMoq);
        }

        public HttpClientHandlerMoqBuilder SetupRequest(
            HttpClientHandlerConditionMoq httpClientHandlerConditionMoq,
            HttpClientHandlerResponseMoq httpClientHandlerResponseMoq)
        {
            _httpClientHandlerData.Add(httpClientHandlerConditionMoq, httpClientHandlerResponseMoq);

            return this;
        }

        public HttpClientHandlerMoqBuilder SetupRequest(
            string url,
            string responseStringData) =>
            SetupRequest(
                new HttpClientHandlerConditionMoq(url),
                new HttpClientHandlerResponseMoq(HttpStatusCode.OK, new StringContent(responseStringData)));

        public HttpClientHandlerMoqBuilder SetupRequest(
            string url,
            HttpStatusCode httpStatusCode,
            string responseStringData) =>
            SetupRequest(
                new HttpClientHandlerConditionMoq(url),
                new HttpClientHandlerResponseMoq(httpStatusCode, new StringContent(responseStringData)));

        public HttpClientHandlerMoqBuilder SetupRequest(
            string url,
            HttpStatusCode httpStatusCode,
            HttpContent httpContent) =>
            SetupRequest(
                new HttpClientHandlerConditionMoq(url),
                new HttpClientHandlerResponseMoq(httpStatusCode, httpContent));

        public HttpClientHandlerMoqBuilder SetupRequest(
            HttpMethod httpMethod,
            string url,
            HttpStatusCode httpStatusCode,
            string responseStringData) =>
            SetupRequest(
                new HttpClientHandlerConditionMoq(url, string.Empty, httpMethod),
                new HttpClientHandlerResponseMoq(httpStatusCode, new StringContent(responseStringData)));

        public HttpClientHandlerMoqBuilder SetupRequest(
            HttpMethod httpMethod,
            string url,
            HttpStatusCode httpStatusCode,
            HttpContent httpContent) =>
            SetupRequest(
                new HttpClientHandlerConditionMoq(url, string.Empty, httpMethod),
                new HttpClientHandlerResponseMoq(httpStatusCode, httpContent));

        public HttpClientHandlerMoqBuilder SetupRequest(
            HttpMethod httpMethod,
            string url,
            string body,
            HttpStatusCode httpStatusCode,
            string responseStringData) =>
            SetupRequest(
                new HttpClientHandlerConditionMoq(url, body, httpMethod),
                new HttpClientHandlerResponseMoq(httpStatusCode, new StringContent(responseStringData)));

        public HttpClientHandlerMoqBuilder SetupRequest(
            HttpMethod httpMethod,
            string url,
            string body,
            HttpStatusCode httpStatusCode,
            HttpContent httpContent) =>
            SetupRequest(
                new HttpClientHandlerConditionMoq(url, body, httpMethod),
                new HttpClientHandlerResponseMoq(httpStatusCode, httpContent));

        public HttpClientHandlerMoqBuilder SetupRequest(
            string url,
            string body,
            HttpMethod httpMethod,
            HttpStatusCode httpStatusCode,
            HttpContent httpContent) =>
            SetupRequest(
                new HttpClientHandlerConditionMoq(url, body, httpMethod),
                new HttpClientHandlerResponseMoq(httpStatusCode, httpContent));

        public HttpClientHandlerMoq Build() => new HttpClientHandlerMoq(_httpClientHandlerData);
    }
}
