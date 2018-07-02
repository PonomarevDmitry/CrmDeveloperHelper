
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    /// (English - United States - 1033): Attachment
    /// (Russian - 1049): Вложение
    /// 
    /// DisplayCollectionName:
    /// (English - United States - 1033): Attachments
    /// (Russian - 1049): Вложения
    /// 
    /// Description:
    /// (English - United States - 1033): MIME attachment for an activity.
    /// (Russian - 1049): Вложение MIME для действия.
    /// 
    /// PropertyName                          Value                             CanBeChanged
    /// ActivityTypeMask                      0
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     False                             False
    /// CanBePrimaryEntityInRelationship      False                             False
    /// CanBeRelatedEntityInRelationship      False                             False
    /// CanChangeHierarchicalRelationship     False                             False
    /// CanChangeTrackingBeEnabled            True                              True
    /// CanCreateAttributes                   False                             False
    /// CanCreateCharts                       False                             False
    /// CanCreateForms                        False                             False
    /// CanCreateViews                        False                             False
    /// CanEnableSyncToExternalSearchIndex    True                              True
    /// CanModifyAdditionalSettings           True                              True
    /// CanTriggerWorkflow                    False
    /// ChangeTrackingEnabled                 True
    /// CollectionSchemaName                  ActivityMimeAttachments
    /// DaysSinceRecordLastModified           0
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         activitymimeattachments
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          False
    /// IsAuditEnabled                        False                             True
    /// IsAvailableOffline                    True
    /// IsBusinessProcessEnabled              False
    /// IsChildEntity                         False
    /// IsConnectionsEnabled                  False                             False
    /// IsCustomEntity                        False
    /// IsCustomizable                        True                              False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           False                             False
    /// IsEnabledForCharts                    False
    /// IsEnabledForExternalChannels          False
    /// IsEnabledForTrace                     False
    /// IsImportable                          False
    /// IsInteractionCentricEnabled           False
    /// IsIntersect                           False
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False                             False
    /// IsMappable                            False                             False
    /// IsOfflineInMobileClient               True                              True
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        True
    /// IsPrivate                             False
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  True
    /// IsReadOnlyInMobileClient              True                              False
    /// IsRenameable                          False                             False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                False
    /// IsValidForQueue                       False                             False
    /// IsVisibleInMobile                     False                             False
    /// IsVisibleInMobileClient               True                              False
    /// LogicalCollectionName                 activitymimeattachments
    /// LogicalName                           activitymimeattachment
    /// ObjectTypeCode                        1001
    /// OwnershipType                         None
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName                        FilteredActivityMimeAttachment
    /// SchemaName                            ActivityMimeAttachment
    /// SyncToExternalSearchIndex             True
    ///</summary>
    public partial class ActivityMimeAttachment
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "activitymimeattachment";

            public const string EntitySchemaName = "ActivityMimeAttachment";

            public const string EntityPrimaryNameAttribute = "filename";

            public const string EntityPrimaryIdAttribute = "activitymimeattachmentid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Regarding
                ///     (Russian - 1049): В отношении
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the activity with which the attachment is associated.
                ///     (Russian - 1049): Уникальный идентификатор действия, с которым связано вложение.
                /// 
                /// SchemaName: ActivityId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: activitypointer
                /// 
                ///     Target activitypointer    PrimaryIdAttribute activityid    PrimaryNameAttribute subject
                ///         DisplayName:
                ///         (English - United States - 1033): Activity
                ///         (Russian - 1049): Действие
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Activities
                ///         (Russian - 1049): Действия
                ///         
                ///         Description:
                ///         (English - United States - 1033): Task performed, or to be performed, by a user. An activity is any action for which an entry can be made on a calendar.
                ///         (Russian - 1049): Выполненная пользователем задача или задача, которую пользователь должен выполнить. "Действие" — это любое действие, для которого в календаре можно создать запись.
                ///</summary>
                public const string activityid = "activityid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Activity Mime Attachment
                ///     (Russian - 1049): Действие в MIME-вложении
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the attachment.
                ///     (Russian - 1049): Уникальный идентификатор вложения.
                /// 
                /// SchemaName: ActivityMimeAttachmentId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string activitymimeattachmentid = "activitymimeattachmentid";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: ActivityMimeAttachmentIdUnique
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string activitymimeattachmentidunique = "activitymimeattachmentidunique";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Descriptive subject for the activity.
                ///     (Russian - 1049): Описание действие.
                /// 
                /// SchemaName: ActivitySubject
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 2000
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string activitysubject = "activitysubject";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// Description:
                ///     (English - United States - 1033): anonymous link
                ///     (Russian - 1049): анонимная ссылка
                /// 
                /// SchemaName: AnonymousLink
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 2000
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string anonymouslink = "anonymouslink";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Content Id
                ///     (Russian - 1049): Идентификатор содержимого
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only
                ///     (Russian - 1049): Только для внутреннего пользования
                /// 
                /// SchemaName: AttachmentContentId
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 250
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string attachmentcontentid = "attachmentcontentid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Attachment
                ///     (Russian - 1049): Вложение
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the attachment with which this activitymimeattachment is associated.
                ///     (Russian - 1049): Уникальный идентификатор вложения, с которым связано activitymimeattachment.
                /// 
                /// SchemaName: AttachmentId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: attachment
                /// 
                ///     Target attachment    PrimaryIdAttribute attachmentid    PrimaryNameAttribute filename
                ///         DisplayName:
                ///         (English - United States - 1033): Attachment
                ///         (Russian - 1049): Вложение
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Attachments
                ///         (Russian - 1049): Вложения
                ///         
                ///         Description:
                ///         (English - United States - 1033): Attachment for an email activity.
                ///         (Russian - 1049): Вложение действия электронной почты.
                ///</summary>
                public const string attachmentid = "attachmentid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Attachment Number
                ///     (Russian - 1049): Номер вложения
                /// 
                /// Description:
                ///     (English - United States - 1033): Number of the attachment.
                ///     (Russian - 1049): Номер вложения.
                /// 
                /// SchemaName: AttachmentNumber
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 1000000000
                /// Format = None
                ///</summary>
                public const string attachmentnumber = "attachmentnumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Body
                ///     (Russian - 1049): Текст
                /// 
                /// Description:
                ///     (English - United States - 1033): Contents of the attachment.
                ///     (Russian - 1049): Содержимое вложения.
                /// 
                /// SchemaName: Body
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 1073741823
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string body = "body";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Component State
                ///     (Russian - 1049): Состояние компонента
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: ComponentState
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Global System  OptionSet componentstate
                /// DefaultFormValue = -1
                ///</summary>
                public const string componentstate = "componentstate";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): File Name
                ///     (Russian - 1049): Имя файла
                /// 
                /// Description:
                ///     (English - United States - 1033): File name of the attachment.
                ///     (Russian - 1049): Имя файла вложения.
                /// 
                /// SchemaName: FileName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 255
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string filename = "filename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): File Size (Bytes)
                ///     (Russian - 1049): Размер файла (байт)
                /// 
                /// Description:
                ///     (English - United States - 1033): File size of the attachment.
                ///     (Russian - 1049): Размер файла вложения.
                /// 
                /// SchemaName: FileSize
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 1000000000
                /// Format = None
                ///</summary>
                public const string filesize = "filesize";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Followed
                ///     (Russian - 1049): Отслеживается
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates if this attachment is followed.
                ///     (Russian - 1049): Указывает, отслеживается ли соответствующее вложение.
                /// 
                /// SchemaName: IsFollowed
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): No
                ///     (Russian - 1049): Нет
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Yes
                ///     (Russian - 1049): Да
                /// TrueOption = 1
                ///</summary>
                public const string isfollowed = "isfollowed";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Managed
                ///     (Russian - 1049): Управляемый
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the solution component is part of a managed solution.
                ///     (Russian - 1049): Указывает, является ли компонент решения частью управляемого решения.
                /// 
                /// SchemaName: IsManaged
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Unmanaged
                ///     (Russian - 1049): Неуправляемый
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Managed
                ///     (Russian - 1049): Управляемый
                /// TrueOption = 1
                ///</summary>
                public const string ismanaged = "ismanaged";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Mime Type
                ///     (Russian - 1049): Тип MIME
                /// 
                /// Description:
                ///     (English - United States - 1033): MIME type of the attachment.
                ///     (Russian - 1049): Тип MIME вложения.
                /// 
                /// SchemaName: MimeType
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 256
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string mimetype = "mimetype";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Item
                ///     (Russian - 1049): Элемент
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the record with which the attachment is associated
                ///     (Russian - 1049): Уникальный идентификатор записи, с которой связано вложение
                /// 
                /// SchemaName: ObjectId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: activitypointer,template
                /// 
                ///     Target activitypointer    PrimaryIdAttribute activityid    PrimaryNameAttribute subject
                ///         DisplayName:
                ///         (English - United States - 1033): Activity
                ///         (Russian - 1049): Действие
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Activities
                ///         (Russian - 1049): Действия
                ///         
                ///         Description:
                ///         (English - United States - 1033): Task performed, or to be performed, by a user. An activity is any action for which an entry can be made on a calendar.
                ///         (Russian - 1049): Выполненная пользователем задача или задача, которую пользователь должен выполнить. "Действие" — это любое действие, для которого в календаре можно создать запись.
                /// 
                ///     Target template    PrimaryIdAttribute templateid    PrimaryNameAttribute title
                ///         DisplayName:
                ///         (English - United States - 1033): Email Template
                ///         (Russian - 1049): Шаблон электронной почты
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Email Templates
                ///         (Russian - 1049): Шаблоны электронной почты
                ///         
                ///         Description:
                ///         (English - United States - 1033): Template for an email message that contains the standard attributes of an email message.
                ///         (Russian - 1049): Шаблон сообщения электронной почты, содержащий стандартные атрибуты сообщения электронной почты.
                ///</summary>
                public const string objectid = "objectid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Entity
                ///     (Russian - 1049): Сущность
                /// 
                /// Description:
                ///     (English - United States - 1033): Object Type Code of the entity that is associated with the attachment.
                ///     (Russian - 1049): Код типа объекта сущности, связанной с вложением.
                /// 
                /// SchemaName: ObjectTypeCode
                /// EntityNameAttributeMetadata    AttributeType: EntityName    AttributeTypeName: EntityNameType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Local System  OptionSet activitymimeattachment_objecttypecode
                /// DefaultFormValue = Null
                ///</summary>
                public const string objecttypecode = "objecttypecode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Record Overwrite Time
                ///     (Russian - 1049): Время замены записи
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: OverwriteTime
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive    Format = DateOnly
                ///</summary>
                public const string overwritetime = "overwritetime";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Owner
                ///     (Russian - 1049): Ответственный
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user or team who owns the activity_mime_attachment.
                ///     (Russian - 1049): Уникальный идентификатор пользователя или рабочей группы, ответственных за activity_mime_attachment.
                /// 
                /// SchemaName: OwnerId
                /// LookupAttributeMetadata    AttributeType: Owner    AttributeTypeName: OwnerType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: systemuser,team
                /// 
                ///     Target systemuser    PrimaryIdAttribute systemuserid    PrimaryNameAttribute fullname
                ///         DisplayName:
                ///         (English - United States - 1033): User
                ///         (Russian - 1049): Пользователь
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Users
                ///         (Russian - 1049): Пользователи
                ///         
                ///         Description:
                ///         (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///         (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                /// 
                ///     Target team    PrimaryIdAttribute teamid    PrimaryNameAttribute name
                ///         DisplayName:
                ///         (English - United States - 1033): Team
                ///         (Russian - 1049): Рабочая группа
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Teams
                ///         (Russian - 1049): Рабочие группы
                ///         
                ///         Description:
                ///         (English - United States - 1033): Collection of system users that routinely collaborate. Teams can be used to simplify record sharing and provide team members with common access to organization data when team members belong to different Business Units.
                ///         (Russian - 1049): Набор системных пользователей, которые обычно участвуют в совместной работе. Рабочие группы можно использовать для упрощения предоставления участникам группы общего доступа к записям и данным организации, если участники принадлежат к разным подразделениям.
                ///</summary>
                public const string ownerid = "ownerid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Owning Business Unit
                ///     (Russian - 1049): Ответственное подразделение
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the business unit that owns the activity mime attachment.
                ///     (Russian - 1049): Уникальный идентификатор подразделения, ответственного за MIME-вложение действия.
                /// 
                /// SchemaName: OwningBusinessUnit
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: businessunit
                /// 
                ///     Target businessunit    PrimaryIdAttribute businessunitid    PrimaryNameAttribute name
                ///         DisplayName:
                ///         (English - United States - 1033): Business Unit
                ///         (Russian - 1049): Подразделение
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Business Units
                ///         (Russian - 1049): Подразделения
                ///         
                ///         Description:
                ///         (English - United States - 1033): Business, division, or department in the Microsoft Dynamics 365 database.
                ///         (Russian - 1049): Компания, подразделение или отдел в базе данных Microsoft Dynamics 365.
                ///</summary>
                public const string owningbusinessunit = "owningbusinessunit";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Owner
                ///     (Russian - 1049): Ответственный
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who owns the activity mime attachment.
                ///     (Russian - 1049): Уникальный идентификатор пользователя, ответственного за MIME-вложение действия.
                /// 
                /// SchemaName: OwningUser
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: systemuser
                /// 
                ///     Target systemuser    PrimaryIdAttribute systemuserid    PrimaryNameAttribute fullname
                ///         DisplayName:
                ///         (English - United States - 1033): User
                ///         (Russian - 1049): Пользователь
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Users
                ///         (Russian - 1049): Пользователи
                ///         
                ///         Description:
                ///         (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///         (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public const string owninguser = "owninguser";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Solution
                ///     (Russian - 1049): Решение
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the associated solution.
                ///     (Russian - 1049): Уникальный идентификатор связанного решения.
                /// 
                /// SchemaName: SolutionId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string solutionid = "solutionid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Subject
                ///     (Russian - 1049): Тема
                /// 
                /// Description:
                ///     (English - United States - 1033): Descriptive subject for the attachment.
                ///     (Russian - 1049): Описание вложения.
                /// 
                /// SchemaName: Subject
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 2000
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string subject = "subject";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Solution
                ///     (Russian - 1049): Решение
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: SupportingSolutionId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: False    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string supportingsolutionid = "supportingsolutionid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Version Number
                ///     (Russian - 1049): Номер версии
                /// 
                /// Description:
                ///     (English - United States - 1033): Version number of the activity mime attachment.
                ///     (Russian - 1049): Номер версии MIME-вложения действия.
                /// 
                /// SchemaName: VersionNumber
                /// BigIntAttributeMetadata    AttributeType: BigInt    AttributeTypeName: BigIntType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = -9223372036854775808    MaxValue = 9223372036854775807
                ///</summary>
                public const string versionnumber = "versionnumber";
            }

            #endregion Attributes.

            #region Relationship ManyToOne - N:1.

            public static partial class ManyToOne
            {
                ///<summary>
                /// N:1 - Relationship activity_pointer_activity_mime_attachment
                /// 
                /// PropertyName                               Value                                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     activity_pointer_activity_mime_attachment
                /// ReferencingEntityNavigationPropertyName    objectid_activitypointer
                /// IsCustomizable                             False                                        False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity activitypointer:
                ///     DisplayName:
                ///     (English - United States - 1033): Activity
                ///     (Russian - 1049): Действие
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Activities
                ///     (Russian - 1049): Действия
                ///     
                ///     Description:
                ///     (English - United States - 1033): Task performed, or to be performed, by a user. An activity is any action for which an entry can be made on a calendar.
                ///     (Russian - 1049): Выполненная пользователем задача или задача, которую пользователь должен выполнить. "Действие" — это любое действие, для которого в календаре можно создать запись.
                ///</summary>
                public static partial class activity_pointer_activity_mime_attachment
                {
                    public const string Name = "activity_pointer_activity_mime_attachment";

                    public const string ReferencedEntity_activitypointer = "activitypointer";

                    public const string ReferencedAttribute_activityid = "activityid";

                    public const string ReferencedEntity_PrimaryNameAttribute_subject = "subject";

                    public const string ReferencingEntity_activitymimeattachment = "activitymimeattachment";

                    public const string ReferencingAttribute_objectid = "objectid";
                }

                ///<summary>
                /// N:1 - Relationship appointment_activity_mime_attachment
                /// 
                /// PropertyName                               Value                                   CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     appointment_activity_mime_attachment
                /// ReferencingEntityNavigationPropertyName    objectid_appointment
                /// IsCustomizable                             False                                   False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity appointment:
                ///     DisplayName:
                ///     (English - United States - 1033): Appointment
                ///     (Russian - 1049): Встреча
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Appointments
                ///     (Russian - 1049): Встречи
                ///     
                ///     Description:
                ///     (English - United States - 1033): Commitment representing a time interval with start/end times and duration.
                ///     (Russian - 1049): Обязательство, представляющее временной интервал с временем начала и окончания, а также длительностью.
                ///</summary>
                public static partial class appointment_activity_mime_attachment
                {
                    public const string Name = "appointment_activity_mime_attachment";

                    public const string ReferencedEntity_appointment = "appointment";

                    public const string ReferencedAttribute_activityid = "activityid";

                    public const string ReferencedEntity_PrimaryNameAttribute_subject = "subject";

                    public const string ReferencingEntity_activitymimeattachment = "activitymimeattachment";

                    public const string ReferencingAttribute_objectid = "objectid";
                }

                ///<summary>
                /// N:1 - Relationship attachment_activity_mime_attachments
                /// 
                /// PropertyName                               Value                                   CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     attachment_activity_mime_attachments
                /// ReferencingEntityNavigationPropertyName    attachmentid
                /// IsCustomizable                             False                                   False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Restrict
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity attachment:
                ///     DisplayName:
                ///     (English - United States - 1033): Attachment
                ///     (Russian - 1049): Вложение
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Attachments
                ///     (Russian - 1049): Вложения
                ///     
                ///     Description:
                ///     (English - United States - 1033): Attachment for an email activity.
                ///     (Russian - 1049): Вложение действия электронной почты.
                ///</summary>
                public static partial class attachment_activity_mime_attachments
                {
                    public const string Name = "attachment_activity_mime_attachments";

                    public const string ReferencedEntity_attachment = "attachment";

                    public const string ReferencedAttribute_attachmentid = "attachmentid";

                    public const string ReferencedEntity_PrimaryNameAttribute_filename = "filename";

                    public const string ReferencingEntity_activitymimeattachment = "activitymimeattachment";

                    public const string ReferencingAttribute_attachmentid = "attachmentid";
                }

                ///<summary>
                /// N:1 - Relationship email_activity_mime_attachment
                /// 
                /// PropertyName                               Value                             CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     email_activity_mime_attachment
                /// ReferencingEntityNavigationPropertyName    objectid_email
                /// IsCustomizable                             False                             False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity email:
                ///     DisplayName:
                ///     (English - United States - 1033): Email
                ///     (Russian - 1049): Электронная почта
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Email Messages
                ///     (Russian - 1049): Сообщения электронной почты
                ///     
                ///     Description:
                ///     (English - United States - 1033): Activity that is delivered using email protocols.
                ///     (Russian - 1049): Действие, передаваемое с помощью протоколов электронной почты.
                ///</summary>
                public static partial class email_activity_mime_attachment
                {
                    public const string Name = "email_activity_mime_attachment";

                    public const string ReferencedEntity_email = "email";

                    public const string ReferencedAttribute_activityid = "activityid";

                    public const string ReferencedEntity_PrimaryNameAttribute_subject = "subject";

                    public const string ReferencingEntity_activitymimeattachment = "activitymimeattachment";

                    public const string ReferencingAttribute_objectid = "objectid";
                }

                ///<summary>
                /// N:1 - Relationship template_activity_mime_attachments
                /// 
                /// PropertyName                               Value                                 CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     template_activity_mime_attachments
                /// ReferencingEntityNavigationPropertyName    objectid_template
                /// IsCustomizable                             False                                 False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity template:
                ///     DisplayName:
                ///     (English - United States - 1033): Email Template
                ///     (Russian - 1049): Шаблон электронной почты
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Email Templates
                ///     (Russian - 1049): Шаблоны электронной почты
                ///     
                ///     Description:
                ///     (English - United States - 1033): Template for an email message that contains the standard attributes of an email message.
                ///     (Russian - 1049): Шаблон сообщения электронной почты, содержащий стандартные атрибуты сообщения электронной почты.
                ///</summary>
                public static partial class template_activity_mime_attachments
                {
                    public const string Name = "template_activity_mime_attachments";

                    public const string ReferencedEntity_template = "template";

                    public const string ReferencedAttribute_templateid = "templateid";

                    public const string ReferencedEntity_PrimaryNameAttribute_title = "title";

                    public const string ReferencingEntity_activitymimeattachment = "activitymimeattachment";

                    public const string ReferencingAttribute_objectid = "objectid";
                }
            }

            #endregion Relationship ManyToOne - N:1.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship ActivityMimeAttachment_AsyncOperations
                /// 
                /// PropertyName                               Value                                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     ActivityMimeAttachment_AsyncOperations
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_activitymimeattachment
                /// IsCustomizable                             False                                       False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                NoCascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity asyncoperation:
                ///     DisplayName:
                ///     (English - United States - 1033): System Job
                ///     (Russian - 1049): Системное задание
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): System Jobs
                ///     (Russian - 1049): Системные задания
                ///     
                ///     Description:
                ///     (English - United States - 1033): Process whose execution can proceed independently or in the background.
                ///     (Russian - 1049): Процесс, который может выполняться независимо или в фоновом режиме.
                ///</summary>
                public static partial class activitymimeattachment_asyncoperations
                {
                    public const string Name = "ActivityMimeAttachment_AsyncOperations";

                    public const string ReferencedEntity_activitymimeattachment = "activitymimeattachment";

                    public const string ReferencedAttribute_activitymimeattachmentid = "activitymimeattachmentid";

                    public const string ReferencingEntity_asyncoperation = "asyncoperation";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship ActivityMimeAttachment_BulkDeleteFailures
                /// 
                /// PropertyName                               Value                                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     ActivityMimeAttachment_BulkDeleteFailures
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_activitymimeattachment
                /// IsCustomizable                             False                                        False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity bulkdeletefailure:
                ///     DisplayName:
                ///     (English - United States - 1033): Bulk Delete Failure
                ///     (Russian - 1049): Ошибка группового удаления
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Bulk Delete Failures
                ///     (Russian - 1049): Ошибки группового удаления
                ///     
                ///     Description:
                ///     (English - United States - 1033): Record that was not deleted during a bulk deletion job.
                ///     (Russian - 1049): Запись не была удалена во время задания группового удаления.
                ///</summary>
                public static partial class activitymimeattachment_bulkdeletefailures
                {
                    public const string Name = "ActivityMimeAttachment_BulkDeleteFailures";

                    public const string ReferencedEntity_activitymimeattachment = "activitymimeattachment";

                    public const string ReferencedAttribute_activitymimeattachmentid = "activitymimeattachmentid";

                    public const string ReferencingEntity_bulkdeletefailure = "bulkdeletefailure";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";
                }

                ///<summary>
                /// 1:N - Relationship ActivityMimeAttachment_SyncErrors
                /// 
                /// PropertyName                               Value                                                 CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     ActivityMimeAttachment_SyncErrors
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_activitymimeattachment_syncerror
                /// IsCustomizable                             True                                                  False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                Cascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 Cascade
                /// CascadeConfiguration.Reparent              Cascade
                /// CascadeConfiguration.Share                 Cascade
                /// CascadeConfiguration.Unshare               Cascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity syncerror:
                ///     DisplayName:
                ///     (English - United States - 1033): Sync Error
                ///     (Russian - 1049): Ошибка синхронизации
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Sync Errors
                ///     (Russian - 1049): Ошибки синхронизации
                ///     
                ///     Description:
                ///     (English - United States - 1033): Failure reason and other detailed information for a record that failed to sync.
                ///     (Russian - 1049): Причина сбоя и другие подробные сведения о записи, при синхронизации которой произошла ошибка.
                ///</summary>
                public static partial class activitymimeattachment_syncerrors
                {
                    public const string Name = "ActivityMimeAttachment_SyncErrors";

                    public const string ReferencedEntity_activitymimeattachment = "activitymimeattachment";

                    public const string ReferencedAttribute_activitymimeattachmentid = "activitymimeattachmentid";

                    public const string ReferencingEntity_syncerror = "syncerror";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship userentityinstancedata_activitymimeattachment
                /// 
                /// PropertyName                               Value                                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_activitymimeattachment
                /// ReferencingEntityNavigationPropertyName    objectid_activitymimeattachment
                /// IsCustomizable                             False                                            False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity userentityinstancedata:
                ///     DisplayName:
                ///     (English - United States - 1033): User Entity Instance Data
                ///     (Russian - 1049): Данные экземпляра сущности пользователя
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): Per User item instance data
                ///     (Russian - 1049): Данные экземпляра позиции "на пользователя"
                ///</summary>
                public static partial class userentityinstancedata_activitymimeattachment
                {
                    public const string Name = "userentityinstancedata_activitymimeattachment";

                    public const string ReferencedEntity_activitymimeattachment = "activitymimeattachment";

                    public const string ReferencedAttribute_activitymimeattachmentid = "activitymimeattachmentid";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_objectid = "objectid";
                }
            }

            #endregion Relationship OneToMany - 1:N.
        }
    }
}
