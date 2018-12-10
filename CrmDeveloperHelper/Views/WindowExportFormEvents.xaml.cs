using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExportFormEvents : WindowBase
    {
        private CommonConfiguration _commonConfig;

        public WindowExportFormEvents(CommonConfiguration commonConfig)
        {
            InitializeComponent();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._commonConfig = commonConfig;

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
            chBOnlyWithLibraries.DataContext = _commonConfig;

            cmBFileAction.DataContext = _commonConfig;
        }

        private void btnCreateFile_Click(object sender, RoutedEventArgs e)
        {
            string folder = txtBFolder.Text.Trim();
            string fileName = txtBFileName.Text.Trim();

            if (string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(fileName))
            {
                MessageBox.Show(Properties.MessageBoxStrings.FolderOrFileNameIsEmpty, Properties.MessageBoxStrings.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
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
