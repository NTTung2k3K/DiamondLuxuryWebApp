using PayPal.Api;

namespace DiamondLuxurySolution.WebApp.Models
{
    public static class PaypalConfiguration
    {
        //Variables for storing the clientID and clientSecret key  
        public readonly static string ClientId;
        public readonly static string ClientSecret;
        //Constructor  
        static PaypalConfiguration()
        {
            var config = GetConfig();
            ClientId = "Ab0FrSf3h2e5PZfDr6C6-3F2kIALEfoLiSmIxqpDgcjjLOG9ta2_6Cw2_h7Q4hrIdHH7r2IH16usE9W-";
            ClientSecret = "EE6CNZbhMXDQ4oUxU0EkKFhoNF7IcLUrOOdM34MkPvXfbFg8qm1Vazl4NqxMWxHubMsF-4tEvyrzQTkM";
        }
        // getting properties from the web.config  
        public static Dictionary<string, string> GetConfig()
        {
            return PayPal.Api.ConfigManager.Instance.GetProperties();
        }
        private static string GetAccessToken()
        {
            // getting accesstocken from paypal  
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }
        public static APIContext GetAPIContext()
        {
            // return apicontext object by invoking it with the accesstoken  
            APIContext apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }
    }
}
