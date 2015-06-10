using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PKTool
{
    public class BasePlayerRequest
    {
        public string secretKey { get; set; }

        public string sessionToken { get; set; }

        public string businessToken { get; set; }

        public BasePlayerRequest(string secretKey, string sessionToken, string businessToken)
        {
            this.secretKey = secretKey;
            this.sessionToken = sessionToken;
            this.businessToken = businessToken;
        }
    }
}
