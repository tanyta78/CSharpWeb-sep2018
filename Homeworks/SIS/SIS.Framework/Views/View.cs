namespace SIS.Framework.Views
{
    using ActionResults;

    public class View : IRenderable
    {
        //private readonly string fullyQualifiedTemplateName;
        //private const string RenderBodyConst = "@RenderBody()";
        //private readonly IDictionary<string, object> viewData;

        //public View(string fullyQualifiedTemplateName, IDictionary<string, object> viewData)
        //{
        //    this.fullyQualifiedTemplateName = fullyQualifiedTemplateName;
        //    this.viewData = viewData;
        //}

        //private string ReadFile()
        //{
        //    if (!File.Exists(this.fullyQualifiedTemplateName))
        //    {
        //        throw new FileNotFoundException($"View does not exist at{this.fullyQualifiedTemplateName}");
        //    }

        //    var result = File.ReadAllText(this.fullyQualifiedTemplateName);

        //    return result;
        //}

        //public string Render()
        //{
        //    var fullHtml = this.ReadFile();
        //    string renderedHtml = this.RenderHtml(fullHtml);

        //    var layoutWithView = this.AddViewToLayout(renderedHtml);
        //    return layoutWithView;
        //}

        //private string AddViewToLayout(string renderedHtml)
        //{
        //    string layoutPath = MvcContext.Get.RootDirectoryRelativePath +
        //                        GlobalConstants.DirectorySeparator +
        //                        MvcContext.Get.ViewsFolderName +
        //                        GlobalConstants.DirectorySeparator +
        //                        MvcContext.Get.LayoutFileName +
        //                        GlobalConstants.HtmlFileExtension;

        //    if (!File.Exists(layoutPath))
        //    {
        //        throw new FileNotFoundException($"View {layoutPath} not found");
        //    }

        //    var layoutContent = File.ReadAllText(layoutPath);

        //    //TODO: ADD NAVIGATION RENDER
        //    if (this.IsLoggedIn)
        //    {
        //        var loginNav = File.ReadAllText("Views/Navigation/loginNav.html");
        //        layoutContent = layoutContent.Replace("@RenderNav()", loginNav);
        //    }
        //    else
        //    {
        //        var logoutNav = File.ReadAllText("Views/Navigation/logoutNav.html");
        //        layoutContent = layoutContent.Replace("@RenderNav()", logoutNav);
        //    }

        //    var layoutWithView = layoutContent.Replace(RenderBodyConst, renderedHtml);

        //    return layoutWithView;
        //}

        ////TODO: refactor to work. Now is set to false
        //public bool IsLoggedIn { get; set; }

        //private string RenderHtml(string fullHtml)
        //{
        //    string renderedHtml = fullHtml;
        //    if (this.viewData.Any())
        //    {
        //        foreach (var parameter in this.viewData)
        //        {
        //            renderedHtml = renderedHtml.Replace($"{{{{{{{parameter.Key}}}}}}}", parameter.Value.ToString());
        //        }
        //    }

        //    return renderedHtml;
        //}
        private readonly string fullHtmlContent;

        public View(string fullHtmlContent)
        {
            this.fullHtmlContent = fullHtmlContent;
        }
        public string Render()
        {
            return this.fullHtmlContent;
        }
    }
}
