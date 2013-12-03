using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccessLayer;

namespace LeetRaids.Models
{
    public class RaidCalendar
    {
        public int MonthNumber { get; set; }
        public string MonthName { get; set; }
        public int Year { get; set; }
        public int DaysInMonth { get; set; }

        public int calendarMonthStartIndex;
        public int calendarMonthEndIndex;

        public IEnumerable<Event> Events { get; set; }

        public RaidCalendar()
        {

        }

        public RaidCalendar(int monthNum, EventInterface evtInterface)
        {
            BuildCalendar(monthNum, DateTime.Now.Year);
        }

        public RaidCalendar(int monthNum, int year, EventInterface evtInterface)
        {
            BuildCalendar(monthNum, year);
        }

        public void BuildCalendar(int monthNum, int year)
        {
            //TODO: Make year valid
            //int year = 2009;
            //Special Case: If monthNum = 13 Then increment the year
            if (monthNum > 12) { year++; monthNum = 1; }
            //special Case: If monthNum = 0 Then decrement the year
            if (monthNum < 1) { year--; monthNum = 12; }
            Year = year;

            MonthNumber = monthNum;
            MonthName = System.Globalization.DateTimeFormatInfo.CurrentInfo.MonthNames[monthNum - 1];

            int daysInMonth = DateTime.DaysInMonth(year, monthNum);
            DateTime d = new DateTime(year, monthNum, 1);
            int dayOfWeek = (int)d.DayOfWeek;

            DaysInMonth = daysInMonth;
            this.calendarMonthStartIndex = dayOfWeek + 1;
            this.calendarMonthEndIndex = calendarMonthStartIndex + (daysInMonth - 1);
        }

        public void PopulateEventsForThisMonth(EventInterface eventInterface, int memberID)
        {
            Events = eventInterface.GetAllEventsForAMonthByMemberID(memberID, MonthNumber, Year);
        }
    }
}
