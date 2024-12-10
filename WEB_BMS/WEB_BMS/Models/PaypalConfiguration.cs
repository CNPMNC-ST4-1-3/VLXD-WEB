using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB_BMS.Controllers
{
    public static class PaypalConfiguration
    {
        public static APIContext GetAPIContext()
        {
            var config = new Dictionary<string, string>
            {
                { "mode", "sandbox" }, // sandbox hoặc live
                { "clientId", "AWJ6QqZq22_2N2dnKl7d_eLAX4Qx-gfi5mMrLtJnaOgGb_ycFpzg7Fg8DEj8uP1sDgUxCs2-tr8O3-yw" },
                { "clientSecret", "EAeQ7EonHH6dvBlM8Ase34j4eKJjWKVeXYNiOoQ0Ho-1AI_hKqSmUsqMVY6AuFcNlbFpCUmLCyeNhp3c" }
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