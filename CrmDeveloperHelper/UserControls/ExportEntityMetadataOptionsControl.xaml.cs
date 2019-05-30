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
        public ExportEntityMetadataOptionsControl()
        {
            InitializeComponent();
        }

        public ExportEntityMetadataOptionsControl(CommonConfiguration commonConfig)
            : this()
        {
            BindCommonConfiguration(commonConfig);
        }

        public void BindCommonConfiguration(CommonConfiguration commonConfig)
        {
            txtBSpaceCount.DataContext = commonConfig;

            rBTab.DataContext = commonConfig;
            rBSpaces.DataContext = commonConfig;

            rBClasses.DataContext = commonConfig;
            rBEnums.DataContext = commonConfig;

            rBReadOnly.DataContext = commonConfig;
            rBConst.DataContext = commonConfig;

            chBAttributesProxyClass.DataContext = commonConfig;
            chBManyToOneProxyClass.DataContext = commonConfig;
            chBManyToManyProxyClass.DataContext = commonConfig;
            chBOneToManyProxyClass.DataContext = commonConfig;
            chBLocalOptionSetsProxyClass.DataContext = commonConfig;
            chBGlobalOptionSetsProxyClass.DataContext = commonConfig;
            chBStatusProxyClass.DataContext = commonConfig;

            chBProxyClassWithDebuggerNonUserCode.DataContext = commonConfig;
            chBAttributesProxyClassWithNameOf.DataContext = commonConfig;
            chBProxyClassUseSchemaConstInCSharpAttributes.DataContext = commonConfig;
            chBProxyClassesWithoutObsoleteAttribute.DataContext = commonConfig;
            chBProxyClassesMakeAllPropertiesEditable.DataContext = commonConfig;
            chBProxyClassesAddConstructorWithAnonymousTypeObject.DataContext = commonConfig;

            cmBAttributesProxyClassEnumsStateStatus.DataContext = commonConfig;
            cmBAttributesProxyClassEnumsLocal.DataContext = commonConfig;
            cmBAttributesProxyClassEnumsGlobal.DataContext = commonConfig;

            chBAttributesProxyClassEnumsUseSchemaStateStatusEnum.DataContext = commonConfig;
            chBAttributesProxyClassEnumsUseSchemaLocalEnum.DataContext = commonConfig;
            cmBAttributesProxyClassEnumsUseSchemaGlobalEnum.DataContext = commonConfig;

            chBAddDescriptionAttribute.DataContext = commonConfig;
            chBProxyClassAddDescriptionAttribute.DataContext = commonConfig;

            chBAddTypeConverterAttributeForEnums.DataContext = commonConfig;

            chBAttributesSchema.DataContext = commonConfig;
            chBManyToOneSchema.DataContext = commonConfig;
            chBManyToManySchema.DataContext = commonConfig;
            chBOneToManySchema.DataContext = commonConfig;
            chBLocalOptionSetsSchema.DataContext = commonConfig;
            chBGlobalOptionSetsSchema.DataContext = commonConfig;
            chBStatusSchema.DataContext = commonConfig;
            chBKeysSchema.DataContext = commonConfig;

            chBIntoSchemaClass.DataContext = commonConfig;

            chBAllDescriptions.DataContext = commonConfig;

            chBWithDependentComponents.DataContext = commonConfig;

            chBWithManagedInfo.DataContext = commonConfig;





            chBForm.DataContext = commonConfig;
            chBHomepageGrid.DataContext = commonConfig;
            chBSubGrid.DataContext = commonConfig;

            chBSortRibbonCommandsAndRulesById.DataContext = commonConfig;
            chBSortXmlAttributes.DataContext = commonConfig;

            chBXmlAttributeOnNewLine.DataContext = commonConfig;

            chBSetXmlSchemas.DataContext = commonConfig;

            chBSetIntellisenseContext.DataContext = commonConfig;






            rBAllcomponents.DataContext = commonConfig;
            rBWorkflowPlugin.DataContext = commonConfig;
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