using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Windows.Controls;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls
{
    /// <summary>
    /// Interaction logic for ExportEntityMetadataOptionsControl.xaml
    /// </summary>
    public partial class ExportEntityMetadataOptionsControl : UserControl
    {
        private CommonConfiguration _commonConfig;

        public ExportEntityMetadataOptionsControl(CommonConfiguration commonConfig)
        {
            InitializeComponent();

            this._commonConfig = commonConfig;

            LoadFromConfig();
        }

        private void LoadFromConfig()
        {
            txtBSpaceCount.DataContext = _commonConfig;

            rBTab.DataContext = _commonConfig;
            rBSpaces.DataContext = _commonConfig;

            rBClasses.DataContext = _commonConfig;
            rBEnums.DataContext = _commonConfig;

            rBReadOnly.DataContext = _commonConfig;
            rBConst.DataContext = _commonConfig;

            chBAttributes.DataContext = _commonConfig;
            chBManyToOne.DataContext = _commonConfig;
            chBManyToMany.DataContext = _commonConfig;
            chBOneToMany.DataContext = _commonConfig;
            chBLocalOptionSets.DataContext = _commonConfig;
            chBGlobalOptionSets.DataContext = _commonConfig;
            chBStatus.DataContext = _commonConfig;
            chBKeys.DataContext = _commonConfig;

            chBIntoSchemaClass.DataContext = _commonConfig;

            chBAllDescriptions.DataContext = _commonConfig;

            chBWithDependentComponents.DataContext = _commonConfig;

            chBWithManagedInfo.DataContext = _commonConfig;
        }
    }
}