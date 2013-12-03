using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer
{
    public partial class Event
    {
        public bool IsExpired()
        {
            if (DateTime.Now > GetDateTime())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsDateExpired(DateTime date, TimeSpan time)
        {
            DateTime totalDatetime = date.AddHours(time.Hours).AddMinutes(time.Minutes);

            return (DateTime.Now > totalDatetime);
        }

        /// <summary>
        /// Retrives a DateTime which contains both the Date and Time fields compounded.
        /// </summary>
        /// <returns>Total Event DateTime Value</returns>
        public DateTime GetDateTime()
        {
            DateTime dt = new DateTime(this.Date.Year, this.Date.Month, this.Date.Day);
            //int[] partsOfTime = this.EventTime.Split(new char[] { ':' }).Select(metric => Convert.ToInt32(metric)).ToArray();
            dt = dt.AddHours(EventTime.Hours);
            dt = dt.AddMinutes(EventTime.Minutes);

            return dt;
        }

        public string GetDisplayTime()
        {
            DateTime dt = new DateTime(this.EventTime.Ticks);

            string returnTime = dt.ToString("h:mm tt ") + "";

            return returnTime;
        }

        public string GenerateEventURL(System.Web.HttpContext context)
        {
            string eventDirectory = String.Format("/{0}/{1}/{2}", "Events", "ViewEvent", this.EventID);

            string absoluteURL = context.Request.Url.GetLeftPart(UriPartial.Authority) + eventDirectory;

            return absoluteURL;
        }
    }
}
