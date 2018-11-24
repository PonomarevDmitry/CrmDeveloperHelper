using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls
{
    /// <summary>
    /// Interaction logic for ExportEntityRibbonOptionsControl.xaml
    /// </summary>
    public partial class ExportEntityRibbonOptionsControl : UserControl
    {
        private CommonConfiguration _commonConfig;

        public ExportEntityRibbonOptionsControl(CommonConfiguration commonConfig)
        {
            InitializeComponent();

            this._commonConfig = commonConfig;

            LoadFromConfig();
        }

        private void LoadFromConfig()
        {
            chBForm.DataContext = _commonConfig;
            chBHomepageGrid.DataContext = _commonConfig;
            chBSubGrid.DataContext = _commonConfig;

            chBXmlAttributeOnNewLine.DataContext = _commonConfig;

            chBSetXmlSchemas.DataContext = _commonConfig;

            chBSetIntellisenseContext.DataContext = _commonConfig;
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