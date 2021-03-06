using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExplorerImportJob : WindowWithConnectionList
    {
        private readonly ObservableCollection<EntityViewItem> _itemsSource;

        private readonly Popup _optionsPopup;

        public WindowExplorerImportJob(
             IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
            , string selection
        ) : base(iWriteToOutput, commonConfig, service)
        {
            this.IncreaseInit();

            InitializeComponent();

            SetInputLanguageEnglish();

            var child = new ExportXmlOptionsControl(_commonConfig, XmlOptionsControls.ImportJobXmlOptions);
            child.CloseClicked += Child_CloseClicked;
            this._optionsPopup = new Popup
            {
                Child = child,

                PlacementTarget = toolBarHeader,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
                Focusable = true,
            };

            LoadFromConfig();

            if (!string.IsNullOrEmpty(selection))
            {
                txtBFilter.Text = selection;
            }

            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            this._itemsSource = new ObservableCollection<EntityViewItem>();

            this.lstVwImportJobs.ItemsSource = _itemsSource;

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            chBOpenFormattedResultsInExcel.IsEnabled = IsExcelInstalled();
            chBOpenFormattedResultsInExcel.Visibility = chBOpenFormattedResultsInExcel.IsEnabled ? Visibility.Visible : Visibility.Collapsed;

            this.DecreaseInit();

            var task = ShowExistingImportJobs();
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;

            chBOpenFormattedResultsInExcel.DataContext = _commonConfig;

            txtBFolder.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            _commonConfig.Save();

            BindingOperations.ClearAllBindings(cmBCurrentConnection);

            cmBCurrentConnection.Items.DetachFromSourceCollection();

            cmBCurrentConnection.DataContext = null;
            cmBCurrentConnection.ItemsSource = null;

            base.OnClosed(e);
        }

        private ConnectionData GetSelectedConnection()
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            return connectionData;
        }

        private Task<IOrganizationServiceExtented> GetService()
        {
            return GetOrganizationService(GetSelectedConnection());
        }

        private async Task ShowExistingImportJobs()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            ToggleControls(connectionData, false, Properties.OutputStrings.LoadingImportJobs);

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                this._itemsSource.Clear();

                textName = txtBFilter.Text.Trim().ToLower();
            });

            IEnumerable<ImportJob> list = Enumerable.Empty<ImportJob>();

            try
            {
                var service = await GetService();

                if (service != null)
                {
                    var repository = new ImportJobRepository(service);

                    list = await repository.GetListAsync(textName, new ColumnSet(
                        ImportJob.Schema.Attributes.importjobid
                        , ImportJob.Schema.Attributes.name
                        , ImportJob.Schema.Attributes.solutionname
                        , ImportJob.Schema.Attributes.startedon
                        , ImportJob.Schema.Attributes.progress
                        , ImportJob.Schema.Attributes.completedon
                        , ImportJob.Schema.Attributes.createdon
                        , ImportJob.Schema.Attributes.createdby
                        , ImportJob.Schema.Attributes.importcontext
                        , ImportJob.Schema.Attributes.operationcontext
                    ));

                    SwitchEntityDatesToLocalTime(list);
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }

            list = FilterList(list, textName);

            LoadImportJobs(list);

            ToggleControls(connectionData, true, Properties.OutputStrings.LoadingImportJobsCompletedFormat1, list.Count());
        }

        private static IEnumerable<ImportJob> FilterList(IEnumerable<ImportJob> list, string textName)
        {
            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                if (Guid.TryParse(textName, out Guid tempGuid))
                {
                    list = list.Where(ent => ent.Id == tempGuid);
                }
            }

            return list;
        }

        private class EntityViewItem
        {
            public string SolutionName => ImportJob.SolutionName;

            public DateTime? StartedOn => ImportJob.StartedOn;

            public double Progress => ImportJob.Progress.GetValueOrDefault();

            public DateTime? CompletedOn => ImportJob.CompletedOn;

            public DateTime? CreatedOn => ImportJob.CreatedOn;

            public string CreatedBy => ImportJob.CreatedBy?.Name;

            public string ImportContext => ImportJob.ImportContext;

            public string OperationContext => ImportJob.OperationContext;

            public ImportJob ImportJob { get; private set; }

            public EntityViewItem(ImportJob ImportJob)
            {
                this.ImportJob = ImportJob;
            }
        }

        private void LoadImportJobs(IEnumerable<ImportJob> results)
        {
            this.lstVwImportJobs.Dispatcher.Invoke(() =>
            {
                foreach (var entity in results)
                {
                    var item = new EntityViewItem(entity);

                    this._itemsSource.Add(item);
                }

                if (this.lstVwImportJobs.Items.Count == 1)
                {
                    this.lstVwImportJobs.SelectedItem = this.lstVwImportJobs.Items[0];
                }
            });
        }

        private void UpdateStatus(ConnectionData connectionData, string format, params object[] args)
        {
            string message = format;

            if (args != null && args.Length > 0)
            {
                message = string.Format(format, args);
            }

            _iWriteToOutput.WriteToOutput(connectionData, message);

            this.stBIStatus.Dispatcher.Invoke(() =>
            {
                this.stBIStatus.Content = message;
            });
        }

        protected override void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(connectionData, statusFormat, args);

            ToggleControl(this.tSProgressBar, cmBCurrentConnection, btnSetCurrentConnection);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwImportJobs.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwImportJobs.SelectedItems.Count > 0;

                    UIElement[] list = { btnExportAll };

                    foreach (var button in list)
                    {
                        button.IsEnabled = enabled;
                    }
                }
                catch (Exception)
                {
                }
            });
        }

        private async void txtBFilterEnitity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await ShowExistingImportJobs();
            }
        }

        private ImportJob GetSelectedEntity()
        {
            return this.lstVwImportJobs.SelectedItems.OfType<EntityViewItem>().Count() == 1
                ? this.lstVwImportJobs.SelectedItems.OfType<EntityViewItem>().Select(e => e.ImportJob).SingleOrDefault() : null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void lstVwImportJobs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

                if (item != null)
                {
                    await ExecuteAction(item.ImportJob.Id, item.ImportJob.SolutionName, item.ImportJob.CreatedOn, PerformExportMouseDoubleClick);
                }
            }
        }

        private async Task PerformExportMouseDoubleClick(string folder, Guid idImportJob, string solutionName, DateTime? createdOn)
        {
            await PerformExportFormattedResults(folder, idImportJob, solutionName, createdOn);
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private async Task ExecuteAction(Guid idImportJob, string solutionName, DateTime? createdOn, Func<string, Guid, string, DateTime?, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            await action(folder, idImportJob, solutionName, createdOn);
        }

        private Task<string> CreateFileAsync(ConnectionData connectionData, string folder, string solutionName, DateTime? createdOn, string fieldTitle, string xmlContent)
        {
            return Task.Run(() => CreateFile(connectionData, folder, solutionName, createdOn, fieldTitle, xmlContent));
        }

        private string CreateFile(ConnectionData connectionData, string folder, string solutionName, DateTime? createdOn, string fieldTitle, string xmlContent)
        {
            string fileName = EntityFileNameFormatter.GetImportJobFileName(connectionData.Name, solutionName, createdOn, fieldTitle, FileExtension.xml);
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            string createdOnString = createdOn?.ToString(EntityFileNameFormatter.dateFormatYearMonthDayHourMinuteSecond);

            string name = string.Format("{0} at {1}", solutionName, createdOnString);

            if (!string.IsNullOrEmpty(xmlContent))
            {
                try
                {
                    xmlContent = ContentComparerHelper.FormatXmlByConfiguration(xmlContent, _commonConfig, XmlOptionsControls.ImportJobXmlOptions);

                    File.WriteAllText(filePath, xmlContent, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, connectionData.Name, ImportJob.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, connectionData.Name, ImportJob.Schema.EntityLogicalName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
            }

            return filePath;
        }

        private async void btnExportAll_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteAction(entity.Id, entity.SolutionName, entity.CreatedOn, PerformExportAllXml);
        }

        private async Task PerformExportAllXml(string folder, Guid idImportJob, string solutionName, DateTime? createdOn)
        {
            await PerformExportEntityDescription(folder, idImportJob, solutionName, createdOn);

            await PerformExportXmlToFile(folder, idImportJob, solutionName, createdOn, ImportJob.Schema.Attributes.data, ImportJob.Schema.Headers.data);

            await PerformExportFormattedResults(folder, idImportJob, solutionName, createdOn);
        }

        private async Task ExecuteActionEntity(Guid idImportJob, string solutionName, DateTime? createdOn, string fieldName, string fieldTitle, Func<string, Guid, string, DateTime?, string, string, Task> action)
        {
            string folder = txtBFolder.Text.Trim();

            if (!this.IsControlsEnabled)
            {
                return;
            }

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            await action(folder, idImportJob, solutionName, createdOn, fieldName, fieldTitle);
        }

        private async Task PerformExportXmlToFile(string folder, Guid idImportJob, string solutionName, DateTime? createdOn, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ExportingXmlFieldToFileFormat1, fieldTitle);

            try
            {
                var repository = new ImportJobRepository(service);

                var importJob = await repository.GetByIdAsync(idImportJob, new ColumnSet(fieldName));

                string xmlContent = importJob.GetAttributeValue<string>(fieldName);

                string filePath = await CreateFileAsync(service.ConnectionData, folder, solutionName, createdOn, fieldTitle, xmlContent);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingXmlFieldToFileCompletedFormat1, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingXmlFieldToFileFailedFormat1, fieldName);
            }
        }

        private async void mIExportImportJobData_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteActionEntity(entity.Id, entity.SolutionName, entity.CreatedOn, ImportJob.Schema.Attributes.data, ImportJob.Schema.Headers.data, PerformExportXmlToFile);
        }

        private async void mIExportImportJobFormattedResults_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteAction(entity.Id, entity.SolutionName, entity.CreatedOn, PerformExportFormattedResults);
        }

        private async Task PerformExportFormattedResults(string folder, Guid idImportJob, string solutionName, DateTime? createdOn)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            string createdOnString = createdOn?.ToString(EntityFileNameFormatter.dateFormatYearMonthDayHourMinuteSecond);

            string name = string.Format("{0} at {1}", solutionName, createdOnString);

            string fieldTitle = "FormattedResults";

            string fileName = EntityFileNameFormatter.GetImportJobFileName(service.ConnectionData.Name, solutionName, createdOn, fieldTitle, FileExtension.xml);
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ExportingXmlFieldToFileFormat1, fieldTitle);

            try
            {
                var repository = new ImportJobRepository(service);

                var formattedResults = await repository.GetFormattedResultsAsync(idImportJob);

                if (!string.IsNullOrEmpty(formattedResults))
                {
                    try
                    {
                        File.WriteAllText(filePath, formattedResults, new UTF8Encoding(false));

                        this._iWriteToOutput.WriteToOutputFilePathUri(service.ConnectionData, filePath);

                        if (IsExcelInstalled())
                        {
                            this._iWriteToOutput.WriteToOutputFilePathUriToOpenInExcel(service.ConnectionData, filePath);

                            if (chBOpenFormattedResultsInExcel.IsChecked.GetValueOrDefault())
                            {
                                this._iWriteToOutput.OpenFileInExcel(service.ConnectionData, filePath);
                            }
                            else if (_commonConfig.DefaultFileAction != FileAction.None)
                            {
                                this._iWriteToOutput.SelectFileInFolder(service.ConnectionData, filePath);
                            }
                        }
                        else if (_commonConfig.DefaultFileAction != FileAction.None)
                        {
                            this._iWriteToOutput.SelectFileInFolder(service.ConnectionData, filePath);
                        }

                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, service.ConnectionData.Name, ImportJob.Schema.EntityLogicalName, name, fieldTitle, filePath);
                    }
                    catch (Exception ex)
                    {
                        this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    }
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, service.ConnectionData.Name, ImportJob.Schema.EntityLogicalName, name, fieldTitle);
                    this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingXmlFieldToFileCompletedFormat1, fieldTitle);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingXmlFieldToFileFailedFormat1, fieldTitle);
            }
        }

        private async void mICreateEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            await ExecuteAction(entity.Id, entity.SolutionName, entity.CreatedOn, PerformExportEntityDescription);
        }

        private async Task PerformExportEntityDescription(string folder, Guid idImportJob, string solutionName, DateTime? createdOn)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingEntityDescription);

            try
            {
                string fileName = EntityFileNameFormatter.GetImportJobFileName(service.ConnectionData.Name, solutionName, createdOn, EntityFileNameFormatter.Headers.EntityDescription, FileExtension.txt);
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                var repository = new ImportJobRepository(service);

                var importJob = await repository.GetByIdAsync(idImportJob, ColumnSetInstances.AllColumns);

                await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, importJob, service.ConnectionData);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.InConnectionExportedEntityDescriptionFormat3
                    , service.ConnectionData.Name
                    , importJob.LogicalName
                    , filePath);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingEntityDescriptionCompleted);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingEntityDescriptionFailed);
            }
        }

        protected override async Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            await ShowExistingImportJobs();
        }

        protected override bool CanCloseWindow(KeyEventArgs e)
        {
            Popup[] _popupArray = new Popup[] { _optionsPopup };

            foreach (var popup in _popupArray)
            {
                if (popup.IsOpen)
                {
                    popup.IsOpen = false;
                    e.Handled = true;

                    return false;
                }
            }

            return true;
        }

        private async void mIOpenInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            if (service != null)
            {
                service.ConnectionData.OpenEntityInstanceInWeb(entity.LogicalName, entity.Id);
            }
        }

        private async void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource?.Clear();
            });

            if (!this.IsControlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                await ShowExistingImportJobs();
            }
        }

        private void miOptions_Click(object sender, RoutedEventArgs e)
        {
            this._optionsPopup.IsOpen = true;
            this._optionsPopup.Child.Focus();
        }

        private void Child_CloseClicked(object sender, EventArgs e)
        {
            if (_optionsPopup.IsOpen)
            {
                _optionsPopup.IsOpen = false;
                this.Focus();
            }
        }

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, GetSelectedConnection());
        }

        private async void hyperlinkData_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.ImportJob;

            await ExecuteActionEntity(entity.Id, entity.SolutionName, entity.CreatedOn, ImportJob.Schema.Attributes.data, ImportJob.Schema.Headers.data, PerformExportXmlToFile);
        }

        private async void hyperlinkFormattedResults_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.ImportJob;

            await ExecuteAction(entity.Id, entity.SolutionName, entity.CreatedOn, PerformExportFormattedResults);
        }

        #region Clipboard

        private void mIClipboardCopySolutionName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityViewItem>(e, ent => ent.SolutionName);
        }

        private void mIClipboardCopyImportContext_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityViewItem>(e, ent => ent.ImportContext);
        }

        private void mIClipboardCopyOperationContext_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityViewItem>(e, ent => ent.OperationContext);
        }

        private void mIClipboardCopyImportJobId_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityViewItem>(e, ent => ent.ImportJob.Id.ToString());
        }

        #endregion Clipboard
    }
}