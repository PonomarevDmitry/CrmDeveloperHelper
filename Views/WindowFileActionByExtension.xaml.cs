using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowFileActionByExtension : WindowBase
    {
        public string SelectedExtension => txtBExtension.Text.Trim();

        public WindowFileActionByExtension(string extension, FileAction fileAction)
        {
            InitializeComponent();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            txtBExtension.Text = extension;

            cmBFileAction.SelectedItem = fileAction;

            txtBExtension.Focus();
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

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            MakeOkClick();
        }

        private void MakeOkClick()
        {
            string folder = this.SelectedExtension;

            if (string.IsNullOrEmpty(folder))
            {
                MessageBox.Show(Properties.MessageBoxStrings.FolderDoesNotExists, Properties.MessageBoxStrings.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.DialogResult = true;
            this.Close();
        }
    }
}
