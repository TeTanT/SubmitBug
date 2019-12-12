using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SubmitBug.BLL;
using SubmitBug.DAL;
using SubmitBug.Models;
using X.PagedList;

namespace SubmitBug.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        //public X.PagedList.IPagedList<TB_BugSubmit> BugSubmit { get; set; }
        public async Task<ActionResult> Index(int? page)
        {
            SubBugEntities db = new SubBugEntities();
            
            var list = await db.TB_BugSubmit.Where(t => t.YN == "N").OrderByDescending(d => d.BId).ToListAsync();
            

            //int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);
            int pageSize = GetPageSize.IsMobileRequest();
            var pageNumber = page ?? 1;
           
            IPagedList<TB_BugSubmit> pagedList = list.ToPagedList(pageNumber, pageSize);

            return View(pagedList);
        }

        public async Task<ActionResult> List()
        {
            using (SubBugEntities db = new SubBugEntities())
            {
                var list = new List<TB_BugSubmit>();
                //var listB = new List<TB_Follow>();
                if (Session["LoginName"] != null)
                {
                    var lid = ((TB_LoginOn)Session["LoginName"]).LId;

                    list = await db.TB_BugSubmit.Include(t => t.LoginOn).Where(t => t.LId.Equals(lid) && t.YN == "N").OrderByDescending(d => d.BId).ToListAsync();
                    //listB = db.TB_Follow.Include(t => t.BugSubmit).Where(t=>t.LId==lid&&t.BugSubmit.YN=="N").OrderByDescending(d => d.BId).ToList();
                    //return View(listB);
                }
                else
                {
                    list = await db.TB_BugSubmit.Where(t => t.YN == "N").OrderByDescending(d => d.BId).ToListAsync();

                }
                return View(list);
            }
        }

       
        public ActionResult SelectList(int? page,string searchString,string strST,string strET,string currentFilter,string currentST,string currentET)
        {
            SubBugEntities db = new SubBugEntities();
            List<TB_LoginOn> listSelect = db.TB_LoginOn.Where(t => t.AId!=1&&t.LoginName!="Guest").ToList();
           
            //ViewData["STime"] = strST;
            //ViewData["ETime"] = strET;
            //ViewData["LoginName"] = searchString;
            searchString = Request.Params["LoginName"];
            strST = Request.Params["STime"];
            strET = Request.Params["ETime"];
            
            var list = new List<TB_Schedule>();
            int pageSize =GetPageSize.IsMobileRequest();
            var pageNumber = page ?? 1;

            if (!string.IsNullOrWhiteSpace(searchString)|| (strST!=null&&strET!=null))
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
                strST = currentST;
                strET= currentET;
            }
            SelectList selList = new SelectList(listSelect, "LoginName", "LoginName",searchString);
            ViewData["Select"] = selList;
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentST"] = strST;
            ViewData["CurrentET"] = strET;

            list = db.TB_Schedule.Include(t => t.Follow).Include(t => t.BugSubmit).Where(t=>t.BugSubmit.YN=="Y").OrderByDescending(t => t.FinishDate).ToList();
            //int TotalCount = list.Count();
            if (!string.IsNullOrWhiteSpace(searchString)&&!string.IsNullOrEmpty(strST) && !string.IsNullOrEmpty(strET))
            {
                //list = list.Where(t => t.Follow.LoginOn.LoginName.Contains(searchString)).ToList();
                list = list.Where(t => t.FinishDate >= Convert.ToDateTime(strST) && t.FinishDate <= Convert.ToDateTime(strET) &&t.Follow.LoginOn.LoginName.Contains(searchString)).ToList();
                IPagedList<TB_Schedule> pagedListD = list.ToPagedList(pageNumber, pageSize);
                //TotalCount = list.Count();
                return View(pagedListD);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    list = list.Where(t => t.Follow.LoginOn.LoginName.Contains(searchString)).ToList();
                    IPagedList<TB_Schedule> pagedListA = list.ToPagedList(pageNumber, pageSize);
                    //TotalCount = list.Count();
                    return View(pagedListA);
                }
                if (!string.IsNullOrEmpty(strST) && !string.IsNullOrEmpty(strET))
                {
                    list = list.Where(t => t.FinishDate >= Convert.ToDateTime(strST) && t.FinishDate <= Convert.ToDateTime(strET)).ToList();
                    IPagedList<TB_Schedule> pagedListB = list.ToPagedList(pageNumber, pageSize);
                    //TotalCount = list.Count();
                    return View(pagedListB);
                }
            }
            IPagedList<TB_Schedule> pagedListC = list.ToPagedList(pageNumber, pageSize);
            return View(pagedListC);   
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "电脑部工单提交程序！";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "联系方式";

            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Create(TB_BugSubmit model)
        {
            if (Session["LoginName"] != null)
            {
                model.LId = ((TB_LoginOn)Session["LoginName"]).LId;
                model.Claimant = ((TB_LoginOn)Session["LoginName"]).LoginName;
                model.LPhone = ((TB_LoginOn)Session["LoginName"]).LPhone;
                model.ComputerNo = ((TB_LoginOn)Session["LoginName"]).ComputerNo;
            }
            else
            {
                if (ModelState.IsValid)
                {
                    model.LId = 1;
                    model.AppName = Request.Params["AppName"].ToUpper();
                    model.Describe = Request.Params["Describe"];
                    model.Claimant = Request.Params["Claimant"].ToUpper();
                    model.LPhone = Request.Params["LPhone"].ToUpper();
                    model.ComputerNo = Request.Params["ComputerNo"].ToUpper();
                }
            }

            model.LIP = System.Web.HttpContext.Current.Request.UserHostAddress;
            model.SubDate = DateTime.Now;
            model.YN = "N";

            using (SubBugEntities db = new SubBugEntities())
            {
                try
                {
                    db.TB_BugSubmit.Add(model);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
                catch
                {
                    return View();
                }
            }


        }

        public ActionResult Details(int? id)
        {
            
            if (id == null)
            {
                return View();
            }
            SubBugEntities db = new SubBugEntities();
            TB_BugSubmit follow = db.TB_BugSubmit.Find(id);//db.TB_BugSubmit.Include(t => t.Follow).Where(t => t.BId == id);
            
            //var follow =db.TB_Follow.Where(t => t.BId == id);
            if (follow == null)
            {
                return HttpNotFound();
            }
            return View(follow);
        }

        public ActionResult LoginNameList(string Lg)
        {
            if(Lg==null)
            {
                return RedirectToAction("Index", "Home");
            }

            using (SubBugEntities db = new SubBugEntities())
            {
                var list = new List<TB_BugSubmit>();
                var tB_Follow = db.TB_Follow.Include(t => t.BugSubmit).Include(t => t.LoginOn).Where(t =>t.BugSubmit.YN == "N"&&t.LoginOn.LoginName==Lg).OrderByDescending(t => t.BId);
                return View(tB_Follow.ToList());
            }
        }

        
        public string GetIp()
        {

            string userIP =string.Empty;

            try
            {
                if (System.Web.HttpContext.Current == null
                 || System.Web.HttpContext.Current.Request == null
                 || System.Web.HttpContext.Current.Request.ServerVariables == null)
                {
                    return "";
                }

                string CustomerIP = "";

                //CDN加速后取到的IP simone 090805
                CustomerIP = System.Web.HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
                if (!string.IsNullOrEmpty(CustomerIP))
                {
                    return CustomerIP;
                }

                CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (!String.IsNullOrEmpty(CustomerIP))
                {
                    return CustomerIP;
                }

                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                    if (CustomerIP == null)
                    {
                        CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    }
                }
                else
                {
                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                if (string.Compare(CustomerIP, "unknown", true) == 0 || String.IsNullOrEmpty(CustomerIP))
                {
                    return System.Web.HttpContext.Current.Request.UserHostAddress;
                }
                return CustomerIP;
            }
            catch { }

            return userIP;

        }
    }
}