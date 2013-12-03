using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Linq.Dynamic;

namespace DataAccessLayer
{
    public class CharacterInterface
    {
        DataAccessLayer.LeetRaidsDBDataContext LeetRaidsDB;

        public CharacterInterface(string connectionString)
        {
            LeetRaidsDB = new DataAccessLayer.LeetRaidsDBDataContext(connectionString);
        }

        public CharacterInterface(DataAccessLayer.LeetRaidsDBDataContext existingConnection)
        {
            LeetRaidsDB = existingConnection;
        } 

        public CommitResponse AddNewCharacter(MemCharacter newMemChar, int[] roleIDs)
        {
            CommitResponse response = new CommitResponse();
            //Validate CharData?

            //Validate Role Data
            if (!ValidateUserRoles(newMemChar.ClassID, roleIDs))
            {
                response.success = false;
                response.errorMsg = Errors.ROLE_IS_NOT_VALID_FOR_CLASS;
            }

            if (response.success)
            {
                newMemChar.CreateDT = DateTime.Now;
                newMemChar.LastModifiedDT = DateTime.Now;

                try
                {
                    // Insert Member Data
                    LeetRaidsDB.MemCharacters.InsertOnSubmit(newMemChar);
                    LeetRaidsDB.SubmitChanges();
                    response.ReturnData = newMemChar;
                    // Insert RoleData
                    CommitResponse rolesResponse = AddRolesForCharacter(newMemChar.CharacterID, roleIDs, newMemChar.ClassID);
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.errorMsg = Errors.INSERT_CHARACTER_FAILED;
                    if (!Convert.ToBoolean(ConfigurationManager.AppSettings["RETHROW_HANDLED_ERRORS"]))
                    {

                    }
                    else
                    { throw ex; }
                }
            }

            return response;
        }

        public CommitResponse EditCharacter(MemCharacter memChar)
        {
            CommitResponse response = new CommitResponse();
            //Make sure the User is only editing a character that he owns already.
            if (!CharacterBelongsToMember(memChar.MemberID, memChar.CharacterID))
            {
                response.success = false;
                response.errorMsg = Errors.CAN_NOT_OPERATE_ON_OTHER_MEMBERS_CHARACTER;
            }

            if (response.success)
            {
                memChar.LastModifiedDT = memChar.LastModifiedDT = DateTime.Now;
                MemCharacter editMemChar = (from eMemChar in LeetRaidsDB.MemCharacters
                                            where eMemChar.CharacterID == memChar.CharacterID
                                            select eMemChar).Single();

                editMemChar.CharacterName = memChar.CharacterName;
                editMemChar.ClassID = memChar.ClassID;
                editMemChar.FactionID = memChar.FactionID;
                //editMemChar.GameID = memChar.GameID;
                editMemChar.LVL = memChar.LVL;
                //editMemChar.RegionID = memChar.RegionID;
                editMemChar.ServerID = memChar.ServerID;
                //How the fuck to handle roles
                
                LeetRaidsDB.SubmitChanges();
            }

            return response;
        }

        public bool CharacterBelongsToMember(int memberID, int charID)
        {
            return GetAllMemberCharacters(memberID).Any(existingMemChar => existingMemChar.CharacterID == charID);
        }

        public CommitResponse DeleteCharacter(int currentMemberID, int charID)
        {
            CommitResponse response = new CommitResponse();
            //Ensure that character belongs to use
            if (!CharacterBelongsToMember(currentMemberID, charID))
            {
                response.success = false;
                response.errorMsg = Errors.CAN_NOT_OPERATE_ON_OTHER_MEMBERS_CHARACTER;
                return response;
            }

            MemCharacter charToDelete = GetCharacterByID(charID);

            if (charToDelete != null)
            {
                LeetRaidsDB.MemCharacters.DeleteOnSubmit(charToDelete);
                LeetRaidsDB.SubmitChanges();

                DeleteRolesForCharacter(charID);
            }
            else
            {
                response.success = false;
                response.errorMsg = Errors.CHARACTER_NOT_FOUND;
            }

            return response;
        }

