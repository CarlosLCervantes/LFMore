using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace DataAccessLayer
{
    public class AccountInterface
    {
        DataAccessLayer.LeetRaidsDBDataContext LeetRaidsDB;
        List<string> logicErrors = new List<string>();

        public AccountInterface(string connectionString)
        {
            LeetRaidsDB = new DataAccessLayer.LeetRaidsDBDataContext(connectionString);
        }

        public AccountInterface(DataAccessLayer.LeetRaidsDBDataContext existingConnection)
        {
            LeetRaidsDB = existingConnection;
        }

        public IEnumerable<Member> GetMembers()
        {
            IEnumerable<Member> members = from member in LeetRaidsDB.Members
                                          select member;

            return members;
        }

        public CommitResponse InsertMember(Member member)
        {
            CommitResponse response = new CommitResponse();
            var uniqueUserName = !LeetRaidsDB.Members.Any(mem => mem.UserName == member.UserName);
            if (!uniqueUserName)
            {
                response.Errors.Add(Errors.USERNAME_TAKEN);
            }
            if (member.UserName.Length < 5 || member.UserName.Length > 15)
            {
                response.Errors.Add(Errors.CHECK_USER_NAME);
            }
            if (!member.AgreedToTerms)
            {
                response.Errors.Add(Errors.MUST_AGREE_TO_TERMS);
            }
            if (member.Password.Length < 5 || member.Password.Length > 14)
            {
                response.Errors.Add(Errors.CHECK_PASSWORD);
            }
            if(!Validate.Email(member.Email))
            {
                response.Errors.Add(Errors.INVALID_EMAIL);
            }

            response.success = (response.Errors.Count < 1);

            if (response.success)
            {
                try
                {
                    LeetRaidsDB.Members.InsertOnSubmit(member);
                    LeetRaidsDB.SubmitChanges();
                }
                catch (Exception ex)
                {

                    if (!Convert.ToBoolean(ConfigurationManager.AppSettings["RETHROW_HANDLED_ERRORS"]))
                    {
                        response.success = false;
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }

            return response;
        }

        public Member Login(Member unauthenticatedUser)
        {
            //throw new Exception("This is the connection string" + LeetRaidsDB.Connection.ConnectionString);

            Member member = (from mem in LeetRaidsDB.Members
                             where mem.UserName == unauthenticatedUser.UserName && mem.Password == unauthenticatedUser.Password
                             select mem).SingleOrDefault();

            return member;
        }

        public Member GetMemberInfoByID(int memberID)
        {
            Member member = (from mem in LeetRaidsDB.Members
                             where mem.MemberID == memberID
                             select mem).Single();

            return member;
        }

        public IEnumerable<MemGame> GetMembersGames(int memberID)
        {
            var memGames = from game in LeetRaidsDB.MemGames
                           where game.MemberID == memberID
                           select game;

            return memGames;
        }

        public IEnumerable<Game> GetMembersGamesAsGames(int memberID)
        {
            var memGames = from game in LeetRaidsDB.Games
                           join memGame in LeetRaidsDB.MemGames on game.GameID equals memGame.GameID
                           where memGame.MemberID == memberID
                           select game;

            return memGames;
        }

        public CommitResponse InsertNewMemberGame(int memberID, int gameID)
        {
            CommitResponse response = new CommitResponse();

            // Check if user already has this game added
            if(GetMembersGames(memberID).Any(game => game.GameID == gameID))
            {
                response.success = false;
                response.errorMsg = Errors.MEMBER_ALREADY_ADDED_THIS_GAME;
            }

            if (response.success)
            {
                MemGame newMemGame = new MemGame() { GameID = gameID, MemberID = memberID };
                try
                {
                    LeetRaidsDB.MemGames.InsertOnSubmit(newMemGame);
                    LeetRaidsDB.SubmitChanges();
                }
                catch (Exception ex)
                {
                    response.success = false;
                    throw ex;
                    //if (!Convert.ToBoolean(ConfigurationManager.AppSettings["RETHROW_HANDLED_ERRORS"]))
                    //{

                    //}
                    //else
                    //{
                    //    throw ex;
                    //}
                }
            }

            return response;
        }

        public Member GetMemberByCharacterID(int characterID)
        {
            Member member = (from mem in LeetRaidsDB.Members
                            join memChar in LeetRaidsDB.MemCharacters on mem.MemberID equals memChar.MemberID
                            where memChar.CharacterID == characterID
                            select mem).SingleOrDefault();

            return member;
        }

        public AccountSettings GetAccountSettings(int memberID)
        {
            AccountSettings settings = new AccountSettings();

            Member mem = GetMemberInfoByID(memberID);
            settings.Email = mem.Email;

            return settings;
        }

        public CommitResponse UpdateAccountSettings(int memberID, AccountSettings settings)
        {
            CommitResponse response = new CommitResponse();
            Member memberInfo = GetMemberInfoByID(memberID);
            memberInfo.Email = settings.Email;

            LeetRaidsDB.SubmitChanges();

            return response;
        }

        public List<MemFriend> GetFriendsForMemberWithEventRestrictions(int memberID, Event eventInfo)
        {
            CharacterInterface charInterface = new CharacterInterface(LeetRaidsDB);
            //EventInterface eventInterface = new EventInterface(LeetRaidsDB);

            List<MemFriend> friends = (from friend in LeetRaidsDB.MemFriends
                                                           where friend.MemberID == memberID
                                                           select friend).ToList();

            //Add in complete data
            foreach (MemFriend f in friends)
            {
                f.CompleteCharData = charInterface.GetCompleteCharacterByID(f.FriendCharacterID);
            }

            RestrictionsInterface restrictionInterface = new RestrictionsInterface(LeetRaidsDB);
            foreach (MemFriend friend in friends)
            {
                RestrictionReason restriction = restrictionInterface.EnforceEventRestrictionOnCharacter(eventInfo, friend.CompleteCharData);

                friend.Restricted = restriction.Restricted;
                friend.Reason = restriction.Restricted_Reason;
            }
                                       

            #region Old Filter which just removes user, doesn't say why

                                                           //select new MemFriendWithEventRestriction()
                                                           //{
                                                           //    MemberID = friend.MemberID,
                                                           //    AddDateTime = friend.AddDateTime,
                                                           //    FriendCharacterID = friend.FriendCharacterID,
                                                           //    HighlightOnList = false,
                                                           //    MemFriendsID = friend.MemFriendsID,
                                                           //    Note = friend.Note,
                                                           //}).ToList();
            //if (charFilter == null) { charFilter = new int[0]; }

            //List<MemFriend> friends = (from friend in LeetRaidsDB.MemFriends
            //                           join friendChar in LeetRaidsDB.MemCharacters on friend.FriendCharacterID equals friendChar.CharacterID
            //                           where friend.MemberID == memberID 
            //                           && friendChar.GameID == (gameIDFilter ?? friendChar.GameID)
            //                           && friendChar.ServerID == (serverIDFilter ?? friendChar.ServerID)
            //                           && !charFilter.Contains(friend.FriendCharacterID)
            //                           select friend).ToList();
            #endregion
            
            return friends;
        }

        public CommitResponse CaptureEmailInvite(int inviterMemberID, string inviteeEmail, int inviteEventID)
        {
            CommitResponse response = new CommitResponse();

            Invite invite = new Invite()
            {
                InviteType = (int)InviteTypes.EMAIL,
                InviterMemberID = inviterMemberID,
                InviteDT = DateTime.Now,
                InviteeEmailAddress = inviteeEmail,
                EventID = inviteEventID
            };

            try
            {
                LeetRaidsDB.Invites.InsertOnSubmit(invite);
                LeetRaidsDB.SubmitChanges();
            }
            catch (Exception ex)
            {
                response.success = false;
            }

            return response;


        }

    }

    public enum InviteTypes { EMAIL = 1 }

    public class AccountSettings
    {
        public string Email { get; set; }
        public string PlayTimeStart { get; set; }
        public string PlayTimeEnd { get; set; }
    }




}
