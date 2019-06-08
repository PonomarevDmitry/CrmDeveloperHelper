using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections;
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
    public partial class WindowOrganizationComparerWorkflow : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private readonly IWriteToOutput _iWriteToOutput;

        private readonly Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();

        private readonly CommonConfiguration _commonConfig;

        private readonly ObservableCollection<EntityViewItem> _itemsSource;

        public WindowOrganizationComparerWorkflow(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filterEntity
            , string filter
        )
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;

            BindingOperations.EnableCollectionSynchronization(connection1.ConnectionConfiguration.Connections, sysObjectConnections);

            InitializeComponent();

            LoadEntityNames(cmBEntityName, connection1, connection2);

            cmBCategory.ItemsSource = new EnumBindingSourceExtension(typeof(Workflow.Schema.OptionSets.category?)).ProvideValue(null) as IEnumerable;
            cmBMode.ItemsSource = new EnumBindingSourceExtension(typeof(Workflow.Schema.OptionSets.mode?)).ProvideValue(null) as IEnumerable;

            this._iWriteToOutput = iWriteToOutput;

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

            this.lstVwWorkflows.ItemsSource = _itemsSource;

            cmBConnection1.ItemsSource = connection1.ConnectionConfiguration.Connections;
            cmBConnection1.SelectedItem = connection1;

            cmBConnection2.ItemsSource = connection1.ConnectionConfiguration.Connections;
            cmBConnection2.SelectedItem = connection2;

            this.DecreaseInit();

            ShowExistingWorkflows();
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

        private async Task ShowExistingWorkflows()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.LoadingWorkflows);

            this._itemsSource.Clear();

            int? category = null;

            cmBCategory.Dispatcher.Invoke(() =>
            {
                if (cmBCategory.SelectedItem is Workflow.Schema.OptionSets.category comboBoxItem)
                {
                    category = (int)comboBoxItem;
                }
            });

            int? mode = null;

            cmBMode.Dispatcher.Invoke(() =>
            {
                if (cmBMode.SelectedItem is Workflow.Schema.OptionSets.mode comboBoxItem)
                {
                    mode = (int)comboBoxItem;
                }
            });

            IEnumerable<LinkedEntities<Workflow>> list = Enumerable.Empty<LinkedEntities<Workflow>>();

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

                    var columnSet = new ColumnSet
                    (
                        Workflow.Schema.Attributes.name
                        , Workflow.Schema.Attributes.uniquename
                        , Workflow.Schema.Attributes.category
                        , Workflow.Schema.Attributes.mode
                        , Workflow.Schema.Attributes.primaryentity
                        , Workflow.Schema.Attributes.statecode
                        , Workflow.Schema.Attributes.statuscode
                    );

                    List<LinkedEntities<Workflow>> temp = new List<LinkedEntities<Workflow>>();

                    if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                    {
                        var repository1 = new WorkflowRepository(service1);
                        var repository2 = new WorkflowRepository(service2);

                        var task1 = repository1.GetListAsync(filterEntity, category, mode, columnSet);
                        var task2 = repository2.GetListAsync(filterEntity, category, mode, columnSet);

                        TranslationRepository.GetDefaultTranslationFromCacheAsync(service1.ConnectionData.ConnectionId, service1);
                        TranslationRepository.GetDefaultTranslationFromCacheAsync(service2.ConnectionData.ConnectionId, service2);

                        var list1 = await task1;
                        var list2 = await task2;

                        foreach (var workflow1 in list1)
                        {
                            var workflow2 = list2.FirstOrDefault(c => c.Id == workflow1.Id);

                            if (workflow2 == null)
                            {
                                continue;
                            }

                            temp.Add(new LinkedEntities<Workflow>(workflow1, workflow2));
                        }
                    }
                    else
                    {
                        var repository1 = new WorkflowRepository(service1);

                        var task1 = repository1.GetListAsync(filterEntity, category, mode, columnSet);

                        TranslationRepository.GetDefaultTranslationFromCacheAsync(service1.ConnectionData.ConnectionId, service1);

                        var list1 = await task1;

                        foreach (var workflow1 in list1)
                        {
                            temp.Add(new LinkedEntities<Workflow>(workflow1, null));
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

        private static IEnumerable<LinkedEntities<Workflow>> FilterList(IEnumerable<LinkedEntities<Workflow>> list, string textName)
        {
            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                if (Guid.TryParse(textName, out Guid tempGuid))
                {
                    list = list.Where(ent =>
                        ent.Entity1?.Id == tempGuid
                        || ent.Entity2?.Id == tempGuid
                        || ent.Entity1?.WorkflowIdUnique == tempGuid
                        || ent.Entity2?.WorkflowIdUnique == tempGuid
                    );
                }
                else
                {
                    list = list.Where(ent =>
                    {
                        var type1 = ent.Entity1?.PrimaryEntity?.ToLower() ?? string.Empty;
                        var name1 = ent.Entity1?.Name?.ToLower() ?? string.Empty;

                        var type2 = ent.Entity2?.PrimaryEntity?.ToLower() ?? string.Empty;
                        var name2 = ent.Entity2?.Name?.ToLower() ?? string.Empty;

                        var nameUnique1 = ent.Entity1?.UniqueName?.ToLower() ?? string.Empty;
                        var nameUnique2 = ent.Entity2?.UniqueName?.ToLower() ?? string.Empty;

                        return type1.Contains(textName)
                            || name1.Contains(textName)
                            || type2.Contains(textName)
                            || name2.Contains(textName)
                            || nameUnique1.Contains(textName)
                            || nameUnique2.Contains(textName)
                            ;
                    });
                }
            }

            return list;
        }

        private class EntityViewItem
        {
            public string EntityName => Link.Entity1?.PrimaryEntity;

            public string Category { get; }

            public string WorkflowName1 => Link.Entity1?.Name;

            public string WorkflowName2 => Link.Entity2?.Name;

            public string WorkflowUniqueName1 => Link.Entity1?.UniqueName;

            public string WorkflowUniqueName2 => Link.Entity2?.UniqueName;

            public string StatusCode1 { get; }

            public string StatusCode2 { get; }

            public LinkedEntities<Workflow> Link { get; private set; }

            public EntityViewItem(LinkedEntities<Workflow> link)
            {
                this.Link = link;

                link.Entity1.FormattedValues.TryGetValue(Workflow.Schema.Attributes.category, out var category);
                this.Category = category;

                link.Entity1.FormattedValues.TryGetValue(Workflow.Schema.Attributes.statuscode, out var statuscode1);
                this.StatusCode1 = statuscode1;

                if (link.Entity2 != null)
                {
                    link.Entity2.FormattedValues.TryGetValue(Workflow.Schema.Attributes.statuscode, out var statuscode2);
                    this.StatusCode2 = statuscode2;
                }
            }
        }

        private void LoadEntities(IEnumerable<LinkedEntities<Workflow>> results)
        {
            this.lstVwWorkflows.Dispatcher.Invoke(() =>
            {
                foreach (var link in results
                      .OrderBy(ent => ent.Entity1.PrimaryEntity)
                      .OrderBy(ent => ent.Entity1?.Category?.Value)
                      .ThenBy(ent => ent.Entity1?.Name)
                      .ThenBy(ent => ent.Entity2?.Name)
                  )
                {
                    var item = new EntityViewItem(link);

                    this._itemsSource.Add(item);
                }

                if (this.lstVwWorkflows.Items.Count == 1)
                {
                    this.lstVwWorkflows.SelectedItem = this.lstVwWorkflows.Items[0];
                }
            });

            ToggleControls(true, Properties.WindowStatusStrings.LoadingWorkflowsCompletedFormat1, results.Count());
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

            ToggleControl(this.tSProgressBar
                , this.cmBConnection1
                , this.cmBConnection2
                , this.cmBCategory
                , this.cmBMode
            );

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwWorkflows.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwWorkflows.SelectedItems.Count > 0;

                    var item = (this.lstVwWorkflows.SelectedItems[0] as EntityViewItem);

                    btnShowDifferenceAll.IsEnabled = tSDDBShowDifference.IsEnabled = enabled && item.Link.Entity1 != null && item.Link.Entity2 != null;

                    tSDDBConnection1.IsEnabled = enabled && item.Link.Entity1 != null;
                    tSDDBConnection2.IsEnabled = enabled && item.Link.Entity2 != null;
                }
                catch (Exception)
                {
                }
            });
        }

        private void txtBFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowExistingWorkflows();
            }
        }

        private EntityViewItem GetSelectedEntity()
        {
            return this.lstVwWorkflows.SelectedItems.OfType<EntityViewItem>().Count() == 1
                ? this.lstVwWorkflows.SelectedItems.OfType<EntityViewItem>().SingleOrDefault() : null;
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
                    ExecuteActionOnLink(item.Link, false, PerformShowingDifferenceAllAsync);
                }
            }
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        private void ExecuteActionOnLink(LinkedEntities<Workflow> linked, bool showAllways, Func<LinkedEntities<Workflow>, bool, Task> action)
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

        private Task<string> CreateFileAsync(ConnectionData connectionData, string entityName, string category, string name, string fieldTitle, string extension, string xmlContent)
        {
            return Task.Run(() => CreateFile(connectionData, entityName, category, name, fieldTitle, extension, xmlContent));
        }

        private string CreateFile(ConnectionData connectionData, string entityName, string category, string name, string fieldTitle, string extension, string xmlContent)
        {
            string fileName = EntityFileNameFormatter.GetWorkflowFileName(connectionData.Name, entityName, category, name, fieldTitle, extension);
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(xmlContent))
            {
                try
                {
                    if (string.Equals(extension, "xml", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (ContentCoparerHelper.TryParseXml(xmlContent, out var doc))
                        {
                            xmlContent = doc.ToString();
                        }
                    }
                    else if (string.Equals(extension, "xml", StringComparison.InvariantCultureIgnoreCase))
                    {
                        xmlContent = ContentCoparerHelper.FormatJson(xmlContent);
                    }

                    File.WriteAllText(filePath, xmlContent, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, Workflow.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, Workflow.Schema.EntityLogicalName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
            }

            return filePath;
        }

        private Task<string> CreateDescriptionFileAsync(ConnectionData connectionData, string entityName, string category, string name, string fieldTitle, string extension, string description)
        {
            return Task.Run(() => CreateDescriptionFile(connectionData, entityName, category, name, fieldTitle, extension, description));
        }

        private string CreateDescriptionFile(ConnectionData connectionData, string entityName, string category, string name, string fieldTitle, string extension, string description)
        {
            string fileName = EntityFileNameFormatter.GetWorkflowFileName(connectionData.Name, entityName, category, name, fieldTitle, extension);
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(description))
            {
                try
                {
                    File.WriteAllText(filePath, description, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldExportedToFormat5, connectionData.Name, Workflow.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
            else
            {
                filePath = string.Empty;
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.EntityFieldIsEmptyFormat4, connectionData.Name, Workflow.Schema.EntityLogicalName, name, fieldTitle);
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

            ExecuteActionOnLink(link.Link, false, PerformShowingDifferenceAllAsync);
        }

        private async Task PerformShowingDifferenceAllAsync(LinkedEntities<Workflow> linked, bool showAllways)
        {
            await PerformShowingDifferenceDescriptionAsync(linked, showAllways);

            //await PerformShowingDifferenceCorrectedXamlAsync(linked, showAllways, Workflow.Schema.Attributes.xaml, "CorrectedXaml);

            await PerformShowingDifferenceSingleXmlAsync(linked, showAllways, Workflow.Schema.Attributes.xaml, Workflow.Schema.Headers.xaml);

            await PerformShowingDifferenceSingleXmlAsync(linked, showAllways, Workflow.Schema.Attributes.inputparameters, Workflow.Schema.Headers.inputparameters);

            await PerformShowingDifferenceSingleXmlAsync(linked, showAllways, Workflow.Schema.Attributes.clientdata, Workflow.Schema.Headers.clientdata);

            await PerformShowingDifferenceSingleXmlAsync(linked, showAllways, Workflow.Schema.Attributes.uidata, Workflow.Schema.Headers.uidata);

            await PerformShowingDifferenceSingleXmlAsync(linked, showAllways, Workflow.Schema.Attributes.processroleassignment, Workflow.Schema.Headers.processroleassignment);
        }

        private void mIShowDifferenceXaml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionLinked(link.Link, true, Workflow.Schema.Attributes.xaml, Workflow.Schema.Headers.xaml, PerformShowingDifferenceSingleXmlAsync);
        }

        private void mIShowDifferenceCorrectedXaml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionLinked(link.Link, true, Workflow.Schema.Attributes.xaml, Workflow.Schema.Headers.CorrectedXaml, PerformShowingDifferenceCorrectedXamlAsync);
        }

        private void mIShowDifferenceInputParameters_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionLinked(link.Link, true, Workflow.Schema.Attributes.inputparameters, Workflow.Schema.Headers.inputparameters, PerformShowingDifferenceSingleXmlAsync);
        }

        private void mIShowDifferenceClientData_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionLinked(link.Link, true, Workflow.Schema.Attributes.clientdata, Workflow.Schema.Headers.clientdata, PerformShowingDifferenceSingleXmlAsync);
        }

        private void mIShowDifferenceUIData_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionLinked(link.Link, true, Workflow.Schema.Attributes.uidata, Workflow.Schema.Headers.uidata, PerformShowingDifferenceSingleXmlAsync);
        }

        private void mIShowDifferenceProcessRoleAssignment_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionLinked(link.Link, true, Workflow.Schema.Attributes.processroleassignment, Workflow.Schema.Headers.processroleassignment, PerformShowingDifferenceSingleXmlAsync);
        }

        private void ExecuteActionLinked(LinkedEntities<Workflow> linked, bool showAllways, string fieldName, string fieldTitle, Func<LinkedEntities<Workflow>, bool, string, string, Task> action)
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

        private async Task PerformShowingDifferenceSingleXmlAsync(LinkedEntities<Workflow> linked, bool showAllways, string fieldName, string fieldTitle)
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
                    var repository1 = new WorkflowRepository(service1);
                    var repository2 = new WorkflowRepository(service2);

                    var workflow1 = await repository1.GetByIdAsync(linked.Entity1.Id, new ColumnSet(true));
                    var workflow2 = await repository2.GetByIdAsync(linked.Entity2.Id, new ColumnSet(true));

                    string xml1 = workflow1.GetAttributeValue<string>(fieldName);
                    string xml2 = workflow2.GetAttributeValue<string>(fieldName);

                    if (showAllways || !ContentCoparerHelper.CompareXML(xml1, xml2).IsEqual)
                    {
                        string entityName1 = workflow1.PrimaryEntity;
                        string entityName2 = workflow2.PrimaryEntity;

                        string name1 = workflow1.Name;
                        string name2 = workflow2.Name;

                        string category1 = workflow1.FormattedValues[Workflow.Schema.Attributes.category];
                        string category2 = workflow2.FormattedValues[Workflow.Schema.Attributes.category];

                        string extension = "json";

                        {
                            if (ContentCoparerHelper.TryParseXml(xml1, out var doc))
                            {
                                extension = "xml";
                                xml1 = doc.ToString();
                            }
                            else
                            {
                                xml1 = ContentCoparerHelper.FormatJson(xml1);
                            }
                        }

                        {
                            if (ContentCoparerHelper.TryParseXml(xml2, out var doc))
                            {
                                extension = "xml";
                                xml2 = doc.ToString();
                            }
                            else
                            {
                                xml2 = ContentCoparerHelper.FormatJson(xml2);
                            }
                        }

                        string filePath1 = await CreateFileAsync(service1.ConnectionData, entityName1, category1, name1, fieldTitle, extension, xml1);
                        string filePath2 = await CreateFileAsync(service2.ConnectionData, entityName2, category2, name2, fieldTitle, extension, xml2);

                        if (!File.Exists(filePath1))
                        {
                            this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service1.ConnectionData.Name, Workflow.Schema.EntityLogicalName, workflow1.Name, fieldTitle);
                            this._iWriteToOutput.ActivateOutputWindow(null);
                        }

                        if (!File.Exists(filePath2))
                        {
                            this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service2.ConnectionData.Name, Workflow.Schema.EntityLogicalName, workflow2.Name, fieldTitle);
                            this._iWriteToOutput.ActivateOutputWindow(null);
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

        private async Task PerformShowingDifferenceCorrectedXamlAsync(LinkedEntities<Workflow> linked, bool showAllways, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.ShowingDifferenceForCorrectedFieldFormat1, fieldTitle);

            var service1 = await GetService1();
            var service2 = await GetService2();

            if (service1 != null && service2 != null)
            {
                var repository1 = new WorkflowRepository(service1);
                var repository2 = new WorkflowRepository(service2);

                var workflow1 = await repository1.GetByIdAsync(linked.Entity1.Id, new ColumnSet(true));
                var workflow2 = await repository2.GetByIdAsync(linked.Entity2.Id, new ColumnSet(true));

                string xml1 = workflow1.GetAttributeValue<string>(fieldName);
                string xml2 = workflow2.GetAttributeValue<string>(fieldName);

                var replacer1 = new LabelReplacer(await TranslationRepository.GetDefaultTranslationFromCacheAsync(service1.ConnectionData.ConnectionId, service1));
                var replacer2 = new LabelReplacer(await TranslationRepository.GetDefaultTranslationFromCacheAsync(service2.ConnectionData.ConnectionId, service2));

                xml1 = ContentCoparerHelper.GetCorrectedXaml(xml1, replacer1);
                xml2 = ContentCoparerHelper.GetCorrectedXaml(xml2, replacer2);

                if (showAllways || !ContentCoparerHelper.CompareXML(xml1, xml2).IsEqual)
                {
                    string entityName1 = workflow1.PrimaryEntity;
                    string entityName2 = workflow2.PrimaryEntity;

                    string name1 = workflow1.Name;
                    string name2 = workflow2.Name;

                    string category1 = workflow1.FormattedValues[Workflow.Schema.Attributes.category];
                    string category2 = workflow2.FormattedValues[Workflow.Schema.Attributes.category];

                    string filePath1 = await CreateFileAsync(service1.ConnectionData, entityName1, category1, name1, Workflow.Schema.Headers.CorrectedXaml, "xml", xml1);
                    string filePath2 = await CreateFileAsync(service2.ConnectionData, entityName2, category2, name2, Workflow.Schema.Headers.CorrectedXaml, "xml", xml2);

                    if (!File.Exists(filePath1))
                    {
                        this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service1.ConnectionData.Name, Workflow.Schema.EntityLogicalName, workflow1.Name, fieldTitle);
                        this._iWriteToOutput.ActivateOutputWindow(null);
                    }

                    if (!File.Exists(filePath2))
                    {
                        this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service2.ConnectionData.Name, Workflow.Schema.EntityLogicalName, workflow2.Name, fieldTitle);
                        this._iWriteToOutput.ActivateOutputWindow(null);
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

            ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceForCorrectedFieldCompletedFormat1, fieldTitle);
        }

        private void mIExportWorkflow1Xaml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity1.Id, GetService1, Workflow.Schema.Attributes.xaml, Workflow.Schema.Headers.xaml, PerformExportXmlToFileAsync);
        }

        private void mIExportWorkflow1CorrectedXaml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity1.Id, GetService1, Workflow.Schema.Attributes.xaml, Workflow.Schema.Headers.CorrectedXaml, PerformExportCorrectedToFileAsync);
        }

        private void mIExportWorkflow2Xaml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity2.Id, GetService2, Workflow.Schema.Attributes.xaml, Workflow.Schema.Headers.xaml, PerformExportXmlToFileAsync);
        }

        private void mIExportWorkflow2CorrectedXaml_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity2.Id, GetService2, Workflow.Schema.Attributes.xaml, Workflow.Schema.Headers.CorrectedXaml, PerformExportCorrectedToFileAsync);
        }

        private void mIExportWorkflow1InputParameters_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity1.Id, GetService1, Workflow.Schema.Attributes.inputparameters, Workflow.Schema.Headers.inputparameters, PerformExportXmlToFileAsync);
        }

        private void mIExportWorkflow1ClientData_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity1.Id, GetService1, Workflow.Schema.Attributes.clientdata, Workflow.Schema.Headers.clientdata, PerformExportXmlToFileAsync);
        }

        private void mIExportWorkflow2InputParameters_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity2.Id, GetService2, Workflow.Schema.Attributes.inputparameters, Workflow.Schema.Headers.inputparameters, PerformExportXmlToFileAsync);
        }

        private void mIExportWorkflow2ClientData_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity1.Id, GetService1, Workflow.Schema.Attributes.clientdata, Workflow.Schema.Headers.clientdata, PerformExportXmlToFileAsync);
        }

        private void mIExportWorkflow1UIData_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity1.Id, GetService1, Workflow.Schema.Attributes.uidata, Workflow.Schema.Headers.uidata, PerformExportXmlToFileAsync);
        }

        private void mIExportWorkflow2UIData_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity2.Id, GetService2, Workflow.Schema.Attributes.uidata, Workflow.Schema.Headers.uidata, PerformExportXmlToFileAsync);
        }

        private void mIExportWorkflow1ProcessRoleAssignment_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity1.Id, GetService1, Workflow.Schema.Attributes.processroleassignment, Workflow.Schema.Headers.processroleassignment, PerformExportXmlToFileAsync);
        }

        private void mIExportWorkflow2ProcessRoleAssignment_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionEntity(link.Link.Entity2.Id, GetService2, Workflow.Schema.Attributes.processroleassignment, Workflow.Schema.Headers.processroleassignment, PerformExportXmlToFileAsync);
        }

        private void ExecuteActionEntity(Guid idWorflow, Func<Task<IOrganizationServiceExtented>> getService, string fieldName, string fieldTitle, Func<Guid, Func<Task<IOrganizationServiceExtented>>, string, string, Task> action)
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

            action(idWorflow, getService, fieldName, fieldTitle);
        }

        private async Task PerformExportXmlToFileAsync(Guid idWorflow, Func<Task<IOrganizationServiceExtented>> getService, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.ExportingXmlFieldToFileFormat1, fieldTitle);

            var service = await getService();

            if (service != null)
            {
                var repository = new WorkflowRepository(service);

                var workflow = await repository.GetByIdAsync(idWorflow, new ColumnSet(true));

                string entityName = workflow.PrimaryEntity;
                string name = workflow.Name;
                string category = workflow.FormattedValues[Workflow.Schema.Attributes.category];

                string xmlContent = workflow.GetAttributeValue<string>(fieldName);

                string extension = "json";

                if (ContentCoparerHelper.TryParseXml(xmlContent, out var doc))
                {
                    extension = "xml";

                    xmlContent = doc.ToString();
                }
                else
                {
                    xmlContent = ContentCoparerHelper.FormatJson(xmlContent);
                }

                string filePath = await CreateFileAsync(service.ConnectionData, entityName, category, name, fieldTitle, extension, xmlContent);

                if (!File.Exists(filePath))
                {
                    this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, Workflow.Schema.EntityLogicalName, workflow.Name, fieldTitle);
                    this._iWriteToOutput.ActivateOutputWindow(null);
                }

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }

            ToggleControls(true, Properties.WindowStatusStrings.ExportingXmlFieldToFileCompletedFormat1, fieldTitle);
        }

        private async Task PerformExportCorrectedToFileAsync(Guid idWorflow, Func<Task<IOrganizationServiceExtented>> getService, string fieldName, string fieldTitle)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.ExportingCorrectedXmlFieldToFileFormat1, fieldTitle);

            var service = await getService();

            if (service != null)
            {
                var repository = new WorkflowRepository(service);

                var workflow = await repository.GetByIdAsync(idWorflow, new ColumnSet(true));

                string xmlContent = workflow.GetAttributeValue<string>(fieldName);

                var replacer = new LabelReplacer(await TranslationRepository.GetDefaultTranslationFromCacheAsync(service.ConnectionData.ConnectionId, service));

                xmlContent = ContentCoparerHelper.GetCorrectedXaml(xmlContent, replacer);

                string entityName = workflow.PrimaryEntity;
                string name = workflow.Name;
                workflow.FormattedValues.TryGetValue(Workflow.Schema.Attributes.category, out var category);

                string filePath = await CreateFileAsync(service.ConnectionData, entityName, category, name, Workflow.Schema.Headers.CorrectedXaml, "xml", xmlContent);

                if (!File.Exists(filePath))
                {
                    this._iWriteToOutput.WriteToOutput(null, Properties.OutputStrings.EntityFieldIsEmptyFormat4, service.ConnectionData.Name, Workflow.Schema.EntityLogicalName, workflow.Name, fieldTitle);
                    this._iWriteToOutput.ActivateOutputWindow(null);
                }

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }

            ToggleControls(true, Properties.WindowStatusStrings.ExportingCorrectedXmlFieldToFileCompletedFormat1, fieldTitle);
        }

        private void mIShowDifferenceEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null)
            {
                return;
            }

            ExecuteActionOnLink(link.Link, true, PerformShowingDifferenceDescriptionAsync);
        }

        private async Task PerformShowingDifferenceDescriptionAsync(LinkedEntities<Workflow> linked, bool showAllways)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.ShowingDifferenceEntityDescriptions);

            var service1 = await GetService1();
            var service2 = await GetService2();

            if (service1 != null && service2 != null)
            {
                var repository1 = new WorkflowRepository(service1);
                var repository2 = new WorkflowRepository(service2);

                var workflow1 = await repository1.GetByIdAsync(linked.Entity1.Id, new ColumnSet(true));
                var workflow2 = await repository2.GetByIdAsync(linked.Entity2.Id, new ColumnSet(true));

                var desc1 = await EntityDescriptionHandler.GetEntityDescriptionAsync(workflow1, EntityFileNameFormatter.WorkflowIgnoreFields);
                var desc2 = await EntityDescriptionHandler.GetEntityDescriptionAsync(workflow2, EntityFileNameFormatter.WorkflowIgnoreFields);

                if (showAllways || desc1 != desc2)
                {
                    string entityName1 = workflow1.PrimaryEntity;
                    string entityName2 = workflow2.PrimaryEntity;

                    string name1 = workflow1.Name;
                    string name2 = workflow2.Name;

                    string category1 = workflow1.FormattedValues[Workflow.Schema.Attributes.category];
                    string category2 = workflow2.FormattedValues[Workflow.Schema.Attributes.category];

                    string filePath1 = await CreateDescriptionFileAsync(service1.ConnectionData, entityName1, category1, name1, "Description", "txt", desc1);
                    string filePath2 = await CreateDescriptionFileAsync(service2.ConnectionData, entityName2, category2, name2, "Description", "txt", desc2);

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

            ToggleControls(true, Properties.WindowStatusStrings.ShowingDifferenceEntityDescriptionsCompleted);
        }

        private void ExecuteActionDescription(Guid idWorflow, Func<Task<IOrganizationServiceExtented>> getService, Func<Guid, Func<Task<IOrganizationServiceExtented>>, Task> action)
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

            action(idWorflow, getService);
        }

        private async Task PerformExportDescriptionToFileAsync(Guid idWorflow, Func<Task<IOrganizationServiceExtented>> getService)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.CreatingDescription);

            var service = await getService();

            if (service != null)
            {
                var repository = new WorkflowRepository(service);

                var workflow = await repository.GetByIdAsync(idWorflow, new ColumnSet(true));

                string entityName = workflow.PrimaryEntity;
                string name = workflow.Name;
                string category = workflow.FormattedValues[Workflow.Schema.Attributes.category];

                var description = await EntityDescriptionHandler.GetEntityDescriptionAsync(workflow, EntityFileNameFormatter.WorkflowIgnoreFields, service.ConnectionData);

                string filePath = await CreateDescriptionFileAsync(service.ConnectionData, entityName, category, name, "Description", "txt", description);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }

            ToggleControls(true, Properties.WindowStatusStrings.CreatingDescriptionCompleted);
        }

        private void mIExportWorkflow1EntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionDescription(link.Link.Entity1.Id, GetService1, PerformExportDescriptionToFileAsync);
        }

        private void mIExportWorkflow2EntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedEntity();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionDescription(link.Link.Entity2.Id, GetService2, PerformExportDescriptionToFileAsync);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingWorkflows();
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

                    LoadEntityNames(cmBEntityName, connection1, connection2);

                    UpdateButtonsEnable();

                    ShowExistingWorkflows();
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

            WindowHelper.OpenOrganizationComparerGlobalOptionSetsWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData);
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

        private async void btnCompareSavedChart_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service1 = await GetService1();
            var service2 = await GetService2();

            WindowHelper.OpenOrganizationComparerSavedQueryVisualizationWindow(this._iWriteToOutput, _commonConfig, service1.ConnectionData, service2.ConnectionData, entity?.EntityName);
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

        private async void btnEntityPrivilegesExplorer1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenEntityPrivilegesExplorer(this._iWriteToOutput, service, _commonConfig, entity?.EntityName);
        }

        private async void btnEntityPrivilegesExplorer2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenEntityPrivilegesExplorer(this._iWriteToOutput, service, _commonConfig, entity?.EntityName);
        }

        private async void btnSecurityRolesExplorer1_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenRolesExplorer(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnSecurityRolesExplorer2_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenRolesExplorer(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnCreateMetadataFile1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName);
        }

        private async void btnExportApplicationRibbon1_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenApplicationRibbonWindow(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnGlobalOptionSets1_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService1();

            _commonConfig.Save();

            WindowHelper.OpenGlobalOptionSetsWindow(
                this._iWriteToOutput
                , service
                , _commonConfig
            );
        }

        private async void btnSystemForms1_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService1();

            WindowHelper.OpenSystemFormWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName);
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

            WindowHelper.OpenWorkflowWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, entity?.WorkflowName1 ?? txtBFilter.Text);
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

            WindowHelper.OpenSdkMessageRequestTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName);
        }

        private async void btnCreateMetadataFile2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName);
        }

        private async void btnExportApplicationRibbon2_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenApplicationRibbonWindow(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnGlobalOptionSets2_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService2();

            _commonConfig.Save();

            WindowHelper.OpenGlobalOptionSetsWindow(
                this._iWriteToOutput
                , service
                , _commonConfig
            );
        }

        private async void btnSystemForms2_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService2();

            WindowHelper.OpenSystemFormWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName);
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

            WindowHelper.OpenWorkflowWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName, entity?.WorkflowName2 ?? txtBFilter.Text);
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

            WindowHelper.OpenSdkMessageRequestTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityName);
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
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.Workflow, entity.Link.Entity1.Id);
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
                service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.Workflow, entity.Link.Entity2.Id);
            }
        }

        private void cmBCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowExistingWorkflows();
        }

        private async void mIConnection1OpenWorkflowListInWeb_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService1();

            if (service != null)
            {
                service.ConnectionData.OpenEntityInstanceListInWeb(Workflow.EntityLogicalName);
            }
        }

        private async void mIConnection2OpenWorkflowListInWeb_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService2();

            if (service != null)
            {
                service.ConnectionData.OpenEntityInstanceListInWeb(Workflow.EntityLogicalName);
            }
        }
    }
}