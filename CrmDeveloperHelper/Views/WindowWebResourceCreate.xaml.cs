using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowWebResourceCreate : WindowBase
    {
        private readonly string _publisherPrefix;
        private readonly string _fileName;
        private readonly string _pathName;

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
                return cmBDisplayName.Text?.Trim() ?? string.Empty;
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
            this._fileName = fileName;
            this._pathName = pathName;

            txtBSolutionName.Text = solutionName;
            txtBPublisherPrefix.Text = publisherPrefix;

            txtBFilePath.Text = pathName.Replace('\\', '/');

            FillComboBoxex();

            DisplayWebResourceName();

            //txtBName.SelectionLength = 0;
            //txtBName.SelectionStart = txtBName.Text.Length;

            cmBName.Focus();
        }

        private void FillComboBoxex()
        {
            cmBName.Items.Clear();
            cmBDisplayName.Items.Clear();

            var pathName = _pathName.Replace('\\', '/').Trim(' ', '/');

            var split = pathName.Split('/');

            string text = string.Empty;

            for (int i = split.Length - 1; i >= 0; i--)
            {
                text = split[i] + (!string.IsNullOrEmpty(text) ? "/" : string.Empty) + text;

                cmBName.Items.Add(text);
                cmBDisplayName.Items.Add(text);
            }

            cmBName.Text = _fileName;
            cmBDisplayName.Text = _fileName;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

            this.Close();
        }

        private void DisplayWebResourceName()
        {
            string name = cmBName.Text?.Trim() ?? string.Empty;

            name = WebResourceRepository.GenerateWebResouceName(name, this._publisherPrefix);

            txtBFinalName.Text = name;
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;

            this.Close();
        }

        private void cmBName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            DisplayWebResourceName();
        }

        private void cmBName_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            DisplayWebResourceName();
        }

        private void cmBName_LostFocus(object sender, RoutedEventArgs e)
        {
            DisplayWebResourceName();
        }
    }
}
