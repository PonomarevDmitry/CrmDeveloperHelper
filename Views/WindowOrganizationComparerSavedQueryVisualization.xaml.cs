using Microsoft.Xrm.Sdk;
using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using System.Windows.Data;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using System.Windows.Controls.Primitives;
using Nav.Common.VSPackages.CrmDeveloperHelper.UserControls;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowOrganizationComparerSavedQueryVisualization : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private IWriteToOutput _iWriteToOutput;

        private string _filterEntity;

        private Dictionary<Guid, IOrganizationServiceExtented> _cacheService = new Dictionary<Guid, IOrganizationServiceExtented>();

        private CommonConfiguration _commonConfig;
        private ConnectionConfiguration _connectionConfig;

        private ObservableCollection<EntityViewItem> _itemsSource;

        private Popup _optionsPopup;

        private bool _controlsEnabled = true;

        private int _init = 0;

        public WindowOrganizationComparerSavedQueryVisualization(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filterEntity
        )
        {
            _init++;

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;
            this._connectionConfig = connection1.ConnectionConfiguration;
            this._filterEntity = filterEntity;

            BindingOperations.EnableCollectionSynchronization(_connectionConfig.Connections, sysObjectConnections);

            InitializeComponent();

            var child = new ExportXmlOptionsControl(_commonConfig, XmlOptionsControls.SetXmlSchemas);
            child.CloseClicked += Child_CloseClicked;
            this._optionsPopup = new Popup
            {
                Child = child,

                PlacementTarget = toolBarHeader,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
                Focusable = true,
            };

            tSDDBConnection1.Header = string.Format(Properties.OperationNames.ExportFromConnectionFormat1, connection1.Name);
            tSDDBConnection2.Header = string.Format(Properties.OperationNames.ExportFromConnectionFormat1, connection2.Name);

            this.Resources["ConnectionName1"] = string.Format(Properties.OperationNames.CreateFromConnectionFormat1, connection1.Name);
            this.Resources["ConnectionName2"] = string.Format(Properties.OperationNames.CreateFromConnectionFormat1, connection2.Name);

            LoadFromConfig();

            txtBFilter.SelectionLength = 0;
            txtBFilter.SelectionStart = txtBFilter.Text.Length;

            txtBFilter.Focus();

            if (string.IsNullOrEmpty(_filterEntity))
            {
                btnClearEntityFilter.IsEnabled = sepClearEntityFilter.IsEnabled = false;
                btnClearEntityFilter.Visibility = sepClearEntityFilter.Visibility = Visibility.Collapsed;
            }

            this._itemsSource = new ObservableCollection<EntityViewItem>();

            this.lstVwCharts.ItemsSource = _itemsSource;

            cmBConnection1.ItemsSource = _connectionConfig.Connections;
            cmBConnection1.SelectedItem = connection1;

            cmBConnection2.ItemsSource = _connectionConfig.Connections;
            cmBConnection2.SelectedItem = connection2;

            _init--;

            ShowExistingCharts();
        }

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            _commonConfig.Save();
            _connectionConfig.Save();

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

        private async Task<IOrganizationServiceExtented> GetService1()
        {
            ConnectionData connectionData = null;

            cmBConnection1.Dispatcher.Invoke(() =>
            {
                connectionData = cmBConnection1.SelectedItem as ConnectionData;
            });

            if (connectionData != null)
            {
                if (!_cacheService.ContainsKey(connectionData.ConnectionId))
                {
                    _iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);
                    _iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());
                    var service = await QuickConnection.ConnectAsync(connectionData);
                    _iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                    _cacheService[connectionData.ConnectionId] = service;
                }

                return _cacheService[connectionData.ConnectionId];
            }

            return null;
        }

        private async Task<IOrganizationServiceExtented> GetService2()
        {
            ConnectionData connectionData = null;

            cmBConnection2.Dispatcher.Invoke(() =>
            {
                connectionData = cmBConnection2.SelectedItem as ConnectionData;
            });

            if (connectionData != null)
            {
                if (!_cacheService.ContainsKey(connectionData.ConnectionId))
                {
                    _iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);
                    _iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());
                    var service = await QuickConnection.ConnectAsync(connectionData);
                    _iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                    _cacheService[connectionData.ConnectionId] = service;
                }

                return _cacheService[connectionData.ConnectionId];
            }

            return null;
        }

        private async Task ShowExistingCharts()
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }
            
            ToggleControls(false, Properties.WindowStatusStrings.LoadingCharts);

            this._itemsSource.Clear();

            IEnumerable<LinkedEntities<SavedQueryVisualization>> list = Enumerable.Empty<LinkedEntities<SavedQueryVisualization>>();

            try
            {
                var service1 = await GetService1();
                var service2 = await GetService2();

                if (service1 != null && service2 != null)
                {
                    var temp = new List<LinkedEntities<SavedQueryVisualization>>();

                    if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                    {
                        var repository1 = new SavedQueryVisualizationRepository(service1);
                        var repository2 = new SavedQueryVisualizationRepository(service2);

                        var task1 = repository1.GetListAsync(this._filterEntity);
                        var task2 = repository2.GetListAsync(this._filterEntity);

                        var list1 = await task1;
                        var list2 = await task2;

                        foreach (var chart1 in list1)
                        {
                            var chart2 = list2.FirstOrDefault(c => c.Id == chart1.Id);

                            if (chart2 == null)
                            {
                                continue;
                            }

                            temp.Add(new LinkedEntities<SavedQueryVisualization>(chart1, chart2));
                        }
                    }
                    else
                    {
                        var repository1 = new SavedQueryVisualizationRepository(service1);

                        var task1 = repository1.GetListAsync(this._filterEntity);

                        var list1 = await task1;

                        foreach (var chart1 in list1)
                        {
                            temp.Add(new LinkedEntities<SavedQueryVisualization>(chart1, null));
                        }
                    }

                    list = temp;
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }

            var textName = string.Empty;

            txtBFilter.Dispatcher.Invoke(() =>
            {
                textName = txtBFilter.Text.Trim().ToLower();
            });

            list = FilterList(list, textName);

            LoadEntities(list);
        }

        private static IEnumerable<LinkedEntities<SavedQueryVisualization>> FilterList(IEnumerable<LinkedEntities<SavedQueryVisualization>> list, string textName)
        {
            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                if (Guid.TryParse(textName, out Guid tempGuid))
                {
                    list = list.Where(ent =>
                        ent.Entity1?.Id == tempGuid
                        || ent.Entity2?.Id == tempGuid
                        || ent.Entity1?.SavedQueryVisualizationIdUnique == tempGuid
                        || ent.Entity2?.SavedQueryVisualizationIdUnique == tempGuid
                    );
                }
                else
                {
                    list = list.Where(ent =>
                    {
                        var type1 = ent.Entity1?.PrimaryEntityTypeCode.ToLower() ?? string.Empty;
                        var name1 = ent.Entity1?.Name.ToLower() ?? string.Empty;

                        var type2 = ent.Entity2?.PrimaryEntityTypeCode.ToLower() ?? string.Empty;
                        var name2 = ent.Entity2?.Name.ToLower() ?? string.Empty;

                        return type1.Contains(textName)
                            || name1.Contains(textName)
                            || type2.Contains(textName)
                            || name2.Contains(textName)
                            ;
                    });
                }
            }

            return list;
        }

        private class EntityViewItem
        {
            public string EntityName { get; private set; }

            public string ChartName1 { get; private set; }

            public string ChartName2 { get; private set; }

            public LinkedEntities<SavedQueryVisualization> Link { get; private set; }

            public EntityViewItem(string entityName, string chartName1, string chartName2, LinkedEntities<SavedQueryVisualization> link)
            {
                this.EntityName = entityName;
                this.ChartName1 = chartName1;
                this.ChartName2 = chartName2;
                this.Link = link;
            }
        }

        private void LoadEntities(IEnumerable<LinkedEntities<SavedQueryVisualization>> results)
        {
            this.lstVwCharts.Dispatcher.Invoke(() =>
            {
                foreach (var link in results
                      .OrderBy(ent => ent.Entity1.PrimaryEntityTypeCode)
                      .ThenBy(ent => ent.Entity1.Name)
                      .ThenBy(ent => ent.Entity2?.Name)
                  )
                {
                    var item = new EntityViewItem(link.Entity1.PrimaryEntityTypeCode, link.Entity1.Name, link.Entity2?.Name, link);

                    this._itemsSource.Add(item);
                }

                if (this.lstVwCharts.Items.Count == 1)
                {
                    this.lstVwCharts.SelectedItem = this.lstVwCharts.Items[0];
                }
            });
            
            ToggleControls(true, Properties.WindowStatusStrings.LoadingChartsCompletedFormat1, results.Count());
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

            ToggleControl(this.tSDDBShowDifference, enabled);
            ToggleControl(this.tSDDBConnection1, enabled);
            ToggleControl(this.tSDDBConnection2, enabled);
            ToggleControl(this.cmBConnection1, enabled);
            ToggleControl(this.cmBConnection2, enabled);

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
            this.lstVwCharts.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.lstVwCharts.SelectedItems.Count > 0;

                    var item = (this.lstVwCharts.SelectedItems[0] as EntityViewItem);

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
                ShowExistingCharts();
            }
        }

        private EntityViewItem GetSelectedEntity()
        {
            return this.lstVwCharts.SelectedItems.OfType<EntityViewItem>().Count() == 1
                ? this.lstVwCharts.SelectedItems.OfType<EntityViewItem>().SingleOrDefault() : null;
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
                    ExecuteAction(item.Link, false, PerformShowingDifferenceAllAsync);
                }
            }
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private void ExecuteAction(LinkedEntities<SavedQueryVisualization> linked, bool showAllways, Func<LinkedEntities<SavedQueryVisualization>, bool, Task> action)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            if (linked.Entity1 == null || linked.Entity2 == null || linked.Entity1 == linked.Entity2)
            {
                return;
            }

            if (string.IsNullOrEmpty(_commonConfig.FolderForExport))
            {
                return;
            }

            if (!Directory.Exists(_commonConfig.FolderForExport))
            {
                return;
            }

            action(linked, showAllways);
        }

        private Task<string> CreateFileAsync(string connectionName, string entityName, string name, string fieldTitle, string xmlContent)
        {
            return Task.Run(() => CreateFile(connectionName, entityName, name, fieldTitle, xmlContent));
        }

        private string CreateFile(string connectionName, string entityName, string name, string fieldTitle, string xmlContent)
        {
            string fileName = EntityFileNameFormatter.GetSavedQueryVisualizationFileName(connectionName, entityName, name, fieldTitle, "xml");
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(xmlContent))
            {
                try
                {
                    if (_commonConfig.SetXmlSchemasDuringExport)
                    {
                        var schemasResources = CommonExportXsdSchemasCommand.GetXsdSchemas(CommonExportXsdSchemasCommand.SchemaVisualizationDataDescription);

                        if (schemasResources != null)
                        {
                            xmlContent = ContentCoparerHelper.SetXsdSchema(xmlContent, schemasResources);
                        }
                    }

                    if (ContentCoparerHelper.TryParseXml(xmlContent, out var doc))
                    {
                        xmlContent = doc.ToString();
                    }

                    File.WriteAllText(filePath, xmlContent, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.EntityFieldExportedToFormat5, connectionName, SavedQueryVisualization.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionName, SavedQueryVisualization.Schema.EntityLogicalName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow();
            }

            return filePath;
        }

        private Task<string> CreateDescriptionFileAsync(string connectionName, string entityName, string name, string fieldTitle, string description)
        {
            return Task.Run(() => CreateDescriptionFile(connectionName, entityName, name, fieldTitle, description));
        }

        private string CreateDescriptionFile(string connectionName, string entityName, string name, string fieldTitle, string description)
        {
            string fileName = EntityFileNameFormatter.GetSavedQueryVisualizationFileName(connectionName, entityName, name, fieldTitle, "txt");
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(description))
            {
                try
                {
                    File.WriteAllText(filePath, description, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.EntityFieldExportedToFormat5, connectionName, SavedQueryVisualization.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            }
            else
            {
                filePath = string.Empty;
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionName, SavedQueryVisualization.Schema.EntityLogicalName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow();
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

            ExecuteAction(link.Link, false, PerformShowingDifferenceAllAsync);
        }

        private async Task PerformShowingDifferenceAllAsync(LinkedEntities<SavedQueryVisualization> linked, bool showAllways)
        {
            await PerformShowingDifferenceDescriptionAsync(linked, showAllways);

            await PerformShowingDifferenceSingleXmlAsync(linked, showAllways, SavedQueryVisualization.Schema.Attributes.datadescription, "DataDescription");

            await PerformShowingDifferenceSingleXmlAsync(linked, showAllways, SavedQueryVisualization.Schema.Attributes.presentationdescription, "PresentationDescription");
        }

        private void mIShowDifferenceDataDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionLinked(link.Link, true, SavedQueryVisualization.Schema.Attributes.datadescription, "DataDescription", PerformShowingDifferenceSingleXmlAsync);
        }

        private void mIShowDifferencePresentationDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionLinked(link.Link, true, SavedQueryVisualization.Schema.Attributes.presentationdescription, "PresentationDescription", PerformShowingDifferenceSingleXmlAsync);
        }

        private void ExecuteActionLinked(
            LinkedEntities<SavedQueryVisualization> linked
            , bool showAllways
            , string fieldName
            , string fieldTitle
            , Func<LinkedEntities<SavedQueryVisualization>, bool, string, string, Task> action
        )
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            if (string.IsNullOrEmpty(_commonConfig.FolderForExport))
            {
                return;
            }

            if (!Directory.Exists(_commonConfig.FolderForExport))
            {
                return;
            }

            action(linked, showAllways, fieldName, fieldTitle);
        }

        private async Task PerformShowingDifferenceSingleXmlAsync(LinkedEntities<SavedQueryVisualization> linked, bool showAllways, string fieldName, string fieldTitle)
        {
            if (_init > 0 || !_controlsEnabled)
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
                    var repository1 = new SavedQueryVisualizationRepository(service1);
                    var repository2 = new SavedQueryVisualizationRepository(service2);

                    var chart1 = await repository1.GetByIdAsync(linked.Entity1.Id, new ColumnSet(true));
                    var chart2 = await repository2.GetByIdAsync(linked.Entity2.Id, new ColumnSet(true));

                    string xml1 = chart1.GetAttributeValue<string>(fieldName);
                    string xml2 = chart2.GetAttributeValue<string>(fieldName);

                    if (showAllways || !ContentCoparerHelper.CompareXML(xml1, xml2).IsEqual)
                    {
                        string filePath1 = await CreateFileAsync(service1.ConnectionData.Name, chart1.PrimaryEntityTypeCode, chart1.Name, fieldTitle, xml1);

                        string filePath2 = await CreateFileAsync(service2.ConnectionData.Name, chart2.PrimaryEntityTypeCode, chart2.Name, fieldTitle, xml2);

                        if (File.Exists(filePath1) && File.Exists(filePath2))
                        {
                            this._iWriteToOutput.ProcessStartProgramComparer(filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
                        }
                        else
                        {
                            this._iWriteToOutput.PerformAction(filePath1);

                            this._iWriteToOutput.PerformAction(filePath2);
                        }
                    }
                }

                ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceXmlForFieldCompletedFormat1, fieldName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceXmlForFieldFailedFormat1, fieldName);
            }
        }

        private void mIExportSystemChart1DataDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity1.Id, GetService1, SavedQueryVisualization.Schema.Attributes.datadescription, "DataDescription", PerformExportXmlToFile);
        }

        private void mIExportSystemChart2DataDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity2.Id, GetService2, SavedQueryVisualization.Schema.Attributes.datadescription, "DataDescription", PerformExportXmlToFile);
        }

        private void mIExportSystemChart1PresentationDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity1.Id, GetService1, SavedQueryVisualization.Schema.Attributes.presentationdescription, "PresentationDescription", PerformExportXmlToFile);
        }

        private void mIExportSystemChart2PresentationDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity2.Id, GetService2, SavedQueryVisualization.Schema.Attributes.presentationdescription, "PresentationDescription", PerformExportXmlToFile);
        }

        private void ExecuteActionEntity(Guid idChart, Func<Task<IOrganizationServiceExtented>> getService, string fieldName, string fieldTitle, Func<Guid, Func<Task<IOrganizationServiceExtented>>, string, string, Task> action)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            if (string.IsNullOrEmpty(_commonConfig.FolderForExport))
            {
                return;
            }

            if (!Directory.Exists(_commonConfig.FolderForExport))
            {
                return;
            }

            action(idChart, getService, fieldName, fieldTitle);
        }

        private async Task PerformExportXmlToFile(Guid idChart, Func<Task<IOrganizationServiceExtented>> getService, string fieldName, string fieldTitle)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.ExportingXmlFieldToFileFormat1, fieldTitle);

            var service = await getService();

            if (service != null)
            {
                var repository = new SavedQueryVisualizationRepository(service);

                var chart = await repository.GetByIdAsync(idChart, new ColumnSet(true));

                string xmlContent = chart.GetAttributeValue<string>(fieldName);

                string filePath = await CreateFileAsync(service.ConnectionData.Name, chart.PrimaryEntityTypeCode, chart.Name, fieldTitle, xmlContent);

                this._iWriteToOutput.PerformAction(filePath);
            }

            ToggleControls(true, Properties.WindowStatusStrings.ExportingXmlFieldToFileCompletedFormat1, fieldName);
        }

        private void mIShowDifferenceEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteAction(link.Link, true, PerformShowingDifferenceDescriptionAsync);
        }

        private async Task PerformShowingDifferenceDescriptionAsync(LinkedEntities<SavedQueryVisualization> linked, bool showAllways)
        {
            if (_init > 0 || !_controlsEnabled)
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
                    var repository1 = new SavedQueryVisualizationRepository(service1);
                    var repository2 = new SavedQueryVisualizationRepository(service2);

                    var chart1 = await repository1.GetByIdAsync(linked.Entity1.Id, new ColumnSet(true));
                    var chart2 = await repository2.GetByIdAsync(linked.Entity2.Id, new ColumnSet(true));

                    var desc1 = await EntityDescriptionHandler.GetEntityDescriptionAsync(chart1, EntityFileNameFormatter.SavedQueryVisualizationIgnoreFields);
                    var desc2 = await EntityDescriptionHandler.GetEntityDescriptionAsync(chart2, EntityFileNameFormatter.SavedQueryVisualizationIgnoreFields);

                    if (showAllways || desc1 != desc2)
                    {
                        string filePath1 = await CreateDescriptionFileAsync(service1.ConnectionData.Name, chart1.PrimaryEntityTypeCode, chart1.Name, "EntityDescription", desc1);
                        string filePath2 = await CreateDescriptionFileAsync(service2.ConnectionData.Name, chart2.PrimaryEntityTypeCode, chart2.Name, "EntityDescription", desc2);

                        if (File.Exists(filePath1) && File.Exists(filePath2))
                        {
                            this._iWriteToOutput.ProcessStartProgramComparer(filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2));
                        }
                        else
                        {
                            this._iWriteToOutput.PerformAction(filePath1);

                            this._iWriteToOutput.PerformAction(filePath2);
                        }
                    }
                }

                ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceEntityDescriptionCompleted);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceEntityDescriptionFailed);
            }
        }

        private void ExecuteActionDescription(Guid idChart, Func<Task<IOrganizationServiceExtented>> getService, Func<Guid, Func<Task<IOrganizationServiceExtented>>, Task> action)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            if (string.IsNullOrEmpty(_commonConfig.FolderForExport))
            {
                return;
            }

            if (!Directory.Exists(_commonConfig.FolderForExport))
            {
                return;
            }

            action(idChart, getService);
        }

        private async Task PerformExportDescriptionToFile(Guid idChart, Func<Task<IOrganizationServiceExtented>> getService)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.CreatingEntityDescription);

            var service = await getService();

            if (service != null)
            {
                var repository = new SavedQueryVisualizationRepository(service);

                var chart = await repository.GetByIdAsync(idChart, new ColumnSet(true));

                var description = await EntityDescriptionHandler.GetEntityDescriptionAsync(chart, EntityFileNameFormatter.SavedQueryVisualizationIgnoreFields, service.ConnectionData);

                string filePath = await CreateDescriptionFileAsync(service.ConnectionData.Name, chart.PrimaryEntityTypeCode, chart.Name, "EntityDescription", description);

                this._iWriteToOutput.PerformAction(filePath);
            }

            ToggleControls(true, Properties.WindowStatusStrings.CreatingEntityDescriptionCompleted);
        }

        private void mIExportSystemChart1EntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionDescription(link.Link.Entity1.Id, GetService1, PerformExportDescriptionToFile);
        }

        private void mIExportSystemChart2EntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionDescription(link.Link.Entity2.Id, GetService2, PerformExportDescriptionToFile);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingCharts();
            }

            if (!e.Handled)
            {
                if (e.Key == Key.Escape
                    || (e.Key == Key.W && e.KeyboardDevice != null && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0)
                    )
                {
                    if (_optionsPopup.IsOpen)
                    {
                        _optionsPopup.IsOpen = false;
                        e.Handled = true;
                    }
                }
            }

            base.OnKeyDown(e);
        }

        private void btnClearEntityFilter_Click(object sender, RoutedEventArgs e)
        {
            this._filterEntity = null;

            btnClearEntityFilter.IsEnabled = sepClearEntityFilter.IsEnabled = false;
            btnClearEntityFilter.Visibility = sepClearEntityFilter.Visibility = Visibility.Collapsed;

            ShowExistingCharts();
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_init > 0 || !_controlsEnabled)
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
                    tSDDBConnection1.Header = string.Format(Properties.OperationNames.ExportFromConnectionFormat1, connection1.Name);
                    tSDDBConnection2.Header = string.Format(Properties.OperationNames.ExportFromConnectionFormat1, connection2.Name);

                    this.Resources["ConnectionName1"] = string.Format(Properties.OperationNames.CreateFromConnectionFormat1, connection1.Name);
                    this.Resources["ConnectionName2"] = string.Format(Properties.OperationNames.CreateFromConnectionFormat1, connection2.Name);

                    UpdateButtonsEnable();

                    ShowExistingCharts();
                }
            });
        }
        private async void btnOrganizationComparer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenOrganizationComparerWindow(this._iWriteToOutput, service.ConnectionData.ConnectionConfiguration, _commonConfig);
        }

        private async void btnCompareMetadataFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerEntityMetadataWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData, entity?.EntityName);
        }

        private async void btnCompareApplicationRibbons_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerApplicationRibbonWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData);
        }

        private async void btnCompareGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerGlobalOptionSetsWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData, entity?.EntityName);
        }

        private async void btnCompareSystemForms_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerSystemFormWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData, entity?.EntityName);
        }

        private async void btnCompareSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerSavedQueryWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData, entity?.EntityName);
        }

        private async void btnCompareWorkflows_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerWorkflowWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData, entity?.EntityName);
        }

        private async void btnEntityAttributeExplorer1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenEntityAttributeExplorer(this._iWriteToOutput, service, _commonConfig, entity?.EntityName);
        }

        private async void btnEntityAttributeExplorer2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenEntityAttributeExplorer(this._iWriteToOutput, service, _commonConfig, entity?.EntityName);
        }

        private async void btnEntityRelationshipOneToManyExplorer1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenEntityRelationshipOneToManyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.EntityName);
        }

        private async void btnEntityRelationshipOneToManyExplorer2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenEntityRelationshipOneToManyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.EntityName);
        }

        private async void btnEntityRelationshipManyToManyExplorer1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenEntityRelationshipManyToManyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.EntityName);
        }

        private async void btnEntityRelationshipManyToManyExplorer2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenEntityRelationshipManyToManyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.EntityName);
        }

        private async void btnEntityKeyExplorer1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenEntityKeyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.EntityName);
        }

        private async void btnEntityKeyExplorer2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenEntityKeyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.EntityName);
        }

        private async void btnEntitySecurityRolesExplorer1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenEntitySecurityRolesExplorer(this._iWriteToOutput, service, _commonConfig, null, entity?.EntityName);
        }

        private async void btnEntitySecurityRolesExplorer2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenEntitySecurityRolesExplorer(this._iWriteToOutput, service, _commonConfig, null, entity?.EntityName);
        }

        private async void btnCreateMetadataFile1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, null, null);
        }

        private async void btnExportApplicationRibbon1_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenApplicationRibbonWindow(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnGlobalOptionSets1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            var service = await GetService1();

            _commonConfig.Save();

            WindowHelper.OpenGlobalOptionSetsWindow(
                this._iWriteToOutput
                , service
                , _commonConfig
                , null
                , string.Empty
                , string.Empty
                );
        }

        private async void btnSystemForms1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenSystemFormWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, string.Empty);
        }

        private async void btnSavedQuery1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenSavedQueryWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, string.Empty);
        }

        private async void btnSavedChart1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenSavedQueryVisualizationWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, string.Empty);
        }

        private async void btnWorkflows1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenWorkflowWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, string.Empty);
        }

        private async void btnPluginTree1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenPluginTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, string.Empty, string.Empty);
        }

        private async void btnMessageTree1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenSdkMessageTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, string.Empty);
        }

        private async void btnMessageRequestTree1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenSdkMessageRequestTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, string.Empty);
        }

        private async void btnCreateMetadataFile2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, null, null);
        }

        private async void btnExportApplicationRibbon2_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenApplicationRibbonWindow(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnGlobalOptionSets2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            var service = await GetService2();

            _commonConfig.Save();

            WindowHelper.OpenGlobalOptionSetsWindow(
                this._iWriteToOutput
                , service
                , _commonConfig
                , null
                , string.Empty
                , string.Empty
                );
        }

        private async void btnSystemForms2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenSystemFormWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, string.Empty);
        }

        private async void btnSavedQuery2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenSavedQueryWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, string.Empty);
        }

        private async void btnSavedChart2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenSavedQueryVisualizationWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, string.Empty);
        }

        private async void btnWorkflows2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenWorkflowWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, string.Empty);
        }

        private async void btnPluginTree2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenPluginTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, string.Empty, string.Empty);
        }

        private async void btnMessageTree2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenSdkMessageTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, string.Empty);
        }

        private async void btnMessageRequestTree2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenSdkMessageRequestTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, string.Empty);
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
    }
}
