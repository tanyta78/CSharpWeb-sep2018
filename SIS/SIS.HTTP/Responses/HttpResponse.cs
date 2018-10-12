namespace SIS.HTTP.Responses
{
    using Common;
    using Contracts;
    using Cookies;
    using Cookies.Contracts;
    using Enums;
    using Extensions;
    using Headers;
    using Headers.Contracts;
    using System.Linq;
    using System.Text;

    public class HttpResponse : IHttpResponse
    {
        public HttpResponse()
        {
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();
            this.Content = new byte[0];
        }

        public HttpResponse(HttpResponseStatusCode statusCode) : this()
        {
            CoreValidator.ThrowIfNull(statusCode, nameof(statusCode));
            this.StatusCode = statusCode;

        }


        public HttpResponseStatusCode StatusCode { get; set; }

        public IHttpHeaderCollection Headers { get; private set; }

        public IHttpCookieCollection Cookies { get; }

        public byte[] Content { get; set; }

        public void AddHeader(HttpHeader header)
        {
            CoreValidator.ThrowIfNull(header, nameof(header));
            this.Headers.Add(header);
        }

        public void AddCookie(HttpCookie cookie)
        {
            CoreValidator.ThrowIfNull(cookie, nameof(cookie));
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
                .Append($"{GlobalConstants.HttpOneProtocolFragment} {(int)this.StatusCode} {this.StatusCode.ToString()}")
                .Append(GlobalConstants.HttpNewLine)
                .Append($"{this.Headers}")
                .Append(GlobalConstants.HttpNewLine);

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
