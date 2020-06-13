using CShorp.Selenium.MSTest.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CShorp.Selenium.MSTest
{
    internal static class WebDriverExtensions
    {
        // DefaultWaitTime.
        private const int DefaultWaitTime = 10;
        // Create a default wait time span so we can reuse the most common time span.
        private static readonly TimeSpan DefaultWaitTimeSpan = TimeSpan.FromSeconds(DefaultWaitTime);
        public static IWait<IWebDriver> Wait(this IWebDriver driver) => Wait(driver, DefaultWaitTimeSpan);
        public static IWait<IWebDriver> Wait(this IWebDriver driver, int waitTime) => Wait(driver, TimeSpan.FromSeconds(waitTime));
        public static IWait<IWebDriver> Wait(this IWebDriver driver, TimeSpan waitTimeSpan) => new WebDriverWait(driver, waitTimeSpan);
        public static IWebElement WaitUntilFindElement(this IWebDriver driver, By locator)
        {
            driver.Wait().Until(condition => condition.FindElement(locator));
            return driver.FindElement(locator);
        }
        public static IWebElement WaitUntilFindElement(this IWebDriver driver, By locator, Func<IWebDriver, IWebElement> condition)
        {
            driver.Wait().Until(condition);
            return driver.FindElement(locator);
        }
        public static IWebElement WaitUntilInitialPageLoad(this IWebDriver driver, string titleOnNewPage)
        {
            driver.Wait().Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleIs(titleOnNewPage));
            return driver.WaitUntilFindElementForPageLoadCheck();
        }
        public static IWebElement WaitUntilPageLoad(this IWebDriver driver, string titleOnNewPage, IWebElement elementOnOldPage)
        {
            driver.Wait().Until(SeleniumExtras.WaitHelpers.ExpectedConditions.StalenessOf(elementOnOldPage));
            driver.Wait().Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleIs(titleOnNewPage));
            return driver.WaitUntilFindElementForPageLoadCheck();
        }
        private static IWebElement WaitUntilFindElementForPageLoadCheck(this IWebDriver driver) => driver.WaitUntilFindElement(By.XPath("html"));
        public static void ScrollIntoView(this IWebDriver driver, IWebElement element)
        {
            // Assumes IWebDriver can be cast as IJavaScriptExecuter.
            ScrollIntoView((IJavaScriptExecutor)driver, element);
        }
        private static void ScrollIntoView(IJavaScriptExecutor driver, IWebElement element)
        {
            // The MoveToElement does not scroll the element to the top of the page.
            //new Actions(driver).MoveToElement(session).Perform();
            driver.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }
        public static void Quit(this IWebDriver driver, BrowserType browserType)
        {
            driver.Quit();
            if (browserType != BrowserType.IE11) return;
            EndProcessTree("IEDriverServer.exe");
            EndProcessTree("iexplore.exe");
        }
        private static void EndProcessTree(string imageName)
        {
            // https://docs.microsoft.com/en-us/windows-server/administration/windows-commands/taskkill
            // https://stackoverflow.com/questions/5901679/kill-process-tree-programmatically-in-c-sharp
            // https://stackoverflow.com/questions/36729512/internet-explorer-11-does-not-close-after-selenium-test
            // /f - force process to terminate
            // /fi <Filter> - /fi \"pid gt 0 \" - select all processes
            // /im <ImageName> - select only processes with this image name
            Process.Start(new ProcessStartInfo
            {
                FileName = "taskkill",
                Arguments = $"/f /fi \"pid gt 0\" /im {imageName}",
                CreateNoWindow = true,
                UseShellExecute = false
            })?.WaitForExit();
        }
    }
}
