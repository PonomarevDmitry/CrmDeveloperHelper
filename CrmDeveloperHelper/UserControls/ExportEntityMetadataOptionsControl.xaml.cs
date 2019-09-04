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