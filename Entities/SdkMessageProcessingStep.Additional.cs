using Microsoft.Xrm.Sdk;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class SdkMessageProcessingStep
    {
        public static partial class Schema
        {
            public static partial class EntityAliasFields
            {
                public const string SdkMessageFilterPrimaryObjectTypeCode = Attributes.sdkmessagefilterid + "." + SdkMessageFilter.Schema.Attributes.primaryobjecttypecode;

                public const string SdkMessageFilterSecondaryObjectTypeCode = Attributes.sdkmessagefilterid + "." + SdkMessageFilter.Schema.Attributes.secondaryobjecttypecode;

                public const string PluginAssemblyId = Attributes.eventhandler + "." + PluginType.Schema.Attributes.pluginassemblyid;
            }
        }

        public string PluginAssemblyName
        {
            get
            {
                if (this.Attributes.ContainsKey(Schema.EntityAliasFields.PluginAssemblyId)
                    && this.Attributes[Schema.EntityAliasFields.PluginAssemblyId] != null
                    && this.Attributes[Schema.EntityAliasFields.PluginAssemblyId] is AliasedValue aliasedValue
                    && aliasedValue.Value is EntityReference aliasedValueValue
                    )
                {
                    return aliasedValueValue.Name;
                }

                return "Unknown";
            }
        }

        public Guid? PluginAssemblyId
        {
            get
            {
                if (this.Attributes.ContainsKey(Schema.EntityAliasFields.PluginAssemblyId)
                    && this.Attributes[Schema.EntityAliasFields.PluginAssemblyId] != null
                    && this.Attributes[Schema.EntityAliasFields.PluginAssemblyId] is AliasedValue aliasedValue
                    && aliasedValue.Value is EntityReference aliasedValueValue
                    )
                {
                    return aliasedValueValue.Id;
                }

                return null;
            }
        }

        public string PrimaryObjectTypeCodeName
        {
            get
            {
                if (this.Attributes.ContainsKey(Schema.EntityAliasFields.SdkMessageFilterPrimaryObjectTypeCode)
                    && this.Attributes[Schema.EntityAliasFields.SdkMessageFilterPrimaryObjectTypeCode] != null
                    && this.Attributes[Schema.EntityAliasFields.SdkMessageFilterPrimaryObjectTypeCode] is AliasedValue aliasedValue
                    && aliasedValue.Value is string aliasedValueValue
                    )
                {
                    return aliasedValueValue;
                }

                return "none";
            }
        }

        public string SecondaryObjectTypeCodeName
        {
            get
            {
                if (this.Attributes.ContainsKey(Schema.EntityAliasFields.SdkMessageFilterSecondaryObjectTypeCode)
                    && this.Attributes[Schema.EntityAliasFields.SdkMessageFilterSecondaryObjectTypeCode] != null
                    && this.Attributes[Schema.EntityAliasFields.SdkMessageFilterSecondaryObjectTypeCode] is AliasedValue aliasedValue
                    && aliasedValue.Value is string aliasedValueValue
                    )
                {
                    return aliasedValueValue;
                }

                return "none";
            }
        }

        public IEnumerable<string> FilteringAttributesStrings
        {
            get
            {
                if (!string.IsNullOrEmpty(this.FilteringAttributes))
                {
                    foreach (var item in this.FilteringAttributes.Split(',').OrderBy(s => s))
                    {
                        yield return item;
                    }
                }
            }
        }

        public string FilteringAttributesStringsSorted
        {
            get
            {
                return string.Join(",", this.FilteringAttributesStrings);
            }
        }
    }
}
