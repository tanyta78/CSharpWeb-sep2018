namespace SIS.Framework.Views
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using ActionResults;

    public class View : IRenderable 
    {
        private readonly string fullyQualifiedTemplateName;
        private readonly IDictionary<string, object> viewData;

        public View(string fullyQualifiedTemplateName, IDictionary<string,object> viewData)
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
            return renderedHtml;
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
