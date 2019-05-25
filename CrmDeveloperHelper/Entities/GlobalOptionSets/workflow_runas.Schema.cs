
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities.GlobalOptionSets
{

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Executing User
    ///     (Russian - 1049): Пользователь, выполняющий операцию
    /// 
    /// Description:
    ///     (English - United States - 1033): Specifies the system user account under which a workflow executes
    ///     (Russian - 1049): Указывает системную учетную запись, от имени которой выполняется бизнес-процесс
    /// 
    /// OptionSet Name: workflow_runas      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 1
    ///     AttributeName     DisplayName    IsCustomizable    Behavior
    ///     workflow.runas    Run As User    True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Executing User")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum workflow_runas
    {
        ///<summary>
        /// 0
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Owner
        ///     (Russian - 1049): Ответственный
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Owner")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Owner_0 = 0,

        ///<summary>
        /// 1
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Calling User
        ///     (Russian - 1049): Вызывающий пользователь
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Calling User")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Calling_User_1 = 1,
    }
}