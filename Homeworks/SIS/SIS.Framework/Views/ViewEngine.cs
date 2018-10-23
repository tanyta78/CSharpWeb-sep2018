namespace SIS.Framework.Views
{
    using System.Collections.Generic;
    using System.IO;

    public class ViewEngine
    {
        private const string DisplayTemplateSuffix = "DisplayTemplate";

        private const string DisplayTemplatesFolderName = "DisplayTemplates";

        private const string ErrorViewName = "_Error";

        private const string ViewExtension = "html";

        private const string ModelCollectionViewParameterPattern = @"\@Model\.Collection\.(\w+)\((.+)\)";

        private string ViewsFolderPath =>
            $@"{MvcContext.Get.RootDirectoryRelativePath}{MvcContext.Get.ViewsFolderName}";

        private string ViewsSharedFolderPath =>
            $@"{this.ViewsFolderPath}/{MvcContext.Get.SharedViewsFolderName}";

        private string ViewsDisplayTemplateFolderPath =>
            $@"{this.ViewsSharedFolderPath}/{DisplayTemplatesFolderName}";

        private string FormatLayoutViewPath() =>
            $@"{this.ViewsSharedFolderPath}/{MvcContext.Get.LayoutFileName}.{ViewExtension}";

        private string FormatErrorViewPath =>
            $@"{this.ViewsSharedFolderPath}/{ErrorViewName}.${ViewExtension}";

        private string FormatViewPath(string controllerName, string actionName) =>
            $@"{this.ViewsFolderPath}/{controllerName}/{actionName}.{ViewExtension}";

        private string FormatDisplayTemplatePath(string objectName)
            => $@"{this.ViewsDisplayTemplateFolderPath}/{objectName}{DisplayTemplateSuffix}.{ViewExtension}";

        private string ReadLayoutHtml(string layoutViewPath)
        {
            if (!File.Exists(layoutViewPath))
            {
                throw new FileNotFoundException($"Layout view does not exist.");
            }

            return File.ReadAllText(layoutViewPath);
        }

        private string ReadErrorHtml(string errorViewPath)
        {
            if (!File.Exists(errorViewPath))
            {
                throw new FileNotFoundException($"Error view does not exist.");
            }

            return File.ReadAllText(errorViewPath);
        }

        private string ReadViewHtml(string viewPath)
        {
            if (!File.Exists(viewPath))
            {
                throw new FileNotFoundException($"View could not be found at{viewPath}");
            }

            return File.ReadAllText(viewPath);
        }

        //TODO:
        private string RenderObject(object viewObject, string displayTemplate)
        {
            return "";
        }

        //TODO:
        private string RenderViewData(string template, object viewObject, string viewObjectName = null)
        {
            return "";
        }

        public string GetErrorContent()
            => this.ReadLayoutHtml(this.FormatLayoutViewPath())
                   .Replace("@RenderError()", this.ReadLayoutHtml(this.FormatErrorViewPath));

        public string GetViewContent(string controllerName, string actionName)
            => this.ReadLayoutHtml(this.FormatLayoutViewPath())
                   .Replace("@RenderBody()", this.ReadViewHtml(this.FormatViewPath(controllerName, actionName)));

        //TODO - depends on login or refactor with display
        public string GetNavigationContent(string controllerName, string actionName)
            => this.ReadLayoutHtml(this.FormatLayoutViewPath())
                   .Replace("@RenderNav()", this.ReadViewHtml(this.FormatViewPath(controllerName, actionName)));

        public string RenderHtml(string fullHtmlContent, IDictionary<string, object> viewData)
        {
            string renderedHtml = fullHtmlContent;

            if (viewData.Count > 0)
            {
                foreach (var param in viewData)
                {
                    renderedHtml = this.RenderViewData(renderedHtml, param.Value, param.Key);
                }
            }

            if (viewData.ContainsKey("Error"))
            {
                renderedHtml = renderedHtml.Replace("@Error", viewData["Error"].ToString());
            }

            return renderedHtml;
        }
    }
}
