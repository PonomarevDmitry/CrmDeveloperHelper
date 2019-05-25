
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities.GlobalOptionSets
{

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): A Yes or No boolean
    ///     (Russian - 1049): Логическое "Да" или "Нет"
    /// 
    /// Description:
    ///     (English - United States - 1033): A Yes or No boolean.
    ///     (Russian - 1049): Логическое "Да" или "Нет".
    /// 
    /// OptionSet Name: field_security_permission_type      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 6
    ///     AttributeName                               DisplayName                 IsCustomizable    Behavior
    ///     fieldpermission.cancreate                   Can create the attribute    False             IncludeSubcomponents
    ///     fieldpermission.canread                     Can Read the attribute      False             IncludeSubcomponents
    ///     fieldpermission.canupdate                   Can Update the attribute    False             IncludeSubcomponents
    ///     principalattributeaccessmap.createaccess                                False             IncludeSubcomponents
    ///     principalattributeaccessmap.readaccess                                  False             IncludeSubcomponents
    ///     principalattributeaccessmap.updateaccess                                False             IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("A Yes or No boolean")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum field_security_permission_type
    {
        ///<summary>
        /// 0
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Not Allowed
        ///     (Russian - 1049): Недопустимо
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Not Allowed")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Not_Allowed_0 = 0,

        ///<summary>
        /// 4
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Allowed
        ///     (Russian - 1049): Допустимо
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Allowed")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Allowed_4 = 4,
    }
}