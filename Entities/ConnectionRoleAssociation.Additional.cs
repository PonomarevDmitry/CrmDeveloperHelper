using Microsoft.Xrm.Sdk;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class ConnectionRoleAssociation
    {
        public static partial class Schema
        {
            public static partial class EntityAliasFields
            {
                public const string ConnectionRoleName = ConnectionRole.Schema.EntityLogicalName + "." + ConnectionRole.Schema.Attributes.name;
            }
        }

        public string AssociatedConnectionRoleName
        {
            get
            {
                if (this.Attributes.ContainsKey(Schema.EntityAliasFields.ConnectionRoleName)
                    && this.Attributes[Schema.EntityAliasFields.ConnectionRoleName] != null
                    && this.Attributes[Schema.EntityAliasFields.ConnectionRoleName] is AliasedValue
                    )
                {
                    return (string)this.GetAttributeValue<AliasedValue>(Schema.EntityAliasFields.ConnectionRoleName).Value;
                }

                return "none";
            }
        }
    }
}
