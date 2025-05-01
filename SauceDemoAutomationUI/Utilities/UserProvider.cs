namespace SauceDemoAutomationUI.Utilities
{
    public static class UserProvider
    {
        public static (string username, string password) GetStandardUser()
        {
            return ("standard_user", "secret_sauce");
        }

        public static (string username, string password) GetLockedOutUser()
        {
            return ("locked_out_user", "secret_sauce");
        }

        public static (string username, string password) GetProblemUser()
        {
            return ("problem_user", "secret_sauce");
        }

        public static (string username, string password) GetPerformanceGlitchUser()
        {
            return ("performance_glitch_user", "secret_sauce");
        }
    }
}

