namespace SIS.MvcFramework
{
    using System;
    using System.Text;
    using HTTP.Cookies.Contracts;
    using HTTP.Enums;
    using HTTP.Headers;
    using HTTP.Requests.Contracts;
    using HTTP.Responses;
    using HTTP.Responses.Contracts;
    using Services;
    using ViewEngine;

    public abstract class Controller
    {
        protected Controller()
        {
            this.Response = new HttpResponse(HttpResponseStatusCode.Ok);
        }

        public IHttpRequest Request { get; set; }

        public IHttpResponse Response { get; set; }

        public IViewEngine ViewEngine { get; set; }

        public IUserCookieService UserCookieService { get; internal set; }

        protected MvcUserInfo User => GetUserData(this.Request.Cookies, this.UserCookieService);

        public static MvcUserInfo GetUserData(
             IHttpCookieCollection cookieCollection,
             IUserCookieService userCookieService)
        {
            if (!cookieCollection.ContainsCookie(".auth-app"))
            {
                return new MvcUserInfo();
            }

            var cookie = cookieCollection.GetCookie(".auth-app");
            var cookieContent = cookie.Value;

            try
            {
                var username = userCookieService.GetUserData(cookieContent);
                return username;
            }
            catch (Exception)
            {
                return new MvcUserInfo();

            }

        }

        //this.View()
        // this.View(model,);
        // this.View(viewName,);
        // this.View(viewName,model,);

        protected IHttpResponse View<T>(
            string viewName = null,
            T model = null,
            string layoutName = "_Layout")
            where T : class
        {
            if (viewName == null)
            {
                //Generate viewName
                viewName = this.Request.Path.Trim('/', '\\');
                if (string.IsNullOrWhiteSpace(viewName))
                {
                    viewName = "Home/Index";
                }
            }

            var allContent = this.GetViewContent(viewName, model, layoutName);
            this.PrepareHtmlResult(allContent);
            return this.Response;
        }

        protected IHttpResponse View<T>(T model = null, string layoutName = "_Layout") where T : class
        {
            return this.View(null, model, layoutName);
        }

        protected IHttpResponse View(string viewName = null, string layoutName = "_Layout")
        {
            return this.View(viewName, (object)null, layoutName);
        }

        private string GetViewContent<T>(string viewName, T model, string layoutName = "_Layout")
        {

            var viewCode = System.IO.File.ReadAllText("Views/" + viewName + ".html");
            var content = this.ViewEngine.GetHtml(viewName, viewCode, model, this.User);

            string navContent = this.User.IsLoggedIn
                ? System.IO.File.ReadAllText("Views/Navigation/loginNav.html")
                : System.IO.File.ReadAllText("Views/Navigation/logoutNav.html");

            if (layoutName == null) return content;
            var layoutCode = System.IO.File.ReadAllText($"Views/{layoutName}.html");

            var allContent = layoutCode.Replace(" @RenderBody()", content);
            allContent = allContent.Replace("@RenderNav()", navContent);

            var layoutContent = this.ViewEngine.GetHtml("_Layout", allContent, model, this.User);

            return layoutContent;

        }


        protected IHttpResponse BadRequestError(string errorMessage)
        {
            var model = new ErrorViewModel()
            {
                Error = errorMessage
            };
            var allContent = this.GetViewContent("Error", model);
            this.PrepareHtmlResult(allContent);
            this.Response.StatusCode = HttpResponseStatusCode.BadRequest;
            return this.Response;

        }

        protected IHttpResponse BadRequestErrorWithView(string errorMessage)
        {
            return this.BadRequestErrorWithView(errorMessage, (object)null);
        }

        protected IHttpResponse BadRequestErrorWithView<T>(string errorMessage, T model, string layoutName = "_Layout")
        {
            var errorContent = this.GetViewContent("Error", new ErrorViewModel { Error = errorMessage }, null);

            var viewName = this.Request.Path.Trim('/', '\\');
            if (string.IsNullOrWhiteSpace(viewName))
            {
                viewName = "Home/Index";
            }

            var viewContent = this.GetViewContent(viewName, model, null);
            var allViewContent = errorContent + Environment.NewLine + viewContent;
            var errorAndViewContent = this.ViewEngine.GetHtml(viewName, allViewContent, model, this.User);

            var layoutFileContent = System.IO.File.ReadAllText($"Views/{layoutName}.html");
            var allContent = layoutFileContent.Replace("@RenderBody()", errorAndViewContent);
            string navContent = this.User.IsLoggedIn
                ? System.IO.File.ReadAllText("Views/Navigation/loginNav.html")
                : System.IO.File.ReadAllText("Views/Navigation/logoutNav.html");
            allContent = allContent.Replace("@RenderNav()", navContent);

            var layoutContent = this.ViewEngine.GetHtml("_Layout", allContent, model, this.User);

            this.PrepareHtmlResult(layoutContent);
            this.Response.StatusCode = HttpResponseStatusCode.BadRequest;
            return this.Response;
        }

        protected IHttpResponse ServerError(string errorMessage)
        {
            var model = new ErrorViewModel()
            {
                Error = errorMessage
            };
            var allContent = this.GetViewContent("Error", model);
            this.PrepareHtmlResult(allContent);
            this.Response.StatusCode = HttpResponseStatusCode.InternalServerError;
            return this.Response;
        }

        protected IHttpResponse File(byte[] content)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.ContentLength, content.Length.ToString()));
            this.Response.Headers.Add(new HttpHeader(HttpHeader.ContentDisposition, "inline"));
            this.Response.Content = content;
            return this.Response;
        }

        protected IHttpResponse Redirect(string location)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.Location, location));
            this.Response.StatusCode = HttpResponseStatusCode.SeeOther;
            return this.Response;
        }

        protected IHttpResponse Text(string content)
        {
            this.Response.Headers.Add(new HttpHeader(HttpHeader.ContentType, "text/plain; charset=utf-8"));
            this.Response.Content = Encoding.UTF8.GetBytes(content);
            return this.Response;
        }

        private void PrepareHtmlResult(string content)
        {
            this.Response.Headers.Add(new HttpHeader("Content-Type", "text/html; charset=utf-8"));
            this.Response.Content = Encoding.UTF8.GetBytes(content);
        }
    }
}
