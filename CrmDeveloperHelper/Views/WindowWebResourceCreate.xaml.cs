using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowWebResourceCreate : WindowBase
    {
        private string _publisherPrefix;

        public string WebResourceName
        {
            get
            {
                return txtBFinalName.Text.Trim();
            }
        }

        public string WebResourceDisplayName
        {
            get
            {
                return txtBDisplayName.Text.Trim();
            }
        }

        public string WebResourceDescription
        {
            get
            {
                return txtBDescription.Text.Trim();
            }
        }

        public WindowWebResourceCreate(string fileName, string pathName, string solutionName, string publisherPrefix)
        {
            InitializeComponent();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._publisherPrefix = publisherPrefix;

            txtBName.Text = fileName;
            txtBDisplayName.Text = fileName;

            txtBFilePath.Text = pathName.Replace('\\', '/');

            txtBSolutionName.Text = solutionName;
            txtBPublisherPrefix.Text = publisherPrefix;

            DisplayWebResourceName();

            txtBName.SelectionLength = 0;
            txtBName.SelectionStart = txtBName.Text.Length;

            txtBName.Focus();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

            this.Close();
        }

        private void DisplayWebResourceName()
        {
            string name = txtBName.Text.Trim();

            name = WebResourceRepository.GenerateWebResouceName(name, this._publisherPrefix);

            txtBFinalName.Text = name;
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;

            this.Close();
        }

        private void txtBName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            DisplayWebResourceName();
        }
    }
}
