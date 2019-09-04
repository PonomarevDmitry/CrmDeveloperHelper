using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExplorerGlobalOptionSetsOptions : WindowBase
    {
        private readonly IWriteToOutput _iWriteToOutput;

        private readonly FileGenerationOptions _fileGenerationOptions;

        public WindowExplorerGlobalOptionSetsOptions(IWriteToOutput iWriteToOutput, ConnectionData connectionData)
        {
            InitializeComponent();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._fileGenerationOptions = FileGenerationConfiguration.GetFileGenerationOptions();
            this._iWriteToOutput = iWriteToOutput;

            options.BindFileGenerationOptions(_fileGenerationOptions);

            cmBCurrentConnection.ItemsSource = connectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = connectionData;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            FileGenerationConfiguration.SaveConfiguration();
        }

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, cmBCurrentConnection.SelectedItem as ConnectionData);
        }

        private void options_CloseClicked(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}