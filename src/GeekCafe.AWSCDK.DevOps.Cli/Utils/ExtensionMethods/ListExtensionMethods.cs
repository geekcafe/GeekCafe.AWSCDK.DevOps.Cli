using System;
using System.Collections.Generic;

namespace GeekCafe.AWSCDK.DevOps.Cli.Utils.ExtensionMethods
{
    public static class ListExtensionMethods
    {

        /// <summary>
        /// Takes a list collection and turns it into a string separated by a delimitor
        /// </summary>
        /// <param name="list">The list to display</param>
        /// <param name="separator">The separator or delimitor. Defaults to a comma</param>
        /// <returns></returns>
        public static string ToDisplayString(this IList<string> list, string separator = ",")
        {
            if (list == null) return "";

            return string.Join(separator, list);
        }
    }
}
