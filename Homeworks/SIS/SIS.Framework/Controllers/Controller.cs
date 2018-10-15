namespace SIS.Framework.Controllers
{
    using ActionResults;
    using HTTP.Requests.Contracts;
    using System.Runtime.CompilerServices;
    using Utilities;
    using Views;

    public abstract class Controller
    {
        public IHttpRequest Request { get; set; }

        protected IViewable View([CallerMemberName] string viewName = "")
        {
            var controllerName = ControllerUtilities.GetControllerName(this);

            var fullyQualifiedName = ControllerUtilities
                .GetViewFullQualifiedName(controllerName, viewName);

            var view = new View(fullyQualifiedName);

            return new ViewResult(view);
        }

        protected IRedirectable RedirectToAction(string redirectUrl) => new RedirectResult(redirectUrl);
    }
}
