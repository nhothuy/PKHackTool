using MyUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace PKWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public String GetPK(string fbid, string accesstoken, string key)
        {
            MyLogfile.WriteLogData(String.Format("id:{0} accesstoken:{1}", fbid, accesstoken));
            Boolean openv = false, openu = false, checku = true;
            Dictionary<string, object> dic = (Dictionary<string, object>) Application["DATA"];
            //throw new NotImplementedException();
            //Application["DATA"] = new Dictionary<string, object>();
            return null;
        }
    }
}
