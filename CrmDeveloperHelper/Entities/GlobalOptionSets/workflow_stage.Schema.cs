
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities.GlobalOptionSets
{

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Workflow Stage
    ///     (Russian - 1049): Этап бизнес-процесса
    /// 
    /// Description:
    ///     (English - United States - 1033): Stage in which the Workflow executes
    ///     (Russian - 1049): Стадия, на которой выполняется бизнес-процесс
    /// 
    /// OptionSet Name: workflow_stage      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 3
    ///     AttributeName           DisplayName     IsCustomizable    Behavior
    ///     workflow.createstage    Create Stage    True              IncludeSubcomponents
    ///     workflow.deletestage    Delete stage    True              IncludeSubcomponents
    ///     workflow.updatestage    Update Stage    True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Workflow Stage")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum workflow_stage
    {
        ///<summary>
        /// 20
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Pre-operation
        ///     (Russian - 1049): Перед основной операцией внутри транзакции
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Pre-operation")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Pre_operation_20 = 20,

        ///<summary>
        /// 40
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Post-operation
        ///     (Russian - 1049): После основной операции внутри транзакции
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Post-operation")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Post_operation_40 = 40,
    }
}