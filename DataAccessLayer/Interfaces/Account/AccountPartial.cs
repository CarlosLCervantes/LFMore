using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer
{
    public partial class MemFriend
    {
        public bool Restricted { get; set; }
        public string Reason { get; set; }
        public bool HighlightOnList { get; set; }
        public CompleteCharacterData CompleteCharData { get; set; }

    }
}
