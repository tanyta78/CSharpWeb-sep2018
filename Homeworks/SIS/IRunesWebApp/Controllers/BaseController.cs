namespace IRunesWebApp.Controllers
{
    using Data;
    using Services;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using SIS.Framework.Controllers;

    public abstract class BaseController:Controller
    {
        protected BaseController()
        {
            this.Db = new IRunesDbContext();
            this.ViewBag = new Dictionary<string, string>();
            this.UserCookieService = new UserCookieService();
          
        }

        protected IDictionary<string, string> ViewBag { get; set; }

        protected IRunesDbContext Db { get; }

        protected IUserCookieService UserCookieService { get; }
        public bool IsLoggedIn { get; private set; }

        protected string GetUsername(IHttpRequest request)
        {
            if (!request.Cookies.ContainsCookie(".auth-irunes"))
            {
                return null;
            }

            var cookie = request.Cookies.GetCookie(".auth-irunes");
            var cookieContent = cookie.Value;
            var username = this.UserCookieService.GetUserData(cookieContent);
            return username;
        }

        protected IHttpResponse View(string viewName)
        {
            var layoutContent = File.ReadAllText("Views/_Layout.html");
           
            var fileContent = File.ReadAllText("Views/" + viewName + ".html");
            foreach (var viewBagKey in this.ViewBag.Keys)
            {
                var dynamicDataPlaceholder = $"{{{{{viewBagKey}}}}}";
                var newValue = this.ViewBag[viewBagKey];
                if (fileContent.Contains(dynamicDataPlaceholder))
                {
                    fileContent = fileContent.Replace(dynamicDataPlaceholder, newValue);
                }
              
            }
             //TODO: ADD NAV   @RenderNav()
            if (this.IsLoggedIn)
            {
                var loginNav = File.ReadAllText("Views/Navigation/loginNav.html");
                layoutContent = layoutContent.Replace("@RenderNav()", loginNav);
            }
            else
            {
                var logoutNav = File.ReadAllText("Views/Navigation/logoutNav.html");
                layoutContent = layoutContent.Replace("@RenderNav()", logoutNav);

            }
            var allContent = layoutContent.Replace("@RenderBody()", fileContent);
            return new HtmlResult(allContent, HttpResponseStatusCode.Ok);
        }

        protected IHttpResponse BadRequestError(string errorMessage)
        {
            return new HtmlResult($"<h1>{errorMessage}</h1>", HttpResponseStatusCode.BadRequest);
        }

        protected IHttpResponse ServerError(string errorMessage)
        {
            return new HtmlResult($"<h1>{errorMessage}</h1>", HttpResponseStatusCode.InternalServerError);
        }

        private string GetCurrentControllerName() => this.GetType().Name.Replace(ControllerDefaultName, string.Empty);

        private const string ControllerDefaultName = "Controller";
        private const string RootDirectoryRelativePath = "../../../";
        private const string ViewsFolderName = "Views";
        private const string DirectorySeparator = "/";
        private const string HtmlFileExtension = ".html";
        private const string LayoutViewFileName = "_Layout";
        private const string RenderBodyConst = "@RenderBody()";

        protected IHttpResponse ViewAuto([CallerMemberName] string viewName = "")
        {

            string layoutPath = RootDirectoryRelativePath +
                              ViewsFolderName +
                              DirectorySeparator +
                              LayoutViewFileName + HtmlFileExtension;
            string filePath = RootDirectoryRelativePath +
                              ViewsFolderName +
                              DirectorySeparator +
                              this.GetCurrentControllerName() +
                              DirectorySeparator + viewName + HtmlFileExtension;
            if (!File.Exists(filePath))
            {
                return new BadRequestResult($"View {viewName} not found");
            }

            var viewContent = File.ReadAllText(filePath);
            var layoutContent = File.ReadAllText(layoutPath);

            foreach (var viewBagKey in this.ViewBag.Keys)
            {
                var dynamicDataPlaceholder = $"{{{{{viewBagKey}}}}}";
                var newValue = this.ViewBag[viewBagKey];
                if (viewContent.Contains(dynamicDataPlaceholder))
                {
                    viewContent = viewContent.Replace(dynamicDataPlaceholder, newValue);
                }
            }
            var fileContent = layoutContent.Replace(RenderBodyConst, viewContent);
            var response = new HtmlResult(fileContent, HttpResponseStatusCode.Ok);

            return response;
        }

        public IHttpResponse SignInUser(string username, IHttpRequest request)
        {
            request.Session.AddParameter("username", username);
         
            var cookieContent = this.UserCookieService.GetUserCookie(username);
            var response = new RedirectResult("/");
            response.Cookies.Add(new HttpCookie(".auth-irunes", cookieContent, 7));
            return response;
        }
        public bool IsAuthenticated(IHttpRequest request)
        {
            this.IsLoggedIn=request.Session.ContainsParameter("username");
            return IsLoggedIn;
        }
    }
}
