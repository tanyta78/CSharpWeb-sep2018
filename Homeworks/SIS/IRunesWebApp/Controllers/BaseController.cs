namespace IRunesWebApp.Controllers
{
    using Data;
    using SIS.Framework.Controllers;
    using SIS.Framework.Services.Contracts;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;

    public abstract class BaseController : Controller
    {
        protected BaseController(IUserCookieService userCookieService)
        {
          
            this.UserCookieService = userCookieService;
        }

      

        protected IUserCookieService UserCookieService { get; }

        protected string Error { get; set; }

        protected IHttpResponse BadRequestError(string errorMessage)
        {
            return new HtmlResult($"<h1>{errorMessage}</h1>", HttpResponseStatusCode.BadRequest);
        }

        protected IHttpResponse ServerError(string errorMessage)
        {
            return new HtmlResult($"<h1>{errorMessage}</h1>", HttpResponseStatusCode.InternalServerError);
        }


        public void SignInUser(string username, IHttpRequest request)
        {
            request.Session.AddParameter("username", username);

            var cookieContent = this.UserCookieService.GetUserCookie(username);

            request.Cookies.Add(new HttpCookie(".auth-irunes", cookieContent, 7));
        }

        public bool IsAuthenticated()
        {
            return this.Request.Session.ContainsParameter("username");

        }
    }
}
