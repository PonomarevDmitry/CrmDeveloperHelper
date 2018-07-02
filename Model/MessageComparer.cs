using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class MessageComparer : IComparer<string>
    {
        private List<string> coll = new List<string>() { "Create", "Update", "Delete", "SetState", "SetStateDynamicEntity", "Merge" };

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

            if (coll.Contains(message1) && coll.Contains(message2))
            {
                int index1 = coll.IndexOf(message1);
                int index2 = coll.IndexOf(message2);

                return index1.CompareTo(index2);
            }

            if (coll.Contains(message1))
            {
                return -1;
            }

            if (coll.Contains(message2))
            {
                return 1;
            }

            return string.Compare(message1, message2);
        }
    }
}