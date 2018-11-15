using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowPluginConfiguration : WindowBase
    {
        private CommonConfiguration _commonConfig;

        private bool _showExportFolder;

        public WindowPluginConfiguration(CommonConfiguration commonConfig, bool showExportFolder)
        {
            InitializeComponent();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._commonConfig = commonConfig;
            this._showExportFolder = showExportFolder;

            txtBFolder.IsReadOnly = !showExportFolder;

            LoadConfigs();

            txtBFileName.Focus();
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

        private void LoadConfigs()
        {
            txtBFolder.DataContext = _commonConfig;
            txtBFileName.DataContext = _commonConfig;

            cmBFileAction.DataContext = _commonConfig;
        }

        private void btnCreateFile_Click(object sender, RoutedEventArgs e)
        {
            string folder = txtBFolder.Text.Trim();
            string fileName = txtBFileName.Text.Trim();

            StringBuilder message = new StringBuilder();

            if (this._showExportFolder)
            {
                if (string.IsNullOrEmpty(folder))
                {
                    if (message.Length > 0) { message.AppendLine(); }

                    message.Append(Properties.MessageBoxStrings.FolderOrFileNameIsEmpty);
                }
                else
                {
                    if (!Directory.Exists(folder))
                    {
                        if (message.Length > 0) { message.AppendLine(); }

                        message.Append(Properties.MessageBoxStrings.FolderDoesNotExists);
                    }
                }
            }

            if (string.IsNullOrEmpty(fileName))
            {
                if (message.Length > 0) { message.AppendLine(); }

                message.Append(Properties.MessageBoxStrings.FolderOrFileNameIsEmpty);
            }

            if (message.Length > 0)
            {
                MessageBox.Show(message.ToString(), Properties.MessageBoxStrings.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.DialogResult = true;
            this.Close();
        }
    }
}
