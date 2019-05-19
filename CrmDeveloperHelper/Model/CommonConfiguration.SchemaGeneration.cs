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

        private bool _GenerateSchemaIntoSchemaClass;
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

        private void LoadFromDiskSchema(CommonConfiguration diskData)
        {
            this.GenerateSchemaIntoSchemaClass = diskData.GenerateSchemaIntoSchemaClass;

            this.GenerateSchemaAttributes = diskData.GenerateSchemaAttributes;
            this.GenerateSchemaManyToOne = diskData.GenerateSchemaManyToOne;
            this.GenerateSchemaOneToMany = diskData.GenerateSchemaOneToMany;
            this.GenerateSchemaManyToMany = diskData.GenerateSchemaManyToMany;
            this.GenerateSchemaLocalOptionSet = diskData.GenerateSchemaLocalOptionSet;
            this.GenerateSchemaGlobalOptionSet = diskData.GenerateSchemaGlobalOptionSet;
            this.GenerateSchemaStatusOptionSet = diskData.GenerateSchemaStatusOptionSet;
            this.GenerateSchemaKeys = diskData.GenerateSchemaKeys;

            this.SpaceCount = diskData.SpaceCount;
            this.IndentType = diskData.IndentType;

            this.ConstantType = diskData.ConstantType;
            this.OptionSetExportType = diskData.OptionSetExportType;

            this.AllDescriptions = diskData.AllDescriptions;
            this.EntityMetadaOptionSetDependentComponents = diskData.EntityMetadaOptionSetDependentComponents;
            this.AllDependentComponentsForAttributes = diskData.AllDependentComponentsForAttributes;
            this.GlobalOptionSetsWithDependentComponents = diskData.GlobalOptionSetsWithDependentComponents;

            this.GenerateSchemaAddDescriptionAttribute = diskData.GenerateSchemaAddDescriptionAttribute;
        }
    }
}
