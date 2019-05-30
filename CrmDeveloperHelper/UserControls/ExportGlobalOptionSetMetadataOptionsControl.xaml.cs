using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls
{
    /// <summary>
    /// Interaction logic for ExportGlobalOptionSetMetadataOptionsControl.xaml
    /// </summary>
    public partial class ExportGlobalOptionSetMetadataOptionsControl : UserControl
    {
        public ExportGlobalOptionSetMetadataOptionsControl()
        {
            InitializeComponent();
        }

        public ExportGlobalOptionSetMetadataOptionsControl(CommonConfiguration commonConfig)
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

            chBAllDescriptions.DataContext = commonConfig;

            chBWithDependentComponents.DataContext = commonConfig;

            chBWithManagedInfo.DataContext = commonConfig;

            chBSchemaAddDescriptionAttribute.DataContext = commonConfig;

            chBSchemaAddTypeConverterAttributeForEnums.DataContext = commonConfig;
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