        public CommitResponse AddRolesForCharacter(int charID, int[] roleIDs, int classID)
        {
            CommitResponse response = new CommitResponse();

            classID = (classID != null) ? classID : GetCharacterByID(charID).ClassID;
            if (!ValidateUserRoles((int)classID, roleIDs))
            {
                response.success = false;
                response.errorMsg = Errors.ROLE_IS_NOT_VALID_FOR_CLASS;
                return response;
            }

            foreach (int roleID in roleIDs)
            {
                MemCharacterRole memRole = new MemCharacterRole()
                {
                    CharacterID = charID,
                    LastModified = DateTime.Now,
                    RoleID = roleID

                };
                LeetRaidsDB.MemCharacterRoles.InsertOnSubmit(memRole);
            }

            try
            {
                LeetRaidsDB.SubmitChanges();
                response.success = true;
            }
            catch (Exception ex)
            {
                response.success = false;
            }

            return response;
        }

        public bool DeleteRolesForCharacter(int charID)
        {
            bool success = false;
            IEnumerable<MemCharacterRole> roles = from role in LeetRaidsDB.MemCharacterRoles
                                                  where role.CharacterID == charID
                                                  select role;

            try
            {
                LeetRaidsDB.MemCharacterRoles.DeleteAllOnSubmit(roles);
                LeetRaidsDB.SubmitChanges();
                success = true;
            }
            catch(Exception ex)
            {

            }

            return success;
        }

        public bool ValidateUserRoles(int classID, int[] roleIDs)
        {
            bool success = true;
            var roleIDsForClass = from classRole in LeetRaidsDB.ClassRoles
                                  where classRole.ClassID == classID
                                  select classRole.RoleID;

            foreach (int roleID in roleIDs)
            {
                if (!roleIDsForClass.Contains(roleID))
                {
                    success = false;
                    break;
                }
            }

            return success;
        }

        public IEnumerable<MemCharacter> GetAllMemberCharacters(int memberID)
        {
            var memChars = from memChar in LeetRaidsDB.MemCharacters
                           where memChar.MemberID == memberID
                           select memChar;

            return memChars;
        }

        public IEnumerable<CompleteCharacterData> GetAllMemberCharactersByGame(int memberID, int gameID)
        {
            var characters = from character in LeetRaidsDB.MemCharacters
                             join clss in LeetRaidsDB.Classes on character.ClassID equals clss.ClassID into classJoin
                             join faction in LeetRaidsDB.Factions on character.FactionID equals faction.FactionID into factionJoin
                             join server in LeetRaidsDB.Servers on character.ServerID equals server.ServerID into serverJoin
  
                             where character.MemberID == memberID && character.GameID == gameID
                             select new CompleteCharacterData()
                             {
                                 CharacterID = character.CharacterID,
                                 CharacterName = character.CharacterName,
                                 ClassName = classJoin.SingleOrDefault().ClassName,
                                 ImageLocation = classJoin.SingleOrDefault().ImageLocation,
                                 ServerID = (int)character.ServerID,
                                 ServerName = serverJoin.SingleOrDefault().Name,
                                 FactionName = factionJoin.SingleOrDefault().FactionName,
                                 Roles = GetRolesByCharacterID(character.CharacterID).ToArray()
                             };

            return characters;
        }

        public IEnumerable<MemCharacter> GetAllCharactersByGameID(int gameID)
        {
            var characters = from game in LeetRaidsDB.MemCharacters
                             where game.GameID == gameID
                             select game;

            return characters;
        }

        public CompleteCharacterData GetCompleteCharacterByID(int charID)
        {
            var characterInfo = from character in LeetRaidsDB.MemCharacters
                                join clss in LeetRaidsDB.Classes on character.ClassID equals clss.ClassID into classJoin
                                join faction in LeetRaidsDB.Factions on character.FactionID equals faction.FactionID into factionJoin
                                join server in LeetRaidsDB.Servers on character.ServerID equals server.ServerID into serverJoin
                                where character.CharacterID == charID
                                select new CompleteCharacterData()
                                {
                                    CharacterID = character.CharacterID,
                                    CharacterName = character.CharacterName,
                                    GameID = character.GameID,
                                    ServerID = character.ServerID.Value,
                                    ImageLocation = classJoin.SingleOrDefault().ImageLocation,
                                    ClassName = classJoin.SingleOrDefault().ClassName,
                                    ServerName = serverJoin.SingleOrDefault().Name,
                                    FactionName = factionJoin.SingleOrDefault().FactionName,
                                    LVL = (int)character.LVL,
                                    Roles = GetRolesByCharacterID(charID).ToArray()
                                };

            return characterInfo.SingleOrDefault();
        }

