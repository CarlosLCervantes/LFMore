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
    public class CharacterController : BaseController
    {
        CharacterInterface characterInterface;
        GameInterface gameInterface;
        EventInterface eventInterface;
        public CharacterController(CharacterInterface iChar, GameInterface iGame, EventInterface iEvent)
        {
            characterInterface = iChar;
            gameInterface = iGame;
            eventInterface = iEvent;
        }

        public ActionResult Index()
        {
            return View();
        }

        /*public ViewResult SearchCharacters(MemCharacter memChar)
        {

            int? roleID = null;
            
            List<CompleteCharacterData> characters = characterInterface.SearchCharacters((int)CurrentEvent.GameID, 
                memChar.CharacterName,
                memChar.ClassID.AsNullable(),
                roleID,
                memChar.ServerID,
                memChar.LVL,
                memChar.LVL
                ).ToList();



            return View("SearchCharacters", characters);
        }*/

    }
}
