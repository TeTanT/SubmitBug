using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace SubmitBug.BLL
{
    public class GetRoles
    {
        public static string GetActionRoles(string action, string controller)
        {
            XElement rootElement = XElement.Load(HttpContext.Current.Server.MapPath("/") + "ActionRoles.xml");
            XElement controllerElement = FindElementByAttribute(rootElement, "Controller", controller);
            if (controllerElement != null)
            {
                XElement actionElement = FindElementByAttribute(controllerElement, "Action", action);
                if (actionElement != null)
                {
                    return actionElement.Value;
                }
            }
            return "";
        }

        public static XElement FindElementByAttribute(XElement xElement, string tagName, string attribute)
        {
            return xElement.Elements(tagName).FirstOrDefault(x => x.Attribute("name").Value.Equals(attribute, StringComparison.OrdinalIgnoreCase));
        }
    }
}