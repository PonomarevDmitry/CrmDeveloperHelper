using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Windows.Controls;
using System.Windows.Input;

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





            chBForm.DataContext = _commonConfig;
            chBHomepageGrid.DataContext = _commonConfig;
            chBSubGrid.DataContext = _commonConfig;

            chBXmlAttributeOnNewLine.DataContext = _commonConfig;

            chBSetXmlSchemas.DataContext = _commonConfig;

            chBSetIntellisenseContext.DataContext = _commonConfig;






            rBAllcomponents.DataContext = _commonConfig;
            rBWorkflowPlugin.DataContext = _commonConfig;
        }

        public event EventHandler<EventArgs> CloseClicked;

        public void OnCloseClicked()
        {
            CloseClicked?.Invoke(this, new EventArgs());
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (!e.Handled)
            {
                if (e.Key == Key.Escape
                    || (e.Key == Key.W && e.KeyboardDevice != null && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0)
                    )
                {
                    e.Handled = true;

                    OnCloseClicked();

                    return;
                }
            }

            base.OnKeyDown(e);
        }
    }
}