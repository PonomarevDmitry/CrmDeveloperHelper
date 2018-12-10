
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Attachment
    ///     (Russian - 1049): Вложение
    /// 
    /// DisplayCollectionName:
    ///     (English - United States - 1033): Attachments
    ///     (Russian - 1049): Вложения
    /// 
    /// Description:
    ///     (English - United States - 1033): Attachment for an email activity.
    ///     (Russian - 1049): Вложение действия электронной почты.
    /// 
    /// PropertyName                          Value                 CanBeChanged
    /// ActivityTypeMask                      0
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     False                 False
    /// CanBePrimaryEntityInRelationship      False                 False
    /// CanBeRelatedEntityInRelationship      False                 False
    /// CanChangeHierarchicalRelationship     False                 False
    /// CanChangeTrackingBeEnabled            False                 False
    /// CanCreateAttributes                   False                 False
    /// CanCreateCharts                       False                 False
    /// CanCreateForms                        False                 False
    /// CanCreateViews                        False                 False
    /// CanEnableSyncToExternalSearchIndex    False                 False
    /// CanModifyAdditionalSettings           False                 True
    /// CanTriggerWorkflow                    False
    /// ChangeTrackingEnabled                 True
    /// CollectionSchemaName                  Attachments
    /// DaysSinceRecordLastModified           10
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         attachments
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          False
    /// IsAuditEnabled                        False                 False
    /// IsAvailableOffline                    True
    /// IsBusinessProcessEnabled              False
    /// IsChildEntity                         False
    /// IsConnectionsEnabled                  False                 False
    /// IsCustomEntity                        False
    /// IsCustomizable                        False                 False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           False                 False
    /// IsEnabledForCharts                    False
    /// IsEnabledForExternalChannels          False
    /// IsEnabledForTrace                     False
    /// IsImportable                          False
    /// IsInteractionCentricEnabled           False
    /// IsIntersect                           False
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False                 False
    /// IsMappable                            False                 False
    /// IsOfflineInMobileClient               True                  True
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        True
    /// IsPrivate                             False
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  True
    /// IsReadOnlyInMobileClient              False                 False
    /// IsRenameable                          False                 False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                False
    /// IsValidForQueue                       False                 False
    /// IsVisibleInMobile                     False                 False
    /// IsVisibleInMobileClient               True                  False
    /// LogicalCollectionName                 attachments
    /// LogicalName                           attachment
    /// ObjectTypeCode                        1002
    /// OwnershipType                         None
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName                        FilteredAttachment
    /// SchemaName                            Attachment
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class Attachment
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "attachment";

            public const string EntitySchemaName = "Attachment";

            public const string EntityPrimaryNameAttribute = "filename";

            public const string EntityPrimaryIdAttribute = "attachmentid";

            public const int EntityTypeCode = 1002;

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Attachment
                ///     (Russian - 1049): Вложение
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the attachment.
                ///     (Russian - 1049): Уникальный идентификатор вложения.
                /// 
                /// SchemaName: AttachmentId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string attachmentid = "attachmentid";

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
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 1073741823
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string body = "body";

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
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
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
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 1000000000
                /// Format = None
                ///</summary>
                public const string filesize = "filesize";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): MIME Type
                ///     (Russian - 1049): Тип MIME
                /// 
                /// Description:
                ///     (English - United States - 1033): MIME type of the attachment.
                ///     (Russian - 1049): Тип MIME вложения.
                /// 
                /// SchemaName: MimeType
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 256
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string mimetype = "mimetype";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Subject
                ///     (Russian - 1049): Тема
                /// 
                /// Description:
                ///     (English - United States - 1033): Subject associated with the attachment.
                ///     (Russian - 1049): Тема, связанная со вложением.
                /// 
                /// SchemaName: Subject
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 2000
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string subject = "subject";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Version Number
                ///     (Russian - 1049): Номер версии
                /// 
                /// Description:
                ///     (English - United States - 1033): Version number of the attachment.
                ///     (Russian - 1049): Номер версии вложения.
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

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship attachment_activity_mime_attachments
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
                /// ReferencingEntity activitymimeattachment:
                ///     DisplayName:
                ///         (English - United States - 1033): Attachment
                ///         (Russian - 1049): Вложение
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Attachments
                ///         (Russian - 1049): Вложения
                ///     
                ///     Description:
                ///         (English - United States - 1033): MIME attachment for an activity.
                ///         (Russian - 1049): Вложение MIME для действия.
                ///</summary>
                public static partial class attachment_activity_mime_attachments
                {
                    public const string Name = "attachment_activity_mime_attachments";

                    public const string ReferencedEntity_attachment = "attachment";

                    public const string ReferencedAttribute_attachmentid = "attachmentid";

                    public const string ReferencingEntity_activitymimeattachment = "activitymimeattachment";

                    public const string ReferencingAttribute_attachmentid = "attachmentid";

                    public const string ReferencingEntity_PrimaryNameAttribute_filename = "filename";
                }

                ///<summary>
                /// 1:N - Relationship Attachment_SyncErrors
                /// 
                /// PropertyName                               Value                                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Attachment_SyncErrors
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_attachment_syncerror
                /// IsCustomizable                             True                                      False
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
                ///         (English - United States - 1033): Sync Error
                ///         (Russian - 1049): Ошибка синхронизации
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Sync Errors
                ///         (Russian - 1049): Ошибки синхронизации
                ///     
                ///     Description:
                ///         (English - United States - 1033): Failure reason and other detailed information for a record that failed to sync.
                ///         (Russian - 1049): Причина сбоя и другие подробные сведения о записи, при синхронизации которой произошла ошибка.
                ///</summary>
                public static partial class attachment_syncerrors
                {
                    public const string Name = "Attachment_SyncErrors";

                    public const string ReferencedEntity_attachment = "attachment";

                    public const string ReferencedAttribute_attachmentid = "attachmentid";

                    public const string ReferencingEntity_syncerror = "syncerror";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship userentityinstancedata_attachment
                /// 
                /// PropertyName                               Value                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_attachment
                /// ReferencingEntityNavigationPropertyName    objectid_attachment
                /// IsCustomizable                             False                                False
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
                ///         (English - United States - 1033): User Entity Instance Data
                ///         (Russian - 1049): Данные экземпляра сущности пользователя
                ///     
                ///     Description:
                ///         (English - United States - 1033): Per User item instance data
                ///         (Russian - 1049): Данные экземпляра позиции "на пользователя"
                ///</summary>
                public static partial class userentityinstancedata_attachment
                {
                    public const string Name = "userentityinstancedata_attachment";

                    public const string ReferencedEntity_attachment = "attachment";

                    public const string ReferencedAttribute_attachmentid = "attachmentid";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_objectid = "objectid";
                }
            }

            #endregion Relationship OneToMany - 1:N.
        }
    }
}