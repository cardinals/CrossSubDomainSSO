using Common;
using System;
using System.Web;
using System.Web.Mvc;

namespace SSOServer.Controllers
{
    public class IdentityController : Controller
    {
        // GET: Identity
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Verify()
        {
            //判断发送该请求的客户端是否符合条件
            if (Request["Auth"] != ConfigHelper.GetAppSettingValue("Auth") || Request["AppKey"] != ConfigHelper.GetAppSettingValue("AppKey"))
            {
                //判断用户是否登录
                string ticketName=Request["TikectName"];
                if (!string.IsNullOrEmpty(ticketName)&& !string.IsNullOrEmpty(Request.Cookies[ticketName].Value))
                {
                    string ReturnURL = Request["ReturnURL"] + "?TikectName=" + ticketName;
                    return Redirect(ReturnURL);                   
                }
                else
                {
                    return RedirectToAction("Login", new { ReturnURL = Request["ReturnURL"] });
                }    
            }
            else
            {
                return RedirectToAction("Error", "Identity");
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            ViewBag.ReturnURL = Request["ReturnURL"];
            return View();
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(string userName, string password)
        {
            if ((userName == "qxh" && password == "123")||(userName == "jlp" && password == "123"))
            {                
                HttpCookie cookie =new HttpCookie("TicketName");
                cookie.HttpOnly = true;
                cookie.Value = "qxh";
                cookie.Expires = DateTime.Now.AddHours(2);
                cookie.Domain = ".Client.cn";
                Response.Cookies.Add(cookie);
                string ReturnURL = Request["ReturnURL"] + "?TicketName=" + "TicketName";
                return Redirect(ReturnURL);            
            }
            return View();
        }


        public ActionResult LoginOut()
        {
            HttpCookie httpCookie = new HttpCookie("TicketName");
            httpCookie.Expires = DateTime.UtcNow.AddYears(-1);
            httpCookie.Domain = ".Client.cn";
            Response.Cookies.Add(httpCookie);
            string ReturnURL = Request["ReturnURL"];
            return this.Redirect(ReturnURL);
        }
        /// <summary>
        /// 错误页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            return View();
        }
    }
}