        public MemCharacter GetCharacterByID(int charID)
        {
            var characterInfo = from character in LeetRaidsDB.MemCharacters
                                where character.CharacterID == charID
                                select character;

            return characterInfo.SingleOrDefault();
        }

        public IEnumerable<MemCharacterRole> GetMemCharRolesByCharacterID(int charID)
        {
            var roles = from role in LeetRaidsDB.MemCharacterRoles
                        where role.CharacterID == charID
                        select role;

            return roles;
        }

        public IEnumerable<Role> GetRolesByCharacterID(int charID)
        {
            var roles = from memRole in LeetRaidsDB.MemCharacterRoles
                        join role in LeetRaidsDB.Roles on memRole.RoleID equals role.RoleID
                        where memRole.CharacterID == charID
                        select role;

            return roles;
        }

        public IEnumerable<Role> GetRolesForGame(int gameID)
        {
            var roles = from role in LeetRaidsDB.Roles
                        where role.GameID == gameID
                        select role;

            return roles;
        }

        public Role GetRoleByID(int roleID)
        {
            Role role = (from r in LeetRaidsDB.Roles
                        where r.RoleID == roleID
                        select r).SingleOrDefault();

            return role;
        }

        public IEnumerable<SearchCharacterResult> SearchCharacters(int gameID, string name, int? classID, int? roleID, int? factionID, int? serverID, int? levelMin, int? levelMax)
        {
            var characters = from charData in LeetRaidsDB.SearchCharacters(gameID, name, classID, roleID, factionID, serverID, levelMin, levelMax)
                             select new SearchCharacterResult()
                            {
                                CharacterInfo = new CompleteCharacterData()
                                {
                                    CharacterID = charData.CharacterID,
                                    CharacterName = charData.CharacterName,
                                    GameID = charData.GameID,
                                    ClassName = charData.ClassName,
                                    ServerName = charData.Name,
                                    FactionName = charData.FactionName,
                                    ServerID = charData.ServerID.Value,
                                    ImageLocation = charData.ImageLocation,
                                    LVL = charData.LVL.Value,
                                    Roles = GetRolesByCharacterID(charData.CharacterID).ToArray() //this could be improved by putting the lookup to MemCharRoles in the SP.
                                },
                                Restriction = new RestrictionReason().DefaultToUnrestricted()
                            };




            #region fail Linq query
            /* I tried to make a dynamic where clause to support the search feature but it couldnt easilt be done with all the joins.
             * Also I think it would be better to do the search in the database beacuse at some point there will be alot of characters. 
             * This will probably be the largest datasource in the app.
             * var characters = from ch in LeetRaidsDB.MemCharacters
                             join clss in LeetRaidsDB.Classes on ch.ClassID equals clss.ClassID
                             join memRole in LeetRaidsDB.MemCharacterRoles on ch.CharacterID equals memRole.CharacterID
                             join faction in LeetRaidsDB.Factions on ch.FactionID equals faction.FactionID
                             join server in LeetRaidsDB.Servers on ch.ServerID equals server.ServerID
                             where ch.GameID == gameID && ch.CharacterName == name && ch.ClassID == classID
                                    && memRole.RoleID == roleID && ch.ServerID == serverID 
                                    && (ch.LVL >= levelMin && ch.LVL <= levelMax)
                             select new CompleteCharacterData()
                             {
                                 CharacterID = ch.CharacterID,
                                 CharacterName = ch.CharacterName,
                                 ClassName = clss.ClassName,
                                 ServerName = server.Name,
                                 FactionName = faction.FactionName,
                             };
             */
            #endregion

            return characters;
        }
    }


    public class CompleteCharacterData
    {
        public int CharacterID { get; set; }
        public string CharacterName { get; set; }
        public string ClassName { get; set; }
        public string ImageLocation { get; set; }
        public int ServerID { get; set; }
        public string ServerName { get; set; }
        public string FactionName { get; set; }
        public int GameID { get; set; }
        public int LVL { get; set; }
        public Role[] Roles { get; set; }

        public CompleteCharacterData()
        {

        }
    }

    public class SearchCharacterResult
    {
        public bool InDemandForEvent { get; set; }
        public CompleteCharacterData CharacterInfo { get; set; }
        public RestrictionReason Restriction { get; set; }
    }

    
}
