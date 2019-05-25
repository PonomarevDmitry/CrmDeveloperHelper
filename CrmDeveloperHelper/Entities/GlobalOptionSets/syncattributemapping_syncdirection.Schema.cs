
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities.GlobalOptionSets
{

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Sync Direction
    ///     (Russian - 1049): Направление синхронизации
    /// 
    /// OptionSet Name: syncattributemapping_syncdirection      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 2
    ///     AttributeName                                DisplayName       IsCustomizable    Behavior
    ///     syncattributemapping.defaultsyncdirection    Sync Direction    False             IncludeSubcomponents
    ///     syncattributemapping.syncdirection           Sync Direction    False             IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Sync Direction")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum syncattributemapping_syncdirection
    {
        ///<summary>
        /// 0
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): None
        ///     (Russian - 1049): Нет
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("None")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        None_0 = 0,

        ///<summary>
        /// 1
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): ToExchange
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("ToExchange")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ToExchange_1 = 1,

        ///<summary>
        /// 2
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): ToCRM
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("ToCRM")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ToCRM_2 = 2,

        ///<summary>
        /// 3
        /// DisplayOrder: 4
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Bidirectional
        ///     (Russian - 1049): Двунаправленный
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Bidirectional")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Bidirectional_3 = 3,
    }
}