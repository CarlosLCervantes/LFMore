using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccessLayer;

namespace LeetRaids.Models
{
    public class FriendList
    {
        public int CreatorsMemberID { get; set; }
        public List<MemFriend> Friends { get; set; }
    }
}
