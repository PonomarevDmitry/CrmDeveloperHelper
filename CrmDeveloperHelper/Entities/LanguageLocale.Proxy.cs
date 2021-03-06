//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    
    
    /// <summary>
    /// DisplayName:
    ///     (English - United States - 1033): Language
    ///     (Russian - 1049): Язык
    /// 
    /// DisplayCollectionName:
    ///     (English - United States - 1033): Languages
    ///     (Russian - 1049): Языки
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute()]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute(LanguageLocale.EntityLogicalName)]
    [System.ComponentModel.DescriptionAttribute("Language")]
    public partial class LanguageLocale : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {
        
        public const string EntityLogicalName = "languagelocale";
        
        public const string EntitySchemaName = "LanguageLocale";
        
        public const int EntityTypeCode = 9957;
        
        public const string EntityPrimaryIdAttribute = "languagelocaleid";
        
        public const string EntityPrimaryNameAttribute = "name";
        
        /// <summary>
        /// Default Constructor languagelocale
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public LanguageLocale() : 
                base(EntityLogicalName)
        {
        }
        
        /// <summary>
        /// Constructor languagelocale for populating via LINQ queries given a LINQ anonymous type object
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public LanguageLocale(object anonymousObject) : 
                this()
        {
            if (anonymousObject == null)
            {
                return;
            }

            System.Type anonymousObjectType = anonymousObject.GetType();

            if (!anonymousObjectType.Name.StartsWith("<>")
                || anonymousObjectType.Name.IndexOf("AnonymousType", System.StringComparison.InvariantCultureIgnoreCase) == -1
            )
            {
                return;
            }

            foreach (var prop in anonymousObjectType.GetProperties())
            {
                var value = prop.GetValue(anonymousObject, null);
                var name = prop.Name.ToLower();

                switch (name)
                {
                    case "id":
                    case EntityPrimaryIdAttribute:
                        if (value is System.Guid idValue)
                        {
                            Attributes[EntityPrimaryIdAttribute] = base.Id = idValue;
                        }
                        break;

                    default:
                        if (value is Microsoft.Xrm.Sdk.FormattedValueCollection formattedValueCollection)
                        {
                            FormattedValues.AddRange(formattedValueCollection);
                        }
                        else
                        {
                            Attributes[name] = value;
                        }
                        break;
                }
            }
        }
        
        #region NotifyProperty Events

        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private void OnPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private void OnPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }
        

        #endregion
        
        #region Primary Attributes

        
        /// <summary>
        /// Description:
        ///     (English - United States - 1033): LanguageLocaleId
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute(EntityPrimaryIdAttribute)]
        [System.ComponentModel.DescriptionAttribute("LanguageLocaleId")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public override System.Guid Id
        {
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            get
            {
                return base.Id;
            }
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            set
            {
                this.LanguageLocaleId = value;
            }
        }
        
        /// <summary>
        /// Description:
        ///     (English - United States - 1033): LanguageLocaleId
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute(EntityPrimaryIdAttribute)]
        [System.ComponentModel.DescriptionAttribute("LanguageLocaleId")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public System.Nullable<System.Guid> LanguageLocaleId
        {
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            get
            {
                return this.GetAttributeValue<System.Nullable<System.Guid>>(EntityPrimaryIdAttribute);
            }
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            set
            {
                this.OnPropertyChanging(nameof(LanguageLocaleId));
                this.SetAttributeValue(EntityPrimaryIdAttribute, value);
                if (value.HasValue)
                {
                    base.Id = value.Value;
                }
                else
                {
                    base.Id = System.Guid.Empty;
                }
                this.OnPropertyChanged(nameof(LanguageLocaleId));
            }
        }
        
        /// <summary>
        /// DisplayName:
        ///     (English - United States - 1033): Name
        ///     (Russian - 1049): Имя
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute(EntityPrimaryNameAttribute)]
        [System.ComponentModel.DescriptionAttribute("Name")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public string Name
        {
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            get
            {
                return this.GetAttributeValue<string>(EntityPrimaryNameAttribute);
            }
        }
        

        #endregion
        
        #region Attributes

        
        /// <summary>
        /// DisplayName:
        ///     (English - United States - 1033): Code
        ///     (Russian - 1049): Код
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.Attributes.code)]
        [System.ComponentModel.DescriptionAttribute("Code")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public string Code
        {
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            get
            {
                return this.GetAttributeValue<string>(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.Attributes.code);
            }
        }
        
        /// <summary>
        /// DisplayName:
        ///     (English - United States - 1033): Language
        ///     (Russian - 1049): Язык
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.Attributes.language)]
        [System.ComponentModel.DescriptionAttribute("Language")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public string Language
        {
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            get
            {
                return this.GetAttributeValue<string>(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.Attributes.language);
            }
        }
        
        /// <summary>
        /// DisplayName:
        ///     (English - United States - 1033): Locale ID
        ///     (Russian - 1049): Идентификатор локали
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.Attributes.localeid)]
        [System.ComponentModel.DescriptionAttribute("Locale ID")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public System.Nullable<int> LocaleId
        {
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            get
            {
                return this.GetAttributeValue<System.Nullable<int>>(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.Attributes.localeid);
            }
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            set
            {
                this.OnPropertyChanging(nameof(LocaleId));
                this.SetAttributeValue(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.Attributes.localeid, value);
                this.OnPropertyChanged(nameof(LocaleId));
            }
        }
        
        /// <summary>
        /// DisplayName:
        ///     (English - United States - 1033): Organization
        ///     (Russian - 1049): Организация
        /// 
        /// Description:
        ///     (English - United States - 1033): Unique identifier of the organization associated with the language locale.
        ///     (Russian - 1049): Уникальный идентификатор организации, связанной с языковым стандартом.
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.Attributes.organizationid)]
        [System.ComponentModel.DescriptionAttribute("Organization")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public Microsoft.Xrm.Sdk.EntityReference OrganizationId
        {
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.Attributes.organizationid);
            }
        }
        
        /// <summary>
        /// DisplayName:
        ///     (English - United States - 1033): Region
        ///     (Russian - 1049): Регион
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.Attributes.region)]
        [System.ComponentModel.DescriptionAttribute("Region")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public string Region
        {
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            get
            {
                return this.GetAttributeValue<string>(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.Attributes.region);
            }
        }
        
        /// <summary>
        /// DisplayName:
        ///     (English - United States - 1033): State Code
        ///     (Russian - 1049): Код состояния
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.Attributes.statecode)]
        [System.ComponentModel.DescriptionAttribute("State Code")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public Microsoft.Xrm.Sdk.OptionSetValue statecode
        {
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.Attributes.statecode);
            }
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            set
            {
                this.OnPropertyChanging(nameof(statecode));
                this.SetAttributeValue(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.Attributes.statecode, value);
                this.OnPropertyChanged(nameof(statecode));
            }
        }
        
        /// <summary>
        /// DisplayName:
        ///     (English - United States - 1033): State Code
        ///     (Russian - 1049): Код состояния
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.Attributes.statecode)]
        [System.ComponentModel.DescriptionAttribute("State Code")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public System.Nullable<Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.OptionSets.statecode> statecodeEnum
        {
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            get
            {
                Microsoft.Xrm.Sdk.OptionSetValue optionSetValue = this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.Attributes.statecode);
                if (((optionSetValue != null) 
                            && System.Enum.IsDefined(typeof(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.OptionSets.statecode), optionSetValue.Value)))
                {
                    return ((Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.OptionSets.statecode)(System.Enum.ToObject(typeof(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.OptionSets.statecode), optionSetValue.Value)));
                }
                else
                {
                    return null;
                }
            }
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            set
            {
                this.OnPropertyChanging(nameof(statecodeEnum));
                this.OnPropertyChanging(nameof(statecode));
                if ((value == null))
                {
                    this.SetAttributeValue(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.Attributes.statecode, null);
                }
                else
                {
                    this.SetAttributeValue(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.Attributes.statecode, new Microsoft.Xrm.Sdk.OptionSetValue(((int)(value))));
                }
                this.OnPropertyChanged(nameof(statecode));
                this.OnPropertyChanged(nameof(statecodeEnum));
            }
        }
        
        /// <summary>
        /// DisplayName:
        ///     (English - United States - 1033): Language Status Code
        ///     (Russian - 1049): Код состояния языка
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.Attributes.statuscode)]
        [System.ComponentModel.DescriptionAttribute("Language Status Code")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public Microsoft.Xrm.Sdk.OptionSetValue statuscode
        {
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            get
            {
                return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.Attributes.statuscode);
            }
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            set
            {
                this.OnPropertyChanging(nameof(statuscode));
                this.SetAttributeValue(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.Attributes.statuscode, value);
                this.OnPropertyChanged(nameof(statuscode));
            }
        }
        
        /// <summary>
        /// DisplayName:
        ///     (English - United States - 1033): Language Status Code
        ///     (Russian - 1049): Код состояния языка
        /// </summary>
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.Attributes.statuscode)]
        [System.ComponentModel.DescriptionAttribute("Language Status Code")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public System.Nullable<Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.OptionSets.statuscode> statuscodeEnum
        {
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            get
            {
                Microsoft.Xrm.Sdk.OptionSetValue optionSetValue = this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.Attributes.statuscode);
                if (((optionSetValue != null) 
                            && System.Enum.IsDefined(typeof(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.OptionSets.statuscode), optionSetValue.Value)))
                {
                    return ((Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.OptionSets.statuscode)(System.Enum.ToObject(typeof(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.OptionSets.statuscode), optionSetValue.Value)));
                }
                else
                {
                    return null;
                }
            }
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            set
            {
                this.OnPropertyChanging(nameof(statuscodeEnum));
                this.OnPropertyChanging(nameof(statuscode));
                if ((value == null))
                {
                    this.SetAttributeValue(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.Attributes.statuscode, null);
                }
                else
                {
                    this.SetAttributeValue(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.Attributes.statuscode, new Microsoft.Xrm.Sdk.OptionSetValue(((int)(value))));
                }
                this.OnPropertyChanged(nameof(statuscode));
                this.OnPropertyChanged(nameof(statuscodeEnum));
            }
        }
        
        [Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.Attributes.versionnumber)]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public System.Nullable<long> VersionNumber
        {
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            get
            {
                return this.GetAttributeValue<System.Nullable<long>>(Nav.Common.VSPackages.CrmDeveloperHelper.Entities.LanguageLocale.Schema.Attributes.versionnumber);
            }
        }
        

        #endregion
    }
}
