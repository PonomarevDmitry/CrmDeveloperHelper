using Microsoft.Xrm.Sdk;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class Solution
    {
        public static partial class Schema
        {
            public static partial class EntityAliasFields
            {
                public const string PublisherCustomizationPrefix = Publisher.Schema.EntityLogicalName + "." + Publisher.Schema.Attributes.customizationprefix;

                public const string SupportingSolution = "supporting" + Solution.EntityLogicalName;
            }
        }

        public static partial class InstancesUniqueNames
        {
            public const string Default = "Default";

            public const string Active = "Active";
        }

        public string UniqueNameEscapeUnderscore
        {
            get
            {
                if (string.IsNullOrEmpty(this.UniqueName))
                {
                    return string.Empty;
                }

                return this.UniqueName.Replace("_", "__");
            }
        }

        public string PublisherCustomizationPrefix
        {
            get
            {
                if (this.Attributes.ContainsKey(Schema.EntityAliasFields.PublisherCustomizationPrefix)
                    && this.Attributes[Schema.EntityAliasFields.PublisherCustomizationPrefix] != null
                    && this.Attributes[Schema.EntityAliasFields.PublisherCustomizationPrefix] is AliasedValue
                    )
                {
                    return (string)this.GetAttributeValue<AliasedValue>(Schema.EntityAliasFields.PublisherCustomizationPrefix).Value;
                }

                return string.Empty;
            }
        }
    }
}
