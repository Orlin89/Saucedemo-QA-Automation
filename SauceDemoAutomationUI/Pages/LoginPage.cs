using OpenQA.Selenium;

namespace SauceDemoAutomationUI.Pages
{
    public class LoginPage : BasePage
    {
        public LoginPage(IWebDriver driver) : base(driver) { }

        private readonly By usernameField = By.Id("user-name");
        private readonly By passwordField = By.Id("password");
        private readonly By loginButton = By.Id("login-button");
        private readonly By errorMessage = By.XPath("//div[@class='error-message-container error']");

        public void InputUsername(string username)
        {
            Type(usernameField, username);
        }

        public void InputPassword(string password)
        {
            Type(passwordField, password);
        }

        public void ClickLoginButton()
        {
            Click(loginButton);
        }

        public string GetErrorMessage()
        {
            return GetText(errorMessage);
        }
    }
}

