namespace SIS.Framework.Views
{
    using System.Collections.Generic;
    using System.IO;
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
            return fullHtml;
        }
    }
}
