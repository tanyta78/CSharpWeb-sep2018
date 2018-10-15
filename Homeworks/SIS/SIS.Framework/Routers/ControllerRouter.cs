namespace SIS.Framework.Routers
{
    using System.Collections.Generic;
    using System.Reflection;
    using Controllers;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using WebServer.Api;

    public class ControllerRouter : IHttpHandler
    {
        private Controller GetController(string controllerName, IHttpRequest request)
        {
            throw new System.NotImplementedException();

        }

        private MethodInfo GetMethod(string reqMethod, Controller controller, string actionName)
        {
            throw new System.NotImplementedException();

        }

        private IEnumerable<MethodInfo> GetSuitableMethod(Controller controller, string actionName)
        {
            throw new System.NotImplementedException();

        }

        private IHttpResponse PrepaResponse(Controller controller, string actionName)
        {
            throw new System.NotImplementedException();

        }


        public IHttpResponse Handle(IHttpRequest request)
        {
            //1.Read the Request Data
            //-Extract the request method * to be able to search for suitable actions
            //-Extract the Controller name * using the Request Path(/Home/Index) and controller suffix  controller name would be HomeController
            //-Extract the Action name * using the Request Path(/Home/Index) action name would be Index

            //2.Use reflection to:
            //-Instantiate a new Controller object - using a constant file path to the app's controllers folder and extracted controller name
            //-Extract the MethodInfo from the Controller object - using the instantiated Controller object, extracted Action Name and using the request method to check the attribute of the method

            //3.Prepare the response :
            //-Invoke the Action and extract it's result - using the extracted MethodInfo and the controller object; format the IActionResult to a proper result - if it's an IViewable => HtmlResult, if it's IRedirectable=>RedirectResult

            throw new System.NotImplementedException();
        }
    }
}
