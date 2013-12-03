using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer
{
    public static class Extensions
    {

        public static string SubStrMax(this string input, int length)
        {
            if (input.Length > length)
            {
                input = input.Substring(0, length);
            }

            return input;
        }

        public static int ToIntOrDefault(this int? input, int defaultVal)
        {
            int returnVal = defaultVal;
            if (input != null)
            {
                returnVal = (int)input;
            }

            return returnVal;
        }
    }
}
