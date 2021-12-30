using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public partial class FileGenerationOptions
    {
        private ConstantType _GenerateSchemaConstantType = ConstantType.Constant;
        /// <summary>
        /// Тип записей в файле с метаданными сущности. const или read only
        /// </summary>
        [DataMember]
        public ConstantType GenerateSchemaConstantType
        {
            get => _GenerateSchemaConstantType;
            set
            {
                if (_GenerateSchemaConstantType == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(GenerateSchemaConstantType));
                this._GenerateSchemaConstantType = value;
                this.OnPropertyChanged(nameof(GenerateSchemaConstantType));
            }
        }

        private OptionSetExportType _GenerateSchemaOptionSetExportType = OptionSetExportType.Enums;
        [DataMember]
        public OptionSetExportType GenerateSchemaOptionSetExportType
        {
            get => _GenerateSchemaOptionSetExportType;
            set
            {
                if (_GenerateSchemaOptionSetExportType == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(GenerateSchemaOptionSetExportType));
                this._GenerateSchemaOptionSetExportType = value;
                this.OnPropertyChanged(nameof(GenerateSchemaOptionSetExportType));
            }
        }

        private bool _GenerateSchemaEntityOptionSetsWithDependentComponents = true;
        [DataMember]
        public bool GenerateSchemaEntityOptionSetsWithDependentComponents
        {
            get => _GenerateSchemaEntityOptionSetsWithDependentComponents;
            set
            {
                this.OnPropertyChanging(nameof(GenerateSchemaEntityOptionSetsWithDependentComponents));
                this._GenerateSchemaEntityOptionSetsWithDependentComponents = value;
                this.OnPropertyChanged(nameof(GenerateSchemaEntityOptionSetsWithDependentComponents));
            }
        }

        private bool _GenerateSchemaAddDescriptionAttribute = true;
        [DataMember]
        public bool GenerateSchemaAddDescriptionAttribute
        {
            get => _GenerateSchemaAddDescriptionAttribute;
            set
            {
                this.OnPropertyChanging(nameof(GenerateSchemaAddDescriptionAttribute));
                this._GenerateSchemaAddDescriptionAttribute = value;
                this.OnPropertyChanged(nameof(GenerateSchemaAddDescriptionAttribute));
            }
        }

        private bool _GenerateSchemaAddTypeConverterAttributeForEnums = false;
        [DataMember]
        public bool GenerateSchemaAddTypeConverterAttributeForEnums
        {
            get => _GenerateSchemaAddTypeConverterAttributeForEnums;
            set
            {
                this.OnPropertyChanging(nameof(GenerateSchemaAddTypeConverterAttributeForEnums));
                this._GenerateSchemaAddTypeConverterAttributeForEnums = value;
                this.OnPropertyChanged(nameof(GenerateSchemaAddTypeConverterAttributeForEnums));
            }
        }

        private bool _GenerateSchemaAttributes = true;
        /// <summary>
        /// Генерировать атрибуты в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateSchemaAttributes
        {
            get => _GenerateSchemaAttributes;
            set
            {
                this.OnPropertyChanging(nameof(GenerateSchemaAttributes));
                this._GenerateSchemaAttributes = value;
                this.OnPropertyChanged(nameof(GenerateSchemaAttributes));
            }
        }

        private bool _GenerateSchemaAttributesProperties = false;
        /// <summary>
        /// Генерировать атрибуты в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateSchemaAttributesProperties
        {
            get => _GenerateSchemaAttributesProperties;
            set
            {
                this.OnPropertyChanging(nameof(GenerateSchemaAttributesProperties));
                this._GenerateSchemaAttributesProperties = value;
                this.OnPropertyChanged(nameof(GenerateSchemaAttributesProperties));
            }
        }

        private bool _GenerateSchemaManyToOne = true;
        /// <summary>
        /// Генерировать связи Many To One (N:1) в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateSchemaManyToOne
        {
            get => _GenerateSchemaManyToOne;
            set
            {
                this.OnPropertyChanging(nameof(GenerateSchemaManyToOne));
                this._GenerateSchemaManyToOne = value;
                this.OnPropertyChanged(nameof(GenerateSchemaManyToOne));
            }
        }

        private bool _GenerateSchemaOneToMany = true;
        /// <summary>
        /// Генерировать One To Many (1:N) в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateSchemaOneToMany
        {
            get => _GenerateSchemaOneToMany;
            set
            {
                this.OnPropertyChanging(nameof(GenerateSchemaOneToMany));
                this._GenerateSchemaOneToMany = value;
                this.OnPropertyChanged(nameof(GenerateSchemaOneToMany));
            }
        }

        private bool _GenerateSchemaManyToMany = true;
        /// <summary>
        /// Генерировать Many To Many (N:N) в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateSchemaManyToMany
        {
            get => _GenerateSchemaManyToMany;
            set
            {
                this.OnPropertyChanging(nameof(GenerateSchemaManyToMany));
                this._GenerateSchemaManyToMany = value;
                this.OnPropertyChanged(nameof(GenerateSchemaManyToMany));
            }
        }

        private bool _GenerateSchemaLocalOptionSet = true;
        /// <summary>
        /// Генерировать enum для значений Picklist-ов в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateSchemaLocalOptionSet
        {
            get => _GenerateSchemaLocalOptionSet;
            set
            {
                this.OnPropertyChanging(nameof(GenerateSchemaLocalOptionSet));
                this._GenerateSchemaLocalOptionSet = value;
                this.OnPropertyChanged(nameof(GenerateSchemaLocalOptionSet));
            }
        }

        private bool _GenerateSchemaGlobalOptionSet = false;
        [DataMember]
        public bool GenerateSchemaGlobalOptionSet
        {
            get => _GenerateSchemaGlobalOptionSet;
            set
            {
                this.OnPropertyChanging(nameof(GenerateSchemaGlobalOptionSet));
                this._GenerateSchemaGlobalOptionSet = value;
                this.OnPropertyChanged(nameof(GenerateSchemaGlobalOptionSet));
            }
        }

        private bool _GenerateSchemaStatusOptionSet = true;
        /// <summary>
        /// Генерировать enum для значения Statecode и Statuscode в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateSchemaStatusOptionSet
        {
            get => _GenerateSchemaStatusOptionSet;
            set
            {
                this.OnPropertyChanging(nameof(GenerateSchemaStatusOptionSet));
                this._GenerateSchemaStatusOptionSet = value;
                this.OnPropertyChanged(nameof(GenerateSchemaStatusOptionSet));
            }
        }

        private bool _GenerateSchemaKeys = true;
        /// <summary>
        /// Генерировать уникальные ключи
        /// </summary>
        [DataMember]
        public bool GenerateSchemaKeys
        {
            get => _GenerateSchemaKeys;
            set
            {
                this.OnPropertyChanging(nameof(GenerateSchemaKeys));
                this._GenerateSchemaKeys = value;
                this.OnPropertyChanged(nameof(GenerateSchemaKeys));
            }
        }

        private bool _GenerateSchemaIntoSchemaClass = true;
        [DataMember]
        public bool GenerateSchemaIntoSchemaClass
        {
            get => _GenerateSchemaIntoSchemaClass;
            set
            {
                this.OnPropertyChanging(nameof(GenerateSchemaIntoSchemaClass));
                this._GenerateSchemaIntoSchemaClass = value;
                this.OnPropertyChanged(nameof(GenerateSchemaIntoSchemaClass));
            }
        }

        private void LoadFromDiskEntitySchemaClass(FileGenerationOptions diskData)
        {
            this.GenerateSchemaIntoSchemaClass = diskData.GenerateSchemaIntoSchemaClass;

            this.GenerateSchemaAttributes = diskData.GenerateSchemaAttributes;
            this.GenerateSchemaAttributesProperties = diskData.GenerateSchemaAttributesProperties;
            this.GenerateSchemaManyToOne = diskData.GenerateSchemaManyToOne;
            this.GenerateSchemaOneToMany = diskData.GenerateSchemaOneToMany;
            this.GenerateSchemaManyToMany = diskData.GenerateSchemaManyToMany;
            this.GenerateSchemaLocalOptionSet = diskData.GenerateSchemaLocalOptionSet;
            this.GenerateSchemaGlobalOptionSet = diskData.GenerateSchemaGlobalOptionSet;
            this.GenerateSchemaStatusOptionSet = diskData.GenerateSchemaStatusOptionSet;
            this.GenerateSchemaKeys = diskData.GenerateSchemaKeys;

            this.GenerateSchemaConstantType = diskData.GenerateSchemaConstantType;
            this.GenerateSchemaOptionSetExportType = diskData.GenerateSchemaOptionSetExportType;

            this.GenerateSchemaEntityOptionSetsWithDependentComponents = diskData.GenerateSchemaEntityOptionSetsWithDependentComponents;

            this.GenerateSchemaAddDescriptionAttribute = diskData.GenerateSchemaAddDescriptionAttribute;
        }
    }
}
