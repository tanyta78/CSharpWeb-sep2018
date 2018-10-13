﻿namespace SIS.MvcFramework
{
    using HTTP.Enums;
    using HTTP.Requests.Contracts;
    using HTTP.Responses.Contracts;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using WebServer;
    using WebServer.Results;
    using WebServer.Routing;

    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {
            var dependencyContainer = new ServiceCollection();
            application.ConfigureServices(dependencyContainer);

            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();
            AutoRegisterRoutes(serverRoutingTable, application, dependencyContainer);

            application.Configure();
            Server server = new Server(80, serverRoutingTable);

            server.Run();
        }

        private static void AutoRegisterRoutes(ServerRoutingTable routingTable, IMvcApplication application, IServiceCollection serviceCollection)
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

                    routingTable.Add(httpAttribute.Method, httpAttribute.Path, (request) => ExecuteAction(controller, methodInfo, request, serviceCollection));

                    Console.WriteLine($"Route registered:{controller.Name} {methodInfo.Name} ");

                }
            }
        }

        private static IHttpResponse ExecuteAction(Type controllerType, MethodInfo methodInfo, IHttpRequest request, IServiceCollection serviceCollection)
        {
            //1.Create instance of controllerName
            var controllerInstance = serviceCollection.CreateInstance(controllerType) as Controller;
            //2.Set request
            if (controllerInstance == null)
            {
                return new TextResult("Controller not found", HttpResponseStatusCode.InternalServerError);
            }
            controllerInstance.Request = request;
            controllerInstance.UserCookieService = serviceCollection.CreateInstance<IUserCookieService>();

            var actionParameters = methodInfo.GetParameters();
            var actionParametersObject = new List<object>();
            foreach (var actionParameter in actionParameters)
            {
                var instance = serviceCollection.CreateInstance(actionParameter.ParameterType);

                // populate from request instance properties
                var properties = actionParameter.ParameterType.GetProperties();
                foreach (var propertyInfo in properties)
                {
                    //TODO: Support IEnumerable
                    var key = propertyInfo.Name.ToLower();
                    object value = null;

                    if (request.FormData.Any(x => x.Key.ToLower() == key))
                    {
                        value = request.FormData.First(x => x.Key.ToLower() == key).Value.ToString().UrlDecode();
                    }
                    else if (request.QueryData.Any(x => x.Key.ToLower() == key))
                    {
                        value = request.QueryData.First(x => x.Key.ToLower() == key).Value.ToString().UrlDecode();

                    }

                    propertyInfo.SetMethod.Invoke(instance, new object[] { value });

                }

                actionParametersObject.Add(instance);
            }
            //3.Invoke actionName
            //4. Return action result
            var httpResponse = methodInfo.Invoke(controllerInstance, actionParametersObject.ToArray()) as IHttpResponse;

            return httpResponse;
        }
    }
}
