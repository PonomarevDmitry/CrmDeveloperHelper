using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public partial class CommonConfiguration
    {
        private bool _GenerateSdkMessageRequestAttributes = true;
        /// <summary>
        /// Генерировать атрибуты в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateSdkMessageRequestAttributes
        {
            get => _GenerateSdkMessageRequestAttributes;
            set
            {
                this.OnPropertyChanging(nameof(GenerateSdkMessageRequestAttributes));
                this._GenerateSdkMessageRequestAttributes = value;
                this.OnPropertyChanged(nameof(GenerateSdkMessageRequestAttributes));
            }
        }

        private bool _GenerateSdkMessageRequestUseSchemaConstInCSharpAttributes = true;
        [DataMember]
        public bool GenerateSdkMessageRequestUseSchemaConstInCSharpAttributes
        {
            get => _GenerateSdkMessageRequestUseSchemaConstInCSharpAttributes;
            set
            {
                this.OnPropertyChanging(nameof(GenerateSdkMessageRequestUseSchemaConstInCSharpAttributes));
                this._GenerateSdkMessageRequestUseSchemaConstInCSharpAttributes = value;
                this.OnPropertyChanged(nameof(GenerateSdkMessageRequestUseSchemaConstInCSharpAttributes));
            }
        }

        private bool _GenerateSdkMessageRequestWithDebuggerNonUserCode = true;
        [DataMember]
        public bool GenerateSdkMessageRequestWithDebuggerNonUserCode
        {
            get => _GenerateSdkMessageRequestWithDebuggerNonUserCode;
            set
            {
                this.OnPropertyChanging(nameof(GenerateSdkMessageRequestWithDebuggerNonUserCode));
                this._GenerateSdkMessageRequestWithDebuggerNonUserCode = value;
                this.OnPropertyChanged(nameof(GenerateSdkMessageRequestWithDebuggerNonUserCode));
            }
        }

        private bool _GenerateSdkMessageRequestMakeAllPropertiesEditable = true;
        [DataMember]
        public bool GenerateSdkMessageRequestMakeAllPropertiesEditable
        {
            get => _GenerateSdkMessageRequestMakeAllPropertiesEditable;
            set
            {
                this.OnPropertyChanging(nameof(GenerateSdkMessageRequestMakeAllPropertiesEditable));
                this._GenerateSdkMessageRequestMakeAllPropertiesEditable = value;
                this.OnPropertyChanged(nameof(GenerateSdkMessageRequestMakeAllPropertiesEditable));
            }
        }

        private bool _GenerateSdkMessageRequestAttributesWithNameOf = true;
        [DataMember]
        public bool GenerateSdkMessageRequestAttributesWithNameOf
        {
            get => _GenerateSdkMessageRequestAttributesWithNameOf;
            set
            {
                this.OnPropertyChanging(nameof(GenerateSdkMessageRequestAttributesWithNameOf));
                this._GenerateSdkMessageRequestAttributesWithNameOf = value;
                this.OnPropertyChanged(nameof(GenerateSdkMessageRequestAttributesWithNameOf));
            }
        }

        private void LoadFromDiskSdkMessageRequest(CommonConfiguration diskData)
        {
            this.GenerateSdkMessageRequestAttributes = diskData.GenerateSdkMessageRequestAttributes;

            this.GenerateSdkMessageRequestUseSchemaConstInCSharpAttributes = diskData.GenerateSdkMessageRequestUseSchemaConstInCSharpAttributes;
            this.GenerateSdkMessageRequestWithDebuggerNonUserCode = diskData.GenerateSdkMessageRequestWithDebuggerNonUserCode;
            this.GenerateSdkMessageRequestMakeAllPropertiesEditable = diskData.GenerateSdkMessageRequestMakeAllPropertiesEditable;

            this.GenerateSdkMessageRequestAttributesWithNameOf = diskData.GenerateSdkMessageRequestAttributesWithNameOf;
        }
    }
}
