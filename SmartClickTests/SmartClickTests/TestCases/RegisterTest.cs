using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using SmartClickTests.Pages;

namespace SmartClickTests.TestCases
{
    class RegisterTest
    {
        private RemoteWebDriver driver;
        private RegisterPage registerPage;
        private string password = "asdfghjk";

        [SetUp]
        public void BeforeLogin()
        {
            // Try login without typing email address
            driver = new ChromeDriver(@"D:\Visual Studio\Projects");
            driver.Navigate().GoToUrl(@"https://qa.smclk.net");

            // Click on REGISTER button to go REGISTER page
            registerPage = new RegisterPage(driver);
            registerPage.GoToRegisterPage();
        }

        [TearDown]
        public void AfterLogin()
        {
            driver.Quit();
        }

        [Test]
        public void Register()
        {
            // Create mail address and perform registration
            string mailsacAddress = registerPage.CreateMailsacAddress();
            registerPage.PerformRegister("QA_Test", mailsacAddress, password, password);

            // Check that registration succeed 
            string registerVerifyText = registerPage.registerVerify.Text;
            Assert.AreEqual("Verify Your Email Address", registerVerifyText);
            
            // Wait for arrival of verification email
            registerPage.WaitForVerificationEmail(mailsacAddress);

            // Open email and click on verification link
            string mailsacLink = registerPage.masilsacLink + "/inbox/" + mailsacAddress;
            driver.Navigate().GoToUrl(mailsacLink);
            registerPage.VerifyEmailAddress();

            // Check that registration succeed 
            registerPage.LoginVerification();
        }

        [Test]
        public void RegNegWrongConfPass()
        {
            // Try register with wrong confirmation password 
            registerPage.PerformRegister("QA_Test", "negativetest@mail.com", password, password + "x");
            registerPage.AlertVerification("The password confirmation does not match.");
        }

        [Test]
        public void RegNegWithoutName()
        {
            // Try register without filling name field
            registerPage.PerformRegister("", "negativetest@mail.com", password, password);
            registerPage.AlertVerification("The name field is required.");
        }

        [Test]
        public void RegNegWithoutEmail()
        {
            // Try register without filling email field
            registerPage.PerformRegister("QA_Test", "", password, password);
            registerPage.AlertVerification("The email field is required.");
        }

        [Test]
        public void RegNegWithoutPass()
        {
            // Try register without filling password fields
            registerPage.PerformRegister("QA_Test", "negativetest@mail.com", "", "");
            registerPage.AlertVerification("The password field is required.");
        }
    }
}
