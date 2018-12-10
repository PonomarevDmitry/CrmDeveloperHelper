using Microsoft.Xrm.Sdk;
using System;

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

                public const string TeamName = Team.Schema.Attributes.teamid + "." + Team.Schema.Attributes.name;

                public const string TeamBusinessUnitName = Team.Schema.Attributes.teamid + "." + Team.Schema.Attributes.businessunitid;

                public const string TeamId = Team.Schema.Attributes.teamid + "." + Team.Schema.Attributes.teamid;

                public const string TeamIsDefault = Team.Schema.Attributes.teamid + "." + Team.Schema.Attributes.isdefault;
            }
        }

        public EntityReference BusinessUnitParentBusinessUnit
        {
            get
            {
                if (this.Attributes.ContainsKey(Schema.EntityAliasFields.BusinessUnitParentBusinessUnit)
                    && this.Attributes[Schema.EntityAliasFields.BusinessUnitParentBusinessUnit] != null
                    && this.Attributes[Schema.EntityAliasFields.BusinessUnitParentBusinessUnit] is AliasedValue aliasedValue
                    && aliasedValue.Value is EntityReference aliasedValueValue
                    )
                {
                    return aliasedValueValue;
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
                    && aliasedValue.Value is string aliasedValueValue
                    )
                {
                    return aliasedValueValue;
                }

                return string.Empty;
            }
        }

        public string TeamName
        {
            get
            {
                if (this.Attributes.ContainsKey(Schema.EntityAliasFields.TeamName)
                    && this.Attributes[Schema.EntityAliasFields.TeamName] != null
                    && this.Attributes[Schema.EntityAliasFields.TeamName] is AliasedValue aliasedValue
                    && aliasedValue.Value is string aliasedValueValue
                    )
                {
                    return aliasedValueValue;
                }

                return string.Empty;
            }
        }

        public string TeamBusinessUnitName
        {
            get
            {
                if (this.Attributes.ContainsKey(Schema.EntityAliasFields.TeamBusinessUnitName)
                    && this.Attributes[Schema.EntityAliasFields.TeamBusinessUnitName] != null
                    && this.Attributes[Schema.EntityAliasFields.TeamBusinessUnitName] is AliasedValue aliasedValue
                    && aliasedValue.Value is EntityReference aliasedValueValue
                    )
                {
                    return aliasedValueValue.Name;
                }

                return string.Empty;
            }
        }

        public Guid? TeamId
        {
            get
            {
                if (this.Attributes.ContainsKey(Schema.EntityAliasFields.TeamId)
                    && this.Attributes[Schema.EntityAliasFields.TeamId] != null
                    && this.Attributes[Schema.EntityAliasFields.TeamId] is AliasedValue aliasedValue
                    && aliasedValue.Value is Guid aliasedValueValue
                    )
                {
                    return aliasedValueValue;
                }

                return null;
            }
        }

        public bool TeamIsDefault
        {
            get
            {
                if (this.Attributes.ContainsKey(Schema.EntityAliasFields.TeamIsDefault)
                    && this.Attributes[Schema.EntityAliasFields.TeamIsDefault] != null
                    && this.Attributes[Schema.EntityAliasFields.TeamIsDefault] is AliasedValue aliasedValue
                    && aliasedValue.Value is bool aliasedValueValue
                    )
                {
                    return aliasedValueValue;
                }

                return false;
            }
        }
    }
}
