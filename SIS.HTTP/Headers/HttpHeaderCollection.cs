namespace SIS.HTTP.Headers
{
    using System;
    using System.Collections.Generic;
    using SIS.HTTP.Exceptions;
    using SIS.HTTP.Headers.Contracts;

    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly Dictionary<string, HttpHeader> headers;

        public HttpHeaderCollection()
        {
            this.headers = new Dictionary<string, HttpHeader>();
        }

        public void Add(HttpHeader header)
        {
            if (string.IsNullOrEmpty(header.Key) || string.IsNullOrEmpty(header.Value))
            {
                throw new BadRequestException();
            }

            this.headers[header.Key] = header;
        }

        public bool ContainsHeader(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new BadRequestException();
            }

            return this.headers.ContainsKey(key);
        }

        public HttpHeader GetHeader(string key)
        {
            if (!this.ContainsHeader(key))
            {
                return null;
            }

            return this.headers[key];
        }

        public override string ToString() => string.Join(Environment.NewLine, this.headers.Values);
    }
}
