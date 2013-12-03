using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer
{
    public class EventAJAX
    {
        public string EventName { get; set; }
        public string Date { get; set; }
        public string MeetupLocation { get; set; }
        public string Notes { get; set; }

        public string EventHour { get; set; }
        public string EventMinute { get; set; }
        public string EventMeridian { get; set; }

        public EventAJAX(Event evt)
        {
            this.EventName = evt.EventName;
            this.Date = evt.Date.ToString("MM/dd/yyyy");
            this.MeetupLocation = evt.MeetupLocation;
            this.Notes = evt.Notes;

            string eventTimeAsString = evt.EventTime.ToString();
            if(!String.IsNullOrEmpty(eventTimeAsString))
            {
                int indexOfColon = eventTimeAsString.IndexOf(':');
                this.EventHour = eventTimeAsString.Substring(0, indexOfColon);
                this.EventMinute = eventTimeAsString.Substring(indexOfColon + 1);

                //Now convert to Meridian time
                int eventHour = Convert.ToInt32(this.EventHour);
                string meridian = "AM";
                if (eventHour > 12)
                {
                    meridian = "PM";
                    int meridianHour = eventHour - 12;
                    this.EventHour = meridianHour.ToString();
                }

                this.EventMeridian = meridian;
            }
        }

        
    }

    public class MemCharacterAJAX
    {
        public int CharID { get; set; }
        public string CharacterName { get; set; }
        public int? LVL { get; set; }
        public int ClassID { get; set; }

        public MemCharacterAJAX(MemCharacter memChar)
        {
            this.CharID = memChar.CharacterID;
            this.CharacterName = memChar.CharacterName;
            this.LVL = memChar.LVL;
            this.ClassID = memChar.ClassID;
        }
    }
}