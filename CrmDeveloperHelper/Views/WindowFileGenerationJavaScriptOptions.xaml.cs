using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Globalization;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowFileGenerationJavaScriptOptions : WindowBase
    {
        private readonly FileGenerationOptions _fileGenerationOptions;

        public WindowFileGenerationJavaScriptOptions(FileGenerationOptions fileGenerationOptions)
        {
            InitializeComponent();

            SetInputLanguageEnglish();

            this._fileGenerationOptions = fileGenerationOptions;

            this.Title = string.Format("JavaScript File Generation Options - {0}", !string.IsNullOrEmpty(fileGenerationOptions.SolutionFilePath) ? fileGenerationOptions.SolutionFilePath : "Default");

            options.BindFileGenerationOptions(this._fileGenerationOptions);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            _fileGenerationOptions?.Configuration?.Save();
        }

        private void options_CloseClicked(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}