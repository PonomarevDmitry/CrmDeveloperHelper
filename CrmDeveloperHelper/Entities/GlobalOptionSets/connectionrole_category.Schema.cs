
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities.GlobalOptionSets
{

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Category
    ///     (Russian - 1049): Категория
    /// 
    /// Description:
    ///     (English - United States - 1033): Categories for connection roles.
    ///     (Russian - 1049): Категории для ролей подключения.
    /// 
    /// OptionSet Name: connectionrole_category      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 1
    ///     AttributeName              DisplayName                 IsCustomizable    Behavior
    ///     connectionrole.category    Connection Role Category    True              IncludeSubcomponents
    /// ComponentType:   ConnectionRole (63)            Count: 33
    ///     Name                            IsCustomizable    Behavior
    ///     Account Manager                 True              IncludeSubcomponents
    ///     Associated Product              True              IncludeSubcomponents
    ///     Champion                        True              IncludeSubcomponents
    ///     Child                           True              IncludeSubcomponents
    ///     Colleague                       True              IncludeSubcomponents
    ///     Decision Maker                  True              IncludeSubcomponents
    ///     Delivery Professional           True              IncludeSubcomponents
    ///     Economic Buyer                  True              IncludeSubcomponents
    ///     Employee                        True              IncludeSubcomponents
    ///     Employer                        True              IncludeSubcomponents
    ///     End User                        True              IncludeSubcomponents
    ///     Former Employee                 True              IncludeSubcomponents
    ///     Former Employer                 True              IncludeSubcomponents
    ///     Friend                          True              IncludeSubcomponents
    ///     Industry Expert                 True              IncludeSubcomponents
    ///     Influencer                      True              IncludeSubcomponents
    ///     Knowledge Article               True              IncludeSubcomponents
    ///     Parent                          True              IncludeSubcomponents
    ///     Partner                         True              IncludeSubcomponents
    ///     Primary Article                 True              IncludeSubcomponents
    ///     Primary Case                    True              IncludeSubcomponents
    ///     Referral                        True              IncludeSubcomponents
    ///     Referred by                     True              IncludeSubcomponents
    ///     Related Article                 True              IncludeSubcomponents
    ///     Related case                    True              IncludeSubcomponents
    ///     Sales Professional              True              IncludeSubcomponents
    ///     Service Professional            True              IncludeSubcomponents
    ///     Spouse/Partner                  True              IncludeSubcomponents
    ///     Stakeholder                     True              IncludeSubcomponents
    ///     Technical Buyer                 True              IncludeSubcomponents
    ///     Technical Sales Professional    True              IncludeSubcomponents
    ///     Territory Default Pricelist     True              IncludeSubcomponents
    ///     Territory Manager               True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Category")]
    public enum connectionrole_category
    {
        ///<summary>
        /// 1
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Business
        ///     (Russian - 1049): Бизнес
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Business")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Business_1 = 1,

        ///<summary>
        /// 2
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Family
        ///     (Russian - 1049): Семья
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Family")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Family_2 = 2,

        ///<summary>
        /// 3
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Social
        ///     (Russian - 1049): Общественный
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Social")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Social_3 = 3,

        ///<summary>
        /// 4
        /// DisplayOrder: 4
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Sales
        ///     (Russian - 1049): Продажи
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Sales")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Sales_4 = 4,

        ///<summary>
        /// 5
        /// DisplayOrder: 5
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Other
        ///     (Russian - 1049): Прочее
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Other")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Other_5 = 5,

        ///<summary>
        /// 1000
        /// DisplayOrder: 6
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Stakeholder
        ///     (Russian - 1049): Заинтересованное лицо
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Stakeholder")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Stakeholder_1000 = 1000,

        ///<summary>
        /// 1001
        /// DisplayOrder: 7
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Sales Team
        ///     (Russian - 1049): ГРУППА ПРОДАЖ
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Sales Team")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Sales_Team_1001 = 1001,

        ///<summary>
        /// 1002
        /// DisplayOrder: 8
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Service
        ///     (Russian - 1049): Сервис
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Service")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Service_1002 = 1002,
    }
}