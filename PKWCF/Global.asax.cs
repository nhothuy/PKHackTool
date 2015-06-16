using MyUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace PKWCF
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            //
            Application["DATA"] = new Dictionary<string, object>();
            try
            {
                String pathToFiles = Server.MapPath("/data.pk");
                String content = MyFile.ReadFile(pathToFiles);
                Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
                if (dic != null) Application["DATA"] = dic;
            }
            catch
            { 
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}