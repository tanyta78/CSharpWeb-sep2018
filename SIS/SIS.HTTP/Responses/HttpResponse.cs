﻿namespace SIS.HTTP.Responses
{
    using System.Linq;
    using System.Text;
    using Common;
    using Contracts;
    using Enums;
    using Extensions;
    using Headers;
    using Headers.Contracts;

    public class HttpResponse:IHttpResponse
    {
        public HttpResponse(){}

        public HttpResponse(HttpResponseStatusCode statusCode)
        {
            this.StatusCode = statusCode;
            this.Headers = new HttpHeaderCollection();
            this.Content = new byte[0];
        }


        public HttpResponseStatusCode StatusCode { get; set; }

        public IHttpHeaderCollection Headers { get; private set; }

        public byte[] Content { get; set; }

        public void AddHeader(HttpHeader header)
        {
           this.Headers.Add(header);
        }

        public byte[] GetBytes()
        {
            return Encoding.UTF8.GetBytes(this.ToString()).Concat(this.Content).ToArray();
        }

        public override string ToString()
        {
            var response = new StringBuilder();

            response
                .AppendLine($"{GlobalConstants.HttpOneProtocolFragment} {this.StatusCode.GetResponseLine()}")
                .AppendLine($"{this.Headers}")
                .AppendLine();

            return response.ToString();
        }

    }
}
