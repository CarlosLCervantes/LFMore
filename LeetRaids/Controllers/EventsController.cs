using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using LeetRaids.Models;
using DataAccessLayer;


namespace LeetRaids.Controllers
{
    public class EventsController : BaseController
    {
        EventInterface eventInterface;
        GameInterface gameInterface;
        CharacterInterface charInterface;
        AccountInterface accountInterface;
        public EventsController(EventInterface iEvent, GameInterface iGame, CharacterInterface iChar, AccountInterface iAcct)
        {
            eventInterface = iEvent;
            charInterface = iChar;
            gameInterface = iGame;
            accountInterface = iAcct;
        }

        [RequiresLogin()]
        public ActionResult Index()
        {
            int monthNum = DateTime.Now.Month;
            RaidCalendar c = new RaidCalendar(monthNum, eventInterface);
            c.PopulateEventsForThisMonth(eventInterface, MemberInfo.MemberID);

            return View(c);
        }

        public ActionResult EventCalendar()
        {
            return View();
        }

        [RequiresLogin()]
        public ActionResult Manage(int id)
        {
            int eventID = id; //Lulz

            //Get the event creators character ID. If the current user is not the event creator Then redirect.
            MemCharacter creatorsChar = eventInterface.GetEventCreatorsCharacter(MemberInfo.MemberID, eventID);
            if (creatorsChar == null) { GlobalMethod.RedirectBack(); }

            ManageEventModel Model = new ManageEventModel();
            Event usersSelectedEvent = eventInterface.GetAllEventsByMemberID(MemberInfo.MemberID).Where(e => e.EventID == eventID).SingleOrDefault();
            Model.EventInfo = usersSelectedEvent;
            CurrentEvent = usersSelectedEvent;
            CurrentCharacter = charInterface.GetCharacterByID(creatorsChar.CharacterID);

            if (Model.EventInfo != null)
            {
                Model.EventTypeInfo = eventInterface.GetAllEventTypes().Where(t => t.EventTypeID == eventID).SingleOrDefault();
                Model.Attendees = eventInterface.GetAllEventAttendees(eventID);
                Model.AttnCharacters = (from attn in Model.Attendees
                                        select charInterface.GetCharacterByID(attn.CharacterID)).ToList();
                //Model.RolesInfo = (from attn in Model.Attendees
                //                   join roles in charInterface.GetRolesForGame((int)Model.EventInfo.GameID) on attn.RoleID equals roles.RoleID
                //                   group attn by roles.RoleID into attnRoles
                //                   select new RoleInfo() { 
                //                       RoleType = charInterface.GetRoleByID(attnRoles.FirstOrDefault().RoleID), 
                //                       Count = attnRoles.Count(),
                //                       CRoleRestriction = eventInterface.GetRoleRestrictionsByEvent(Model.EventInfo.EventID).Where(r => r.RoleID == attnRoles.FirstOrDefault().RoleID).SingleOrDefault()
                //                   }).ToList();
                Model.RolesInfo = (from role in gameInterface.GetAllRolesByGameID(Model.EventInfo.GameID)
                                   select new EventRoleInfo()
                                   {
                                       Role = role,
                                       RoleCount = Model.Attendees.Where(attn => attn.RoleID == role.RoleID).Count(),
                                       RoleRestriction = eventInterface.GetRoleRestictionByEvent(eventID, role.RoleID)
                                   }).ToList();
                Model.ClassInfo = (from clss in gameInterface.GetAllClassesByGame((int)Model.EventInfo.GameID)
                                   select new EventClassInfo()
                                   {
                                       Class = clss,
                                       ClassCount = Model.AttnCharacters.Where(attnChar => attnChar.ClassID == clss.ClassID).Count(),
                                       ClassRestrictionCount = eventInterface.GetClassResctrictionCountForEventbyClassID(eventID, clss.ClassID)
                                   }).ToList();
                Model.ServerName = gameInterface.GetServer((int)Model.EventInfo.ServerID).Name;

            }
            else
            {
                //couldnt find the event passed in the URL
                //Display some Error
            }

            return View(Model);
        }

