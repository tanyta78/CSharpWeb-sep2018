namespace SIS.MvcFramework
{
    using System.Collections.Generic;
    using System.IO;
    using HTTP.Enums;
    using HTTP.Requests.Contracts;
    using HTTP.Responses.Contracts;
    using Services;
    using WebServer.Results;

    public class Controller
    {
        public Controller()
        {
            this.UserCookieService=new UserCookieService();
        }

        public IHttpRequest Request { get; set;}

        protected IUserCookieService UserCookieService { get; }

        protected string GetUsername()
        {
            if (!this.Request.Cookies.ContainsCookie(".auth-cakes"))
            {
                return null;
            }

            var cookie = this.Request.Cookies.GetCookie(".auth-cakes");
            var cookieContent = cookie.Value;
            var username = this.UserCookieService.GetUserData(cookieContent);
            return username;
        }

        protected IHttpResponse View(string viewName, IDictionary<string, string> viewBag = null)
        {
            if (viewBag == null)
            {
                viewBag = new Dictionary<string, string>();
            }
            var allContent = this.GetViewContent(viewName, viewBag);
            return new HtmlResult(allContent, HttpResponseStatusCode.Ok);
        }

        private string GetViewContent(string viewName, IDictionary<string, string> viewBag)
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
