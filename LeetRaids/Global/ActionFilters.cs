using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeetRaids
{
    public class NoCache : ActionFilterAttribute, IActionFilter
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            HttpContext.Current.Response.Cache.SetValidUntilExpires(false);
            HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();

            base.OnResultExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
        }
    }

    public class RequiresLogin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Authorize(filterContext);
            base.OnActionExecuting(filterContext);
        }

        private void Authorize(ActionExecutingContext filterContext)
        {
            // If there is no logged in user
            if (GlobalMethod.MemberInfo() == null)
            {
                // Store the current requested action
                HttpContext.Current.Session[GlobalConst.SESSION_PREVIOUS_ACTION] = filterContext.RequestContext.HttpContext.Request.Url.AbsolutePath; 

                HttpContext.Current.Response.Redirect("/Account/Login"); // Then Redirect the user to login

            }
        }
    }

    public class RequiresCurrentEventContext : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Authorize(filterContext);
            base.OnActionExecuting(filterContext);
        }

        private void Authorize(ActionExecutingContext filterContext)
        {
            // If there is no Event Context kick the user back to the page they were jst one
            if (GlobalMethod.CurrentEvent() == null)
            {
                GlobalMethod.RedirectBack();
            }
        }
    }

}