        [RequiresLogin()]
        //[RequiresCurrentEventContext()]
        public ActionResult ViewEvent(int id)
        {
            int eventID = id; //Lulz
            ViewEventModel Model = new ViewEventModel();
            Model.EventInfo = eventInterface.GetEventInfoByID(eventID);
            CurrentEvent = Model.EventInfo;
            if (Model.EventInfo != null)
            {
                CurrentEvent = Model.EventInfo;
                Model.EventTypeInfo = eventInterface.GetAllEventTypes().Where(t => t.EventTypeID == eventID).SingleOrDefault();
                Model.Attendees = eventInterface.GetAllEventAttendees(eventID);
                Model.IsUserAttendee = Model.Attendees.Any(attn => attn.MemberID == MemberInfo.MemberID);
                Model.AttnCharacters = (from attn in Model.Attendees
                                        select charInterface.GetCharacterByID(attn.CharacterID)).ToList();
                Model.RolesInfo = (from role in gameInterface.GetAllRolesByGameID(Model.EventInfo.GameID)
                                   select new EventRoleInfo()
                                   {
                                       Role = role,
                                       RoleCount = Model.Attendees.Where(attn => attn.RoleID == role.RoleID).Count(),
                                       RoleRestriction = eventInterface.GetRoleRestictionByEvent(eventID, role.RoleID)
                                   }).ToList();
                Model.ClassInfo = (from clss in gameInterface.GetAllClassesByGame((int)Model.EventInfo.GameID)
                                   select new EventClassInfo()
                                   {
                                       Class = clss,
                                       ClassCount = Model.AttnCharacters.Where(attnChar => attnChar.ClassID == clss.ClassID).Count(),
                                       ClassRestrictionCount = eventInterface.GetClassResctrictionCountForEventbyClassID(eventID, clss.ClassID)
                                   }).ToList();
                Model.ServerName = gameInterface.GetServer((int)Model.EventInfo.ServerID).Name;
            }
            else
            {
                //couldnt find the event passed in the URL
                //Display some Error
            }

            return View(Model);
        }

        public ViewResult DisplayMonth(int monthNum, int year)
        {
            RaidCalendar c = new RaidCalendar(monthNum, year, eventInterface);
            c.PopulateEventsForThisMonth(eventInterface, MemberInfo.MemberID);

            return View("Index", c);
        }

        [RequiresLogin()]
        public ViewResult Add(string date)
        {
            DateTime evtDate = new DateTime();
            ViewData["Date"] = (DateTime.TryParse(date, out evtDate)) ? evtDate : DateTime.Now;

            AddEventModel model = new AddEventModel();
            model.Games = accountInterface.GetMembersGamesAsGames(MemberInfo.MemberID);
            model.RolesInfo = (from role in gameInterface.GetAllRolesByGameID(1) //TODO: Remove World Of Warcraft Hardcode
                               select new EventRoleInfo()
                               {
                                   Role = role,
                                   RoleCount = 0,
                                   RoleRestriction = null
                               }).ToList();

            return View(model);
        }

        //[NoCache]
        public JsonResult GetAllEventGroupsAJAX(int gameID)
        {
            EventGroup[] eventGroups = eventInterface.GetAllEventGroupsByGameID(gameID).ToArray();

            return new JsonResult() { Data = eventGroups };
        }

        //[NoCache]
        public JsonResult GetAllEventSubGroupsAJAX(int groupID)
        {
            EventGroupSubType[] eventSubGrps = eventInterface.GetAllEventGroupSubTypes(groupID).ToArray();

            return new JsonResult() { Data = eventSubGrps };
        }

        //[NoCache]
        public JsonResult GetAllEventGroupsWithSubGroups(int gameID)
        {
            EventGroup[] eventGroups = eventInterface.GetAllEventGroupsByGameID(gameID).ToArray();

            List<EventGroupsWithSubGroups> grpsWithSubGrps = new List<EventGroupsWithSubGroups>();
            foreach(EventGroup grp in eventGroups)
            {
                EventGroupSubType[] eventSubGrps = eventInterface.GetAllEventGroupSubTypes(grp.EventGroupID).ToArray();
                grpsWithSubGrps.Add(new EventGroupsWithSubGroups(grp, eventSubGrps));
            }

            return new JsonResult() { Data = grpsWithSubGrps.ToArray() };
        }

        public JsonResult GetAllEventTypesBySubGrpIDAJAX(int subGrpID)
        {
            EventType[] eventTypes = eventInterface.GetAllEventTypesBySubGrpID(subGrpID).ToArray();

            return new JsonResult() { Data = eventTypes };
        }

