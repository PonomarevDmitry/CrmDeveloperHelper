
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities.GlobalOptionSets
{

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
}