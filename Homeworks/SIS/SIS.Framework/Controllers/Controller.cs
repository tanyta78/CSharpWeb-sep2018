namespace SIS.Framework.Controllers
{
    using ActionResults;
    using HTTP.Requests.Contracts;
    using System.Runtime.CompilerServices;
    using Models;
    using Utilities;
    using Views;

    public abstract class Controller
    {
        protected Controller()
        {
            this.Model = new ViewModel();
        }

        public IHttpRequest Request { get; set; }

        public ViewModel Model { get; }

        public Model ModelState { get; }=new Model();

        protected IViewable View([CallerMemberName] string viewName = "")
        {
            var controllerName = ControllerUtilities.GetControllerName(this);

            var fullyQualifiedName = ControllerUtilities
                .GetViewFullQualifiedName(controllerName, viewName);

            var view = new View(fullyQualifiedName,this.Model.Data);

            return new ViewResult(view);
        }

        protected IRedirectable RedirectToAction(string redirectUrl) => new RedirectResult(redirectUrl);
    }
}
