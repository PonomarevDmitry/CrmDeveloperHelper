using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    /// <summary>
    /// Сведения об отношениях, которые должны использоваться для подписки на записи связанной сущности для элемента профиля Mobile Offline.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute()]
    [Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("mobileofflineprofileitemassociation")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.0.0.9479")]
    public partial class MobileOfflineProfileItemAssociation : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        /// <summary>
        /// Default Constructor.
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode()]
        public MobileOfflineProfileItemAssociation() :
                base(EntityLogicalName)
        {
        }

        public const string EntityLogicalName = "mobileofflineprofileitemassociation";

        public const string PrimaryIdAttribute = "mobileofflineprofileitemassociationid";

        public const string PrimaryNameAttribute = "name";

        public const int EntityTypeCode = 9868;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        [System.Diagnostics.DebuggerNonUserCode()]
        private void OnPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        [System.Diagnostics.DebuggerNonUserCode()]
        private void OnPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Только для внутреннего использования.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("componentstate")]
        public Microsoft.Xrm.Sdk.OptionSetValue ComponentState
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("componentstate");
            }
        }

        /// <summary>
        /// Показывает, кто создал запись.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedBy
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdby");
            }
        }

        /// <summary>
        /// Показывает дату и время, в которые была создана запись. Дата и время отображаются для часового пояса, выбранного в параметрах Microsoft Dynamics 365.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdon")]
        public System.Nullable<System.DateTime> CreatedOn
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("createdon");
            }
        }

        /// <summary>
        /// Показывает, кто создал запись от имени другого пользователя.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference CreatedOnBehalfBy
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdonbehalfby");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("CreatedOnBehalfBy");
                this.SetAttributeValue("createdonbehalfby", value);
                this.OnPropertyChanged("CreatedOnBehalfBy");
            }
        }

        /// <summary>
        /// Версия, в которой введена связь элемента профиля Mobile Offline.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("introducedversion")]
        public string IntroducedVersion
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<string>("introducedversion");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("IntroducedVersion");
                this.SetAttributeValue("introducedversion", value);
                this.OnPropertyChanged("IntroducedVersion");
            }
        }

        /// <summary>
        /// Только для внутреннего использования.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ismanaged")]
        public System.Nullable<bool> IsManaged
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("ismanaged");
            }
        }

        /// <summary>
        /// Информация о том, является связь элемента профиля проверенной или нет
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("isvalidated")]
        public System.Nullable<bool> IsValidated
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<bool>>("isvalidated");
            }
        }

        /// <summary>
        /// Уникальный идентификатор связи элемента профиля Mobile Offline.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("mobileofflineprofileitemassociationid")]
        public System.Nullable<System.Guid> MobileOfflineProfileItemAssociationId
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("mobileofflineprofileitemassociationid");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("MobileOfflineProfileItemAssociationId");
                this.SetAttributeValue("mobileofflineprofileitemassociationid", value);
                if (value.HasValue)
                {
                    base.Id = value.Value;
                }
                else
                {
                    base.Id = System.Guid.Empty;
                }
                this.OnPropertyChanged("MobileOfflineProfileItemAssociationId");
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("mobileofflineprofileitemassociationid")]
        public override System.Guid Id
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return base.Id;
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.MobileOfflineProfileItemAssociationId = value;
            }
        }

        /// <summary>
        /// Только для внутреннего использования
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("mobileofflineprofileitemassociationidunique")]
        public System.Nullable<System.Guid> MobileOfflineProfileItemAssociationIdUnique
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("mobileofflineprofileitemassociationidunique");
            }
        }

        /// <summary>
        /// Код элемента родительского профиля.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("mobileofflineprofileitemid")]
        public Microsoft.Xrm.Sdk.EntityReference MobileOfflineProfileItemId
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("mobileofflineprofileitemid");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("MobileOfflineProfileItemId");
                this.SetAttributeValue("mobileofflineprofileitemid", value);
                this.OnPropertyChanged("MobileOfflineProfileItemId");
            }
        }

        /// <summary>
        /// Показывает, кто последний обновил запись.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedBy
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedby");
            }
        }

        /// <summary>
        /// Показывает дату и время последнего обновления записи. Дата и время отображаются для часового пояса, выбранного в параметрах Microsoft Dynamics 365.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedon")]
        public System.Nullable<System.DateTime> ModifiedOn
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("modifiedon");
            }
        }

        /// <summary>
        /// Показывает, кто обновил запись от имени другого пользователя.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
        public Microsoft.Xrm.Sdk.EntityReference ModifiedOnBehalfBy
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedonbehalfby");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("ModifiedOnBehalfBy");
                this.SetAttributeValue("modifiedonbehalfby", value);
                this.OnPropertyChanged("ModifiedOnBehalfBy");
            }
        }

        /// <summary>
        /// Введите имя связи элемента профиля Mobile Offline.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("name")]
        public string Name
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<string>("name");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("Name");
                this.SetAttributeValue("name", value);
                this.OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Уникальный идентификатор организации, относящейся к связи элемента профиля Mobile Offline.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
        public Microsoft.Xrm.Sdk.EntityReference OrganizationId
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("organizationid");
            }
        }

        /// <summary>
        /// Только для внутреннего использования.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("overwritetime")]
        public System.Nullable<System.DateTime> OverwriteTime
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("overwritetime");
            }
        }

        /// <summary>
        /// Показывает идентификатор процесса.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("processid")]
        public System.Nullable<System.Guid> ProcessId
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("processid");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("ProcessId");
                this.SetAttributeValue("processid", value);
                this.OnPropertyChanged("ProcessId");
            }
        }

        /// <summary>
        /// Критерии фильтра сущностей связей элементов профиля.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("profileitemassociationentityfilter")]
        public string ProfileItemAssociationEntityFilter
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<string>("profileitemassociationentityfilter");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("ProfileItemAssociationEntityFilter");
                this.SetAttributeValue("profileitemassociationentityfilter", value);
                this.OnPropertyChanged("ProfileItemAssociationEntityFilter");
            }
        }

        /// <summary>
        /// Показывает дату и время последней публикации.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("publishedon")]
        public System.Nullable<System.DateTime> PublishedOn
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<System.DateTime>>("publishedon");
            }
        }

        /// <summary>
        /// Только для внутреннего использования
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("relationshipdata")]
        public string RelationshipData
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<string>("relationshipdata");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("RelationshipData");
                this.SetAttributeValue("relationshipdata", value);
                this.OnPropertyChanged("RelationshipData");
            }
        }

        /// <summary>
        /// Имя схемы отношений сущности
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("relationshipdisplayname")]
        public string RelationshipDisplayName
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<string>("relationshipdisplayname");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("RelationshipDisplayName");
                this.SetAttributeValue("relationshipdisplayname", value);
                this.OnPropertyChanged("RelationshipDisplayName");
            }
        }

        /// <summary>
        /// Показывает отношение
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("relationshipid")]
        public System.Nullable<System.Guid> RelationshipId
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("relationshipid");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("RelationshipId");
                this.SetAttributeValue("relationshipid", value);
                this.OnPropertyChanged("RelationshipId");
            }
        }

        /// <summary>
        /// Отображаемое имя отношения сущности
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("relationshipname")]
        public string RelationshipName
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<string>("relationshipname");
            }
        }

        /// <summary>
        /// Список отношений выбранной сущности в элементе родительского профиля
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("selectedrelationshipsschema")]
        public Microsoft.Xrm.Sdk.OptionSetValue SelectedRelationShipsSchema
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("selectedrelationshipsschema");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("SelectedRelationShipsSchema");
                this.SetAttributeValue("selectedrelationshipsschema", value);
                this.OnPropertyChanged("SelectedRelationShipsSchema");
            }
        }

        /// <summary>
        /// Уникальный идентификатор связанного решения.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("solutionid")]
        public System.Nullable<System.Guid> SolutionId
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("solutionid");
            }
        }

        /// <summary>
        /// Показывает идентификатор стадии.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("stageid")]
        public System.Nullable<System.Guid> StageId
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("stageid");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("StageId");
                this.SetAttributeValue("stageid", value);
                this.OnPropertyChanged("StageId");
            }
        }

        /// <summary>
        /// Только для внутреннего использования.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("traversedpath")]
        public string TraversedPath
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<string>("traversedpath");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("TraversedPath");
                this.SetAttributeValue("traversedpath", value);
                this.OnPropertyChanged("TraversedPath");
            }
        }

        /// <summary>
        /// Номер версии Mobile Offline profileitemassociation.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("versionnumber")]
        public System.Nullable<long> VersionNumber
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<long>>("versionnumber");
            }
        }

        /// <summary>
        /// N:1 lk_MobileOfflineProfileItemAssociation_createdby
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_MobileOfflineProfileItemAssociation_createdby")]
        public SystemUser lk_MobileOfflineProfileItemAssociation_createdby
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetRelatedEntity<SystemUser>("lk_MobileOfflineProfileItemAssociation_createdby", null);
            }
        }

        /// <summary>
        /// N:1 lk_mobileofflineprofileitemassociation_createdonbehalfby
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_mobileofflineprofileitemassociation_createdonbehalfby")]
        public SystemUser lk_mobileofflineprofileitemassociation_createdonbehalfby
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetRelatedEntity<SystemUser>("lk_mobileofflineprofileitemassociation_createdonbehalfby", null);
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("lk_mobileofflineprofileitemassociation_createdonbehalfby");
                this.SetRelatedEntity<SystemUser>("lk_mobileofflineprofileitemassociation_createdonbehalfby", null, value);
                this.OnPropertyChanged("lk_mobileofflineprofileitemassociation_createdonbehalfby");
            }
        }

        /// <summary>
        /// N:1 lk_mobileofflineprofileitemassociation_modifiedby
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_mobileofflineprofileitemassociation_modifiedby")]
        public SystemUser lk_mobileofflineprofileitemassociation_modifiedby
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetRelatedEntity<SystemUser>("lk_mobileofflineprofileitemassociation_modifiedby", null);
            }
        }

        /// <summary>
        /// N:1 lk_mobileofflineprofileitemassociation_modifiedonbehalfby
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("lk_mobileofflineprofileitemassociation_modifiedonbehalfby")]
        public SystemUser lk_mobileofflineprofileitemassociation_modifiedonbehalfby
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetRelatedEntity<SystemUser>("lk_mobileofflineprofileitemassociation_modifiedonbehalfby", null);
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("lk_mobileofflineprofileitemassociation_modifiedonbehalfby");
                this.SetRelatedEntity<SystemUser>("lk_mobileofflineprofileitemassociation_modifiedonbehalfby", null, value);
                this.OnPropertyChanged("lk_mobileofflineprofileitemassociation_modifiedonbehalfby");
            }
        }

        /// <summary>
        /// N:1 MobileOfflineProfileItem_MobileOfflineProfileItemAssociation
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("mobileofflineprofileitemid")]
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("MobileOfflineProfileItem_MobileOfflineProfileItemAssociation")]
        public MobileOfflineProfileItem MobileOfflineProfileItem_MobileOfflineProfileItemAssociation
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetRelatedEntity<MobileOfflineProfileItem>("MobileOfflineProfileItem_MobileOfflineProfileItemAssociation", null);
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("MobileOfflineProfileItem_MobileOfflineProfileItemAssociation");
                this.SetRelatedEntity<MobileOfflineProfileItem>("MobileOfflineProfileItem_MobileOfflineProfileItemAssociation", null, value);
                this.OnPropertyChanged("MobileOfflineProfileItem_MobileOfflineProfileItemAssociation");
            }
        }

        /// <summary>
        /// N:1 MobileOfflineProfileItemAssociation_organization
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("MobileOfflineProfileItemAssociation_organization")]
        public Organization MobileOfflineProfileItemAssociation_organization
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetRelatedEntity<Organization>("MobileOfflineProfileItemAssociation_organization", null);
            }
        }

        /// <summary>
        /// Constructor for populating via LINQ queries given a LINQ anonymous type
        /// <param name="anonymousType">LINQ anonymous type.</param>
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode()]
        public MobileOfflineProfileItemAssociation(object anonymousType) :
                this()
        {
            foreach (var p in anonymousType.GetType().GetProperties())
            {
                var value = p.GetValue(anonymousType, null);
                var name = p.Name.ToLower();

                if (name.EndsWith("enum") && value.GetType().BaseType == typeof(System.Enum))
                {
                    value = new Microsoft.Xrm.Sdk.OptionSetValue((int)value);
                    name = name.Remove(name.Length - "enum".Length);
                }

                switch (name)
                {
                    case "id":
                        base.Id = (System.Guid)value;
                        Attributes["mobileofflineprofileitemassociationid"] = base.Id;
                        break;
                    case "mobileofflineprofileitemassociationid":
                        var id = (System.Nullable<System.Guid>)value;
                        if (id == null) { continue; }
                        base.Id = id.Value;
                        Attributes[name] = base.Id;
                        break;
                    case "formattedvalues":
                        // Add Support for FormattedValues
                        FormattedValues.AddRange((Microsoft.Xrm.Sdk.FormattedValueCollection)value);
                        break;
                    default:
                        Attributes[name] = value;
                        break;
                }
            }
        }
    }
}
