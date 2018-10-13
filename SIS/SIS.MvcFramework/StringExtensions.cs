namespace SIS.MvcFramework
{
    using System.Net;

    public static class StringExtensions
    {
        public static string UrlDecode(this string str)
        {
            return WebUtility.UrlDecode(str);
        }

        public static decimal ToDecimalOrDefault(this string str)
        {
            if (decimal.TryParse(str, out var result))
            {
                return result;
            }

            return default(decimal);
        }
    }
}
