namespace UrlValidation
{
    using System;
    using System.Net;
    using System.Text.RegularExpressions;

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello! Please enter encoded URL!");
            var encodedUrl = Console.ReadLine();
            var decodedUrl = WebUtility.UrlDecode(encodedUrl);
            Uri uri;
            bool isValid = Uri.TryCreate(decodedUrl, UriKind.Absolute, out uri);
            isValid = Uri.IsWellFormedUriString(decodedUrl, UriKind.Absolute);
            //var rgx = "^(ht|f)tp(s?)\\:\\/\\/[0-9a-zA-Z]([-.\\w]*[0-9a-zA-Z])*(:(0-9)*)*(\\/?)([a-zA-Z0-9\\-\\.\\?\\,\\\'\\/\\\\\\+&%\\$#_]*)?$";
            var rgx = "^((http|https):\\/\\/([0-9a-zA-Z\\-\\.]+)(\\:\\d+)?)(\\/([0-9a-zA-Z\\-\\.\\/]+)*)?(\\?[0-9a-zA-Z\\-\\.\\+\\=\\$_]+)?(\\#[0-9a-zA-Z\\-\\.]+)?$";

            if (!Regex.IsMatch(decodedUrl, rgx))
            {
                isValid = false;
            }

            if (!isValid)
            {
                Console.WriteLine("Invalid Url");
                return;
            }

            //extract url parts
            var protocol = uri.Scheme;
            var host = uri.Host;
            var port = uri.Port;
            var path = uri.AbsolutePath;
            var query = uri.Query;
            var fragment = uri.Fragment;

            //check requirements
            if (host == null || protocol == null || path == null)
            {
                Console.WriteLine("Invalid Url");
                return;
            }


            var result = $"Protocol: {protocol}" + Environment.NewLine +
                         $"Host: {host}" + Environment.NewLine +
                         $"Port: {port}" + Environment.NewLine +
                         $"Path: {path}" ;
            if (query != "")
            {
                result += Environment.NewLine + $"Query: {query} ";
            }

            if (fragment != "")
            {
                result += Environment.NewLine + $"Fragment: {fragment}";
            }
            Console.WriteLine(result);

        }


    }
}

