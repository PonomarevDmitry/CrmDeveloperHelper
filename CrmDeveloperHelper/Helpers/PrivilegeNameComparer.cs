using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class PrivilegeNameComparer : IComparer<string>
    {
        private static readonly List<string> _prefixes = new List<string>() {
            "prvCreate"
            , "prvRead"
            , "prvWrite"
            , "prvUpdate"
            , "prvDelete"

            , "prvAppendTo"
            , "prvAppend"

            , "prvAssign"
            , "prvShare"

            , "prvPublish"
            , "prvReparent"
            , "prvDisable"
            , "prvRollup"
        };

        private PrivilegeNameComparer()
        {

        }

        public static PrivilegeNameComparer Comparer { get; } = new PrivilegeNameComparer();

        private enum PrivilegeCategory
        {
            Include,

            NotInclude,
        }

        private static PrivilegeCategory CategorizationPrivilege(string name)
        {
            foreach (var item in _prefixes)
            {
                if (name.StartsWith(item, StringComparison.InvariantCultureIgnoreCase))
                {
                    return PrivilegeCategory.Include;
                }
            }

            return PrivilegeCategory.NotInclude;
        }

        private static int CategorizationPrivilegeIndex(string name)
        {
            for (int i = 0; i < _prefixes.Count; i++)
            {
                var item = _prefixes[i];

                if (name.StartsWith(item, StringComparison.InvariantCultureIgnoreCase))
                {
                    return i;
                }
            }

            return _prefixes.Count + 1;
        }
        private static string GetObjectNameFromPrivilege(string name)
        {
            foreach (var item in _prefixes)
            {
                if (name.StartsWith(item, StringComparison.InvariantCultureIgnoreCase))
                {
                    return Regex.Replace(name, item, string.Empty, RegexOptions.IgnoreCase);
                }
            }

            return name;
        }

        public int Compare(string privilegeName1, string privilegeName2)
        {
            if (string.IsNullOrEmpty(privilegeName1))
            {
                return -1;
            }

            if (string.IsNullOrEmpty(privilegeName2))
            {
                return 1;
            }

            var category1 = CategorizationPrivilege(privilegeName1);
            var category2 = CategorizationPrivilege(privilegeName2);

            {
                if (category1 != category2)
                {
                    return category1.CompareTo(category2);
                }
            }

            {
                string objectName1 = GetObjectNameFromPrivilege(privilegeName1);
                string objectName2 = GetObjectNameFromPrivilege(privilegeName2);

                if (!string.Equals(objectName1, objectName2, StringComparison.InvariantCultureIgnoreCase))
                {
                    return string.Compare(objectName1, objectName2, true);
                }
            }

            {
                if (category1 == PrivilegeCategory.Include && category2 == PrivilegeCategory.Include)
                {
                    int index1 = CategorizationPrivilegeIndex(privilegeName1);
                    int index2 = CategorizationPrivilegeIndex(privilegeName2);

                    if (index1 != index2)
                    {
                        return index1.CompareTo(index2);
                    }
                }
            }

            return string.Compare(privilegeName1, privilegeName2, true);
        }
    }
}
