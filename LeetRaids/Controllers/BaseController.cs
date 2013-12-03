using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccessLayer;

namespace LeetRaids
{
    public class BaseController : System.Web.Mvc.Controller
    {
        protected Dictionary<string, string> ValidationErrors = new Dictionary<string, string>();

        public BaseController()
        {

        }

        protected override void OnResultExecuted(System.Web.Mvc.ResultExecutedContext filterContext)
        {
            //Action was executed successfully, store this current action so we can redirect later if we have problems.
            if (filterContext.Result.GetType() != typeof(System.Web.Mvc.JsonResult)) //Exclude AJAX Results
            {
                Session[GlobalConst.SESSION_CURRENT_ACTION] = filterContext.RequestContext.HttpContext.Request.Url.AbsolutePath;
            }
            base.OnResultExecuted(filterContext);
        }

        protected override void OnActionExecuted(System.Web.Mvc.ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        public Member MemberInfo
        {
            get
            {
                return GlobalMethod.MemberInfo();
            }
            set
            {
                Session[GlobalConst.SESSION_MEMBER] = value;
            }
        }

        /// <summary>
        /// The event that a user is currently operating on
        /// </summary>
        /// <returns>The Event that a user is currently operating on</returns>
        public Event CurrentEvent
        {
            get 
            {
                return GlobalMethod.CurrentEvent();
            }
            set
            {
                Session[GlobalConst.SESSION_CURRENT_EVENT] = value;
            }
        }

        /// <summary>
        /// The character who is performing event operations
        /// </summary>
        //Created this mainly so that I could know what faction the current event is under. But i feel it will come in handy elsewhere.
        public MemCharacter CurrentCharacter
        {
            get
            {
                MemCharacter curMemChar = Session[GlobalConst.SESSION_CURRENT_MEMBER_CHARACTER] as MemCharacter;
                return curMemChar;
            }
            set
            {
                Session[GlobalConst.SESSION_CURRENT_MEMBER_CHARACTER] = value;
            }
        }

        protected void RegisterErrorsWithModel()
        {
            foreach (string key in ValidationErrors.Keys)
            {
                ModelState.AddModelError(key, ValidationErrors[key]);
            }
        }

        protected void RegisterErrorsWithModel(string keySeed, List<string> errors)
        {
            int count = 0;
            foreach (string error in errors)
            {
                ModelState.AddModelError(keySeed + count, error);
                count++;
            }
        }


    }
}


