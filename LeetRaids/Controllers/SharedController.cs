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

    public class SharedController : BaseController
    {
        GameInterface gameInterface;
        EventInterface eventInterface;
        CharacterInterface charInterface;
        RestrictionsInterface restrictionsInterface;
        //AccountInterface accountInterface;

        public SharedController(GameInterface iGame, CharacterInterface iChar, RestrictionsInterface iRest, EventInterface iEvent)
        {
            gameInterface = iGame;
            charInterface = iChar;
            restrictionsInterface = iRest;
            eventInterface = iEvent;
        }

        public ViewResult GameList()
        {
            List<Game> games = gameInterface.GetAllGames(true).ToList();

            return View(games);
        }

        public ViewResult Confirmation()
        {
            return View();
        }

        public ViewResult RoleRestrictions()
        {
            return View();
        }

        public ViewResult CharacterList()
        {
            return View();
        }

        #region SearchCharacters

        public ViewResult SearchCharacters(int? serverID, int? factionID, int? evtRest)
        {
            bool enforceEventRestrictions = (evtRest.GetValueOrDefault(0) == 1);

            //Pass serverID or factionID to lock to that specific factionID
            SearchCharactersModel model = new SearchCharactersModel();
            model.Classes = (from clss in gameInterface.GetAllClassesByGame((int)CurrentEvent.GameID)
                             select clss).ToList();
            model.Roles = (from role in gameInterface.GetAllRolesByGameID((int)CurrentEvent.GameID)
                           select role).ToList();
            model.Servers = (from server in gameInterface.GetAllServersByGameID((int)CurrentEvent.GameID)
                             where server.ServerID == (serverID ?? server.ServerID)
                             select server).ToList();
            model.Factions = (from faction in gameInterface.GetAllFactionsByGameID(CurrentEvent.GameID)
                              where faction.FactionID == (factionID ?? faction.FactionID)
                              select faction).ToList();
            model.EnforceEventRestrictions = enforceEventRestrictions;

            if (serverID != null || factionID != null)
            {
                ViewData["FilterDisclaimer"] = "Currently some filters are locked to restraints";
            }


            return View(model);
        }

        [NoCache()]
        public JsonResult SearchCharactersAJAX(string charName, int? classID, int? roleID, int? serverID, int? minLvl, int? maxLvl, int? evtRest)
        {
            bool enforceEventRestrictions = (evtRest.GetValueOrDefault(0) == 1);

            IEnumerable<SearchCharacterResult> characters = charInterface.SearchCharacters((int)CurrentEvent.GameID,
                                                    charName.AsNullIfEmpty(),
                                                    classID.AsNullIfNegative(),
                                                    roleID.AsNullIfNegative(),
                                                    null,
                                                    serverID.AsNullIfNegative(),
                                                    minLvl.AsNullIfNegative(),
                                                    maxLvl.AsNullIfNegative()
                                                    );

            //IF event restrictions are being re-inforced then go ahead and add them
            if (enforceEventRestrictions)
            {
                characters = restrictionsInterface.AddEventRestrictionsToCharacterInfo(characters.ToList(), CurrentEvent);
            }

            //Highlight characters in demand
            foreach (SearchCharacterResult character in characters)
            {
                if (!character.Restriction.Restricted) //For right now let's just combine restrictions and "in demand". They should really be two different values though.
                {
                    character.Restriction.Restricted = !eventInterface.IsCharacterInDemandForEvent(CurrentEvent.EventID, character.CharacterInfo.CharacterID);
                    if (character.Restriction.Restricted) { character.Restriction.Restricted_Reason = "Character is of a Role/Class that is needed"; }
                }
            }


            return new JsonResult() { Data = characters.ToArray() };
        }

        public ViewResult UpdateSearchCharacters(int id)
        {

            return View("SearchCharacters");
        }

        #endregion

        public RedirectToRouteResult ReturnPrevious()
        {

            GlobalMethod.RedirectBack(); //Redirect back using our Response.Redrect method
                
            // If something goes wrong just redirect to home
            return RedirectToAction("Index", "Home");
        }

        public RedirectToRouteResult ReturnHome()
        {
            return RedirectToAction("Index", "Home");
        }

    }
}
