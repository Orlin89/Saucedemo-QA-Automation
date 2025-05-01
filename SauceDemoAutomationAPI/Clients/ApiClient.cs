using RestSharp;

namespace SauceDemoAutomationAPI.Clients
{
    public class LoginApiClient
    {
        private readonly RestClient _client;

        public LoginApiClient()
        {
            _client = new RestClient("https://www.saucedemo.com/");
        }

        public RestResponse Login(string username, string password)
        {
            var request = new RestRequest("v1/login", Method.Post);
            request.AddJsonBody(new { username, password });

            return _client.Execute(request);
        }
    }
}

