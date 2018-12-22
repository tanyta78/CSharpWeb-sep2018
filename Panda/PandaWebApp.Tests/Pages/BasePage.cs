namespace PandaWebApp.Tests.Pages
{
    using System;
    using System.Configuration;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public class BasePage
    {
        private readonly IWebDriver driver = BrowserHost.Instance.Application.Browser;
        private WebDriverWait wait;
        protected string URL = ConfigurationManager.AppSettings["URL"];

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(5));
        }

        public IWebDriver Driver => this.driver;
        public WebDriverWait Wait =>  this.wait;
    }
}
