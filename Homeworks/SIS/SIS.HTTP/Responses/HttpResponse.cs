﻿namespace SIS.HTTP.Responses
{
    using System.Linq;
    using System.Text;
    using Common;
    using Contracts;
    using Cookies;
    using Cookies.Contracts;
    using Enums;
    using Extensions;
    using Headers;
    using Headers.Contracts;

    public class HttpResponse:IHttpResponse
    {
        public HttpResponse(){}

        public HttpResponse(HttpResponseStatusCode statusCode)
        {
            CoreValidator.ThrowIfNull(statusCode,nameof(statusCode));

            this.StatusCode = statusCode;
            this.Headers = new HttpHeaderCollection();
            this.Cookies=new HttpCookieCollection();
            this.Content = new byte[0];
        }


        public HttpResponseStatusCode StatusCode { get; set; }

        public IHttpHeaderCollection Headers { get; private set; }

        public IHttpCookieCollection Cookies { get; }

        public byte[] Content { get; set; }

        public void AddHeader(HttpHeader header)
        {
            CoreValidator.ThrowIfNull(header,nameof(header));
           this.Headers.Add(header);
        }

        public void AddCookie(HttpCookie cookie)
        {
            CoreValidator.ThrowIfNull(cookie,nameof(cookie));
            this.Cookies.Add(cookie);
        }

        public byte[] GetBytes()
        {
            return Encoding.UTF8
                .GetBytes(this.ToString())
                .Concat(this.Content)
                .ToArray();
        }

        public override string ToString()
        {
            var response = new StringBuilder();

            response
                .AppendLine($"{GlobalConstants.HttpOneProtocolFragment} {this.StatusCode.GetResponseLine()}")
                .AppendLine($"{this.Headers}");

            if (this.Cookies.HasCookies())
            {
                foreach (var cookie in this.Cookies)
                {
                    response.AppendLine($"{GlobalConstants.CookieResponseHeaderName}: {cookie}");
                }
               
            }

            response.AppendLine();

            return response.ToString();
        }

    }
}
