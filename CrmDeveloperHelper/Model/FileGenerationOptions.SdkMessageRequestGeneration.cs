using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public partial class FileGenerationOptions
    {
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

        private bool _GenerateSdkMessageRequestMakeAllPropertiesEditable = false;
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

        private void LoadFromDiskSdkMessageRequest(FileGenerationOptions diskData)
        {
            this.GenerateSdkMessageRequestWithDebuggerNonUserCode = diskData.GenerateSdkMessageRequestWithDebuggerNonUserCode;

            this.GenerateSdkMessageRequestMakeAllPropertiesEditable = diskData.GenerateSdkMessageRequestMakeAllPropertiesEditable;

            this.GenerateSdkMessageRequestAttributesWithNameOf = diskData.GenerateSdkMessageRequestAttributesWithNameOf;
        }
    }
}
