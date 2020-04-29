using Microsoft.Xrm.Sdk;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class SdkMessageFilter
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
                public const string SdkMessageName = Attributes.sdkmessageid + "." + SdkMessage.Schema.Attributes.name;

                public const string SdkMessageCategoryName = Attributes.sdkmessageid + "." + SdkMessage.Schema.Attributes.categoryname;
            }
        }

        public string MessageName
        {
            get
            {
                if (this.Attributes.ContainsKey(Schema.EntityAliasFields.SdkMessageName)
                    && this.Attributes[Schema.EntityAliasFields.SdkMessageName] != null
                    && this.Attributes[Schema.EntityAliasFields.SdkMessageName] is AliasedValue aliasedValue
                    && aliasedValue.Value != null
                    && aliasedValue.Value is string aliasedValueValue
                )
                {
                    return aliasedValueValue;
                }

                return "none";
            }
        }

        public string MessageCategoryName
        {
            get
            {
                if (this.Attributes.ContainsKey(Schema.EntityAliasFields.SdkMessageCategoryName)
                    && this.Attributes[Schema.EntityAliasFields.SdkMessageCategoryName] != null
                    && this.Attributes[Schema.EntityAliasFields.SdkMessageCategoryName] is AliasedValue aliasedValue
                    && aliasedValue.Value != null
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
