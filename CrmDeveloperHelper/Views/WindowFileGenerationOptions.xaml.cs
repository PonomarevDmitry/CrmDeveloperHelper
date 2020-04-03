using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Globalization;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowFileGenerationOptions : WindowBase
    {
        private readonly FileGenerationOptions _fileGenerationOptions;

        public WindowFileGenerationOptions(FileGenerationOptions fileGenerationOptions)
        {
            InitializeComponent();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._fileGenerationOptions = fileGenerationOptions;

            this.Title = string.Format("File Generation Options - {0}", !string.IsNullOrEmpty(fileGenerationOptions.SolutionFilePath) ? fileGenerationOptions.SolutionFilePath : "Default");

            optionsEntityMetadataOptions.BindFileGenerationOptions(_fileGenerationOptions);
            optionsGlobalOptionSetMetadataOptions.BindFileGenerationOptions(_fileGenerationOptions);
            optionsSdkMessageRequestsOptions.BindFileGenerationOptions(_fileGenerationOptions);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            _fileGenerationOptions.Configuration?.Save();
        }

        private void options_CloseClicked(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}