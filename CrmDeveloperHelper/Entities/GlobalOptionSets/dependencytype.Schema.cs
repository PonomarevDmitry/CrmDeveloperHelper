
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities.GlobalOptionSets
{

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Dependency Type
    ///     (Russian - 1049): Тип зависимости
    /// 
    /// Description:
    ///     (English - United States - 1033): The kind of dependency.
    ///     (Russian - 1049): Вид зависимости.
    /// 
    /// OptionSet Name: dependencytype      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 2
    ///     AttributeName                               DisplayName        IsCustomizable    Behavior
    ///     dependency.dependencytype                   Dependency Type    False             IncludeSubcomponents
    ///     invaliddependency.existingdependencytype    Weight             False             IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Dependency Type")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum dependencytype
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
        ///     (English - United States - 1033): Solution Internal
        ///     (Russian - 1049): Внутри решения
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Solution Internal")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Solution_Internal_1 = 1,

        ///<summary>
        /// 2
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Published
        ///     (Russian - 1049): Опубликовано
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Published")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Published_2 = 2,

        ///<summary>
        /// 4
        /// DisplayOrder: 4
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Unpublished
        ///     (Russian - 1049): Неопубликованный
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Unpublished")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Unpublished_4 = 4,
    }
}