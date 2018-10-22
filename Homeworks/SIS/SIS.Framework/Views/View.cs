﻿namespace SIS.Framework.Views
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using ActionResults;
    using HTTP.Common;

    public class View : IRenderable
    {
        private readonly string fullyQualifiedTemplateName;
        private const string RenderBodyConst = "@RenderBody()";
        private readonly IDictionary<string, object> viewData;

        public View(string fullyQualifiedTemplateName, IDictionary<string, object> viewData)
        {
            this.fullyQualifiedTemplateName = fullyQualifiedTemplateName;
            this.viewData = viewData;
        }

        private string ReadFile()
        {
            if (!File.Exists(this.fullyQualifiedTemplateName))
            {
                throw new FileNotFoundException($"View does not exist at{this.fullyQualifiedTemplateName}");
            }

            var result = File.ReadAllText(this.fullyQualifiedTemplateName);

            return result;
        }

        public string Render()
        {
            var fullHtml = this.ReadFile();
            string renderedHtml = this.RenderHtml(fullHtml);

            var layoutWithView = this.AddViewToLayout(renderedHtml);
            return renderedHtml;
        }

        private string AddViewToLayout(string renderedHtml)
        {
            string layoutPath = MvcContext.Get.RootDirectoryRelativePath +
                                GlobalConstants.DirectorySeparator +
                                MvcContext.Get.ViewsFolderName +
                                GlobalConstants.DirectorySeparator +
                                MvcContext.Get.LayoutFileName +
                                GlobalConstants.HtmlFileExtension;

            if (!File.Exists(layoutPath))
            {
                throw new FileNotFoundException($"View {layoutPath} not found");
            }

            var layoutContent = File.ReadAllText(layoutPath);

            var layoutWithView = layoutContent.Replace(RenderBodyConst, renderedHtml);
            //TODO: ADD NAVIGATION RENDER
            return layoutWithView;
        }

        private string RenderHtml(string fullHtml)
        {
            string renderedHtml = fullHtml;
            if (this.viewData.Any())
            {
                foreach (var parameter in this.viewData)
                {
                    renderedHtml = renderedHtml.Replace($"{{{{{{{parameter.Key}}}}}}}", parameter.Value.ToString());
                }
            }

            return renderedHtml;
        }
    }
}
