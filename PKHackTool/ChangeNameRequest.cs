using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PKTool
{
    internal class ChangeNameRequest : BasePlayerRequest
    {
        public string NewName;
        public string NewAvatar;

        public ChangeNameRequest(string secretKey, string sessionToken, string businessToken, string newName, string newAvatar)
            : base(secretKey, sessionToken, businessToken)
        {
            this.NewName = newName;
            this.NewAvatar = newAvatar;
        }
    }
}
