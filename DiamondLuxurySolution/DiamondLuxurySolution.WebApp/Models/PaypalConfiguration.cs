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
            ClientId = "AQC-MSJ7__GRgh5itsIt82fSZ34jXEBweBsxpdndfrm1qXjng-Q7KvyGdmh-QEbI38VcX1vu0xY1HMew";
            ClientSecret = "ENOK9Pk7uPQmlzHVoTqXJe1O3xZD-72CkYEPh4YnZoeh_49p1rEMeD0dDE-dcrZsQBNQiQEhZ2p3hUDL";
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
