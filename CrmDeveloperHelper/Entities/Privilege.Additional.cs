using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class Privilege
    {
        public static partial class Schema
        {
            public static partial class EntityAliasFields
            {
                public const string EntityName = PrivilegeObjectTypeCodes.Schema.EntityLogicalName + "." + PrivilegeObjectTypeCodes.Schema.Attributes.objecttypecode;
            }
        }

        public string EntityName
        {
            get
            {
                if (this.Attributes.ContainsKey(Schema.EntityAliasFields.EntityName)
                    && this.Attributes[Schema.EntityAliasFields.EntityName] != null
                    && this.Attributes[Schema.EntityAliasFields.EntityName] is AliasedValue aliasedValue
                    && aliasedValue.Value is string aliasedValueValue
                    )
                {
                    return aliasedValueValue;
                }

                return string.Empty;
            }
        }
    }
}
