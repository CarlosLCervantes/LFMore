using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer.Global
{
    public static class Global
    {
        public static string Convert24HourTimeToMeridian(string timeAs24Hour)
        {
            string timeAsMeridian = timeAs24Hour;
            if (!String.IsNullOrEmpty(timeAs24Hour))
            {
                string hour = "";
                string minute = "";
                int indexOfColon = timeAs24Hour.IndexOf(':');
                hour = timeAs24Hour.Substring(0, indexOfColon);
                minute = timeAs24Hour.Substring(indexOfColon + 1);

                int eventHour = Convert.ToInt32(hour);
                string meridian = "AM";
                if (eventHour > 12)
                {
                    meridian = "PM";
                    int meridianHour = eventHour - 12;
                    hour = meridianHour.ToString();
                }

                timeAsMeridian = String.Format("{0}:{1} {2}", hour, minute, meridian);
            }

            return timeAsMeridian;
        }

        public static string ConvertMeridianTimeTo24Hour(string hour, string minute, string meridian)
        {
            if (meridian == "PM")
            {
                int hourAsInt = Convert.ToInt32(hour) + 12;
                hour = hourAsInt.ToString();
            }

            return String.Format("{0}:{1}", hour, minute);
        }
    }
}

public class CommitResponse
{
    public bool success;
    public string errorMsg;
    public List<string> Errors {get;set;}
    public object ReturnData { get; set; }

    public CommitResponse()
    {
        Errors = new List<string>();
        success = true;
    }
}




