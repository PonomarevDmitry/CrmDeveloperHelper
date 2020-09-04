using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Globalization;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExplorerGlobalOptionSetsOptions : WindowBase
    {
        private readonly FileGenerationOptions _fileGenerationOptions;

        public WindowExplorerGlobalOptionSetsOptions(FileGenerationOptions fileGenerationOptions)
        {
            InitializeComponent();

            SetInputLanguageEnglish();

            this._fileGenerationOptions = fileGenerationOptions;

            this.Title = string.Format("Global OptionSets File Generation Options - {0}", !string.IsNullOrEmpty(fileGenerationOptions.SolutionFilePath) ? fileGenerationOptions.SolutionFilePath : "Default");

            options.BindFileGenerationOptions(_fileGenerationOptions);
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