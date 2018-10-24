namespace SIS.Framework.Views
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class ViewEngine
    {
        private const string DisplayTemplateSuffix = "DisplayTemplate";

        private const string DisplayTemplatesFolderName = "DisplayTemplates";

        private const string ErrorViewName = "_Error";

        private const string NavViewName = "_Nav";
        
        private const string ViewExtension = "html";

        private const string ModelCollectionViewParameterPattern = @"\@Model\.Collection\.(\w+)\((.+)\)";

        private string ViewsFolderPath =>
            $@"{MvcContext.Get.RootDirectoryRelativePath}/{MvcContext.Get.ViewsFolderName}";

        private string ViewsSharedFolderPath =>
            $@"{this.ViewsFolderPath}/{MvcContext.Get.SharedViewsFolderName}";

        private string FormatNavViewPath =>
            $@"{this.ViewsFolderPath}/{MvcContext.Get.NavigationFolder}/{NavViewName}.{ViewExtension}";

        private string ViewsDisplayTemplateFolderPath =>
            $@"{this.ViewsSharedFolderPath}/{DisplayTemplatesFolderName}";

        private string FormatLayoutViewPath() =>
            $@"{this.ViewsSharedFolderPath}/{MvcContext.Get.LayoutFileName}.{ViewExtension}";

        private string FormatErrorViewPath =>
            $@"{this.ViewsSharedFolderPath}/{ErrorViewName}.{ViewExtension}";

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

        private string ReadNavHtml(string navViewPath)
        {
            
            if (!File.Exists(navViewPath))
            {
                throw new FileNotFoundException($"Navigation view does not exist.");
            }

            return File.ReadAllText(navViewPath);
        }

        private string RenderObject(object viewObject, string displayTemplate)
        {
            var properties = viewObject.GetType().GetProperties();

            foreach (var prop in properties)
            {
                var obj = prop.GetValue(viewObject);
                var objName = prop.Name;
                displayTemplate = this.RenderViewData(displayTemplate, obj, objName);
            }

            return displayTemplate;
        }

        private string RenderViewData(string template, object viewObject, string viewObjectName = null)
        {

            if (viewObject != null
                && viewObject.GetType()!=typeof(string)
                && viewObject is IEnumerable enumerable
                && Regex.IsMatch(template, ModelCollectionViewParameterPattern))
            {
                Match collectionMatch = Regex.Matches(template, ModelCollectionViewParameterPattern)
                                             .First(cm => cm.Groups[1].Value == viewObjectName);

                string fullMatch = collectionMatch.Groups[0].Value;
                string itemPattern = collectionMatch.Groups[2].Value;

                var result = string.Empty;

                foreach (var subObj in enumerable)
                {
                    result += itemPattern.Replace("@item", this.RenderViewData(template, subObj));
                }

                return template.Replace(fullMatch, result);
            }

            if (viewObject != null
                && viewObject.GetType() != typeof(string)
                && !viewObject.GetType().IsPrimitive
                )
            {
                if (File.Exists(this.FormatDisplayTemplatePath(viewObject.GetType().Name)))
                {
                    var displayTemplate = File.ReadAllText(this.FormatDisplayTemplatePath(viewObject.GetType().Name));

                    var renderedObj = this.RenderObject(viewObject, displayTemplate);

                    return viewObjectName != null
                        ? template.Replace($"@Model.{viewObjectName}", renderedObj)
                        : renderedObj;
                }
            }
            
            return viewObjectName != null 
                ? template.Replace($"@Model.{viewObjectName}", viewObject?.ToString())
                : viewObject?.ToString();
        }

        public string GetErrorContent()
            => this.ReadLayoutHtml(this.FormatLayoutViewPath())
                   .Replace("@Error", this.ReadLayoutHtml(this.FormatErrorViewPath));

        public string GetViewContent(string controllerName, string actionName)
            => this.ReadLayoutHtml(this.FormatLayoutViewPath())
                   .Replace("@RenderBody()", this.ReadViewHtml(this.FormatViewPath(controllerName, actionName)))
                   .Replace("@RenderNav()", this.ReadNavHtml(this.FormatNavViewPath));

       
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
            else
            {
                renderedHtml = renderedHtml.Replace("@Error","");
            }

            return renderedHtml;
        }
    }
}
