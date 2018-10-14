namespace SIS.Framework.Views
{
    using System.IO;
    using ActionResults;

    public class View : IRenderable 
    {
        private readonly string fullyQualifiedTemplateName;

        public View(string fullyQualifiedTemplateName)
        {
            this.fullyQualifiedTemplateName = fullyQualifiedTemplateName;
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
