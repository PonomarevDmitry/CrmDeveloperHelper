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
            }
        }

        public EntityReference BusinessUnitParentBusinessUnit
        {
            get
            {
                if (this.Attributes.ContainsKey(Schema.EntityAliasFields.BusinessUnitParentBusinessUnit)
                    && this.Attributes[Schema.EntityAliasFields.BusinessUnitParentBusinessUnit] != null
                    && this.Attributes[Schema.EntityAliasFields.BusinessUnitParentBusinessUnit] is AliasedValue
                    )
                {
                    return (EntityReference)this.GetAttributeValue<AliasedValue>(Schema.EntityAliasFields.BusinessUnitParentBusinessUnit).Value;
                }

                return null;
            }
        }
    }
}
