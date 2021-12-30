using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public partial class FileGenerationOptions
    {
        private bool _GenerateProxyClassesUseSchemaConstInCSharpAttributes = true;
        [DataMember]
        public bool GenerateProxyClassesUseSchemaConstInCSharpAttributes
        {
            get => _GenerateProxyClassesUseSchemaConstInCSharpAttributes;
            set
            {
                this.OnPropertyChanging(nameof(GenerateProxyClassesUseSchemaConstInCSharpAttributes));
                this._GenerateProxyClassesUseSchemaConstInCSharpAttributes = value;
                this.OnPropertyChanged(nameof(GenerateProxyClassesUseSchemaConstInCSharpAttributes));
            }
        }

        private bool _GenerateProxyClassesWithDebuggerNonUserCode = true;
        [DataMember]
        public bool GenerateProxyClassesWithDebuggerNonUserCode
        {
            get => _GenerateProxyClassesWithDebuggerNonUserCode;
            set
            {
                this.OnPropertyChanging(nameof(GenerateProxyClassesWithDebuggerNonUserCode));
                this._GenerateProxyClassesWithDebuggerNonUserCode = value;
                this.OnPropertyChanged(nameof(GenerateProxyClassesWithDebuggerNonUserCode));
            }
        }

        private bool _GenerateProxyClassesMakeAllPropertiesEditable = false;
        [DataMember]
        public bool GenerateProxyClassesMakeAllPropertiesEditable
        {
            get => _GenerateProxyClassesMakeAllPropertiesEditable;
            set
            {
                this.OnPropertyChanging(nameof(GenerateProxyClassesMakeAllPropertiesEditable));
                this._GenerateProxyClassesMakeAllPropertiesEditable = value;
                this.OnPropertyChanged(nameof(GenerateProxyClassesMakeAllPropertiesEditable));
            }
        }

        private bool _GenerateProxyClassesWithoutObsoleteAttribute = false;
        [DataMember]
        public bool GenerateProxyClassesWithoutObsoleteAttribute
        {
            get => _GenerateProxyClassesWithoutObsoleteAttribute;
            set
            {
                this.OnPropertyChanging(nameof(GenerateProxyClassesWithoutObsoleteAttribute));
                this._GenerateProxyClassesWithoutObsoleteAttribute = value;
                this.OnPropertyChanged(nameof(GenerateProxyClassesWithoutObsoleteAttribute));
            }
        }

        private bool _GenerateProxyClassesAddConstructorWithAnonymousTypeObject = true;
        [DataMember]
        public bool GenerateProxyClassesAddConstructorWithAnonymousTypeObject
        {
            get => _GenerateProxyClassesAddConstructorWithAnonymousTypeObject;
            set
            {
                this.OnPropertyChanging(nameof(GenerateProxyClassesAddConstructorWithAnonymousTypeObject));
                this._GenerateProxyClassesAddConstructorWithAnonymousTypeObject = value;
                this.OnPropertyChanged(nameof(GenerateProxyClassesAddConstructorWithAnonymousTypeObject));
            }
        }

        private bool _GenerateProxyClassesOverrideToStringMethod = true;
        [DataMember]
        public bool GenerateProxyClassesOverrideToStringMethod
        {
            get => _GenerateProxyClassesOverrideToStringMethod;
            set
            {
                this.OnPropertyChanging(nameof(GenerateProxyClassesOverrideToStringMethod));
                this._GenerateProxyClassesOverrideToStringMethod = value;
                this.OnPropertyChanged(nameof(GenerateProxyClassesOverrideToStringMethod));
            }
        }

        private bool _GenerateProxyClassesAddDescriptionAttribute = true;
        [DataMember]
        public bool GenerateProxyClassesAddDescriptionAttribute
        {
            get => _GenerateProxyClassesAddDescriptionAttribute;
            set
            {
                this.OnPropertyChanging(nameof(GenerateProxyClassesAddDescriptionAttribute));
                this._GenerateProxyClassesAddDescriptionAttribute = value;
                this.OnPropertyChanged(nameof(GenerateProxyClassesAddDescriptionAttribute));
            }
        }

        private bool _GenerateProxyClassesAttributes = true;
        /// <summary>
        /// Генерировать атрибуты в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateProxyClassesAttributes
        {
            get => _GenerateProxyClassesAttributes;
            set
            {
                this.OnPropertyChanging(nameof(GenerateProxyClassesAttributes));
                this._GenerateProxyClassesAttributes = value;
                this.OnPropertyChanged(nameof(GenerateProxyClassesAttributes));
            }
        }

        private bool _GenerateProxyClassesAttributesWithNameOf = true;
        [DataMember]
        public bool GenerateProxyClassesAttributesWithNameOf
        {
            get => _GenerateProxyClassesAttributesWithNameOf;
            set
            {
                this.OnPropertyChanging(nameof(GenerateProxyClassesAttributesWithNameOf));
                this._GenerateProxyClassesAttributesWithNameOf = value;
                this.OnPropertyChanged(nameof(GenerateProxyClassesAttributesWithNameOf));
            }
        }

        private ProxyClassAttributeEnums _GenerateProxyClassesAttributesEnumsStateStatus = ProxyClassAttributeEnums.AddWithNameAttributeEnum;
        [DataMember]
        public ProxyClassAttributeEnums GenerateProxyClassesAttributesEnumsStateStatus
        {
            get => _GenerateProxyClassesAttributesEnumsStateStatus;
            set
            {
                this.OnPropertyChanging(nameof(GenerateProxyClassesAttributesEnumsStateStatus));
                this._GenerateProxyClassesAttributesEnumsStateStatus = value;
                this.OnPropertyChanged(nameof(GenerateProxyClassesAttributesEnumsStateStatus));
            }
        }

        private ProxyClassAttributeEnums _GenerateProxyClassesAttributesEnumsLocal = ProxyClassAttributeEnums.AddWithNameAttributeEnum;
        [DataMember]
        public ProxyClassAttributeEnums GenerateProxyClassesAttributesEnumsLocal
        {
            get => _GenerateProxyClassesAttributesEnumsLocal;
            set
            {
                this.OnPropertyChanging(nameof(GenerateProxyClassesAttributesEnumsLocal));
                this._GenerateProxyClassesAttributesEnumsLocal = value;
                this.OnPropertyChanged(nameof(GenerateProxyClassesAttributesEnumsLocal));
            }
        }

        private ProxyClassAttributeEnums _GenerateProxyClassesAttributesEnumsGlobal = ProxyClassAttributeEnums.AddWithNameAttributeEnum;
        [DataMember]
        public ProxyClassAttributeEnums GenerateProxyClassesAttributesEnumsGlobal
        {
            get => _GenerateProxyClassesAttributesEnumsGlobal;
            set
            {
                this.OnPropertyChanging(nameof(GenerateProxyClassesAttributesEnumsGlobal));
                this._GenerateProxyClassesAttributesEnumsGlobal = value;
                this.OnPropertyChanged(nameof(GenerateProxyClassesAttributesEnumsGlobal));
            }
        }

        private bool _GenerateProxyClassesAttributesEnumsUseSchemaStateStatusEnum = true;
        [DataMember]
        public bool GenerateProxyClassesAttributesEnumsUseSchemaStateStatusEnum
        {
            get => _GenerateProxyClassesAttributesEnumsUseSchemaStateStatusEnum;
            set
            {
                this.OnPropertyChanging(nameof(GenerateProxyClassesAttributesEnumsUseSchemaStateStatusEnum));
                this._GenerateProxyClassesAttributesEnumsUseSchemaStateStatusEnum = value;
                this.OnPropertyChanged(nameof(GenerateProxyClassesAttributesEnumsUseSchemaStateStatusEnum));
            }
        }

        private bool _GenerateProxyClassesAttributesEnumsUseSchemaLocalEnum = true;
        [DataMember]
        public bool GenerateProxyClassesAttributesEnumsUseSchemaLocalEnum
        {
            get => _GenerateProxyClassesAttributesEnumsUseSchemaLocalEnum;
            set
            {
                this.OnPropertyChanging(nameof(GenerateProxyClassesAttributesEnumsUseSchemaLocalEnum));
                this._GenerateProxyClassesAttributesEnumsUseSchemaLocalEnum = value;
                this.OnPropertyChanged(nameof(GenerateProxyClassesAttributesEnumsUseSchemaLocalEnum));
            }
        }

        private ProxyClassAttributeEnumsGlobalOptionSetLocation _GenerateProxyClassesAttributesEnumsUseSchemaGlobalEnum = ProxyClassAttributeEnumsGlobalOptionSetLocation.InGlobalOptionSetNamespace;
        [DataMember]
        public ProxyClassAttributeEnumsGlobalOptionSetLocation GenerateProxyClassesAttributesEnumsUseSchemaGlobalEnum
        {
            get => _GenerateProxyClassesAttributesEnumsUseSchemaGlobalEnum;
            set
            {
                this.OnPropertyChanging(nameof(GenerateProxyClassesAttributesEnumsUseSchemaGlobalEnum));
                this._GenerateProxyClassesAttributesEnumsUseSchemaGlobalEnum = value;
                this.OnPropertyChanged(nameof(GenerateProxyClassesAttributesEnumsUseSchemaGlobalEnum));
            }
        }

        private bool _GenerateProxyClassesManyToOne = false;
        /// <summary>
        /// Генерировать связи Many To One (N:1) в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateProxyClassesManyToOne
        {
            get => _GenerateProxyClassesManyToOne;
            set
            {
                this.OnPropertyChanging(nameof(GenerateProxyClassesManyToOne));
                this._GenerateProxyClassesManyToOne = value;
                this.OnPropertyChanged(nameof(GenerateProxyClassesManyToOne));
            }
        }

        private bool _GenerateProxyClassesOneToMany = false;
        /// <summary>
        /// Генерировать One To Many (1:N) в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateProxyClassesOneToMany
        {
            get => _GenerateProxyClassesOneToMany;
            set
            {
                this.OnPropertyChanging(nameof(GenerateProxyClassesOneToMany));
                this._GenerateProxyClassesOneToMany = value;
                this.OnPropertyChanged(nameof(GenerateProxyClassesOneToMany));
            }
        }

        private bool _GenerateProxyClassesManyToMany = false;
        /// <summary>
        /// Генерировать Many To Many (N:N) в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateProxyClassesManyToMany
        {
            get => _GenerateProxyClassesManyToMany;
            set
            {
                this.OnPropertyChanging(nameof(GenerateProxyClassesManyToMany));
                this._GenerateProxyClassesManyToMany = value;
                this.OnPropertyChanged(nameof(GenerateProxyClassesManyToMany));
            }
        }

        private bool _GenerateProxyClassesLocalOptionSet = false;
        /// <summary>
        /// Генерировать enum для значений Picklist-ов в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateProxyClassesLocalOptionSet
        {
            get => _GenerateProxyClassesLocalOptionSet;
            set
            {
                this.OnPropertyChanging(nameof(GenerateProxyClassesLocalOptionSet));
                this._GenerateProxyClassesLocalOptionSet = value;
                this.OnPropertyChanged(nameof(GenerateProxyClassesLocalOptionSet));
            }
        }

        private bool _GenerateProxyClassesGlobalOptionSet = false;
        [DataMember]
        public bool GenerateProxyClassesGlobalOptionSet
        {
            get => _GenerateProxyClassesGlobalOptionSet;
            set
            {
                this.OnPropertyChanging(nameof(GenerateProxyClassesGlobalOptionSet));
                this._GenerateProxyClassesGlobalOptionSet = value;
                this.OnPropertyChanged(nameof(GenerateProxyClassesGlobalOptionSet));
            }
        }

        private bool _GenerateProxyClassesStatusOptionSet = false;
        /// <summary>
        /// Генерировать enum для значения Statecode и Statuscode в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateProxyClassesStatusOptionSet
        {
            get => _GenerateProxyClassesStatusOptionSet;
            set
            {
                this.OnPropertyChanging(nameof(GenerateProxyClassesStatusOptionSet));
                this._GenerateProxyClassesStatusOptionSet = value;
                this.OnPropertyChanged(nameof(GenerateProxyClassesStatusOptionSet));
            }
        }

        private void LoadFromDiskEntityProxyClass(FileGenerationOptions diskData)
        {
            this.GenerateProxyClassesAttributes = diskData.GenerateProxyClassesAttributes;
            this.GenerateProxyClassesManyToOne = diskData.GenerateProxyClassesManyToOne;
            this.GenerateProxyClassesOneToMany = diskData.GenerateProxyClassesOneToMany;
            this.GenerateProxyClassesManyToMany = diskData.GenerateProxyClassesManyToMany;
            this.GenerateProxyClassesLocalOptionSet = diskData.GenerateProxyClassesLocalOptionSet;
            this.GenerateProxyClassesGlobalOptionSet = diskData.GenerateProxyClassesGlobalOptionSet;
            this.GenerateProxyClassesStatusOptionSet = diskData.GenerateProxyClassesStatusOptionSet;

            this.GenerateProxyClassesUseSchemaConstInCSharpAttributes = diskData.GenerateProxyClassesUseSchemaConstInCSharpAttributes;
            this.GenerateProxyClassesWithDebuggerNonUserCode = diskData.GenerateProxyClassesWithDebuggerNonUserCode;
            this.GenerateProxyClassesMakeAllPropertiesEditable = diskData.GenerateProxyClassesMakeAllPropertiesEditable;
            this.GenerateProxyClassesWithoutObsoleteAttribute = diskData.GenerateProxyClassesWithoutObsoleteAttribute;
            this.GenerateProxyClassesAddConstructorWithAnonymousTypeObject = diskData.GenerateProxyClassesAddConstructorWithAnonymousTypeObject;
            this.GenerateProxyClassesAddDescriptionAttribute = diskData.GenerateProxyClassesAddDescriptionAttribute;

            this.GenerateProxyClassesAttributesWithNameOf = diskData.GenerateProxyClassesAttributesWithNameOf;
            this.GenerateProxyClassesAttributesEnumsStateStatus = diskData.GenerateProxyClassesAttributesEnumsStateStatus;
            this.GenerateProxyClassesAttributesEnumsLocal = diskData.GenerateProxyClassesAttributesEnumsLocal;
            this.GenerateProxyClassesAttributesEnumsGlobal = diskData.GenerateProxyClassesAttributesEnumsGlobal;

            this.GenerateProxyClassesAttributesEnumsUseSchemaStateStatusEnum = diskData.GenerateProxyClassesAttributesEnumsUseSchemaStateStatusEnum;
            this.GenerateProxyClassesAttributesEnumsUseSchemaLocalEnum = diskData.GenerateProxyClassesAttributesEnumsUseSchemaLocalEnum;
            this.GenerateProxyClassesAttributesEnumsUseSchemaGlobalEnum = diskData.GenerateProxyClassesAttributesEnumsUseSchemaGlobalEnum;
        }
    }
}
