using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SubmitBug.DAL;
using SubmitBug.Models;

namespace SubmitBug.BLL
{
    public class LoginOnManager
    {
        LoginOnService service = new LoginOnService();
        public bool LoginOn(TB_LoginOn login)
        {
            return service.LoginOn(login);
        }
    }
}