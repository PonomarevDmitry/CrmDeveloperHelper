using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class XNameComparer : IComparer<XName>
    {
        private readonly List<string> _predefinedNames;
        private readonly List<string> _predefinedNamespaces;

        public XNameComparer(List<string> predefinedNames, List<string> predefinedNamespaces = null)
        {
            this._predefinedNames = predefinedNames ?? throw new ArgumentNullException(nameof(predefinedNames));

            if (predefinedNamespaces != null)
            {
                _predefinedNamespaces = predefinedNamespaces;
            }
        }

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

            if (!string.Equals(name1.NamespaceName, name2.NamespaceName, StringComparison.InvariantCultureIgnoreCase))
            {
                if (_predefinedNamespaces != null
                    && _predefinedNamespaces.Contains(name1.NamespaceName, StringComparer.InvariantCultureIgnoreCase)
                    && _predefinedNamespaces.Contains(name2.NamespaceName, StringComparer.InvariantCultureIgnoreCase)
                )
                {
                    var item1 = _predefinedNamespaces.First(i => StringComparer.InvariantCultureIgnoreCase.Equals(i, name1.NamespaceName));
                    var item2 = _predefinedNamespaces.First(i => StringComparer.InvariantCultureIgnoreCase.Equals(i, name2.NamespaceName));

                    int index1 = _predefinedNamespaces.IndexOf(item1);
                    int index2 = _predefinedNamespaces.IndexOf(item2);

                    return index1.CompareTo(index2);
                }

                if (_predefinedNamespaces.Contains(name1.NamespaceName, StringComparer.InvariantCultureIgnoreCase))
                {
                    return -1;
                }

                if (_predefinedNamespaces.Contains(name2.NamespaceName, StringComparer.InvariantCultureIgnoreCase))
                {
                    return 1;
                }

                return (name1.NamespaceName ?? string.Empty).CompareTo(name2.NamespaceName ?? string.Empty);
            }

            if (_predefinedNames != null
                && _predefinedNames.Contains(name1.LocalName, StringComparer.InvariantCultureIgnoreCase)
                && _predefinedNames.Contains(name2.LocalName, StringComparer.InvariantCultureIgnoreCase)
            )
            {
                var item1 = _predefinedNames.First(i => StringComparer.InvariantCultureIgnoreCase.Equals(i, name1.LocalName));
                var item2 = _predefinedNames.First(i => StringComparer.InvariantCultureIgnoreCase.Equals(i, name2.LocalName));

                int index1 = _predefinedNames.IndexOf(item1);
                int index2 = _predefinedNames.IndexOf(item2);

                return index1.CompareTo(index2);
            }

            if (_predefinedNames.Contains(name1.LocalName, StringComparer.InvariantCultureIgnoreCase))
            {
                return -1;
            }

            if (_predefinedNames.Contains(name2.LocalName, StringComparer.InvariantCultureIgnoreCase))
            {
                return 1;
            }

            return string.Compare(name1.LocalName, name2.LocalName);
        }
    }
}