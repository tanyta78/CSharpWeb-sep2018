namespace PandaWebApp.Tests.Attributes
{
    using System;
    using System.Configuration;
    using System.IO;
    using NUnit.Framework;
    using NUnit.Framework.Interfaces;
    using OpenQA.Selenium;
    using Pages;

    public class TearDownClass : BasePage
    {
        public TearDownClass(IWebDriver driver) : base(driver)
        {
        }

        public void TearLogs()
        {
           // string path = AppDomain.CurrentDomain.BaseDirectory + "Logs\\";
             string path = ConfigurationManager.AppSettings["Logs"];
             path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);

            string filename = path + TestContext.CurrentContext.Test.Name + ".txt";
            string filenameJpg = path + TestContext.CurrentContext.Test.Name + ".jpg";
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            string testContextInfo = $"[ {DateTime.Now} ] [ {TestContext.CurrentContext.Result.Outcome.Status} ] [ {TestContext.CurrentContext.Test.MethodName} ] :: {TestContext.CurrentContext.Result.Message} ";

            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                filename = path + TestContext.CurrentContext.Test.Name + "  [FAILED] " + ".txt";
                ;
                File.WriteAllText(filename, testContextInfo);

            }
            else
            {
                File.WriteAllText(filename, testContextInfo);
            }


            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                if (File.Exists(filenameJpg))
                {
                    File.Delete(filenameJpg);
                }

                filenameJpg = path + TestContext.CurrentContext.Test.Name + "  [FAILED] " + ".jpg";
                var screenshot = ((ITakesScreenshot) this.Driver).GetScreenshot();
                screenshot.SaveAsFile(filenameJpg, ScreenshotImageFormat.Jpeg);

            }
        }
    }
}
