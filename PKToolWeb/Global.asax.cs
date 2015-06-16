using MyUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace PKToolWeb
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                String dataPath = Server.MapPath("~/App_Data/data.pk");
                String data = MyFile.ReadFile(dataPath);
                dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            }
            catch
            { 
            }
            Application["DATA"] = dic;
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