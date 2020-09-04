using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowSelectFolderAndText : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private readonly CommonConfiguration _commonConfig;
        private readonly Func<string, bool> _checker;
        private readonly string _message;

        public WindowSelectFolderAndText(
            CommonConfiguration commonConfig
            , ConnectionData connectionData
            , string windowTitle
            , string labelTitle
            , Func<string, bool> checker = null
            , string message = null
        )
        {
            InitializeComponent();

            SetInputLanguageEnglish();

            this.Title = windowTitle;
            this._checker = checker;
            this._message = message;
            this._commonConfig = commonConfig;

            BindingOperations.EnableCollectionSynchronization(connectionData.ConnectionConfiguration.Connections, sysObjectConnections);

            LoadConfigs(commonConfig);

            cmBConnection.ItemsSource = connectionData.ConnectionConfiguration.Connections;
            cmBConnection.SelectedItem = connectionData;

            lblText.Content = labelTitle;

            txtBText.Focus();
        }

        protected override void OnClosed(EventArgs e)
        {
            this._commonConfig.Save();

            base.OnClosed(e);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

            this.Close();
        }

        private void LoadConfigs(CommonConfiguration commonConfig)
        {
            txtBFolder.DataContext = commonConfig;

            cmBFileAction.DataContext = commonConfig;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;

                MakeOkClick();
            }

            base.OnKeyDown(e);
        }

        private void MakeOkClick()
        {
            string folder = txtBFolder.Text.Trim();
            string text = txtBText.Text.Trim();

            if (string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(text))
            {
                MessageBox.Show(Properties.MessageBoxStrings.FolderOrTextIsEmpty, Properties.MessageBoxStrings.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Directory.Exists(folder))
            {
                MessageBox.Show(Properties.MessageBoxStrings.FolderDoesNotExists, Properties.MessageBoxStrings.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool isValid = _checker?.Invoke(text) ?? true;

            if (!isValid)
            {
                MessageBox.Show(_message, Properties.MessageBoxStrings.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.DialogResult = true;
            this.Close();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            MakeOkClick();
        }

        public string GetText()
        {
            return txtBText.Text.Trim();
        }

        public ConnectionData GetConnectionData()
        {
            return cmBConnection.SelectedItem as ConnectionData;
        }
    }
}
