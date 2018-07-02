using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowSelectFolderForExport : WindowBase
    {
        public string SelectedFolder
        {
            get
            {
                return txtBFolder.Text.Trim();
            }
        }

        public WindowSelectFolderForExport(string folder, FileAction fileAction)
        {
            InitializeComponent();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            txtBFolder.Text = folder;

            cmBFileAction.SelectedIndex = (int)fileAction;

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
                return (FileAction)cmBFileAction.SelectedIndex;
            }

            return FileAction.None;
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            MakeOkClick();
        }

        private void MakeOkClick()
        {
            string folder = this.SelectedFolder;

            if (!string.IsNullOrEmpty(folder))
            {
                if (Directory.Exists(folder))
                {
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Folder does not exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Folder does not exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
