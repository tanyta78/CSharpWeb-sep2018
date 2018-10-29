namespace SIS.MvcFramework.Routing
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using HTTP.Enums;
    using HTTP.Headers;
    using HTTP.Requests.Contracts;
    using HTTP.Responses;
    using HTTP.Responses.Contracts;
    using Services;
    using WebServer.Results;
    using WebServer.Routing;

    public class RoutingEngine
    {
        public void RegisterRoutes(
            ServerRoutingTable routingTable,
            IMvcApplication application, 
            MvcFrameworkSettings settings,
            IServiceCollection serviceCollection)
        {
            //1. Register static files
            RegisterStaticFiles(routingTable, settings);

            //2. Register actions
            RegisterActions(routingTable, application, settings,  serviceCollection);

            //3. Register /
            RegisterDefaultRoute(routingTable);
        }

        private static void RegisterStaticFiles(
            ServerRoutingTable routingTable, 
            MvcFrameworkSettings settings)
        {
            var path = settings.WwwrootPath;
            var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                var filePath = file.Replace("\\", "/").Replace(settings.WwwrootPath, String.Empty);
                routingTable.Routes[HttpRequestMethod.Get][filePath] = (request) =>
                {
                    var content = System.IO.File.ReadAllText(file);
                    var contentType = "text/plain";

                    if (file.EndsWith(".css"))
                    {
                        contentType = "text/css";
                    }
                   else if (file.EndsWith(".js"))
                    {
                        contentType = "application/json";
                    }
                   else if (file.EndsWith(".png"))
                    {
                        contentType = "image/png";
                    }
                   else if (file.EndsWith(".jpg") || file.EndsWith(".jpeg"))
                    {
                        contentType = "image/jpeg";
                    }
                    else  if (file.EndsWith(".bmp"))
                    {
                        contentType = "image/bmp";
                    }
                    else if (file.EndsWith(".ico"))
                    {
                        contentType = "image/x-icon ";
                    }

                    return new TextResult(content, HttpResponseStatusCode.Ok, contentType);
                };
                Console.WriteLine($"Content registered:{file} => {HttpRequestMethod.Get} => {filePath}");
            }
        }

        private static void RegisterDefaultRoute(ServerRoutingTable routingTable)
        {
            if (!routingTable.Routes[HttpRequestMethod.Get].ContainsKey("/") && routingTable.Routes[HttpRequestMethod.Get].ContainsKey("/Home/Index"))
            {
                routingTable.Routes[HttpRequestMethod.Get]["/"] = (request) => routingTable.Routes[HttpRequestMethod.Get]["/Home/Index"](request);
                Console.WriteLine($"Route registered:reuse /Home/Index => {HttpRequestMethod.Get} => /");
            }
        }

        private static void RegisterActions(
            ServerRoutingTable routingTable,
            IMvcApplication application, 
            MvcFrameworkSettings settings,
            IServiceCollection serviceCollection)
        {
            var userCookieService = serviceCollection.CreateInstance<IUserCookieService>();

            var controllers = application.GetType().Assembly.GetTypes()
                 .Where(t => t.IsClass
                             && !t.IsAbstract
                             && t.IsSubclassOf(typeof(Controller)));

            foreach (var controller in controllers)
            {
                var getMethods = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

                foreach (var methodInfo in getMethods)
                {
                    var httpAttribute = (HttpAttribute)
                        methodInfo
                            .GetCustomAttributes(true)
                            .FirstOrDefault(ca =>
                            ca.GetType().IsSubclassOf(typeof(HttpAttribute)));

                    var method = HttpRequestMethod.Get;
                    string path = null;

                    if (httpAttribute != null)
                    {
                        path = httpAttribute.Path;
                        method = httpAttribute.Method;
                    }

                    if (path == null)
                    {
                        //If path is null => generate path from controller and action /ControllerName/ActionName
                        var controllerName = controller.Name;
                        if (controllerName.EndsWith("Controller"))
                        {
                            controllerName = controllerName.Substring(0, controllerName.Length - "Controller".Length);
                        }

                        var actionName = methodInfo.Name;

                        path = $"/{controllerName}/{actionName}";
                    }
                    else if (!path.StartsWith("/"))
                    {
                        path = "/" + path;
                    }

                    var hasAuthorizeAttribute = methodInfo
                            .GetCustomAttributes(true)
                            .Any(ca =>
                                ca.GetType()== typeof(AuthorizeAttribute));

                    routingTable.Add(method, path, (request) =>
                    {
                        // if (method has AuthorizeAttribute)
                        if (hasAuthorizeAttribute)
                        {
                            //get username Controller.GetUserData
                            var userData = Controller.GetUserData(request.Cookies, userCookieService);
                            // check if user is logged
                            if (userData==null)
                            {
                                //if not redirect to login page
                                var response = new HttpResponse();
                                response.Headers.Add(new HttpHeader(HttpHeader.Location, settings.LoginPageUrl));
                                response.StatusCode = HttpResponseStatusCode.SeeOther;
                                return response;
                            }
                        }
                        
                        return ExecuteAction(controller, methodInfo, request, serviceCollection);
                    });

                    Console.WriteLine($"Route registered:{controller.Name} {methodInfo.Name} => {method} => {path}");

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
            controllerInstance.ViewEngine = new ViewEngine.ViewEngine();
            //TODO: use serviceCollection
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
                    actionParametersObject.Add(ObjectMapper.TryParse(stringValue, actionParameter.ParameterType));
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
                        object value = ObjectMapper.TryParse(strValue, propertyInfo.PropertyType);

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
    }
}
