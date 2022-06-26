using System;
using System.Net;
using System.Net.Http;

namespace HttpClientMoq.Package.Models
{
    public sealed class HttpClientHandlerResponseMoq
    {
        private readonly HttpStatusCode? _httpStatusCode;
        private readonly HttpContent? _httpContent;
        private readonly Exception? _exception;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpStatusCode"><see cref="HttpStatusCode"/></param>
        /// <param name="httpContent"><see cref="HttpContent"/></param>
        public HttpClientHandlerResponseMoq(
            HttpStatusCode? httpStatusCode,
            HttpContent? httpContent)
        {
            _httpStatusCode = httpStatusCode ?? throw new ArgumentNullException(nameof(httpStatusCode));
            _httpContent = httpContent ?? throw new ArgumentNullException(nameof(httpContent));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception">Exception </param>
        public HttpClientHandlerResponseMoq(Exception exception)
        {
            _exception = exception;
        }

        public HttpStatusCode HttpStatusCode => (_exception is null) ? _httpStatusCode!.Value : throw new ArgumentNullException(nameof(_httpStatusCode));
        public HttpContent HttpContent => (_exception is null) ? _httpContent! : throw new ArgumentNullException(nameof(_httpContent));
        public Exception? Exception => _exception;
    }
}
