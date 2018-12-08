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
    public partial class WindowExportReport : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private IWriteToOutput _iWriteToOutput;

        private CommonConfiguration _commonConfig;
        private ConnectionConfiguration _connectionConfig;

        private bool _controlsEnabled = true;

        private ObservableCollection<EntityViewItem> _itemsSource;

        private Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();
        private Dictionary<Guid, SolutionComponentDescriptor> _descriptorCache = new Dictionary<Guid, SolutionComponentDescriptor>();

        private int _init = 0;

        public WindowExportReport(
             IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string selection
            )
        {
            _init++;

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;
            this._connectionConfig = service.ConnectionData.ConnectionConfiguration;

            _connectionCache[service.ConnectionData.ConnectionId] = service;
            _descriptorCache[service.ConnectionData.ConnectionId] = new SolutionComponentDescriptor(service, true);

            BindingOperations.EnableCollectionSynchronization(_connectionConfig.Connections, sysObjectConnections);

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

            cmBCurrentConnection.ItemsSource = _connectionConfig.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            _init--;

            if (service != null)
            {
                ShowExistingReports();
            }
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;

            txtBFolder.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            _commonConfig.Save();
            _connectionConfig.Save();

            BindingOperations.ClearAllBindings(cmBCurrentConnection);

            cmBCurrentConnection.Items.DetachFromSourceCollection();

            cmBCurrentConnection.DataContext = null;
            cmBCurrentConnection.ItemsSource = null;

            base.OnClosed(e);
        }

        private async Task<IOrganizationServiceExtented> GetService()
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData != null)
            {
                if (!_connectionCache.ContainsKey(connectionData.ConnectionId))
                {
                    _iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);
                    _iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());
                    var service = await QuickConnection.ConnectAsync(connectionData);
                    _iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                    _connectionCache[connectionData.ConnectionId] = service;
                }

                return _connectionCache[connectionData.ConnectionId];
            }

            return null;
        }

        private async Task<SolutionComponentDescriptor> GetDescriptor()
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData != null)
            {
                if (!_descriptorCache.ContainsKey(connectionData.ConnectionId))
                {
                    var service = await GetService();

                    _descriptorCache[connectionData.ConnectionId] = new SolutionComponentDescriptor(service, true);
                }

                return _descriptorCache[connectionData.ConnectionId];
            }

            return null;
        }

        private async Task ShowExistingReports()
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.LoadingReports);

            this._itemsSource.Clear();

            string textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            IEnumerable<Report> list = Enumerable.Empty<Report>();

            try
            {
                var service = await GetService();

                if (service != null)
                {
                    var repository = new ReportRepository(service);
                    list = await repository.GetListAsync(textName,
                        new ColumnSet(
                            Report.Schema.Attributes.name
                            , Report.Schema.Attributes.ispersonal
                            , Report.Schema.Attributes.ownerid
                            , Report.Schema.Attributes.reporttypecode
                            , Report.Schema.Attributes.filename
                            , Report.Schema.Attributes.iscustomizable
                        ));
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }

            LoadReports(list);
        }

        private class EntityViewItem
        {
            public string ReportName { get; private set; }

            public string FileName { get; private set; }

            public string ReportType { get; private set; }

            public string Owner { get; private set; }

            public string ViewableBy { get; private set; }

            public Report Report { get; private set; }

            public EntityViewItem(string name, string filename, string reportType, string owner, string ispersonal, Report report)
            {
                this.ReportName = name;
                this.FileName = filename;
                this.ReportType = reportType;
                this.Owner = owner;
                this.ViewableBy = ispersonal;
                this.Report = report;
            }
        }

        private void LoadReports(IEnumerable<Report> results)
        {
            this.lstVwReports.Dispatcher.Invoke(() =>
            {
                foreach (var report in results)
                {
                    string reportType = report.FormattedValues[Report.Schema.Attributes.reporttypecode];

                    string owner = string.Empty;
                    if (report.OwnerId != null)
                    {
                        owner = report.OwnerId.Name;
                    }

                    string ispersonal = report.FormattedValues[Report.Schema.Attributes.ispersonal];

                    var item = new EntityViewItem(report.Name, report.FileName, reportType, owner, ispersonal, report);

                    this._itemsSource.Add(item);
                }

                if (this.lstVwReports.Items.Count == 1)
                {
                    this.lstVwReports.SelectedItem = this.lstVwReports.Items[0];
                }
            });

            ToggleControls(true, Properties.WindowStatusStrings.LoadingReportsCompletedFormat1, results.Count());
        }

        private void UpdateStatus(string format, params object[] args)
        {
            string message = format;

            if (args != null && args.Length > 0)
            {
                message = string.Format(format, args);
            }

            _iWriteToOutput.WriteToOutput(message);

            this.stBIStatus.Dispatcher.Invoke(() =>
            {
                this.stBIStatus.Content = message;
            });
        }

        private void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this._controlsEnabled = enabled;

            UpdateStatus(statusFormat, args);

            ToggleControl(cmBCurrentConnection, enabled);

            ToggleProgressBar(enabled);

            UpdateButtonsEnable();
        }

        private void ToggleProgressBar(bool enabled)
        {
            if (tSProgressBar == null)
            {
                return;
            }

            this.tSProgressBar.Dispatcher.Invoke(() =>
            {
                tSProgressBar.IsIndeterminate = !enabled;
            });
        }

        private void ToggleControl(Control c, bool enabled)
        {
            c.Dispatcher.Invoke(() =>
            {
                if (c is TextBox)
                {
                    ((TextBox)c).IsReadOnly = !enabled;
                }
                else
                {
                    c.IsEnabled = enabled;
                }
            });
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwReports.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this._controlsEnabled && this.lstVwReports.SelectedItems.Count > 0;

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

        private void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var item = ((FrameworkElement)e.OriginalSource).DataContext as EntityViewItem;

                if (item != null)
                {
                    ExecuteAction(item.Report.Id, item.Report.Name, item.Report.FileName, PerformExportMouseDoubleClick);
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
            string folder = txtBFolder.Text.Trim();

            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            if (string.IsNullOrEmpty(folder))
            {
                return;
            }

            if (!Directory.Exists(folder))
            {
                return;
            }

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
                    if (ContentCoparerHelper.TryParseXml(xmlContent, out var doc))
                    {
                        xmlContent = doc.ToString();
                    }

                    File.WriteAllText(filePath, xmlContent, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, Report.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, Report.Schema.EntityLogicalName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow();
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

            await PerformExportXmlToFile(folder, idReport, name, filename, Report.Schema.Attributes.bodytext, "BodyText");

            await PerformExportXmlToFile(folder, idReport, name, filename, Report.Schema.Attributes.originalbodytext, "OriginalBodyText");

            await PerformExportXmlToFile(folder, idReport, name, filename, Report.Schema.Attributes.defaultfilter, "DefaultFilter");

            await PerformExportXmlToFile(folder, idReport, name, filename, Report.Schema.Attributes.customreportxml, "CustomReportXml");

            await PerformExportXmlToFile(folder, idReport, name, filename, Report.Schema.Attributes.schedulexml, "ScheduleXml");

            await PerformExportXmlToFile(folder, idReport, name, filename, Report.Schema.Attributes.queryinfo, "QueryInfo");
        }

        private async Task ExecuteActionEntity(Guid idReport, string name, string filename, string fieldName, string fieldTitle, Func<string, Guid, string, string, string, string, Task> action)
        {
            string folder = txtBFolder.Text.Trim();

            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            if (string.IsNullOrEmpty(folder))
            {
                return;
            }

            if (!Directory.Exists(folder))
            {
                return;
            }

            await action(folder, idReport, name, filename, fieldName, fieldTitle);
        }

        private async Task PerformExportXmlToFile(string folder, Guid idReport, string name, string filename, string fieldName, string fieldTitle)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.ExportingXmlFieldToFileFormat1, fieldTitle);

            try
            {
                var service = await GetService();

                var repository = new ReportRepository(service);

                var report = await repository.GetByIdAsync(idReport, new ColumnSet(fieldName));

                string xmlContent = report.GetAttributeValue<string>(fieldName);

                string filePath = await CreateFileAsync(folder, name, idReport, fieldTitle, xmlContent);

                this._iWriteToOutput.PerformAction(filePath);

                ToggleControls(true, Properties.WindowStatusStrings.ExportingXmlFieldToFileCompletedFormat1, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.ExportingXmlFieldToFileFailedFormat1, fieldName);
            }
        }

        private async Task PerformUpdateEntityField(string folder, Guid idReport, string name, string filename, string fieldName, string fieldTitle)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(false, Properties.WindowStatusStrings.UpdatingFieldFormat2, service.ConnectionData.Name, fieldName);

            try
            {
                var repository = new ReportRepository(service);

                var report = await repository.GetByIdAsync(idReport, new ColumnSet(fieldName));

                string xmlContent = report.GetAttributeValue<string>(fieldName);

                {
                    if (ContentCoparerHelper.TryParseXml(xmlContent, out var doc))
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
                    ToggleControls(true, Properties.WindowStatusStrings.UpdatingFieldCanceledFormat2, service.ConnectionData.Name, fieldName);
                    return;
                }

                newText = ContentCoparerHelper.RemoveAllCustomXmlAttributesAndNamespaces(newText);

                {
                    if (ContentCoparerHelper.TryParseXml(newText, out var doc))
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

                ToggleControls(true, Properties.WindowStatusStrings.UpdatingFieldCompletedFormat2, service.ConnectionData.Name, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.UpdatingFieldFailedFormat2, service.ConnectionData.Name, fieldName);
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

        private async Task PerformExportEntityDescription(string folder, Guid idReport, string name, string filename)
        {
            ToggleControls(false, Properties.WindowStatusStrings.CreatingEntityDescription);

            try
            {
                var service = await GetService();

                string fileName = EntityFileNameFormatter.GetReportFileName(service.ConnectionData.Name, name, idReport, "EntityDescription", "txt");
                string filePath = Path.Combine(folder, FileOperations.RemoveWrongSymbols(fileName));

                var repository = new ReportRepository(service);

                var report = await repository.GetByIdAsync(idReport);

                await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, report, EntityFileNameFormatter.ReportIgnoreFields, service.ConnectionData);

                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                    , service.ConnectionData.Name
                    , report.LogicalName
                    , filePath);

                this._iWriteToOutput.PerformAction(filePath);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingEntityDescriptionCompleted);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.CreatingEntityDescriptionFailed);
            }
        }

        private void mIExportReportBodyText_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.Name, entity.FileName, Report.Schema.Attributes.bodytext, "BodyText", PerformExportXmlToFile);
        }

        private void mIExportReportOriginalBodyText_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.Name, entity.FileName, Report.Schema.Attributes.originalbodytext, "OriginalBodyText", PerformExportXmlToFile);
        }

        private void mIExportReportBodyBinary_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.Name, entity.FileName, Report.Schema.Attributes.bodybinary, "BodyBinary", PerformExportBodyBinary);
        }

        private void mIExportReportDefaultFilter_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.Name, entity.FileName, Report.Schema.Attributes.defaultfilter, "DefaultFilter", PerformExportXmlToFile);
        }

        private void mIExportReportCustomReportXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.Name, entity.FileName, Report.Schema.Attributes.customreportxml, "CustomReportXml", PerformExportXmlToFile);
        }

        private void mIExportReportScheduleXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.Name, entity.FileName, Report.Schema.Attributes.schedulexml, "ScheduleXml", PerformExportXmlToFile);
        }

        private void mIExportReportQueryInfo_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.Name, entity.FileName, Report.Schema.Attributes.queryinfo, "QueryInfo", PerformExportXmlToFile);
        }

        private async Task PerformExportBodyBinary(string folder, Guid idReport, string name, string filename, string fieldName, string fieldTitle)
        {
            ToggleControls(false, Properties.WindowStatusStrings.ExportingBodyBinaryForFieldFormat1, fieldName);

            try
            {
                var service = await GetService();

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

                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.EntityFieldExportedToFormat5, service.ConnectionData.Name, Report.Schema.EntityLogicalName, name, fieldTitle, filePath);

                    if (File.Exists(filePath))
                    {
                        if (_commonConfig.DefaultFileAction != FileAction.None)
                        {
                            this._iWriteToOutput.SelectFileInFolder(filePath);
                        }
                    }
                }
                else
                {
                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, Report.Schema.EntityLogicalName, name, fieldTitle);
                    this._iWriteToOutput.ActivateOutputWindow();
                }

                ToggleControls(true, Properties.WindowStatusStrings.ExportingBodyBinaryForFieldCompletedFormat1, fieldName);

            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.ExportingBodyBinaryForFieldFailedFormat1, fieldName);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingReports();
            }

            base.OnKeyDown(e);
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
                connectionData.OpenEntityListInWeb(Report.EntityLogicalName);
            }
        }

        private async void AddIntoCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddIntoSolution(true, null);
        }

        private async void AddIntoCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddIntoSolution(false, solutionUniqueName);
            }
        }

        private async Task AddIntoSolution(bool withSelect, string solutionUniqueName)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService();
            var descriptor = await GetDescriptor();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow();

                await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.Report, new[] { entity.Id }, null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
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

            FillLastSolutionItems(connectionData, items, true, AddIntoCrmSolutionLast_Click, "contMnAddIntoSolutionLast");

            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as EntityViewItem;

            ActivateControls(items, (nodeItem.Report.IsCustomizable?.Value).GetValueOrDefault(true), "controlChangeEntityAttribute");
        }

        private void tSDDBExportReport_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            var nodeItem = ((FrameworkElement)e.OriginalSource).DataContext as EntityViewItem;

            ActivateControls(tSDDBExportReport.Items.OfType<Control>(), (nodeItem.Report.IsCustomizable?.Value).GetValueOrDefault(true), "controlChangeEntityAttribute");
        }

        private async void mIOpenDependentComponentsInWindow_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();
            var descriptor = await GetDescriptor();

            WindowHelper.OpenSolutionComponentDependenciesWindow(
                _iWriteToOutput
                , service
                , descriptor
                , _commonConfig
                , (int)ComponentType.Report
                , entity.Id
                , null
                );
        }

        private async void mIOpenSolutionsContainingComponentInWindow_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenExplorerSolutionWindow(
                _iWriteToOutput
                , service
                , _commonConfig
                , (int)ComponentType.Report
                , entity.Id
                , null
            );
        }

        private async void btnOrganizationComparer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerWindow(this._iWriteToOutput, service.ConnectionData.ConnectionConfiguration, _commonConfig);
        }

        private async void btnCompareReports_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerReportWindow(
                _iWriteToOutput
                , _commonConfig
                , service.ConnectionData
                , service.ConnectionData
            );
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource?.Clear();
            });

            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                ShowExistingReports();
            }
        }

        private void mIUpdateReportDefaultFilter_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.Name, entity.FileName, Report.Schema.Attributes.defaultfilter, "DefaultFilter", PerformExportXmlToFile);
        }

        private void mIUpdateReportCustomReportXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.Name, entity.FileName, Report.Schema.Attributes.customreportxml, "CustomReportXml", PerformExportXmlToFile);
        }

        private void mIUpdateReportScheduleXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionEntity(entity.Id, entity.Name, entity.FileName, Report.Schema.Attributes.schedulexml, "ScheduleXml", PerformExportXmlToFile);
        }
    }
}