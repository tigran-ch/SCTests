using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Remote;
using NUnit.Framework;
using System.Threading;

namespace SmartClickTests.Pages
{
    abstract class Page
    {
        public RemoteWebDriver _driver;

        public Page(RemoteWebDriver driver)
        {
            _driver = driver;
        }

        // Base page elements 
        private IWebElement btnLogin => _driver.FindElement(By.CssSelector("a[href*=login]"));
        private IWebElement btnRegister => _driver.FindElement(By.CssSelector("a[href*=register]"));
        private IWebElement dashboardTextEle => _driver.FindElement(By.CssSelector("div[class^=card-body]"));
        private IWebElement alertText => _driver.FindElement(By.CssSelector("span[role*=alert]"));

        public void GoToLoginPage()
        {
            btnLogin.Click();
        }

        public void GoToRegisterPage()
        {
            btnRegister.Click();
        }

        // Wait for element and fill field
        public void SendKeys(IWebElement element, string keys)
        {
            WaitForElement(element);
            element.SendKeys(keys);
        }

        // Wait until element become visible
        public void WaitForElement(IWebElement element)
        {
            DefaultWait<IWebElement> wait = new DefaultWait<IWebElement>(element);
            wait.Timeout = TimeSpan.FromSeconds(5);
            Func<IWebElement, bool> waiter = new Func<IWebElement, bool>((IWebElement ele) =>
            {
                return element.Displayed && element.Enabled;
            });
            wait.Until(waiter);
            Thread.Sleep(1000);
        }

        public int GenerateRundomeId()
        {
            Random rnd = new Random();
            int mId = rnd.Next(0, 9999);
            return mId;
        }

        // Create email address for regisrtation
        public string CreateMailsacAddress()
        {
            int mId = GenerateRundomeId();
            string mailsacAddress = "SmartClickQATask" + mId + "@mailsac.com";
            return mailsacAddress;
        }

        // Check that login succeed 
        public void LoginVerification()
        {
            string dashboardText = dashboardTextEle.Text;
            Assert.AreEqual("You are logged in!", dashboardText);
        }

        // Check that given right alert
        public void AlertVerification(string expeectedAlert)
        {
            string strAlertText = alertText.Text;
            Assert.AreEqual(expeectedAlert, strAlertText);
        }
    }
}
