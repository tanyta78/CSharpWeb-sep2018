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

    public abstract class BaseController
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
            return new HtmlResult(fileContent, HttpResponseStatusCode.Ok);
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

        protected IHttpResponse ViewAuto([CallerMemberName] string viewName = "")
        {
            string filePath = RootDirectoryRelativePath +
                              ViewsFolderName +
                              DirectorySeparator +
                              this.GetCurrentControllerName() +
                              DirectorySeparator + viewName + HtmlFileExtension;
            if (!File.Exists(filePath))
            {
                return new BadRequestResult($"View {viewName} not found");
            }

            var fileContent = File.ReadAllText(filePath);

            foreach (var viewBagKey in this.ViewBag.Keys)
            {
                var dynamicDataPlaceholder = $"{{{{{viewBagKey}}}}}";
                var newValue = this.ViewBag[viewBagKey];
                if (fileContent.Contains(dynamicDataPlaceholder))
                {
                    fileContent = fileContent.Replace(dynamicDataPlaceholder, newValue);
                }
            }

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
            return request.Session.ContainsParameter("username");
        }
    }
}
