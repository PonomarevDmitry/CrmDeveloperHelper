using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Globalization;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExplorerEntityMetadataOptions : WindowBase
    {
        private readonly FileGenerationOptions _fileGenerationOptions;

        public WindowExplorerEntityMetadataOptions(FileGenerationOptions fileGenerationOptions)
        {
            InitializeComponent();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._fileGenerationOptions = fileGenerationOptions;

            options.BindFileGenerationOptions(this._fileGenerationOptions);
        }

        protected override void OnClosed(EventArgs e)
        {
            FileGenerationConfiguration.SaveConfiguration();

            base.OnClosed(e);
        }
    }
}