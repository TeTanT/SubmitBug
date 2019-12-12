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
    public class SysAdminController : Controller
    {
        private SubBugEntities db = new SubBugEntities();
        // GET: SysAdmin
        public ActionResult Index(int? page)
        {
            if (Session["LoginName"] != null)
            {
                var lg = ((TB_LoginOn)Session["LoginName"]).AId;
                var result = db.TB_LoginOn.Where(t => t.AId == lg);
                //Console.WriteLine(result);
                if (lg == 2)
                {
                    var list = db.TB_BugSubmit.Include(t => t.LoginOn).Where(t => t.BId != t.Follow.FirstOrDefault().BId && t.YN == "N").OrderByDescending(t=>t.BId);
                    int pageSize = GetPageSize.IsMobileRequest();
                    var pageNumber = page ?? 1;

                    IPagedList<TB_BugSubmit> pagedList = list.ToPagedList(pageNumber, pageSize);
                    return View(pagedList);
                }

            }
            //ViewData["info"] = "您无权进入该窗体！";
            return Content("<script>alert('您无权限进入该窗体！！');window.location.href='/Home/Index';</script>");
            //return RedirectToAction("Index", "Home");

        }

        public ActionResult Create()
        {
            if (Session["LoginName"] != null)
            {
                var lg = ((TB_LoginOn)Session["LoginName"]).AId;
                var result = db.TB_LoginOn.Where(t => t.AId == lg);
                //Console.WriteLine(result);
                if (lg == 2)
                {
                    //显示未分配工单
                    ViewBag.BId = new SelectList(db.TB_BugSubmit.Where(t => t.YN == "N").Where(t => t.BId != t.Follow.FirstOrDefault().BId), "BId", "AppName");
                    ViewBag.LId = new SelectList(db.TB_LoginOn.Where(t => t.AId != 1), "LId", "LoginName");
                    return View();
                }
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: TB_Follow/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FId,Schedule,FollowDate,LId,BId,DId")] TB_Follow Follow)
        {
            if (ModelState.IsValid)
            {
                db.TB_Follow.Add(Follow);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BId = new SelectList(db.TB_BugSubmit, "BId", "AppName", Follow.BId);
            ViewBag.LId = new SelectList(db.TB_LoginOn, "LId", "LoginName", Follow.LId);
            ViewBag.LId = new SelectList(db.TB_LoginOn, "DId", "DLevel", Follow.LId);
            return View(Follow);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_BugSubmit tB_BugSubmit = db.TB_BugSubmit.Find(id);
            //ViewBag.BId = new SelectList(db.TB_BugSubmit.Where(t => t.YN == "N").Where(t => t.BId != t.Follow.FirstOrDefault().BId), "BId", "AppName");
            ViewBag.LId = new SelectList(db.TB_LoginOn.Where(t => t.AId != 1), "LId", "LoginName");
            if (tB_BugSubmit == null)
            {
                return HttpNotFound();
            }
            return View(tB_BugSubmit);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_BugSubmit tB_BugSubmit = db.TB_BugSubmit.Find(id);
            //ViewBag.BId = new SelectList(db.TB_BugSubmit.Where(t => t.YN == "N").Where(t => t.BId != t.Follow.FirstOrDefault().BId), "BId", "AppName");
            ViewBag.LId = new SelectList(db.TB_LoginOn.Where(t => t.AId != 1), "LId", "LoginName");
            ViewBag.DId = new SelectList(db.TB_Difficultiy, "DId", "DLevel");
            if (tB_BugSubmit == null)
            {
                return HttpNotFound();
            }
            return View(tB_BugSubmit);
            
        }

        // POST: TB_Follow/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Schedule,FollowDate,LId,BId,DId")] TB_Follow follow)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(follow).State = EntityState.Modified;
                follow = new TB_Follow()
                {
                    LId =Convert.ToInt32(Request.Params["LId"]),
                    BId= Convert.ToInt32(Request.Params["BId"]),
                    DId = Convert.ToInt32(Request.Params["DId"]),
                    Schedule = Request.Params["Schedule"],
                    FollowDate=Convert.ToDateTime(Request.Params["FollowDate"])
                };
                db.TB_Follow.Add(follow);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DId = new SelectList(db.TB_Difficultiy, "DId", "DLevel", follow.DId);
            ViewBag.LId = new SelectList(db.TB_LoginOn, "LId", "LoginNo", follow.LId);
            return View();
        }
    }
}