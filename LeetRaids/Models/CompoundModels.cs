using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccessLayer;

namespace LeetRaids.Models
{
    /// <summary>
    /// This is a compound model. Add a character depends on multiple Models.
    /// </summary>
    public class AddGameModel
    {
        public Member member;
        public List<MemCharacter> memberCharacters;
        public Game gameToAdd;
        public List<Game> gamesSupported;
        /// <summary>
        /// This is the URL of the action the user was on before they registered. This is important so user can get back to the page they had initially
        /// tried to view, which was maybe an invitatio link.
        /// </summary>
        public string PrevActionURL;
    }

    public class AddEventModel
    {
        //public List<CompleteCharacterData> UserCharacters { get; set; }
        public IEnumerable<Game> Games { get; set; }
        public List<EventRoleInfo> RolesInfo { get; set; }
    }

    public class ManageEventModel
    {
        public Event EventInfo { get; set; }
        public EventType EventTypeInfo { get; set; }
        public IEnumerable<EventAttendee> Attendees { get; set; }
        public List<EventRoleInfo> RolesInfo { get; set; }
        public List<EventClassInfo> ClassInfo { get; set; }
        public List<MemCharacter> AttnCharacters { get; set; }
        public string ServerName { get; set; }
    }

    public class EventClassInfo
    {
        public Class Class { get; set; }
        public int ClassCount { get; set; }
        public int? ClassRestrictionCount { get; set; }
        public string ClassRestrictionDisplay
        {
            get
            {
                string classRestrictionCountDisplay = (ClassRestrictionCount == null) ? "N/A" : ClassRestrictionCount.ToString();
                return String.Format("{0} / {1}", ClassCount, classRestrictionCountDisplay);

            }
        }
    }

    public class EventRoleInfo
    {
        public Role Role { get; set; }
        public int RoleCount { get; set; }
        public RoleRestriction RoleRestriction { get; set; }
        public string RoleRestictionDisplay
        {
            get
            {
                string roleRestrictionCountDisplay = (RoleRestriction == null || RoleRestriction.Quantity < 1) ? "N/A" : RoleRestriction.Quantity.ToString();
                return String.Format("{0} / {1}", RoleCount, roleRestrictionCountDisplay);
            }
        }
    }

    public class ViewEventModel
    {
        public bool IsUserAttendee { get; set; }
        public Event EventInfo { get; set; }
        public EventType EventTypeInfo { get; set; }
        public IEnumerable<EventAttendee> Attendees { get; set; }
        public List<EventRoleInfo> RolesInfo { get; set; }
        public List<EventClassInfo> ClassInfo { get; set; }
        public List<MemCharacter> AttnCharacters { get; set; }
        public string ServerName { get; set; }
    }

    public class SearchCharactersModel
    {
        public bool EnforceEventRestrictions { get; set; }
        public List<Role> Roles { get; set; }
        public List<Class> Classes { get; set; }
        public List<Server> Servers { get; set; }
        public List<Faction> Factions { get; set; }

    }

    public class AddCharacterModel
    {
        public int GameID { get; set; }
        public MemCharacter Character { get; set; }
        public List<Class> Classes { get; set; }
        public List<Faction> Factions { get; set; }
        public List<Server> Servers { get; set; }
        public List<Role> Roles { get; set; }
    }

    public class EditCharacterModel
    {
        public MemCharacter CharInfo { get; set; }
        public List<Class> Classes { get; set; }
        public List<Faction> Factions { get; set; }
        public List<Server> Servers { get; set; }
        public List<Role> Roles { get; set; }
    }

    public class EditEventModel
    {
        public int CharID { get; set; }
        public int GameID { get; set; }
        public Event EventInfo { get; set; }
        public EventType EventTypeInfo { get; set; }
        public EventGroupSubType EventSubTypeInfo { get; set; }
        public IEnumerable<EventRoleInfo> RolesInfo { get; set; }
        public IEnumerable<Game> Games { get; set; }
    }

    public class EventSearchModel
    {
        public List<Game> Games { get; set; }

        public SearchEventsParams EventSearchParams { get; set; }
        public List<EventOverview> Events { get; set; }

        public EventSearchModel()
        {
            this.Events = new List<EventOverview>();
            this.EventSearchParams = new SearchEventsParams();
        }
    }


    public class JoinEventModel
    {
        public bool IsEventFull { get; set; }
        public string Note { get; set; }
        public int AttendeeStatus { get; set; }
        public List<CompleteCharacterData> UserCharacters { get; set; }
        public List<Role> RestrictedRoles { get; set; }
        public string CantJoinEventReason { get; set; }
    }

    public class UpdateStatusModel
    {
        public string Note { get; set; }
        public int AttendeeStatus { get; set; }
        public int SelectedRole { get; set; }
        public int SelectedCharacter { get; set; }
        public List<CompleteCharacterData> UserCharacters { get; set; }
        public List<Role> RestrictedRoles { get; set; }
    }

    public class InviteModel
    {
        public List<MemFriend> FriendsList { get; set; }
        public Event EventInfo { get; set; }
        public EventRestriction EventRestritions {get;set;}
        public string Event_GameName { get; set; }
        public string Event_ServerName { get; set; }
    }

    //public class EventsOverviewModel
    //{
    //    public Event EventInfo { get; set; }
    //    public string EventTypeName { get; set; }
    //    public string CurrentAttendees { get; set; }
    //    public bool UserIsCreator { get; set; }
    //}


}
