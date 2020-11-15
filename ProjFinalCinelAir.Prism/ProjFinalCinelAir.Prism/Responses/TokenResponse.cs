using System;
using System.Collections.Generic;
using System.Text;

namespace ProjFinalCinelAir.Prism.Responses
{
    public class TokenResponse
    {
        public string Token { get; set; }

        public UserResponse userModel { get; set; }

        public DateTime Expiration { get; set; }

        public DateTime ExpirationLocal => Expiration.ToLocalTime();
    }

}
