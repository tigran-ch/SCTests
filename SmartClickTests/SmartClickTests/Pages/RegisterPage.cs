using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using RestSharp;

namespace SmartClickTests.Pages
{
    class RegisterPage : Page
    {
        public RegisterPage(RemoteWebDriver driver) : base(driver) { }

        public string masilsacLink = "https://mailsac.com";
        private string apiKay = "XadBM8C8VxWzEis8rqbvXGhWgrMStg";
        private string mailsacUsername = "tigranch";
        private string mailsacPassword = "tigranch";

        // Register page elements
        private IWebElement txtName => _driver.FindElement(By.Name("name"));
        private IWebElement txtEmail => _driver.FindElement(By.Name("email"));
        private IWebElement txtPassword => _driver.FindElement(By.Name("password"));
        private IWebElement txtPasswordConf => _driver.FindElement(By.Name("password_confirmation"));
        private IWebElement btnRegister => _driver.FindElement(By.CssSelector("button[type*=sub]"));
        public IWebElement registerVerify => _driver.FindElement(By.CssSelector("div[class^=card-header]"));

        // MailSac(verification email) page elements
        private IWebElement verificationEmail => _driver.FindElementByCssSelector("strong[class*=ng-binding]");
        private IWebElement btnUnblockLinks => _driver.FindElementByCssSelector("a[ng-href*=\"dirty\"]");
        private IWebElement btnVerifyEmailAddress => _driver.FindElementByCssSelector("a[class*=\"button button-primary\"]");
        private IWebElement txtMailsacUsername => _driver.FindElementByCssSelector("input[type=\"text\"]");
        private IWebElement txtMailsacPassword => _driver.FindElementByCssSelector("input[type=\"password\"]");
        private IWebElement btnMailsacSignIn => _driver.FindElementByCssSelector("button[type=\"submit\"]");

        // Fill text fields and click on Register button
        public void PerformRegister(string name, string email, string password, string passwordConf)
        {
            SendKeys(txtName, name);
            SendKeys(txtEmail, email);
            SendKeys(txtPassword, password);
            SendKeys(txtPasswordConf, passwordConf);
            btnRegister.Click();
        }

        // Open verification email and click on verification link
        public void VerifyEmailAddress()
        {
            verificationEmail.Click();
            btnUnblockLinks.Click();
            _driver.SwitchTo().Window(_driver.WindowHandles[1]);
            mailsacLogin();
            btnVerifyEmailAddress.Click();
            _driver.SwitchTo().Window(_driver.WindowHandles[2]);
        }

        private void mailsacLogin()
        {
            SendKeys(txtMailsacUsername, mailsacUsername);
            SendKeys(txtMailsacPassword, mailsacPassword);
            btnMailsacSignIn.Submit();
            Thread.Sleep(5000);
        }

        public void WaitForVerificationEmail(string emailAddress)
        {
            DefaultWait<bool> wait = new DefaultWait<bool>(true);
            wait.Timeout = TimeSpan.FromSeconds(60);
            Func<Boolean, bool> waiter = new Func<Boolean, bool>((Boolean bo) =>
            {
                Thread.Sleep(1000);
                return IsVerivicationEmailArrive(emailAddress);
            });
            wait.Until(waiter);
        }

        // Check verification email arrival
        public bool IsVerivicationEmailArrive(string emailAddress)
        {
            RestClient client = new RestClient(masilsacLink);
            string resource = "/api/addresses/" + emailAddress.ToLower() + "/messages?_mailsacKey=" + apiKay;
            RestRequest request = new RestRequest(resource, DataFormat.Json);
            IRestResponse response = client.Get(request);
            string jsonContent = response.Content;
            return jsonContent.Contains("QA_task");
        }
    }
}
