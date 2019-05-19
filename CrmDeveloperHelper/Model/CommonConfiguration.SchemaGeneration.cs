using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public partial class CommonConfiguration
    {
        private int _SpaceCount = 4;
        /// <summary>
        /// Количество пробелов для отступа в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public int SpaceCount
        {
            get => _SpaceCount;
            set
            {
                if (_SpaceCount == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(SpaceCount));
                this._SpaceCount = value;
                this.OnPropertyChanged(nameof(SpaceCount));
            }
        }

        private IndentType _IndentType = IndentType.Spaces;
        /// <summary>
        /// Тип отступа в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public IndentType IndentType
        {
            get => _IndentType;
            set
            {
                if (_IndentType == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IndentType));
                this._IndentType = value;
                this.OnPropertyChanged(nameof(IndentType));
            }
        }

        private ConstantType _ConstantType = ConstantType.Constant;
        /// <summary>
        /// Тип записей в файле с метаданными сущности. const или read only
        /// </summary>
        [DataMember]
        public ConstantType ConstantType
        {
            get => _ConstantType;
            set
            {
                if (_ConstantType == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ConstantType));
                this._ConstantType = value;
                this.OnPropertyChanged(nameof(ConstantType));
            }
        }

        private OptionSetExportType _OptionSetExportType = OptionSetExportType.Enums;
        [DataMember]
        public OptionSetExportType OptionSetExportType
        {
            get => _OptionSetExportType;
            set
            {
                if (_OptionSetExportType == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(OptionSetExportType));
                this._OptionSetExportType = value;
                this.OnPropertyChanged(nameof(OptionSetExportType));
            }
        }

        private bool _AllDescriptions;
        /// <summary>
        /// Генерировать все описания (description) или только первое по приоритету в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool AllDescriptions
        {
            get => _AllDescriptions;
            set
            {
                this.OnPropertyChanging(nameof(AllDescriptions));
                this._AllDescriptions = value;
                this.OnPropertyChanged(nameof(AllDescriptions));
            }
        }

        private bool _EntityMetadaOptionSetDependentComponents;
        [DataMember]
        public bool EntityMetadaOptionSetDependentComponents
        {
            get => _EntityMetadaOptionSetDependentComponents;
            set
            {
                this.OnPropertyChanging(nameof(EntityMetadaOptionSetDependentComponents));
                this._EntityMetadaOptionSetDependentComponents = value;
                this.OnPropertyChanged(nameof(EntityMetadaOptionSetDependentComponents));
            }
        }

        private bool _GlobalOptionSetsWithDependentComponents;
        [DataMember]
        public bool GlobalOptionSetsWithDependentComponents
        {
            get => _GlobalOptionSetsWithDependentComponents;
            set
            {
                this.OnPropertyChanging(nameof(GlobalOptionSetsWithDependentComponents));
                this._GlobalOptionSetsWithDependentComponents = value;
                this.OnPropertyChanged(nameof(GlobalOptionSetsWithDependentComponents));
            }
        }

        private bool _AllDependentComponentsForAttributes;
        /// <summary>
        /// Отображать все зависимые элементы
        /// </summary>
        [DataMember]
        public bool AllDependentComponentsForAttributes
        {
            get => _AllDependentComponentsForAttributes;
            set
            {
                this.OnPropertyChanging(nameof(AllDependentComponentsForAttributes));
                this._AllDependentComponentsForAttributes = value;
                this.OnPropertyChanged(nameof(AllDependentComponentsForAttributes));
            }
        }

        private bool _GenerateAddDescriptionAttribute = true;
        [DataMember]
        public bool GenerateAddDescriptionAttribute
        {
            get => _GenerateAddDescriptionAttribute;
            set
            {
                this.OnPropertyChanging(nameof(GenerateAddDescriptionAttribute));
                this._GenerateAddDescriptionAttribute = value;
                this.OnPropertyChanged(nameof(GenerateAddDescriptionAttribute));
            }
        }

        private bool _GenerateAttributesSchema = true;
        /// <summary>
        /// Генерировать атрибуты в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateAttributesSchema
        {
            get => _GenerateAttributesSchema;
            set
            {
                this.OnPropertyChanging(nameof(GenerateAttributesSchema));
                this._GenerateAttributesSchema = value;
                this.OnPropertyChanged(nameof(GenerateAttributesSchema));
            }
        }

        private bool _GenerateManyToOneSchema = true;
        /// <summary>
        /// Генерировать связи Many To One (N:1) в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateManyToOneSchema
        {
            get => _GenerateManyToOneSchema;
            set
            {
                this.OnPropertyChanging(nameof(GenerateManyToOneSchema));
                this._GenerateManyToOneSchema = value;
                this.OnPropertyChanged(nameof(GenerateManyToOneSchema));
            }
        }

        private bool _GenerateOneToManySchema = true;
        /// <summary>
        /// Генерировать One To Many (1:N) в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateOneToManySchema
        {
            get => _GenerateOneToManySchema;
            set
            {
                this.OnPropertyChanging(nameof(GenerateOneToManySchema));
                this._GenerateOneToManySchema = value;
                this.OnPropertyChanged(nameof(GenerateOneToManySchema));
            }
        }

        private bool _GenerateManyToManySchema = true;
        /// <summary>
        /// Генерировать Many To Many (N:N) в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateManyToManySchema
        {
            get => _GenerateManyToManySchema;
            set
            {
                this.OnPropertyChanging(nameof(GenerateManyToManySchema));
                this._GenerateManyToManySchema = value;
                this.OnPropertyChanged(nameof(GenerateManyToManySchema));
            }
        }

        private bool _GenerateLocalOptionSetSchema = true;
        /// <summary>
        /// Генерировать enum для значений Picklist-ов в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateLocalOptionSetSchema
        {
            get => _GenerateLocalOptionSetSchema;
            set
            {
                this.OnPropertyChanging(nameof(GenerateLocalOptionSetSchema));
                this._GenerateLocalOptionSetSchema = value;
                this.OnPropertyChanged(nameof(GenerateLocalOptionSetSchema));
            }
        }

        private bool _GenerateGlobalOptionSetSchema = false;
        [DataMember]
        public bool GenerateGlobalOptionSetSchema
        {
            get => _GenerateGlobalOptionSetSchema;
            set
            {
                this.OnPropertyChanging(nameof(GenerateGlobalOptionSetSchema));
                this._GenerateGlobalOptionSetSchema = value;
                this.OnPropertyChanged(nameof(GenerateGlobalOptionSetSchema));
            }
        }

        private bool _GenerateStatusOptionSetSchema = true;
        /// <summary>
        /// Генерировать enum для значения Statecode и Statuscode в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateStatusOptionSetSchema
        {
            get => _GenerateStatusOptionSetSchema;
            set
            {
                this.OnPropertyChanging(nameof(GenerateStatusOptionSetSchema));
                this._GenerateStatusOptionSetSchema = value;
                this.OnPropertyChanged(nameof(GenerateStatusOptionSetSchema));
            }
        }

        private bool _GenerateKeysSchema = true;
        /// <summary>
        /// Генерировать уникальные ключи
        /// </summary>
        [DataMember]
        public bool GenerateKeysSchema
        {
            get => _GenerateKeysSchema;
            set
            {
                this.OnPropertyChanging(nameof(GenerateKeysSchema));
                this._GenerateKeysSchema = value;
                this.OnPropertyChanged(nameof(GenerateKeysSchema));
            }
        }

        private bool _GenerateIntoSchemaClass;
        [DataMember]
        public bool GenerateIntoSchemaClass
        {
            get => _GenerateIntoSchemaClass;
            set
            {
                this.OnPropertyChanging(nameof(GenerateIntoSchemaClass));
                this._GenerateIntoSchemaClass = value;
                this.OnPropertyChanged(nameof(GenerateIntoSchemaClass));
            }
        }

        private void LoadFromDiskSchema(CommonConfiguration diskData)
        {
            this.GenerateIntoSchemaClass = diskData.GenerateIntoSchemaClass;

            this.GenerateAttributesSchema = diskData.GenerateAttributesSchema;
            this.GenerateManyToOneSchema = diskData.GenerateManyToOneSchema;
            this.GenerateOneToManySchema = diskData.GenerateOneToManySchema;
            this.GenerateManyToManySchema = diskData.GenerateManyToManySchema;
            this.GenerateLocalOptionSetSchema = diskData.GenerateLocalOptionSetSchema;
            this.GenerateGlobalOptionSetSchema = diskData.GenerateGlobalOptionSetSchema;
            this.GenerateStatusOptionSetSchema = diskData.GenerateStatusOptionSetSchema;
            this.GenerateKeysSchema = diskData.GenerateKeysSchema;

            this.SpaceCount = diskData.SpaceCount;
            this.IndentType = diskData.IndentType;
            this.ConstantType = diskData.ConstantType;
            this.OptionSetExportType = diskData.OptionSetExportType;

            this.AllDescriptions = diskData.AllDescriptions;
            this.EntityMetadaOptionSetDependentComponents = diskData.EntityMetadaOptionSetDependentComponents;
            this.AllDependentComponentsForAttributes = diskData.AllDependentComponentsForAttributes;
            this.GlobalOptionSetsWithDependentComponents = diskData.GlobalOptionSetsWithDependentComponents;

            this.GenerateAddDescriptionAttribute = diskData.GenerateAddDescriptionAttribute;
        }
    }
}
