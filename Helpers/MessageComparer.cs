using System.Linq;
using System.Collections.Generic;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class MessageComparer : IComparer<string>
    {
        private static List<string> coll = new List<string>() { "Create", "Update", "Delete", "SetState", "SetStateDynamicEntity", "Merge" };

        public int Compare(string message1, string message2)
        {
            if (string.IsNullOrEmpty(message1))
            {
                return -1;
            }

            if (string.IsNullOrEmpty(message2))
            {
                return 1;
            }

            if (coll.Contains(message1, StringComparer.InvariantCultureIgnoreCase)
                && coll.Contains(message2, StringComparer.InvariantCultureIgnoreCase)
                )
            {
                var item1 = coll.First(i => StringComparer.InvariantCultureIgnoreCase.Equals(i, message1));
                var item2 = coll.First(i => StringComparer.InvariantCultureIgnoreCase.Equals(i, message2));

                int index1 = coll.IndexOf(item1);
                int index2 = coll.IndexOf(item2);

                return index1.CompareTo(index2);
            }

            if (coll.Contains(message1, StringComparer.InvariantCultureIgnoreCase))
            {
                return -1;
            }

            if (coll.Contains(message2, StringComparer.InvariantCultureIgnoreCase))
            {
                return 1;
            }

            return string.Compare(message1, message2);
        }
    }
}