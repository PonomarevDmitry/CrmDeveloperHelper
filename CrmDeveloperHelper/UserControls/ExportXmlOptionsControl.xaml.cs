using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls
{
    /// <summary>
    /// Interaction logic for ExportXmlOptionsControl.xaml
    /// </summary>
    public partial class ExportXmlOptionsControl : UserControl
    {
        private CommonConfiguration _commonConfig;

        public ExportXmlOptionsControl(CommonConfiguration commonConfig, XmlOptionsControls controls)
        {
            InitializeComponent();

            this._commonConfig = commonConfig;

            LoadFromConfig();

            grBRibbon.Visibility = (controls & XmlOptionsControls.Ribbon) != 0 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            chBXmlAttributeOnNewLine.Visibility = (controls & XmlOptionsControls.XmlAttributeOnNewLine) != 0 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            chBSetXmlSchemas.Visibility = (controls & XmlOptionsControls.SetXmlSchemas) != 0 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            chBSetIntellisenseContext.Visibility = (controls & XmlOptionsControls.SetIntellisenseContext) != 0 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
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