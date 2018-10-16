namespace SIS.Framework.Routers
{
    using ActionResults;
    using Attributes.Methods;
    using Controllers;
    using HTTP.Enums;
    using HTTP.Extensions;
    using HTTP.Responses;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using WebServer.Api;
    using WebServer.Results;

    public class ControllerRouter : IHttpHandler
    {
        public IHttpResponse Handle(IHttpRequest request)
        {
            //1.Read the Request Data
            //-Extract the request method * to be able to search for suitable actions
            //-Extract the Controller name * using the Request Path(/Home/Index) and controller suffix  controller name would be HomeController
            //-Extract the Action name * using the Request Path(/Home/Index) action name would be Index
            //url = users/info

            var controllerName = string.Empty;
            var actionName = string.Empty;
            var reqMethod = request.RequestMethod.ToString();

            if (request.Path == "/")
            {
                controllerName = "Home";
                actionName = "Index";
            }
            else
            {
                var requestUrlSplit = request.Path.Split('/', StringSplitOptions.RemoveEmptyEntries);

                controllerName = requestUrlSplit[0].Capitalize();
                actionName = requestUrlSplit[1].Capitalize();
            }
            //TODO: /users

            //2.Use reflection to:
            //-Instantiate a new Controller object - using a constant file path to the app's controllers folder and extracted controller name
            var controller = this.GetController(controllerName, request);

            //-Extract the MethodInfo from the Controller object - using the instantiated Controller object, extracted Action Name and using the request method to check the attribute of the method
            var action = this.GetMethod(reqMethod, controller, actionName);

            if (controller == null || action == null)
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }
            //3.Prepare the response :
            //-Invoke the Action and extract it's result - using the extracted MethodInfo and the controller object; format the IActionResult to a proper result - if it's an IViewable => HtmlResult, if it's IRedirectable=>RedirectResult
            object[] actionParameters = this.MapActionParameters(action, request, controller);
            IActionResult actionResult = this.InvokeAction(controller, action, actionParameters);

            return this.PrepareResponse(actionResult);
        }

        private IHttpResponse PrepareResponse(IActionResult actionResult)
        {
            string invocationResult = actionResult.Invoke();

            if (actionResult is IViewable)
            {
                return new HtmlResult(invocationResult, HttpResponseStatusCode.Ok);
            }
            else if (actionResult is IRedirectable)
            {
                return new WebServer.Results.RedirectResult(invocationResult);
            }
            else
            {
                throw new InvalidOperationException("The view result is not supported");
            }
        }

        private IActionResult InvokeAction(Controller controller, MethodInfo action, object[] actionParameters)
        {
            return (IActionResult)action.Invoke(controller, actionParameters);
        }

        private object[] MapActionParameters(MethodInfo action, IHttpRequest request, Controller controller)
        {
            ParameterInfo[] actionParametersInfo = action.GetParameters();
            object[] mappedActionParameters = new object[actionParametersInfo.Length];

            for (int index = 0; index < actionParametersInfo.Length; index++)
            {
                ParameterInfo currentParameterInfo = actionParametersInfo[index];
                var currentParameterType = currentParameterInfo.ParameterType;

                if (currentParameterType.IsPrimitive || currentParameterType == typeof(string))
                {
                    mappedActionParameters[index] = this.ProcessPrimitiveParameter(currentParameterInfo, request);
                }
                else
                {
                    var bindingObject = this.ProcessBindingModelParameter(currentParameterInfo, request);
                    controller.ModelState.IsValid = this.isValidModel(bindingObject);
                    mappedActionParameters[index] = bindingObject;
                }

            }

            return mappedActionParameters;
        }

        private bool? isValidModel(object bindingModel)
        {
            // Traverse all of the bindingModel's properties.
            var properties = bindingModel.GetType().GetProperties();

            foreach (var property in properties)
            {
                //Extract all ValidationAttributes from the current Property(if any).
                var propertyAttributes = property
                    .GetCustomAttributes()
                    .Where(ca => ca is ValidationAttribute)
                    .Cast<ValidationAttribute>()
                    .ToList();

                //Call isValid() method on the property's value, for each ValidationAttribute.

                foreach (var attribute in propertyAttributes)
                {
                    if (!attribute.IsValid(property.GetValue(bindingModel)))
                    {
                        return false;
                    }
                }
            }
            //If even one return false, this method should return false.
            //If everything is valid, this method should return true.
            return true;
        }

        private object ProcessBindingModelParameter(ParameterInfo param, IHttpRequest request)
        {
            Type bindingModelType = param.ParameterType;

            //TODO: viewModels should be with empty constructors!!!
            var bindingModelInstance = Activator.CreateInstance(bindingModelType);
            var bindingModelProperties = bindingModelType.GetProperties();

            foreach (var property in bindingModelProperties)
            {
                try
                {
                    //TODO: in this case we should have only primitive types in ViewModels. If property is collection?!?
                    object value = this.GetParameterFromRequest(request, property.Name.ToLower());
                    property.SetValue(bindingModelInstance, Convert.ChangeType(value, property.PropertyType));
                }
                catch
                {
                    Console.WriteLine($"The {property.Name} field could not be mapped.");
                }
            }

            return Convert.ChangeType(bindingModelInstance, bindingModelType);
        }

        private object ProcessPrimitiveParameter(ParameterInfo param, IHttpRequest request)
        {
            object value = this.GetParameterFromRequest(request, param.Name);
            return Convert.ChangeType(value, param.ParameterType);
        }

        private object GetParameterFromRequest(IHttpRequest request, string paramName)
        {
            if (request.QueryData.ContainsKey(paramName)) return request.QueryData[paramName];
            if (request.FormData.ContainsKey(paramName)) return request.FormData[paramName];
            return null;
        }

        private Controller GetController(string controllerName, IHttpRequest request)
        {
            //IRunesWebApp.Controllers.AccountController,IRunesWebApp
            if (!string.IsNullOrWhiteSpace(controllerName))
            {
                string fullyQualifiedControllerName = string.Format("{0}.{1}.{2}{3}, {0}",
                    MvcContext.Get.AssemblyName,
                    MvcContext.Get.ControllersFolder,
                    controllerName,
                    MvcContext.Get.ControllersSuffix);

                var controllerType = Type.GetType(fullyQualifiedControllerName);
                var controller = (Controller)Activator.CreateInstance(controllerType);

                if (controller != null)
                {
                    controller.Request = request;
                }

                return controller;
            }

            return null;
        }

        private MethodInfo GetMethod(string reqMethod, Controller controller, string actionName)
        {

            var actions = this.GetSuitableMethod(controller, actionName).ToList();

            foreach (var action in actions)
            {
                var attributes = action
                    .GetCustomAttributes()
                    .Where(attr => attr is HttpMethodAttribute)
                    .Cast<HttpMethodAttribute>()
                    .ToList();

                if (!attributes.Any() && reqMethod.ToUpper() == "GET")
                {
                    return action;
                }

                foreach (var attribute in attributes)
                {
                    if (attribute.IsValid(reqMethod))
                    {
                        return action;
                    }
                }
            }

            return null;
        }

        private IEnumerable<MethodInfo> GetSuitableMethod(Controller controller, string actionName)
        {
            if (controller == null)
            {
                return new MethodInfo[0];
            }

            return controller
                .GetType()
                .GetMethods()
                .Where(mi => mi.Name.ToLower() == actionName.ToLower());

        }

        private IHttpResponse PrepareResponseOld(Controller controller, MethodInfo action)
        {
            Console.WriteLine(controller.ToString());
            Console.WriteLine(action.ToString());
            ;
            IActionResult actionResult = (IActionResult)action.Invoke(controller, null);
            string result = actionResult.Invoke();

            if (actionResult is IViewable)
            {
                return new HtmlResult(result, HttpResponseStatusCode.Ok);
            }
            else if (actionResult is IRedirectable)
            {
                return new WebServer.Results.RedirectResult(result);
            }
            else
            {
                throw new InvalidOperationException("The view result is not supported");
            }
        }

    }
}
