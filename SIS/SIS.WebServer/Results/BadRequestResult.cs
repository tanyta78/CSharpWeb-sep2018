namespace SIS.WebServer.Results
{
    using System;
    using HTTP.Enums;
    using HTTP.Headers;
    using HTTP.Responses;
    using System.Text;

    public class BadRequestResult : HttpResponse
    {
        private const string DefaultErrorHeading = "<h1>Error occured, see details. </h1>";

        public BadRequestResult(string content) : base(HttpResponseStatusCode.BadRequest)
        {
            content = DefaultErrorHeading +Environment.NewLine+ content;
            this.Headers.Add(new HttpHeader("Content-Type", "text/html"));
            this.Content = Encoding.UTF8.GetBytes(content);
        }
    }
}
