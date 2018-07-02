using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    internal class LocaleComparer : IComparer<int>
    {
        private List<int> coll = new List<int>() { 1033, 1049, 1031, 1029 };

        public int Compare(int locale1, int locale2)
        {
            if (coll.Contains(locale1) && coll.Contains(locale2))
            {
                int index1 = coll.IndexOf(locale1);
                int index2 = coll.IndexOf(locale2);

                return index1.CompareTo(index2);
            }

            if (coll.Contains(locale1))
            {
                return -1;
            }

            if (coll.Contains(locale2))
            {
                return 1;
            }

            return locale1.CompareTo(locale2);
        }
    }
}