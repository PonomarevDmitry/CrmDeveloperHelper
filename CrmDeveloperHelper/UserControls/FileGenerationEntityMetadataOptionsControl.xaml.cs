using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls
{
    public partial class FileGenerationEntityMetadataOptionsControl : UserControl
    {
        public FileGenerationEntityMetadataOptionsControl()
        {
            InitializeComponent();
        }

        public void BindFileGenerationOptions(FileGenerationOptions fileGenerationOptions)
        {
            txtBSpaceCount.DataContext = fileGenerationOptions;

            rBTab.DataContext = fileGenerationOptions;
            rBSpaces.DataContext = fileGenerationOptions;

            rBClasses.DataContext = fileGenerationOptions;
            rBEnums.DataContext = fileGenerationOptions;

            rBReadOnly.DataContext = fileGenerationOptions;
            rBConst.DataContext = fileGenerationOptions;

            chBAttributesProxyClass.DataContext = fileGenerationOptions;
            chBManyToOneProxyClass.DataContext = fileGenerationOptions;
            chBManyToManyProxyClass.DataContext = fileGenerationOptions;
            chBOneToManyProxyClass.DataContext = fileGenerationOptions;
            chBLocalOptionSetsProxyClass.DataContext = fileGenerationOptions;
            chBGlobalOptionSetsProxyClass.DataContext = fileGenerationOptions;
            chBStatusProxyClass.DataContext = fileGenerationOptions;

            chBProxyClassWithDebuggerNonUserCode.DataContext = fileGenerationOptions;
            chBAttributesProxyClassWithNameOf.DataContext = fileGenerationOptions;
            chBProxyClassUseSchemaConstInCSharpAttributes.DataContext = fileGenerationOptions;
            chBProxyClassesWithoutObsoleteAttribute.DataContext = fileGenerationOptions;
            chBProxyClassesMakeAllPropertiesEditable.DataContext = fileGenerationOptions;
            chBProxyClassesAddConstructorWithAnonymousTypeObject.DataContext = fileGenerationOptions;

            cmBAttributesProxyClassEnumsStateStatus.DataContext = fileGenerationOptions;
            cmBAttributesProxyClassEnumsLocal.DataContext = fileGenerationOptions;
            cmBAttributesProxyClassEnumsGlobal.DataContext = fileGenerationOptions;

            chBAttributesProxyClassEnumsUseSchemaStateStatusEnum.DataContext = fileGenerationOptions;
            chBAttributesProxyClassEnumsUseSchemaLocalEnum.DataContext = fileGenerationOptions;
            cmBAttributesProxyClassEnumsUseSchemaGlobalEnum.DataContext = fileGenerationOptions;

            chBAddDescriptionAttribute.DataContext = fileGenerationOptions;
            chBProxyClassAddDescriptionAttribute.DataContext = fileGenerationOptions;

            chBAddTypeConverterAttributeForEnums.DataContext = fileGenerationOptions;

            chBAttributesSchema.DataContext = fileGenerationOptions;
            chBManyToOneSchema.DataContext = fileGenerationOptions;
            chBManyToManySchema.DataContext = fileGenerationOptions;
            chBOneToManySchema.DataContext = fileGenerationOptions;
            chBLocalOptionSetsSchema.DataContext = fileGenerationOptions;
            chBGlobalOptionSetsSchema.DataContext = fileGenerationOptions;
            chBStatusSchema.DataContext = fileGenerationOptions;
            chBKeysSchema.DataContext = fileGenerationOptions;

            chBIntoSchemaClass.DataContext = fileGenerationOptions;

            chBAllDescriptions.DataContext = fileGenerationOptions;

            chBWithDependentComponents.DataContext = fileGenerationOptions;

            chBWithManagedInfo.DataContext = fileGenerationOptions;



            txtBNamespaceClassesCSharp.DataContext = fileGenerationOptions;
            txtBNamespaceClassesJavaScript.DataContext = fileGenerationOptions;

            txtBNamespaceGlobalOptionSetsCSharp.DataContext = fileGenerationOptions;
            txtBNamespaceGlobalOptionSetsJavaScript.DataContext = fileGenerationOptions;

            txtBTypeConverterName.DataContext = fileGenerationOptions;
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