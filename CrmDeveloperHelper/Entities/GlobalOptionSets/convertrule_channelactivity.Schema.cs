
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities.GlobalOptionSets
{

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Channel Activities
    ///     (Russian - 1049): Действия каналов
    /// 
    /// Description:
    ///     (English - United States - 1033): Type of  channel activities.
    ///     (Russian - 1049): Тип действий каналов.
    /// 
    /// OptionSet Name: convertrule_channelactivity      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 2
    ///     AttributeName                             DisplayName       IsCustomizable    Behavior
    ///     channelpropertygroup.regardingtypecode    Regarding Type    False             IncludeSubcomponents
    ///     convertrule.sourcechanneltypecode         Source Type       False             IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Channel Activities")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum convertrule_channelactivity
    {
        ///<summary>
        /// 4210
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Phone Call
        ///     (Russian - 1049): Звонок
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Phone Call")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Phone_Call_4210 = 4210,

        ///<summary>
        /// 4202
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Email
        ///     (Russian - 1049): Электронная почта
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Email")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Email_4202 = 4202,

        ///<summary>
        /// 4201
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Appointment
        ///     (Russian - 1049): Встреча
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Appointment")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Appointment_4201 = 4201,

        ///<summary>
        /// 4212
        /// DisplayOrder: 4
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Task
        ///     (Russian - 1049): Задача
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Task")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Task_4212 = 4212,

        ///<summary>
        /// 4216
        /// DisplayOrder: 5
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Social Activity
        ///     (Russian - 1049): Действие социальной сети
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Social Activity")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Social_Activity_4216 = 4216,

        ///<summary>
        /// 4214
        /// DisplayOrder: 6
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Service Activity
        ///     (Russian - 1049): Действие сервиса
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Service Activity")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Service_Activity_4214 = 4214,
    }
}