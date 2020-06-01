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
            System.Windows.FrameworkElement[] dataContextElements =
            {
                chBForm
                , chBHomepageGrid
                , chBSubGrid

                , chBSortRibbonCommandsAndRulesById
                , chBSortXmlAttributes

                , chBXmlAttributeOnNewLine

                , chBSetXmlSchemas

                , chBSetIntellisenseContext



                , rBAllcomponents
                , rBWorkflowPlugin
            };

            foreach (var element in dataContextElements)
            {
                element.DataContext = commonConfig;
            }
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