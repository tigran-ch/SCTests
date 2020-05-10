using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace SmartClickTests.Pages
{
    class LoginPage : Page
    {
        public LoginPage(RemoteWebDriver driver) : base(driver) { }

        // Login page elements
        private IWebElement txtEmail => _driver.FindElement(By.Name("email"));
        private IWebElement txtPassword => _driver.FindElement(By.Name("password"));
        private IWebElement btnSubmit => _driver.FindElement(By.CssSelector("button[type*=sub]"));

        // Fill text fields and click on Login button
        public void PerformLogin(string email, string password)
        {
            SendKeys(txtEmail, email);
            SendKeys(txtPassword, password);
            btnSubmit.Click();
        }
    }
}
