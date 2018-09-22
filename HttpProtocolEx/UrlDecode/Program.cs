using System;

namespace UrlDecode
{
    using System.Net;

    public class Program
    {
       public static void Main(string[] args)
        {
            Console.WriteLine("Hello! Please enter encoded URL!");
            var encodedUrl = Console.ReadLine();
            var decodedUrl = WebUtility.UrlDecode(encodedUrl);
            Console.WriteLine("This is your decoded version!");
            Console.WriteLine(decodedUrl);
        }
    }
}
