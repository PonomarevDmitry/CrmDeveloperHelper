using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public partial class FileGenerationOptions
    {
        private bool _GenerateSchemaGlobalOptionSetsWithDependentComponents = true;
        [DataMember]
        public bool GenerateSchemaGlobalOptionSetsWithDependentComponents
        {
            get => _GenerateSchemaGlobalOptionSetsWithDependentComponents;
            set
            {
                this.OnPropertyChanging(nameof(GenerateSchemaGlobalOptionSetsWithDependentComponents));
                this._GenerateSchemaGlobalOptionSetsWithDependentComponents = value;
                this.OnPropertyChanged(nameof(GenerateSchemaGlobalOptionSetsWithDependentComponents));
            }
        }

        private string _NamespaceGlobalOptionSetsCSharp;
        [DataMember]
        public string NamespaceGlobalOptionSetsCSharp
        {
            get => _NamespaceGlobalOptionSetsCSharp;
            set
            {
                this.OnPropertyChanging(nameof(NamespaceGlobalOptionSetsCSharp));

                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }
                else
                {
                    value = string.Empty;
                }

                this._NamespaceGlobalOptionSetsCSharp = value;
                this.OnPropertyChanged(nameof(NamespaceGlobalOptionSetsCSharp));
            }
        }

        private void LoadFromDiskGlobalOptionSetSchema(FileGenerationOptions diskData)
        {
            this.NamespaceGlobalOptionSetsCSharp = diskData.NamespaceGlobalOptionSetsCSharp;
            this.GenerateSchemaGlobalOptionSetsWithDependentComponents = diskData.GenerateSchemaGlobalOptionSetsWithDependentComponents;
        }
    }
}
