using System;

namespace HttpClientMoq.Package.Exceptions
{
    public sealed class HttpClientHandlerMoqException : Exception
    {
        public HttpClientHandlerMoqException() : base() { }

        public HttpClientHandlerMoqException(string message) : base(message) { }

        public HttpClientHandlerMoqException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