        public JsonResult AddNewEventAJAX(string name, string date, string timeHour, string timeMin, string timeMeridian, string typeID, int charID, int roleID, 
                                          string meetupLocation, string notes, string inHealRestriction, string inTankRestriction, string inDamageRestriction)
        {
            List<string> errors = new List<string>();
            DateTime evtDate = new DateTime();
            if (!DateTime.TryParse(date, out evtDate)) { errors.Add(Errors.DATE_INVALID); }
            //Validate that charID is for a character that belongs to the current member
            MemCharacter creatorCharacter = charInterface.GetCharacterByID(charID);
            if (creatorCharacter == null) { errors.Add(Errors.CHARACTER_NOT_FOUND); }
            //Validate Time
            TimeSpan eventTime = new TimeSpan();
            string time = DataAccessLayer.Global.Global.ConvertMeridianTimeTo24Hour(timeHour, timeMin, timeMeridian);
            //if (!System.Text.RegularExpressions.Regex.IsMatch(time, @"[0-23]:[0-59]")) { errors.Add(Errors.TIME_INVALID); }
            if (!TimeSpan.TryParse(time, out eventTime)) { errors.Add(Errors.TIME_INVALID); }

            //Validate Role Restrictions
            string healerRestrictionInput = (!String.IsNullOrEmpty(inHealRestriction)) ? inHealRestriction : "0";
            string tankRestrictionInput = (!String.IsNullOrEmpty(inTankRestriction)) ? inTankRestriction : "0";
            string damageRestrictionInput = (!String.IsNullOrEmpty(inDamageRestriction)) ? inDamageRestriction : "0";

            int healerRestriction = 0;
            if(!Int32.TryParse(healerRestrictionInput, out healerRestriction))
            {
                errors.Add("Issue with Healer Restriction");
            }
            int tankRestriction = 0;
            if (!Int32.TryParse(tankRestrictionInput, out tankRestriction))
            {
                errors.Add("Issue with Tank Restriction");
            }
            int damageRestriction = 0;
            if (!Int32.TryParse(damageRestrictionInput, out damageRestriction))
            {
                errors.Add("Issue with Damage Restriction");
            }


            //Need to handle event time better. If I wanted to make an hourly calendar how would that work
            bool success = false;
            if (errors.Count == 0)
            {
                //Add new Event
                Event evt = new Event()
                {
                    Date = evtDate,
                    EventCreaterMemberID = MemberInfo.MemberID,
                    EventName = name,
                    //EventRestrictionsID = null,
                    EventTime = eventTime,
                    EventTypeID = Convert.ToInt32(typeID),
                    MeetupLocation = meetupLocation,
                    Notes = notes,
                    GameID = creatorCharacter.GameID,
                    Active = true,
                    ServerID = (int)creatorCharacter.ServerID
                    
                };
                success = eventInterface.AddNewEvent(evt);

                //If there is a serverID or a factionID go ahead and setup a basic restriction
                if (evt.ServerID != null || creatorCharacter.FactionID != null)
                {
                    eventInterface.AddBasicRestrictions(evt.EventID, evt.ServerID, creatorCharacter.FactionID);
                }

                //Add Event Restrictions
                RoleRestriction[] roleRestrictions = new RoleRestriction[] {
                    new RoleRestriction() { RoleID = 1, Quantity = damageRestriction}, 
                      new RoleRestriction() {RoleID = 2, Quantity = healerRestriction},
                      new RoleRestriction() {RoleID = 3, Quantity = tankRestriction}
                    };
                eventInterface.InsertRoleRestrictions(evt.EventID, roleRestrictions);

                //Creator is automatically added to the raid attendees
                CommitResponse autoMemberAddSuccess  = eventInterface.AddNewEventAttendee(charID, evt.EventID, roleID, String.Empty, (int)ATTENDEE_STATUS.ACCEPTED, MemberInfo.MemberID);
            }
            else
            {
                //There was an issue with input
            }


            //Success Response
            return new JsonResult() { Data = success };
        }

        [NoCache()]
        public JsonResult GetMembersEventsByDayAJAX(int day, int month, int year)
        {
            //DateTime date = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));
            DateTime date = new DateTime(year, month, day);

            var eventInfo = from e in eventInterface.GetAllEventsForDayByMemberID(MemberInfo.MemberID, date)
                            join etype in eventInterface.GetAllEventTypes() on e.EventTypeID equals etype.EventTypeID
                            select new
                            {
                                e.EventID,
                                e.EventName,
                                etype.EventTypeName,
                                e.Date,
                                Players = String.Format("{0}/{1}", eventInterface.GetAllEventAttendees(e.EventID).Count(), etype.PlayerCount)
                            };

            return new JsonResult() { Data = eventInfo };

        }

        [NoCache()]
        public JsonResult GetRosterAJAX(int eventID)
        {
            Event eventInfo = eventInterface.GetEventInfoByID(eventID);

            var attendees = from attn in eventInterface.GetAllEventAttendees(eventID)
                            join memChar in charInterface.GetAllCharactersByGameID((int)eventInfo.GameID) on attn.CharacterID equals memChar.CharacterID
                            select new { 
                                attn.AttendeeID,
                                Status = eventInterface.DecodeAttendeeStatus(attn.Status),
                                memChar.CharacterID,
                                memChar.CharacterName,
                                memChar.LVL,
                                RoleName = (attn.RoleID.HasValue) ? charInterface.GetRoleByID(attn.RoleID.Value).RoleName : "Not Selected",
                                Note = (!String.IsNullOrEmpty(attn.Note)) ? attn.Note : "N/A" ,
                                gameInterface.GetClassByID(memChar.ClassID).ImageLocation,
                                IsCreator = (eventInfo.EventCreaterMemberID == memChar.MemberID)
                            };
            //attendees.FirstOrDefault().CharacterID

            return new JsonResult() { Data = attendees };

        }

