using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model
{
    public static class IntellisenseContext
    {
        public const string NameIntellisenseContextName = "intellisenseContext";
        public const string NameIntellisenseContextAttributeEntityName = "entityName";

        public static readonly XNamespace IntellisenseContextNamespace = "https://navicongroup.ru/XsdSchemas/IntellisenseContext";
        public static readonly XName IntellisenseContextAttributeEntityName = IntellisenseContextNamespace + NameIntellisenseContextAttributeEntityName;

        public static readonly XNamespace NamespaceXMLSchemaInstance = "http://www.w3.org/2001/XMLSchema-instance";
    }
}
