using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
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
    public partial class WindowOrganizationComparerSavedQueryVisualization : WindowWithConnectionList
    {
        private readonly ObservableCollection<EntityViewItem> _itemsSource;

        private readonly Popup _optionsPopup;

        public WindowOrganizationComparerSavedQueryVisualization(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filterEntity
            , string filter
        ) : base(iWriteToOutput, commonConfig, connection1)
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            InitializeComponent();

            LoadEntityNames(cmBEntityName, connection1, connection2);

            var child = new ExportXmlOptionsControl(_commonConfig, XmlOptionsControls.SavedQueryVisualizationXmlOptions);
            child.CloseClicked += Child_CloseClicked;
            this._optionsPopup = new Popup
            {
                Child = child,

                PlacementTarget = toolBarHeader,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
                Focusable = true,
            };

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

            cmBEntityName.Text = filterEntity;

            this._itemsSource = new ObservableCollection<EntityViewItem>();

            this.lstVwCharts.ItemsSource = _itemsSource;

            cmBConnection1.ItemsSource = connection1.ConnectionConfiguration.Connections;
            cmBConnection1.SelectedItem = connection1;

            cmBConnection2.ItemsSource = connection1.ConnectionConfiguration.Connections;
            cmBConnection2.SelectedItem = connection2;

            FillExplorersMenuItems();

            this.DecreaseInit();

            var task = ShowExistingCharts();
        }

        private void FillExplorersMenuItems()
        {
            var explorersHelper1 = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService1
                , getEntityName: GetEntityName
                , getSavedQueryVisualizationName: GetChartName1
            );

            var explorersHelper2 = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService2
                , getEntityName: GetEntityName
                , getSavedQueryVisualizationName: GetChartName2
            );

            var compareWindowsHelper = new CompareWindowsHelper(_iWriteToOutput, _commonConfig, () => Tuple.Create(GetConnection1(), GetConnection2())
                , getEntityName: GetEntityName
                , getSavedQueryVisualizationName: GetChartName1
            );

            explorersHelper1.FillExplorers(miExplorers1);
            explorersHelper2.FillExplorers(miExplorers2);

            compareWindowsHelper.FillCompareWindows(miCompareOrganizations);

            if (this.Resources.Contains("listContextMenu")
                && this.Resources["listContextMenu"] is ContextMenu contextMenu
            )
            {
                var items = contextMenu.Items.OfType<MenuItem>();

                foreach (var item in items)
                {
                    if (string.Equals(item.Uid, "miExplorers1", StringComparison.InvariantCultureIgnoreCase))
                    {
                        explorersHelper1.FillExplorers(item);
                    }
                    else if (string.Equals(item.Uid, "miExplorers2", StringComparison.InvariantCultureIgnoreCase))
                    {
                        explorersHelper2.FillExplorers(item);
                    }
                    else if (string.Equals(item.Uid, "miCompareOrganizations", StringComparison.InvariantCultureIgnoreCase))
                    {
                        compareWindowsHelper.FillCompareWindows(item);
                    }
                }
            }
        }

        private string GetEntityName()
        {
            var entity = GetSelectedEntity();

            return entity?.EntityName;
        }

        private string GetChartName1()
        {
            var entity = GetSelectedEntity();

            return entity?.ChartName1 ?? txtBFilter.Text.Trim();
        }

        private string GetChartName2()
        {
            var entity = GetSelectedEntity();

            return entity?.ChartName2 ?? txtBFilter.Text.Trim();
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

        private async Task ShowExistingCharts()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.LoadingCharts);

            this._itemsSource.Clear();

            IEnumerable<LinkedEntities<SavedQueryVisualization>> list = Enumerable.Empty<LinkedEntities<SavedQueryVisualization>>();

            try
            {
                var service1 = await GetService1();
                var service2 = await GetService2();

                if (service1 != null && service2 != null)
                {
                    string entityName = string.Empty;

                    this.Dispatcher.Invoke(() =>
                    {
                        if (!string.IsNullOrEmpty(cmBEntityName.Text)
                            && cmBEntityName.Items.Contains(cmBEntityName.Text)
                        )
                        {
                            entityName = cmBEntityName.Text.Trim().ToLower();
                        }
                    });

                    string filterEntity = null;

                    if (service1.ConnectionData.IsValidEntityName(entityName)
                        && service2.ConnectionData.IsValidEntityName(entityName)
                    )
                    {
                        filterEntity = entityName;
                    }

                    var temp = new List<LinkedEntities<SavedQueryVisualization>>();

                    if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                    {
                        var repository1 = new SavedQueryVisualizationRepository(service1);
                        var repository2 = new SavedQueryVisualizationRepository(service2);

                        var task1 = repository1.GetListAsync(filterEntity);
                        var task2 = repository2.GetListAsync(filterEntity);

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

                        var task1 = repository1.GetListAsync(filterEntity);

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
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
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
                        var type1 = ent.Entity1?.PrimaryEntityTypeCode ?? string.Empty;
                        var name1 = ent.Entity1?.Name ?? string.Empty;

                        var type2 = ent.Entity2?.PrimaryEntityTypeCode ?? string.Empty;
                        var name2 = ent.Entity2?.Name ?? string.Empty;

                        return type1.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                            || name1.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                            || type2.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                            || name2.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
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

            ToggleControls(true, Properties.OutputStrings.LoadingChartsCompletedFormat1, results.Count());
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

        private async void txtBFilterEnitity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await ShowExistingCharts();
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
                EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

                if (item != null)
                {
                    ExecuteAction(item.Link, false, PerformMouseDoubleClickAsync);
                }
            }
        }

        private async Task PerformMouseDoubleClickAsync(LinkedEntities<SavedQueryVisualization> linked, bool showAllways)
        {
            await PerformShowingDifferenceDescriptionAsync(linked, true);
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private void ExecuteAction(LinkedEntities<SavedQueryVisualization> linked, bool showAllways, Func<LinkedEntities<SavedQueryVisualization>, bool, Task> action)
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

        private Task<string> CreateFileAsync(ConnectionData connectionData, string entityName, string name, string fieldTitle, string xmlContent)
        {
            return Task.Run(() => CreateFile(connectionData, entityName, name, fieldTitle, xmlContent));
        }

        private string CreateFile(ConnectionData connectionData, string entityName, string name, string fieldTitle, string xmlContent)
        {
            string fileName = EntityFileNameFormatter.GetSavedQueryVisualizationFileName(connectionData.Name, entityName, name, fieldTitle, "xml");
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(xmlContent))
            {
                try
                {
                    xmlContent = ContentComparerHelper.FormatXmlByConfiguration(
                        xmlContent
                        , _commonConfig
                        , XmlOptionsControls.SavedQueryVisualizationXmlOptions
                        , schemaName: AbstractDynamicCommandXsdSchemas.VisualizationDataDescriptionSchema
                    );

                    File.WriteAllText(filePath, xmlContent, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, SavedQueryVisualization.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, SavedQueryVisualization.Schema.EntityLogicalName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
            }

            return filePath;
        }

        private Task<string> CreateDescriptionFileAsync(ConnectionData connectionData, string entityName, string name, string fieldTitle, string description)
        {
            return Task.Run(() => CreateDescriptionFile(connectionData, entityName, name, fieldTitle, description));
        }

        private string CreateDescriptionFile(ConnectionData connectionData, string entityName, string name, string fieldTitle, string description)
        {
            string fileName = EntityFileNameFormatter.GetSavedQueryVisualizationFileName(connectionData.Name, entityName, name, fieldTitle, "txt");
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(description))
            {
                try
                {
                    File.WriteAllText(filePath, description, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, SavedQueryVisualization.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
            else
            {
                filePath = string.Empty;
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, SavedQueryVisualization.Schema.EntityLogicalName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
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

            await PerformShowingDifferenceSingleXmlAsync(linked, showAllways, SavedQueryVisualization.Schema.Attributes.datadescription, SavedQueryVisualization.Schema.Headers.datadescription);

            await PerformShowingDifferenceSingleXmlAsync(linked, showAllways, SavedQueryVisualization.Schema.Attributes.presentationdescription, SavedQueryVisualization.Schema.Headers.presentationdescription);
        }

        private void mIShowDifferenceDataDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionLinked(link.Link, true, SavedQueryVisualization.Schema.Attributes.datadescription, SavedQueryVisualization.Schema.Headers.datadescription, PerformShowingDifferenceSingleXmlAsync);
        }

        private void mIShowDifferencePresentationDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionLinked(link.Link, true, SavedQueryVisualization.Schema.Attributes.presentationdescription, SavedQueryVisualization.Schema.Headers.presentationdescription, PerformShowingDifferenceSingleXmlAsync);
        }

        private void ExecuteActionLinked(
            LinkedEntities<SavedQueryVisualization> linked
            , bool showAllways
            , string fieldName
            , string fieldTitle
            , Func<LinkedEntities<SavedQueryVisualization>, bool, string, string, Task> action
        )
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            action(linked, showAllways, fieldName, fieldTitle);
        }

        private async Task PerformShowingDifferenceSingleXmlAsync(LinkedEntities<SavedQueryVisualization> linked, bool showAllways, string fieldName, string fieldTitle)
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
                    var repository1 = new SavedQueryVisualizationRepository(service1);
                    var repository2 = new SavedQueryVisualizationRepository(service2);

                    var chart1 = await repository1.GetByIdAsync(linked.Entity1.Id, new ColumnSet(true));
                    var chart2 = await repository2.GetByIdAsync(linked.Entity2.Id, new ColumnSet(true));

                    string xml1 = chart1.GetAttributeValue<string>(fieldName);
                    string xml2 = chart2.GetAttributeValue<string>(fieldName);

                    if (showAllways || !ContentComparerHelper.CompareXML(xml1, xml2).IsEqual)
                    {
                        string filePath1 = await CreateFileAsync(service1.ConnectionData, chart1.PrimaryEntityTypeCode, chart1.Name, fieldTitle, xml1);
                        string filePath2 = await CreateFileAsync(service2.ConnectionData, chart2.PrimaryEntityTypeCode, chart2.Name, fieldTitle, xml2);

                        if (!File.Exists(filePath1))
                        {
                            this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service1.ConnectionData.Name, SavedQueryVisualization.Schema.EntityLogicalName, chart1.Name, fieldTitle);
                            this._iWriteToOutput.ActivateOutputWindow(null);
                        }

                        if (!File.Exists(filePath2))
                        {
                            this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service2.ConnectionData.Name, SavedQueryVisualization.Schema.EntityLogicalName, chart2.Name, fieldTitle);
                            this._iWriteToOutput.ActivateOutputWindow(null);
                        }

                        if (File.Exists(filePath1) && File.Exists(filePath2))
                        {
                            await this._iWriteToOutput.ProcessStartProgramComparerAsync(service1.ConnectionData, filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2), service2.ConnectionData);
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

        private void mIExportSystemChart1DataDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity1.Id, GetService1, SavedQueryVisualization.Schema.Attributes.datadescription, SavedQueryVisualization.Schema.Headers.datadescription, PerformExportXmlToFile);
        }

        private void mIExportSystemChart2DataDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity2.Id, GetService2, SavedQueryVisualization.Schema.Attributes.datadescription, SavedQueryVisualization.Schema.Headers.datadescription, PerformExportXmlToFile);
        }

        private void mIExportSystemChart1PresentationDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity1.Id, GetService1, SavedQueryVisualization.Schema.Attributes.presentationdescription, SavedQueryVisualization.Schema.Headers.presentationdescription, PerformExportXmlToFile);
        }

        private void mIExportSystemChart2PresentationDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity2.Id, GetService2, SavedQueryVisualization.Schema.Attributes.presentationdescription, SavedQueryVisualization.Schema.Headers.presentationdescription, PerformExportXmlToFile);
        }

        private void ExecuteActionEntity(Guid idChart, Func<Task<IOrganizationServiceExtented>> getService, string fieldName, string fieldTitle, Func<Guid, Func<Task<IOrganizationServiceExtented>>, string, string, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            action(idChart, getService, fieldName, fieldTitle);
        }

        private async Task PerformExportXmlToFile(Guid idChart, Func<Task<IOrganizationServiceExtented>> getService, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.ExportingXmlFieldToFileFormat1, fieldTitle);

            var service = await getService();

            if (service != null)
            {
                var repository = new SavedQueryVisualizationRepository(service);

                var chart = await repository.GetByIdAsync(idChart, new ColumnSet(true));

                string xmlContent = chart.GetAttributeValue<string>(fieldName);

                string filePath = await CreateFileAsync(service.ConnectionData, chart.PrimaryEntityTypeCode, chart.Name, fieldTitle, xmlContent);

                if (!File.Exists(filePath))
                {
                    this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, SavedQueryVisualization.Schema.EntityLogicalName, chart.Name, fieldTitle);
                    this._iWriteToOutput.ActivateOutputWindow(null);
                }

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }

            ToggleControls(true, Properties.OutputStrings.ExportingXmlFieldToFileCompletedFormat1, fieldName);
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
                    var repository1 = new SavedQueryVisualizationRepository(service1);
                    var repository2 = new SavedQueryVisualizationRepository(service2);

                    var chart1 = await repository1.GetByIdAsync(linked.Entity1.Id, new ColumnSet(true));
                    var chart2 = await repository2.GetByIdAsync(linked.Entity2.Id, new ColumnSet(true));

                    var desc1 = await EntityDescriptionHandler.GetEntityDescriptionAsync(chart1);
                    var desc2 = await EntityDescriptionHandler.GetEntityDescriptionAsync(chart2);

                    if (showAllways || desc1 != desc2)
                    {
                        string filePath1 = await CreateDescriptionFileAsync(service1.ConnectionData, chart1.PrimaryEntityTypeCode, chart1.Name, EntityFileNameFormatter.Headers.EntityDescription, desc1);
                        string filePath2 = await CreateDescriptionFileAsync(service2.ConnectionData, chart2.PrimaryEntityTypeCode, chart2.Name, EntityFileNameFormatter.Headers.EntityDescription, desc2);

                        if (File.Exists(filePath1) && File.Exists(filePath2))
                        {
                            await this._iWriteToOutput.ProcessStartProgramComparerAsync(service1.ConnectionData, filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2), service2.ConnectionData);
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

        private void ExecuteActionDescription(Guid idChart, Func<Task<IOrganizationServiceExtented>> getService, Func<Guid, Func<Task<IOrganizationServiceExtented>>, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            action(idChart, getService);
        }

        private async Task PerformExportDescriptionToFile(Guid idChart, Func<Task<IOrganizationServiceExtented>> getService)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.CreatingEntityDescription);

            var service = await getService();

            if (service != null)
            {
                var repository = new SavedQueryVisualizationRepository(service);

                var chart = await repository.GetByIdAsync(idChart, new ColumnSet(true));

                var description = await EntityDescriptionHandler.GetEntityDescriptionAsync(chart, service.ConnectionData);

                string filePath = await CreateDescriptionFileAsync(service.ConnectionData, chart.PrimaryEntityTypeCode, chart.Name, EntityFileNameFormatter.Headers.EntityDescription, description);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }

            ToggleControls(true, Properties.OutputStrings.CreatingEntityDescriptionCompleted);
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

        protected override async Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            await ShowExistingCharts();
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

                    LoadEntityNames(cmBEntityName, connection1, connection2);

                    UpdateButtonsEnable();

                    var task = ShowExistingCharts();
                }
            });
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

            ActivateControls(items, hasSecondEntity, "menuContextConnection2", "miExplorers2");
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

        private async void miConnection1OpenEntityMetadataInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
               || !entity.EntityName.IsValidEntityName()
            )
            {
                return;
            }

            var service = await GetService1();

            service.ConnectionData.OpenEntityMetadataInWeb(entity.EntityName);
        }

        private async void miConnection2OpenEntityMetadataInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
               || !entity.EntityName.IsValidEntityName()
            )
            {
                return;
            }

            var service = await GetService2();

            service.ConnectionData.OpenEntityMetadataInWeb(entity.EntityName);
        }

        private async void miConnection1OpenEntityFetchXmlFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
               || !entity.EntityName.IsValidEntityName()
            )
            {
                return;
            }

            var service = await GetService1();

            this._iWriteToOutput.OpenFetchXmlFile(service.ConnectionData, _commonConfig, entity.EntityName);
        }

        private async void miConnection2OpenEntityFetchXmlFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
               || !entity.EntityName.IsValidEntityName()
            )
            {
                return;
            }

            var service = await GetService2();

            this._iWriteToOutput.OpenFetchXmlFile(service.ConnectionData, _commonConfig, entity.EntityName);
        }

        private async void miConnection1OpenEntityInstanceListInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
               || !entity.EntityName.IsValidEntityName()
            )
            {
                return;
            }

            var service = await GetService1();

            service.ConnectionData.OpenEntityInstanceListInWeb(entity.EntityName);
        }

        private async void miConnection2OpenEntityInstanceListInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null
               || !entity.EntityName.IsValidEntityName()
            )
            {
                return;
            }

            var service = await GetService2();

            service.ConnectionData.OpenEntityInstanceListInWeb(entity.EntityName);
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
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.SavedQueryVisualization, entity.Link.Entity1.Id);
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
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.SavedQueryVisualization, entity.Link.Entity2.Id);
            }
        }
    }
}