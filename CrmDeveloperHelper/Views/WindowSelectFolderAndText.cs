using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowSelectFolderAndText : WindowBase
    {
        private CommonConfiguration _commonConfig;
        private readonly Func<string, bool> _checker;
        private readonly string _message;

        public WindowSelectFolderAndText(CommonConfiguration commonConfig, string windowTitle, string labelTitle, Func<string, bool> checker = null, string message = null)
        {
            InitializeComponent();

            this.Title = windowTitle;
            this._checker = checker;
            this._message = message;
            lblText.Content = labelTitle;

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._commonConfig = commonConfig;

            LoadConfigs(commonConfig);

            txtBText.Focus();
        }

        protected override void OnClosed(EventArgs e)
        {
            this._commonConfig.FolderForExport = txtBFolder.Text.Trim();
            this._commonConfig.AfterCreateFileAction = GetFileAction();
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
            txtBFolder.Text = commonConfig.FolderForExport;

            cmBFileAction.SelectedIndex = (int)commonConfig.AfterCreateFileAction;
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

            if (!string.IsNullOrEmpty(folder) && !string.IsNullOrEmpty(text))
            {
                if (Directory.Exists(folder))
                {
                    bool isValid = _checker?.Invoke(text) ?? true;

                    if (isValid)
                    {
                        this.DialogResult = true;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(_message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Folder does not exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Folder or Text is empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private FileAction GetFileAction()
        {
            if (cmBFileAction.SelectedIndex != -1)
            {
                return (FileAction)cmBFileAction.SelectedIndex;
            }

            return FileAction.None;
        }

        private void btnCreateFile_Click(object sender, RoutedEventArgs e)
        {
            MakeOkClick();
        }

        public string GetText()
        {
            return txtBText.Text.Trim();
        }
    }
}
