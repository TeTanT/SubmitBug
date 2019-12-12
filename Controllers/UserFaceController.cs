using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.EnterpriseServices;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using SubmitBug.DAL;
using SubmitBug.Models;

namespace SubmitBug.Controllers
{
    public class UserFaceController : Controller
    {
        private SubBugEntities db = new SubBugEntities();

        // GET: UserFace
        public ActionResult Index()
        {
            var tB_UserFace = db.TB_UserFace.Include(t => t.LoginOn);
            return View(tB_UserFace.ToList());
        }

        // GET: UserFace/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_UserFace tB_UserFace = db.TB_UserFace.Find(id);
            if (tB_UserFace == null)
            {
                return HttpNotFound();
            }
            return View(tB_UserFace);
        }

        // GET: UserFace/Create
        public ActionResult Create()
        {
            ViewBag.LId = new SelectList(db.TB_LoginOn, "LId", "LoginNo");
            return View();
        }

        // POST: UserFace/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UId,UserFace,LId")] TB_UserFace tB_UserFace)
        {
            if (ModelState.IsValid)
            {
                db.TB_UserFace.Add(tB_UserFace);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LId = new SelectList(db.TB_LoginOn, "LId", "LoginNo", tB_UserFace.LId);
            return View(tB_UserFace);
        }

        // GET: UserFace/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_UserFace tB_UserFace = db.TB_UserFace.Find(id);
            if (tB_UserFace == null)
            {
                return HttpNotFound();
            }
            ViewBag.LId = new SelectList(db.TB_LoginOn, "LId", "LoginNo", tB_UserFace.LId);
            return View(tB_UserFace);
        }

