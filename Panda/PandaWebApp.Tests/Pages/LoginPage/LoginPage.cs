using System;

namespace PandaWebApp.Tests.Pages.LoginPage
{
    using Models;
    using OpenQA.Selenium;

    public partial class LoginPage : BasePage
    {
        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        public string URL => base.URL + "Users/Login/";

        public void NavigateTo()
        {
            // this.Driver.Navigate().GoToUrl("http://localhost:60634/Account/Login");
            this.Driver.Navigate().GoToUrl(this.URL);
            this.Driver.Manage().Window.Maximize();
        }
        
        public void FillLogInData(LoginPageUserModel userData)
        {
            if (userData.Username != null)
                this.LogInUsername.SendKeys(userData.Username);
            if (userData.Password != null)
                this.LogInPassword.SendKeys(userData.Password);
           
            this.LogInBtn.Click();
        }
    }
}
