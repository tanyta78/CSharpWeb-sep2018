namespace SIS.MvcFramework
{
    using HTTP.Enums;
    using HTTP.Headers;
    using HTTP.Requests.Contracts;
    using HTTP.Responses;
    using HTTP.Responses.Contracts;
    using Services;
    using System.Text;
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

        protected string User
        {
            get
            {
                if (!this.Request.Cookies.ContainsCookie(".auth-app"))
                {
                    return null;
                }

                var cookie = this.Request.Cookies.GetCookie(".auth-app");
                var cookieContent = cookie.Value;
                var username = this.UserCookieService.GetUserData(cookieContent);
                return username;
            }
        }


        protected IHttpResponse View<T>(string viewName, T model = null, string layoutName = "_Layout") where T : class
        {
            var allContent = this.GetViewContent(viewName, model, layoutName);
            this.PrepareHtmlResult(allContent);
            return this.Response;
        }

        protected IHttpResponse View(string viewName, string layoutName = "_Layout")
        {
            var allContent = this.GetViewContent(viewName, (object)null, layoutName);
            this.PrepareHtmlResult(allContent);
            return this.Response;
        }

        private string GetViewContent<T>(string viewName, T model, string layoutName = "_Layout")
        {
            var layoutCode = System.IO.File.ReadAllText($"Views/{layoutName}.html");

            var viewCode = System.IO.File.ReadAllText("Views/" + viewName + ".html");
            var content = this.ViewEngine.GetHtml(viewName, viewCode, model);


            var allContent = layoutCode.Replace(" @RenderBody()", content);

            if (this.User != null)
            {
                var loginNav = System.IO.File.ReadAllText("Views/Navigation/loginNav.html");
                allContent = allContent.Replace("@RenderNav()", loginNav);
            }
            else
            {
                var logoutNav = System.IO.File.ReadAllText("Views/Navigation/logoutNav.html");
                allContent = allContent.Replace("@RenderNav()", logoutNav);

            }

            var layoutContent = this.ViewEngine.GetHtml("_Layout", allContent, model);

            return layoutContent;
        }

        //private string GetCurrentControllerName() => this.GetType().Name.Replace(ControllerDefaultName, string.Empty);

        //private const string ControllerDefaultName = "Controller";
        //private const string RootDirectoryRelativePath = "../../../";
        //private const string ViewsFolderName = "Views";
        //private const string DirectorySeparator = "/";
        //private const string HtmlFileExtension = ".html";
        //private const string LayoutViewFileName = "_Layout";
        //private const string RenderBodyConst = "@RenderBody()";

        //private IHttpResponse ViewAuto([CallerMemberName] string viewName = "")
        //{

        //    string layoutPath = RootDirectoryRelativePath +
        //                      ViewsFolderName +
        //                      DirectorySeparator +
        //                      LayoutViewFileName + HtmlFileExtension;
        //    string filePath = RootDirectoryRelativePath +
        //                      ViewsFolderName +
        //                      DirectorySeparator +
        //                      this.GetCurrentControllerName() +
        //                      DirectorySeparator + viewName + HtmlFileExtension;
        //    if (!System.IO.File.Exists(filePath))
        //    {
        //        return new BadRequestResult($"View {viewName} not found");
        //    }

        //    var viewContent = System.IO.File.ReadAllText(filePath);
        //    var layoutContent = System.IO.File.ReadAllText(layoutPath);

        //    foreach (var viewBagKey in this.ViewBag.Keys)
        //    {
        //        var dynamicDataPlaceholder = $"{{{{{viewBagKey}}}}}";
        //        var newValue = this.ViewBag[viewBagKey];
        //        if (viewContent.Contains(dynamicDataPlaceholder))
        //        {
        //            viewContent = viewContent.Replace(dynamicDataPlaceholder, newValue);
        //        }
        //    }
        //    var allContent = layoutContent.Replace(RenderBodyConst, viewContent);
        //    this.PrepareHtmlResult(allContent);
        //    return this.Response;
        //}

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
