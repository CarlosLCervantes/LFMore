using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using DataAccessLayer;
using LeetRaids.Models;

namespace LeetRaids.Controllers
{
    public class AccountController : BaseController
    {
        AccountInterface accountInterface;
        GameInterface gameInterface;
        CharacterInterface charInterface;

        public AccountController(AccountInterface currentAccountInterface, GameInterface currentGameInterface, CharacterInterface currentCharInterface)
        {
            accountInterface = currentAccountInterface;
            gameInterface = currentGameInterface;
            charInterface = currentCharInterface;
        }

        public ActionResult Index()
        {
            return View();
        }

        #region ======================Register/Login/Logout======================

        public ActionResult Register()
        {
            if (!GlobalMethod.UserLoggedIn())
            {
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Register(Member member)
        {
            string timeZone = Request["ddlTimezone"];
            member.TimeZone = timeZone;
            string[] playTime = Request["ddlTime"].Split(new char[] {','});
            string[] playTimeStartByUnit = playTime[0].Split(new char[] {':'});
            string[] playTimeEndByUnit = playTime[1].Split(new char[] {':'});

            TimeSpan playTimeStart = new TimeSpan(Convert.ToInt32(playTimeStartByUnit[0]), Convert.ToInt32(playTimeStartByUnit[1]), 0);
            member.PlayTimeStart = playTimeStart;

            TimeSpan playTimeEnd = new TimeSpan(Convert.ToInt32(playTimeEndByUnit[0]), Convert.ToInt32(playTimeEndByUnit[1]), 0);
            member.PlayTimeEnd = playTimeEnd;

            member.CreateDT = DateTime.Now;

            if (ModelState.IsValid)
            {
                CommitResponse response = accountInterface.InsertMember(member);

                if (!response.success)
                {
                    RegisterErrorsWithModel("regError", response.Errors);
                    //ModelState.AddModelError("Error", response.errorMsg);
                    return View(member);
                }
            }

            //if (TempData["UserJustRegistered"] == null)
            //{
            TempData.Add("UserJustRegistered", true);
            //}
            Session.Add(GlobalConst.SESSION_MEMBER, member);
            return RedirectToAction("ManageCharacters");
        }

        public ActionResult Login()
        {
            if (!GlobalMethod.UserLoggedIn())
            {
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Login(Member user)
        {
            Member member = accountInterface.Login(user);

            if (member != null)
            {
                MemberInfo = member;

                if (Session[GlobalConst.SESSION_PREVIOUS_ACTION] != null && !String.IsNullOrEmpty(Session[GlobalConst.SESSION_PREVIOUS_ACTION].ToString()))
                {
                    //return RedirectToAction("Index", "Home");
                    Response.Redirect(Session[GlobalConst.SESSION_PREVIOUS_ACTION].ToString());
                }
                //return View(member);
            }
            else
            {
                TempData["LoginFailure"] = true;
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult LoginPanel()
        {

            ViewData["UserLoggedIn"] = GlobalMethod.UserLoggedIn();
            return View();
        }

        //LoginPanel
        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult LoginPanel(Member user)
        //{
        //    Member member = accountInterface.Login(user);

        //    if (member != null)
        //    {
        //        MemberInfo = member;
        //    }
        //    else
        //    {
        //        TempData["LoginFailure"] = true;
        //    }
            

        //    return RedirectToAction("Index", "Event");
        //}

        public RedirectToRouteResult Logout()
        {
            MemberInfo = null;

            return RedirectToAction("Index", "Home");
        }

        #endregion

        [RequiresLogin()]
        public ViewResult ManageCharacters()
        {   
            AddGameModel addGameModel = new AddGameModel();
            addGameModel.member = MemberInfo; //accountInterface.GetMemberInfoByID(memberID);
            //Get all games we support
            addGameModel.gamesSupported = gameInterface.GetAllGames(true).ToList();
            //Get Characters for Member
            addGameModel.memberCharacters = gameInterface.GetMemberCharacters(addGameModel.member.MemberID).ToList();
            //See if user was attempting to access a page before they created characters
            addGameModel.PrevActionURL = Session[GlobalConst.SESSION_PREVIOUS_ACTION] as string;

            return View(addGameModel);
        }

        [NoCache]
        public ActionResult GetMembersGames()
        {
            var membersGames = accountInterface.GetMembersGames(MemberInfo.MemberID);
            IEnumerable<Game> memberGamesDisplay = from game in gameInterface.GetAllGames(null)
                                                   where membersGames.Any(memGame => memGame.GameID == game.GameID)
                                                   select game;

            return new JsonResult() { Data = memberGamesDisplay.ToArray() };
        }

        public ActionResult InsertNewMemberGameAJAX(int id)
        {
            CommitResponse success = accountInterface.InsertNewMemberGame(MemberInfo.MemberID, id);

            return new JsonResult() { Data = success };
        }

        [NoCache]
        public JsonResult InsertNewCharacterAJAX(int? gameID, string charName, int? classID, int? factionID, int? level)
        {
            //Do some Validation
            CommitResponse newCharResponse = new CommitResponse();

            if (gameID == null || charName == null || classID == null || factionID == null || level == null)
            {
                newCharResponse.success = false;
                newCharResponse.errorMsg = Errors.INVALID_CHARACTER_INFORMATION;
            }

            if (newCharResponse.success)
            {
                MemCharacter memChar = new MemCharacter()
                {
                    MemberID = MemberInfo.MemberID,
                    CharacterName = charName,
                    ClassID = (int)classID,
                    FactionID = factionID,
                    GameID = (int)gameID,
                    LVL = level
                };

                newCharResponse = charInterface.AddNewCharacter(memChar, new int[0]);
            }

            return new JsonResult() { Data = newCharResponse };
        }

        [NoCache]
        public ActionResult GetAllMemberCharactersByGameAJAX(int id)
        {
            CompleteCharacterData[] characters = charInterface.GetAllMemberCharactersByGame(MemberInfo.MemberID, id).ToArray();
            
            return new JsonResult() { Data = characters };
        }

        public ActionResult GetCharacterRoles(int charID)
        {
            MemCharacterRole[] roles = charInterface.GetMemCharRolesByCharacterID(charID).ToArray();

            return new JsonResult { Data = roles };
        }

        [RequiresLogin()]
        public ViewResult AddCharacter(int gameID)
        {
            //if (MemberInfo == null)
            //{
            //    ViewData["LoginRequired"] = "You must be logged in to add characters to your account. Please login.";
            //    return View("Login");
            //}

            AddCharacterModel model = GetAddCharacterModel(gameID);
            return View(model);
        }

        private AddCharacterModel GetAddCharacterModel(int gameID)
        {
            AddCharacterModel model = new AddCharacterModel();

            model.GameID = gameID;
            model.Classes = gameInterface.GetAllClassesByGame(gameID).ToList();
            model.Factions = gameInterface.GetAllFactionsByGameID(gameID).ToList();
            model.Roles = gameInterface.GetAllRolesByGameID(gameID).ToList();
            model.Servers = gameInterface.GetAllServersByGameID(gameID).ToList();
            model.Character = new MemCharacter();

            return model;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddCharacter(MemCharacter memChar)
        {
            //MemCharacter memChar = m.Character;
            //TODO: Shit loads MORE serverside validation
            bool valid = true;
            int outLvl = 0;
            valid = Int32.TryParse(memChar.LVL.ToString(), out outLvl);

            //Assert that the current member is editing charater data only for his account
            if (memChar.MemberID != 0 && MemberInfo.MemberID != memChar.MemberID)
            {
                valid = false;
                ModelState.AddModelError("MemberHoax", Errors.CAN_NOT_OPERATE_ON_OTHER_MEMBERS_DATA);
            }

            if (valid)
            {
                int[] roleIDs = (!String.IsNullOrEmpty(Request["RoleID"])) ? Request["RoleID"].Split(new char[] { ',' }).Select(roleID => Convert.ToInt32(roleID)).ToArray() : new int[0];
                // Add Character and Roles
                memChar.MemberID = MemberInfo.MemberID;
                CommitResponse newCharResponse = charInterface.AddNewCharacter(memChar, roleIDs);

                if (newCharResponse.success)
                {
                    TempData[GlobalConst.TEMPDATA_CONFIRMATION_MESSAGE] = memChar.CharacterName + " was successfully added to your account.";
                    return RedirectToAction("Confirmation", "Shared");
                }
                else
                {
                    ModelState.AddModelError("memberNotCreated", newCharResponse.errorMsg);
                }
            }

            AddCharacterModel model = GetAddCharacterModel(memChar.GameID);
            model.Character = memChar;
            //ModelState.AddModelError("memberNotCreated", "Member could not be created.");
            //return RedirectToAction("AddCharacter", new { gameID = memChar.GameID });
            return View(model);

        }

        [RequiresLogin()]
        public ViewResult EditCharacter(int id)
        {
            //if (MemberInfo == null)
            //{
            //    ViewData["LoginRequired"] = "You must be logged in to add characters to your account. Please login.";
            //    return View("Login");
            //}

            MemCharacter usersChar = charInterface.GetCharacterByID(id);
            EditCharacterModel model = new EditCharacterModel();
            if (usersChar != null)
            {
                model.CharInfo = usersChar;

                model.Classes = gameInterface.GetAllClassesByGame(usersChar.GameID).ToList();

                model.Factions = gameInterface.GetAllFactionsByGameID(usersChar.GameID).ToList();

                model.Roles = gameInterface.GetAllRolesByGameID(usersChar.GameID).ToList();

                model.Servers = gameInterface.GetAllServersByGameID(usersChar.GameID).ToList();

                return View(model);
            }
            else
            {
                ModelState.AddModelError("InvalidCharacter", "The Character Requested Does Not Exist");
                
            }

            return View(model);
            
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditCharacter(MemCharacter memChar)
        {
            //Assert that the current member is editing charater data only for his account
            bool valid = true;
            if (memChar.MemberID != 0 && MemberInfo.MemberID != memChar.MemberID)
            {
                valid = false;
                ModelState.AddModelError("MemberHoax", Errors.CAN_NOT_OPERATE_ON_OTHER_MEMBERS_DATA);
            }

            //Assert User Selected Some Roles
            int[] roleIDs = (!String.IsNullOrEmpty(Request["RoleID"])) ? Request["RoleID"].Split(new char[] { ',' }).Select(roleID => Convert.ToInt32(roleID)).ToArray() : new int[0];
            if (roleIDs.Length == 0)
            {
                valid = false;
                ModelState.AddModelError("NoRolesSelected", Errors.NO_ROLES_SELECTED);
            }

            memChar.MemberID = (memChar.MemberID != 0) ? memChar.MemberID : MemberInfo.MemberID;

            if (valid)
            {
                CommitResponse editCharResponse = charInterface.EditCharacter(memChar);
                if (editCharResponse.success)
                {
                    CommitResponse roleUpdate;
                    if (charInterface.ValidateUserRoles(memChar.ClassID, roleIDs))
                    {
                        charInterface.DeleteRolesForCharacter(memChar.CharacterID);
                        roleUpdate = charInterface.AddRolesForCharacter(memChar.CharacterID, roleIDs, memChar.ClassID);
                    }
                    else
                    {
                        roleUpdate = new CommitResponse() { success = false };
                        ModelState.AddModelError("RoleInvalid", Errors.ROLE_IS_NOT_VALID_FOR_CLASS);
                    }

                    if (roleUpdate.success)
                    {
                        TempData[GlobalConst.TEMPDATA_CONFIRMATION_MESSAGE] = "Character was successfully edited.";
                        return RedirectToAction("Confirmation", "Shared");
                    }
                }
                else
                {
                    ModelState.AddModelError("EditFailed", Errors.EDIT_CHARACTER_FAILED);
                }
            }

            //return RedirectToAction("EditCharacter", new { id = memChar.CharacterID });
            return View(memChar);
        }

        public JsonResult DeleteCharacterAJAX(int charID)
        {
            CommitResponse response = charInterface.DeleteCharacter(MemberInfo.MemberID, charID);

            return new JsonResult() { Data = response };
        }

        [RequiresLogin()]
        public ViewResult Settings()
        {
            AccountSettings settings = accountInterface.GetAccountSettings(MemberInfo.MemberID);
            
            return View(settings);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ViewResult Settings(AccountSettings model)
        {
            //Valid Email
            if(!String.IsNullOrEmpty(model.Email) && !Validate.Email(model.Email))
            {
                ValidationErrors.Add("BadEmail", "Invalid Email");
            }

            if (ValidationErrors.Count == 0)
            {
                accountInterface.UpdateAccountSettings(MemberInfo.MemberID, model);
                ViewData.Add("Success", "<p>Account information successfully changed</p>");
            }
            else
            {
                RegisterErrorsWithModel();
            }

            return View(model);
        }
    }
}


