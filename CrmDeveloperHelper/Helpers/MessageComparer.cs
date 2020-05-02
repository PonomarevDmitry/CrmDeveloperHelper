using System;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class MessageComparer : IComparer<string>
    {
        private MessageComparer()
        {

        }

        private static Dictionary<string, int> coll = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase)
        {
            {  "Create", 0 }

            , {  "Update", 1 }

            , {  "Delete", 2 }

            , {  "SetState", 3 }

            , {  "SetStateDynamicEntity", 4 }

            , {  "Merge", 5 }

            //, {  "22222222222", 6 }
        };

        public static MessageComparer Comparer { get; private set; }

        static MessageComparer()
        {
            Comparer = new MessageComparer();
        }

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

            if (coll.ContainsKey(message1)
                && coll.ContainsKey(message2)
            )
            {
                int index1 = coll[message1];
                int index2 = coll[message2];

                return index1.CompareTo(index2);
            }

            if (coll.ContainsKey(message1))
            {
                return -1;
            }

            if (coll.ContainsKey(message2))
            {
                return 1;
            }

            return string.Compare(message1, message2, true);
        }
    }
}