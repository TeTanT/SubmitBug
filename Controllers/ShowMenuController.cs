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
    public class ShowMenuController : Controller
    {
        private SubBugEntities db = new SubBugEntities();

        // GET: ShowMenu
        public ActionResult Index()
        {
            var tB_ShowMenu = db.TB_ShowMenu.Include(t => t.UserAccess);
            return View(tB_ShowMenu.ToList());
        }

        // GET: ShowMenu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_ShowMenu tB_ShowMenu = db.TB_ShowMenu.Find(id);
            if (tB_ShowMenu == null)
            {
                return HttpNotFound();
            }
            return View(tB_ShowMenu);
        }

        // GET: ShowMenu/Create
        public ActionResult Create()
        {
            ViewBag.AId = new SelectList(db.TB_UserAccess, "AId", "AccessName");
            return View();
        }

        // POST: ShowMenu/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MId,MenuName,AId")] TB_ShowMenu tB_ShowMenu)
        {
            if (ModelState.IsValid)
            {
                db.TB_ShowMenu.Add(tB_ShowMenu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AId = new SelectList(db.TB_UserAccess, "AId", "AccessName", tB_ShowMenu.AId);
            return View(tB_ShowMenu);
        }

        // GET: ShowMenu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_ShowMenu tB_ShowMenu = db.TB_ShowMenu.Find(id);
            if (tB_ShowMenu == null)
            {
                return HttpNotFound();
            }
            ViewBag.AId = new SelectList(db.TB_UserAccess, "AId", "AccessName", tB_ShowMenu.AId);
            return View(tB_ShowMenu);
        }

        // POST: ShowMenu/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MId,MenuName,AId")] TB_ShowMenu tB_ShowMenu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tB_ShowMenu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AId = new SelectList(db.TB_UserAccess, "AId", "AccessName", tB_ShowMenu.AId);
            return View(tB_ShowMenu);
        }

        // GET: ShowMenu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_ShowMenu tB_ShowMenu = db.TB_ShowMenu.Find(id);
            if (tB_ShowMenu == null)
            {
                return HttpNotFound();
            }
            return View(tB_ShowMenu);
        }

        // POST: ShowMenu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TB_ShowMenu tB_ShowMenu = db.TB_ShowMenu.Find(id);
            db.TB_ShowMenu.Remove(tB_ShowMenu);
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
