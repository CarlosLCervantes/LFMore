using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer
{
    public class RestrictionsInterface
    {
        protected DataAccessLayer.LeetRaidsDBDataContext LeetRaidsDB;

        public RestrictionsInterface(string connectionString)
        {
            LeetRaidsDB = new DataAccessLayer.LeetRaidsDBDataContext(connectionString);
        }

        public RestrictionsInterface(DataAccessLayer.LeetRaidsDBDataContext existingConnection)
        {
            LeetRaidsDB = existingConnection;
        }

        public RestrictionReason EnforceEventRestrictionOnCharacter(Event eventInfo, CompleteCharacterData charInfo)
        {
            RestrictionReason restriction = new RestrictionReason().DefaultToUnrestricted();
            //Enforce Game Restriction
            if (charInfo.GameID != eventInfo.GameID)
            {
                restriction.Restricted = true;
                restriction.Restricted_Reason = RestrictionReason.REASON_DOES_NOT_BELONG_TO_GAME;
            }

            //Enforce Game Restriction

            if (charInfo.ServerID != eventInfo.ServerID)
            {
                restriction.Restricted = true;
                restriction.Restricted_Reason = RestrictionReason.REASON_DOES_NOT_EXIST_ON_SERVER;
            }

            //Enforce Attendence Restriction
            EventInterface eventInterface = new EventInterface(LeetRaidsDB);
            IEnumerable<EventAttendee> evtAttendees = eventInterface.GetAllEventAttendees(eventInfo.EventID);
            if (evtAttendees.Any(attn => attn.CharacterID == charInfo.CharacterID))
            {
                restriction.Restricted = true;
                restriction.Restricted_Reason = RestrictionReason.REASON_ALREADY_INVITED;
            }

            return restriction;
        }

        public List<SearchCharacterResult> SearchCharactersWithEventRestricitons(int gameID, string name, int? classID, int? roleID, int? factionID, int? serverID, int? levelMin, int? levelMax, Event eventInfo)
        {
            CharacterInterface charInterface = new CharacterInterface(LeetRaidsDB);
            List<SearchCharacterResult> characters = charInterface.SearchCharacters(gameID, name, classID, roleID, factionID, serverID, levelMin, levelMax).ToList();

            return AddEventRestrictionsToCharacterInfo(characters, eventInfo);
        }

        public List<SearchCharacterResult> AddEventRestrictionsToCharacterInfo(List<SearchCharacterResult> characters, Event eventInfo)
        {
            RestrictionsInterface restrictionsInterface = new RestrictionsInterface(LeetRaidsDB);
            foreach (SearchCharacterResult c in characters)
            {
                c.Restriction = restrictionsInterface.EnforceEventRestrictionOnCharacter(eventInfo, c.CharacterInfo);
            }

            return characters;
        }


    }


    public class RestrictionReason
    {
        public bool Restricted { get; set; }
        public string Restricted_Reason { get; set; }

        public RestrictionReason DefaultToUnrestricted()
        {
            this.Restricted = false;
            this.Restricted_Reason = String.Empty;
            return this;
        }


        //Restriction Reaons
        public static string REASON_DOES_NOT_BELONG_TO_GAME = "Doesn't belong to the right game";
        public static string REASON_DOES_NOT_EXIST_ON_SERVER = "Doesn't belong to the right server";
        public static string REASON_ALREADY_INVITED = "This user is already invite";
    }
}
