using SubmitBug.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SubmitBug.BLL;

using SubmitBug.Models;


namespace SubmitBug.Controllers
{
    public class AccountController : Controller
    {
        //SubBugEntities db = new SubBugEntities();
        //LoginOnManager loginOnManager = new LoginOnManager();
        MD5DataEncryption md5 = new MD5DataEncryption();
        // GET: Account
        public ActionResult LogOff()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public class InputLogin
        {
            [Required]
            [MaxLength(20), Display(Name = "工号")]
            public string LoginNo { get; set; }
            
            [Required]
            [Display(Name = "密码"), DataType(DataType.Password)]
            public string LoginPwd { get; set; }

        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(InputLogin model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //var result = loginOnManager.LoginOn(model);
            using (SubBugEntities db = new SubBugEntities())
            {
                var Pwd = md5.MD5Encrypt(model.LoginPwd);
                var result = await db.TB_LoginOn.FirstOrDefaultAsync(l => l.LoginNo==model.LoginNo && l.LoginPwd==Pwd);

                if (result != null)
                {
                    Session["LoginName"] = result;
                    if (((TB_LoginOn)Session["LoginName"]).AId != 1)
                    {
                        return RedirectToAction("Index", "Follow");
                    }
                    
                    return RedirectToAction("List", "Home");
                }

                ViewData["info"] = "失败！";
                return View(model);

            }
        }

        public class InputModel
        {
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

        }
        public ActionResult SetPwd(int? id)
        {
            if (Session["LoginName"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult SetPwd(TB_LoginOn login)
        {
            using (SubBugEntities db = new SubBugEntities())
            {
                var lid= ((TB_LoginOn)Session["LoginName"]).LId;
                TB_LoginOn lg = db.TB_LoginOn.Where(t => t.LId == lid).FirstOrDefault();
                lg.LoginPwd = md5.MD5Encrypt(login.LoginPwd);
                db.SaveChanges();
                Session.Clear();

                return Content("<script>alert('密码修改成功！');window.location.href='Login';</script>");
            }
            
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(TB_LoginOn model)
        {
            if (ModelState.IsValid)
            {
                using (SubBugEntities db = new SubBugEntities())
                {
                    if (await db.TB_LoginOn.FirstOrDefaultAsync(l => l.LoginNo==model.LoginNo) != null)
                    {
                        ModelState.AddModelError("", "用户名已经存在！");
                        return View();
                    }
                    else
                    {
                        model = new TB_LoginOn()
                        {
                            LoginNo = Request.Params["LoginNo"].ToUpper(),
                            LoginPwd = md5.MD5Encrypt(Request.Params["LoginPwd"]),
                            LoginName=Request.Params["LoginName"].ToUpper(),
                            LPhone=Request.Params["LPhone"],
                            ComputerNo=Request.Params["ComputerNo"].ToUpper(),
                            AId=1
                        };
                        db.TB_LoginOn.Add(model);
                        await db.SaveChangesAsync();
                        return RedirectToAction("Login", "Account");
                    }

                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LId,LoginNo,LoginName,LoginPwd,LPhone,ComputerNo")] TB_LoginOn loginOn)
        {
            if (ModelState.IsValid)
            {
                using (SubBugEntities db = new SubBugEntities())
                {
                    db.Entry(loginOn).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(loginOn);
        }
    }
}