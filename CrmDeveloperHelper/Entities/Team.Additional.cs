using Microsoft.Xrm.Sdk;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class Team
    {
        public static partial class Schema
        {
            public static partial class EntityAliasFields
            {
                public const string TeamTemplateName = Schema.Attributes.teamtemplateid + "." + TeamTemplate.Schema.Attributes.teamtemplatename;
            }
        }

        public string TeamTypeName
        {
            get
            {
                if (this.FormattedValues.ContainsKey(Schema.Attributes.teamtype))
                {
                    return this.FormattedValues[Schema.Attributes.teamtype];
                }

                return string.Empty;
            }
        }

        public string TeamTemplateName
        {
            get
            {
                if (this.Attributes.ContainsKey(Schema.EntityAliasFields.TeamTemplateName)
                    && this.Attributes[Schema.EntityAliasFields.TeamTemplateName] != null
                    && this.Attributes[Schema.EntityAliasFields.TeamTemplateName] is AliasedValue aliasedValue
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