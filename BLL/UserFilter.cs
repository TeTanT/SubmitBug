using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SubmitBug.BLL
{
    public class UserFilter: AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //base.OnAuthorization(filterContext);    
            var user = filterContext.HttpContext.Session["LoginName"];
            if (user == null || !string.IsNullOrWhiteSpace(user.ToString()))
            {
                //return RedirectToAction("Index", "Home");
                //Content("<script>alert('密码修改成功！');window.location.href='Login';</script>");
                
                filterContext.Result = new RedirectResult("~/Home/Index");

            }
            else
            {
                return;
            }

        }
    }
}