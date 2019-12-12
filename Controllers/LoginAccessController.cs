using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using SubmitBug.BLL;
using SubmitBug.DAL;
using SubmitBug.Models;
using X.PagedList;

namespace SubmitBug.Controllers
{
    public class LoginAccessController : Controller
    {
        private SubBugEntities db = new SubBugEntities();
        MD5DataEncryption md5 = new MD5DataEncryption();
        // GET: LoginAccess
        public ActionResult Index(int? page)
        {
            if (Session["LoginName"] != null)
            {
                var lg = ((TB_LoginOn)Session["LoginName"]).AId;
                var result = db.TB_LoginOn.Where(t => t.AId == lg);
                //Console.WriteLine(result);
                if (lg == 2)
                {
                    var list = db.TB_LoginOn.Include(t => t.UserAccess).OrderByDescending(t=>t.LId).ToList();
                    //return View(tB_LoginOn.ToList());
                    int pageSize = GetPageSize.IsMobileRequest();
                    var pageNumber = page ?? 1;

                    IPagedList<TB_LoginOn> pagedList = list.ToPagedList(pageNumber, pageSize);

                    return View(pagedList);
                }
            }
            return Content("<script>alert('您无权限进入该窗体！！');window.location.href='/Home/Index';</script>");
            //return RedirectToAction("Index", "Home");
            //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult SetPwd(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var lg = db.TB_LoginOn.FirstOrDefault(t => t.LId == id);
            lg.LoginPwd=md5.MD5Encrypt("123456");
            db.SaveChanges();
            return Content("<script>alert('密码重置成功！');history.go(-1);</script>");
        }

        // GET: LoginAccess/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_LoginOn tB_LoginOn = db.TB_LoginOn.Find(id);
            if (tB_LoginOn == null)
            {
                return HttpNotFound();
            }
            return View(tB_LoginOn);
        }

        // GET: LoginAccess/Create
        public ActionResult Create()
        {
            if (Session["LoginName"] != null)
            {
                var lg = ((TB_LoginOn)Session["LoginName"]).AId;
                var result = db.TB_LoginOn.Where(t => t.AId == lg);
                //Console.WriteLine(result);
                if (lg == 2)
                {
                    ViewBag.AId = new SelectList(db.TB_UserAccess, "AId", "AccessName");
                    return View();
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: LoginAccess/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LId,LoginNo,LoginName,LoginPwd,LPhone,ComputerNo,AId")] TB_LoginOn tB_LoginOn)
        {
            if (ModelState.IsValid)
            {
                db.TB_LoginOn.Add(tB_LoginOn);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AId = new SelectList(db.TB_UserAccess, "AId", "AccessName", tB_LoginOn.AId);
            return View(tB_LoginOn);
        }

        // GET: LoginAccess/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session["LoginName"] != null)
            {
                var lg = ((TB_LoginOn)Session["LoginName"]).AId;
                var result = db.TB_LoginOn.Where(t => t.AId == lg);
                //Console.WriteLine(result);
                if (lg == 2)
                {
                    TB_LoginOn tB_LoginOn = db.TB_LoginOn.Find(id);
                    if (tB_LoginOn == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.AId = new SelectList(db.TB_UserAccess, "AId", "AccessName", tB_LoginOn.AId);
                    return View(tB_LoginOn);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: LoginAccess/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TB_LoginOn tB_LoginOn)
        {
            try
            {
                int lid = Convert.ToInt32(Request.Params["LID"]);
                var lg = db.TB_LoginOn.FirstOrDefault(t => t.LId == lid);
                lg.AId = tB_LoginOn.AId;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.AId = new SelectList(db.TB_UserAccess, "AId", "AccessName", tB_LoginOn.AId);
                return View(tB_LoginOn);
                throw;
            }
           

            
        }

        // GET: LoginAccess/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session["LoginName"] != null)
            {
                var lg = ((TB_LoginOn)Session["LoginName"]).AId;
                var result = db.TB_LoginOn.Where(t => t.AId == lg);
                //Console.WriteLine(result);
                if (lg == 2)
                {
                    TB_LoginOn tB_LoginOn = db.TB_LoginOn.Find(id);
                    if (tB_LoginOn == null)
                    {
                        return HttpNotFound();
                    }
                    return View(tB_LoginOn);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: LoginAccess/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TB_LoginOn tB_LoginOn = db.TB_LoginOn.Find(id);
            db.TB_LoginOn.Remove(tB_LoginOn);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
