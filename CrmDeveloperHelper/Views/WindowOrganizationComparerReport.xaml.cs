using Microsoft.Xrm.Sdk.Query;
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

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowOrganizationComparerReport : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private readonly IWriteToOutput _iWriteToOutput;

        private readonly Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();

        private readonly CommonConfiguration _commonConfig;

        private readonly ObservableCollection<EntityViewItem> _itemsSource;

        public WindowOrganizationComparerReport(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filter
        )
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;

            BindingOperations.EnableCollectionSynchronization(connection1.ConnectionConfiguration.Connections, sysObjectConnections);

            InitializeComponent();

            this.Resources["ConnectionName1"] = connection1.Name;
            this.Resources["ConnectionName2"] = connection2.Name;

            LoadFromConfig();

            if (!string.IsNullOrEmpty(filter))
            {
                txtBFilter.Text = filter;
            }

            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            this._itemsSource = new ObservableCollection<EntityViewItem>();

            this.lstVwReports.ItemsSource = _itemsSource;

            cmBConnection1.ItemsSource = connection1.ConnectionConfiguration.Connections;
            cmBConnection1.SelectedItem = connection1;

            cmBConnection2.ItemsSource = connection1.ConnectionConfiguration.Connections;
            cmBConnection2.SelectedItem = connection2;

            this.DecreaseInit();

            ShowExistingReports();
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            _commonConfig.Save();

            BindingOperations.ClearAllBindings(cmBConnection1);
            cmBConnection1.Items.DetachFromSourceCollection();
            cmBConnection1.DataContext = null;
            cmBConnection1.ItemsSource = null;

            BindingOperations.ClearAllBindings(cmBConnection2);
            cmBConnection2.Items.DetachFromSourceCollection();
            cmBConnection2.DataContext = null;
            cmBConnection2.ItemsSource = null;

            base.OnClosed(e);
        }

        private ConnectionData GetConnection1()
        {
            ConnectionData connectionData = null;

            cmBConnection1.Dispatcher.Invoke(() =>
            {
                connectionData = cmBConnection1.SelectedItem as ConnectionData;
            });

            return connectionData;
        }

        private ConnectionData GetConnection2()
        {
            ConnectionData connectionData = null;

            cmBConnection1.Dispatcher.Invoke(() =>
            {
                connectionData = cmBConnection2.SelectedItem as ConnectionData;
            });

            return connectionData;
        }

        private async Task<IOrganizationServiceExtented> GetService1()
        {
            return await GetService(GetConnection1());
        }

        private async Task<IOrganizationServiceExtented> GetService2()
        {
            return await GetService(GetConnection2());
        }

        private async Task<IOrganizationServiceExtented> GetService(ConnectionData connectionData)
        {
            if (connectionData == null)
            {
                return null;
            }

            if (_connectionCache.ContainsKey(connectionData.ConnectionId))
            {
                return _connectionCache[connectionData.ConnectionId];
            }

            ToggleControls(false, string.Empty);

            _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);
            _iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            try
            {
                var service = await QuickConnection.ConnectAsync(connectionData);

                if (service != null)
                {
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                    _connectionCache[connectionData.ConnectionId] = service;

                    return service;
                }
                else
                {
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                }
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                ToggleControls(true, string.Empty);
            }

            return null;
        }

        private async Task ShowExistingReports()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.LoadingReports);

            this._itemsSource.Clear();

            var textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            IEnumerable<LinkedEntities<Report>> list = Enumerable.Empty<LinkedEntities<Report>>();

            try
            {
                var service1 = await GetService1();
                var service2 = await GetService2();

                if (service1 != null && service2 != null)
                {
                    var columnSet = new ColumnSet(
                        Report.Schema.Attributes.name
                        , Report.Schema.Attributes.filename
                        , Report.Schema.Attributes.signatureid
                        , Report.Schema.Attributes.signaturelcid
                        , Report.Schema.Attributes.signaturedate
                    );

                    var temp = new List<LinkedEntities<Report>>();

                    if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                    {
                        var repository1 = new ReportRepository(service1);
                        var repository2 = new ReportRepository(service2);

                        var task1 = repository1.GetListAsync(textName, columnSet);
                        var task2 = repository2.GetListAsync(textName, columnSet);

                        var list1 = await task1;
                        var list2 = await task2;

                        foreach (var report1 in list1)
                        {
                            var report2 = list2.FirstOrDefault(c => c.Id == report1.Id);

                            if (report2 == null
                                && report1.SignatureDate.HasValue
                                && report1.SignatureId.HasValue
                                && report1.SignatureLcid.HasValue
                                )
                            {
                                report2 = list2.FirstOrDefault(c => c.SignatureLcid == report1.SignatureLcid && c.SignatureId == report1.SignatureId && c.SignatureDate == report1.SignatureDate);
                            }

                            if (report2 == null)
                            {
                                continue;
                            }

                            temp.Add(new LinkedEntities<Report>(report1, report2));
                        }
                    }
                    else
                    {
                        var repository1 = new ReportRepository(service1);

                        var task1 = repository1.GetListAsync(textName, columnSet);

                        var list1 = await task1;

                        foreach (var report1 in list1)
                        {
                            temp.Add(new LinkedEntities<Report>(report1, null));
                        }
                    }

                    list = temp;
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }

            list = FilterList(list, textName);

            LoadEntities(list);
        }

        private static IEnumerable<LinkedEntities<Report>> FilterList(IEnumerable<LinkedEntities<Report>> list, string textName)
        {
            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                if (Guid.TryParse(textName, out Guid tempGuid))
                {
                    list = list.Where(ent =>
                        ent.Entity1?.Id == tempGuid
                        || ent.Entity2?.Id == tempGuid
                        || ent.Entity1?.ReportIdUnique == tempGuid
                        || ent.Entity2?.ReportIdUnique == tempGuid
                    );
                }
                else
                {
                    list = list.Where(ent =>
                    {
                        var name1 = ent.Entity1?.Name.ToLower();
                        var name2 = ent.Entity2?.Name.ToLower();

                        return name1.Contains(textName)
                            || name2.Contains(textName)
                            ;
                    });
                }
            }

            return list;
        }

        private class EntityViewItem
        {
            public string ReportName1 => Link.Entity1?.Name;

            public string ReportName2 => Link.Entity2?.Name;

            public LinkedEntities<Report> Link { get; }

            public EntityViewItem(LinkedEntities<Report> link)
            {
                this.Link = link;
            }
        }

        private void LoadEntities(IEnumerable<LinkedEntities<Report>> results)
        {
            this.lstVwReports.Dispatcher.Invoke(() =>
            {
                foreach (var link in results
                    .OrderBy(ent => ent.Entity1.Name)
                    .ThenBy(ent => ent.Entity2?.Name)
                    .ThenBy(ent => ent.Entity1.Id)
                )
                {
                    var item = new EntityViewItem(link);

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

            _iWriteToOutput.WriteToOutput(null, message);

            this.stBIStatus.Dispatcher.Invoke(() =>
            {
                this.stBIStatus.Content = message;
            });
        }

        private void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(statusFormat, args);

            ToggleControl(this.tSProgressBar, this.cmBConnection1, this.cmBConnection2);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwReports.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwReports.SelectedItems.Count > 0;

                    var item = (this.lstVwReports.SelectedItems[0] as EntityViewItem);

                    tSDDBShowDifference.IsEnabled = enabled && item.Link.Entity1 != null && item.Link.Entity2 != null;
                    tSDDBConnection1.IsEnabled = enabled && item.Link.Entity1 != null;
                    tSDDBConnection2.IsEnabled = enabled && item.Link.Entity2 != null;
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

        private EntityViewItem GetSelectedEntity()
        {
            return this.lstVwReports.SelectedItems.OfType<EntityViewItem>().Count() == 1
                ? this.lstVwReports.SelectedItems.OfType<EntityViewItem>().SingleOrDefault() : null;
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
                    ExecuteActionForLinked(item.Link, false, PerformShowingDifferenceAllAsync);
                }
            }
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private void ExecuteActionForLinked(LinkedEntities<Report> linked, bool showAllways, Func<LinkedEntities<Report>, bool, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (linked.Entity1 == null || linked.Entity2 == null || linked.Entity1 == linked.Entity2)
            {
                return;
            }

            if (string.IsNullOrEmpty(_commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                _commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(_commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, _commonConfig.FolderForExport);
                _commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }

            action(linked, showAllways);
        }

        private Task<string> CreateFileAsync(ConnectionData connectionData, string name, Guid id, string fieldTitle, string xmlContent)
        {
            return Task.Run(() => CreateFile(connectionData, name, id, fieldTitle, xmlContent));
        }

        private string CreateFile(ConnectionData connectionData, string name, Guid id, string fieldTitle, string xmlContent)
        {
            string fileName = EntityFileNameFormatter.GetReportFileName(connectionData.Name, name, id, fieldTitle, "xml");
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(xmlContent))
            {
                try
                {
                    if (ContentCoparerHelper.TryParseXml(xmlContent, out var doc))
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
                filePath = string.Empty;
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, Report.Schema.EntityLogicalName, name, fieldTitle);
            }

            return filePath;
        }

        private Task<string> CreateDescriptionFileAsync(ConnectionData connectionData, string name, Guid id, string fieldTitle, string description)
        {
            return Task.Run(() => CreateDescriptionFile(connectionData, name, id, fieldTitle, description));
        }

        private string CreateDescriptionFile(ConnectionData connectionData, string name, Guid id, string fieldTitle, string description)
        {
            string fileName = EntityFileNameFormatter.GetReportFileName(connectionData.Name, name, id, fieldTitle, "txt");
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(description))
            {
                try
                {
                    File.WriteAllText(filePath, description, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, Report.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
            else
            {
                filePath = string.Empty;
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, Report.Schema.EntityLogicalName, name, fieldTitle);
            }

            return filePath;
        }

        private void btnShowDifferenceAll_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionForLinked(link.Link, false, PerformShowingDifferenceAllAsync);
        }

        private async Task PerformShowingDifferenceAllAsync(LinkedEntities<Report> linked, bool showAllways)
        {
            await PerformShowingDifferenceDescriptionAsync(linked, showAllways);

            await PerformShowingDifferenceSingleXmlAsync(linked, showAllways, Report.Schema.Attributes.bodytext, Report.Schema.Headers.bodytext);

            await PerformShowingDifferenceSingleXmlAsync(linked, showAllways, Report.Schema.Attributes.originalbodytext, Report.Schema.Headers.originalbodytext);

            await PerformShowingDifferenceSingleXmlAsync(linked, showAllways, Report.Schema.Attributes.defaultfilter, Report.Schema.Headers.defaultfilter);

            await PerformShowingDifferenceSingleXmlAsync(linked, showAllways, Report.Schema.Attributes.customreportxml, Report.Schema.Headers.customreportxml);

            await PerformShowingDifferenceSingleXmlAsync(linked, showAllways, Report.Schema.Attributes.schedulexml, Report.Schema.Headers.schedulexml);

            await PerformShowingDifferenceSingleXmlAsync(linked, showAllways, Report.Schema.Attributes.queryinfo, Report.Schema.Headers.queryinfo);
        }

        private void mIShowDifferenceBodyText_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionLinked(link.Link, true, Report.Schema.Attributes.bodytext, Report.Schema.Headers.bodytext, PerformShowingDifferenceSingleXmlAsync);
        }

        private void mIShowDifferenceOriginalBodyText_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionLinked(link.Link, true, Report.Schema.Attributes.originalbodytext, Report.Schema.Headers.originalbodytext, PerformShowingDifferenceSingleXmlAsync);
        }

        private void mIShowDifferenceDefaultFilter_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionLinked(link.Link, true, Report.Schema.Attributes.defaultfilter, Report.Schema.Headers.defaultfilter, PerformShowingDifferenceSingleXmlAsync);
        }

        private void mIShowDifferenceCustomReportXml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionLinked(link.Link, true, Report.Schema.Attributes.customreportxml, Report.Schema.Headers.customreportxml, PerformShowingDifferenceSingleXmlAsync);
        }

        private void mIShowDifferenceScheduleXml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionLinked(link.Link, true, Report.Schema.Attributes.schedulexml, Report.Schema.Headers.schedulexml, PerformShowingDifferenceSingleXmlAsync);
        }

        private void mIShowDifferenceQueryInfo_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionLinked(link.Link, true, Report.Schema.Attributes.queryinfo, Report.Schema.Headers.queryinfo, PerformShowingDifferenceSingleXmlAsync);
        }

        private void ExecuteActionLinked(LinkedEntities<Report> linked, bool showAllways, string fieldName, string fieldTitle, Func<LinkedEntities<Report>, bool, string, string, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (linked.Entity1 == null || linked.Entity2 == null || linked.Entity1 == linked.Entity2)
            {
                return;
            }

            if (string.IsNullOrEmpty(_commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                _commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(_commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, _commonConfig.FolderForExport);
                _commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }

            action(linked, showAllways, fieldName, fieldTitle);
        }

        private async Task PerformShowingDifferenceSingleXmlAsync(LinkedEntities<Report> linked, bool showAllways, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.ShowingDifferenceXmlForFieldFormat1, fieldName);

            try
            {
                var service1 = await GetService1();
                var service2 = await GetService2();

                if (service1 != null && service2 != null)
                {
                    var repository1 = new ReportRepository(service1);
                    var repository2 = new ReportRepository(service2);

                    var report1 = await repository1.GetByIdAsync(linked.Entity1.Id, new ColumnSet(true));
                    var report2 = await repository2.GetByIdAsync(linked.Entity2.Id, new ColumnSet(true));

                    string xml1 = report1.GetAttributeValue<string>(fieldName);
                    string xml2 = report2.GetAttributeValue<string>(fieldName);

                    if (showAllways || !ContentCoparerHelper.CompareXML(xml1, xml2).IsEqual)
                    {
                        string filePath1 = await CreateFileAsync(service1.ConnectionData, report1.Name, report1.Id, fieldTitle, xml1);

                        string filePath2 = await CreateFileAsync(service2.ConnectionData, report2.Name, report2.Id, fieldTitle, xml2);

                        if (!File.Exists(filePath1))
                        {
                            this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service1.ConnectionData.Name, Report.Schema.EntityLogicalName, report1.Name, fieldTitle);
                        }

                        if (!File.Exists(filePath2))
                        {
                            this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service2.ConnectionData.Name, Report.Schema.EntityLogicalName, report2.Name, fieldTitle);
                        }

                        if (File.Exists(filePath1) && File.Exists(filePath2))
                        {
                            this._iWriteToOutput.ProcessStartProgramComparer(filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
                        }
                        else
                        {
                            this._iWriteToOutput.PerformAction(service1.ConnectionData, filePath1);

                            this._iWriteToOutput.PerformAction(service2.ConnectionData, filePath2);
                        }
                    }
                }

                ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceXmlForFieldCompletedFormat1, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);

                ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceXmlForFieldFailedFormat1, fieldName);
            }
        }

        private void ExecuteActionOnEntity(Guid idReport, Func<Task<IOrganizationServiceExtented>> getService, string fieldName, string fieldTitle, Func<Guid, Func<Task<IOrganizationServiceExtented>>, string, string, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (string.IsNullOrEmpty(_commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                _commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(_commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, _commonConfig.FolderForExport);
                _commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }

            action(idReport, getService, fieldName, fieldTitle);
        }

        private async Task PerformExportXmlToFile(Guid idReport, Func<Task<IOrganizationServiceExtented>> getService, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.ExportingXmlFieldToFileFormat1, fieldTitle);

            var service = await getService();

            if (service != null)
            {
                var repository = new ReportRepository(service);

                var report = await repository.GetByIdAsync(idReport, new ColumnSet(true));

                string xmlContent = report.GetAttributeValue<string>(fieldName);

                string filePath = await CreateFileAsync(service.ConnectionData, report.Name, report.Id, fieldTitle, xmlContent);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }

            ToggleControls(true, Properties.WindowStatusStrings.ExportingXmlFieldToFileCompletedFormat1, fieldName);
        }

        private async Task PerformExportBodyBinary(Guid idReport, Func<Task<IOrganizationServiceExtented>> getService, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.ExportingBodyBinaryForFieldFormat1, fieldName);

            var service = await getService();

            try
            {
                if (service != null)
                {
                    var repository = new ReportRepository(service);

                    Report reportWithBodyBinary = await repository.GetByIdAsync(idReport, new ColumnSet(true));

                    string extension = Path.GetExtension(reportWithBodyBinary.FileName);

                    string fileName = EntityFileNameFormatter.GetReportFileName(service.ConnectionData.Name, reportWithBodyBinary.Name, reportWithBodyBinary.Id, fieldTitle, extension);
                    string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    if (!string.IsNullOrEmpty(reportWithBodyBinary.BodyBinary))
                    {
                        var array = Convert.FromBase64String(reportWithBodyBinary.BodyBinary);

                        File.WriteAllBytes(filePath, array);

                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, service.ConnectionData.Name, Report.Schema.EntityLogicalName, reportWithBodyBinary.Name, fieldTitle, filePath);

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
                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, Report.Schema.EntityLogicalName, reportWithBodyBinary.Name, fieldTitle);
                    }
                }

                ToggleControls(true, Properties.WindowStatusStrings.ExportingBodyBinaryForFieldCompletedFormat1, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(true, Properties.WindowStatusStrings.ExportingBodyBinaryForFieldFailedFormat1, fieldName);
            }
        }

        private void mIShowDifferenceEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionForLinked(link.Link, true, PerformShowingDifferenceDescriptionAsync);
        }

        private async Task PerformShowingDifferenceDescriptionAsync(LinkedEntities<Report> linked, bool showAllways)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.ShowingDifferenceEntityDescription);

            try
            {
                var service1 = await GetService1();
                var service2 = await GetService2();

                if (service1 != null && service2 != null)
                {
                    var repository1 = new ReportRepository(service1);
                    var repository2 = new ReportRepository(service2);

                    var report1 = await repository1.GetByIdAsync(linked.Entity1.Id, new ColumnSet(true));
                    var report2 = await repository2.GetByIdAsync(linked.Entity2.Id, new ColumnSet(true));

                    var desc1 = await EntityDescriptionHandler.GetEntityDescriptionAsync(report1, EntityFileNameFormatter.ReportIgnoreFields);
                    var desc2 = await EntityDescriptionHandler.GetEntityDescriptionAsync(report2, EntityFileNameFormatter.ReportIgnoreFields);

                    if (showAllways || desc1 != desc2)
                    {
                        string filePath1 = await CreateDescriptionFileAsync(service1.ConnectionData, report1.Name, report1.Id, "Description", desc1);
                        string filePath2 = await CreateDescriptionFileAsync(service2.ConnectionData, report2.Name, report2.Id, "Description", desc2);

                        if (File.Exists(filePath1) && File.Exists(filePath2))
                        {
                            this._iWriteToOutput.ProcessStartProgramComparer(filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
                        }
                        else
                        {
                            this._iWriteToOutput.PerformAction(service1.ConnectionData, filePath1);

                            this._iWriteToOutput.PerformAction(service2.ConnectionData, filePath2);
                        }
                    }
                }

                ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceEntityDescriptionCompleted);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);

                ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceEntityDescriptionFailed);
            }
        }

        private void ExecuteActionDescription(Guid idReport, Func<Task<IOrganizationServiceExtented>> getService, Func<Guid, Func<Task<IOrganizationServiceExtented>>, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (string.IsNullOrEmpty(_commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportIsEmpty);
                _commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }
            else if (!Directory.Exists(_commonConfig.FolderForExport))
            {
                _iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.FolderForExportDoesNotExistsFormat1, _commonConfig.FolderForExport);
                _commonConfig.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
            }

            action(idReport, getService);
        }

        private async Task PerformExportDescriptionToFile(Guid idReport, Func<Task<IOrganizationServiceExtented>> getService)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.CreatingEntityDescription);

            var service = await getService();

            if (service != null)
            {
                var repository = new ReportRepository(service);

                var report = await repository.GetByIdAsync(idReport, new ColumnSet(true));

                var description = await EntityDescriptionHandler.GetEntityDescriptionAsync(report, EntityFileNameFormatter.ReportIgnoreFields, service.ConnectionData);

                string filePath = await CreateDescriptionFileAsync(service.ConnectionData, report.Name, report.Id, "Description", description);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }

            ToggleControls(true, Properties.WindowStatusStrings.CreatingEntityDescriptionCompleted);
        }

        private void mIExportReport1EntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionDescription(link.Link.Entity1.Id, GetService1, PerformExportDescriptionToFile);
        }

        private void mIExportReport2EntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionDescription(link.Link.Entity2.Id, GetService2, PerformExportDescriptionToFile);
        }

        private void mIExportReport1BodyText_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionOnEntity(link.Link.Entity1.Id, GetService1, Report.Schema.Attributes.bodytext, Report.Schema.Headers.bodytext, PerformExportXmlToFile);
        }

        private void mIExportReport2BodyText_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionOnEntity(link.Link.Entity2.Id, GetService2, Report.Schema.Attributes.bodytext, Report.Schema.Headers.bodytext, PerformExportXmlToFile);
        }

        private void mIExportReport1OriginalBodyText_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionOnEntity(link.Link.Entity1.Id, GetService1, Report.Schema.Attributes.originalbodytext, Report.Schema.Headers.originalbodytext, PerformExportXmlToFile);
        }

        private void mIExportReport2OriginalBodyText_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionOnEntity(link.Link.Entity2.Id, GetService2, Report.Schema.Attributes.originalbodytext, Report.Schema.Headers.originalbodytext, PerformExportXmlToFile);
        }

        private void mIExportReport1DefaultFilter_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionOnEntity(link.Link.Entity1.Id, GetService1, Report.Schema.Attributes.defaultfilter, Report.Schema.Headers.defaultfilter, PerformExportXmlToFile);
        }

        private void mIExportReport2DefaultFilter_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionOnEntity(link.Link.Entity2.Id, GetService2, Report.Schema.Attributes.defaultfilter, Report.Schema.Headers.defaultfilter, PerformExportXmlToFile);
        }

        private void mIExportReport1CustomReportXml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionOnEntity(link.Link.Entity1.Id, GetService1, Report.Schema.Attributes.customreportxml, Report.Schema.Headers.customreportxml, PerformExportXmlToFile);
        }

        private void mIExportReport2CustomReportXml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionOnEntity(link.Link.Entity2.Id, GetService2, Report.Schema.Attributes.customreportxml, Report.Schema.Headers.customreportxml, PerformExportXmlToFile);
        }

        private void mIExportReport1ScheduleXml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionOnEntity(link.Link.Entity1.Id, GetService1, Report.Schema.Attributes.schedulexml, Report.Schema.Headers.schedulexml, PerformExportXmlToFile);
        }

        private void mIExportReport2ScheduleXml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionOnEntity(link.Link.Entity2.Id, GetService2, Report.Schema.Attributes.schedulexml, Report.Schema.Headers.schedulexml, PerformExportXmlToFile);
        }

        private void mIExportReport1QueryInfo_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionOnEntity(link.Link.Entity1.Id, GetService1, Report.Schema.Attributes.queryinfo, Report.Schema.Headers.queryinfo, PerformExportXmlToFile);
        }

        private void mIExportReport2QueryInfo_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionOnEntity(link.Link.Entity2.Id, GetService2, Report.Schema.Attributes.queryinfo, Report.Schema.Headers.queryinfo, PerformExportXmlToFile);
        }

        private void mIExportReport1BodyBinary_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionOnEntity(link.Link.Entity1.Id, GetService1, Report.Schema.Attributes.bodybinary, Report.Schema.Headers.bodybinary, PerformExportBodyBinary);
        }

        private void mIExportReport2BodyBinary_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionOnEntity(link.Link.Entity2.Id, GetService2, Report.Schema.Attributes.bodybinary, Report.Schema.Headers.bodybinary, PerformExportBodyBinary);
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

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource?.Clear();

                ConnectionData connection1 = cmBConnection1.SelectedItem as ConnectionData;
                ConnectionData connection2 = cmBConnection2.SelectedItem as ConnectionData;

                if (connection1 != null && connection2 != null)
                {
                    this.Resources["ConnectionName1"] = connection1.Name;
                    this.Resources["ConnectionName2"] = connection2.Name;

                    UpdateButtonsEnable();

                    ShowExistingReports();
                }
            });
        }

        private async void btnOrganizationComparer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenOrganizationComparerWindow(this._iWriteToOutput, service.ConnectionData.ConnectionConfiguration, _commonConfig);
        }

        private async void btnExportReport1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenReportExplorerWindow(this._iWriteToOutput, service, _commonConfig, entity?.ReportName1 ?? txtBFilter.Text);
        }

        private async void btnExportReport2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenReportExplorerWindow(this._iWriteToOutput, service, _commonConfig, entity?.ReportName2 ?? txtBFilter.Text);
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (!(sender is ContextMenu contextMenu))
            {
                return;
            }

            var linkedEntityMetadata = ((FrameworkElement)e.OriginalSource).DataContext as EntityViewItem;

            var items = contextMenu.Items.OfType<Control>();

            foreach (var menuContextDifference in items.Where(i => string.Equals(i.Uid, "menuContextDifference", StringComparison.InvariantCultureIgnoreCase)))
            {
                menuContextDifference.IsEnabled = false;
                menuContextDifference.Visibility = Visibility.Collapsed;

                if (linkedEntityMetadata != null
                     && linkedEntityMetadata.Link != null
                     && linkedEntityMetadata.Link.Entity1 != null
                     && linkedEntityMetadata.Link.Entity2 != null
                )
                {
                    menuContextDifference.IsEnabled = true;
                    menuContextDifference.Visibility = Visibility.Visible;
                }
            }

            foreach (var menuContextConnection2 in items.Where(i => string.Equals(i.Uid, "menuContextConnection2", StringComparison.InvariantCultureIgnoreCase)))
            {
                menuContextConnection2.IsEnabled = false;
                menuContextConnection2.Visibility = Visibility.Collapsed;

                if (linkedEntityMetadata != null
                    && linkedEntityMetadata.Link != null
                    && linkedEntityMetadata.Link.Entity2 != null
                )
                {
                    menuContextConnection2.IsEnabled = true;
                    menuContextConnection2.Visibility = Visibility.Visible;
                }
            }
        }

        private async void mIConnection1OpenSolutionComponentInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService1();

            if (service != null)
            {
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.Report, entity.Link.Entity1.Id);
            }
        }

        private async void mIConnection2OpenSolutionComponentInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService2();

            if (service != null)
            {
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.Report, entity.Link.Entity2.Id);
            }
        }

        private async void mIConnection1OpenReportListInWeb_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService1();

            service.ConnectionData.OpenEntityInstanceListInWeb(Report.EntityLogicalName);
        }

        private async void mIConnection2OpenReportListInWeb_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService2();

            service.ConnectionData.OpenEntityInstanceListInWeb(Report.EntityLogicalName);
        }
    }
}