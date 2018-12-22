namespace PandaWebApp.Tests
{
    using OpenQA.Selenium.Chrome;
    using TestStack.Seleno.Configuration;

    public static class BrowserHost
    {
        public static readonly SelenoHost Instance = new SelenoHost();
        public static string RootUrl = @"http://localhost:80/";

        static BrowserHost()
        {
            //Chrome           
            Instance.Run("PandaWebApp", 80, w => w.WithRemoteWebDriver(() => new ChromeDriver()));
            //FireFox
            // Instance.Run("PandaWebApp", 80);

        }
    }
}
