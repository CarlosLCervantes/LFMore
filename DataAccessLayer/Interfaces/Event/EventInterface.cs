using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DataAccessLayer
{
    public class EventInterface
    {
        DataAccessLayer.LeetRaidsDBDataContext LeetRaidsDB;

        public EventInterface(string connectionString)
        {
            LeetRaidsDB = new LeetRaidsDBDataContext(connectionString);
        }

        public EventInterface(LeetRaidsDBDataContext existingConnection)
        {
            LeetRaidsDB = existingConnection;
        }

        public Event GetEventInfoByID(int eventID)
        {
            Event evt = (from e in LeetRaidsDB.Events
                         where e.EventID == eventID
                         select e).SingleOrDefault();

            return evt;
        }

        public IEnumerable<EventGroup> GetAllEventGroupsByGameID(int gameID)
        {
            var groupTypes = from grp in LeetRaidsDB.EventGroups
                             where grp.GameID == gameID
                             select grp;

            return groupTypes;
        }

        public IEnumerable<EventGroupSubType> GetAllEventGroupSubTypes(int eventGrpID)
        {
            var eventSubTypes = from subType in LeetRaidsDB.EventGroupSubTypes
                                where subType.EventGroupID == eventGrpID
                                select subType;

            return eventSubTypes;
        }

        public EventGroupSubType GetEventGroupSubTypeByID(int eventGroupSubTypeID)
        {
            EventGroupSubType eventSubType = (from subType in LeetRaidsDB.EventGroupSubTypes
                                              where subType.EventGroupSubTypeID == eventGroupSubTypeID
                                              select subType).SingleOrDefault();

            return eventSubType;
        }

        public IEnumerable<EventType> GetAllEventTypesBySubGrpID(int subGrpID)
        {
            var eventType = from evtType in LeetRaidsDB.EventTypes
                            where evtType.EventGroupSubTypeID == subGrpID
                            select evtType;

            return eventType;
        }

        public IEnumerable<EventType> GetAllEventTypes()
        {
            var eventType = from evtType in LeetRaidsDB.EventTypes
                            select evtType;

            return eventType;
        }

        public EventType GetEventTypeByID(int id)
        {
            var eventType = (from evtType in LeetRaidsDB.EventTypes
                             where evtType.EventTypeID == id
                             select evtType).SingleOrDefault();

            return eventType;
        }

        public IEnumerable<EventType> GetAllEventTypesByGameID(int gameID)
        {
            var eventType = from evtType in LeetRaidsDB.EventTypes
                            where evtType.GameID == gameID
                            select evtType;

            return eventType;
        }

        public IEnumerable<Event> GetAllEventsByMemberID(int memberID)
        {
            var events = from evt in LeetRaidsDB.Events
                         join attn in LeetRaidsDB.EventAttendees on evt.EventID equals attn.EventID
                         where attn.MemberID == memberID
                         select evt;

            return events;

        }

        public IEnumerable<Event> GetAllEventsForAMonthByMemberID(int memberID, int month, int year)
        {
            var events = GetAllEventsByMemberID(memberID).Where(evt => evt.Date.Month == month && evt.Date.Year == year);

            return events;
        }

        public IEnumerable<Event> GetAllEventsForDayByMemberID(int memberID, DateTime date )
        {
            var events = GetAllEventsByMemberID(memberID).Where(evt => evt.Date.Day == date.Day && evt.Date.Month == date.Month && evt.Date.Year == date.Year);

            return events;
        }

        public bool AddNewEvent(Event evt)
        {
            bool success = false;
            evt.EventName = HttpUtility.HtmlEncode(evt.EventName);
            evt.MeetupLocation = HttpUtility.HtmlEncode(evt.MeetupLocation);
            evt.Notes = HttpUtility.HtmlEncode((evt.Notes.Length > 50) ? evt.Notes.SubStrMax(49) : evt.Notes);

            try
            {
                LeetRaidsDB.Events.InsertOnSubmit(evt);
                LeetRaidsDB.SubmitChanges();
                success = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return success;
        }

        public CommitResponse AddBasicRestrictions(int eventID, int? serverID, int? factionID)
        {
            CommitResponse response = new CommitResponse();
            EventRestriction evtRestriction = GetEventRestrictionOrCreate(eventID);
            evtRestriction.ServerID = serverID;
            evtRestriction.FactionID = factionID;

            //LeetRaidsDB.EventRestrictions.InsertOnSubmit(evtRestriction);
            LeetRaidsDB.SubmitChanges();

            response.success = true;
            return response;
        }

        public bool EditEvent(Event evtUpdateInfo)
        {
            bool valid = true;
            //Check if the game type for this event is valid for the game being added
            valid = (GetEventTypeByID(evtUpdateInfo.EventTypeID).GameID == evtUpdateInfo.GameID);
 
            bool success = false;
            if (valid)
            {
                Event editEvent = (from evt in LeetRaidsDB.Events
                                   where evt.EventID == evtUpdateInfo.EventID
                                   select evt).SingleOrDefault();

                editEvent.Date = (evtUpdateInfo.Date != null) ? evtUpdateInfo.Date : editEvent.Date;
                editEvent.EventName = (!String.IsNullOrEmpty(evtUpdateInfo.EventName)) ? HttpUtility.HtmlEncode(evtUpdateInfo.EventName) : editEvent.EventName;
                //editEvent.EventRestrictionsID = (evtUpdateInfo.EventRestrictionsID != null) ? evtUpdateInfo.EventRestrictionsID : editEvent.EventRestrictionsID;
                editEvent.EventTime = (evtUpdateInfo.EventTime.Ticks > 0) ? evtUpdateInfo.EventTime : editEvent.EventTime;
                editEvent.EventTypeID = (evtUpdateInfo.EventTypeID != 0) ? evtUpdateInfo.EventTypeID : editEvent.EventTypeID;
                editEvent.MeetupLocation = (!String.IsNullOrEmpty(evtUpdateInfo.MeetupLocation)) ? HttpUtility.HtmlEncode(evtUpdateInfo.MeetupLocation) : editEvent.MeetupLocation;
                editEvent.Notes = (!String.IsNullOrEmpty(evtUpdateInfo.Notes)) ? HttpUtility.HtmlEncode(evtUpdateInfo.Notes.SubStrMax(99)) : editEvent.Notes;

                try
                {
                    LeetRaidsDB.SubmitChanges();
                    success = true;
                }
                catch
                {

                }
            }

            return success;
        }

        public CommitResponse DeactivateEvent(int memberID, int eventID)
        {
            CommitResponse response = new CommitResponse();
            // Ensure that user deleting event is the creator
            if (GetEventCreatorsMemberInfo(eventID).MemberID == memberID)
            {
                response.success = false;
                response.errorMsg = Errors.NOT_EVENT_CREATOR;
                return response;
            }

            Event eventToDeactivate = (from evt in LeetRaidsDB.Events
                                       where evt.EventID == eventID
                                       select evt).SingleOrDefault();

            if (eventToDeactivate != null)
            {
                eventToDeactivate.Active = false;
                LeetRaidsDB.SubmitChanges();
            }
            else
            {
                response.success = false;
                response.errorMsg = Errors.INVALID_EVENT_ID;
            }

            return response;
        }

        public CommitResponse LeaveEvent(int eventID, int characterID)
        {
            CommitResponse response = new CommitResponse();
            EventAttendee eventAttn = (from attn in LeetRaidsDB.EventAttendees
                                       where attn.EventID == eventID && attn.CharacterID == characterID
                                       select attn).SingleOrDefault();

            if (eventAttn != null)
            {
                LeetRaidsDB.EventAttendees.DeleteOnSubmit(eventAttn);
                LeetRaidsDB.SubmitChanges();
            }
            else
            {
                response.success = false;
                response.errorMsg = Errors.CHARACTER_NOT_FOUND;
            }

            return response;
        }

        public CommitResponse AddNewEventAttendee(int charID, int eventID, int roleID, string note, int status, int? memberID)
        {
            int memID = 0;
            if (memberID != null)
                memID = (int)memberID;
            else
            {
                Member Member = new AccountInterface(LeetRaidsDB).GetMemberByCharacterID(charID);
                memID = Member.MemberID;
            }
            
            
            EventAttendee attendee = new EventAttendee()
            {
                CharacterID = charID,
                EventID = eventID,
                RoleID = roleID,
                Note = String.Empty,
                Status = status,
                MemberID = memID
            };

            bool success = false;

            try
            {

                if (IsMemberAlreadyAttendee(eventID, memID))
                {
                    return new CommitResponse() { success = false, errorMsg = Errors.MEMBER_ALREADY_HAS_CHARACTER_IN_EVENT };
                }
                LeetRaidsDB.EventAttendees.InsertOnSubmit(attendee);
                LeetRaidsDB.SubmitChanges();
                success = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return new CommitResponse() { success = success };
        }

        public CommitResponse AddNewEventAttendee(int charID, int eventID, int status) //InviteFriendFromFriendsList
        {
            throw new NotImplementedException();
            return new CommitResponse() { success = false };
        }

        public List<string> AddNewEventAttendeesBulk(int[] charIDs, int eventID, int statusID)
        {
            List<string> charsNotAdded = new List<string>();
            Event eventInfo = GetEventInfoByID(eventID);
            List<EventAttendee> validCharactersAsAttendees = new List<EventAttendee>();

            CharacterInterface charInterface = new CharacterInterface(LeetRaidsDB);
            foreach (int charID in charIDs)
            {
                MemCharacter memChar = charInterface.GetCharacterByID(charID);
                //Does character belong to the right Game/Server AND isnt a current attendee
                if (memChar.GameID == eventInfo.GameID && memChar.ServerID == eventInfo.ServerID && !IsMemberAlreadyAttendee(eventID, memChar.MemberID))
                {
                    validCharactersAsAttendees.Add(
                    new EventAttendee()
                    {
                        CharacterID = memChar.CharacterID,
                        EventID = eventInfo.EventID,
                        MemberID = memChar.MemberID,
                        Note = null,
                        RoleID = null,
                        Status = statusID
                    });
                }
                else
                {
                    charsNotAdded.Add(memChar.CharacterName);
                }
            }

            if (validCharactersAsAttendees.Count > 0)
            {
                LeetRaidsDB.EventAttendees.InsertAllOnSubmit(validCharactersAsAttendees);
                LeetRaidsDB.SubmitChanges();
            }

            return charsNotAdded;
        }

        public bool IsCharacterAnAttendee(int eventID, int charID)
        {
            //Check to see if the member already has a character tied to this event
            bool characterExists = (from attn in LeetRaidsDB.EventAttendees
                                    where attn.CharacterID == charID && attn.EventID == eventID
                                    select attn).Count() > 0;

            return characterExists;
        }

        //ALWAYS check that a Member only has 1 character in an event.
        public bool IsMemberAlreadyAttendee(int eventID, int memberID)
        {
            //Check to see if the member already has a character tied to this event
            bool memberCharacterExists = (from attn in LeetRaidsDB.EventAttendees
                                          where attn.MemberID == memberID && attn.EventID == eventID
                                          select attn).Count() > 0;

            return memberCharacterExists;
        }

        public IEnumerable<EventAttendee> GetAllEventAttendees(int eventID)
        {
            var attendees = from attn in LeetRaidsDB.EventAttendees
                            where attn.EventID == eventID
                            select attn;

            return attendees;
        }

        public EventAttendee GetEventAttendee(int eventID, int memberID)
        {
            EventAttendee attendee = (from attn in LeetRaidsDB.EventAttendees
                                      where attn.EventID == eventID && attn.MemberID == memberID
                                      select attn).SingleOrDefault(); ;

            return attendee;
        }

        public MemCharacter GetEventCreatorsCharacter(int memberID, int eventID)
        {
            MemCharacter memChar = null;
            EventAttendee attn = GetAllEventAttendees(eventID).Where(x => x.MemberID == memberID).SingleOrDefault();
            if (attn != null)
            {
                memChar = new CharacterInterface(LeetRaidsDB).GetCharacterByID(attn.CharacterID);
            }

            return memChar;
        }

        public Member GetEventCreatorsMemberInfo(int eventID)
        {
            Member member = (from mem in LeetRaidsDB.Members
                             join evtInfo in LeetRaidsDB.Events on mem.MemberID equals evtInfo.EventCreaterMemberID
                             where evtInfo.EventID == eventID
                             select mem).SingleOrDefault();

            return member;

        }

        public List<EventOverview> SearchEvents(SearchEventsParams sp, int curMemberID)
        {
            List<string> validationErrors = sp.Validate();

            //SP is fucking up beacuse string value are defaultign to empty and not null
            sp.EventCreatorsCharName = (sp.EventCreatorsCharName == String.Empty) ? null : sp.EventCreatorsCharName;
            sp.EventName = (sp.EventName == String.Empty) ? null : sp.EventName;

            List<EventOverview> eventsReturn = new List<EventOverview>();
            if (validationErrors.Count == 0)
            {
                var events = from sr in LeetRaidsDB.SearchEvents(sp.GameID, sp.SearchTerm, sp.ShowFullEvents, sp.EventName, sp.EventCreatorsCharName, sp.EventTypeName, sp.ServerID, 
                                 sp.EventStartDate, sp.EventEndDate, sp.EventStartTime, sp.EventEndTime, sp.EventGroupID, sp.EventSubGroupID, sp.EventTypeID)
                             where !Event.IsDateExpired(sr.Date, sr.EventTime)
                             select new EventOverview()
                             {
                                 EventInfo = new Event(){
                                     EventID = sr.EventID, 
                                     EventName =  sr.EventName,
                                     Date = sr.Date,
                                     EventTypeID = sr.EventTypeID,
                                     //EventRestrictionsID = sr.EventRestrictionsID,
                                     EventCreaterMemberID = sr.EventCreaterMemberID,
                                     EventTime = sr.EventTime,
                                     MeetupLocation = sr.MeetupLocation,
                                     Notes = sr.Notes,
                                     GameID = sr.GameID,
                                     Active = sr.Active
                                 },
                                 EventTypeName = sr.EventTypeName,
                                 CurrentAttendees = String.Format("{0}/{1}", GetAllEventAttendees(sr.EventID).Count(), sr.PlayerCount),
                                 UserIsCreator = (GetEventCreatorsMemberInfo(sr.EventID).MemberID == curMemberID)
                             };

                eventsReturn = events.ToList();
            }

            return eventsReturn;
        }

        public CommitResponse UpdateAttendeeStatus(int eventID, int memberID, int newStatusID, int newCharID, string newNote, int newRoleID)
        {
            CommitResponse response = new CommitResponse();
            EventAttendee attendee = (from attn in LeetRaidsDB.EventAttendees
                                      where attn.EventID == eventID && attn.MemberID == memberID
                                      select attn).SingleOrDefault();

            

            if (attendee != null)
            {
                //I am not supporting nullable value. If you dont want to update the info then pass in the current values.
                attendee.Status = newStatusID;
                attendee.CharacterID = newCharID;
                attendee.Note = HttpUtility.HtmlEncode((newNote.Length > 50) ? newNote.SubStrMax(49) : newNote);
                attendee.RoleID = newRoleID;
                LeetRaidsDB.SubmitChanges();
            }
            else
            {
                response.success = false;
                response.errorMsg = Errors.ATTENDEE_COULDNT_BE_FOUND;
            }

            return response;
        }

        public CommitResponse UpdateAttendeeAttendenceStatusBulk(int eventID, IEnumerable<int> charIDs, int statusID)
        {
            CommitResponse response = new CommitResponse();
            var attendees = (from attn in LeetRaidsDB.EventAttendees
                             where attn.EventID == eventID && charIDs.Contains(attn.CharacterID)
                             select attn);

            if (attendees.Count() > 0)
            {
                foreach (EventAttendee attn in attendees)
                {
                    attn.Status = statusID;
                }

                LeetRaidsDB.SubmitChanges();
            }
            else
            {
                response.success = false;
                response.errorMsg = Errors.ATTENDEE_COULDNT_BE_FOUND;
            }

            return response;
        }

        public CommitResponse RemoveAttendeeBulk(int eventID, IEnumerable<int> charIDs)
        {
            CommitResponse response = new CommitResponse();
            var attendees = (from attn in LeetRaidsDB.EventAttendees
                             where attn.EventID == eventID && charIDs.Contains(attn.CharacterID)
                             select attn);

            if (attendees.Count() > 0)
            {
                LeetRaidsDB.EventAttendees.DeleteAllOnSubmit(attendees);
                LeetRaidsDB.SubmitChanges();
            }
            else
            {
                response.success = false;
                response.errorMsg = Errors.ATTENDEE_COULDNT_BE_FOUND;
            }

            return response;
        }

        public bool IsEventFull(int eventID)
        {
            int maxEventOccupancy = (from evt in LeetRaidsDB.Events
                                     join evtType in LeetRaidsDB.EventTypes on evt.EventTypeID equals evtType.EventTypeID
                                     where evt.EventID == eventID
                                     select evtType.PlayerCount).SingleOrDefault();

            int numAttendees = (from attn in LeetRaidsDB.EventAttendees
                                where attn.EventID == eventID &&
                                attn.Status == (int)ATTENDEE_STATUS.ACCEPTED
                                select attn).Count();

            return (numAttendees >= maxEventOccupancy);
        }

        #region =====================Restrictions=====================

        public CommitResponse InsertRoleRestrictions(int eventID, IEnumerable<RoleRestriction> restrictions)
        {
            CommitResponse response = new CommitResponse();
            //Create Identity for Role Restrictions
            int? maxRoleRestricitons = LeetRaidsDB.RoleRestrictions.Select(r => (int?)r.RestrictionID).Max();
            maxRoleRestricitons = (maxRoleRestricitons != null) ? maxRoleRestricitons : 0;
            int restrictionID = ((int)maxRoleRestricitons) + 1;

            //Update roleRestrictions with the restrictionID Identity
            foreach (RoleRestriction rest in restrictions)
            {
                rest.RestrictionID = restrictionID;
            }
            //Setup Insert for Role Restrictions
            LeetRaidsDB.RoleRestrictions.InsertAllOnSubmit(restrictions);

            //Get The EventRestriction, or create it if it doesnt exist
            EventRestriction evtRestriction = GetEventRestrictionOrCreate(eventID);

            //Update Event Restriction
            evtRestriction.RoleRestrictionID = restrictionID;

            try
            {
                LeetRaidsDB.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        public CommitResponse UpdateRestrictions(int eventID, IEnumerable<RoleRestriction> updtRoleRest, int? serverID, int? factionID)
        {
            CommitResponse response = new CommitResponse();
            EventRestriction evtRestriction = GetEventRestrictionOrCreate(eventID);

            //Go ahead and update the basic restrictions
            evtRestriction.ServerID = serverID;
            evtRestriction.FactionID = factionID;
            LeetRaidsDB.SubmitChanges();


            //Update Role Restrictions
            response.success = UpdateRoleRestrictions(eventID, updtRoleRest).success;

            return response;
        }

        public CommitResponse UpdateRoleRestrictions(int eventID, IEnumerable<RoleRestriction> updtRoleRest)
        {
            CommitResponse response = new CommitResponse();

            //Get Existing Role Restrictions
            var roleRestrictions = from roleRest in LeetRaidsDB.RoleRestrictions
                                   join evtRest in LeetRaidsDB.EventRestrictions on roleRest.RestrictionID equals evtRest.RoleRestrictionID
                                   where evtRest.EventID == eventID
                                   select roleRest;

            foreach (RoleRestriction roleRest in updtRoleRest)
            {
                RoleRestriction existingRestriction = roleRestrictions.Where(r => r.RoleID == roleRest.RoleID).SingleOrDefault();
                if (existingRestriction != null)
                {
                    //If some exist then update
                    existingRestriction.Quantity = roleRest.Quantity;
                }
                else
                {
                    //Else Make new ones
                    LeetRaidsDB.RoleRestrictions.InsertOnSubmit(roleRest);
                }
            }

            LeetRaidsDB.SubmitChanges();

            return response;
        }

        public IEnumerable<RoleRestriction> GetRoleRestrictionsByEvent(int eventID)
        {
            var roleRestrictions = from eventRestriction in LeetRaidsDB.EventRestrictions
                                   join roleRestriction in LeetRaidsDB.RoleRestrictions on eventRestriction.RoleRestrictionID equals roleRestriction.RestrictionID
                                   where eventRestriction.EventID == eventID
                                   select roleRestriction;


            return roleRestrictions;
        }

        public RoleRestriction GetRoleRestictionByEvent(int eventID, int roleID)
        {
            RoleRestriction roleRestrict = (from role in GetRoleRestrictionsByEvent(eventID)
                                            where role.RoleID == roleID
                                            select role).SingleOrDefault();

            return roleRestrict;
        }

        public EventRestriction GetEventRestriction(int eventID)
        {
            
            EventRestriction evtRestriction = (from eRestriction in LeetRaidsDB.EventRestrictions
                                               where eRestriction.EventID == eventID
                                               select eRestriction).SingleOrDefault();

            return evtRestriction;
            
        }

        public EventRestriction GetEventRestrictionOrCreate(int eventID)
        {
            EventRestriction evtRestriction = (from evtRest in LeetRaidsDB.EventRestrictions
                                               where evtRest.EventID == eventID
                                               select evtRest).SingleOrDefault();

            if (evtRestriction == null)
            {
                evtRestriction = new EventRestriction()
                {
                    EventID = eventID
                };
                LeetRaidsDB.EventRestrictions.InsertOnSubmit(evtRestriction);
                LeetRaidsDB.SubmitChanges();
            }

            return evtRestriction;
        }

        public IEnumerable<Role> GetRestrictedRoles(int eventID)
        {
            CharacterInterface charinterface = new CharacterInterface(LeetRaidsDB);
            IEnumerable<Role> restrictedRoles = from roleRestr in GetRoleRestrictionsByEvent(eventID)
                                                select charinterface.GetRoleByID(roleRestr.RoleID);

            return restrictedRoles;
        }

        public IEnumerable<Role> GetRestrictedRolesThatAreFull(int eventID)
        {
            CharacterInterface charinterface = new CharacterInterface(LeetRaidsDB);

            Event evtInfo = GetEventInfoByID(eventID);

            List<RoleCount> roles = (from r in LeetRaidsDB.Roles
                                  where r.GameID == evtInfo.GameID
                                     select new RoleCount() { RoleID = r.RoleID, Count = 0 }).ToList(); ;

            foreach (RoleCount r in roles)
            {
                r.Count = GetAllEventAttendees(eventID).Where(attn => attn.RoleID == r.RoleID).ToList().Count;
            }

            IEnumerable<Role> restrictedRoles = from roleRestr in GetRoleRestrictionsByEvent(eventID)
                                                where roleRestr.Quantity > 0 && roleRestr.Quantity <= roles.Where(r => r.RoleID == roleRestr.RoleID).SingleOrDefault().Count 
                                                select charinterface.GetRoleByID(roleRestr.RoleID);
            return restrictedRoles;
        }

        public bool IsRoleRestricted(int eventID, int roleID)
        {
            bool full = false;
            var fullRoles = GetRestrictedRolesThatAreFull(eventID);
            if (fullRoles.Any(r => r.RoleID == roleID)) { full = true; }

            return full;
        }

        public int? GetClassResctrictionCountForEventbyClassID(int eventID, int classID)
        {
            return null; //currently class restircitons are not supported
        }

        public bool IsCharacterInDemandForEvent(int eventID, int charID)
        {
            CharacterInterface charInterface = new CharacterInterface(LeetRaidsDB);
            MemCharacter memChar = charInterface.GetCharacterByID(charID);
            Event evt = GetEventInfoByID(eventID);
            //First of all is the character on the same server
            if (memChar.ServerID != evt.ServerID)
            {
                return false;
            }

            //OK, so does he belong to a role that isn't restricted
            Role[] roles = charInterface.GetRolesByCharacterID(charID).ToArray();
            int restrictedRoleCount = 0;
            foreach(Role r in roles)
            {
                if(IsRoleRestricted(eventID, r.RoleID)) {restrictedRoleCount++;}
            }
            if(restrictedRoleCount >= roles.Length) {return false;}

            //Is her part of a class that isnt restricted?
            //TODO: Add Class Restriction

            return true;
            
        }

        #endregion

        public string DecodeAttendeeStatus(int status)
        {
            string decode = "Unknown";
            switch (status)
            {
                case (int)ATTENDEE_STATUS.INVITED: decode = "Invited";
                    break;
                case (int)ATTENDEE_STATUS.TENTATIVE: decode = "Tentative";
                    break;
                case (int)ATTENDEE_STATUS.ACCEPTED: decode = "Accepted";
                    break;
                case (int)ATTENDEE_STATUS.DECLINED: decode = "Declined";
                    break;
                case (int)ATTENDEE_STATUS.STANDBY: decode = "Standby";
                    break;
            }

            return decode;
        }
    }

    public class RoleCount
    {
        public int RoleID;
        public int Count;
    }

    public class EventOverview
    {
        public Event EventInfo { get; set; }
        public string EventTypeName { get; set; }
        public string EventImageFile { get; set; }
        public string CurrentAttendees { get; set; }
        public bool UserIsCreator { get; set; }
    }

    public class SearchEventsParams
    {
        public int GameID { get; set; }
        public string SearchTerm { get; set; }
        public bool ShowFullEvents { get; set; }
        public string EventName {get; set;}
        public string EventCreatorsCharName { get; set; }
        public string EventTypeName { get; set; }
        public int? ServerID { get; set; }
        public DateTime? EventStartDate { get; set; }
        public DateTime? EventEndDate { get; set; }
        public TimeSpan? EventStartTime { get; set; }
        public TimeSpan? EventEndTime { get; set; }

        public int? EventGroupID { get; set; }
        public int? EventSubGroupID { get; set; }
        public int? EventTypeID { get; set; }

        public SearchEventsParams()
        {
            this.ShowFullEvents = false;
        }

        public List<string> Validate()
        {
            List<string> errors = new List<string>();
            if(String.IsNullOrEmpty(this.SearchTerm)) { errors.Add(Errors.SEARCH_TERM_CANNOT_BE_EMPTY); }

            //if (EventStartDate != null)
            //{
            //    if(DateTime.TryParse()
            //}

            return errors;
        }
    }

    public class EventGroupsWithSubGroups
    {
        public EventGroup EventGroup { get; set; }
        public EventGroupSubType[] SubGroups { get; set; }

        public EventGroupsWithSubGroups()
        {
        }

        public EventGroupsWithSubGroups(EventGroup grp, EventGroupSubType[] subGrps)
        {
            EventGroup = grp;
            SubGroups = subGrps;
        }


    }

    public enum ATTENDEE_STATUS { INVITED = 1, ACCEPTED = 2, TENTATIVE = 3, DECLINED = 4, STANDBY = 5 }

    public enum EVENT_RESTRICTION_TYPES { ROLE = 1, CLASS = 2 }

}