        // POST: UserFace/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UId,UserFace,LId")] TB_UserFace tB_UserFace)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tB_UserFace).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LId = new SelectList(db.TB_LoginOn, "LId", "LoginNo", tB_UserFace.LId);
            return View(tB_UserFace);
        }

        // GET: UserFace/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TB_UserFace tB_UserFace = db.TB_UserFace.Find(id);
            if (tB_UserFace == null)
            {
                return HttpNotFound();
            }
            return View(tB_UserFace);
        }

        // POST: UserFace/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TB_UserFace tB_UserFace = db.TB_UserFace.Find(id);
            db.TB_UserFace.Remove(tB_UserFace);
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

        public class ResultInfo
        {
            public ResultInfo()
            {  //默认值
                res = false;
                startcode = 449;
                info = "";
                other = "";
            }
            public bool res { get; set; }  //返回状态（true or false）
            public int startcode { get; set; } //返回http状态码
            public string info { get; set; }  //返回结果
            public string other { get; set; }  //其他
        }

        /// <summary>
        /// 人脸注册
        /// </summary>
        /// <returns></returns>
        public JsonResult FaceRegistration()
        {
            // 设置APPID/AK/SK
            var API_KEY = "mxzzu1vLxca9KjnLwBCgOZs5";                   //你的 Api Key
            var SECRET_KEY = "D9CkVbdziW9GrHiAZDENt8rOf0tVw9im";        //你的 Secret Key
            var client = new Baidu.Aip.Face.Face(API_KEY, SECRET_KEY);
            client.Timeout = 60000;  // 修改超时时间

            var imageType = "BASE64";  //BASE64   URL
            string imgData64 = Request["imgData64"];
            imgData64 = imgData64.Substring(imgData64.IndexOf(",") + 1);      //将‘，’以前的多余字符串删除

            ResultInfo result = new ResultInfo();
            try
            {
                //注册人脸
                var groupId = "TestGroupA";
                var userId = "TestUserA";
                //首先查询是否存在人脸
                var result2 = client.Search(imgData64, imageType, userId);  //会出现222207（未找到用户）这个错误
                var strJson = Newtonsoft.Json.JsonConvert.SerializeObject(result2);
                var o2 = Newtonsoft.Json.JsonConvert.DeserializeObject(strJson) as JObject;


                //判断是否存在当前人脸，相识度是否大于88
                if (o2["error_code"].ToString() == "0" && o2["error_msg"].ToString() == "SUCCESS")
                {
                    var result_list = Newtonsoft.Json.JsonConvert.DeserializeObject(o2["result"].ToString()) as JObject;
                    var user_list = result_list["user_list"];
                    var Obj = JArray.Parse(user_list.ToString());
                    foreach (var item in Obj)
                    {
                        //88分以上可以判断为同一人，此分值对应万分之一误识率
                        var score = Convert.ToInt32(item["score"]);
                        if (score > 88)
                        {
                            result.info = result2.ToString();
                            result.res = true;
                            result.startcode = 221;
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                var guid = Guid.NewGuid();
                // 调用人脸注册，可能会抛出网络等异常，请使用try/catch捕获
                // 如果有可选参数
                var options = new Dictionary<string, object>{
                            {"user_info", guid}
                        };
                // 带参数调用人脸注册
                var resultData = client.UserAdd(imgData64, imageType, groupId, userId, options);
                result.info = resultData.ToString();
                result.res = true;
                result.other = guid.ToString();
            }
            catch (Exception e)
            {
                result.info = e.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 人脸识别
        /// </summary>
        /// <returns></returns>
        public JsonResult FaceDistinguish()
        {
            // 设置APPID/AK/SK
            var API_KEY = "mxzzu1vLxca9KjnLwBCgOZs5";                   //你的 Api Key
            var SECRET_KEY = "D9CkVbdziW9GrHiAZDENt8rOf0tVw9im";        //你的 Secret Key
            var client = new Baidu.Aip.Face.Face(API_KEY, SECRET_KEY);
            client.Timeout = 60000;  // 修改超时时间

            var imageType = "BASE64";  //BASE64   URL
            string imgData64 = Request["imgData64"];
            imgData64 = imgData64.Substring(imgData64.IndexOf(",") + 1);      //将‘，’以前的多余字符串删除

            ResultInfo result = new ResultInfo();
            try
            {
                var groupId = "TestGroupA";
                var userId = "TestUserA";


                var result323 = client.Detect(imgData64, imageType);


                //活体检测阈值是多少
                //0.05 活体误拒率：万分之一；拒绝率：63.9%
                //0.3 活体误拒率：千分之一；拒绝率：90.3%
                //0.9 活体误拒率：百分之一；拒绝率：97.6%
                //1误拒率: 把真人识别为假人的概率. 阈值越高，安全性越高, 要求也就越高, 对应的误识率就越高
                //2、通过率=1-误拒率
                //所以你thresholds参数返回 和 face_liveness 比较大于推荐值就是活体

                ////活体判断
                var faces = new JArray
                        {
                            new JObject
                            {
                                {"image", imgData64},
                                {"image_type", "BASE64"}
                            }
                        };
                var Living = client.Faceverify(faces);  //活体检测交互返回
                var LivingJson = Newtonsoft.Json.JsonConvert.SerializeObject(Living);
                var LivingObj = Newtonsoft.Json.JsonConvert.DeserializeObject(LivingJson) as JObject;
                if (LivingObj["error_code"].ToString() == "0" && LivingObj["error_msg"].ToString() == "SUCCESS")
                {
                    var Living_result = Newtonsoft.Json.JsonConvert.DeserializeObject(LivingObj["result"].ToString()) as JObject;
                    var Living_list = Living_result["thresholds"];
                    double face_liveness = Convert.ToDouble(Living_result["face_liveness"]);
                    var frr = Newtonsoft.Json.JsonConvert.SerializeObject(Living_list.ToString());
                    var frr_1eObj = Newtonsoft.Json.JsonConvert.DeserializeObject(Living_list.ToString()) as JObject;
                    double frr_1e4 = Convert.ToDouble(frr_1eObj["frr_1e-4"]);
                    if (face_liveness < frr_1e4)
                    {
                        result.info = "识别失败：这是相片之类的非活体！";
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }

                //首先查询是否存在人脸
                var result2 = client.Search(imgData64, imageType, groupId);
                var strJson = Newtonsoft.Json.JsonConvert.SerializeObject(result2);
                var o2 = Newtonsoft.Json.JsonConvert.DeserializeObject(strJson) as JObject;


                //判断是否存在当前人脸，相识度是否大于80
                if (o2["error_code"].ToString() == "0" && o2["error_msg"].ToString() == "SUCCESS")
                {
                    var result_list = Newtonsoft.Json.JsonConvert.DeserializeObject(o2["result"].ToString()) as JObject;
                    var user_list = result_list["user_list"];
                    var Obj = JArray.Parse(user_list.ToString());
                    foreach (var item in Obj)
                    {
                        //80分以上可以判断为同一人，此分值对应万分之一误识率
                        var score = Convert.ToInt32(item["score"]);
                        if (score > 80)
                        {
                            result.info = result2.ToString();
                            result.res = true;
                            result.startcode = 221;
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    result.info = strJson.ToString();
                    result.res = false;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                result.info = e.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult face_userInfoSace()
        {
            ResultInfo result = new ResultInfo();

            try
            {
                //这里就不进行非空判断了，后期根据实际情况进行优化
                var LId =Convert.ToInt32(Request["LId"]);
                var UserFace = Request["UserFace"];
                

                TB_UserFace model = new TB_UserFace();
                model.LId = LId;
                model.UserFace = UserFace;

                var find_ID = db.TB_UserFace.Where(t => t.UserFace == UserFace);
                //根据人脸唯一标识判断是否存在数据
                
                if (find_ID.Count() > 0)
                {
                    result.res = true;
                    result.info = "当前用户已注册过！";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (ModelState.IsValid)
                {
                    db.TB_UserFace.Add(model);
                    db.SaveChanges();
                    result.res = true;
                    result.info = "注册成功";
                }
                
            }
            catch (Exception e)
            {
                result.info = e.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Face_UserInfoList()
        {
            ResultInfo result = new ResultInfo();
            //这里就不进行非空判断了，后期根据实际情况进行优化
            var userFace = Request["UserFace"];
            //根据人脸唯一标识判断是否存在数据
            
            var find_ID = db.TB_UserFace.Where(t => t.UserFace == userFace);
            var strJson = Newtonsoft.Json.JsonConvert.SerializeObject(find_ID);
            result.info = strJson;
            result.res = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult decodeBase64ToImage(string dataURL, string imgName)
        {
            string filename = "";//声明一个string类型的相对路径

            String base64 = dataURL.Substring(dataURL.IndexOf(",") + 1);      //将‘，’以前的多余字符串删除

            System.Drawing.Bitmap bitmap = null;//定义一个Bitmap对象，接收转换完成的图片
            ResultInfo result = new ResultInfo();
            try//会有异常抛出，try，catch一下
            {
                String inputStr = base64;//把纯净的Base64资源扔给inpuStr,这一步有点多余

                byte[] arr = Convert.FromBase64String(inputStr);//将纯净资源Base64转换成等效的8位无符号整形数组

                System.IO.MemoryStream ms = new System.IO.MemoryStream(arr);//转换成无法调整大小的MemoryStream对象
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(ms);//将MemoryStream对象转换成Bitmap对象
                ms.Close();//关闭当前流，并释放所有与之关联的资源
                bitmap = bmp;
                filename = "/upload/" + imgName + ".png";//所要保存的相对路径及名字
                string tmpRootDir = Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath.ToString()); //获取程序根目录 
                string imagesurl2 = tmpRootDir + filename.Replace(@"/", @"\"); //转换成绝对路径 

                bitmap.Save(imagesurl2, System.Drawing.Imaging.ImageFormat.Png);//保存到服务器路径
                result.info = imagesurl2;  //返回相对路径
                result.res = true;
            }
            catch (Exception)
            {
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
