using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class SdkMessage
    {
        public static partial class Schema
        {
            public static partial class EntityAliasFields
            {
                public const string SdkMessageFilterPrimaryObjectTypeCode = SdkMessageFilter.EntityPrimaryIdAttribute + "." + SdkMessageFilter.Schema.Attributes.primaryobjecttypecode;

                public const string SdkMessageFilterSecondaryObjectTypeCode = SdkMessageFilter.EntityPrimaryIdAttribute + "." + SdkMessageFilter.Schema.Attributes.secondaryobjecttypecode;

                public const string SdkMessageFilterId = SdkMessageFilter.EntityPrimaryIdAttribute + "." + SdkMessageFilter.EntityPrimaryIdAttribute;
            }
        }

        public static partial class Instances
        {
            public const string ExportFieldTranslation = "ExportFieldTranslation";

            public const string RetrieveMultiple = "RetrieveMultiple";

            public const string Retrieve = "Retrieve";
        }

        public string PrimaryObjectTypeCodeName
        {
            get
            {
                if (this.Attributes.ContainsKey(Schema.EntityAliasFields.SdkMessageFilterPrimaryObjectTypeCode)
                    && this.Attributes[Schema.EntityAliasFields.SdkMessageFilterPrimaryObjectTypeCode] != null
                    && this.Attributes[Schema.EntityAliasFields.SdkMessageFilterPrimaryObjectTypeCode] is AliasedValue aliasedValue
                    && aliasedValue.Value != null
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
                    && aliasedValue.Value != null
                    && aliasedValue.Value is string aliasedValueValue
                )
                {
                    return aliasedValueValue;
                }

                return "none";
            }
        }

        public Guid? SdkMessageFilterId
        {
            get
            {
                if (this.Attributes.ContainsKey(Schema.EntityAliasFields.SdkMessageFilterId)
                    && this.Attributes[Schema.EntityAliasFields.SdkMessageFilterId] != null
                    && this.Attributes[Schema.EntityAliasFields.SdkMessageFilterId] is AliasedValue aliasedValue
                    && aliasedValue.Value != null
                    && aliasedValue.Value is Guid aliasedValueValue
                )
                {
                    return aliasedValueValue;
                }

                return null;
            }
        }
    }
}
