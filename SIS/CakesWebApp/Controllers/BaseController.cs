namespace CakesWebApp.Controllers
{
    using System.Collections.Generic;
    using Data;
    using Services;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;
    using System.IO;

    public abstract class BaseController
    {
        protected BaseController()
        {
            this.Db = new CakesDbContext();

            this.UserCookieService = new UserCookieService();
        }

        protected CakesDbContext Db { get; }

        protected IUserCookieService UserCookieService { get; }

        protected string GetUsername(IHttpRequest request)
        {
            if (!request.Cookies.ContainsCookie(".auth-cakes"))
            {
                return null;
            }

            var cookie = request.Cookies.GetCookie(".auth-cakes");
            var cookieContent = cookie.Value;
            var username = this.UserCookieService.GetUserData(cookieContent);
            return username;
        }

        protected IHttpResponse View(string viewName)
        {
            var allContent = this.GetViewContent(viewName,new Dictionary<string, string>());
            return new HtmlResult(allContent, HttpResponseStatusCode.Ok);
        }

        private string GetViewContent(string viewName, IDictionary<string,string> viewBag)
        {
            var layoutContent = File.ReadAllText("Views/_Layout.html");
            var content = File.ReadAllText("Views/" + viewName + ".html");
            foreach (var item in viewBag)
            {
                content = content.Replace("@Model." + item.Key, item.Value);
            }
            var allContent = layoutContent.Replace(" @RenderBody()", content);
            return allContent;
        }

        protected IHttpResponse BadRequestError(string errorMessage)
        {
            var viewBag = new Dictionary<string, string>();
            viewBag.Add("Error", errorMessage);
            var allContent = this.GetViewContent("Error", viewBag);
            return new HtmlResult(allContent, HttpResponseStatusCode.BadRequest);
        }

        protected IHttpResponse ServerError(string errorMessage)
        {
            var viewBag = new Dictionary<string, string>
            {
                {
                    "Error", errorMessage
                }
            };
            var allContent = this.GetViewContent("Error", viewBag);
            return new HtmlResult(allContent, HttpResponseStatusCode.InternalServerError);
        }
    }
}
