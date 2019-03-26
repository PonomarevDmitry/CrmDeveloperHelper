﻿using Microsoft.Xrm.Sdk;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class Solution
    {
        public static partial class Schema
        {
            public static partial class EntityAliasFields
            {
                public const string PublisherCustomizationPrefix = Attributes.publisherid + "." + Publisher.Schema.Attributes.customizationprefix;

                public const string SupportingSolution = "supporting" + Solution.EntityLogicalName;
            }

            public static partial class InstancesUniqueNames
            {
                public const string Default = "Default";

                public const string Active = "Active";
            }

            public static partial class InstancesUniqueId
            {
                public readonly static Guid DefaultId = Guid.Parse("fd140aaf-4df4-11dd-bd17-0019b9312238");
            }
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
                    && this.Attributes[Schema.EntityAliasFields.PublisherCustomizationPrefix] is AliasedValue aliasedValue
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
