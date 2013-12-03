using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeetRaids
{
    public static class Extensions
    {
        public static int? AsNullable(this int num)
        {
            return (num != 0) ? (Nullable<int>)num : null;
        }

        public static int? AsNullIfNegative(this int? num)
        {
            return (num >= 0) ? (Nullable<int>)num : null;
        }

        public static string AsNullIfEmpty(this string str)
        {
            return (String.IsNullOrEmpty(str)) ? null : str;
        }

        public static bool ToBoolOrDefault(this object input, bool defaultValue)
        {
            bool returnVal = false;
            if(input != null && Boolean.TryParse(input.ToString(), out returnVal))
            {
                //lol
            }
            else
            {
                returnVal = defaultValue;
            }

            return returnVal;
        }
    }
}
