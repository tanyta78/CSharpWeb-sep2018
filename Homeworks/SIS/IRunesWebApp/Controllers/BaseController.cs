namespace IRunesWebApp.Controllers
{
    using System.IO;
    using Data;
    using Services;
    using SIS.Framework.Controllers;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;

    public abstract class BaseController : Controller
    {
        protected BaseController(IUserCookieService userCookieService)
        {
            this.Db = new IRunesDbContext();
            this.UserCookieService = userCookieService;
        }

        protected IRunesDbContext Db { get; }

        protected IUserCookieService UserCookieService { get; }

        protected IHttpResponse BadRequestError(string errorMessage)
        {
            return new HtmlResult($"<h1>{errorMessage}</h1>", HttpResponseStatusCode.BadRequest);
        }

        protected IHttpResponse ServerError(string errorMessage)
        {
            return new HtmlResult($"<h1>{errorMessage}</h1>", HttpResponseStatusCode.InternalServerError);
        }


       public IHttpResponse SignInUser(string username, IHttpRequest request)
        {
            request.Session.AddParameter("username", username);

            var cookieContent = this.UserCookieService.GetUserCookie(username);
            var response = new RedirectResult("/");
            response.Cookies.Add(new HttpCookie(".auth-irunes", cookieContent, 7));
            return response;
        }

        public bool IsAuthenticated()
        {
            return this.Request.Session.ContainsParameter("username");

        }
    }
}
