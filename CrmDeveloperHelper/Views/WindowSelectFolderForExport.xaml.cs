using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowSelectFolderForExport : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        public string SelectedFolder => txtBFolder.Text.Trim();

        public WindowSelectFolderForExport(ConnectionData connectionData
            , string folder
            , FileAction fileAction
        )
        {
            InitializeComponent();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            txtBFolder.Text = folder;

            cmBFileAction.SelectedItem = fileAction;

            if (connectionData != null)
            {
                BindingOperations.EnableCollectionSynchronization(connectionData.ConnectionConfiguration.Connections, sysObjectConnections);

                cmBConnection.ItemsSource = connectionData.ConnectionConfiguration.Connections;
                cmBConnection.SelectedItem = connectionData;
            }
            else
            {
                lblConnection.IsEnabled = cmBConnection.IsEnabled = false;
                lblConnection.Visibility = cmBConnection.Visibility = Visibility.Collapsed;
            }

            txtBFolder.Focus();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

            this.Close();
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

        public FileAction GetFileAction()
        {
            if (cmBFileAction.SelectedIndex != -1)
            {
                return (FileAction)cmBFileAction.SelectedItem;
            }

            return FileAction.None;
        }

        public ConnectionData GetConnectionData()
        {
            return cmBConnection.SelectedItem as ConnectionData;
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            MakeOkClick();
        }

        private void MakeOkClick()
        {
            string folder = this.SelectedFolder;

            if (string.IsNullOrEmpty(folder))
            {
                MessageBox.Show(Properties.MessageBoxStrings.FolderDoesNotExists, Properties.MessageBoxStrings.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Directory.Exists(folder))
            {
                MessageBox.Show(Properties.MessageBoxStrings.FolderDoesNotExists, Properties.MessageBoxStrings.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.DialogResult = true;
            this.Close();
        }
    }
}
