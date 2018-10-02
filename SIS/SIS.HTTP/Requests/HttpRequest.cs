namespace SIS.HTTP.Requests
{
    using Common;
    using Contacts;
    using Cookies;
    using Cookies.Contracts;
    using Enums;
    using Exceptions;
    using Headers;
    using Headers.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Sessions.Contracts;


    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            this.FormData = new Dictionary<string, object>();
            this.QueryData = new Dictionary<string, object>();
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();

            this.ParseRequest(requestString);
        }

        public string Path { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public IHttpCookieCollection Cookies { get; }

        public IHttpHeaderCollection Headers { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        public IHttpSession Session { get; set; }

        private void ParseRequest(string requestString)
        {
            string[] reqContent = requestString.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            string[] reqLine = reqContent[0].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (!this.IsValidRequestLine(reqLine))
            {
                throw new BadRequestException();
            }

            this.ParseRequestMethod(reqLine);
            this.ParseRerquestUrl(reqLine);
            this.ParseRequestPath();

            this.ParseHeaders(reqContent.Skip(1).ToArray());
            this.ParseCookies();

            //bool hasBody = reqContent.Length > 1; always true ?!?
            this.ParseRequestParameters(reqContent[reqContent.Length - 1]);
        }

        private void ParseCookies()
        {
            if (!this.Headers.ContainsHeader(GlobalConstants.CookieRequestHeaderName))
            {
                return;
            }

            var cookiesData = this.Headers.GetHeader(GlobalConstants.CookieRequestHeaderName).Value;
          
            if (string.IsNullOrEmpty(cookiesData))
            {
                return;
            }
            var cookies = cookiesData.Split(GlobalConstants.CookieSplitDelimiter, StringSplitOptions.RemoveEmptyEntries);

            foreach (var cookiePair in cookies)
            {
                var cookieInfo = cookiePair.Split(GlobalConstants.PairSplitDelimiter, 2);

                if (cookieInfo.Length != 2)
                {
                    throw new BadRequestException();
                }

                var cookieKey = cookieInfo[0];
                var cookieValue = cookieInfo[1];
                
                this.Cookies.Add(new HttpCookie(cookieKey, cookieValue));
            }

        }

        private bool IsValidRequestQueryString(string queryString, string[] queryParams)
        {
            return !string.IsNullOrEmpty(queryString) && queryString.Contains('=') && queryParams.Length >= 1;
        }

        private bool IsValidRequestLine(string[] reqLine)
        {
            if (!reqLine.Any())
            {
                throw new BadRequestException();
            }

            return reqLine.Length == 3 && reqLine[2].Equals(GlobalConstants.HttpOneProtocolFragment);
        }

        private void ParseRequestMethod(string[] reqLine)
        {
            var parseResult = Enum.TryParse(reqLine[0], true, out HttpRequestMethod httpRequestMethod);

            if (!parseResult)
            {
                throw new BadRequestException();
            }

            this.RequestMethod = httpRequestMethod;
        }

        private void ParseRerquestUrl(string[] reqLine)
        {
            if (string.IsNullOrEmpty(reqLine[1]))
            {
                throw new BadRequestException();
            }

            this.Url = reqLine[1];
        }

        private void ParseRequestPath()
        {
            var path = this.Url.Split(new[] { '?', '#' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();

            if (string.IsNullOrEmpty(path))
            {
                throw new BadRequestException();
            }

            this.Path = path;
        }

        private void ParseHeaders(string[] headers)
        {
            if (!headers.Any())
            {
                throw new BadRequestException();
            }

            foreach (var headerInfo in headers)
            {
                if (string.IsNullOrEmpty(headerInfo))
                {
                    break;
                }

                var headerParts = headerInfo.Split(": ", StringSplitOptions.RemoveEmptyEntries);

                var headerObject = new HttpHeader(headerParts[0], headerParts[1]);

                this.Headers.Add(headerObject);
            }

            if (!this.Headers.ContainsHeader("Host"))
            {
                throw new BadRequestException();
            }


        }

        private void ParseFormDataParameters(string reqBody)
        {
            /*
             * Splits the Request’s Body into different parameters, and maps each of them into the Form Data Dictionary.
             * Does nothing if the Request contains NO Body.
             */

            if (this.RequestMethod == HttpRequestMethod.Get)
            {
                return;
            }

            //Splits the Request’s Body into different parameters
            var formDataPairs = reqBody.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

            //maps each of them into the Form Data Dictionary
            this.MapReqParamsToDictionary(formDataPairs, this.FormData);

        }

        private void MapReqParamsToDictionary(string[] paramsPairs, Dictionary<string, object> paramsDictionary)
        {
            foreach (var paramPair in paramsPairs)
            {
                var paramInfo = paramPair.Split(GlobalConstants.PairSplitDelimiter, StringSplitOptions.RemoveEmptyEntries);

                if (paramInfo.Length != 2)
                {
                    throw new BadRequestException();
                }

                var paramKey = WebUtility.UrlDecode(paramInfo[0]);
                var paramValue = WebUtility.UrlDecode(paramInfo[1]);

                //overwrite?!? && key=>key,{key,value}?!?
                if (!paramsDictionary.ContainsKey(paramKey))
                {
                    paramsDictionary.Add(paramKey, paramValue);
                }
                else
                {
                    paramsDictionary[paramKey] = paramValue;
                }

            }
        }

        private void ParseQueryParameters()
        {
            if (!this.Url.Contains("?"))
            {
                return;
            }

            var queryParams = this.Url
                .Split(new[] { '?', '#' })
                .Skip(1)
                .ToArray()[0];

            // username=pesho&pass=133

            //Does nothing if the Request’s Url contains NO Query string.
            if (!string.IsNullOrEmpty(queryParams))
            {
                return;
            }

            //Then splits the Query string into different parameters, and maps each of them into the Query Data Dictionary.
            var queryPairs = queryParams.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

            //Validates the Query string and parameters by calling the IsValidRequestQueryString() method.Throws a BadRequestException if the Query string is invalid.
            if (this.IsValidRequestQueryString(queryParams, queryPairs))
            {
                throw new BadRequestException();
            };

            //Then splits the Query string into different parameters, and maps each of them into the Query Data Dictionary.
            this.MapReqParamsToDictionary(queryPairs, this.QueryData);
        }

        private void ParseRequestParameters(string reqBody)
        {

            this.ParseQueryParameters();

            this.ParseFormDataParameters(reqBody);

        }


    }
}
