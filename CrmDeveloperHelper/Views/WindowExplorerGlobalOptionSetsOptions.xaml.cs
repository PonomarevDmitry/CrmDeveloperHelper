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

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._fileGenerationOptions = fileGenerationOptions;

            options.BindFileGenerationOptions(_fileGenerationOptions);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            FileGenerationConfiguration.SaveConfiguration();
        }

        private void options_CloseClicked(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}