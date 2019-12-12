using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SubmitBug.DAL;
using SubmitBug.Models;

namespace SubmitBug.Controllers
{
    public class UserAccessController : Controller
    {
        private SubBugEntities db = new SubBugEntities();

        // GET: UserAccess
        public ActionResult Index()
        {
            if (Session["LoginName"] != null)
            {
                var lg = ((TB_LoginOn)Session["LoginName"]).AId;
                var result = db.TB_LoginOn.Where(t => t.AId == lg);

                if (lg == 2)
                {
                    return View(db.TB_UserAccess.ToList());
                }
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: UserAccess/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_UserAccess tB_UserAccess = db.TB_UserAccess.Find(id);
            if (tB_UserAccess == null)
            {
                return HttpNotFound();
            }
            return View(tB_UserAccess);
        }

        // GET: UserAccess/Create
        public ActionResult Create()
        {
            if (Session["LoginName"] != null)
            {
                var lg = ((TB_LoginOn)Session["LoginName"]).AId;
                var result = db.TB_LoginOn.Where(t => t.AId == lg);

                if (lg == 2)
                {
                    return View();
                }
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: UserAccess/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AId,AccessName")] TB_UserAccess tB_UserAccess)
        {
            if (ModelState.IsValid)
            {
                db.TB_UserAccess.Add(tB_UserAccess);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tB_UserAccess);
        }

        // GET: UserAccess/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_UserAccess tB_UserAccess = db.TB_UserAccess.Find(id);
            if (tB_UserAccess == null)
            {
                return HttpNotFound();
            }
            return View(tB_UserAccess);
        }

        // POST: UserAccess/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AId,AccessName")] TB_UserAccess tB_UserAccess)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tB_UserAccess).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tB_UserAccess);
        }

        // GET: UserAccess/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_UserAccess tB_UserAccess = db.TB_UserAccess.Find(id);
            if (tB_UserAccess == null)
            {
                return HttpNotFound();
            }
            return View(tB_UserAccess);
        }

        // POST: UserAccess/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TB_UserAccess tB_UserAccess = db.TB_UserAccess.Find(id);
            db.TB_UserAccess.Remove(tB_UserAccess);
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
