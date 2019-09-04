using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls
{
    public partial class ExportGlobalOptionSetMetadataOptionsControl : UserControl
    {
        public ExportGlobalOptionSetMetadataOptionsControl()
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

            chBAllDescriptions.DataContext = fileGenerationOptions;

            chBWithDependentComponents.DataContext = fileGenerationOptions;

            chBWithManagedInfo.DataContext = fileGenerationOptions;

            chBSchemaAddDescriptionAttribute.DataContext = fileGenerationOptions;

            chBSchemaAddTypeConverterAttributeForEnums.DataContext = fileGenerationOptions;

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