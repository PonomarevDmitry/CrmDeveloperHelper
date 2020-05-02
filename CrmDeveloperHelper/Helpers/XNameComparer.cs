using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class XNameComparer : IComparer<XName>
    {
        private readonly Dictionary<string, int> _predefinedNames;
        private readonly Dictionary<string, int> _predefinedNamespaces;

        public XNameComparer(List<string> predefinedNames, List<string> predefinedNamespaces = null)
        {
            if (predefinedNames == null)
            {
                throw new ArgumentNullException(nameof(predefinedNames));
            }

            this._predefinedNames = CreateDictionary(predefinedNames);

            if (predefinedNamespaces != null)
            {
                _predefinedNamespaces = CreateDictionary(predefinedNamespaces);
            }
        }

        private static Dictionary<string, int> CreateDictionary(List<string> predefinedNames)
        {
            var result = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);

            int index = 0;
            foreach (var item in predefinedNames)
            {
                if (!result.ContainsKey(item))
                {
                    result.Add(item, index);
                }

                index++;
            }

            return result;
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
                    && _predefinedNamespaces.ContainsKey(name1.NamespaceName)
                    && _predefinedNamespaces.ContainsKey(name2.NamespaceName)
                )
                {
                    int index1 = _predefinedNamespaces[name1.NamespaceName];
                    int index2 = _predefinedNamespaces[name2.NamespaceName];

                    return index1.CompareTo(index2);
                }

                if (_predefinedNamespaces.ContainsKey(name1.NamespaceName))
                {
                    return -1;
                }

                if (_predefinedNamespaces.ContainsKey(name2.NamespaceName))
                {
                    return 1;
                }

                return (name1.NamespaceName ?? string.Empty).CompareTo(name2.NamespaceName ?? string.Empty);
            }

            if (_predefinedNames != null
                && _predefinedNames.ContainsKey(name1.LocalName)
                && _predefinedNames.ContainsKey(name2.LocalName)
            )
            {
                int index1 = _predefinedNames[name1.LocalName];
                int index2 = _predefinedNames[name2.LocalName];

                return index1.CompareTo(index2);
            }

            if (_predefinedNames.ContainsKey(name1.LocalName))
            {
                return -1;
            }

            if (_predefinedNames.ContainsKey(name2.LocalName))
            {
                return 1;
            }

            return string.Compare(name1.LocalName, name2.LocalName, true);
        }
    }
}