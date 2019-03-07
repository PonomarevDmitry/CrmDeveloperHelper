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
            #region OptionSets.

            public static partial class OptionSets
            {
                #region Picklist OptionSet OptionSets.

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Availability
                ///     (Russian - 1049): Доступность
                /// 
                /// Description:
                ///     (English - United States - 1033): Identifies where a method will be exposed. 0 - Server, 1 - Client, 2 - both.
                ///     (Russian - 1049): Указывает, где будет применен метод. 0 - сервер, 1 - клиент, 2 - оба.
                /// 
                /// SchemaName: Availability
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public enum availability
                {
                    ///<summary>
                    /// 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Server
                    ///     (Russian - 1049): Сервер
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Server = 0,

                    ///<summary>
                    /// 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Client
                    ///     (Russian - 1049): Клиент
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Client = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Both
                    ///     (Russian - 1049): Оба
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Both = 2,
                }

                #endregion Picklist OptionSets.
            }

            #endregion OptionSets.

            public static partial class EntityAliasFields
            {
                public const string SdkMessageFilterPrimaryObjectTypeCode = SdkMessageFilter.PrimaryIdAttribute + "." + SdkMessageFilter.Schema.Attributes.primaryobjecttypecode;

                public const string SdkMessageFilterSecondaryObjectTypeCode = SdkMessageFilter.PrimaryIdAttribute + "." + SdkMessageFilter.Schema.Attributes.secondaryobjecttypecode;

                public const string SdkMessageFilterId = SdkMessageFilter.PrimaryIdAttribute + "." + SdkMessageFilter.PrimaryIdAttribute;
            }
        }

        public static partial class Instances
        {
            public const string ExportFieldTranslation = "ExportFieldTranslation";

            public const string RetrieveMultiple = "RetrieveMultiple";
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
