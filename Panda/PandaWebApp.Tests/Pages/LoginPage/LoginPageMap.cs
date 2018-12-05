namespace PandaWebApp.Tests.Pages.LoginPage
{
    using OpenQA.Selenium;

    public partial class LoginPage
    {
        public IWebElement LogInBtn => this.Driver.FindElement(By.XPath("/html/body/div/main/form/div[3]/button"));

        public IWebElement LogInUsername => this.Driver.FindElement(By.Id("username"));

        public IWebElement LogInPassword => this.Driver.FindElement(By.Id("password"));

        
        //public IWebElement LogOffBtn
        //{
        //    get
        //    {
        //        return this.Driver.FindElement(By.XPath("//*[@id=\"logoutForm\"]/ul/li[3]/a"));
        //    }
        //}
        //public IWebElement ErrorMessageInvalidEmail
        //{
        //    get
        //    {
        //        this.Wait.Until(w => w.FindElement(By.XPath("/html/body/div[2]/div/div/form/div[1]/div/span/span")));
        //        return this.Driver.FindElement(By.XPath("/html/body/div[2]/div/div/form/div[1]/div/span/span"));
        //    }
        //}
        //public IWebElement ErrorMessageInvalidPassword
        //{
        //    get
        //    {
        //        this.Wait.Until(w => w.FindElement(By.XPath("/html/body/div[2]/div/div/form/div[1]/ul/li")));
        //        return this.Driver.FindElement(By.XPath("/html/body/div[2]/div/div/form/div[1]/ul/li"));
        //    }
        //}
        //public IWebElement ErrorMessageWithoutEmail
        //{
        //    get
        //    {
        //        this.Wait.Until(w => w.FindElement(By.XPath("html/body/div[2]/div/div/form/div[1]/div/span/span")));
        //        return this.Driver.FindElement(By.XPath("html/body/div[2]/div/div/form/div[1]/div/span/span"));
        //    }
        //}
        //public IWebElement ErrorMessageWithoutPassword
        //{
        //    get
        //    {
        //        this.Wait.Until(w => w.FindElement(By.XPath("/html/body/div[2]/div/div/form/div[2]/div/span/span")));
        //        return this.Driver.FindElement(By.XPath("/html/body/div[2]/div/div/form/div[2]/div/span/span"));
        //    }
        //}
    }
}

