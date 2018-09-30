namespace SIS.HTTP.Headers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;

    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly Dictionary<string, HttpHeader> headers;

        public HttpHeaderCollection()
        {
            this.headers = new Dictionary<string, HttpHeader>();
        }

        public void Add(HttpHeader header)
        {
            if (header == null ||
                string.IsNullOrEmpty(header.Key) ||
                string.IsNullOrEmpty(header.Value) ||
                this.ContainsHeader(header.Key))
            {
                //TO DO: add custom exceptions
                throw new Exception();
            }

            this.headers.Add(header.Key, header);
        }

        public bool ContainsHeader(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException($"{nameof(key)} can not be null!");
            }

            return this.headers.ContainsKey(key);
        }

        public HttpHeader GetHeader(string key)
        {
            if (this.ContainsHeader(key))
            {
                return this.headers[key];
            }
            else
            {
                return null;
            }

        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this.headers.Values.Select(e => e.ToString()).ToArray());
        }
    }
}
