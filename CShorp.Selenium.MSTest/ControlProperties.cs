using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace CShorp.Selenium.MSTest
{
    public static class ControlProperties
    {
        public static By inputUsername = By.Id("username");
        public static By inputPassword = By.Id("password");
        public static By btnLogin = By.XPath("//button[text()='Sign in']");
    }
}
