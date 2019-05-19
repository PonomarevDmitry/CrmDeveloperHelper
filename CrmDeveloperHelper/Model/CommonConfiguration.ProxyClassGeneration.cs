using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public partial class CommonConfiguration
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

        private bool _GenerateProxyClassesMakeAllPropertiesEditable = true;
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

        private bool _GenerateProxyClassesWithoutObsoleteAttribute = true;
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

        private bool _GenerateProxyClassAddDescriptionAttribute = true;
        [DataMember]
        public bool GenerateProxyClassAddDescriptionAttribute
        {
            get => _GenerateProxyClassAddDescriptionAttribute;
            set
            {
                this.OnPropertyChanging(nameof(GenerateProxyClassAddDescriptionAttribute));
                this._GenerateProxyClassAddDescriptionAttribute = value;
                this.OnPropertyChanged(nameof(GenerateProxyClassAddDescriptionAttribute));
            }
        }

        private bool _GenerateAttributesProxyClass = true;
        /// <summary>
        /// Генерировать атрибуты в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateAttributesProxyClass
        {
            get => _GenerateAttributesProxyClass;
            set
            {
                this.OnPropertyChanging(nameof(GenerateAttributesProxyClass));
                this._GenerateAttributesProxyClass = value;
                this.OnPropertyChanged(nameof(GenerateAttributesProxyClass));
            }
        }

        private bool _GenerateAttributesProxyClassWithNameOf = true;
        [DataMember]
        public bool GenerateAttributesProxyClassWithNameOf
        {
            get => _GenerateAttributesProxyClassWithNameOf;
            set
            {
                this.OnPropertyChanging(nameof(GenerateAttributesProxyClassWithNameOf));
                this._GenerateAttributesProxyClassWithNameOf = value;
                this.OnPropertyChanged(nameof(GenerateAttributesProxyClassWithNameOf));
            }
        }

        private ProxyClassAttributeEnums _GenerateAttributesProxyClassEnumsStateStatus = ProxyClassAttributeEnums.NotNeeded;
        [DataMember]
        public ProxyClassAttributeEnums GenerateAttributesProxyClassEnumsStateStatus
        {
            get => _GenerateAttributesProxyClassEnumsStateStatus;
            set
            {
                this.OnPropertyChanging(nameof(GenerateAttributesProxyClassEnumsStateStatus));
                this._GenerateAttributesProxyClassEnumsStateStatus = value;
                this.OnPropertyChanged(nameof(GenerateAttributesProxyClassEnumsStateStatus));
            }
        }

        private ProxyClassAttributeEnums _GenerateAttributesProxyClassEnumsLocal = ProxyClassAttributeEnums.NotNeeded;
        [DataMember]
        public ProxyClassAttributeEnums GenerateAttributesProxyClassEnumsLocal
        {
            get => _GenerateAttributesProxyClassEnumsLocal;
            set
            {
                this.OnPropertyChanging(nameof(GenerateAttributesProxyClassEnumsLocal));
                this._GenerateAttributesProxyClassEnumsLocal = value;
                this.OnPropertyChanged(nameof(GenerateAttributesProxyClassEnumsLocal));
            }
        }

        private ProxyClassAttributeEnums _GenerateAttributesProxyClassEnumsGlobal = ProxyClassAttributeEnums.NotNeeded;
        [DataMember]
        public ProxyClassAttributeEnums GenerateAttributesProxyClassEnumsGlobal
        {
            get => _GenerateAttributesProxyClassEnumsGlobal;
            set
            {
                this.OnPropertyChanging(nameof(GenerateAttributesProxyClassEnumsGlobal));
                this._GenerateAttributesProxyClassEnumsGlobal = value;
                this.OnPropertyChanged(nameof(GenerateAttributesProxyClassEnumsGlobal));
            }
        }

        private bool _GenerateAttributesProxyClassEnumsUseSchemaStateStatusEnum = true;
        [DataMember]
        public bool GenerateAttributesProxyClassEnumsUseSchemaStateStatusEnum
        {
            get => _GenerateAttributesProxyClassEnumsUseSchemaStateStatusEnum;
            set
            {
                this.OnPropertyChanging(nameof(GenerateAttributesProxyClassEnumsUseSchemaStateStatusEnum));
                this._GenerateAttributesProxyClassEnumsUseSchemaStateStatusEnum = value;
                this.OnPropertyChanged(nameof(GenerateAttributesProxyClassEnumsUseSchemaStateStatusEnum));
            }
        }

        private bool _GenerateAttributesProxyClassEnumsUseSchemaLocalEnum = true;
        [DataMember]
        public bool GenerateAttributesProxyClassEnumsUseSchemaLocalEnum
        {
            get => _GenerateAttributesProxyClassEnumsUseSchemaLocalEnum;
            set
            {
                this.OnPropertyChanging(nameof(GenerateAttributesProxyClassEnumsUseSchemaLocalEnum));
                this._GenerateAttributesProxyClassEnumsUseSchemaLocalEnum = value;
                this.OnPropertyChanged(nameof(GenerateAttributesProxyClassEnumsUseSchemaLocalEnum));
            }
        }

        private ProxyClassAttributeEnumsGlobalOptionSetLocation _GenerateAttributesProxyClassEnumsUseSchemaGlobalEnum = ProxyClassAttributeEnumsGlobalOptionSetLocation.InGlobalOptionSetNamespace;
        [DataMember]
        public ProxyClassAttributeEnumsGlobalOptionSetLocation GenerateAttributesProxyClassEnumsUseSchemaGlobalEnum
        {
            get => _GenerateAttributesProxyClassEnumsUseSchemaGlobalEnum;
            set
            {
                this.OnPropertyChanging(nameof(GenerateAttributesProxyClassEnumsUseSchemaGlobalEnum));
                this._GenerateAttributesProxyClassEnumsUseSchemaGlobalEnum = value;
                this.OnPropertyChanged(nameof(GenerateAttributesProxyClassEnumsUseSchemaGlobalEnum));
            }
        }

        private bool _GenerateManyToOneProxyClass = false;
        /// <summary>
        /// Генерировать связи Many To One (N:1) в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateManyToOneProxyClass
        {
            get => _GenerateManyToOneProxyClass;
            set
            {
                this.OnPropertyChanging(nameof(GenerateManyToOneProxyClass));
                this._GenerateManyToOneProxyClass = value;
                this.OnPropertyChanged(nameof(GenerateManyToOneProxyClass));
            }
        }

        private bool _GenerateOneToManyProxyClass = false;
        /// <summary>
        /// Генерировать One To Many (1:N) в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateOneToManyProxyClass
        {
            get => _GenerateOneToManyProxyClass;
            set
            {
                this.OnPropertyChanging(nameof(GenerateOneToManyProxyClass));
                this._GenerateOneToManyProxyClass = value;
                this.OnPropertyChanged(nameof(GenerateOneToManyProxyClass));
            }
        }

        private bool _GenerateManyToManyProxyClass = true;
        /// <summary>
        /// Генерировать Many To Many (N:N) в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateManyToManyProxyClass
        {
            get => _GenerateManyToManyProxyClass;
            set
            {
                this.OnPropertyChanging(nameof(GenerateManyToManyProxyClass));
                this._GenerateManyToManyProxyClass = value;
                this.OnPropertyChanged(nameof(GenerateManyToManyProxyClass));
            }
        }

        private bool _GenerateLocalOptionSetProxyClass = true;
        /// <summary>
        /// Генерировать enum для значений Picklist-ов в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateLocalOptionSetProxyClass
        {
            get => _GenerateLocalOptionSetProxyClass;
            set
            {
                this.OnPropertyChanging(nameof(GenerateLocalOptionSetProxyClass));
                this._GenerateLocalOptionSetProxyClass = value;
                this.OnPropertyChanged(nameof(GenerateLocalOptionSetProxyClass));
            }
        }

        private bool _GenerateGlobalOptionSetProxyClass = false;
        [DataMember]
        public bool GenerateGlobalOptionSetProxyClass
        {
            get => _GenerateGlobalOptionSetProxyClass;
            set
            {
                this.OnPropertyChanging(nameof(GenerateGlobalOptionSetProxyClass));
                this._GenerateGlobalOptionSetProxyClass = value;
                this.OnPropertyChanged(nameof(GenerateGlobalOptionSetProxyClass));
            }
        }

        private bool _GenerateStatusOptionSetProxyClass = false;
        /// <summary>
        /// Генерировать enum для значения Statecode и Statuscode в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateStatusOptionSetProxyClass
        {
            get => _GenerateStatusOptionSetProxyClass;
            set
            {
                this.OnPropertyChanging(nameof(GenerateStatusOptionSetProxyClass));
                this._GenerateStatusOptionSetProxyClass = value;
                this.OnPropertyChanged(nameof(GenerateStatusOptionSetProxyClass));
            }
        }

        private void LoadFromDiskProxyClass(CommonConfiguration diskData)
        {
            this.GenerateAttributesProxyClass = diskData.GenerateAttributesProxyClass;
            this.GenerateManyToOneProxyClass = diskData.GenerateManyToOneProxyClass;
            this.GenerateOneToManyProxyClass = diskData.GenerateOneToManyProxyClass;
            this.GenerateManyToManyProxyClass = diskData.GenerateManyToManyProxyClass;
            this.GenerateLocalOptionSetProxyClass = diskData.GenerateLocalOptionSetProxyClass;
            this.GenerateGlobalOptionSetProxyClass = diskData.GenerateGlobalOptionSetProxyClass;
            this.GenerateStatusOptionSetProxyClass = diskData.GenerateStatusOptionSetProxyClass;

            this.GenerateProxyClassesUseSchemaConstInCSharpAttributes = diskData.GenerateProxyClassesUseSchemaConstInCSharpAttributes;
            this.GenerateProxyClassesWithDebuggerNonUserCode = diskData.GenerateProxyClassesWithDebuggerNonUserCode;
            this.GenerateProxyClassesMakeAllPropertiesEditable = diskData.GenerateProxyClassesMakeAllPropertiesEditable;
            this.GenerateProxyClassesWithoutObsoleteAttribute = diskData.GenerateProxyClassesWithoutObsoleteAttribute;
            this.GenerateProxyClassesAddConstructorWithAnonymousTypeObject = diskData.GenerateProxyClassesAddConstructorWithAnonymousTypeObject;
            this.GenerateProxyClassAddDescriptionAttribute = diskData.GenerateProxyClassAddDescriptionAttribute;
            this.GenerateAttributesProxyClassWithNameOf = diskData.GenerateAttributesProxyClassWithNameOf;
            this.GenerateAttributesProxyClassEnumsStateStatus = diskData.GenerateAttributesProxyClassEnumsStateStatus;
            this.GenerateAttributesProxyClassEnumsLocal = diskData.GenerateAttributesProxyClassEnumsLocal;
            this.GenerateAttributesProxyClassEnumsGlobal = diskData.GenerateAttributesProxyClassEnumsGlobal;
            this.GenerateAttributesProxyClassEnumsUseSchemaStateStatusEnum = diskData.GenerateAttributesProxyClassEnumsUseSchemaStateStatusEnum;
            this.GenerateAttributesProxyClassEnumsUseSchemaLocalEnum = diskData.GenerateAttributesProxyClassEnumsUseSchemaLocalEnum;
            this.GenerateAttributesProxyClassEnumsUseSchemaGlobalEnum = diskData.GenerateAttributesProxyClassEnumsUseSchemaGlobalEnum;
        }
    }
}
