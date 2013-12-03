using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Configuration;
using System.Text;

namespace LeetRaids.Controllers
{
    public class AboutController : BaseController
    {
        public ActionResult Index()
        {
            //throw new Exception("Stupid Exception");
            return View();
        }

        public ViewResult TermsOfUse()
        {
            return View();
        }

        public ViewResult PrivacyPolicy()
        {
            return View();
        }

        public ViewResult FeedBack()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult FeedBack(object model)
        {
            string subject = Request["Subject"];
            string message = Request["Message"];

            if (String.IsNullOrEmpty(subject)) { ValidationErrors.Add("NoSubject", "Please enter a subject"); }
            if (String.IsNullOrEmpty(message)) { ValidationErrors.Add("NoMessage", "Please enter a message"); }

            if (ValidationErrors.Count == 0)
            {
                //Send out the email
                string emailAddress = ConfigurationManager.AppSettings["FeedBackEmailAddress"];

                StringBuilder sbBody = new StringBuilder();
                sbBody.Append("FeedBack for LFMore.com \n");
                string memberInfo = (MemberInfo != null) ? MemberInfo.MemberID.ToString() : "Not a Member";
                string lastPageVisited = (Session[GlobalConst.SESSION_CURRENT_ACTION] != null) ? Session[GlobalConst.SESSION_CURRENT_ACTION].ToString() : "None";
                sbBody.Append(String.Format("Subject:{0} \n MemberID: {1} \n Last Page Visited: {2} \n", subject, memberInfo, lastPageVisited));
                sbBody.Append(message);

                bool success = false;
                try
                {
                    SMTPEmail.SMTPEMailS email = new SMTPEmail.SMTPEMailS(null);
                    //string emailFileLocation = Server.MapPath("~/Static/InviteEmail.txt");
                    success = email.SendEmail(new string[] { emailAddress }, "DoNotReply@lfmore.com", "Feedback for LFMore", sbBody.ToString(), null, false);
                }
                catch (Exception ex)
                {
                    //TODO: We really want to know if this fails. Its possible to capture all the failures and then email them out later
                }


                TempData[GlobalConst.TEMPDATA_CONFIRMATION_MESSAGE] = "Your feedback has been sent. Thank you for helping to make LFMore a better place!";
                return RedirectToAction("Confirmation", "Shared");

            }
            else
            {
                RegisterErrorsWithModel();
            }

            return View();
        }

        //public ViewResult ContactUs()
        //{

        //    return View();
        //}

    }
}
