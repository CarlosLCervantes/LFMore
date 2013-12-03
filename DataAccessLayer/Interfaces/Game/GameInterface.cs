using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace DataAccessLayer
{
    public class GameInterface
    {
        DataAccessLayer.LeetRaidsDBDataContext LeetRaidsDB;
        List<string> logicErrors = new List<string>();

        public GameInterface(string connectionString)
        {
            LeetRaidsDB = new DataAccessLayer.LeetRaidsDBDataContext(connectionString);
        }

        public GameInterface(LeetRaidsDBDataContext existingConnection)
        {
            LeetRaidsDB = existingConnection;
        }

        public IEnumerable<Game> GetAllGames(bool? active)
        {
            var allGames = from game in LeetRaidsDB.Games
                           where game.Enabled == (active ?? game.Enabled)
                           select game;

            return allGames;
        }

        public Game GetGameByID(int id)
        {
            Game game = (from gme in LeetRaidsDB.Games
                        where gme.GameID == id
                        select gme).SingleOrDefault();

            return game;
        }

        public bool AddGameToMember(int memberID, Game game)
        {
            MemGame memberToGameRelation = new MemGame() { MemberID = memberID, GameID = game.GameID };
            try
            {
                LeetRaidsDB.MemGames.InsertOnSubmit(memberToGameRelation);
                LeetRaidsDB.SubmitChanges();
            }
            catch (Exception ex)
            {
                if (!Convert.ToBoolean(ConfigurationManager.AppSettings["RETHROW_HANDLED_ERRORS"]))
                {
                    return false;
                }
                else
                {
                    throw ex;
                }
            }

            return true;
        }

        //It's possible that this doesn't belong in this interface, and an account interface should be made. I will decide later.
        public IEnumerable<MemCharacter> GetMemberCharacters(int memberID)
        {
            var memberCharacters = from character in LeetRaidsDB.MemCharacters
                                   where character.MemberID == memberID
                                   select character;

            return memberCharacters;
        }

        public IEnumerable<Class> GetAllClassesByGame(int gameID)
        {
            var classes = from clss in LeetRaidsDB.Classes
                          where clss.GameID == gameID
                          select clss;

            return classes;
        }

        public IEnumerable<Role> GetAllRolesByGameID(int gameID)
        {
            var roles = from role in LeetRaidsDB.Roles
                          where role.GameID == gameID
                          select role;

            return roles;
        }

        public IEnumerable<Faction> GetAllFactionsByGameID(int gameID)
        {
            var factions = from faction in LeetRaidsDB.Factions
                           where faction.GameID == gameID
                           select faction;

            return factions;
        }

        public IEnumerable<Server> GetAllServersByGameID(int gameID)
        {
            var servers = from server in LeetRaidsDB.Servers
                          where server.GameID == gameID
                          select server;

            return servers;
        }

        public Server GetServer(int serverID)
        {
            Server server = (from svr in LeetRaidsDB.Servers
                             where svr.ServerID == serverID
                             select svr).SingleOrDefault();

            return server;
        }

        public Class GetClassByID(int classID)
        {
            Class clss = (from c in LeetRaidsDB.Classes
                          where c.ClassID == classID
                          select c).SingleOrDefault();

            return clss;
        }

    }
}
