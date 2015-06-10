using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PKTool
{
    public class WebPlayerServerCommunicator
    {
        private readonly string ServerBaseAddress;
        protected string BuildURL(string FunctionName, params string[] parameters)
        {
            StringBuilder stringBuilder = new StringBuilder(string.Format("{0}/{1}", (object)this.ServerBaseAddress, (object)FunctionName));
            if (parameters == null || parameters.Length == 0)
            {
                stringBuilder.Append("/");
            }
            else
            {
                foreach (string str in parameters)
                    stringBuilder.AppendFormat("/{0}", (object)str);
            }
            stringBuilder.AppendFormat("?{0}", (object)DateTime.Now.ToOADate());
            return new Uri(stringBuilder.ToString()).AbsoluteUri;
        }
    }
}
