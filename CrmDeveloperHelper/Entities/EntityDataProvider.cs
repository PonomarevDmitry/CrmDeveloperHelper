using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    /// <summary>
    /// Разработчики могут регистрировать подключаемые модули для поставщика данных, чтобы обеспечить виртуальным сущностям в системе доступ к данным.
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute()]
    [Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("entitydataprovider")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.0.0.9479")]
    public partial class EntityDataProvider : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        /// <summary>
        /// Default Constructor.
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode()]
        public EntityDataProvider() :
                base(EntityLogicalName)
        {
        }

        public const string EntityLogicalName = "entitydataprovider";

        public const string PrimaryIdAttribute = "entitydataproviderid";

        public const string PrimaryNameAttribute = "name";

        public const int EntityTypeCode = 78;

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
        /// Подключаемый модуль Create
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createplugin")]
        public System.Nullable<System.Guid> CreatePlugin
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("createplugin");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("CreatePlugin");
                this.SetAttributeValue("createplugin", value);
                this.OnPropertyChanged("CreatePlugin");
            }
        }

        /// <summary>
        /// При создании поставщика данных конечный пользователь должен выбрать сущность источника данных, которая будет создана для этого поставщика.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("datasourcelogicalname")]
        public string DataSourceLogicalName
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<string>("datasourcelogicalname");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("DataSourceLogicalName");
                this.SetAttributeValue("datasourcelogicalname", value);
                this.OnPropertyChanged("DataSourceLogicalName");
            }
        }

        /// <summary>
        /// Подключаемый модуль Delete
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("deleteplugin")]
        public System.Nullable<System.Guid> DeletePlugin
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("deleteplugin");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("DeletePlugin");
                this.SetAttributeValue("deleteplugin", value);
                this.OnPropertyChanged("DeletePlugin");
            }
        }

        /// <summary>
        /// Для чего используется этот поставщик данных? На какие технологии хранилища данных он ориентирован?
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("description")]
        public string Description
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<string>("description");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("Description");
                this.SetAttributeValue("description", value);
                this.OnPropertyChanged("Description");
            }
        }

        /// <summary>
        /// Уникальный идентификатор поставщика данных.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("entitydataproviderid")]
        public System.Nullable<System.Guid> EntityDataProviderId
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("entitydataproviderid");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("EntityDataProviderId");
                this.SetAttributeValue("entitydataproviderid", value);
                if (value.HasValue)
                {
                    base.Id = value.Value;
                }
                else
                {
                    base.Id = System.Guid.Empty;
                }
                this.OnPropertyChanged("EntityDataProviderId");
            }
        }

        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("entitydataproviderid")]
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
                this.EntityDataProviderId = value;
            }
        }

        /// <summary>
        /// Только для внутреннего использования.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("entitydataprovideridunique")]
        public System.Nullable<System.Guid> EntityDataProviderIdUnique
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("entitydataprovideridunique");
            }
        }

        /// <summary>
        /// Версия, в которой была добавлена форма.
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
        /// Сведения, указывающие на возможность настройки этого компонента.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("iscustomizable")]
        public Microsoft.Xrm.Sdk.BooleanManagedProperty IsCustomizable
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.BooleanManagedProperty>("iscustomizable");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("IsCustomizable");
                this.SetAttributeValue("iscustomizable", value);
                this.OnPropertyChanged("IsCustomizable");
            }
        }

        /// <summary>
        /// Указывает, является ли компонент решения частью управляемого решения.
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
        /// Имя этого поставщика данных. Это имя отображается в раскрывающемся списке при создании новой сущности.
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
        /// Уникальный идентификатор организации.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
        public System.Nullable<System.Guid> OrganizationId
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("organizationid");
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
        /// Подключаемый модуль MultipleRetrieve
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("retrievemultipleplugin")]
        public System.Nullable<System.Guid> RetrieveMultiplePlugin
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("retrievemultipleplugin");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("RetrieveMultiplePlugin");
                this.SetAttributeValue("retrievemultipleplugin", value);
                this.OnPropertyChanged("RetrieveMultiplePlugin");
            }
        }

        /// <summary>
        /// Подключаемый модуль Retrieve
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("retrieveplugin")]
        public System.Nullable<System.Guid> RetrievePlugin
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("retrieveplugin");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("RetrievePlugin");
                this.SetAttributeValue("retrieveplugin", value);
                this.OnPropertyChanged("RetrievePlugin");
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
        /// Подключаемый модуль Update
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("updateplugin")]
        public System.Nullable<System.Guid> UpdatePlugin
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>("updateplugin");
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("UpdatePlugin");
                this.SetAttributeValue("updateplugin", value);
                this.OnPropertyChanged("UpdatePlugin");
            }
        }

        /// <summary>
        /// 1:N entitydataprovider_datasource
        /// </summary>
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("entitydataprovider_datasource")]
        public System.Collections.Generic.IEnumerable<EntityDataSource> entitydataprovider_datasource
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetRelatedEntities<EntityDataSource>("entitydataprovider_datasource", null);
            }
            [System.Diagnostics.DebuggerNonUserCode()]
            set
            {
                this.OnPropertyChanging("entitydataprovider_datasource");
                this.SetRelatedEntities<EntityDataSource>("entitydataprovider_datasource", null, value);
                this.OnPropertyChanged("entitydataprovider_datasource");
            }
        }

        /// <summary>
        /// N:1 organization_entitydataprovider
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
        [Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("organization_entitydataprovider")]
        public Organization organization_entitydataprovider
        {
            [System.Diagnostics.DebuggerNonUserCode()]
            get
            {
                return this.GetRelatedEntity<Organization>("organization_entitydataprovider", null);
            }
        }

        /// <summary>
        /// Constructor for populating via LINQ queries given a LINQ anonymous type
        /// <param name="anonymousType">LINQ anonymous type.</param>
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode()]
        public EntityDataProvider(object anonymousType) :
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
                        Attributes["entitydataproviderid"] = base.Id;
                        break;
                    case "entitydataproviderid":
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
