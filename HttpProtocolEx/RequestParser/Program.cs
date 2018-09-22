using System;

namespace RequestParser
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    class Program
    {
        static void Main(string[] args)
        {
            
            var routesByMethod = new Dictionary<string, HashSet<string>>();

            var input = Console.ReadLine().ToLower();

            //receiving valid paths
            while (input != "END")
            {
                var inputArgs = input.Split("/", StringSplitOptions.RemoveEmptyEntries);

                var httpMethod = inputArgs[1];
                var endPoint = inputArgs[0];

                if (!routesByMethod.ContainsKey(httpMethod))
                {
                    routesByMethod.Add(httpMethod, new HashSet<string>());
                }

                routesByMethod[httpMethod].Add(endPoint);
                input = Console.ReadLine();
            }

            //receive HTTP request 
            var requestInput = Console.ReadLine().ToLower();

            var requestParts = requestInput.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var reqHttpMethod = requestParts[0];
            var reqEndpoint = requestParts[1].Trim('/');
            var reqHttpProtocol = requestParts[2].ToUpper();

            if (routesByMethod.ContainsKey(reqHttpMethod))
            {
                var route = routesByMethod[reqHttpMethod].FirstOrDefault(r => r == reqEndpoint);
                var httpResponse = "";
                if (route != null)
                {
                    httpResponse = $"{reqHttpProtocol} {(int)HttpStatusCode.OK} OK" + Environment.NewLine +
                                       $"Content-Length: {HttpStatusCode.OK.ToString().Length}" +
                                       Environment.NewLine +
                                       "Content-Type: text/plain" +
                                       Environment.NewLine +
                                       Environment.NewLine +
                                       $"{HttpStatusCode.OK}";


                }
                else
                {
                    httpResponse = $"{reqHttpProtocol} {(int)HttpStatusCode.NotFound} NotFound" + Environment.NewLine +
                                   $"Content-Length: {HttpStatusCode.NotFound.ToString().Length}" +
                                   Environment.NewLine +
                                   "Content-Type: text/plain" +
                                   Environment.NewLine +
                                   Environment.NewLine +
                                   $"{HttpStatusCode.NotFound}";
                }

                Console.WriteLine(httpResponse);
            }
        }
    }
}
