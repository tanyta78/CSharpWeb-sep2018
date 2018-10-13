﻿namespace SIS.MvcFramework
{
    using HTTP.Enums;
    using HTTP.Requests.Contracts;
    using HTTP.Responses.Contracts;
    using System;
    using System.Linq;
    using System.Reflection;
    using WebServer;
    using WebServer.Results;
    using WebServer.Routing;

    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {
            application.ConfigureServices();

            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();
            AutoRegisterRoutes(serverRoutingTable, application);

            application.Configure();
            Server server = new Server(80, serverRoutingTable);

            server.Run();
        }

        private static void AutoRegisterRoutes(ServerRoutingTable routingTable, IMvcApplication application)
        {
            var controllers = application.GetType().Assembly.GetTypes()
                 .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Controller)));
            foreach (var controller in controllers)
            {
                var getMethods = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                     .Where(m => m.CustomAttributes.Any(ca => ca.AttributeType.IsSubclassOf(typeof(HttpAttribute))));

                foreach (var methodInfo in getMethods)
                {
                    var httpAttribute = (HttpAttribute)
                        methodInfo.GetCustomAttributes(true).FirstOrDefault(ca =>
                            ca.GetType().IsSubclassOf(typeof(HttpAttribute)));

                    if (httpAttribute == null)
                    {
                        continue;
                    }

                    routingTable.Add(httpAttribute.Method, httpAttribute.Path, (request) => ExecuteAction(controller, methodInfo, request));

                    Console.WriteLine($"Route registered:{controller.Name} {methodInfo.Name} ");

                }
            }
        }

        private static IHttpResponse ExecuteAction(Type controllerType, MethodInfo methodInfo, IHttpRequest request)
        {
            //1.Create instance of controllerName
            var controllerInstance = Activator.CreateInstance(controllerType) as Controller;
            //2.Set request
            if (controllerInstance == null)
            {
                return new TextResult("Controller not found", HttpResponseStatusCode.InternalServerError);
            }
            controllerInstance.Request = request;

            //3.Invoke actionName
            //4. Return action result
            var httpResponse = methodInfo.Invoke(controllerInstance, new object[0]) as IHttpResponse;

            return httpResponse;
        }
    }
}