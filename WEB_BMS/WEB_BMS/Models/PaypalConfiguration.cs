using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace WEB_BMS.Controllers
{
    public static class PaypalConfiguration
    {
        public static APIContext GetAPIContext()
        {
            var config = new Dictionary<string, string>
            {
                { "mode", ConfigurationManager.AppSettings["PayPalMode"] },
                { "clientId", ConfigurationManager.AppSettings["PayPalClientId"] },
                { "clientSecret", ConfigurationManager.AppSettings["PayPalClientSecret"] }
            };

            var accessToken = new OAuthTokenCredential(
                config["clientId"],
                config["clientSecret"],
                config).GetAccessToken();

            var apiContext = new APIContext(accessToken)
            {
                Config = config
            };

            return apiContext;
        }
    }

}