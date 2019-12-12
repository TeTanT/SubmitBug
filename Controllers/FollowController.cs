using System;

using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;

using System.Web.Mvc;
using log4net;
using SubmitBug.BLL;
using SubmitBug.DAL;
using SubmitBug.Models;
using X.PagedList;

namespace SubmitBug.Controllers
{
    public class FollowController : Controller
    {
        private SubBugEntities db = new SubBugEntities();

        // GET: Follow
        public ActionResult Index(int? page)
        {
            if (Session["LoginName"] != null)
            {
                ILog logger = LogManager.GetLogger(typeof(MvcApplication));
                var lg = ((TB_LoginOn)Session["LoginName"]).AId;
                int pageSize = GetPageSize.IsMobileRequest();
                var pageNumber = page ?? 1;
                
                switch (lg)
                {
                    case 2:
                        var listA = db.TB_Follow.Include(t => t.BugSubmit).Include(t => t.LoginOn).Where(t=>t.BugSubmit.YN=="N").OrderByDescending(t => t.BId).ToList();
                        //return View(tb_Follow.ToList());
                        IPagedList<TB_Follow> pagedListA = listA.ToPagedList(pageNumber, pageSize);

                        return View(pagedListA);
                    case 3:
                        var lid = ((TB_LoginOn)Session["LoginName"]).LId;
                        var listB = db.TB_Follow.Include(t => t.BugSubmit).Include(t => t.LoginOn).Where(t => t.LId == lid&&t.BugSubmit.YN=="N").OrderByDescending(t => t.BId).ToList();
                        //return View(tB_Follow.ToList());
                        

                        IPagedList<TB_Follow> pagedListB = listB.ToPagedList(pageNumber, pageSize);

                        return View(pagedListB);
                }
                
            }
            return Content("<script>alert('您无权限进入该窗体！！');window.location.href='/Home/Index';</script>");
            //return RedirectToAction("Index", "Home");
        }
        public ActionResult LateFollow(int? page)
        {
            var addDate = DateTime.Now.AddDays(-1);
            var list = db.TB_Follow.Include(t => t.BugSubmit).Include(t => t.LoginOn).Where(t => t.BugSubmit.YN == "N" && t.FollowDate < addDate).OrderByDescending(t => t.BId).ToList();

            int pageSize = GetPageSize.IsMobileRequest();
            var pageNumber = page ?? 1;

            IPagedList<TB_Follow> pagedList = list.ToPagedList(pageNumber, pageSize);

            return View(pagedList);
        }

        // GET: Follow/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_Follow tB_Follow = db.TB_Follow.Find(id);
            if (tB_Follow == null)
            {
                return HttpNotFound();
            }
            return View(tB_Follow);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details()
        {
            var Dt = Request.Params["FollowDate"];
            if ( Dt== null)
            {
                Dt = DateTime.Now.ToShortDateString();
            }
            TB_Schedule tb_Follow = new TB_Schedule
            {
                BId = Convert.ToInt32(Request.Params["BId"]),
                FId = Convert.ToInt32(Request.Params["FId"]),
                FinishDate = Convert.ToDateTime(Dt)
            };
            db.TB_Schedule.Add(tb_Follow);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        // GET: Follow/Create
        public ActionResult Create(int? id)
        {
            if (id != null)
            {
                var bid = db.TB_Schedule.FirstOrDefault(l => l.BId == id);
                if (bid==null)
                {
                    if (Session["LoginName"] != null)
                    {
                        var lg = ((TB_LoginOn)Session["LoginName"]).AId;
                        if (lg != 1)
                        {
                            ViewBag.BId = new SelectList(db.TB_BugSubmit.Where(t => t.BId == id), "BId", "AppName");
                            return View();
                        }
                    }
                }
                //ViewData["info"]="工单正在审核中！";
                //return RedirectToAction("Index","Follow");
                return Content("<script>alert('工单正在审核中！');history.go(-1);</script>");
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Follow/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FinishDate,BId,FId")] TB_Schedule tB_Schedule)
        {
            if (ModelState.IsValid)
            {
                
                db.TB_Schedule.Add(tB_Schedule);
                var lg = ((TB_LoginOn)Session["LoginName"]).AId;
                if (lg == 2)
                {
                    TB_BugSubmit YN = db.TB_BugSubmit.Where(t => t.BId == tB_Schedule.BId).FirstOrDefault();
                    YN.YN = "Y";
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BId = new SelectList(db.TB_BugSubmit, "BId", "AppName", tB_Schedule.BId);
            return View(tB_Schedule);
        }

        // GET: Follow/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                var bid = db.TB_Schedule.FirstOrDefault(l => l.FId == id);
                if (bid == null)
                {
                    TB_Follow tB_Follow = db.TB_Follow.Find(id);
                    if (tB_Follow == null)
                    {
                        return HttpNotFound();
                    }
                    ViewBag.BId = new SelectList(db.TB_BugSubmit, "BId", "AppName", tB_Follow.BId);
                    ViewBag.LId = new SelectList(db.TB_LoginOn, "LId", "LoginNo", tB_Follow.LId);
                    return View(tB_Follow);
                }
                return Content("<script>alert('工单正在审核中！');history.go(-1);</script>");
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Follow/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckDateRangeAttribute]
        public ActionResult Edit()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TB_Schedule tb_Schedule = new TB_Schedule
                    {
                        BId = Convert.ToInt32(Request.Params["BId"]),
                        FId = Convert.ToInt32(Request.Params["FId"]),

                        FinishDate = Convert.ToDateTime(Request.Params["FollowDate"])
                        //FinishDate = DateTime.Today
                    };
                    db.TB_Schedule.Add(tb_Schedule);
                    db.SaveChanges();
                    var lg = ((TB_LoginOn)Session["LoginName"]).AId;
                    if (lg == 2)
                    {
                        TB_BugSubmit YN = db.TB_BugSubmit.Where(t => t.BId == tb_Schedule.BId).FirstOrDefault();
                        YN.YN = "Y";
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Edit");
                }

                
            }
            return View();
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
