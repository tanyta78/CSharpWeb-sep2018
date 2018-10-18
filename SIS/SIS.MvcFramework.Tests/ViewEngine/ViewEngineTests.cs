namespace SIS.MvcFramework.Tests.ViewEngine
{
    using System.IO;
    using MvcFramework.ViewEngine;
    using Xunit;

    public class ViewEngineTests
    {
        [Theory]
        [InlineData("IfForForeach")]
        [InlineData("ViewWithNoCode")]
        [InlineData("WorkWithViewModel")]
        public void RunTestViews(string testViewName)
        {
            // read view
            var viewCode = File.ReadAllText($"TestViews/{testViewName}.html");

            //read result
            var expectedResult = File.ReadAllText($"TestViews/{testViewName}.Result.html");

            //run view engine
            IViewEngine viewEngine = new ViewEngine();
           var engineResult = viewEngine.GetHtml(viewCode);

            // compare result!= view engine result
            Assert.Equal(expectedResult,engineResult);
        }
    }
}
