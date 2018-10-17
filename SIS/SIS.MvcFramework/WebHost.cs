namespace SIS.MvcFramework
{
    using HTTP.Enums;
    using HTTP.Requests.Contracts;
    using HTTP.Responses.Contracts;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using WebServer;
    using WebServer.Results;
    using WebServer.Routing;

    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

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

            //3.Get parameters for action
            List<object> actionParametersObject = GetActionParameterObjects(methodInfo, request, serviceCollection);

            //3.Invoke actionName
            //4. Return action result
            var httpResponse = methodInfo.Invoke(controllerInstance, actionParametersObject.ToArray()) as IHttpResponse;

            return httpResponse;
        }

        private static List<object> GetActionParameterObjects(MethodInfo methodInfo, IHttpRequest request, IServiceCollection serviceCollection)
        {
            var actionParameters = methodInfo.GetParameters();
            var actionParametersObject = new List<object>();
            foreach (var actionParameter in actionParameters)
            {
                //TODO: improve this check
                if (actionParameter.ParameterType.IsValueType
                    || Type.GetTypeCode(actionParameter.ParameterType) == TypeCode.String)
                {
                    var stringValue = GetRequestData(request, actionParameter.Name);
                    actionParametersObject.Add(TryParse(stringValue, actionParameter.ParameterType));
                }
                else
                {
                    var instance = serviceCollection.CreateInstance(actionParameter.ParameterType);

                    // populate from request instance properties
                    var properties = actionParameter.ParameterType.GetProperties();
                    foreach (var propertyInfo in properties)
                    {
                        //TODO: Support IEnumerable
                        string strValue = GetRequestData(request, propertyInfo.Name);

                        //-> check type of propertyInfo and set it correctly
                        //-> int, double, long, decimal, DateTime

                        // can be simplified by using Convert.ChangeType()
                        object value = TryParse(strValue, propertyInfo.PropertyType);

                        propertyInfo.SetMethod.Invoke(instance, new object[] { value });

                    }
                    actionParametersObject.Add(instance);
                }
            }
            return actionParametersObject;
        }

        private static string GetRequestData(IHttpRequest request, string key)
        {
            key = key.ToLower();
            string strValue = null;

            if (request.FormData.Any(x => x.Key.ToLower() == key))
            {
                strValue = request.FormData.First(x => x.Key.ToLower() == key).Value.ToString().UrlDecode();
            }
            else if (request.QueryData.Any(x => x.Key.ToLower() == key))
            {
                strValue = request.QueryData.First(x => x.Key.ToLower() == key).Value.ToString().UrlDecode();

            }

            return strValue;
        }

        private static object TryParse(string strValue, Type type)
        {
            var typeCode = Type.GetTypeCode(type);
            object value = null;
            switch (typeCode)
            {
                case TypeCode.Int32:
                    if (int.TryParse(strValue, out var intResult))
                    {
                        value = intResult;
                    }
                    break;
                case TypeCode.Int64:
                    if (long.TryParse(strValue, out var longResult))
                    {
                        value = longResult;
                    }
                    break;
                case TypeCode.Double:
                    if (double.TryParse(strValue, out var doubleResult))
                    {
                        value = doubleResult;
                    }
                    break;
                case TypeCode.Decimal:
                    if (decimal.TryParse(strValue, out var decimalResult))
                    {
                        value = decimalResult;
                    }
                    break;
                case TypeCode.Char:
                    if (char.TryParse(strValue, out var charResult))
                    {
                        value = charResult;
                    }
                    break;
                case TypeCode.DateTime:
                    if (DateTime.TryParse(strValue, out var dateResult))
                    {
                        value = dateResult;
                    }
                    break;
                case TypeCode.String:
                    value = strValue;
                    break;
            }

            return value;
        }
    }
}
