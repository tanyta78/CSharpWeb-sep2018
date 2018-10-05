namespace SIS.HTTP.Common
{
    public static class GlobalConstants
    {
        public const string HttpOneProtocolFragment = "HTTP/1.1";

        public const string HostHeaderKey = "Host";

        public const string CookieRequestHeaderName = "Cookie";

        public const string CookieResponseHeaderName = "Set-Cookie";

        public const string CookieSplitDelimiter = "; ";

        public const string PairSplitDelimiter = "=";

        public const char HttpRequestUrlQuerySeparator = '?';

        public const char HttpRequestUrlFragmentSeparator = '#';

        public const string HttpRequestHeaderNameValueSeparator = ": ";

        public const char HttpRequestParameterSeparator = '&';

        public const string NotSupportedStatusCodeExceptionMessage = "Status Code {0} not supported.";

    }
}
