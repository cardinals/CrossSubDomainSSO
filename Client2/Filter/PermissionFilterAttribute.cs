using System;
using System.Web;
using System.Web.Mvc;

namespace Client2.Filter
{
    /// <summary>
    /// 判断登陆用的访问权限
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class PermissionFilterAttribute : FilterAttribute, IAuthorizationFilter
    {

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            if (OutputCacheAttribute.IsChildActionCacheActive(filterContext))
            {
                throw new InvalidOperationException("AuthorizeAttribute Cannot Use Within Child Action Cache");
            }
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);
            if (skipAuthorization)
            {
                return;
            }
            if (this.AuthorizeCore(filterContext.HttpContext))
            {
                // ** IMPORTANT **
                // Since we're performing authorization at the action level, the authorization code runs
                // after the output caching module. In the worst case this could allow an authorized user
                // to cause the page to be cached, then an unauthorized user would later be served the
                // cached page. We work around this by telling proxies not to cache the sensitive page,
                // then we hook our custom authorization code into the caching mechanism so that we have
                // the final say on whether a page should be served from the cache.
                //HttpCachePolicyBase cachePolicy = filterContext.HttpContext.Response.Cache;
                //cachePolicy.SetProxyMaxAge(new TimeSpan(0));
                //cachePolicy.AddValidationCallback(this.CacheValidateHandler, null /* data */);
            }
            else
            {
                this.HandleUnauthorizedRequest(filterContext);
            }
        }


        /// <summary>
        /// 具体判断方法
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            
            //验证cookie 用户是否有效
            if (httpContext.Request.Cookies.Count <= 0)
            {
                return false;
            }
            //这里可以做授权验证
            //....
            return true;
        }

        protected void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var response = filterContext.HttpContext.Response;
            string Passport = System.Configuration.ConfigurationManager.AppSettings["Passport"];
            string AppKey = System.Configuration.ConfigurationManager.AppSettings["AppKey"];
            string Auth = System.Configuration.ConfigurationManager.AppSettings["Auth"];
            string ReturnURL = request.Url.AbsoluteUri;
            filterContext.Result = new RedirectResult(string.Format("{0}/Identity/Verify?AppKey={1}&Auth={2}&Tikect={3}&ReturnURL={4}",
             Passport, AppKey, Auth, "", ReturnURL));
            return;
        }
    }
}