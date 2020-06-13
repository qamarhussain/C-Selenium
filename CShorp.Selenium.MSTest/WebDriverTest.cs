using CShorp.Selenium.MSTest.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Threading;

namespace CShorp.Selenium.MSTest
{
    [TestClass]
    public class WebDriverTest
    {
        private const string AppUrl = @"https://www.linkedin.com/login";

        [TestMethod]
        [DataRow(BrowserType.Chrome)]
        //[DataRow(BrowserType.Firefox)]
        //[DataRow(BrowserType.Edge)]
        //[DataRow(BrowserType.IE11)]
        public void VerifyDrivers(BrowserType browserType)
        {
            using (var driver = WebDriverFactory.GetDriver(browserType))
            {
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl(AppUrl);

                IWebElement username = driver.FindElement(ControlProperties.inputUsername);
                IWebElement password = driver.FindElement(ControlProperties.inputPassword);
                IWebElement login = driver.FindElement(ControlProperties.btnLogin);
                username.SendKeys("example@gmail.com");
                password.SendKeys("password");
                login.Click();
                string actualUrl = "https://www.linkedin.com/feed/";
                string expectedUrl = driver.Url;
                Assert.AreEqual(expectedUrl, actualUrl);

                driver.Quit(browserType);
                Assert.IsTrue(true);
            }
        }
    }
}
