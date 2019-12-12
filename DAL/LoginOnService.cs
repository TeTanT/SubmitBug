using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SubmitBug.Models;
using SubmitBug.DAL;

namespace SubmitBug.DAL
{
    public class LoginOnService
    {
        
        public bool LoginOn(TB_LoginOn login)
        {
            using (SubBugEntities db = new SubBugEntities())
            {
                var result = db.TB_LoginOn.Where(l => l.LoginName.Equals(login.LoginName) && l.LoginPwd.Equals(login.LoginPwd));
                if(result!=null)
                {
                    return true;
                }
                return false;
            }

        }
    }
}