using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
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
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExplorerReport : WindowWithConnectionList
    {
        private readonly ObservableCollection<EntityViewItem> _itemsSource;

        public WindowExplorerReport(
             IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
            , string selection
        ) : base(iWriteToOutput, commonConfig, service)
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            InitializeComponent();

            LoadFromConfig();

            if (!string.IsNullOrEmpty(selection))
            {
                txtBFilter.Text = selection;
            }

            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            this._itemsSource = new ObservableCollection<EntityViewItem>();

            this.lstVwReports.ItemsSource = _itemsSource;

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            FillExplorersMenuItems();

            this.DecreaseInit();

            if (service != null)
            {
                ShowExistingReports();
            }
        }

        private void FillExplorersMenuItems()
        {
            var explorersHelper = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService
                , getReportName: GetReportName
            );

            var compareWindowsHelper = new CompareWindowsHelper(_iWriteToOutput, _commonConfig, () => Tuple.Create(GetSelectedConnection(), GetSelectedConnection())
                , getReportName: GetReportName
            );

            explorersHelper.FillExplorers(miExplorers);
            compareWindowsHelper.FillCompareWindows(miCompareOrganizations);

            if (this.Resources.Contains("listContextMenu")
                && this.Resources["listContextMenu"] is ContextMenu listContextMenu
            )
            {
                explorersHelper.FillExplorers(listContextMenu, nameof(miExplorers));

                compareWindowsHelper.FillCompareWindows(listContextMenu, nameof(miCompareOrganizations));
            }
        }

        private string GetReportName()
        {
            var entity = GetSelectedEntity();

            return entity?.Name ?? txtBFilter.Text.Trim();
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;

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

        private async Task ShowExistingReports()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.LoadingReports);

            this._itemsSource.Clear();

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            IEnumerable<Report> list = Enumerable.Empty<Report>();

            try
            {
                if (service != null)
                {
                    var repository = new ReportRepository(service);
                    list = await repository.GetListAsync(textName,
                        new ColumnSet
                        (
                            Report.Schema.Attributes.name
                            , Report.Schema.Attributes.ispersonal
                            , Report.Schema.Attributes.ownerid
                            , Report.Schema.Attributes.reporttypecode
                            , Report.Schema.Attributes.filename
                            , Report.Schema.Attributes.iscustomizable
                            , Report.Schema.Attributes.description
                        )
                    );
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            LoadReports(list);

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.LoadingReportsCompletedFormat1, list.Count());
        }

        private class EntityViewItem
        {
            public string Name => Report.Name;

            public string FileName => Report.FileName;

            public string ReportTypeCode { get; }

            public string Owner => Report.OwnerId?.Name;

            public string IsPersonal { get; }

            public string Description => Report.Description;

            public bool HasDescription => !string.IsNullOrEmpty(Report.Description);

            public Report Report { get; }

            public EntityViewItem(Report report)
            {
                report.FormattedValues.TryGetValue(Report.Schema.Attributes.reporttypecode, out var reporttypecode);
                report.FormattedValues.TryGetValue(Report.Schema.Attributes.ispersonal, out var ispersonal);

                this.ReportTypeCode = reporttypecode;
                this.IsPersonal = ispersonal;

                this.Report = report;
            }
        }

        private void LoadReports(IEnumerable<Report> results)
        {
            this.lstVwReports.Dispatcher.Invoke(() =>
            {
                foreach (var report in results)
                {
                    var item = new EntityViewItem(report);

                    this._itemsSource.Add(item);
                }

                if (this.lstVwReports.Items.Count == 1)
                {
                    this.lstVwReports.SelectedItem = this.lstVwReports.Items[0];
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
            this.lstVwReports.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwReports.SelectedItems.Count > 0;

                    UIElement[] list = { tSDDBExportReport, btnExportAll };

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

        private void txtBFilterEnitity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowExistingReports();
            }
        }

        private Report GetSelectedEntity()
        {
            return this.lstVwReports.SelectedItems.OfType<EntityViewItem>().Count() == 1
                ? this.lstVwReports.SelectedItems.OfType<EntityViewItem>().Select(e => e.Report).SingleOrDefault() : null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

                if (item != null)
                {
                    var service = await GetService();

                    if (service != null)
                    {
                        service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.Report, item.Report.Id);
                    }
                }
            }
        }

        private async Task PerformExportMouseDoubleClick(string folder, Guid idReport, string name, string filename)
        {
            await PerformExportEntityDescription(folder, idReport, name, filename);
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private async Task ExecuteAction(Guid idReport, string name, string filename, Func<string, Guid, string, string, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            await action(folder, idReport, name, filename);
        }

        private Task<string> CreateFileAsync(string folder, string name, Guid id, string fieldTitle, string xmlContent)
        {
            return Task.Run(() => CreateFile(folder, name, id, fieldTitle, xmlContent));
        }

        private string CreateFile(string folder, string name, Guid id, string fieldTitle, string xmlContent)
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData == null)
            {
                return null;
            }

            string fileName = EntityFileNameFormatter.GetReportFileName(connectionData.Name, name, id, fieldTitle, "xml");
            string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(xmlContent))
            {
                try
                {
                    if (ContentComparerHelper.TryParseXml(xmlContent, out var doc))
                    {
                        xmlContent = doc.ToString();
                    }

                    File.WriteAllText(filePath, xmlContent, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, Report.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, Report.Schema.EntityLogicalName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
            }

            return filePath;
        }

        private void btnExportAll_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity.Id, entity.Name, entity.FileName, PerformExportAll);
        }

        private async Task PerformExportAll(string folder, Guid idReport, string name, string filename)
        {
            await PerformExportEntityDescription(folder, idReport, name, filename);

            await PerformExportXmlToFile(folder, idReport, name, filename, Report.Schema.Attributes.bodytext, Report.Schema.Headers.bodytext);

            await PerformExportXmlToFile(folder, idReport, name, filename, Report.Schema.Attributes.originalbodytext, Report.Schema.Headers.originalbodytext);

            await PerformExportXmlToFile(folder, idReport, name, filename, Report.Schema.Attributes.defaultfilter, Report.Schema.Headers.defaultfilter);

            await PerformExportXmlToFile(folder, idReport, name, filename, Report.Schema.Attributes.customreportxml, Report.Schema.Headers.customreportxml);

            await PerformExportXmlToFile(folder, idReport, name, filename, Report.Schema.Attributes.schedulexml, Report.Schema.Headers.schedulexml);

            await PerformExportXmlToFile(folder, idReport, name, filename, Report.Schema.Attributes.queryinfo, Report.Schema.Headers.queryinfo);
        }

        private async Task ExecuteActionEntity(Guid idReport, string name, string filename, string fieldName, string fieldTitle, Func<string, Guid, string, string, string, string, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            string folder = txtBFolder.Text.Trim();

            folder = CorrectFolderIfEmptyOrNotExists(_iWriteToOutput, folder);

            await action(folder, idReport, name, filename, fieldName, fieldTitle);
        }

        private async Task PerformExportXmlToFile(string folder, Guid idReport, string name, string filename, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ExportingXmlFieldToFileFormat1, fieldTitle);

            try
            {
                var repository = new ReportRepository(service);

                var report = await repository.GetByIdAsync(idReport, new ColumnSet(fieldName));

                string xmlContent = report.GetAttributeValue<string>(fieldName);

                string filePath = await CreateFileAsync(folder, name, idReport, fieldTitle, xmlContent);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingXmlFieldToFileCompletedFormat1, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingXmlFieldToFileFailedFormat1, fieldName);
            }
        }

        private async Task PerformUpdateEntityField(string folder, Guid idReport, string name, string filename, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.UpdatingFieldFormat2, service.ConnectionData.Name, fieldName);

            try
            {
                var repository = new ReportRepository(service);

                var report = await repository.GetByIdAsync(idReport, new ColumnSet(fieldName));

                string xmlContent = report.GetAttributeValue<string>(fieldName);

                {
                    if (ContentComparerHelper.TryParseXml(xmlContent, out var doc))
                    {
                        xmlContent = doc.ToString();
                    }
                }

                string filePath = await CreateFileAsync(folder, name, idReport, fieldTitle + " BackUp", xmlContent);

                var newText = string.Empty;
                bool? dialogResult = false;

                this.Dispatcher.Invoke(() =>
                {
                    var form = new WindowTextField("Enter " + fieldTitle, fieldTitle, xmlContent);

                    dialogResult = form.ShowDialog();

                    newText = form.FieldText;
                });

                if (dialogResult.GetValueOrDefault() == false)
                {
                    ToggleControls(service.ConnectionData, true, Properties.OutputStrings.UpdatingFieldCanceledFormat2, service.ConnectionData.Name, fieldName);
                    return;
                }

                newText = ContentComparerHelper.RemoveInTextAllCustomXmlAttributesAndNamespaces(newText);

                {
                    if (ContentComparerHelper.TryParseXml(newText, out var doc))
                    {
                        newText = doc.ToString(SaveOptions.DisableFormatting);
                    }
                }

                var updateEntity = new Report
                {
                    Id = idReport
                };
                updateEntity.Attributes[fieldName] = newText;

                await service.UpdateAsync(updateEntity);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.UpdatingFieldCompletedFormat2, service.ConnectionData.Name, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.UpdatingFieldFailedFormat2, service.ConnectionData.Name, fieldName);
            }
        }

        private void mICreateEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity.Id, entity.Name, entity.FileName, PerformExportEntityDescription);
        }

        private void mIChangeEntityInEditor_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity.Id, entity.Name, entity.FileName, PerformEntityEditor);
        }

        private void mIDeleteReport_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteAction(entity.Id, entity.Name, entity.FileName, PerformDeleteEntity);
        }

        private async Task PerformExportEntityDescription(string folder, Guid idReport, string name, string filename)
        {
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingEntityDescription);

            try
            {
                string fileName = EntityFileNameFormatter.GetReportFileName(service.ConnectionData.Name, name, idReport, EntityFileNameFormatter.Headers.EntityDescription, "txt");
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                var repository = new ReportRepository(service);

                var report = await repository.GetByIdAsync(idReport);

                await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, report, EntityFileNameFormatter.ReportIgnoreFields, service.ConnectionData);

                this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                    , service.ConnectionData.Name
                    , report.LogicalName
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

        private async Task PerformEntityEditor(string folder, Guid idReport, string name, string filename)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenEntityEditor(_iWriteToOutput, service, _commonConfig, Report.EntityLogicalName, idReport);
        }

        private async Task PerformDeleteEntity(string folder, Guid idReport, string name, string filename)
        {
            string message = string.Format(Properties.MessageBoxStrings.AreYouSureDeleteSdkObjectFormat2, Report.EntityLogicalName, name);

            if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                var service = await GetService();

                ToggleControls(service.ConnectionData, false, Properties.OutputStrings.DeletingEntityFormat2, service.ConnectionData.Name, Report.EntityLogicalName);

                try
                {
                    _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.DeletingEntity);
                    _iWriteToOutput.WriteToOutputEntityInstance(service.ConnectionData, Report.EntityLogicalName, idReport);

                    await service.DeleteAsync(Report.EntityLogicalName, idReport);
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    _iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.DeletingEntityCompletedFormat2, service.ConnectionData.Name, Report.EntityLogicalName);

                ShowExistingReports();
            }
        }

        private void mIExportReportBodyText_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.Name, entity.FileName, Report.Schema.Attributes.bodytext, Report.Schema.Headers.bodytext, PerformExportXmlToFile);
        }

        private void mIExportReportOriginalBodyText_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.Name, entity.FileName, Report.Schema.Attributes.originalbodytext, Report.Schema.Headers.originalbodytext, PerformExportXmlToFile);
        }

        private void mIExportReportBodyBinary_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.Name, entity.FileName, Report.Schema.Attributes.bodybinary, Report.Schema.Headers.bodybinary, PerformExportBodyBinary);
        }

        private void mIExportReportDefaultFilter_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.Name, entity.FileName, Report.Schema.Attributes.defaultfilter, Report.Schema.Headers.defaultfilter, PerformExportXmlToFile);
        }

        private void mIExportReportCustomReportXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.Name, entity.FileName, Report.Schema.Attributes.customreportxml, Report.Schema.Headers.customreportxml, PerformExportXmlToFile);
        }

        private void mIExportReportScheduleXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.Name, entity.FileName, Report.Schema.Attributes.schedulexml, Report.Schema.Headers.schedulexml, PerformExportXmlToFile);
        }

        private void mIExportReportQueryInfo_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.Name, entity.FileName, Report.Schema.Attributes.queryinfo, Report.Schema.Headers.queryinfo, PerformExportXmlToFile);
        }

        private async Task PerformExportBodyBinary(string folder, Guid idReport, string name, string filename, string fieldName, string fieldTitle)
        {
            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ExportingBodyBinaryForFieldFormat1, fieldName);

            try
            {
                var repository = new ReportRepository(service);
                Report reportWithBodyBinary = await repository.GetByIdAsync(idReport, new ColumnSet(fieldName));

                string extension = Path.GetExtension(filename);

                string fileName = EntityFileNameFormatter.GetReportFileName(service.ConnectionData.Name, name, idReport, fieldTitle, extension);
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                var body = reportWithBodyBinary.GetAttributeValue<string>(fieldName);

                if (!string.IsNullOrEmpty(body))
                {
                    var array = Convert.FromBase64String(body);

                    File.WriteAllBytes(filePath, array);

                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, service.ConnectionData.Name, Report.Schema.EntityLogicalName, name, fieldTitle, filePath);

                    if (File.Exists(filePath))
                    {
                        if (_commonConfig.DefaultFileAction != FileAction.None)
                        {
                            this._iWriteToOutput.SelectFileInFolder(service.ConnectionData, filePath);
                        }
                    }
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, Report.Schema.EntityLogicalName, name, fieldTitle);
                    this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);
                }

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingBodyBinaryForFieldCompletedFormat1, fieldName);

            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ExportingBodyBinaryForFieldFailedFormat1, fieldName);
            }
        }

        protected override void OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            ShowExistingReports();
        }

        private void mIOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.Report, entity.Id);
            }
        }

        private async void mIOpenInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService();

            if (service != null)
            {
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.Report, entity.Id);
            }
        }

        private void mIOpenEntityListInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenEntityInstanceListInWeb(Report.EntityLogicalName);
            }
        }

        private async void AddToCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddToSolution(true, null);
        }

        private async void AddToCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddToSolution(false, solutionUniqueName);
            }
        }

        private async Task AddToSolution(bool withSelect, string solutionUniqueName)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.Report, new[] { entity.Id }, null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        private void mICreateNewReport_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenReportCreateInWeb();
            }
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (!(sender is ContextMenu contextMenu))
            {
                return;
            }

            var items = contextMenu.Items.OfType<Control>();

            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            FillLastSolutionItems(connectionData, items, true, AddToCrmSolutionLast_Click, "contMnAddToSolutionLast");

            EntityViewItem nodeItem = GetItemFromRoutedDataContext<EntityViewItem>(e);

            ActivateControls(items, (nodeItem.Report.IsCustomizable?.Value).GetValueOrDefault(true), "controlChangeEntityAttribute");
        }

        private void tSDDBExportReport_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            var report = GetSelectedEntity();

            ActivateControls(tSDDBExportReport.Items.OfType<Control>(), (report?.IsCustomizable?.Value).GetValueOrDefault(true), "controlChangeEntityAttribute");
        }

        private async void mIOpenDependentComponentsInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSolutionComponentDependenciesExplorer(
                _iWriteToOutput
                , service
                , null
                , _commonConfig
                , (int)ComponentType.Report
                , entity.Id
                , null
                );
        }

        private async void mIOpenSolutionsContainingComponentInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenExplorerSolutionExplorer(
                _iWriteToOutput
                , service
                , _commonConfig
                , (int)ComponentType.Report
                , entity.Id
                , null
            );
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource?.Clear();
            });

            if (!this.IsControlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                ShowExistingReports();
            }
        }

        private void mIUpdateReportBodyText_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.Name, entity.FileName, Report.Schema.Attributes.bodytext, Report.Schema.Headers.bodytext, PerformUpdateEntityField);
        }

        private void mIUpdateReportDefaultFilter_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.Name, entity.FileName, Report.Schema.Attributes.defaultfilter, Report.Schema.Headers.defaultfilter, PerformUpdateEntityField);
        }

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, cmBCurrentConnection.SelectedItem as ConnectionData);
        }

        private void hyperlinkBodyText_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.Report;

            ExecuteActionEntity(entity.Id, entity.Name, entity.FileName, Report.Schema.Attributes.bodytext, Report.Schema.Headers.bodytext, PerformExportXmlToFile);
        }

        private void hyperlinkOriginalBodyText_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.Report;

            ExecuteActionEntity(entity.Id, entity.Name, entity.FileName, Report.Schema.Attributes.originalbodytext, Report.Schema.Headers.originalbodytext, PerformExportXmlToFile);
        }

        private void hyperlinkDefaultFilter_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.Report;

            ExecuteActionEntity(entity.Id, entity.Name, entity.FileName, Report.Schema.Attributes.defaultfilter, Report.Schema.Headers.defaultfilter, PerformExportXmlToFile);
        }

        private void hyperlinkCustomReportXml_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.Report;

            ExecuteActionEntity(entity.Id, entity.Name, entity.FileName, Report.Schema.Attributes.customreportxml, Report.Schema.Headers.customreportxml, PerformExportXmlToFile);
        }

        private void hyperlinkScheduleXml_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.Report;

            ExecuteActionEntity(entity.Id, entity.Name, entity.FileName, Report.Schema.Attributes.schedulexml, Report.Schema.Headers.schedulexml, PerformExportXmlToFile);
        }

        private void hyperlinkQueryInfo_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.Report;

            ExecuteActionEntity(entity.Id, entity.Name, entity.FileName, Report.Schema.Attributes.queryinfo, Report.Schema.Headers.queryinfo, PerformExportXmlToFile);
        }

        private void hyperlinkBodyBinary_Click(object sender, RoutedEventArgs e)
        {
            EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

            if (item == null)
            {
                return;
            }

            var entity = item.Report;

            ExecuteActionEntity(entity.Id, entity.Name, entity.FileName, Report.Schema.Attributes.bodybinary, Report.Schema.Headers.bodybinary, PerformExportBodyBinary);
        }

        private void lstVwReports_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.IsControlsEnabled;
            e.ContinueRouting = false;
        }

        private void lstVwReports_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            mIDeleteReport_Click(sender, e);
        }
    }
}