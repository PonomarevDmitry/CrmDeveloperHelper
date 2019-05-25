using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public partial class CommonConfiguration
    {
        private bool _GenerateSchemaGlobalOptionSetsWithDependentComponents;
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

        private void LoadFromDiskGlobalOptionSetSchema(CommonConfiguration diskData)
        {
            this.GenerateSchemaGlobalOptionSetsWithDependentComponents = diskData.GenerateSchemaGlobalOptionSetsWithDependentComponents;
        }
    }
}
