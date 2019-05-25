
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities.GlobalOptionSets
{

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Select an activity type
    ///     (Russian - 1049): Выберите тип действия
    /// 
    /// OptionSet Name: activity_mailmergetypecode      IsCustomOptionSet: True
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Select an activity type")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum activity_mailmergetypecode
    {
        ///<summary>
        /// 4201
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Appointment
        ///     (Russian - 1049): Встреча
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Appointment")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Appointment_4201 = 4201,

        ///<summary>
        /// 4202
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Email
        ///     (Russian - 1049): Электронная почта
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Email")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Email_4202 = 4202,

        ///<summary>
        /// 4204
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Fax
        ///     (Russian - 1049): Факс
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Fax")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Fax_4204 = 4204,

        ///<summary>
        /// 4207
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Letter
        ///     (Russian - 1049): Письмо
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Letter")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Letter_4207 = 4207,

        ///<summary>
        /// 4210
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Phone Call
        ///     (Russian - 1049): Звонок
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Phone Call")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Phone_Call_4210 = 4210,

        ///<summary>
        /// 42020
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Email via Mail Merge
        ///     (Russian - 1049): Электронная почта через слияние почты
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Email via Mail Merge")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Email_via_Mail_Merge_42020 = 42020,

        ///<summary>
        /// 42040
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Fax via Mail Merge
        ///     (Russian - 1049): Факс через слияние
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Fax via Mail Merge")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Fax_via_Mail_Merge_42040 = 42040,

        ///<summary>
        /// 42070
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Letter via Mail Merge
        ///     (Russian - 1049): Письмо через слияние
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Letter via Mail Merge")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Letter_via_Mail_Merge_42070 = 42070,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Select an activity type
    ///     (Russian - 1049): Выберите тип действия
    /// 
    /// OptionSet Name: activity_typecode      IsCustomOptionSet: True
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Select an activity type")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum activity_typecode
    {
        ///<summary>
        /// 4201
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Appointment
        ///     (Russian - 1049): Встреча
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Appointment")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Appointment_4201 = 4201,

        ///<summary>
        /// 4202
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Email
        ///     (Russian - 1049): Электронная почта
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Email")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Email_4202 = 4202,

        ///<summary>
        /// 4204
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Fax
        ///     (Russian - 1049): Факс
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Fax")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Fax_4204 = 4204,

        ///<summary>
        /// 4207
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Letter
        ///     (Russian - 1049): Письмо
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Letter")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Letter_4207 = 4207,

        ///<summary>
        /// 4210
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Phone Call
        ///     (Russian - 1049): Звонок
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Phone Call")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Phone_Call_4210 = 4210,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Activity Type
    ///     (Russian - 1049): Тип действия
    /// 
    /// Description:
    ///     (English - United States - 1033): Type of activity.
    ///     (Russian - 1049): Тип действия.
    /// 
    /// OptionSet Name: activitypointer_activitytypecode      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 18
    ///     AttributeName                                  DisplayName      IsCustomizable    Behavior
    ///     activitypointer.activitytypecode               Activity Type    False             IncludeSubcomponents
    ///     appointment.activitytypecode                   Activity Type    True              IncludeSubcomponents
    ///     bulkoperation.activitytypecode                 Activity Type    True              IncludeSubcomponents
    ///     campaignactivity.activitytypecode              Activity Type    True              IncludeSubcomponents
    ///     campaignresponse.activitytypecode              Activity Type    True              IncludeSubcomponents
    ///     email.activitytypecode                         Activity Type    True              IncludeSubcomponents
    ///     fax.activitytypecode                           Activity Type    True              IncludeSubcomponents
    ///     incidentresolution.activitytypecode            Activity Type    True              IncludeSubcomponents
    ///     letter.activitytypecode                        Activity Type    True              IncludeSubcomponents
    ///     opportunityclose.activitytypecode              Activity Type    True              IncludeSubcomponents
    ///     orderclose.activitytypecode                    Activity Type    True              IncludeSubcomponents
    ///     phonecall.activitytypecode                     Activity Type    True              IncludeSubcomponents
    ///     quoteclose.activitytypecode                    Activity Type    True              IncludeSubcomponents
    ///     recurringappointmentmaster.activitytypecode    Activity Type    True              IncludeSubcomponents
    ///     serviceappointment.activitytypecode            Activity Type    True              IncludeSubcomponents
    ///     socialactivity.activitytypecode                Activity Type    True              IncludeSubcomponents
    ///     task.activitytypecode                          Activity Type    True              IncludeSubcomponents
    ///     untrackedemail.activitytypecode                Activity Type    True              IncludeSubcomponents
    ///</summary>
    // public enum activitypointer_activitytypecode

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Delivery Priority
    ///     (Russian - 1049): Приоритет доставки
    /// 
    /// Description:
    ///     (English - United States - 1033): Priority of delivery of the activity to the email server.
    ///     (Russian - 1049): Приоритет доставки операции на сервер электронной почты.
    /// 
    /// OptionSet Name: activitypointer_deliveryprioritycode      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 10
    ///     AttributeName                              DisplayName          IsCustomizable    Behavior
    ///     activitypointer.deliveryprioritycode       Delivery Priority    False             IncludeSubcomponents
    ///     bulkoperation.deliveryprioritycode         Delivery Priority    True              IncludeSubcomponents
    ///     campaignactivity.deliveryprioritycode      Delivery Priority    True              IncludeSubcomponents
    ///     campaignresponse.deliveryprioritycode      Delivery Priority    True              IncludeSubcomponents
    ///     email.deliveryprioritycode                 Delivery Priority    True              IncludeSubcomponents
    ///     incidentresolution.deliveryprioritycode    Delivery Priority    True              IncludeSubcomponents
    ///     opportunityclose.deliveryprioritycode      Delivery Priority    True              IncludeSubcomponents
    ///     orderclose.deliveryprioritycode            Delivery Priority    True              IncludeSubcomponents
    ///     quoteclose.deliveryprioritycode            Delivery Priority    True              IncludeSubcomponents
    ///     serviceappointment.deliveryprioritycode    Delivery Priority    True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Delivery Priority")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum activitypointer_deliveryprioritycode
    {
        ///<summary>
        /// 0
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Low
        ///     (Russian - 1049): Низкий
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Low")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Low_0 = 0,

        ///<summary>
        /// 1
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Normal
        ///     (Russian - 1049): Обычный
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Normal")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Normal_1 = 1,

        ///<summary>
        /// 2
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): High
        ///     (Russian - 1049): Высокий
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("High")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        High_2 = 2,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Add Marketing List
    ///     (Russian - 1049): Добавить маркетинговый список
    /// 
    /// Description:
    ///     (English - United States - 1033): Select where to add the marketing list
    ///     (Russian - 1049): Выберите, куда требуется добавить маркетинговый список
    /// 
    /// OptionSet Name: addlistcampaign      IsCustomOptionSet: True
    /// 
    /// ComponentType:   SystemForm (60)            Count: 1
    ///     EntityName    FormType    FormName            IsCustomizable    Behavior
    ///     none          Dialog      Confirm Addition    False             IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Add Marketing List")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum addlistcampaign
    {
        ///<summary>
        /// 0
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): To the campaign only.
        ///     (Russian - 1049): Только в кампанию.
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("To the campaign only.")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        To_the_campaign_only_0 = 0,

        ///<summary>
        /// 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): To the campaign and all undistributed campaign activities.
        ///     (Russian - 1049): В кампанию и все нераспределенные действия кампании.
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("To the campaign and all undistributed campaign activities.")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        To_the_campaign_and_all_undistributed_campaign_activities_1 = 1,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Bookable Resource Characteristic Type
    ///     (Russian - 1049): Тип характеристики резервируемого ресурса
    /// 
    /// Description:
    ///     (English - United States - 1033): Type of the characteristic, e.g. skill, certification.
    ///     (Russian - 1049): Тип характеристики, например навык или сертификат.
    /// 
    /// OptionSet Name: bookableresourcecharacteristictype      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 1
    ///     AttributeName                        DisplayName            IsCustomizable    Behavior
    ///     characteristic.characteristictype    Characteristic Type    True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Bookable Resource Characteristic Type")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum bookableresourcecharacteristictype
    {
        ///<summary>
        /// 1
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Skill
        ///     (Russian - 1049): Навык
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Skill")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Skill_1 = 1,

        ///<summary>
        /// 2
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Certification
        ///     (Russian - 1049): Сертификация
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Certification")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Certification_2 = 2,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Budget
    ///     (Russian - 1049): Бюджет
    /// 
    /// Description:
    ///     (English - United States - 1033): Do they have a budget?
    ///     (Russian - 1049): У них есть бюджет?
    /// 
    /// OptionSet Name: budgetstatus      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 2
    ///     AttributeName               DisplayName    IsCustomizable    Behavior
    ///     lead.budgetstatus           Budget         True              IncludeSubcomponents
    ///     opportunity.budgetstatus    Budget         True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Budget")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum budgetstatus
    {
        ///<summary>
        /// 0
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): No Committed Budget
        ///     (Russian - 1049): Нет выделенного бюджета
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("No Committed Budget")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        No_Committed_Budget_0 = 0,

        ///<summary>
        /// 1
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): May Buy
        ///     (Russian - 1049): Могут купить
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("May Buy")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        May_Buy_1 = 1,

        ///<summary>
        /// 2
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Can Buy
        ///     (Russian - 1049): Могут купить
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Can Buy")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Can_Buy_2 = 2,

        ///<summary>
        /// 3
        /// DisplayOrder: 4
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Will Buy
        ///     (Russian - 1049): Купят
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Will Buy")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Will_Buy_3 = 3,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Send Direct Email To
    /// 
    /// Description:
    ///     (English - United States - 1033): Select the records to send the direct email to
    /// 
    /// OptionSet Name: bulkemail_recipients      IsCustomOptionSet: True
    /// 
    /// ComponentType:   SystemForm (60)            Count: 1
    ///     EntityName    FormType    FormName            IsCustomizable    Behavior
    ///     none          Dialog      BulkEmail Dialog    False             IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Send Direct Email To")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum bulkemail_recipients
    {
        ///<summary>
        /// 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Selected records on current page
        /// 
        /// Description:
        ///     (English - United States - 1033): Send direct email only to the records you selected on this page.
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Selected records on current page")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Selected_records_on_current_page_1 = 1,

        ///<summary>
        /// 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): All records on current page
        /// 
        /// Description:
        ///     (English - United States - 1033): Send direct email to all the records on this page.
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("All records on current page")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        All_records_on_current_page_2 = 2,

        ///<summary>
        /// 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): All records on all pages
        /// 
        /// Description:
        ///     (English - United States - 1033): Send direct email to all the records on all the pages in the current view.
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("All records on all pages")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        All_records_on_all_pages_3 = 3,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Case Closure Preference
    ///     (Russian - 1049): Параметры закрытия обращений
    /// 
    /// Description:
    ///     (English - United States - 1033): Options to cascade closing cases
    ///     (Russian - 1049): Параметры каскадного закрытия обращений
    /// 
    /// OptionSet Name: cascadecaseclosurepreference      IsCustomOptionSet: True
    /// 
    /// ComponentType:   SystemForm (60)            Count: 1
    ///     EntityName    FormType    FormName                    IsCustomizable    Behavior
    ///     none          Dialog      Dialog for case settings    True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Case Closure Preference")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum cascadecaseclosurepreference
    {
        ///<summary>
        /// 0
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Close all child cases when parent case is closed
        ///     (Russian - 1049): Закрывать все дочерние обращения при закрытии родительского
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Close all child cases when parent case is closed")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Close_all_child_cases_when_parent_case_is_closed_0 = 0,

        ///<summary>
        /// 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Don't allow parent case closure until all child cases are closed
        ///     (Russian - 1049): Запретить закрытие родительского обращения, пока не будут закрыты все дочерние
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Don't allow parent case closure until all child cases are closed")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Don_t_allow_parent_case_closure_until_all_child_cases_are_closed_1 = 1,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Component State
    ///     (Russian - 1049): Состояние компонента
    /// 
    /// Description:
    ///     (English - United States - 1033): The state of this component.
    ///     (Russian - 1049): Состояние этого компонента.
    /// 
    /// OptionSet Name: componentstate      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 95
    ///     Default Description for Unknowned Solution Components:
    ///     0513aa19-b341-427e-b229-a681d79d6a1a    IncludeSubcomponents
    ///     9c64a506-22b6-484d-99d8-637001b3be7b    IncludeSubcomponents
    ///     ba7ad728-ef8c-45e7-a579-b51d9b2783d2    IncludeSubcomponents
    ///     AttributeName                                           DisplayName        IsCustomizable    Behavior
    ///     activitymimeattachment.componentstate                   Component State    True              IncludeSubcomponents
    ///     advancedsimilarityrule.componentstate                   Component State    False             IncludeSubcomponents
    ///     appconfig.componentstate                                Component State    False             IncludeSubcomponents
    ///     appconfiginstance.componentstate                        Component State    False             IncludeSubcomponents
    ///     appmodule.componentstate                                Component State    False             IncludeSubcomponents
    ///     appmoduleroles.componentstate                           Component State    False             IncludeSubcomponents
    ///     attributemap.componentstate                             Component State    False             IncludeSubcomponents
    ///     channelaccessprofile.componentstate                     Component State    False             IncludeSubcomponents
    ///     channelaccessprofileentityaccesslevel.componentstate    Component State    True              IncludeSubcomponents
    ///     channelaccessprofilerule.componentstate                 Component State    False             IncludeSubcomponents
    ///     channelaccessprofileruleitem.componentstate             Component State    False             IncludeSubcomponents
    ///     channelproperty.componentstate                          Component State    False             IncludeSubcomponents
    ///     channelpropertygroup.componentstate                     Component State    False             IncludeSubcomponents
    ///     columnmapping.componentstate                            Component State    False             IncludeSubcomponents
    ///     complexcontrol.componentstate                           Component State    False             IncludeSubcomponents
    ///     connectionrole.componentstate                           Component State    True              IncludeSubcomponents
    ///     contracttemplate.componentstate                         Component State    True              IncludeSubcomponents
    ///     convertrule.componentstate                              Component State    False             IncludeSubcomponents
    ///     convertruleitem.componentstate                          Component State    False             IncludeSubcomponents
    ///     customcontrol.componentstate                            Component State    False             IncludeSubcomponents
    ///     customcontroldefaultconfig.componentstate               Component State    False             IncludeSubcomponents
    ///     customcontrolresource.componentstate                    Component State    False             IncludeSubcomponents
    ///     dependencyfeature.componentstate                        Component State    True              IncludeSubcomponents
    ///     displaystring.componentstate                            Component State    False             IncludeSubcomponents
    ///     displaystringmap.componentstate                         Component State    False             IncludeSubcomponents
    ///     emailsignature.componentstate                           Component State    True              IncludeSubcomponents
    ///     entitydataprovider.componentstate                       Component State    False             IncludeSubcomponents
    ///     entitydatasource.componentstate                         Component State    False             IncludeSubcomponents
    ///     entitymap.componentstate                                Component State    False             IncludeSubcomponents
    ///     fieldpermission.componentstate                          Component State    False             IncludeSubcomponents
    ///     fieldsecurityprofile.componentstate                     Component State    False             IncludeSubcomponents
    ///     globalsearchconfiguration.componentstate                Component State    True              IncludeSubcomponents
    ///     hierarchyrule.componentstate                            Component State    False             IncludeSubcomponents
    ///     importentitymapping.componentstate                      Component State    False             IncludeSubcomponents
    ///     importmap.componentstate                                Component State    False             IncludeSubcomponents
    ///     kbarticletemplate.componentstate                        Component State    False             IncludeSubcomponents
    ///     knowledgesearchmodel.componentstate                     Component State    False             IncludeSubcomponents
    ///     lookupmapping.componentstate                            Component State    False             IncludeSubcomponents
    ///     mailmergetemplate.componentstate                        Component State    True              IncludeSubcomponents
    ///     mobileofflineprofile.componentstate                     Component State    False             IncludeSubcomponents
    ///     mobileofflineprofileitem.componentstate                 Component State    False             IncludeSubcomponents
    ///     mobileofflineprofileitemassociation.componentstate      Component State    False             IncludeSubcomponents
    ///     navigationsetting.componentstate                        Component State    False             IncludeSubcomponents
    ///     organizationui.componentstate                           Component State    False             IncludeSubcomponents
    ///     ownermapping.componentstate                             Component State    False             IncludeSubcomponents
    ///     picklistmapping.componentstate                          Component State    False             IncludeSubcomponents
    ///     pluginassembly.componentstate                           Component State    False             IncludeSubcomponents
    ///     plugintype.componentstate                               Component State    False             IncludeSubcomponents
    ///     privilege.componentstate                                Component State    False             IncludeSubcomponents
    ///     privilegeobjecttypecodes.componentstate                 Component State    False             IncludeSubcomponents
    ///     processtrigger.componentstate                           Component State    False             IncludeSubcomponents
    ///     report.componentstate                                   Component State    True              IncludeSubcomponents
    ///     reportcategory.componentstate                           Component State    True              IncludeSubcomponents
    ///     reportentity.componentstate                             Component State    False             IncludeSubcomponents
    ///     reportvisibility.componentstate                         Component State    False             IncludeSubcomponents
    ///     ribboncommand.componentstate                            Component State    False             IncludeSubcomponents
    ///     ribboncontextgroup.componentstate                       Component State    False             IncludeSubcomponents
    ///     ribboncustomization.componentstate                      Component State    False             IncludeSubcomponents
    ///     ribbondiff.componentstate                               Component State    False             IncludeSubcomponents
    ///     ribbonrule.componentstate                               Component State    False             IncludeSubcomponents
    ///     ribbontabtocommandmap.componentstate                    Component State    False             IncludeSubcomponents
    ///     role.componentstate                                     Component State    True              IncludeSubcomponents
    ///     roleprivileges.componentstate                           Component State    True              IncludeSubcomponents
    ///     routingrule.componentstate                              Component State    True              IncludeSubcomponents
    ///     routingruleitem.componentstate                          Component State    False             IncludeSubcomponents
    ///     savedquery.componentstate                               Component State    True              IncludeSubcomponents
    ///     savedqueryvisualization.componentstate                  Component State    True              IncludeSubcomponents
    ///     sdkmessage.componentstate                               Component State    False             IncludeSubcomponents
    ///     sdkmessagefilter.componentstate                         Component State    False             IncludeSubcomponents
    ///     sdkmessagepair.componentstate                           Component State    False             IncludeSubcomponents
    ///     sdkmessageprocessingstep.componentstate                 Component State    False             IncludeSubcomponents
    ///     sdkmessageprocessingstepimage.componentstate            Component State    False             IncludeSubcomponents
    ///     sdkmessagerequest.componentstate                        Component State    False             IncludeSubcomponents
    ///     sdkmessagerequestfield.componentstate                   Component State    False             IncludeSubcomponents
    ///     sdkmessageresponse.componentstate                       Component State    False             IncludeSubcomponents
    ///     sdkmessageresponsefield.componentstate                  Component State    False             IncludeSubcomponents
    ///     serviceendpoint.componentstate                          Component State    False             IncludeSubcomponents
    ///     similarityrule.componentstate                           Component State    False             IncludeSubcomponents
    ///     sitemap.componentstate                                  Component State    False             IncludeSubcomponents
    ///     sla.componentstate                                      Component State    True              IncludeSubcomponents
    ///     slaitem.componentstate                                  Component State    True              IncludeSubcomponents
    ///     syncattributemapping.componentstate                     Component State    False             IncludeSubcomponents
    ///     syncattributemappingprofile.componentstate              Component State    False             IncludeSubcomponents
    ///     systemform.componentstate                               Component State    False             IncludeSubcomponents
    ///     template.componentstate                                 Component State    True              IncludeSubcomponents
    ///     textanalyticsentitymapping.componentstate               Component State    False             IncludeSubcomponents
    ///     topicmodelconfiguration.componentstate                  Component State    False             IncludeSubcomponents
    ///     transformationmapping.componentstate                    Component State    False             IncludeSubcomponents
    ///     transformationparametermapping.componentstate           Component State    False             IncludeSubcomponents
    ///     webresource.componentstate                              Component State    False             IncludeSubcomponents
    ///     webwizard.componentstate                                Component State    False             IncludeSubcomponents
    ///     workflow.componentstate                                 Component State    True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Component State")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum componentstate
    {
        ///<summary>
        /// 0
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Published
        ///     (Russian - 1049): Опубликовано
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Published")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Published_0 = 0,

        ///<summary>
        /// 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Unpublished
        ///     (Russian - 1049): Неопубликованный
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Unpublished")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Unpublished_1 = 1,

        ///<summary>
        /// 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Deleted
        ///     (Russian - 1049): Удалено
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Deleted")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Deleted_2 = 2,

        ///<summary>
        /// 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Deleted Unpublished
        ///     (Russian - 1049): Удален неопубликованным
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Deleted Unpublished")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Deleted_Unpublished_3 = 3,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Component Type
    ///     (Russian - 1049): Тип компонента
    /// 
    /// Description:
    ///     (English - United States - 1033): All of the possible component types for solutions.
    ///     (Russian - 1049): Все возможные типы компонентов для решений.
    /// 
    /// OptionSet Name: componenttype      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 6
    ///     AttributeName                              DisplayName                         IsCustomizable    Behavior
    ///     dependency.dependentcomponenttype                                              False             IncludeSubcomponents
    ///     dependency.requiredcomponenttype                                               False             IncludeSubcomponents
    ///     dependencynode.componenttype               Type Code                           False             IncludeSubcomponents
    ///     invaliddependency.existingcomponenttype    Existing Object's Component Type    False             IncludeSubcomponents
    ///     invaliddependency.missingcomponenttype     Type Code                           False             IncludeSubcomponents
    ///     solutioncomponent.componenttype            Object Type Code                    False             IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Component Type")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum componenttype
    {
        ///<summary>
        /// 1
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Entity
        ///     (Russian - 1049): Сущность
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Entity")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Entity_1 = 1,

        ///<summary>
        /// 2
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Attribute
        ///     (Russian - 1049): Атрибут
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Attribute")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Attribute_2 = 2,

        ///<summary>
        /// 3
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Relationship
        ///     (Russian - 1049): Отношение
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Relationship")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Relationship_3 = 3,

        ///<summary>
        /// 4
        /// DisplayOrder: 4
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Attribute Picklist Value
        ///     (Russian - 1049): Значение поля выбора атрибута
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Attribute Picklist Value")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Attribute_Picklist_Value_4 = 4,

        ///<summary>
        /// 5
        /// DisplayOrder: 5
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Attribute Lookup Value
        ///     (Russian - 1049): Значение подстановки атрибута
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Attribute Lookup Value")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Attribute_Lookup_Value_5 = 5,

        ///<summary>
        /// 6
        /// DisplayOrder: 6
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): View Attribute
        ///     (Russian - 1049): Просмотр атрибута
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("View Attribute")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        View_Attribute_6 = 6,

        ///<summary>
        /// 7
        /// DisplayOrder: 7
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Localized Label
        ///     (Russian - 1049): Локализованная надпись
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Localized Label")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Localized_Label_7 = 7,

        ///<summary>
        /// 8
        /// DisplayOrder: 8
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Relationship Extra Condition
        ///     (Russian - 1049): Дополнительное условие отношения
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Relationship Extra Condition")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Relationship_Extra_Condition_8 = 8,

        ///<summary>
        /// 9
        /// DisplayOrder: 9
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Option Set
        ///     (Russian - 1049): Набор параметров
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Option Set")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Option_Set_9 = 9,

        ///<summary>
        /// 10
        /// DisplayOrder: 10
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Entity Relationship
        ///     (Russian - 1049): Отношение сущности
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Entity Relationship")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Entity_Relationship_10 = 10,

        ///<summary>
        /// 11
        /// DisplayOrder: 11
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Entity Relationship Role
        ///     (Russian - 1049): Роль отношения сущности
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Entity Relationship Role")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Entity_Relationship_Role_11 = 11,

        ///<summary>
        /// 12
        /// DisplayOrder: 12
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Entity Relationship Relationships
        ///     (Russian - 1049): Отношения отношения сущности
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Entity Relationship Relationships")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Entity_Relationship_Relationships_12 = 12,

        ///<summary>
        /// 13
        /// DisplayOrder: 13
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Managed Property
        ///     (Russian - 1049): Управляемое свойство
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Managed Property")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Managed_Property_13 = 13,

        ///<summary>
        /// 14
        /// DisplayOrder: 14
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Entity Key
        ///     (Russian - 1049): Ключ сущности
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Entity Key")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Entity_Key_14 = 14,

        ///<summary>
        /// 16
        /// DisplayOrder: 15
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Privilege
        ///     (Russian - 1049): Привилегия
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Privilege")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Privilege_16 = 16,

        ///<summary>
        /// 17
        /// DisplayOrder: 16
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): PrivilegeObjectTypeCode
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("PrivilegeObjectTypeCode")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        PrivilegeObjectTypeCode_17 = 17,

        ///<summary>
        /// 20
        /// DisplayOrder: 17
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Role
        ///     (Russian - 1049): Роль
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Role")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Role_20 = 20,

        ///<summary>
        /// 21
        /// DisplayOrder: 18
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Role Privilege
        ///     (Russian - 1049): Привилегия роли
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Role Privilege")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Role_Privilege_21 = 21,

        ///<summary>
        /// 22
        /// DisplayOrder: 19
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Display String
        ///     (Russian - 1049): Отображаемая строка
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Display String")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Display_String_22 = 22,

        ///<summary>
        /// 23
        /// DisplayOrder: 20
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Display String Map
        ///     (Russian - 1049): Сопоставление отображаемой строки
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Display String Map")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Display_String_Map_23 = 23,

        ///<summary>
        /// 24
        /// DisplayOrder: 21
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Form
        ///     (Russian - 1049): Форма
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Form")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Form_24 = 24,

        ///<summary>
        /// 25
        /// DisplayOrder: 22
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Organization
        ///     (Russian - 1049): Предприятие
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Organization")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Organization_25 = 25,

        ///<summary>
        /// 26
        /// DisplayOrder: 23
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Saved Query
        ///     (Russian - 1049): Сохраненный запрос
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Saved Query")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Saved_Query_26 = 26,

        ///<summary>
        /// 29
        /// DisplayOrder: 24
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Workflow
        ///     (Russian - 1049): Бизнес-процесс
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Workflow")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Workflow_29 = 29,

        ///<summary>
        /// 31
        /// DisplayOrder: 25
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Report
        ///     (Russian - 1049): Отчет
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Report")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Report_31 = 31,

        ///<summary>
        /// 32
        /// DisplayOrder: 26
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Report Entity
        ///     (Russian - 1049): Сущность отчета
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Report Entity")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Report_Entity_32 = 32,

        ///<summary>
        /// 33
        /// DisplayOrder: 27
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Report Category
        ///     (Russian - 1049): Категория отчета
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Report Category")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Report_Category_33 = 33,

        ///<summary>
        /// 34
        /// DisplayOrder: 28
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Report Visibility
        ///     (Russian - 1049): Отображение отчета
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Report Visibility")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Report_Visibility_34 = 34,

        ///<summary>
        /// 35
        /// DisplayOrder: 29
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Attachment
        ///     (Russian - 1049): Вложение
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Attachment")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Attachment_35 = 35,

        ///<summary>
        /// 36
        /// DisplayOrder: 30
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Email Template
        ///     (Russian - 1049): Шаблон электронной почты
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Email Template")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Email_Template_36 = 36,

        ///<summary>
        /// 37
        /// DisplayOrder: 31
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Contract Template
        ///     (Russian - 1049): Шаблон контракта
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Contract Template")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Contract_Template_37 = 37,

        ///<summary>
        /// 38
        /// DisplayOrder: 32
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): KB Article Template
        ///     (Russian - 1049): Шаблон статьи базы знаний
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("KB Article Template")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        KB_Article_Template_38 = 38,

        ///<summary>
        /// 39
        /// DisplayOrder: 33
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Mail Merge Template
        ///     (Russian - 1049): Шаблон слияния
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Mail Merge Template")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Mail_Merge_Template_39 = 39,

        ///<summary>
        /// 44
        /// DisplayOrder: 34
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Duplicate Rule
        ///     (Russian - 1049): Правило поиска дубликатов
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Duplicate Rule")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Duplicate_Rule_44 = 44,

        ///<summary>
        /// 45
        /// DisplayOrder: 35
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Duplicate Rule Condition
        ///     (Russian - 1049): Условие правила обнаружения повторяющихся данных
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Duplicate Rule Condition")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Duplicate_Rule_Condition_45 = 45,

        ///<summary>
        /// 46
        /// DisplayOrder: 36
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Entity Map
        ///     (Russian - 1049): Сопоставление сущностей
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Entity Map")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Entity_Map_46 = 46,

        ///<summary>
        /// 47
        /// DisplayOrder: 37
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Attribute Map
        ///     (Russian - 1049): Сопоставление атрибутов
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Attribute Map")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Attribute_Map_47 = 47,

        ///<summary>
        /// 48
        /// DisplayOrder: 38
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Ribbon Command
        ///     (Russian - 1049): Команда ленты
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Ribbon Command")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Ribbon_Command_48 = 48,

        ///<summary>
        /// 49
        /// DisplayOrder: 39
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Ribbon Context Group
        ///     (Russian - 1049): Контекстная группа ленты
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Ribbon Context Group")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Ribbon_Context_Group_49 = 49,

        ///<summary>
        /// 50
        /// DisplayOrder: 40
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Ribbon Customization
        ///     (Russian - 1049): Настройка ленты
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Ribbon Customization")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Ribbon_Customization_50 = 50,

        ///<summary>
        /// 52
        /// DisplayOrder: 41
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Ribbon Rule
        ///     (Russian - 1049): Правило ленты
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Ribbon Rule")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Ribbon_Rule_52 = 52,

        ///<summary>
        /// 53
        /// DisplayOrder: 42
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Ribbon Tab To Command Map
        ///     (Russian - 1049): Сопоставление вкладки ленты с командой
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Ribbon Tab To Command Map")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Ribbon_Tab_To_Command_Map_53 = 53,

        ///<summary>
        /// 55
        /// DisplayOrder: 43
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Ribbon Diff
        ///     (Russian - 1049): Различие ленты
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Ribbon Diff")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Ribbon_Diff_55 = 55,

        ///<summary>
        /// 59
        /// DisplayOrder: 44
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Saved Query Visualization
        ///     (Russian - 1049): Сохраненная визуализация запроса
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Saved Query Visualization")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Saved_Query_Visualization_59 = 59,

        ///<summary>
        /// 60
        /// DisplayOrder: 45
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): System Form
        ///     (Russian - 1049): Системная форма
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("System Form")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        System_Form_60 = 60,

        ///<summary>
        /// 61
        /// DisplayOrder: 46
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Web Resource
        ///     (Russian - 1049): Веб-ресурс
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Web Resource")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Web_Resource_61 = 61,

        ///<summary>
        /// 62
        /// DisplayOrder: 47
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Site Map
        ///     (Russian - 1049): Карта сайта
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Site Map")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Site_Map_62 = 62,

        ///<summary>
        /// 63
        /// DisplayOrder: 48
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Connection Role
        ///     (Russian - 1049): Роль подключения
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Connection Role")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Connection_Role_63 = 63,

        ///<summary>
        /// 64
        /// DisplayOrder: 49
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Complex Control
        ///     (Russian - 1049): Сложный элемент управления
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Complex Control")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Complex_Control_64 = 64,

        ///<summary>
        /// 70
        /// DisplayOrder: 50
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Field Security Profile
        ///     (Russian - 1049): Профиль безопасности поля
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Field Security Profile")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Field_Security_Profile_70 = 70,

        ///<summary>
        /// 71
        /// DisplayOrder: 51
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Field Permission
        ///     (Russian - 1049): Разрешение поля
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Field Permission")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Field_Permission_71 = 71,

        ///<summary>
        /// 90
        /// DisplayOrder: 52
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Plugin Type
        ///     (Russian - 1049): Тип подключаемого модуля
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Plugin Type")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Plugin_Type_90 = 90,

        ///<summary>
        /// 91
        /// DisplayOrder: 53
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Plugin Assembly
        ///     (Russian - 1049): Сборка подключаемого модуля
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Plugin Assembly")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Plugin_Assembly_91 = 91,

        ///<summary>
        /// 92
        /// DisplayOrder: 54
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): SDK Message Processing Step
        ///     (Russian - 1049): Шаг обработки сообщения SDK
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("SDK Message Processing Step")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SDK_Message_Processing_Step_92 = 92,

        ///<summary>
        /// 93
        /// DisplayOrder: 55
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): SDK Message Processing Step Image
        ///     (Russian - 1049): Образ шага обработки сообщения SDK
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("SDK Message Processing Step Image")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SDK_Message_Processing_Step_Image_93 = 93,

        ///<summary>
        /// 95
        /// DisplayOrder: 56
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Service Endpoint
        ///     (Russian - 1049): Конечная точка сервиса
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Service Endpoint")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Service_Endpoint_95 = 95,

        ///<summary>
        /// 150
        /// DisplayOrder: 57
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Routing Rule
        ///     (Russian - 1049): Правило маршрутизации
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Routing Rule")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Routing_Rule_150 = 150,

        ///<summary>
        /// 151
        /// DisplayOrder: 58
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Routing Rule Item
        ///     (Russian - 1049): Элемент правила маршрутизации
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Routing Rule Item")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Routing_Rule_Item_151 = 151,

        ///<summary>
        /// 152
        /// DisplayOrder: 59
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): SLA
        ///     (Russian - 1049): Соглашение об уровне обслуживания
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("SLA")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SLA_152 = 152,

        ///<summary>
        /// 153
        /// DisplayOrder: 60
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): SLA Item
        ///     (Russian - 1049): Элемент SLA
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("SLA Item")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SLA_Item_153 = 153,

        ///<summary>
        /// 154
        /// DisplayOrder: 61
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Convert Rule
        ///     (Russian - 1049): Правило преобразования
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Convert Rule")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Convert_Rule_154 = 154,

        ///<summary>
        /// 155
        /// DisplayOrder: 62
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Convert Rule Item
        ///     (Russian - 1049): Элемент правила преобразования
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Convert Rule Item")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Convert_Rule_Item_155 = 155,

        ///<summary>
        /// 65
        /// DisplayOrder: 63
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Hierarchy Rule
        ///     (Russian - 1049): Правило иерархии
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Hierarchy Rule")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Hierarchy_Rule_65 = 65,

        ///<summary>
        /// 161
        /// DisplayOrder: 64
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Mobile Offline Profile
        ///     (Russian - 1049): Профиль Mobile Offline
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Mobile Offline Profile")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Mobile_Offline_Profile_161 = 161,

        ///<summary>
        /// 162
        /// DisplayOrder: 65
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Mobile Offline Profile Item
        ///     (Russian - 1049): Элемент профиля Mobile Offline
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Mobile Offline Profile Item")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Mobile_Offline_Profile_Item_162 = 162,

        ///<summary>
        /// 165
        /// DisplayOrder: 66
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Similarity Rule
        ///     (Russian - 1049): Правило подобия
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Similarity Rule")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Similarity_Rule_165 = 165,

        ///<summary>
        /// 66
        /// DisplayOrder: 67
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Custom Control
        ///     (Russian - 1049): Пользовательский элемент управления
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Custom Control")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Custom_Control_66 = 66,

        ///<summary>
        /// 68
        /// DisplayOrder: 68
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Custom Control Default Config
        ///     (Russian - 1049): Конфигурация пользовательского элемента управления по умолчанию
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Custom Control Default Config")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Custom_Control_Default_Config_68 = 68,

        ///<summary>
        /// 166
        /// DisplayOrder: 69
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Data Source Mapping
        ///     (Russian - 1049): Сопоставление источника данных
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Data Source Mapping")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Data_Source_Mapping_166 = 166,

        ///<summary>
        /// 201
        /// DisplayOrder: 70
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): SDKMessage
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("SDKMessage")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SDKMessage_201 = 201,

        ///<summary>
        /// 202
        /// DisplayOrder: 71
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): SDKMessageFilter
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("SDKMessageFilter")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SDKMessageFilter_202 = 202,

        ///<summary>
        /// 203
        /// DisplayOrder: 72
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): SdkMessagePair
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("SdkMessagePair")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SdkMessagePair_203 = 203,

        ///<summary>
        /// 204
        /// DisplayOrder: 73
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): SdkMessageRequest
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("SdkMessageRequest")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SdkMessageRequest_204 = 204,

        ///<summary>
        /// 205
        /// DisplayOrder: 74
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): SdkMessageRequestField
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("SdkMessageRequestField")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SdkMessageRequestField_205 = 205,

        ///<summary>
        /// 206
        /// DisplayOrder: 75
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): SdkMessageResponse
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("SdkMessageResponse")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SdkMessageResponse_206 = 206,

        ///<summary>
        /// 207
        /// DisplayOrder: 76
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): SdkMessageResponseField
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("SdkMessageResponseField")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SdkMessageResponseField_207 = 207,

        ///<summary>
        /// 210
        /// DisplayOrder: 77
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): WebWizard
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("WebWizard")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        WebWizard_210 = 210,

        ///<summary>
        /// 18
        /// DisplayOrder: 78
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Index
        ///     (Russian - 1049): Индекс
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Index")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Index_18 = 18,

        ///<summary>
        /// 208
        /// DisplayOrder: 79
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Import Map
        ///     (Russian - 1049): Сопоставление для импорта
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Import Map")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Import_Map_208 = 208,
    }

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
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
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

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Campaign response deactivate status
    ///     (Russian - 1049): Состояние деактивирования отклика от кампании
    /// 
    /// Description:
    ///     (English - United States - 1033): Select the status for the deactivated campaign response
    ///     (Russian - 1049): Выберите состояние для отклика от деактивированной кампании
    /// 
    /// OptionSet Name: convert_campaign_response_deactivate_status      IsCustomOptionSet: True
    /// 
    /// ComponentType:   SystemForm (60)            Count: 1
    ///     EntityName    FormType    FormName                            IsCustomizable    Behavior
    ///     none          Dialog      Convert Campaign Response Dialog    False             IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Campaign response deactivate status")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum convert_campaign_response_deactivate_status
    {
        ///<summary>
        /// 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Closed
        ///     (Russian - 1049): Закрыто
        /// 
        /// Description:
        ///     (English - United States - 1033): Closed Label
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Closed")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Closed_1 = 1,

        ///<summary>
        /// 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Cancelled
        ///     (Russian - 1049): Отменено
        /// 
        /// Description:
        ///     (English - United States - 1033): Cancelled Label
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Cancelled")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Cancelled_2 = 2,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Campaign response convert option
    ///     (Russian - 1049): Параметр преобразования отклика от кампании
    /// 
    /// Description:
    ///     (English - United States - 1033): Select the option for converting the campaign response
    ///     (Russian - 1049): Выберите вариант для преобразования отклика от кампании
    /// 
    /// OptionSet Name: convert_campaign_response_option      IsCustomOptionSet: True
    /// 
    /// ComponentType:   SystemForm (60)            Count: 1
    ///     EntityName    FormType    FormName                            IsCustomizable    Behavior
    ///     none          Dialog      Convert Campaign Response Dialog    False             IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Campaign response convert option")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum convert_campaign_response_option
    {
        ///<summary>
        /// 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Create a lead
        ///     (Russian - 1049): Создать интерес
        /// 
        /// Description:
        ///     (English - United States - 1033): Create lead label
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Create a lead")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Create_a_lead_1 = 1,

        ///<summary>
        /// 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Convert to an existing lead
        ///     (Russian - 1049): Преобразовать в существующий интерес
        /// 
        /// Description:
        ///     (English - United States - 1033): Convert to existing lead label
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Convert to an existing lead")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Convert_to_an_existing_lead_2 = 2,

        ///<summary>
        /// 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Create a quote, order, or opportunity for an account or contact
        ///     (Russian - 1049): Создать предложение с расценками, заказ или возможную сделку для организации или контакта
        /// 
        /// Description:
        ///     (English - United States - 1033): Create sales entity label
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Create a quote, order, or opportunity for an account or contact")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Create_a_quote_order_or_opportunity_for_an_account_or_contact_3 = 3,

        ///<summary>
        /// 4
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Close response
        ///     (Russian - 1049): Закрыть отклик от кампании
        /// 
        /// Description:
        ///     (English - United States - 1033): Close response label
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Close response")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Close_response_4 = 4,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Campaign response convert to sales entity type
    ///     (Russian - 1049): Преобразование отклика от кампании в тип сущности продаж
    /// 
    /// Description:
    ///     (English - United States - 1033): Select the type of sales entity which the campaign response should be converted to
    ///     (Russian - 1049): Выберите тип сущности продаж, в который следует преобразовать отклик от кампании
    /// 
    /// OptionSet Name: convert_campaign_response_sales_entity_type      IsCustomOptionSet: True
    /// 
    /// ComponentType:   SystemForm (60)            Count: 1
    ///     EntityName    FormType    FormName                            IsCustomizable    Behavior
    ///     none          Dialog      Convert Campaign Response Dialog    False             IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Campaign response convert to sales entity type")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum convert_campaign_response_sales_entity_type
    {
        ///<summary>
        /// 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Opportunity
        ///     (Russian - 1049): Возможная сделка
        /// 
        /// Description:
        ///     (English - United States - 1033): Opportunity Label
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Opportunity")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Opportunity_1 = 1,

        ///<summary>
        /// 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Order
        ///     (Russian - 1049): Заказ
        /// 
        /// Description:
        ///     (English - United States - 1033): Order Label
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Order")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Order_2 = 2,

        ///<summary>
        /// 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Quote
        ///     (Russian - 1049): Предложение с расценками
        /// 
        /// Description:
        ///     (English - United States - 1033): Quote Label
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Quote")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Quote_3 = 3,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Campaign response convert to lead disqualify status
    ///     (Russian - 1049): Состояние дисквалификации преобразования отклика от кампании в интерес
    /// 
    /// Description:
    ///     (English - United States - 1033): Select the status for the disqualified lead
    ///     (Russian - 1049): Выберите состояние для дисквалифицированного интереса
    /// 
    /// OptionSet Name: convert_campaign_response_to_lead_disqualify_status      IsCustomOptionSet: True
    /// 
    /// ComponentType:   SystemForm (60)            Count: 1
    ///     EntityName    FormType    FormName                                    IsCustomizable    Behavior
    ///     none          Dialog      Convert Campaign Response To Lead Dialog    False             IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Campaign response convert to lead disqualify status")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum convert_campaign_response_to_lead_disqualify_status
    {
        ///<summary>
        /// 4
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Lost
        ///     (Russian - 1049): Потерян
        /// 
        /// Description:
        ///     (English - United States - 1033): Lost Label
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Lost")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Lost_4 = 4,

        ///<summary>
        /// 5
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Cannot Contact
        ///     (Russian - 1049): Невозможно связаться
        /// 
        /// Description:
        ///     (English - United States - 1033): Cannot Contact Label
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Cannot Contact")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Cannot_Contact_5 = 5,

        ///<summary>
        /// 6
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): No Longer Interested
        ///     (Russian - 1049): Больше не интересуется
        /// 
        /// Description:
        ///     (English - United States - 1033): No Longer Interested Label
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("No Longer Interested")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        No_Longer_Interested_6 = 6,

        ///<summary>
        /// 7
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Canceled
        ///     (Russian - 1049): Отменен
        /// 
        /// Description:
        ///     (English - United States - 1033): Canceled Label
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Canceled")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Canceled_7 = 7,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Campaign response convert to lead option
    ///     (Russian - 1049): Параметр преобразования отклика от кампании в интерес
    /// 
    /// Description:
    ///     (English - United States - 1033): Select the option for converting the campaign response to a lead
    ///     (Russian - 1049): Выберите параметр для преобразования отклика от кампании в интерес
    /// 
    /// OptionSet Name: convert_campaign_response_to_lead_option      IsCustomOptionSet: True
    /// 
    /// ComponentType:   SystemForm (60)            Count: 1
    ///     EntityName    FormType    FormName                                    IsCustomizable    Behavior
    ///     none          Dialog      Convert Campaign Response To Lead Dialog    False             IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Campaign response convert to lead option")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum convert_campaign_response_to_lead_option
    {
        ///<summary>
        /// 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Qualify and convert into the following records
        ///     (Russian - 1049): Квалифицировать и преобразовать в записи
        /// 
        /// Description:
        ///     (English - United States - 1033): Qualify Label
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Qualify and convert into the following records")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Qualify_and_convert_into_the_following_records_1 = 1,

        ///<summary>
        /// 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Disqualify
        ///     (Russian - 1049): Дисквалифицировать
        /// 
        /// Description:
        ///     (English - United States - 1033): Disqualify Label
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Disqualify")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Disqualify_2 = 2,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Campaign response convert to lead qualify status
    ///     (Russian - 1049): Состояние квалификации преобразования отклика от кампании в интерес
    /// 
    /// Description:
    ///     (English - United States - 1033): Select the status for the qualified lead
    ///     (Russian - 1049): Выберите состояние для квалифицированного интереса
    /// 
    /// OptionSet Name: convert_campaign_response_to_lead_qualify_status      IsCustomOptionSet: True
    /// 
    /// ComponentType:   SystemForm (60)            Count: 1
    ///     EntityName    FormType    FormName                                    IsCustomizable    Behavior
    ///     none          Dialog      Convert Campaign Response To Lead Dialog    False             IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Campaign response convert to lead qualify status")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum convert_campaign_response_to_lead_qualify_status
    {
        ///<summary>
        /// 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Qualified
        ///     (Russian - 1049): Квалифицированный
        /// 
        /// Description:
        ///     (English - United States - 1033): Qualified Label
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Qualified")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Qualified_3 = 3,
    }

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

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Authentication Protocol
    ///     (Russian - 1049): Протокол проверки подлинности
    /// 
    /// Description:
    ///     (English - United States - 1033): Authentication protocol used when connecting to the email server.
    ///     (Russian - 1049): Протокол проверки подлинности, используемый при подключении к серверу электронной почты.
    /// 
    /// OptionSet Name: emailserverprofile_authenticationprotocol      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 2
    ///     AttributeName                                        DisplayName                         IsCustomizable    Behavior
    ///     emailserverprofile.incomingauthenticationprotocol    Incoming Authentication Protocol    True              IncludeSubcomponents
    ///     emailserverprofile.outgoingauthenticationprotocol    Outgoing Authentication Protocol    True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Authentication Protocol")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum emailserverprofile_authenticationprotocol
    {
        ///<summary>
        /// 0
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Auto Detect
        ///     (Russian - 1049): Автоматическое обнаружение
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Auto Detect")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Auto_Detect_0 = 0,

        ///<summary>
        /// 1
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Negotiate
        ///     (Russian - 1049): Согласовывать
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Negotiate")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Negotiate_1 = 1,

        ///<summary>
        /// 2
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): NTLM
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("NTLM")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        NTLM_2 = 2,

        ///<summary>
        /// 3
        /// DisplayOrder: 4
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Basic
        ///     (Russian - 1049): Базовая
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Basic")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Basic_3 = 3,
    }

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

    ///<summary>
    /// OptionSet Name: flipswitch_options      IsCustomOptionSet: False
    /// 
    /// ComponentType:   SystemForm (60)            Count: 1
    ///     EntityName    FormType    FormName                  IsCustomizable    Behavior
    ///     none          Dialog      Device Settings Dialog    False             IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum flipswitch_options
    {
        ///<summary>
        /// 0
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Off
        ///     (Russian - 1049): Выкл.
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Off")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Off_0 = 0,

        ///<summary>
        /// 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): On
        ///     (Russian - 1049): Вкл.
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("On")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        On_1 = 1,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Multi Select Picklist
    /// 
    /// OptionSet Name: gbc_multiselect      IsCustomOptionSet: True
    /// 
    /// ComponentType:   Attribute (2)            Count: 2
    ///     AttributeName                              DisplayName              IsCustomizable    Behavior
    ///     account.gbc_multiselectpicklist            Multi Select Picklist    True              IncludeSubcomponents
    ///     gbc_entity_test.gbc_multiselectpicklist    Multi Select Picklist    True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Multi Select Picklist")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum gbc_multiselect
    {
        ///<summary>
        /// 865240000
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): One
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("One")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        One_865240000 = 865240000,

        ///<summary>
        /// 865240001
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Two
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Two")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Two_865240001 = 865240001,

        ///<summary>
        /// 865240002
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Three
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Three")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Three_865240002 = 865240002,

        ///<summary>
        /// 865240003
        /// DisplayOrder: 4
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Four
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Four")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Four_865240003 = 865240003,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Fiscal Period
    ///     (Russian - 1049): Финансовый период
    /// 
    /// Description:
    ///     (English - United States - 1033): Fiscal Period of Goal
    ///     (Russian - 1049): Финансовый период цели
    /// 
    /// OptionSet Name: goal_fiscalperiod      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 1
    ///     AttributeName        DisplayName      IsCustomizable    Behavior
    ///     goal.fiscalperiod    Fiscal Period    True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Fiscal Period")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum goal_fiscalperiod
    {
        ///<summary>
        /// 1
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Quarter 1
        ///     (Russian - 1049): Квартал 1
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Quarter 1")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Quarter_1_1 = 1,

        ///<summary>
        /// 2
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Quarter 2
        ///     (Russian - 1049): Квартал 2
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Quarter 2")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Quarter_2_2 = 2,

        ///<summary>
        /// 3
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Quarter 3
        ///     (Russian - 1049): Квартал 3
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Quarter 3")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Quarter_3_3 = 3,

        ///<summary>
        /// 4
        /// DisplayOrder: 4
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Quarter 4
        ///     (Russian - 1049): Квартал 4
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Quarter 4")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Quarter_4_4 = 4,

        ///<summary>
        /// 101
        /// DisplayOrder: 5
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): January
        ///     (Russian - 1049): Январь
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("January")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        January_101 = 101,

        ///<summary>
        /// 102
        /// DisplayOrder: 6
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): February
        ///     (Russian - 1049): Февраль
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("February")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        February_102 = 102,

        ///<summary>
        /// 103
        /// DisplayOrder: 7
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): March
        ///     (Russian - 1049): Март
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("March")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        March_103 = 103,

        ///<summary>
        /// 104
        /// DisplayOrder: 8
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): April
        ///     (Russian - 1049): Апрель
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("April")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        April_104 = 104,

        ///<summary>
        /// 105
        /// DisplayOrder: 9
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): May
        ///     (Russian - 1049): Май
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("May")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        May_105 = 105,

        ///<summary>
        /// 106
        /// DisplayOrder: 10
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): June
        ///     (Russian - 1049): Июнь
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("June")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        June_106 = 106,

        ///<summary>
        /// 107
        /// DisplayOrder: 11
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): July
        ///     (Russian - 1049): Июль
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("July")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        July_107 = 107,

        ///<summary>
        /// 108
        /// DisplayOrder: 12
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): August
        ///     (Russian - 1049): Август
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("August")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        August_108 = 108,

        ///<summary>
        /// 109
        /// DisplayOrder: 13
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): September
        ///     (Russian - 1049): Сентябрь
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("September")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        September_109 = 109,

        ///<summary>
        /// 110
        /// DisplayOrder: 14
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): October
        ///     (Russian - 1049): Октябрь
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("October")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        October_110 = 110,

        ///<summary>
        /// 111
        /// DisplayOrder: 15
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): November
        ///     (Russian - 1049): Ноябрь
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("November")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        November_111 = 111,

        ///<summary>
        /// 112
        /// DisplayOrder: 16
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): December
        ///     (Russian - 1049): Декабрь
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("December")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        December_112 = 112,

        ///<summary>
        /// 201
        /// DisplayOrder: 17
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Semester 1
        ///     (Russian - 1049): Семестр 1
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Semester 1")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Semester_1_201 = 201,

        ///<summary>
        /// 202
        /// DisplayOrder: 18
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Semester 2
        ///     (Russian - 1049): Семестр 2
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Semester 2")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Semester_2_202 = 202,

        ///<summary>
        /// 301
        /// DisplayOrder: 19
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Annual
        ///     (Russian - 1049): Годовой
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Annual")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Annual_301 = 301,

        ///<summary>
        /// 401
        /// DisplayOrder: 20
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): P1
        ///     (Russian - 1049): П1
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("P1")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        P1_401 = 401,

        ///<summary>
        /// 402
        /// DisplayOrder: 21
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): P2
        ///     (Russian - 1049): П2
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("P2")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        P2_402 = 402,

        ///<summary>
        /// 403
        /// DisplayOrder: 22
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): P3
        ///     (Russian - 1049): П3
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("P3")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        P3_403 = 403,

        ///<summary>
        /// 404
        /// DisplayOrder: 23
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): P4
        ///     (Russian - 1049): П4
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("P4")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        P4_404 = 404,

        ///<summary>
        /// 405
        /// DisplayOrder: 24
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): P5
        ///     (Russian - 1049): П5
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("P5")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        P5_405 = 405,

        ///<summary>
        /// 406
        /// DisplayOrder: 25
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): P6
        ///     (Russian - 1049): П6
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("P6")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        P6_406 = 406,

        ///<summary>
        /// 407
        /// DisplayOrder: 26
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): P7
        ///     (Russian - 1049): П7
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("P7")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        P7_407 = 407,

        ///<summary>
        /// 408
        /// DisplayOrder: 27
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): P8
        ///     (Russian - 1049): П8
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("P8")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        P8_408 = 408,

        ///<summary>
        /// 409
        /// DisplayOrder: 28
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): P9
        ///     (Russian - 1049): П9
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("P9")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        P9_409 = 409,

        ///<summary>
        /// 410
        /// DisplayOrder: 29
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): P10
        ///     (Russian - 1049): П10
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("P10")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        P10_410 = 410,

        ///<summary>
        /// 411
        /// DisplayOrder: 30
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): P11
        ///     (Russian - 1049): П11
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("P11")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        P11_411 = 411,

        ///<summary>
        /// 412
        /// DisplayOrder: 31
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): P12
        ///     (Russian - 1049): П12
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("P12")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        P12_412 = 412,

        ///<summary>
        /// 413
        /// DisplayOrder: 32
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): P13
        ///     (Russian - 1049): П13
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("P13")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        P13_413 = 413,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Fiscal Year
    ///     (Russian - 1049): Финансовый год
    /// 
    /// Description:
    ///     (English - United States - 1033): Fiscal Year of Goal
    ///     (Russian - 1049): Финансовый год цели
    /// 
    /// OptionSet Name: goal_fiscalyear      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 1
    ///     AttributeName      DisplayName    IsCustomizable    Behavior
    ///     goal.fiscalyear    Fiscal Year    True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Fiscal Year")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum goal_fiscalyear
    {
        ///<summary>
        /// 2038
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2038
        ///     (Russian - 1049): ФГ2038
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2038")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2038_2038 = 2038,

        ///<summary>
        /// 2037
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2037
        ///     (Russian - 1049): ФГ2037
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2037")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2037_2037 = 2037,

        ///<summary>
        /// 2036
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2036
        ///     (Russian - 1049): ФГ2036
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2036")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2036_2036 = 2036,

        ///<summary>
        /// 2035
        /// DisplayOrder: 4
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2035
        ///     (Russian - 1049): ФГ2035
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2035")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2035_2035 = 2035,

        ///<summary>
        /// 2034
        /// DisplayOrder: 5
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2034
        ///     (Russian - 1049): ФГ2034
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2034")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2034_2034 = 2034,

        ///<summary>
        /// 2033
        /// DisplayOrder: 6
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2033
        ///     (Russian - 1049): ФГ2033
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2033")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2033_2033 = 2033,

        ///<summary>
        /// 2032
        /// DisplayOrder: 7
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2032
        ///     (Russian - 1049): ФГ2032
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2032")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2032_2032 = 2032,

        ///<summary>
        /// 2031
        /// DisplayOrder: 8
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2031
        ///     (Russian - 1049): ФГ2031
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2031")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2031_2031 = 2031,

        ///<summary>
        /// 2030
        /// DisplayOrder: 9
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2030
        ///     (Russian - 1049): ФГ2030
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2030")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2030_2030 = 2030,

        ///<summary>
        /// 2029
        /// DisplayOrder: 10
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2029
        ///     (Russian - 1049): ФГ2029
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2029")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2029_2029 = 2029,

        ///<summary>
        /// 2028
        /// DisplayOrder: 11
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2028
        ///     (Russian - 1049): ФГ2028
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2028")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2028_2028 = 2028,

        ///<summary>
        /// 2027
        /// DisplayOrder: 12
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2027
        ///     (Russian - 1049): ФГ2027
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2027")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2027_2027 = 2027,

        ///<summary>
        /// 2026
        /// DisplayOrder: 13
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2026
        ///     (Russian - 1049): ФГ2026
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2026")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2026_2026 = 2026,

        ///<summary>
        /// 2025
        /// DisplayOrder: 14
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2025
        ///     (Russian - 1049): ФГ2025
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2025")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2025_2025 = 2025,

        ///<summary>
        /// 2024
        /// DisplayOrder: 15
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2024
        ///     (Russian - 1049): ФГ2024
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2024")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2024_2024 = 2024,

        ///<summary>
        /// 2023
        /// DisplayOrder: 16
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2023
        ///     (Russian - 1049): ФГ2023
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2023")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2023_2023 = 2023,

        ///<summary>
        /// 2022
        /// DisplayOrder: 17
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2022
        ///     (Russian - 1049): ФГ2022
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2022")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2022_2022 = 2022,

        ///<summary>
        /// 2021
        /// DisplayOrder: 18
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2021
        ///     (Russian - 1049): ФГ2021
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2021")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2021_2021 = 2021,

        ///<summary>
        /// 2020
        /// DisplayOrder: 19
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2020
        ///     (Russian - 1049): ФГ2020
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2020")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2020_2020 = 2020,

        ///<summary>
        /// 2019
        /// DisplayOrder: 20
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2019
        ///     (Russian - 1049): ФГ2019
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2019")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2019_2019 = 2019,

        ///<summary>
        /// 2018
        /// DisplayOrder: 21
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2018
        ///     (Russian - 1049): ФГ2018
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2018")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2018_2018 = 2018,

        ///<summary>
        /// 2017
        /// DisplayOrder: 22
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2017
        ///     (Russian - 1049): ФГ2017
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2017")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2017_2017 = 2017,

        ///<summary>
        /// 2016
        /// DisplayOrder: 23
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2016
        ///     (Russian - 1049): ФГ2016
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2016")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2016_2016 = 2016,

        ///<summary>
        /// 2015
        /// DisplayOrder: 24
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2015
        ///     (Russian - 1049): ФГ2015
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2015")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2015_2015 = 2015,

        ///<summary>
        /// 2014
        /// DisplayOrder: 25
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2014
        ///     (Russian - 1049): ФГ2014
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2014")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2014_2014 = 2014,

        ///<summary>
        /// 2013
        /// DisplayOrder: 26
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2013
        ///     (Russian - 1049): ФГ2013
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2013")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2013_2013 = 2013,

        ///<summary>
        /// 2012
        /// DisplayOrder: 27
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2012
        ///     (Russian - 1049): ФГ2012
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2012")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2012_2012 = 2012,

        ///<summary>
        /// 2011
        /// DisplayOrder: 28
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2011
        ///     (Russian - 1049): ФГ2011
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2011")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2011_2011 = 2011,

        ///<summary>
        /// 2010
        /// DisplayOrder: 29
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2010
        ///     (Russian - 1049): ФГ2010
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2010")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2010_2010 = 2010,

        ///<summary>
        /// 2009
        /// DisplayOrder: 30
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2009
        ///     (Russian - 1049): ФГ2009
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2009")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2009_2009 = 2009,

        ///<summary>
        /// 2008
        /// DisplayOrder: 31
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2008
        ///     (Russian - 1049): ФГ2008
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2008")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2008_2008 = 2008,

        ///<summary>
        /// 2007
        /// DisplayOrder: 32
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2007
        ///     (Russian - 1049): ФГ2007
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2007")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2007_2007 = 2007,

        ///<summary>
        /// 2006
        /// DisplayOrder: 33
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2006
        ///     (Russian - 1049): ФГ2006
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2006")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2006_2006 = 2006,

        ///<summary>
        /// 2005
        /// DisplayOrder: 34
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2005
        ///     (Russian - 1049): ФГ2005
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2005")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2005_2005 = 2005,

        ///<summary>
        /// 2004
        /// DisplayOrder: 35
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2004
        ///     (Russian - 1049): ФГ2004
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2004")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2004_2004 = 2004,

        ///<summary>
        /// 2003
        /// DisplayOrder: 36
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2003
        ///     (Russian - 1049): ФГ2003
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2003")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2003_2003 = 2003,

        ///<summary>
        /// 2002
        /// DisplayOrder: 37
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2002
        ///     (Russian - 1049): ФГ2002
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2002")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2002_2002 = 2002,

        ///<summary>
        /// 2001
        /// DisplayOrder: 38
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2001
        ///     (Russian - 1049): ФГ2001
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2001")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2001_2001 = 2001,

        ///<summary>
        /// 2000
        /// DisplayOrder: 39
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY2000
        ///     (Russian - 1049): ФГ2000
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY2000")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY2000_2000 = 2000,

        ///<summary>
        /// 1999
        /// DisplayOrder: 40
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1999
        ///     (Russian - 1049): ФГ1999
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1999")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1999_1999 = 1999,

        ///<summary>
        /// 1998
        /// DisplayOrder: 41
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1998
        ///     (Russian - 1049): ФГ1998
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1998")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1998_1998 = 1998,

        ///<summary>
        /// 1997
        /// DisplayOrder: 42
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1997
        ///     (Russian - 1049): ФГ1997
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1997")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1997_1997 = 1997,

        ///<summary>
        /// 1996
        /// DisplayOrder: 43
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1996
        ///     (Russian - 1049): ФГ1996
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1996")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1996_1996 = 1996,

        ///<summary>
        /// 1995
        /// DisplayOrder: 44
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1995
        ///     (Russian - 1049): ФГ1995
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1995")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1995_1995 = 1995,

        ///<summary>
        /// 1994
        /// DisplayOrder: 45
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1994
        ///     (Russian - 1049): ФГ1994
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1994")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1994_1994 = 1994,

        ///<summary>
        /// 1993
        /// DisplayOrder: 46
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1993
        ///     (Russian - 1049): ФГ1993
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1993")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1993_1993 = 1993,

        ///<summary>
        /// 1992
        /// DisplayOrder: 47
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1992
        ///     (Russian - 1049): ФГ1992
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1992")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1992_1992 = 1992,

        ///<summary>
        /// 1991
        /// DisplayOrder: 48
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1991
        ///     (Russian - 1049): ФГ1991
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1991")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1991_1991 = 1991,

        ///<summary>
        /// 1990
        /// DisplayOrder: 49
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1990
        ///     (Russian - 1049): ФГ1990
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1990")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1990_1990 = 1990,

        ///<summary>
        /// 1989
        /// DisplayOrder: 50
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1989
        ///     (Russian - 1049): ФГ1989
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1989")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1989_1989 = 1989,

        ///<summary>
        /// 1988
        /// DisplayOrder: 51
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1988
        ///     (Russian - 1049): ФГ1988
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1988")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1988_1988 = 1988,

        ///<summary>
        /// 1987
        /// DisplayOrder: 52
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1987
        ///     (Russian - 1049): ФГ1987
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1987")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1987_1987 = 1987,

        ///<summary>
        /// 1986
        /// DisplayOrder: 53
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1986
        ///     (Russian - 1049): ФГ1986
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1986")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1986_1986 = 1986,

        ///<summary>
        /// 1985
        /// DisplayOrder: 54
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1985
        ///     (Russian - 1049): ФГ1985
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1985")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1985_1985 = 1985,

        ///<summary>
        /// 1984
        /// DisplayOrder: 55
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1984
        ///     (Russian - 1049): ФГ1984
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1984")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1984_1984 = 1984,

        ///<summary>
        /// 1983
        /// DisplayOrder: 56
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1983
        ///     (Russian - 1049): ФГ1983
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1983")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1983_1983 = 1983,

        ///<summary>
        /// 1982
        /// DisplayOrder: 57
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1982
        ///     (Russian - 1049): ФГ1982
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1982")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1982_1982 = 1982,

        ///<summary>
        /// 1981
        /// DisplayOrder: 58
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1981
        ///     (Russian - 1049): ФГ1981
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1981")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1981_1981 = 1981,

        ///<summary>
        /// 1980
        /// DisplayOrder: 59
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1980
        ///     (Russian - 1049): ФГ1980
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1980")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1980_1980 = 1980,

        ///<summary>
        /// 1979
        /// DisplayOrder: 60
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1979
        ///     (Russian - 1049): ФГ1979
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1979")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1979_1979 = 1979,

        ///<summary>
        /// 1978
        /// DisplayOrder: 61
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1978
        ///     (Russian - 1049): ФГ1978
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1978")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1978_1978 = 1978,

        ///<summary>
        /// 1977
        /// DisplayOrder: 62
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1977
        ///     (Russian - 1049): ФГ1977
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1977")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1977_1977 = 1977,

        ///<summary>
        /// 1976
        /// DisplayOrder: 63
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1976
        ///     (Russian - 1049): ФГ1976
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1976")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1976_1976 = 1976,

        ///<summary>
        /// 1975
        /// DisplayOrder: 64
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1975
        ///     (Russian - 1049): ФГ1975
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1975")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1975_1975 = 1975,

        ///<summary>
        /// 1974
        /// DisplayOrder: 65
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1974
        ///     (Russian - 1049): ФГ1974
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1974")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1974_1974 = 1974,

        ///<summary>
        /// 1973
        /// DisplayOrder: 66
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1973
        ///     (Russian - 1049): ФГ1973
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1973")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1973_1973 = 1973,

        ///<summary>
        /// 1972
        /// DisplayOrder: 67
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1972
        ///     (Russian - 1049): ФГ1972
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1972")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1972_1972 = 1972,

        ///<summary>
        /// 1971
        /// DisplayOrder: 68
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1971
        ///     (Russian - 1049): ФГ1971
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1971")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1971_1971 = 1971,

        ///<summary>
        /// 1970
        /// DisplayOrder: 69
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): FY1970
        ///     (Russian - 1049): ФГ1970
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("FY1970")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FY1970_1970 = 1970,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Case Origin
    ///     (Russian - 1049): Происхождение обращения
    /// 
    /// Description:
    ///     (English - United States - 1033): Information that specifies the source of the case information, such as Web, telephone, or email.
    ///     (Russian - 1049): Сведения об источнике поступления обращения: Интернет, телефон или электронная почта.
    /// 
    /// OptionSet Name: incident_caseorigincode      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 3
    ///     AttributeName                         DisplayName    IsCustomizable    Behavior
    ///     entitlementchannel.channel            Name           False             IncludeSubcomponents
    ///     entitlementtemplatechannel.channel    Name           False             IncludeSubcomponents
    ///     incident.caseorigincode               Origin         True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Case Origin")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum incident_caseorigincode
    {
        ///<summary>
        /// 1
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Phone
        ///     (Russian - 1049): Телефон
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Phone")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Phone_1 = 1,

        ///<summary>
        /// 2
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Email
        ///     (Russian - 1049): Электронная почта
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Email")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Email_2 = 2,

        ///<summary>
        /// 3
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Web
        ///     (Russian - 1049): Интернет
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Web")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Web_3 = 3,

        ///<summary>
        /// 2483
        /// DisplayOrder: 4
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Facebook
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Facebook")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Facebook_2483 = 2483,

        ///<summary>
        /// 3986
        /// DisplayOrder: 5
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Twitter
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Twitter")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Twitter_3986 = 3986,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Initial Communication
    ///     (Russian - 1049): Первоначальное обращение
    /// 
    /// Description:
    ///     (English - United States - 1033): If there has been initial communication with this lead.
    ///     (Russian - 1049): Было ли первоначальное обращение к этому интересу.
    /// 
    /// OptionSet Name: initialcommunication      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 2
    ///     AttributeName                       DisplayName              IsCustomizable    Behavior
    ///     lead.initialcommunication           Initial Communication    True              IncludeSubcomponents
    ///     opportunity.initialcommunication    Initial Communication    True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Initial Communication")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum initialcommunication
    {
        ///<summary>
        /// 0
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Contacted
        ///     (Russian - 1049): Связь установлена
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Contacted")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Contacted_0 = 0,

        ///<summary>
        /// 1
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Not Contacted
        ///     (Russian - 1049): Контактов не было
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Not Contacted")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Not_Contacted_1 = 1,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Expiration State
    ///     (Russian - 1049): Состояние истечения срока действия
    /// 
    /// OptionSet Name: knowledgearticle_expirationstate      IsCustomOptionSet: False
    /// 
    /// ComponentType:   SystemForm (60)            Count: 1
    ///     EntityName    FormType    FormName                     IsCustomizable    Behavior
    ///     none          Dialog      Publish Knowledge Article    False             IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Expiration State")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum knowledgearticle_expirationstate
    {
        ///<summary>
        /// 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Published
        ///     (Russian - 1049): Опубликовано
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Published")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Published_3 = 3,

        ///<summary>
        /// 4
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Expired
        ///     (Russian - 1049): Истекло
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Expired")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Expired_4 = 4,

        ///<summary>
        /// 5
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Archived
        ///     (Russian - 1049): Архивировано
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Archived")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Archived_5 = 5,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Lead Sales Stage
    ///     (Russian - 1049): Этап продаж интереса
    /// 
    /// Description:
    ///     (English - United States - 1033): The sales process stage that this entity is in.
    ///     (Russian - 1049): Этап процесса продажи этой сущности.
    /// 
    /// OptionSet Name: lead_salesstage      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 1
    ///     AttributeName      DisplayName    IsCustomizable    Behavior
    ///     lead.salesstage    Sales Stage    True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Lead Sales Stage")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum lead_salesstage
    {
        ///<summary>
        /// 0
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Qualify
        ///     (Russian - 1049): Квалифицировать
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Qualify")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Qualify_0 = 0,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Goal Type
    ///     (Russian - 1049): Тип цели
    /// 
    /// Description:
    ///     (English - United States - 1033): Data type of the amount.
    ///     (Russian - 1049): Тип данных суммы.
    /// 
    /// OptionSet Name: metric_goaltype      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 2
    ///     AttributeName            DisplayName         IsCustomizable    Behavior
    ///     goal.amountdatatype      Amount Data Type    True              IncludeSubcomponents
    ///     metric.amountdatatype    Amount Data Type    True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Goal Type")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum metric_goaltype
    {
        ///<summary>
        /// 0
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Money
        ///     (Russian - 1049): Деньги
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Money")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Money_0 = 0,

        ///<summary>
        /// 1
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Decimal
        ///     (Russian - 1049): Десятичный
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Decimal")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Decimal_1 = 1,

        ///<summary>
        /// 2
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Integer
        ///     (Russian - 1049): Целое число
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Integer")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Integer_2 = 2,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Mobile Offline Enabled Entities
    ///     (Russian - 1049): Сущности с включенным режимом Mobile Offline
    /// 
    /// Description:
    ///     (English - United States - 1033): List of Mobile Offline Enabled Entities.
    ///     (Russian - 1049): Список сущностей с включенным режимом Mobile Offline.
    /// 
    /// OptionSet Name: mobileofflineenabledentities      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 1
    ///     AttributeName                                      DisplayName    IsCustomizable    Behavior
    ///     mobileofflineprofileitem.selectedentitytypecode    Entity         False             IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Mobile Offline Enabled Entities")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum mobileofflineenabledentities
    {
        ///<summary>
        /// 1
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Account
        ///     (Russian - 1049): Организация
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Account")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Account_1 = 1,

        ///<summary>
        /// 2
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Contact
        ///     (Russian - 1049): Контакт
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Contact")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Contact_2 = 2,

        ///<summary>
        /// 5
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Note
        ///     (Russian - 1049): Примечание
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Note")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Note_5 = 5,

        ///<summary>
        /// 8
        /// DisplayOrder: 4
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): User
        ///     (Russian - 1049): Пользователь
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("User")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        User_8 = 8,

        ///<summary>
        /// 9
        /// DisplayOrder: 5
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Team
        ///     (Russian - 1049): Рабочая группа
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Team")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Team_9 = 9,

        ///<summary>
        /// 1001
        /// DisplayOrder: 6
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Attachment
        ///     (Russian - 1049): Вложение
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Attachment")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Attachment_1001 = 1001,

        ///<summary>
        /// 2020
        /// DisplayOrder: 7
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Queue
        ///     (Russian - 1049): Очередь
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Queue")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Queue_2020 = 2020,

        ///<summary>
        /// 2029
        /// DisplayOrder: 8
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Queue Item
        ///     (Russian - 1049): Элемент очереди
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Queue Item")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Queue_Item_2029 = 2029,

        ///<summary>
        /// 4201
        /// DisplayOrder: 9
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Appointment
        ///     (Russian - 1049): Встреча
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Appointment")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Appointment_4201 = 4201,

        ///<summary>
        /// 4202
        /// DisplayOrder: 10
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Email
        ///     (Russian - 1049): Электронная почта
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Email")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Email_4202 = 4202,

        ///<summary>
        /// 4212
        /// DisplayOrder: 11
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Task
        ///     (Russian - 1049): Задача
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Task")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Task_4212 = 4212,

        ///<summary>
        /// 9752
        /// DisplayOrder: 12
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): SLA KPI Instance
        ///     (Russian - 1049): Экземпляр KPI по SLA
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("SLA KPI Instance")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SLA_KPI_Instance_9752 = 9752,

        ///<summary>
        /// 16
        /// DisplayOrder: 13
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): AccountLeads
        /// 
        /// Description:
        ///     (Russian - 1049): описание.
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("AccountLeads")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        AccountLeads_16 = 16,

        ///<summary>
        /// 22
        /// DisplayOrder: 14
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): ContactLeads
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("ContactLeads")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ContactLeads_22 = 22,

        ///<summary>
        /// 4
        /// DisplayOrder: 15
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Lead
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Lead")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Lead_4 = 4,

        ///<summary>
        /// 1024
        /// DisplayOrder: 16
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Product
        ///     (Russian - 1049): Продукт
        /// 
        /// Description:
        ///     (Russian - 1049): Информация о продуктах и ценообразовании, применяющемся к ним.
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Product")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Product_1024 = 1024,

        ///<summary>
        /// 9700
        /// DisplayOrder: 17
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Entitlement
        ///     (Russian - 1049): Объем обслуживания
        /// 
        /// Description:
        ///     (Russian - 1049): Определяет объем и тип поддержки, которую должен получать клиент.
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Entitlement")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Entitlement_9700 = 9700,

        ///<summary>
        /// 7272
        /// DisplayOrder: 18
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Entitlement Contact
        ///     (Russian - 1049): Контакт объема обслуживания
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Entitlement Contact")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Entitlement_Contact_7272 = 7272,

        ///<summary>
        /// 6363
        /// DisplayOrder: 19
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Entitlement Product
        ///     (Russian - 1049): Продукт объема обслуживания
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Entitlement Product")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Entitlement_Product_6363 = 6363,

        ///<summary>
        /// 4545
        /// DisplayOrder: 20
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Entitlement Template Product
        ///     (Russian - 1049): Продукт шаблона объема обслуживания
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Entitlement Template Product")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Entitlement_Template_Product_4545 = 4545,

        ///<summary>
        /// 112
        /// DisplayOrder: 21
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Case
        ///     (Russian - 1049): Обращение
        /// 
        /// Description:
        ///     (Russian - 1049): Обращение по запросу на обслуживание, связанному с контрактом.
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Case")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Case_112 = 112,

        ///<summary>
        /// 9931
        /// DisplayOrder: 22
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Incident KnowledgeBaseRecord
        ///     (Russian - 1049): Запись базы знаний инцидентов
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Incident KnowledgeBaseRecord")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Incident_KnowledgeBaseRecord_9931 = 9931,

        ///<summary>
        /// 952
        /// DisplayOrder: 23
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Phone To Case Process
        ///     (Russian - 1049): Преобразование звонка в обращение
        /// 
        /// Description:
        ///     (Russian - 1049): Преобразование звонка в обращение — последовательность операций бизнес-процесса
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Phone To Case Process")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Phone_To_Case_Process_952 = 952,

        ///<summary>
        /// 1004
        /// DisplayOrder: 24
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Competitor Address
        ///     (Russian - 1049): Адрес конкурента
        /// 
        /// Description:
        ///     (Russian - 1049): Дополнительные адреса для конкурента. Первые два адреса сохраняются в объекте конкуренции.
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Competitor Address")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Competitor_Address_1004 = 1004,

        ///<summary>
        /// 1006
        /// DisplayOrder: 25
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Competitor Product
        ///     (Russian - 1049): Продукт конкурента
        /// 
        /// Description:
        ///     (Russian - 1049): Связь между конкурентом и продуктом, предлагаемым конкурентом.
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Competitor Product")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Competitor_Product_1006 = 1006,

        ///<summary>
        /// 24
        /// DisplayOrder: 26
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): LeadCompetitors
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("LeadCompetitors")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        LeadCompetitors_24 = 24,

        ///<summary>
        /// 27
        /// DisplayOrder: 27
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): LeadProduct
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("LeadProduct")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        LeadProduct_27 = 27,

        ///<summary>
        /// 954
        /// DisplayOrder: 28
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Lead To Opportunity Sales Process
        ///     (Russian - 1049): Преобразование интереса в возможную сделку
        /// 
        /// Description:
        ///     (Russian - 1049): Преобразование интереса в возможную сделку — последовательность операций бизнес-процесса
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Lead To Opportunity Sales Process")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Lead_To_Opportunity_Sales_Process_954 = 954,

        ///<summary>
        /// 3
        /// DisplayOrder: 29
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Opportunity
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Opportunity")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Opportunity_3 = 3,

        ///<summary>
        /// 25
        /// DisplayOrder: 30
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): OpportunityCompetitors
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("OpportunityCompetitors")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        OpportunityCompetitors_25 = 25,

        ///<summary>
        /// 1083
        /// DisplayOrder: 31
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Opportunity Product
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Opportunity Product")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Opportunity_Product_1083 = 1083,

        ///<summary>
        /// 953
        /// DisplayOrder: 32
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Opportunity Sales Process
        ///     (Russian - 1049): Преобразование возможной сделки в продажу
        /// 
        /// Description:
        ///     (Russian - 1049): Преобразование возможной сделки в продажу — последовательность операций бизнес-процесса
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Opportunity Sales Process")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Opportunity_Sales_Process_953 = 953,

        ///<summary>
        /// 123
        /// DisplayOrder: 33
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Competitor
        ///     (Russian - 1049): Конкурент
        /// 
        /// Description:
        ///     (Russian - 1049): Компания, которая борется за продажу, представленную интересом или возможной сделкой.
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Competitor")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Competitor_123 = 123,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Bookable Resource Type
    ///     (Russian - 1049): Тип резервируемых ресурсов
    /// 
    /// Description:
    ///     (English - United States - 1033): Select whether the resource is an account, contact, user, equipment, crew, facility or a pool of resources.
    ///     (Russian - 1049): Выберите, чем является ресурс: организацией, контактом, пользователем, оборудованием, командой, помещением или пулом ресурсов.
    /// 
    /// OptionSet Name: msdyn_bookableresourcetype      IsCustomOptionSet: True
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Bookable Resource Type")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum msdyn_bookableresourcetype
    {
        ///<summary>
        /// 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Generic
        ///     (Russian - 1049): Универсальный
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Generic")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Generic_1 = 1,

        ///<summary>
        /// 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Contact
        ///     (Russian - 1049): Контакт
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Contact")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Contact_2 = 2,

        ///<summary>
        /// 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): User
        ///     (Russian - 1049): Пользователь
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("User")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        User_3 = 3,

        ///<summary>
        /// 4
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Equipment
        ///     (Russian - 1049): Оборудование
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Equipment")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Equipment_4 = 4,

        ///<summary>
        /// 5
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Account
        ///     (Russian - 1049): Организация
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Account")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Account_5 = 5,

        ///<summary>
        /// 6
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Crew
        ///     (Russian - 1049): Команда
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Crew")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Crew_6 = 6,

        ///<summary>
        /// 7
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Facility
        ///     (Russian - 1049): Помещение
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Facility")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Facility_7 = 7,

        ///<summary>
        /// 8
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Pool
        ///     (Russian - 1049): Пул
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Pool")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Pool_8 = 8,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Need
    ///     (Russian - 1049): Потребность
    /// 
    /// Description:
    ///     (English - United States - 1033): Need type
    ///     (Russian - 1049): Тип потребности
    /// 
    /// OptionSet Name: need      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 2
    ///     AttributeName       DisplayName    IsCustomizable    Behavior
    ///     lead.need           Need           True              IncludeSubcomponents
    ///     opportunity.need    Need           True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Need")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum need
    {
        ///<summary>
        /// 0
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Must have
        ///     (Russian - 1049): Обязан
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Must have")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Must_have_0 = 0,

        ///<summary>
        /// 1
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Should have
        ///     (Russian - 1049): Должен
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Should have")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Should_have_1 = 1,

        ///<summary>
        /// 2
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Good to have
        ///     (Russian - 1049): Хорошо
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Good to have")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Good_to_have_2 = 2,

        ///<summary>
        /// 3
        /// DisplayOrder: 4
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): No need
        ///     (Russian - 1049): Не требуется
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("No need")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        No_need_3 = 3,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Opportunity Sales Stage
    ///     (Russian - 1049): Этап продаж возможной сделки
    /// 
    /// Description:
    ///     (English - United States - 1033): The sales process stage that this entity is in.
    ///     (Russian - 1049): Этап процесса продажи этой сущности.
    /// 
    /// OptionSet Name: opportunity_salesstage      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 1
    ///     AttributeName             DisplayName    IsCustomizable    Behavior
    ///     opportunity.salesstage    Sales Stage    True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Opportunity Sales Stage")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum opportunity_salesstage
    {
        ///<summary>
        /// 0
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Qualify
        ///     (Russian - 1049): Квалифицировать
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Qualify")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Qualify_0 = 0,

        ///<summary>
        /// 1
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Develop
        ///     (Russian - 1049): Развить
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Develop")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Develop_1 = 1,

        ///<summary>
        /// 2
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Propose
        ///     (Russian - 1049): Предложить
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Propose")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Propose_2 = 2,

        ///<summary>
        /// 3
        /// DisplayOrder: 4
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Close
        ///     (Russian - 1049): Закрыть
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Close")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Close_3 = 3,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Lookback
    ///     (Russian - 1049): Ретроспективный обзор
    /// 
    /// Description:
    ///     (English - United States - 1033): Lookback referenced by configuration.
    ///     (Russian - 1049): Ретроспективный обзор, на которую ссылается настройка.
    /// 
    /// OptionSet Name: orginsightsconfiguration_lookback      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 1
    ///     AttributeName                             DisplayName    IsCustomizable    Behavior
    ///     savedorginsightsconfiguration.lookback    Lookback       False             IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Lookback")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum orginsightsconfiguration_lookback
    {
        ///<summary>
        /// 1
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): 2H
        ///     (Russian - 1049): 2 ч
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("2H")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        V_2H_1 = 1,

        ///<summary>
        /// 2
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): 48H
        ///     (Russian - 1049): 48 ч
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("48H")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        V_48H_2 = 2,

        ///<summary>
        /// 3
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): 7D
        ///     (Russian - 1049): 7 д
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("7D")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        V_7D_3 = 3,

        ///<summary>
        /// 4
        /// DisplayOrder: 4
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): 30D
        ///     (Russian - 1049): 30 д
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("30D")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        V_30D_4 = 4,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Plot Option
    ///     (Russian - 1049): Вариант диаграммы
    /// 
    /// Description:
    ///     (English - United States - 1033): Plot Option referenced by configuration.
    ///     (Russian - 1049): Вариант диаграммы, на который ссылается настройка.
    /// 
    /// OptionSet Name: orginsightsconfiguration_plotoption      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 1
    ///     AttributeName                               DisplayName    IsCustomizable    Behavior
    ///     savedorginsightsconfiguration.plotoption    Plot Option    False             IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Plot Option")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum orginsightsconfiguration_plotoption
    {
        ///<summary>
        /// 1
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Line
        ///     (Russian - 1049): Линейная
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Line")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Line_1 = 1,

        ///<summary>
        /// 2
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Column
        ///     (Russian - 1049): Гистограмма
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Column")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Column_2 = 2,

        ///<summary>
        /// 3
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Area
        ///     (Russian - 1049): С областями
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Area")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Area_3 = 3,

        ///<summary>
        /// 4
        /// DisplayOrder: 4
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Pie
        ///     (Russian - 1049): Круговая
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Pie")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Pie_4 = 4,

        ///<summary>
        /// 5
        /// DisplayOrder: 5
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Bar
        ///     (Russian - 1049): Линейчатая
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Bar")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Bar_5 = 5,

        ///<summary>
        /// 6
        /// DisplayOrder: 6
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Donut
        ///     (Russian - 1049): Круговая
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Donut")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Donut_6 = 6,

        ///<summary>
        /// 7
        /// DisplayOrder: 7
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Infocard
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Infocard")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Infocard_7 = 7,

        ///<summary>
        /// 8
        /// DisplayOrder: 8
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): List
        ///     (Russian - 1049): Список
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("List")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        List_8 = 8,

        ///<summary>
        /// 9
        /// DisplayOrder: 9
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): DoubleDonut
        ///     (Russian - 1049): Двойная круговая
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("DoubleDonut")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DoubleDonut_9 = 9,

        ///<summary>
        /// 10
        /// DisplayOrder: 10
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): LinearGauge
        ///     (Russian - 1049): Линейная измерительная
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("LinearGauge")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        LinearGauge_10 = 10,

        ///<summary>
        /// 11
        /// DisplayOrder: 11
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Bubble
        ///     (Russian - 1049): Пузырьковая
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Bubble")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Bubble_11 = 11,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Photo Resolution
    ///     (Russian - 1049): Разрешение фото
    /// 
    /// OptionSet Name: photo_resolution      IsCustomOptionSet: False
    /// 
    /// ComponentType:   SystemForm (60)            Count: 1
    ///     EntityName    FormType    FormName                  IsCustomizable    Behavior
    ///     none          Dialog      Device Settings Dialog    False             IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Photo Resolution")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum photo_resolution
    {
        ///<summary>
        /// 0
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Device Default
        ///     (Russian - 1049): По умолчанию для устройства
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Device Default")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Device_Default_0 = 0,

        ///<summary>
        /// 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): 640 x 480
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("640 x 480")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        V_640_x_480_1 = 1,

        ///<summary>
        /// 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): 1024 x 768
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("1024 x 768")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        V_1024_x_768_2 = 2,

        ///<summary>
        /// 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): 1600 x 1200
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("1600 x 1200")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        V_1600_x_1200_3 = 3,

        ///<summary>
        /// 4
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): 2048 x 1536
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("2048 x 1536")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        V_2048_x_1536_4 = 4,

        ///<summary>
        /// 5
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): 2592 x 1936
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("2592 x 1936")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        V_2592_x_1936_5 = 5,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Sync Direction
    ///     (Russian - 1049): Направление синхронизации
    /// 
    /// OptionSet Name: principalsyncattributemapping_syncdirection      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 2
    ///     AttributeName                                     DisplayName       IsCustomizable    Behavior
    ///     principalsyncattributemap.defaultsyncdirection    Sync Direction    False             IncludeSubcomponents
    ///     principalsyncattributemap.syncdirection           Sync Direction    False             IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Sync Direction")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum principalsyncattributemapping_syncdirection
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

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Stage Category
    ///     (Russian - 1049): Подкатегория стадии
    /// 
    /// Description:
    ///     (English - United States - 1033): Category of the process stage.
    ///     (Russian - 1049): Категория стадии процесса.
    /// 
    /// OptionSet Name: processstage_category      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 1
    ///     AttributeName                 DisplayName       IsCustomizable    Behavior
    ///     processstage.stagecategory    Stage Category    True              IncludeSubcomponents
    /// ComponentType:   Workflow (29)            Count: 10
    ///     Entity              Category                 Name                                                                                 Type             Scope           Mode          StatusCode    IsCustomizable    Behavior
    ///     appointment         Business Process Flow    After Meeting    (UniqueName "after_meeting")                                        Task Flow        Organization    Background    Activated     True              IncludeSubcomponents
    ///     contact             Business Process Flow    Update Contact    (UniqueName "update_contact")                                      Task Flow        Organization    Background    Activated     True              IncludeSubcomponents
    ///     gbc_entity_test     Business Process Flow    Test    (UniqueName "gbc_test")                                                      Business Flow    Organization    Background    Activated     True              IncludeSubcomponents
    ///     incident            Business Process Flow    Phone to Case Process    (UniqueName "phonetocaseprocess")                           Business Flow    Organization    Background    Activated     True              IncludeSubcomponents
    ///     knowledgearticle    Business Process Flow    Expired Process    (UniqueName "expiredprocess")                                     Business Flow    Organization    Background    Activated     True              IncludeSubcomponents
    ///     knowledgearticle    Business Process Flow    New Process    (UniqueName "newprocess")                                             Business Flow    Organization    Background    Activated     True              IncludeSubcomponents
    ///     knowledgearticle    Business Process Flow    Translation Process    (UniqueName "translationprocess")                             Business Flow    Organization    Background    Activated     True              IncludeSubcomponents
    ///     lead                Business Process Flow    Lead to Opportunity Sales Process    (UniqueName "leadtoopportunitysalesprocess")    Business Flow    Organization    Background    Activated     True              IncludeSubcomponents
    ///     opportunity         Business Process Flow    Follow up with Opportunity    (UniqueName "make_contact_on_opportunity")             Task Flow        Organization    Background    Activated     True              IncludeSubcomponents
    ///     opportunity         Business Process Flow    Opportunity Sales Process    (UniqueName "opportunitysalesprocess")                  Business Flow    Organization    Background    Activated     True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Stage Category")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum processstage_category
    {
        ///<summary>
        /// 0
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Qualify
        ///     (Russian - 1049): Квалифицировать
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Qualify")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Qualify_0 = 0,

        ///<summary>
        /// 1
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Develop
        ///     (Russian - 1049): Развить
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Develop")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Develop_1 = 1,

        ///<summary>
        /// 2
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Propose
        ///     (Russian - 1049): Предложить
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Propose")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Propose_2 = 2,

        ///<summary>
        /// 3
        /// DisplayOrder: 4
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Close
        ///     (Russian - 1049): Закрыть
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Close")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Close_3 = 3,

        ///<summary>
        /// 4
        /// DisplayOrder: 5
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Identify
        ///     (Russian - 1049): Определить
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Identify")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Identify_4 = 4,

        ///<summary>
        /// 5
        /// DisplayOrder: 6
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Research
        ///     (Russian - 1049): Исследование
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Research")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Research_5 = 5,

        ///<summary>
        /// 6
        /// DisplayOrder: 7
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Resolve
        ///     (Russian - 1049): Разрешить
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Resolve")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Resolve_6 = 6,

        ///<summary>
        /// 7
        /// DisplayOrder: 8
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Approval
        ///     (Russian - 1049): Утверждение
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Approval")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Approval_7 = 7,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Purchase Process
    ///     (Russian - 1049): Процесс покупки
    /// 
    /// Description:
    ///     (English - United States - 1033): The type of Purchase Process for this lead.
    ///     (Russian - 1049): Тип процесса закупок для этого интереса.
    /// 
    /// OptionSet Name: purchaseprocess      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 2
    ///     AttributeName                  DisplayName         IsCustomizable    Behavior
    ///     lead.purchaseprocess           Purchase Process    True              IncludeSubcomponents
    ///     opportunity.purchaseprocess    Purchase Process    True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Purchase Process")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum purchaseprocess
    {
        ///<summary>
        /// 0
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Individual
        ///     (Russian - 1049): Отдельное лицо
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Individual")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Individual_0 = 0,

        ///<summary>
        /// 1
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Committee
        ///     (Russian - 1049): Комитет
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Committee")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Committee_1 = 1,

        ///<summary>
        /// 2
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Unknown
        ///     (Russian - 1049): Неизвестно
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Unknown")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Unknown_2 = 2,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Purchase Timeframe
    ///     (Russian - 1049): Временной период покупки
    /// 
    /// Description:
    ///     (English - United States - 1033): The timeframe that this lead is likely to make a purchase in.
    ///     (Russian - 1049): Интервал времени, в течение которого этот интерес может совершить покупку.
    /// 
    /// OptionSet Name: purchasetimeframe      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 2
    ///     AttributeName                    DisplayName           IsCustomizable    Behavior
    ///     lead.purchasetimeframe           Purchase Timeframe    True              IncludeSubcomponents
    ///     opportunity.purchasetimeframe    Purchase Timeframe    True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Purchase Timeframe")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum purchasetimeframe
    {
        ///<summary>
        /// 0
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Immediate
        ///     (Russian - 1049): Немедленно
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Immediate")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Immediate_0 = 0,

        ///<summary>
        /// 1
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): This Quarter
        ///     (Russian - 1049): Этот квартал
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("This Quarter")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        This_Quarter_1 = 1,

        ///<summary>
        /// 2
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Next Quarter
        ///     (Russian - 1049): Следующий квартал
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Next Quarter")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Next_Quarter_2 = 2,

        ///<summary>
        /// 3
        /// DisplayOrder: 4
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): This Year
        ///     (Russian - 1049): Этот год
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("This Year")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        This_Year_3 = 3,

        ///<summary>
        /// 4
        /// DisplayOrder: 5
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Unknown
        ///     (Russian - 1049): Неизвестно
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Unknown")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Unknown_4 = 4,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Pricing Error 
    ///     (Russian - 1049): Ошибка ценообразования 
    /// 
    /// Description:
    ///     (English - United States - 1033): Pricing error code.
    ///     (Russian - 1049): Код ошибки при вычислении цены.
    /// 
    /// OptionSet Name: qooi_pricingerrorcode      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 8
    ///     AttributeName                          DisplayName       IsCustomizable    Behavior
    ///     invoice.pricingerrorcode               Pricing Error     True              IncludeSubcomponents
    ///     invoicedetail.pricingerrorcode         Pricing Error     True              IncludeSubcomponents
    ///     opportunity.pricingerrorcode           Pricing Error     True              IncludeSubcomponents
    ///     opportunityproduct.pricingerrorcode    Pricing Error     True              IncludeSubcomponents
    ///     quote.pricingerrorcode                 Pricing Error     True              IncludeSubcomponents
    ///     quotedetail.pricingerrorcode           Pricing Error     True              IncludeSubcomponents
    ///     salesorder.pricingerrorcode            Pricing Error     True              IncludeSubcomponents
    ///     salesorderdetail.pricingerrorcode      Pricing Error     True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Pricing Error ")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum qooi_pricingerrorcode
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
        ///     (English - United States - 1033): Detail Error
        ///     (Russian - 1049): Сведения об ошибке
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Detail Error")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Detail_Error_1 = 1,

        ///<summary>
        /// 2
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Missing Price Level
        ///     (Russian - 1049): Отсутствует уровень цены
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Missing Price Level")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Missing_Price_Level_2 = 2,

        ///<summary>
        /// 3
        /// DisplayOrder: 4
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Inactive Price Level
        ///     (Russian - 1049): Неактивный уровень цен
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Inactive Price Level")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Inactive_Price_Level_3 = 3,

        ///<summary>
        /// 4
        /// DisplayOrder: 5
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Missing Quantity
        ///     (Russian - 1049): Отсутствует количество
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Missing Quantity")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Missing_Quantity_4 = 4,

        ///<summary>
        /// 5
        /// DisplayOrder: 6
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Missing Unit Price
        ///     (Russian - 1049): Отсутствует цена за единицу
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Missing Unit Price")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Missing_Unit_Price_5 = 5,

        ///<summary>
        /// 6
        /// DisplayOrder: 7
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Missing Product
        ///     (Russian - 1049): Отсутствует продукт
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Missing Product")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Missing_Product_6 = 6,

        ///<summary>
        /// 7
        /// DisplayOrder: 8
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Invalid Product
        ///     (Russian - 1049): Недопустимый продукт
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Invalid Product")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Invalid_Product_7 = 7,

        ///<summary>
        /// 8
        /// DisplayOrder: 9
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Missing Pricing Code
        ///     (Russian - 1049): Отсутствует код ценообразования
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Missing Pricing Code")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Missing_Pricing_Code_8 = 8,

        ///<summary>
        /// 9
        /// DisplayOrder: 10
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Invalid Pricing Code
        ///     (Russian - 1049): Недопустимый код ценообразования
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Invalid Pricing Code")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Invalid_Pricing_Code_9 = 9,

        ///<summary>
        /// 10
        /// DisplayOrder: 11
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Missing UOM
        ///     (Russian - 1049): Отсутствует единица измерения
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Missing UOM")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Missing_UOM_10 = 10,

        ///<summary>
        /// 11
        /// DisplayOrder: 12
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Product Not In Price Level
        ///     (Russian - 1049): Продукт отсутствует в уровне цены
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Product Not In Price Level")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Product_Not_In_Price_Level_11 = 11,

        ///<summary>
        /// 12
        /// DisplayOrder: 13
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Missing Price Level Amount
        ///     (Russian - 1049): Отсутствует объем уровня цены
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Missing Price Level Amount")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Missing_Price_Level_Amount_12 = 12,

        ///<summary>
        /// 13
        /// DisplayOrder: 14
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Missing Price Level Percentage
        ///     (Russian - 1049): Отсутствует процент уровня цены
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Missing Price Level Percentage")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Missing_Price_Level_Percentage_13 = 13,

        ///<summary>
        /// 14
        /// DisplayOrder: 15
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Missing Price
        ///     (Russian - 1049): Отсутствует цена
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Missing Price")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Missing_Price_14 = 14,

        ///<summary>
        /// 15
        /// DisplayOrder: 16
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Missing Current Cost
        ///     (Russian - 1049): Отсутствует текущая стоимость
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Missing Current Cost")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Missing_Current_Cost_15 = 15,

        ///<summary>
        /// 16
        /// DisplayOrder: 17
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Missing Standard Cost
        ///     (Russian - 1049): Отсутствует нормативная стоимость
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Missing Standard Cost")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Missing_Standard_Cost_16 = 16,

        ///<summary>
        /// 17
        /// DisplayOrder: 18
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Invalid Price Level Amount
        ///     (Russian - 1049): Недопустимый объем уровня цены
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Invalid Price Level Amount")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Invalid_Price_Level_Amount_17 = 17,

        ///<summary>
        /// 18
        /// DisplayOrder: 19
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Invalid Price Level Percentage
        ///     (Russian - 1049): Недопустимый процент уровня цены
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Invalid Price Level Percentage")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Invalid_Price_Level_Percentage_18 = 18,

        ///<summary>
        /// 19
        /// DisplayOrder: 20
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Invalid Price
        ///     (Russian - 1049): Недопустимая цена
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Invalid Price")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Invalid_Price_19 = 19,

        ///<summary>
        /// 20
        /// DisplayOrder: 21
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Invalid Current Cost
        ///     (Russian - 1049): Недопустимая текущая стоимость
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Invalid Current Cost")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Invalid_Current_Cost_20 = 20,

        ///<summary>
        /// 21
        /// DisplayOrder: 22
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Invalid Standard Cost
        ///     (Russian - 1049): Недопустимая нормативная стоимость
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Invalid Standard Cost")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Invalid_Standard_Cost_21 = 21,

        ///<summary>
        /// 22
        /// DisplayOrder: 23
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Invalid Rounding Policy
        ///     (Russian - 1049): Недопустимое правило округления
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Invalid Rounding Policy")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Invalid_Rounding_Policy_22 = 22,

        ///<summary>
        /// 23
        /// DisplayOrder: 24
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Invalid Rounding Option
        ///     (Russian - 1049): Недопустимый тип округления
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Invalid Rounding Option")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Invalid_Rounding_Option_23 = 23,

        ///<summary>
        /// 24
        /// DisplayOrder: 25
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Invalid Rounding Amount
        ///     (Russian - 1049): Недопустимая величина округления
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Invalid Rounding Amount")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Invalid_Rounding_Amount_24 = 24,

        ///<summary>
        /// 25
        /// DisplayOrder: 26
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Price Calculation Error
        ///     (Russian - 1049): Ошибка расчета цены
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Price Calculation Error")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Price_Calculation_Error_25 = 25,

        ///<summary>
        /// 26
        /// DisplayOrder: 27
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Invalid Discount Type
        ///     (Russian - 1049): Недопустимый тип скидки
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Invalid Discount Type")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Invalid_Discount_Type_26 = 26,

        ///<summary>
        /// 27
        /// DisplayOrder: 28
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Discount Type Invalid State
        ///     (Russian - 1049): Недопустимое состояние типа скидки
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Discount Type Invalid State")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Discount_Type_Invalid_State_27 = 27,

        ///<summary>
        /// 28
        /// DisplayOrder: 29
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Invalid Discount
        ///     (Russian - 1049): Недопустимая скидка
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Invalid Discount")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Invalid_Discount_28 = 28,

        ///<summary>
        /// 29
        /// DisplayOrder: 30
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Invalid Quantity
        ///     (Russian - 1049): Недопустимое количество
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Invalid Quantity")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Invalid_Quantity_29 = 29,

        ///<summary>
        /// 30
        /// DisplayOrder: 31
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Invalid Pricing Precision
        ///     (Russian - 1049): Недопустимая точность ценообразования
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Invalid Pricing Precision")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Invalid_Pricing_Precision_30 = 30,

        ///<summary>
        /// 31
        /// DisplayOrder: 32
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Missing Product Default UOM
        ///     (Russian - 1049): Отсутствует единица измерения продукта по умолчанию
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Missing Product Default UOM")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Missing_Product_Default_UOM_31 = 31,

        ///<summary>
        /// 32
        /// DisplayOrder: 33
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Missing Product UOM Schedule 
        ///     (Russian - 1049): Отсутствует перечень единиц измерения продукта 
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Missing Product UOM Schedule ")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Missing_Product_UOM_Schedule_32 = 32,

        ///<summary>
        /// 33
        /// DisplayOrder: 34
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Inactive Discount Type
        ///     (Russian - 1049): Неактивный тип скидки
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Inactive Discount Type")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Inactive_Discount_Type_33 = 33,

        ///<summary>
        /// 34
        /// DisplayOrder: 35
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Invalid Price Level Currency
        ///     (Russian - 1049): Недопустимая валюта уровня цены
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Invalid Price Level Currency")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Invalid_Price_Level_Currency_34 = 34,

        ///<summary>
        /// 35
        /// DisplayOrder: 36
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Price Attribute Out Of Range
        ///     (Russian - 1049): Значение атрибута "Цена" выходит за границы допустимого диапазона
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Price Attribute Out Of Range")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Price_Attribute_Out_Of_Range_35 = 35,

        ///<summary>
        /// 36
        /// DisplayOrder: 37
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Base Currency Attribute Overflow
        ///     (Russian - 1049): Переполнение атрибута "Базовая валюта"
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Base Currency Attribute Overflow")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Base_Currency_Attribute_Overflow_36 = 36,

        ///<summary>
        /// 37
        /// DisplayOrder: 38
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Base Currency Attribute Underflow
        ///     (Russian - 1049): Недостаточное заполнение атрибута "Базовая валюта"
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Base Currency Attribute Underflow")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Base_Currency_Attribute_Underflow_37 = 37,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Product Type
    ///     (Russian - 1049): Тип продукта
    /// 
    /// Description:
    ///     (English - United States - 1033): Product Type Code
    ///     (Russian - 1049): Код типа продукта
    /// 
    /// OptionSet Name: qooiproduct_producttype      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 4
    ///     AttributeName                         DisplayName     IsCustomizable    Behavior
    ///     invoicedetail.producttypecode         Product type    True              IncludeSubcomponents
    ///     opportunityproduct.producttypecode    Product type    True              IncludeSubcomponents
    ///     quotedetail.producttypecode           Product type    True              IncludeSubcomponents
    ///     salesorderdetail.producttypecode      Product type    True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Product Type")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum qooiproduct_producttype
    {
        ///<summary>
        /// 1
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Product
        ///     (Russian - 1049): Продукт
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Product")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Product_1 = 1,

        ///<summary>
        /// 2
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Bundle
        ///     (Russian - 1049): Набор
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Bundle")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Bundle_2 = 2,

        ///<summary>
        /// 3
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Required Bundle Product
        ///     (Russian - 1049): Обязательный продукт набора
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Required Bundle Product")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Required_Bundle_Product_3 = 3,

        ///<summary>
        /// 4
        /// DisplayOrder: 4
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Optional Bundle Product
        ///     (Russian - 1049): Дополнительный продукт набора
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Optional Bundle Product")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Optional_Bundle_Product_4 = 4,

        ///<summary>
        /// 5
        /// DisplayOrder: 5
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Project-based Service
        ///     (Russian - 1049): Служба на основе проекта
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Project-based Service")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Project_based_Service_5 = 5,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Properties Configuration
    ///     (Russian - 1049): Конфигурация свойств
    /// 
    /// Description:
    ///     (English - United States - 1033): Specifies whether the property is in Edit or Rectify mode.
    ///     (Russian - 1049): Определяет режим свойства: редактирование или уточнение.
    /// 
    /// OptionSet Name: qooiproduct_propertiesconfigurationstatus      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 4
    ///     AttributeName                                     DisplayName               IsCustomizable    Behavior
    ///     invoicedetail.propertyconfigurationstatus         Property Configuration    True              IncludeSubcomponents
    ///     opportunityproduct.propertyconfigurationstatus    Property Configuration    True              IncludeSubcomponents
    ///     quotedetail.propertyconfigurationstatus           Property Configuration    True              IncludeSubcomponents
    ///     salesorderdetail.propertyconfigurationstatus      Property Configuration    True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Properties Configuration")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum qooiproduct_propertiesconfigurationstatus
    {
        ///<summary>
        /// 0
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Edit
        ///     (Russian - 1049): Изменить
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Edit")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Edit_0 = 0,

        ///<summary>
        /// 1
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Rectify
        ///     (Russian - 1049): Уточнить
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Rectify")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Rectify_1 = 1,

        ///<summary>
        /// 2
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Not Configured
        ///     (Russian - 1049): Не настроено
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Not Configured")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Not_Configured_2 = 2,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): MonthOfYear
    /// 
    /// Description:
    ///     (English - United States - 1033): Specifies the month of the year valid for the recurrence pattern.
    ///     (Russian - 1049): Указывает месяц года, допустимый для расписания повторения.
    /// 
    /// OptionSet Name: recurrencerule_monthofyear      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 2
    ///     AttributeName                             DisplayName      IsCustomizable    Behavior
    ///     recurrencerule.monthofyear                Month Of Year    False             IncludeSubcomponents
    ///     recurringappointmentmaster.monthofyear    Month Of Year    True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("MonthOfYear")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum recurrencerule_monthofyear
    {
        ///<summary>
        /// 0
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Invalid Month Of Year
        ///     (Russian - 1049): Недействительный месяц года
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Invalid Month Of Year")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Invalid_Month_Of_Year_0 = 0,

        ///<summary>
        /// 1
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): January
        ///     (Russian - 1049): Январь
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("January")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        January_1 = 1,

        ///<summary>
        /// 2
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): February
        ///     (Russian - 1049): Февраль
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("February")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        February_2 = 2,

        ///<summary>
        /// 3
        /// DisplayOrder: 4
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): March
        ///     (Russian - 1049): Март
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("March")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        March_3 = 3,

        ///<summary>
        /// 4
        /// DisplayOrder: 5
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): April
        ///     (Russian - 1049): Апрель
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("April")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        April_4 = 4,

        ///<summary>
        /// 5
        /// DisplayOrder: 6
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): May
        ///     (Russian - 1049): Май
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("May")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        May_5 = 5,

        ///<summary>
        /// 6
        /// DisplayOrder: 7
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): June
        ///     (Russian - 1049): Июнь
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("June")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        June_6 = 6,

        ///<summary>
        /// 7
        /// DisplayOrder: 8
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): July
        ///     (Russian - 1049): Июль
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("July")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        July_7 = 7,

        ///<summary>
        /// 8
        /// DisplayOrder: 9
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): August
        ///     (Russian - 1049): Август
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("August")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        August_8 = 8,

        ///<summary>
        /// 9
        /// DisplayOrder: 10
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): September
        ///     (Russian - 1049): Сентябрь
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("September")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        September_9 = 9,

        ///<summary>
        /// 10
        /// DisplayOrder: 11
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): October
        ///     (Russian - 1049): Октябрь
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("October")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        October_10 = 10,

        ///<summary>
        /// 11
        /// DisplayOrder: 12
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): November
        ///     (Russian - 1049): Ноябрь
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("November")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        November_11 = 11,

        ///<summary>
        /// 12
        /// DisplayOrder: 13
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): December
        ///     (Russian - 1049): Декабрь
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("December")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        December_12 = 12,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Service Stage
    ///     (Russian - 1049): Этап сервисного обслуживания
    /// 
    /// Description:
    ///     (English - United States - 1033): The service process stage that this entity is in.
    ///     (Russian - 1049): Этап процесса обслуживания этой сущности.
    /// 
    /// OptionSet Name: servicestage      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 1
    ///     AttributeName            DisplayName      IsCustomizable    Behavior
    ///     incident.servicestage    Service Stage    True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Service Stage")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum servicestage
    {
        ///<summary>
        /// 0
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Identify
        ///     (Russian - 1049): Определить
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Identify")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Identify_0 = 0,

        ///<summary>
        /// 1
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Research
        ///     (Russian - 1049): Исследование
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Research")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Research_1 = 1,

        ///<summary>
        /// 2
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Resolve
        ///     (Russian - 1049): Разрешить
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Resolve")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Resolve_2 = 2,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Validation Status
    ///     (Russian - 1049): Состояние проверки
    /// 
    /// Description:
    ///     (English - United States - 1033): Validation status of the record URL.
    ///     (Russian - 1049): Состояние проверки URL-адреса записи.
    /// 
    /// OptionSet Name: sharepoint_validationstatus      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 1
    ///     AttributeName                      DisplayName               IsCustomizable    Behavior
    ///     sharepointsite.validationstatus    Last Validation Status    True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Validation Status")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum sharepoint_validationstatus
    {
        ///<summary>
        /// 1
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Not Validated
        ///     (Russian - 1049): Не проверен
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Not Validated")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Not_Validated_1 = 1,

        ///<summary>
        /// 2
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): In Progress
        ///     (Russian - 1049): Выполняется
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("In Progress")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        In_Progress_2 = 2,

        ///<summary>
        /// 3
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Invalid
        ///     (Russian - 1049): Проверен
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Invalid")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Invalid_3 = 3,

        ///<summary>
        /// 4
        /// DisplayOrder: 4
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Valid
        ///     (Russian - 1049): Действителен
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Valid")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Valid_4 = 4,

        ///<summary>
        /// 5
        /// DisplayOrder: 5
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Could not validate
        ///     (Russian - 1049): Не удалось проверить
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Could not validate")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Could_not_validate_5 = 5,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Validation Status Reason
    ///     (Russian - 1049): Причина состояния проверки
    /// 
    /// Description:
    ///     (English - United States - 1033): Validation status reason of the record URL.
    ///     (Russian - 1049): Причина состояния проверки URL-адреса записи.
    /// 
    /// OptionSet Name: sharepoint_validationstatusreason      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 1
    ///     AttributeName                               DisplayName               IsCustomizable    Behavior
    ///     sharepointsite.validationstatuserrorcode    Additional Information    True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Validation Status Reason")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum sharepoint_validationstatusreason
    {
        ///<summary>
        /// 1
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): This record's URL has not been validated.
        ///     (Russian - 1049): Этот URL-адрес записи не был проверен.
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("This record's URL has not been validated.")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        This_record_s_URL_has_not_been_validated_1 = 1,

        ///<summary>
        /// 2
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): This record's URL is valid.
        ///     (Russian - 1049): Этот URL-адрес записи действителен.
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("This record's URL is valid.")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        This_record_s_URL_is_valid_2 = 2,

        ///<summary>
        /// 3
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): This record's URL is not valid.
        ///     (Russian - 1049): Этот URL-адрес записи недействителен.
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("This record's URL is not valid.")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        This_record_s_URL_is_not_valid_3 = 3,

        ///<summary>
        /// 4
        /// DisplayOrder: 4
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): The URL schemes of Microsoft Dynamics 365 and SharePoint are different.
        ///     (Russian - 1049): Схемы URL-адресов Microsoft Dynamics 365 и SharePoint отличаются.
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("The URL schemes of Microsoft Dynamics 365 and SharePoint are different.")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        The_URL_schemes_of_Microsoft_Dynamics_365_and_SharePoint_are_different_4 = 4,

        ///<summary>
        /// 5
        /// DisplayOrder: 5
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): The URL could not be accessed because of Internet Explorer security settings.
        ///     (Russian - 1049): Не удалось получить URL-адрес из-за параметров безопасности Internet Explorer.
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("The URL could not be accessed because of Internet Explorer security settings.")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        The_URL_could_not_be_accessed_because_of_Internet_Explorer_security_settings_5 = 5,

        ///<summary>
        /// 6
        /// DisplayOrder: 6
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Authentication failure.
        ///     (Russian - 1049): Ошибка при проверке подлинности.
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Authentication failure.")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Authentication_failure_6 = 6,

        ///<summary>
        /// 7
        /// DisplayOrder: 7
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Invalid certificates.
        ///     (Russian - 1049): Недопустимые сертификаты.
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Invalid certificates.")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Invalid_certificates_7 = 7,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Location Type
    ///     (Russian - 1049): Тип расположения
    /// 
    /// Description:
    ///     (English - United States - 1033): Location type of the SharePoint document location.
    ///     (Russian - 1049): Тип расположения для расположения документа SharePoint.
    /// 
    /// OptionSet Name: sharepointdocumentlocation_locationtype      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 2
    ///     AttributeName                              DisplayName               IsCustomizable    Behavior
    ///     sharepointdocument.documentlocationtype    Document Location Type    True              IncludeSubcomponents
    ///     sharepointdocumentlocation.locationtype    Location Type             True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Location Type")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum sharepointdocumentlocation_locationtype
    {
        ///<summary>
        /// 0
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): General
        ///     (Russian - 1049): Общее
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("General")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        General_0 = 0,

        ///<summary>
        /// 1
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Dedicated for OneNote Integration
        ///     (Russian - 1049): Выделенное для интеграции с OneNote
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Dedicated for OneNote Integration")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Dedicated_for_OneNote_Integration_1 = 1,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): ServiceType
    /// 
    /// Description:
    ///     (English - United States - 1033): Shows the service type of the SharePoint site
    ///     (Russian - 1049): Показывает тип сервиса для сайта SharePoint
    /// 
    /// OptionSet Name: sharepointsite_servicetype      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 3
    ///     AttributeName                             DisplayName          IsCustomizable    Behavior
    ///     sharepointdocument.servicetype            Document Location    False             IncludeSubcomponents
    ///     sharepointdocumentlocation.servicetype    Service Type         False             IncludeSubcomponents
    ///     sharepointsite.servicetype                Service Type         False             IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("ServiceType")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum sharepointsite_servicetype
    {
        ///<summary>
        /// 0
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): SharePoint
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("SharePoint")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SharePoint_0 = 0,

        ///<summary>
        /// 1
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): OneDrive
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("OneDrive")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        OneDrive_1 = 1,

        ///<summary>
        /// 2
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Shared with me
        ///     (Russian - 1049): Мне предоставлен доступ
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Shared with me")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Shared_with_me_2 = 2,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): SLA new MDD dialog
    ///     (Russian - 1049): Диалоговое окно SLA для новой MDD
    /// 
    /// Description:
    ///     (English - United States - 1033): Select the entity for which SLA is to be created
    ///     (Russian - 1049): Выберите сущность, для которой необходимо создать SLA
    /// 
    /// OptionSet Name: sla_slaenabledentities      IsCustomOptionSet: False
    /// 
    /// ComponentType:   SystemForm (60)            Count: 1
    ///     EntityName    FormType    FormName           IsCustomizable    Behavior
    ///     none          Dialog      CreateSlaDialog    False             IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("SLA new MDD dialog")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum sla_slaenabledentities
    {
        ///<summary>
        /// 112
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Case
        ///     (Russian - 1049): Обращение
        /// 
        /// Description:
        ///     (Russian - 1049): Обращение по запросу на обслуживание, связанному с контрактом.
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Case")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Case_112 = 112,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Post Message type
    ///     (Russian - 1049): Тип сообщения записи
    /// 
    /// Description:
    ///     (English - United States - 1033): Post message type private or direct.
    ///     (Russian - 1049): Тип сообщения записи: частное или прямое.
    /// 
    /// OptionSet Name: socialactivity_postmessagetype      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 2
    ///     AttributeName                     DisplayName    IsCustomizable    Behavior
    ///     incident.messagetypecode          Received As    True              IncludeSubcomponents
    ///     socialactivity.postmessagetype    Received As    True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Post Message type")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum socialactivity_postmessagetype
    {
        ///<summary>
        /// 0
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Public Message
        ///     (Russian - 1049): Общедоступное сообщение
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Public Message")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Public_Message_0 = 0,

        ///<summary>
        /// 1
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Private Message
        ///     (Russian - 1049): Личное сообщение
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Private Message")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Private_Message_1 = 1,
    }

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Social Channel
    ///     (Russian - 1049): Канал социальной сети
    /// 
    /// Description:
    ///     (English - United States - 1033): Identifies where the social profile originated from, such as Twitter, or FaceBook.
    ///     (Russian - 1049): Указывает, откуда получен профиль социальной сети, например из Twitter или FaceBook.
    /// 
    /// OptionSet Name: socialprofile_community      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 11
    ///     AttributeName                   DisplayName       IsCustomizable    Behavior
    ///     activitypointer.community       Social Channel    True              IncludeSubcomponents
    ///     bulkoperation.community         Social Channel    True              IncludeSubcomponents
    ///     campaignactivity.community      Social Channel    True              IncludeSubcomponents
    ///     campaignresponse.community      Social Channel    True              IncludeSubcomponents
    ///     incidentresolution.community    Social Channel    True              IncludeSubcomponents
    ///     opportunityclose.community      Social Channel    True              IncludeSubcomponents
    ///     orderclose.community            Social Channel    True              IncludeSubcomponents
    ///     quoteclose.community            Social Channel    True              IncludeSubcomponents
    ///     serviceappointment.community    Social Channel    True              IncludeSubcomponents
    ///     socialactivity.community        Social Channel    True              IncludeSubcomponents
    ///     socialprofile.community         Social Channel    True              IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Social Channel")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum socialprofile_community
    {
        ///<summary>
        /// 1
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Facebook
        /// 
        /// Description:
        ///     (English - United States - 1033): Facebook item.
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Facebook")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Facebook_1 = 1,

        ///<summary>
        /// 2
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Twitter
        /// 
        /// Description:
        ///     (English - United States - 1033): Twitter.
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Twitter")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Twitter_2 = 2,

        ///<summary>
        /// 0
        /// DisplayOrder: 3
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Other
        ///     (Russian - 1049): Прочее
        /// 
        /// Description:
        ///     (English - United States - 1033): Other default
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Other")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Other_0 = 0,
    }

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

    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Object Type
    ///     (Russian - 1049): Тип объекта
    /// 
    /// Description:
    ///     (English - United States - 1033): Type of entity with which the workflow log is associated.
    ///     (Russian - 1049): Тип сущности, с которой связан журнал бизнес-процесса.
    /// 
    /// OptionSet Name: workflowlog_objecttypecode      IsCustomOptionSet: False
    /// 
    /// ComponentType:   Attribute (2)            Count: 2
    ///     AttributeName                                      DisplayName    IsCustomizable    Behavior
    ///     workflowlog.childworkflowinstanceobjecttypecode    Entity         False             IncludeSubcomponents
    ///     workflowlog.objecttypecode                         Entity         False             IncludeSubcomponents
    ///</summary>
    [System.ComponentModel.DescriptionAttribute("Object Type")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum workflowlog_objecttypecode
    {
        ///<summary>
        /// 4700
        /// DisplayOrder: 1
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): System Job
        ///     (Russian - 1049): Системное задание
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("System Job")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        System_Job_4700 = 4700,

        ///<summary>
        /// 4710
        /// DisplayOrder: 2
        /// 
        /// DisplayName:
        ///     (English - United States - 1033): Workflow Session
        ///     (Russian - 1049): Сеанс бизнес-процесса
        ///</summary>
        [System.ComponentModel.DescriptionAttribute("Workflow Session")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Workflow_Session_4710 = 4710,
    }
}