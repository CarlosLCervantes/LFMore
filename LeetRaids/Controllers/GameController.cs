using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using DataAccessLayer;

namespace LeetRaids.Controllers
{
    public class GameController : BaseController
    {
        GameInterface gameInterface;

        public GameController(GameInterface currentGameInterface)
        {
            gameInterface = currentGameInterface;
        }

        public ActionResult Index()
        {
            return View();
        }

        //TODO: Remove this
        [NoCache]
        public ActionResult GetGameDetails(int id)
        {
            Game gameInfo = gameInterface.GetGameByID(id);

            return new JsonResult { Data = gameInfo };
        }

        [NoCache]
        public ActionResult GetAllClassesByGameID(int id)
        {
            Class[] classes = gameInterface.GetAllClassesByGame(id).ToArray();
            
            return new JsonResult() { Data = classes };
        }

        [NoCache]
        public ActionResult GetAllRolesByGameID(int id)
        {
            Role[] roles = gameInterface.GetAllRolesByGameID(id).ToArray();
            
            return new JsonResult() { Data = roles };
        }

        public ActionResult GetAllFactionsByGameID(int id)
        {
            Faction[] factions = gameInterface.GetAllFactionsByGameID(id).ToArray();

            return new JsonResult() { Data = factions };
        }

        public ActionResult GetAllServersByGameIDAJAX(int id)
        {
            Server[] servers = gameInterface.GetAllServersByGameID(id).ToArray();
            return new JsonResult() { Data = servers };
        }
    }
}
