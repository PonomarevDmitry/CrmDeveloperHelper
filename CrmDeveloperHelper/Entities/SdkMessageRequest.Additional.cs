using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class SdkMessageRequest
    {
        public static partial class Schema
        {
            public static partial class EntityAliasFields
            {
                public const string SdkMessageName = Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid + "." + SdkMessage.Schema.Attributes.name;

                public const string SdkMessageId = Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.sdkmessageid;

                public const string Namespace = Attributes.sdkmessagepairid + "." + SdkMessagePair.Schema.Attributes.@namespace;
            }
        }

        public static partial class Instances
        {
            public const string ExportFieldTranslationRequest = "ExportFieldTranslationRequest";

            public const string RetrieveEntityKeyRequest = "RetrieveEntityKeyRequest";
        }

        public Guid? SdkMessageId
        {
            get
            {
                if (this.Attributes.ContainsKey(Schema.EntityAliasFields.SdkMessageId)
                    && this.Attributes[Schema.EntityAliasFields.SdkMessageId] != null
                    && this.Attributes[Schema.EntityAliasFields.SdkMessageId] is AliasedValue aliasedValue
                    && aliasedValue.Value is EntityReference aliasedValueValue
                    )
                {
                    return aliasedValueValue.Id;
                }

                return null;
            }
        }

        public string SdkMessageName
        {
            get
            {
                if (this.Attributes.ContainsKey(Schema.EntityAliasFields.SdkMessageName)
                    && this.Attributes[Schema.EntityAliasFields.SdkMessageName] != null
                    && this.Attributes[Schema.EntityAliasFields.SdkMessageName] is AliasedValue aliasedValue
                    && aliasedValue.Value is string aliasedValueValue
                    )
                {
                    return aliasedValueValue;
                }

                return "Unknown";
            }
        }

        public string Namespace
        {
            get
            {
                if (this.Attributes.ContainsKey(Schema.EntityAliasFields.Namespace)
                    && this.Attributes[Schema.EntityAliasFields.Namespace] != null
                    && this.Attributes[Schema.EntityAliasFields.Namespace] is AliasedValue aliasedValue
                    && aliasedValue.Value is string aliasedValueValue
                    )
                {
                    return aliasedValueValue;
                }

                return "None";
            }
        }
    }
}
