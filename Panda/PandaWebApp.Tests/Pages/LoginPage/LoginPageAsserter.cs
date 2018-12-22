namespace PandaWebApp.Tests.Pages.LoginPage
{
    using NUnit.Framework;

    public static class LoginPageAsserter
    {
        public static void AssertLogInPage(this LoginPage page, string text)
        {
            Assert.AreEqual(page.h2logInPage.Text, text);
        }
        public static void AssertLogInSuccessfully(this LoginPage page, string text)
        {
            Assert.AreEqual(page.LogOffBtn.Text,text);
        }
        public static void AssertMessageInvalidEmail(this LoginPage page, string text)
        {
            Assert.IsTrue(page.ErrorMessageInvalidEmail.Displayed);
            Assert.AreEqual( page.ErrorMessageInvalidEmail.Text, text);
        }
        public static void AssertMessageInvalidPass(this LoginPage page, string text)
        {
            Assert.IsTrue(page.ErrorMessageInvalidPassword.Displayed);
            Assert.AreEqual(page.ErrorMessageInvalidPassword.Text, text);
        }
        public static void AssertMessageWithoutEmail(this LoginPage page, string text)
        {
            Assert.IsTrue(page.ErrorMessageWithoutEmail.Displayed);
            Assert.AreEqual(page.ErrorMessageWithoutEmail.Text, text);
        }
        public static void AssertMessageWithoutPass(this LoginPage page, string text)
        {
            Assert.IsTrue(page.ErrorMessageWithoutPassword.Displayed);
            Assert.AreEqual(page.ErrorMessageWithoutPassword.Text, text);
        }
    }
}
