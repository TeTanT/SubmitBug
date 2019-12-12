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
    public class ScheduleController : Controller
    {
        private SubBugEntities db = new SubBugEntities();

        // GET: Schedule
        public ActionResult Index(int? page)
        {
            if (Session["LoginName"] != null)
            {
                var lg = ((TB_LoginOn)Session["LoginName"]).AId;
                if (lg == 2)
                {
                    var list = db.TB_Schedule.Include(t => t.BugSubmit).Include(t=>t.Follow).Where(t => t.BugSubmit.YN == "N").OrderByDescending(t=>t.BId).ToList();
                    //return View(tb_Follow.ToList());
                    int pageSize = GetPageSize.IsMobileRequest();
                    var pageNumber = page ?? 1;
                    IPagedList<TB_Schedule> pagedList = list.ToPagedList(pageNumber, pageSize);

                    return View(pagedList);
                }
            }
            //return RedirectToAction("Index", "Home");
            return Content("<script>alert('您无权限进入该窗体！！');window.location.href='/Home/Index';</script>");
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("Index", "Home");
            }
            TB_Schedule tB_Schedule = db.TB_Schedule.Find(id);
            if (tB_Schedule == null)
            {
                return HttpNotFound();
            }
            if (Session["LoginName"] != null)
            {
                return View(tB_Schedule);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int id)
        {
            TB_Schedule tb = db.TB_Schedule.Find(id);
            if (tb != null)
            {
                int bid = tb.BId;
                TB_BugSubmit YN = db.TB_BugSubmit.Where(t => t.BId == bid).FirstOrDefault();
                YN.YN = "Y";
                db.SaveChanges();
               
            }
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
