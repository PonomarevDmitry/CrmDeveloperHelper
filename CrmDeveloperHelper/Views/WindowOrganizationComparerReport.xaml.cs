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
    public partial class WindowOrganizationComparerReport : WindowWithConnectionList
    {
        private readonly ObservableCollection<EntityViewItem> _itemsSource;

        public WindowOrganizationComparerReport(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filter
        ) : base(iWriteToOutput, commonConfig, connection1)
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

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

            FillExplorersMenuItems();

            this.DecreaseInit();

            var task = ShowExistingReports();
        }

        private void FillExplorersMenuItems()
        {
            //var explorersHelper1 = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService1
            //    , getReportName: GetReportName1
            //);

            //var explorersHelper2 = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService2
            //    , getReportName: GetReportName2
            //);

            //explorersHelper1.FillExplorers(miExplorers1);
            //explorersHelper2.FillExplorers(miExplorers2);

            var compareWindowsHelper = new CompareWindowsHelper(_iWriteToOutput, _commonConfig, () => Tuple.Create(GetConnection1(), GetConnection2())
                , getReportName: GetReportName1
            );
            compareWindowsHelper.FillCompareWindows(miCompareOrganizations);

            if (this.Resources.Contains("listContextMenu")
                && this.Resources["listContextMenu"] is ContextMenu contextMenu
            )
            {
                var items = contextMenu.Items.OfType<MenuItem>();

                foreach (var item in items)
                {
                    //if (string.Equals(item.Uid, "miExplorers1", StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    explorersHelper1.FillExplorers(item);
                    //}
                    //else if (string.Equals(item.Uid, "miExplorers2", StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    explorersHelper2.FillExplorers(item);
                    //}
                    //else
                    if (string.Equals(item.Uid, "miCompareOrganizations", StringComparison.InvariantCultureIgnoreCase))
                    {
                        compareWindowsHelper.FillCompareWindows(item);
                    }
                }
            }
        }

        private string GetReportName1()
        {
            var entity = GetSelectedEntity();

            return entity?.ReportName1 ?? txtBFilter.Text.Trim();
        }

        private string GetReportName2()
        {
            var entity = GetSelectedEntity();

            return entity?.ReportName2 ?? txtBFilter.Text.Trim();
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            BindingOperations.ClearAllBindings(cmBConnection1);
            cmBConnection1.Items.DetachFromSourceCollection();
            cmBConnection1.DataContext = null;
            cmBConnection1.ItemsSource = null;

            BindingOperations.ClearAllBindings(cmBConnection2);
            cmBConnection2.Items.DetachFromSourceCollection();
            cmBConnection2.DataContext = null;
            cmBConnection2.ItemsSource = null;
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

        private Task<IOrganizationServiceExtented> GetService1()
        {
            return GetOrganizationService(GetConnection1());
        }

        private Task<IOrganizationServiceExtented> GetService2()
        {
            return GetOrganizationService(GetConnection2());
        }

        private async Task ShowExistingReports()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.LoadingReports);

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
                        var name1 = ent.Entity1?.Name;
                        var name2 = ent.Entity2?.Name;

                        return name1.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                            || name2.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
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

            ToggleControls(true, Properties.OutputStrings.LoadingReportsCompletedFormat1, results.Count());
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

        protected override void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            ToggleControls(enabled, statusFormat, args);
        }

        protected void ToggleControls(bool enabled, string statusFormat, params object[] args)
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

        private async void txtBFilterEnitity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await ShowExistingReports();
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
                EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

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

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

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

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            action(linked, showAllways, fieldName, fieldTitle);
        }

        private async Task PerformShowingDifferenceSingleXmlAsync(LinkedEntities<Report> linked, bool showAllways, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.ShowingDifferenceXmlForFieldFormat1, fieldName);

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

                    if (showAllways || !ContentComparerHelper.CompareXML(xml1, xml2).IsEqual)
                    {
                        string filePath1 = await CreateFileAsync(service1.ConnectionData, report1.Name, report1.Id, fieldTitle, xml1);

                        string filePath2 = await CreateFileAsync(service2.ConnectionData, report2.Name, report2.Id, fieldTitle, xml2);

                        if (!File.Exists(filePath1))
                        {
                            this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service1.ConnectionData.Name, Report.Schema.EntityLogicalName, report1.Name, fieldTitle);
                            this._iWriteToOutput.ActivateOutputWindow(null);
                        }

                        if (!File.Exists(filePath2))
                        {
                            this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service2.ConnectionData.Name, Report.Schema.EntityLogicalName, report2.Name, fieldTitle);
                            this._iWriteToOutput.ActivateOutputWindow(null);
                        }

                        if (File.Exists(filePath1) && File.Exists(filePath2))
                        {
                            await this._iWriteToOutput.ProcessStartProgramComparerAsync(filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
                        }
                        else
                        {
                            this._iWriteToOutput.PerformAction(service1.ConnectionData, filePath1);

                            this._iWriteToOutput.PerformAction(service2.ConnectionData, filePath2);
                        }
                    }
                }

                ToggleControls(true, Properties.OutputStrings.ShowingDifferenceXmlForFieldCompletedFormat1, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);

                ToggleControls(true, Properties.OutputStrings.ShowingDifferenceXmlForFieldFailedFormat1, fieldName);
            }
        }

        private void ExecuteActionOnEntity(Guid idReport, Func<Task<IOrganizationServiceExtented>> getService, string fieldName, string fieldTitle, Func<Guid, Func<Task<IOrganizationServiceExtented>>, string, string, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            action(idReport, getService, fieldName, fieldTitle);
        }

        private async Task PerformExportXmlToFile(Guid idReport, Func<Task<IOrganizationServiceExtented>> getService, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.ExportingXmlFieldToFileFormat1, fieldTitle);

            var service = await getService();

            if (service != null)
            {
                var repository = new ReportRepository(service);

                var report = await repository.GetByIdAsync(idReport, new ColumnSet(true));

                string xmlContent = report.GetAttributeValue<string>(fieldName);

                string filePath = await CreateFileAsync(service.ConnectionData, report.Name, report.Id, fieldTitle, xmlContent);

                if (!File.Exists(filePath))
                {
                    this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, Report.Schema.EntityLogicalName, report.Name, fieldTitle);
                    this._iWriteToOutput.ActivateOutputWindow(null);
                }

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }

            ToggleControls(true, Properties.OutputStrings.ExportingXmlFieldToFileCompletedFormat1, fieldName);
        }

        private async Task PerformExportBodyBinary(Guid idReport, Func<Task<IOrganizationServiceExtented>> getService, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.ExportingBodyBinaryForFieldFormat1, fieldName);

            var service = await getService();

            try
            {
                if (service != null)
                {
                    var repository = new ReportRepository(service);

                    Report reportWithBodyBinary = await repository.GetByIdAsync(idReport, new ColumnSet(true));

                    if (!string.IsNullOrEmpty(reportWithBodyBinary.BodyBinary))
                    {
                        string extension = Path.GetExtension(reportWithBodyBinary.FileName);

                        string fileName = EntityFileNameFormatter.GetReportFileName(service.ConnectionData.Name, reportWithBodyBinary.Name, reportWithBodyBinary.Id, fieldTitle, extension);
                        string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

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
                        this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, Report.Schema.EntityLogicalName, reportWithBodyBinary.Name, fieldTitle);
                        this._iWriteToOutput.ActivateOutputWindow(null);
                    }
                }

                ToggleControls(true, Properties.OutputStrings.ExportingBodyBinaryForFieldCompletedFormat1, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(true, Properties.OutputStrings.ExportingBodyBinaryForFieldFailedFormat1, fieldName);
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

            ToggleControls(false, Properties.OutputStrings.ShowingDifferenceEntityDescription);

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
                            await this._iWriteToOutput.ProcessStartProgramComparerAsync(filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
                        }
                        else
                        {
                            this._iWriteToOutput.PerformAction(service1.ConnectionData, filePath1);

                            this._iWriteToOutput.PerformAction(service2.ConnectionData, filePath2);
                        }
                    }
                }

                ToggleControls(true, Properties.OutputStrings.ShowingDifferenceEntityDescriptionCompleted);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);

                ToggleControls(true, Properties.OutputStrings.ShowingDifferenceEntityDescriptionFailed);
            }
        }

        private void ExecuteActionDescription(Guid idReport, Func<Task<IOrganizationServiceExtented>> getService, Func<Guid, Func<Task<IOrganizationServiceExtented>>, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            action(idReport, getService);
        }

        private async Task PerformExportDescriptionToFile(Guid idReport, Func<Task<IOrganizationServiceExtented>> getService)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.CreatingEntityDescription);

            var service = await getService();

            if (service != null)
            {
                var repository = new ReportRepository(service);

                var report = await repository.GetByIdAsync(idReport, new ColumnSet(true));

                var description = await EntityDescriptionHandler.GetEntityDescriptionAsync(report, EntityFileNameFormatter.ReportIgnoreFields, service.ConnectionData);

                string filePath = await CreateDescriptionFileAsync(service.ConnectionData, report.Name, report.Id, "Description", description);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }

            ToggleControls(true, Properties.OutputStrings.CreatingEntityDescriptionCompleted);
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

        protected override async Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            await ShowExistingReports();
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

                    var task = ShowExistingReports();
                }
            });
        }

        private async void btnExportReport1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenReportExplorer(this._iWriteToOutput, service, _commonConfig, entity?.ReportName1 ?? txtBFilter.Text);
        }

        private async void btnExportReport2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenReportExplorer(this._iWriteToOutput, service, _commonConfig, entity?.ReportName2 ?? txtBFilter.Text);
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (!(sender is ContextMenu contextMenu))
            {
                return;
            }

            EntityViewItem linkedEntityMetadata = GetItemFromRoutedDataContext<EntityViewItem>(e);

            var hasTwoEntities = linkedEntityMetadata != null
                && linkedEntityMetadata.Link != null
                && linkedEntityMetadata.Link.Entity1 != null
                && linkedEntityMetadata.Link.Entity2 != null;

            var hasSecondEntity = linkedEntityMetadata != null
                && linkedEntityMetadata.Link != null
                && linkedEntityMetadata.Link.Entity2 != null;

            var items = contextMenu.Items.OfType<Control>();

            ActivateControls(items, hasTwoEntities, "menuContextDifference", "miCompareOrganizations");

            ActivateControls(items, hasSecondEntity, "menuContextConnection2");
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