namespace SIS.HTTP.Extensions
{
    using System;
    using Common;
    using Enums;

    public static class HttpResponseStatusExtensions
    {
        public static string GetResponseLine(this HttpResponseStatusCode httpResponseStatus)
        {
            return GetLineByCode((int) httpResponseStatus);
        }

        private static string GetLineByCode(int currentCode)
        {
            switch (currentCode)
            {
                case 200: return "200 OK";
                case 201: return "201 Created";
                case 301: return "301 Redirect";
                case 302: return "302 Found";
                case 303: return "303 See Other";
                case 400: return "400 Bad Request";
                case 401: return "401 Unauthorized";
                case 403: return "403 Forbidden";
                case 404: return "404 Not Found";
                case 500: return "500 Internal Server Error";
            }

            throw new NotSupportedException(string.Format(GlobalConstants.NotSupportedStatusCodeExceptionMessage, currentCode));
        }
    }
}
