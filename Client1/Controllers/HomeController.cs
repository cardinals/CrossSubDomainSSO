using System;
using System.Web;
using System.Web.Mvc;

namespace Client1.Controllers
{       
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public void LoginOut()
        {            
            HttpCookie httpCookie = new HttpCookie("TicketName");
            httpCookie.Expires = DateTime.UtcNow.AddYears(-1); 
            httpCookie.Domain = ".Client.cn";
            Response.Cookies.Add(httpCookie);            
            string Passport = System.Configuration.ConfigurationManager.AppSettings["Passport"];           
            string ReturnURL =HttpContext.Request.Url.AbsoluteUri;
            AuthorizationContext filterContext = new AuthorizationContext();
            filterContext.Result = new RedirectResult(string.Format("{0}/Identity/LoginOut?ReturnURL={1}", Passport, ReturnURL));
            return;
        }
    }
}