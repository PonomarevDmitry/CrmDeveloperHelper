using Microsoft.Xrm.Sdk;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class AttributeMap
    {
        public static partial class Schema
        {
            public static partial class EntityAliasFields
            {
                public const string EntityMapIdSourceEntityName = Attributes.entitymapid + "." + EntityMap.Schema.Attributes.sourceentityname;

                public const string EntityMapIdTargetEntityName = Attributes.entitymapid + "." + EntityMap.Schema.Attributes.targetentityname;
            }
        }

        public string EntityMapIdSourceEntityName
        {
            get
            {
                if (this.Attributes.ContainsKey(Schema.EntityAliasFields.EntityMapIdSourceEntityName)
                    && this.Attributes[Schema.EntityAliasFields.EntityMapIdSourceEntityName] != null
                    && this.Attributes[Schema.EntityAliasFields.EntityMapIdSourceEntityName] is AliasedValue aliasedValue
                    && aliasedValue.Value is string aliasedValueValue
                    )
                {
                    return aliasedValueValue;
                }

                return "none";
            }
        }

        public string EntityMapIdTargetEntityName
        {
            get
            {
                if (this.Attributes.ContainsKey(Schema.EntityAliasFields.EntityMapIdTargetEntityName)
                    && this.Attributes[Schema.EntityAliasFields.EntityMapIdTargetEntityName] != null
                    && this.Attributes[Schema.EntityAliasFields.EntityMapIdTargetEntityName] is AliasedValue aliasedValue
                    && aliasedValue.Value is string aliasedValueValue
                    )
                {
                    return aliasedValueValue;
                }

                return "none";
            }
        }
    }
}
