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

        private void LoadFromDiskGlobalOptionSetSchema(FileGenerationOptions diskData)
        {
            this.GenerateSchemaGlobalOptionSetsWithDependentComponents = diskData.GenerateSchemaGlobalOptionSetsWithDependentComponents;
        }
    }
}
