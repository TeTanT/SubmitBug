using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SubmitBug.BLL
{
    public class GetPageSize
    {
        public static int IsMobileRequest()
        {
            string uAgent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
            string[] mobileAgents = { "iphone", "android" };
            bool isMoblie = false;
            if (uAgent != null)
            {
                for (int i = 0; i < mobileAgents.Length; i++)
                {
                    if (uAgent.ToLower().IndexOf(mobileAgents[i], System.StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        isMoblie = true;
                        break;
                    }
                }
            }
            if (isMoblie)
            {
                return int.Parse(ConfigurationManager.AppSettings["ApageSize"]);
            }
            else
            {
                return int.Parse(ConfigurationManager.AppSettings["WpageSize"]); ;
            }
        }
    }
}