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
        private readonly CommonConfiguration _commonConfig;

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

            chBAttributesProxyClass.DataContext = _commonConfig;
            chBManyToOneProxyClass.DataContext = _commonConfig;
            chBManyToManyProxyClass.DataContext = _commonConfig;
            chBOneToManyProxyClass.DataContext = _commonConfig;
            chBLocalOptionSetsProxyClass.DataContext = _commonConfig;
            chBGlobalOptionSetsProxyClass.DataContext = _commonConfig;
            chBStatusProxyClass.DataContext = _commonConfig;

            chBProxyClassWithDebuggerNonUserCode.DataContext = _commonConfig;
            chBAttributesProxyClassWithNameOf.DataContext = _commonConfig;
            chBProxyClassUseSchemaConstInCSharpAttributes.DataContext = _commonConfig;
            chBProxyClassesWithoutObsoleteAttribute.DataContext = _commonConfig;
            chBProxyClassesMakeAllPropertiesEditable.DataContext = _commonConfig;
            chBProxyClassesAddConstructorWithAnonymousTypeObject.DataContext = _commonConfig;

            chBAttributesSchema.DataContext = _commonConfig;
            chBManyToOneSchema.DataContext = _commonConfig;
            chBManyToManySchema.DataContext = _commonConfig;
            chBOneToManySchema.DataContext = _commonConfig;
            chBLocalOptionSetsSchema.DataContext = _commonConfig;
            chBGlobalOptionSetsSchema.DataContext = _commonConfig;
            chBStatusSchema.DataContext = _commonConfig;
            chBKeysSchema.DataContext = _commonConfig;

            chBIntoSchemaClass.DataContext = _commonConfig;

            chBAllDescriptions.DataContext = _commonConfig;

            chBWithDependentComponents.DataContext = _commonConfig;

            chBWithManagedInfo.DataContext = _commonConfig;





            chBForm.DataContext = _commonConfig;
            chBHomepageGrid.DataContext = _commonConfig;
            chBSubGrid.DataContext = _commonConfig;

            chBSortRibbonCommandsAndRulesById.DataContext = _commonConfig;
            chBSortXmlAttributes.DataContext = _commonConfig;

            chBXmlAttributeOnNewLine.DataContext = _commonConfig;

            chBSetXmlSchemas.DataContext = _commonConfig;

            chBSetIntellisenseContext.DataContext = _commonConfig;






            rBAllcomponents.DataContext = _commonConfig;
            rBWorkflowPlugin.DataContext = _commonConfig;
        }

        public event EventHandler<EventArgs> CloseClicked;

        private void OnCloseClicked()
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