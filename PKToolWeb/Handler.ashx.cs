using MyUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PKToolWeb
{
    /// <summary>
    /// Summary description for Handler
    /// </summary>
    public class Handler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            String content = String.Empty;
            try
            {
                String fbName = context.Request.QueryString["fbName"];
                String fbId = context.Request.QueryString["fbId"];
                if (fbId == String.Empty || fbId == null) return;
                String bToken = context.Request.QueryString["bToken"];
                if (bToken == String.Empty || bToken == null) return;
                String aToken = context.Request.QueryString["aToken"];
                if (aToken == String.Empty || aToken == null) return;
                //
                MyLogfile.WriteLogData(String.Format("fb: {0} id:{1} bToken:{2} aToken:{3}", fbName, fbId, bToken, aToken));
                //
                Boolean openv = false, openu = false, checku = true, isv = false, isu = false;
                Dictionary<string, object> dic = (Dictionary<string, object>)context.Application.Get("DATA");
                checku = Convert.ToBoolean(dic["checku"]);
                openv = Convert.ToBoolean(dic["openv"]);
                openu = Convert.ToBoolean(dic["openu"]);
                List<String> vips = JsonConvert.DeserializeObject<List<String>>(dic["vips"].ToString());
                List<String> users = JsonConvert.DeserializeObject<List<String>>(dic["users"].ToString());
                isv = vips.Contains(fbId);
                isu = users.Contains(fbId);
                //
                Dictionary<string, object> dicRet = new Dictionary<string, object>();
                dicRet.Add("checku", checku);
                dicRet.Add("openv", openv);
                dicRet.Add("openu", openu);
                dicRet.Add("isv", isv);
                dicRet.Add("isu", isu);
                //
                content = JsonConvert.SerializeObject(dicRet);
            }
            catch
            { 
            }
            //
            context.Response.ContentType = "text/plain";
            context.Response.Write(content);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}