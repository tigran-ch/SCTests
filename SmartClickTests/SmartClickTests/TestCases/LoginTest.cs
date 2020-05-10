using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using SmartClickTests.Pages;
using OpenQA.Selenium.Remote;

namespace SmartClickTests.TestCases
{
    class LoginTest
    {
        private RemoteWebDriver driver;
        private LoginPage loginPage;


        [SetUp] 
        public void BeforeLogin()
        {
            // Open the browser and go by URL
            driver = new ChromeDriver(@"D:\Visual Studio\Projects");
            driver.Navigate().GoToUrl(@"https://qa.smclk.net");

            // Click on LOGIN button to go login page
            loginPage = new LoginPage(driver);
            loginPage.GoToLoginPage();
        }

        [TearDown]
        public void AfterLogin()
        {
            driver.Close();
        }

        [Test]
        public void Login()
        {
            // Perform login and check that login succeed 
            loginPage.PerformLogin(@"testing2021@inbox.ru", @"asdfghjk");
            loginPage.LoginVerification();
        }

        [Test]
        public void LoginWithNegativePassword()
        {
            // Try login with wrong password
            loginPage.PerformLogin(@"testing2021@inbox.ru", @"negativePassword");
            loginPage.AlertVerification("These credentials do not match our records.");
        }

        [Test]
        public void LoginWithNegativeEmail()
        {
            // Try login with not registered email
            loginPage.PerformLogin(@"notRegisteredEmail@inbox.ru", @"asdfghjk");
            loginPage.AlertVerification("These credentials do not match our records.");
        }

        [Test]
        public void LoginWithoutEmail()
        {
            // Try login without typing email address
            loginPage.PerformLogin("", @"asdfghjk");
            loginPage.AlertVerification("The email field is required.");
        }
    }
}
