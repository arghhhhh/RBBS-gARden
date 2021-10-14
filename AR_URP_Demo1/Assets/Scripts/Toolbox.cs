using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Joss.Helpers
{
    public class Toolbox
    {
        public static string RemoveLineEndings(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            string lineSeparator = ((char)0x2028).ToString();
            string paragraphSeparator = ((char)0x2029).ToString();

            return value.Replace("\r\n", string.Empty)
                        .Replace("\n", string.Empty)
                        .Replace("\r", string.Empty)
                        .Replace(lineSeparator, string.Empty)
                        .Replace(paragraphSeparator, string.Empty);
        }

        public static GameObject GetAncestor(GameObject item)
        {
            if (item.transform.parent == null)
                return item;
            return (GetAncestor(item.transform.parent.gameObject));
        }
    }
}
