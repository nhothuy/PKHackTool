using MyUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Hosting;

namespace PKWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class pkTool : IpkTool
    {
        
        public String getPK(String fbName, String fbId, String bToken, String aToken)
        {
            try
            {
                MyLogfile.WriteLogData(String.Format("fb: {0} id:{1} bToken:{2} aToken:{3}", fbName, fbId, bToken, aToken));
                Boolean openv = false, openu = false, checku = true, isv = false, isu = false;
                String pathData = HostingEnvironment.MapPath("~/App_Data/data.pk");
                String data = MyFile.ReadFile(pathData);
                //
                Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
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
                return JsonConvert.SerializeObject(dicRet);
            }
            catch
            {
                return String.Empty;
            }
            
        }
    }
}
