using Microsoft.Xrm.Sdk;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class Role
    {
        public static partial class Schema
        {
            public static partial class EntityAliasFields
            {
                public const string BusinessUnitParentBusinessUnit = BusinessUnit.Schema.EntityLogicalName + "." + BusinessUnit.Schema.Attributes.parentbusinessunitid;

                public const string RoleTemplateName = Role.Schema.Attributes.roletemplateid + "." + RoleTemplate.Schema.Attributes.name;
            }
        }

        public EntityReference BusinessUnitParentBusinessUnit
        {
            get
            {
                if (this.Attributes.ContainsKey(Schema.EntityAliasFields.BusinessUnitParentBusinessUnit)
                    && this.Attributes[Schema.EntityAliasFields.BusinessUnitParentBusinessUnit] != null
                    && this.Attributes[Schema.EntityAliasFields.BusinessUnitParentBusinessUnit] is AliasedValue aliasedValue
                    )
                {
                    return (EntityReference)aliasedValue.Value;
                }

                return null;
            }
        }

        public string RoleTemplateName
        {
            get
            {
                if (this.Attributes.ContainsKey(Schema.EntityAliasFields.RoleTemplateName)
                    && this.Attributes[Schema.EntityAliasFields.RoleTemplateName] != null
                    && this.Attributes[Schema.EntityAliasFields.RoleTemplateName] is AliasedValue aliasedValue
                    )
                {
                    return (string)aliasedValue.Value;
                }

                return string.Empty;
            }
        }
    }
}