        [NoCache()]
        public JsonResult AddCharacterToEventAJAX(int? charID, int? roleID)
        {
            CommitResponse commitResponse = new CommitResponse();
            if (charID != null && roleID != null)
            {
                commitResponse = eventInterface.AddNewEventAttendee((int)charID, CurrentEvent.EventID, (int)roleID, String.Empty, (int)ATTENDEE_STATUS.INVITED, null);
            }

            return new JsonResult() { Data = commitResponse };
        }

        //[NoCache()]
        [RequiresLogin()]
        public ActionResult Edit(int id)
        {
            int eventID = id;

            //Get Attendee info and ensure user belongs to this event
            MemCharacter creatorsChar = eventInterface.GetEventCreatorsCharacter(MemberInfo.MemberID, eventID);
            if (creatorsChar == null) { GlobalMethod.RedirectBack(); }

            EditEventModel model = new EditEventModel();
            model.EventInfo = eventInterface.GetEventInfoByID(eventID);
            if (model.EventInfo != null)
            {
                model.GameID = (int)model.EventInfo.GameID;
                model.EventTypeInfo = eventInterface.GetAllEventTypes().Where(e => e.EventTypeID == model.EventInfo.EventTypeID).SingleOrDefault();
                model.EventSubTypeInfo = eventInterface.GetEventGroupSubTypeByID(model.EventTypeInfo.EventGroupSubTypeID);
                model.RolesInfo= (from role in gameInterface.GetAllRolesByGameID(model.GameID)
                                   select new EventRoleInfo()
                                   {
                                       Role = role,
                                       RoleCount = 0,
                                       RoleRestriction = eventInterface.GetRoleRestictionByEvent(eventID, role.RoleID)
                                   }).ToList();
                model.CharID = creatorsChar.CharacterID;
                model.Games = accountInterface.GetMembersGamesAsGames(MemberInfo.MemberID);

                CurrentEvent = model.EventInfo;

                return View(model);
            }
            else
            {
                //TODO: This should display an error message
                return RedirectToAction("ManageEvents");
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [RequiresCurrentEventContext()]
        public ActionResult Edit(Event evt)
        {
            int charID = Convert.ToInt32(Request["Char"]);
            MemCharacter creatorCharacter = charInterface.GetCharacterByID(charID);

            //Add in sensetive data
            evt.EventID = CurrentEvent.EventID;
            evt.GameID = CurrentEvent.GameID;
            evt.EventTypeID = (evt.EventTypeID != 0) ? evt.EventTypeID : CurrentEvent.EventTypeID;
            evt.EventCreaterMemberID = CurrentEvent.EventCreaterMemberID;
            evt.ServerID = creatorCharacter.ServerID;

            string note = (!String.IsNullOrEmpty(Request["Note"])) ? Request["Note"] : String.Empty;
            int roleID =  (!String.IsNullOrEmpty(Request["RoleID"])) ? Convert.ToInt32(Request["RoleID"]) : 0;

            //Add in Role Restriction Data
            string healerRestrictionInput = (!String.IsNullOrEmpty(Request["HealerRestriction"])) ? Request["HealerRestriction"] : "0";
            string tankRestrictionInput = (!String.IsNullOrEmpty(Request["TankRestriction"])) ? Request["TankRestriction"] : "0";
            string damageRestrictionInput = (!String.IsNullOrEmpty(Request["DamageRestriction"])) ? Request["DamageRestriction"] : "0";
            int healerRestriction = 0;
            if (!Int32.TryParse(healerRestrictionInput, out healerRestriction))
            {
                //errors.Add("Issue with Healer Restriction");
            }
            int tankRestriction = 0;
            if (!Int32.TryParse(tankRestrictionInput, out tankRestriction))
            {
                //errors.Add("Issue with Tank Restriction");
            }
            int damageRestriction = 0;
            if (!Int32.TryParse(damageRestrictionInput, out damageRestriction))
            {
                //errors.Add("Issue with Damage Restriction");
            }

            bool success = eventInterface.EditEvent(evt);
            if (success)
            {
                eventInterface.UpdateAttendeeStatus(CurrentEvent.EventID, MemberInfo.MemberID, (int)ATTENDEE_STATUS.ACCEPTED, charID , note, roleID);

                //Updatee Restrictions
                    RoleRestriction[] roleRestrictions = new RoleRestriction[] {
                        new RoleRestriction() { RoleID = 1, Quantity = damageRestriction}, 
                        new RoleRestriction() {RoleID = 2, Quantity = healerRestriction},
                        new RoleRestriction() {RoleID = 3, Quantity = tankRestriction}
                    };
                eventInterface.UpdateRestrictions(CurrentEvent.EventID, roleRestrictions, evt.ServerID, creatorCharacter.FactionID);
                //eventInterface.UpdateRoleRestrictions(CurrentEvent.EventID, roleRestrictions);

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("Error", "Couldnt edit this event");
                return View();
            }
        }

        [NoCache]
        public JsonResult GetEditEventInfoAJAX(int evtID)
        {
            List<object> eventInfo = new List<object>();
            Event evtInfo = eventInterface.GetEventInfoByID(evtID);
            EventAJAX evtInfoAJAX = new EventAJAX(evtInfo);
            eventInfo.Add(evtInfoAJAX);

            //character info
            MemCharacter creatorMemCharInfo = eventInterface.GetEventCreatorsCharacter(evtInfo.EventCreaterMemberID, evtID);
            MemCharacterAJAX cMemCharInfoAJAX = new MemCharacterAJAX(creatorMemCharInfo);
            eventInfo.Add(cMemCharInfoAJAX);

            //role info
            Role[] CharRoles = charInterface.GetRolesByCharacterID(creatorMemCharInfo.CharacterID).ToArray();
            eventInfo.Add(CharRoles);
            int CurrentRoleID = eventInterface.GetAllEventAttendees(evtID).Where(attn => attn.CharacterID == creatorMemCharInfo.CharacterID).SingleOrDefault().RoleID.Value;
            //Role creatorsAssignedRoleInfo = charInterface.GetRoleByID(creatorsRoleID);
            eventInfo.Add(CurrentRoleID);

            //class info
            Class creatorsClass = gameInterface.GetClassByID(creatorMemCharInfo.ClassID);
            eventInfo.Add(creatorsClass);

            return new JsonResult() { Data = eventInfo.ToArray() };
        }

        [RequiresLogin()]
        public ViewResult EventsOverview(string date)
        {
            List<EventOverview> eventInfo = new List<EventOverview>();
            ViewData["BadDate"] = true;
            DateTime evtDate = new DateTime();
            if (DateTime.TryParse(date, out evtDate))
            {
                eventInfo = (from e in eventInterface.GetAllEventsForDayByMemberID(MemberInfo.MemberID, evtDate)
                             join etype in eventInterface.GetAllEventTypes() on e.EventTypeID equals etype.EventTypeID
                             select new EventOverview()
                             {
                                 EventInfo = e,
                                 EventTypeName = etype.EventTypeName,
                                 EventImageFile = etype.ImageFile,
                                 CurrentAttendees = String.Format("{0}/{1}", eventInterface.GetAllEventAttendees(e.EventID).Count(), etype.PlayerCount),
                                 UserIsCreator = (eventInterface.GetEventCreatorsMemberInfo(e.EventID).MemberID == MemberInfo.MemberID)
                                 //e.EventID,
                                 //e.EventName,
                                 //etype.EventTypeName,
                                 //e.Date,
                                 //Players = String.Format("{0}/{1}", eventInterface.GetAllEventAttendees(e.EventID).Count(), etype.PlayerCount)
                             }).OrderBy(e => e.EventInfo.GetDateTime()).ToList();

                ViewData["BadDate"] = false;
            }

            ViewData["Date"] = (!String.IsNullOrEmpty(evtDate.ToString())) ? evtDate : new DateTime();

            return View(eventInfo);
        }

        public JsonResult DeactivateEventAJAX(int id)
        {
            CommitResponse response = eventInterface.DeactivateEvent(MemberInfo.MemberID, id);

            return new JsonResult() { Data = response };
        }

        [NoCache()]
        public JsonResult LeaveEventAJAX(int eventID)
        {
            EventAttendee attn = eventInterface.GetEventAttendee(eventID, MemberInfo.MemberID);
            CommitResponse response = eventInterface.LeaveEvent(eventID, attn.CharacterID);
            
            return new JsonResult(){ Data = response };
        }

        [RequiresLogin()]
        public ViewResult Search()
        {
            EventSearchModel model = new EventSearchModel();
            
            model.Games = gameInterface.GetAllGames(true).ToList();
            model.Games.Insert(0, new Game(){GameID = -1, GameName = "Select a Game"});

            ViewData.Add("StartMeridian", "PM");
            ViewData.Add("EndMeridian", "PM");

            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ViewResult Search(EventSearchModel eventSearch)
        {
            eventSearch.EventSearchParams.EventStartTime = GetTimespanFromStringValues(Request["startHour"], Request["startMinute"], Request["startMeridian"]);
            eventSearch.EventSearchParams.EventEndTime = GetTimespanFromStringValues(Request["endHour"], Request["endMinute"], Request["endMeridian"]);
            //eventSearch.EventSearchParams.ServerID = (eventSearch.EventSearchParams.ServerID > 0) ? eventSearch.EventSearchParams.ServerID : null;

            eventSearch.Events = eventInterface.SearchEvents(eventSearch.EventSearchParams, MemberInfo.MemberID);
            if (eventSearch.Events.Count < 1)
            {
                //No Events were found
            }

            //Re-pop games
            eventSearch.Games = gameInterface.GetAllGames(true).ToList();
            eventSearch.Games.Insert(0, new Game() { GameID = -1, GameName = "Select a Game" });

            //SelectList startMeridian = new SelectList(new List<string> { "AM", "PM" }, Request["startMeridian"]);
            //ViewData["startMeridianList"] = 

            //Re-pop Times
            ViewData.Add("StartHour", Request["startHour"]);
            ViewData.Add("StartMinute", Request["startMinute"]);
            ViewData.Add("StartMeridian", Request["startMeridian"]);

            ViewData.Add("EndHour", Request["endHour"]);
            ViewData.Add("EndMinute", Request["endMinute"]);
            ViewData.Add("EndMeridian", Request["endMeridian"]);
            

            return View(eventSearch);
        }

        [RequiresCurrentEventContext()]
        public ViewResult JoinEvent(int? gameID)
        {
            int intGameID = Convert.ToInt32(gameID);
            JoinEventModel model = new JoinEventModel();
            model.IsEventFull = eventInterface.IsEventFull(CurrentEvent.EventID);
            model.UserCharacters = (from memChar in charInterface.GetAllMemberCharactersByGame(MemberInfo.MemberID, intGameID)
                                    where memChar.ServerID == (int)CurrentEvent.ServerID // Filter by characters which exists on the same server as the event
                                    select memChar).ToList();
            model.RestrictedRoles = eventInterface.GetRestrictedRolesThatAreFull(CurrentEvent.EventID).ToList();
            model.CantJoinEventReason = (model.UserCharacters.Count > 0) ? null : GetReasonWhyUserCantJoinEvent(intGameID, (int)CurrentEvent.ServerID);
            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult JoinEvent(JoinEventModel model)
        {
            List<string> Error = new List<string>();

            int roleID = (!String.IsNullOrEmpty(Request["Role"])) ? Convert.ToInt32(Request["Role"]) : 0;
            if(roleID == 0)
            {
                Error.Add("Role Not Specified");
            }

            int charID = (!String.IsNullOrEmpty(Request["SelectedCharacter"])) ? Convert.ToInt32(Request["SelectedCharacter"]) : 0;
            if(charID == 0)
            {
                Error.Add("Character not Specified");
            }

            // Ensure the character exists on the same server as the evemt
            if (charInterface.GetCharacterByID(charID).ServerID != CurrentEvent.ServerID)
            {
                Error.Add("Character doesnt exist on same server as event");
            }

            //Enfore Role Restrictions
            if(roleID != 0 && eventInterface.IsRoleRestricted(CurrentEvent.EventID, roleID))
            {
                Error.Add("The role you selected is full");
            }

            if (Error.Count == 0)
            {
                MemCharacter memChar = charInterface.GetCharacterByID(charID);
                if (charInterface.ValidateUserRoles(memChar.ClassID, new int[] { roleID }))
                {
                    CommitResponse response = eventInterface.AddNewEventAttendee(charID, CurrentEvent.EventID, roleID, model.Note, model.AttendeeStatus, MemberInfo.MemberID);
                    if (response.success)
                    {
                        return RedirectToAction("Confirmation", "Shared");
                    }
                }
            }
            else
            {
                int eCount = 0;
                foreach(string e in Error)
                {
                    this.ModelState.AddModelError("Error" + eCount, e);
                }
            }

            model.UserCharacters = charInterface.GetAllMemberCharactersByGame(MemberInfo.MemberID, CurrentEvent.GameID).ToList();
            model.RestrictedRoles = eventInterface.GetRestrictedRolesThatAreFull(CurrentEvent.EventID).ToList();
            return View(model);
        }

        [RequiresLogin()]
        [RequiresCurrentEventContext()]
        public ViewResult UpdateStatus(int gameID)
        {
            UpdateStatusModel model = new UpdateStatusModel();
            var attn = eventInterface.GetEventAttendee(CurrentEvent.EventID, MemberInfo.MemberID);
            model.Note = attn.Note;
            model.AttendeeStatus = attn.Status;
            model.SelectedRole = attn.RoleID.Value;
            model.SelectedCharacter = attn.CharacterID;
            model.UserCharacters = (from memChar in charInterface.GetAllMemberCharactersByGame(MemberInfo.MemberID, gameID)
                                    where memChar.ServerID == (int)CurrentEvent.ServerID // Filter by characters which exists on the same server as the event
                                    select memChar).ToList();
            model.RestrictedRoles = eventInterface.GetRestrictedRolesThatAreFull(CurrentEvent.EventID).ToList();

            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateStatus(UpdateStatusModel model)
        {
            List<string> Error = new List<string>();
            EventAttendee attn = eventInterface.GetEventAttendee(CurrentEvent.EventID, MemberInfo.MemberID);
            int roleID = (!String.IsNullOrEmpty(Request["Role"])) ? Convert.ToInt32(Request["Role"]) : 0;
            if (roleID == 0)
            {
                Error.Add("Role Not Specified");
            }

            int charID = (!String.IsNullOrEmpty(Request["SelectedCharacter"])) ? Convert.ToInt32(Request["SelectedCharacter"]) : 0;
            if (charID == 0)
            {
                Error.Add("Character not Specified");
            }

            // Ensure the character exists on the same server as the evemt
            if (charInterface.GetCharacterByID(charID).ServerID != CurrentEvent.ServerID)
            {
                Error.Add("Character doesnt exist on same server as event");
            }

            //Enfore Role Restrictions
            bool roleHasChanged = (attn.RoleID != roleID);
            if (roleID != 0 && roleHasChanged && eventInterface.IsRoleRestricted(CurrentEvent.EventID, roleID))
            {
                Error.Add("The role you selected is full");
            }

            if (Error.Count == 0)
            {
                MemCharacter memChar = charInterface.GetCharacterByID(charID);
                if (charInterface.ValidateUserRoles(memChar.ClassID, new int[] { roleID }))
                {
                    CommitResponse response = eventInterface.UpdateAttendeeStatus(CurrentEvent.EventID, MemberInfo.MemberID, model.AttendeeStatus, charID, model.Note, roleID);
                    if (response.success)
                    {
                        return RedirectToAction("Confirmation", "Shared");
                    }
                }
            }
            else
            {
                int eCount = 0;
                foreach (string e in Error)
                {
                    this.ModelState.AddModelError("Error" + eCount, e);
                }
            }

            model.UserCharacters = charInterface.GetAllMemberCharactersByGame(MemberInfo.MemberID, CurrentEvent.GameID).ToList();
            model.RestrictedRoles = eventInterface.GetRestrictedRolesThatAreFull(CurrentEvent.EventID).ToList();
            return View(model);
        }

        [NoCache]
        public JsonResult UpdateAttendeeStatusAsCreatorAJAX(int statusID, string chars)
        {
            CommitResponse response = new CommitResponse();
            //bool userOwnsThisCharacter = charInterface.GetAllMemberCharacters(MemberInfo.MemberID).Any(c => c.CharacterID == charID);
            bool userIsEventCreator = eventInterface.GetEventCreatorsMemberInfo(CurrentEvent.EventID).MemberID == MemberInfo.MemberID;

            if (userIsEventCreator)
            {
                int[] realCharIDs = chars.Split(new char[] { '|' }).Select(s => Int32.Parse(s)).ToArray();
                if (realCharIDs.Length > 0)
                {
                    response = eventInterface.UpdateAttendeeAttendenceStatusBulk(CurrentEvent.EventID, realCharIDs, statusID);
                }
            }
            else
            {
                response.success = false;
                response.errorMsg = Errors.CAN_NOT_OPERATE_ON_OTHER_MEMBERS_CHARACTER;
            }

            return new JsonResult() { Data = response };
        }

        [NoCache]
        public JsonResult RemoveAttendeesAsCreator(string chars)
        {
            CommitResponse response = new CommitResponse();
            //bool userOwnsThisCharacter = charInterface.GetAllMemberCharacters(MemberInfo.MemberID).Any(c => c.CharacterID == charID);
            bool userIsEventCreator = eventInterface.GetEventCreatorsMemberInfo(CurrentEvent.EventID).MemberID == MemberInfo.MemberID;

            if (userIsEventCreator)
            {
                int[] realCharIDs = chars.Split(new char[] { '|' }).Select(s => Int32.Parse(s)).ToArray();
                if (realCharIDs.Length > 0)
                {
                    response = eventInterface.RemoveAttendeeBulk(CurrentEvent.EventID, realCharIDs);
                }
            }
            else
            {
                response.success = false;
                response.errorMsg = Errors.CAN_NOT_OPERATE_ON_OTHER_MEMBERS_CHARACTER;
            }

            return new JsonResult() { Data = response };
        }

        //Requires Event Context
        [RequiresCurrentEventContext()]
        public ViewResult Invite()
        {            
            InviteModel model = new InviteModel();
            model.FriendsList = accountInterface.GetFriendsForMemberWithEventRestrictions(MemberInfo.MemberID, CurrentEvent);
            foreach (MemFriend friend in model.FriendsList)
            {
                friend.HighlightOnList = !friend.Restricted; //First evaluate is the character is restricted
                if (!friend.Restricted) //Second evaluate Role/Class/ETc.
                {
                    friend.HighlightOnList = eventInterface.IsCharacterInDemandForEvent(CurrentEvent.EventID, friend.FriendCharacterID);
                }
            }
            model.EventInfo = CurrentEvent;
            model.Event_GameName = gameInterface.GetGameByID(CurrentEvent.GameID).GameName;
            model.Event_ServerName = gameInterface.GetServer((int)CurrentEvent.ServerID).Name;
            model.EventRestritions = eventInterface.GetEventRestriction(CurrentEvent.EventID) ?? new EventRestriction(); //Return a new instance of restrictions beacuse every field should be null anyways

            return View(model);
        }

        public ViewResult InviteFriends(InviteModel model)
        {
            string selectedCharacters = Request["SelectedCharacter"];
            if (!String.IsNullOrEmpty(selectedCharacters))
            {
                int[] friendsToAdd = selectedCharacters.Split(new char[] { ',' }).Select(charID => Convert.ToInt32(charID)).ToArray();

                if (friendsToAdd.Length > 0)
                {
                    List<string> friendsNotAdded = eventInterface.AddNewEventAttendeesBulk(friendsToAdd, CurrentEvent.EventID, (int)ATTENDEE_STATUS.INVITED)
                                                   .Select(x => String.Format("Couldn't invite {0}, there was a conflict.", x)).ToList();

                    RegisterErrorsWithModel("friendInviteFailure", friendsNotAdded);

                    if (friendsNotAdded.Count < friendsToAdd.Length)
                    {
                        ViewData["FriendAddSuccessMsg"] = "<p><strong>Friends where successfully added</string></p>";
                    }
                }
            }

            //Rebind all this shit
            model.FriendsList = accountInterface.GetFriendsForMemberWithEventRestrictions(MemberInfo.MemberID, CurrentEvent);
            foreach (MemFriend friend in model.FriendsList)
            {
                friend.HighlightOnList = !friend.Restricted; //First evaluate is the character is restricted
                if (!friend.Restricted) //Second evaluate Role/Class/ETc.
                {
                    friend.HighlightOnList = eventInterface.IsCharacterInDemandForEvent(CurrentEvent.EventID, friend.FriendCharacterID);
                }
            }
            model.EventInfo = CurrentEvent;
            model.Event_GameName = gameInterface.GetGameByID(CurrentEvent.GameID).GameName;
            model.Event_ServerName = gameInterface.GetServer((int)CurrentEvent.ServerID).Name;
            model.EventRestritions = eventInterface.GetEventRestriction(CurrentEvent.EventID) ?? new EventRestriction(); //Return a new instance of restrictions beacuse every field should be null anyways
            return View("Invite", model);
        }

        //[NoCache] cache so that if users enter the same emai twice nothing happens
        public JsonResult InviteViaEmailAJAX(string e)
        {
            string emailAddress = e;

            if(!Validate.Email(emailAddress))
            {
                ValidationErrors.Add("Invalid Email", Errors.INVALID_EMAIL);
            }

            CommitResponse response = new CommitResponse() { success = false };
            if (ValidationErrors.Count == 0)
            {
                //Capture the Invitation
                accountInterface.CaptureEmailInvite(MemberInfo.MemberID, emailAddress, CurrentEvent.EventID);

                //Send out the email
                Dictionary<string, string> replaceValues = new Dictionary<string, string>();
                replaceValues.Add("INVITERS_EMAIL", MemberInfo.Email);
                string gameName = gameInterface.GetGameByID(CurrentEvent.GameID).GameName;
                replaceValues.Add("GAME_NAME", gameName);
                string eventTypeName = eventInterface.GetEventTypeByID(CurrentEvent.EventID).EventTypeName;
                replaceValues.Add("EVENT_TYPE", eventTypeName);
                replaceValues.Add("EVENT_LINK", CurrentEvent.GenerateEventURL(System.Web.HttpContext.Current));

                try
                {
                    SMTPEmail.SMTPEMailS email = new SMTPEmail.SMTPEMailS(replaceValues);
                    string emailFileLocation = Server.MapPath("~/Static/InviteEmail.txt");
                    response.success = email.SendEmailFromFile(new string[] { emailAddress }, "DoNotReply@lfmore.com", "A friend has invited you to a Group", emailFileLocation, null, false);
                }
                catch (Exception ex)
                {
                    //TODO: We really want to know if this fails. Its possible to capture all the failures and then email them out later
                }

            }

            return new JsonResult() { Data = response };
        }
































        private string GetReasonWhyUserCantJoinEvent(int gameID, int eventServerID)
        {
            //Does user have any characters for this game?
            List<CompleteCharacterData> userCharactersForGame = charInterface.GetAllMemberCharactersByGame(MemberInfo.MemberID, gameID).ToList();
            if(userCharactersForGame.Count == 0)
            {
                return "You have no characters for this game";
            }

            //Does user have any character on this server?
            if (userCharactersForGame.Where(c => c.ServerID == eventServerID).Count() == 0)
            {
                return "You have no characters on the server for this event";
            }

            return String.Empty;
        }

        private TimeSpan? GetTimespanFromStringValues(string hour, string minute, string meridian)
        {
            TimeSpan? t = null;
            if (!String.IsNullOrEmpty(hour))
            {
                string validMinute = (String.IsNullOrEmpty(minute)) ? "00" : minute;
                TimeSpan validTimeSpan = new TimeSpan();
                t = (TimeSpan.TryParse(String.Format("{0}:{1}", hour, validMinute), out validTimeSpan)) ? (Nullable<TimeSpan>)validTimeSpan : null;
                if (t != null && meridian == "PM") { t.Value.Add(new TimeSpan(12, 0, 0));  }
            }
            return t;
        }
    }
}
