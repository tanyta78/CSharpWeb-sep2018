namespace CakesWebApp.Extensions
{
    using System.Net;

    public static class StringExtensions
    {
        public static string UrlDecode(this string str)
        {
            return WebUtility.UrlDecode(str);
        }
    }
}
