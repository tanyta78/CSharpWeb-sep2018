namespace SIS.Framework.Controllers
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using ActionResults;
    using HTTP.Requests.Contracts;
    using Models;
    using Security.Contracts;
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

        public Model ModelState { get; } = new Model();

        private ViewEngine ViewEngine { get; } = new ViewEngine();

        protected IViewable View([CallerMemberName] string viewName = "")
        {
            var controllerName = ControllerUtilities.GetControllerName(this);
            string viewContent = null;

            try
            {
                viewContent = this.ViewEngine.GetViewContent(controllerName, viewName);
            }
            catch (FileNotFoundException e)
            {
                this.Model.Data["Error"] = e.Message;
                viewContent = this.ViewEngine.GetErrorContent();
            }

            var renderedContent = this.ViewEngine.RenderHtml(viewContent, this.Model.Data);
            return new ViewResult(new View(renderedContent));
        }

        protected IRedirectable RedirectToAction(string redirectUrl) => new RedirectResult(redirectUrl);

        protected void SignIn(IIdentity auth)
        {
            this.Request.Session.AddParameter("auth", auth);
        }

        protected void SignOut()
        {
            this.Request.Session.ClearParameters();
        }

        public IIdentity Identity => (IIdentity) this.Request.Session.GetParameter("auth");
    }
}
