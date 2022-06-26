using System;
using System.Net.Http;

namespace HttpClientMoq.Package.Models
{
    public sealed class HttpClientHandlerConditionMoq
    {
        private Uri _uri;
        private string _body;
        private HttpMethod _httpMethod;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri">Request url.</param>
        /// <param name="body">Request data.</param>
        /// <param name="httpMethod">HTTP Standart request method.</param>
        public HttpClientHandlerConditionMoq(
            string uri,
            string body,
            HttpMethod? httpMethod)
        {
            _uri = new Uri(uri);
            _body = body;
            _httpMethod = httpMethod ?? HttpMethod.Get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">Request url.</param>
        public HttpClientHandlerConditionMoq(
            string uri)
            : this(uri, string.Empty, HttpMethod.Get)
        {
        }

        public string Url => _uri.AbsoluteUri;

        public string Body => _body;

        public HttpMethod HttpMethod => _httpMethod;
    }
}
