using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class XNameComparer : IComparer<XName>
    {
        private static List<string> coll = new List<string>()
        {
            "Id"
            , "GroupId"
            , "Name"
            , "Entity"
            , "Command"
            , "Sequence"
            , "LabelText"
            , "Alt"
            , "ToolTipTitle"
            , "ToolTipDescription"
        };

        public int Compare(XName name1, XName name2)
        {
            if (string.IsNullOrEmpty(name1.ToString()))
            {
                return -1;
            }

            if (string.IsNullOrEmpty(name2.ToString()))
            {
                return 1;
            }

            if (!string.Equals(name1.NamespaceName, name2.NamespaceName))
            {
                return (name1.NamespaceName ?? string.Empty).CompareTo(name2.NamespaceName ?? string.Empty);
            }

            if (coll.Contains(name1.LocalName, StringComparer.InvariantCultureIgnoreCase)
               && coll.Contains(name2.LocalName, StringComparer.InvariantCultureIgnoreCase)
               )
            {
                var item1 = coll.First(i => StringComparer.InvariantCultureIgnoreCase.Equals(i, name1.LocalName));
                var item2 = coll.First(i => StringComparer.InvariantCultureIgnoreCase.Equals(i, name2.LocalName));

                int index1 = coll.IndexOf(item1);
                int index2 = coll.IndexOf(item2);

                return index1.CompareTo(index2);
            }

            if (coll.Contains(name1.LocalName, StringComparer.InvariantCultureIgnoreCase))
            {
                return -1;
            }

            if (coll.Contains(name2.LocalName, StringComparer.InvariantCultureIgnoreCase))
            {
                return 1;
            }

            return string.Compare(name1.LocalName, name2.LocalName);
        }
    }
}