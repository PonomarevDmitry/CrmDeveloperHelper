using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
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
using System.Windows.Resources;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExportRibbon : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private IWriteToOutput _iWriteToOutput;

        private CommonConfiguration _commonConfig;
        private ConnectionConfiguration _connectionConfig;

        private Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();
        private Dictionary<Guid, SolutionComponentDescriptor> _descriptorCache = new Dictionary<Guid, SolutionComponentDescriptor>();

        private Dictionary<Guid, IEnumerable<EntityMetadata>> _cacheEntityMetadata = new Dictionary<Guid, IEnumerable<EntityMetadata>>();
        private Dictionary<Guid, HashSet<string>> _cacheRibbonCustomization = new Dictionary<Guid, HashSet<string>>();

        private ObservableCollection<EntityMetadataListViewItem> _itemsSource;

        private bool _controlsEnabled = true;

        private int _init = 0;

        public WindowExportRibbon(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntity
            , IEnumerable<EntityMetadata> allEntities
        )
        {
            _init++;

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;
            this._connectionConfig = service.ConnectionData.ConnectionConfiguration;

            _connectionCache[service.ConnectionData.ConnectionId] = service;
            _descriptorCache[service.ConnectionData.ConnectionId] = new SolutionComponentDescriptor(service, true);

            if (allEntities != null)
            {
                _cacheEntityMetadata[service.ConnectionData.ConnectionId] = allEntities;
            }

            BindingOperations.EnableCollectionSynchronization(_connectionConfig.Connections, sysObjectConnections);

            InitializeComponent();

            LoadFromConfig();

            txtBFilterEnitity.Text = filterEntity;
            txtBFilterEnitity.SelectionLength = 0;
            txtBFilterEnitity.SelectionStart = txtBFilterEnitity.Text.Length;

            txtBFilterEnitity.Focus();

            _itemsSource = new ObservableCollection<EntityMetadataListViewItem>();

            lstVwEntities.ItemsSource = _itemsSource;

            UpdateButtonsEnable();

            cmBCurrentConnection.ItemsSource = _connectionConfig.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            _init--;

            ShowExistingEntities();
        }

        private void LoadFromConfig()
        {
            chBForm.DataContext = _commonConfig;
            chBHomepageGrid.DataContext = _commonConfig;
            chBSubGrid.DataContext = _commonConfig;

            chBXmlAttributeOnNewLine.DataContext = _commonConfig;

            cmBFileAction.DataContext = _commonConfig;

            chBSetXmlSchemas.DataContext = _commonConfig;

            chBSetIntellisenseContext.DataContext = _commonConfig;

            txtBFolder.DataContext = _commonConfig;
        }

        protected override void OnClosed(EventArgs e)
        {
            _commonConfig.Save();
            _connectionConfig.Save();

            BindingOperations.ClearAllBindings(cmBCurrentConnection);

            cmBCurrentConnection.Items.DetachFromSourceCollection();

            cmBCurrentConnection.ItemsSource = null;

            base.OnClosed(e);
        }

        private RibbonLocationFilters GetRibbonLocationFilters()
        {
            RibbonLocationFilters filter = RibbonLocationFilters.All;

            chBForm.Dispatcher.Invoke(() =>
            {

                if (chBForm.IsChecked.GetValueOrDefault() || chBHomepageGrid.IsChecked.GetValueOrDefault() || chBSubGrid.IsChecked.GetValueOrDefault())
                {
                    filter = 0;

                    if (chBForm.IsChecked.GetValueOrDefault())
                    {
                        filter |= RibbonLocationFilters.Form;
                    }

                    if (chBHomepageGrid.IsChecked.GetValueOrDefault())
                    {
                        filter |= RibbonLocationFilters.HomepageGrid;
                    }

                    if (chBSubGrid.IsChecked.GetValueOrDefault())
                    {
                        filter |= RibbonLocationFilters.SubGrid;
                    }
                }
            });

            return filter;
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

        private async Task ShowExistingEntities()
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.LoadingEntities);

            this._itemsSource.Clear();

            IEnumerable<EntityMetadata> list = Enumerable.Empty<EntityMetadata>();

            HashSet<string> hash = new HashSet<string>();

            try
            {
                var service = await GetService();

                if (service != null)
                {
                    if (!_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
                    {
                        EntityMetadataRepository repository = new EntityMetadataRepository(service);

                        var task = repository.GetEntitiesDisplayNameAsync();

                        _cacheEntityMetadata.Add(service.ConnectionData.ConnectionId, await task);
                    }

                    list = _cacheEntityMetadata[service.ConnectionData.ConnectionId];

                    if (!_cacheRibbonCustomization.ContainsKey(service.ConnectionData.ConnectionId))
                    {
                        var repository = new RibbonCustomizationRepository(service);

                        var task = repository.GetEntitiesWithRibbonCustomizationAsync();

                        _cacheRibbonCustomization.Add(service.ConnectionData.ConnectionId, await task);
                    }

                    hash = _cacheRibbonCustomization[service.ConnectionData.ConnectionId];
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }

            var textName = string.Empty;

            txtBFilterEnitity.Dispatcher.Invoke(() =>
            {
                textName = txtBFilterEnitity.Text.Trim().ToLower();
            });

            list = FilterList(list, textName, hash);

            LoadEntities(list);
        }

        private static IEnumerable<EntityMetadata> FilterList(IEnumerable<EntityMetadata> list, string textName, HashSet<string> hash)
        {
            list = list.Where(e => !e.IsIntersect.GetValueOrDefault());

            if (hash != null && hash.Any())
            {
                list = list.Where(e => hash.Contains(e.LogicalName));
            }

            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                if (int.TryParse(textName, out int tempInt))
                {
                    list = list.Where(ent => ent.ObjectTypeCode == tempInt);
                }
                else
                {
                    if (Guid.TryParse(textName, out Guid tempGuid))
                    {
                        list = list.Where(ent => ent.MetadataId == tempGuid);
                    }
                    else
                    {
                        list = list
                        .Where(ent =>
                            ent.LogicalName.ToLower().Contains(textName)
                            || (ent.DisplayName != null && ent.DisplayName.LocalizedLabels
                                .Where(l => !string.IsNullOrEmpty(l.Label))
                                .Any(lbl => lbl.Label.ToLower().Contains(textName)))

                        //|| (ent.Description != null && ent.Description.LocalizedLabels
                        //    .Where(l => !string.IsNullOrEmpty(l.Label))
                        //    .Any(lbl => lbl.Label.ToLower().Contains(textName)))

                        //|| (ent.DisplayCollectionName != null && ent.DisplayCollectionName.LocalizedLabels
                        //    .Where(l => !string.IsNullOrEmpty(l.Label))
                        //    .Any(lbl => lbl.Label.ToLower().Contains(textName)))
                        );
                    }
                }
            }

            return list;
        }

        private void LoadEntities(IEnumerable<EntityMetadata> results)
        {
            this.lstVwEntities.Dispatcher.Invoke(() =>
            {
                string textName = txtBFilterEnitity.Text.Trim().ToLower();

                foreach (var entity in results)
                {
                    string name = entity.LogicalName;
                    string displayName = CreateFileHandler.GetLocalizedLabel(entity.DisplayName);

                    EntityMetadataListViewItem item = new EntityMetadataListViewItem(name, displayName, entity);

                    this._itemsSource.Add(item);
                }

                if (this.lstVwEntities.Items.Count == 1)
                {
                    this.lstVwEntities.SelectedItem = this.lstVwEntities.Items[0];
                }
            });

            ToggleControls(true, Properties.WindowStatusStrings.LoadingEntitiesCompletedFormat1, results.Count());
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

            ToggleProgressBar(enabled);

            ToggleControl(cmBCurrentConnection, enabled);

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
            this.lstVwEntities.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this._controlsEnabled && this.lstVwEntities.SelectedItems.Count > 0;

                    UIElement[] list =
                    {
                        btnExportEntityRibbon
                    };

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
                ShowExistingEntities();
            }
        }

        private EntityMetadataListViewItem GetSelectedEntity()
        {
            return this.lstVwEntities.SelectedItems.OfType<EntityMetadataListViewItem>().Count() == 1
                ? this.lstVwEntities.SelectedItems.OfType<EntityMetadataListViewItem>().SingleOrDefault() : null;
        }

        #region Кнопки открытия других форм с информация о сущности.

        private async void btnCreateMetadataFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            IEnumerable<EntityMetadata> entityMetadataList = null;

            if (_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId];
            }

            WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName, entityMetadataList, null);
        }

        private async void btnEntityAttributeExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityAttributeExplorer(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName);
        }

        private async void btnEntityRelationshipOneToManyExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityRelationshipOneToManyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName);
        }

        private async void btnEntityRelationshipManyToManyExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityRelationshipManyToManyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName);
        }

        private async void btnEntityKeyExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityKeyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName);
        }

        private async void btnGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService();
            var descriptor = await GetDescriptor();

            var entityMetadata = descriptor.MetadataSource.GetEntityMetadata(entity.EntityMetadata.MetadataId.Value);

            IEnumerable<OptionSetMetadata> optionSets =
                entityMetadata
                ?.Attributes
                ?.OfType<PicklistAttributeMetadata>()
                .Where(a => a.OptionSet.IsGlobal.GetValueOrDefault())
                .Select(a => a.OptionSet)
                .GroupBy(o => o.MetadataId)
                .Select(g => g.FirstOrDefault())
                ?? new OptionSetMetadata[0]
                ;

            _commonConfig.Save();

            WindowHelper.OpenGlobalOptionSetsWindow(
                this._iWriteToOutput
                , service
                , _commonConfig
                , optionSets
                , string.Empty
                , string.Empty
                );
        }

        private async void btnSystemForms_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSystemFormWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName, string.Empty);
        }

        private async void btnSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSavedQueryWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName, string.Empty);
        }

        private async void btnSavedChart_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSavedQueryVisualizationWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName, string.Empty);
        }

        private async void btnWorkflows_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenWorkflowWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName, string.Empty);
        }

        private async void btnAttributesDependentComponent_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            IEnumerable<EntityMetadata> entityMetadataList = null;

            if (_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId];
            }

            WindowHelper.OpenAttributesDependentComponentWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName, entityMetadataList);
        }

        private async void btnPluginTree_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenPluginTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName, string.Empty, string.Empty);
        }

        private async void btnMessageTree_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName, string.Empty);
        }

        private async void btnMessageRequestTree_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageRequestTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName, string.Empty);
        }

        private async void btnOrganizationComparer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerWindow(this._iWriteToOutput, service.ConnectionData.ConnectionConfiguration, _commonConfig);
        }

        private async void btnCompareMetadataFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerEntityMetadataWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.EntityLogicalName);
        }

        private async void btnCompareRibbon_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerRibbonWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.EntityLogicalName);
        }

        private async void btnCompareGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerGlobalOptionSetsWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.EntityLogicalName);
        }

        private async void btnCompareSystemForms_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSystemFormWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.EntityLogicalName);
        }

        private async void btnCompareSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSavedQueryWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.EntityLogicalName);
        }

        private async void btnCompareSavedChart_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSavedQueryVisualizationWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.EntityLogicalName);
        }

        private async void btnCompareWorkflows_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerWorkflowWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.EntityLogicalName);
        }

        #endregion Кнопки открытия других форм с информация о сущности.

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnExportApplicationRibbon_Click(object sender, RoutedEventArgs e)
        {
            ExecuteActionOnApplicationRibbonAsync(PerformExportApplicationRibbon);
        }

        private async Task PerformExportApplicationRibbon()
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.ExportingApplicationRibbon);

            ToggleControls(false, Properties.WindowStatusStrings.ExportingApplicationRibbon);

            var service = await GetService();

            string fileName = EntityFileNameFormatter.GetApplicationRibbonFileName(service.ConnectionData.Name);
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            try
            {
                var repository = new RibbonCustomizationRepository(service);

                await repository.ExportApplicationRibbonAsync(filePath, _commonConfig);

                this._iWriteToOutput.WriteToOutput("Application Ribbon exported to {0}", filePath);

                this._iWriteToOutput.PerformAction(filePath, _commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }

            ToggleControls(true, Properties.WindowStatusStrings.ExportingApplicationRibbonCompleted);

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.ExportingApplicationRibbon);
        }

        private void btnExportApplicationRibbonDiffXml_Click(object sender, RoutedEventArgs e)
        {
            ExecuteActionOnApplicationRibbonAsync(PerformExportApplicationRibbonDiffXml);
        }

        private async Task PerformExportApplicationRibbonDiffXml()
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.ExportingApplicationRibbonDiffXml);

            ToggleControls(false, Properties.WindowStatusStrings.ExportingApplicationRibbonDiffXml);

            var service = await GetService();

            try
            {
                var repositoryPublisher = new PublisherRepository(service);

                var publisherDefault = await repositoryPublisher.GetDefaultPublisherAsync();

                var repositoryRibbonCustomization = new RibbonCustomizationRepository(service);

                var ribbonCustomization = await repositoryRibbonCustomization.FindApplicationRibbonCustomizationAsync();

                if (publisherDefault != null && ribbonCustomization != null)
                {
                    var solutionUniqueName = string.Format("RibbonDiffXml_{0}", Guid.NewGuid());

                    solutionUniqueName = solutionUniqueName.Replace("-", "_");

                    var solution = new Solution()
                    {
                        UniqueName = solutionUniqueName,
                        FriendlyName = solutionUniqueName,

                        Description = "Temporary solution for exporting RibbonDiffXml.",

                        PublisherId = publisherDefault.ToEntityReference(),

                        Version = "1.0.0.0",
                    };

                    UpdateStatus(Properties.WindowStatusStrings.CreatingNewSolutionFormat1, solutionUniqueName);

                    solution.Id = service.Create(solution);

                    UpdateStatus(Properties.WindowStatusStrings.AddingInSolutionApplicationRibbonFormat1, solutionUniqueName);

                    {
                        var repositorySolutionComponent = new SolutionComponentRepository(service);

                        await repositorySolutionComponent.AddSolutionComponentsAsync(solutionUniqueName, new[] { new SolutionComponent()
                        {
                            ComponentType = new OptionSetValue((int)ComponentType.RibbonCustomization),
                            ObjectId = ribbonCustomization.Id,
                            RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                        }});
                    }

                    UpdateStatus(Properties.WindowStatusStrings.ExportingSolutionAndExtractingApplicationRibbonDiffXmlFormat1, solutionUniqueName);

                    var repository = new ExportSolutionHelper(service);

                    string ribbonDiffXml = await repository.ExportSolutionAndGetApplicationRibbonDiffAsync(solutionUniqueName);

                    if (_commonConfig.SetXmlSchemasDuringExport)
                    {
                        var schemasResources = CommonExportXsdSchemasCommand.ListXsdSchemas.FirstOrDefault(e => string.Equals(e.Item1, "RibbonXml", StringComparison.InvariantCultureIgnoreCase));

                        if (schemasResources != null)
                        {
                            string schemas = ContentCoparerHelper.HandleExportXsdSchemaIntoSchamasFolder(schemasResources.Item2);

                            if (!string.IsNullOrEmpty(schemas))
                            {
                                ribbonDiffXml = ContentCoparerHelper.ReplaceXsdSchema(ribbonDiffXml, schemas);
                            }
                        }
                    }

                    if (_commonConfig.SetIntellisenseContext)
                    {
                        ribbonDiffXml = ContentCoparerHelper.SetRibbonDiffXmlIntellisenseContextEntityName(ribbonDiffXml, string.Empty);
                    }

                    ribbonDiffXml = ContentCoparerHelper.FormatXml(ribbonDiffXml, _commonConfig.ExportRibbonXmlXmlAttributeOnNewLine);

                    {
                        string fileName = EntityFileNameFormatter.GetApplicationRibbonDiffXmlFileName(service.ConnectionData.Name);
                        string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                        File.WriteAllText(filePath, ribbonDiffXml, new UTF8Encoding(false));

                        this._iWriteToOutput.WriteToOutput("Application RibbonDiffXml exported to {0}", filePath);

                        this._iWriteToOutput.PerformAction(filePath, _commonConfig);
                    }

                    UpdateStatus(Properties.WindowStatusStrings.DeletingSolutionFormat1, solutionUniqueName);

                    await service.DeleteAsync(solution.LogicalName, solution.Id);

                    ToggleControls(true, Properties.WindowStatusStrings.ExportingApplicationRibbonDiffXmlCompleted);
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.ExportingApplicationRibbonDiffXmlFailed);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.ExportingApplicationRibbonDiffXml);
        }

        private void mIUpdateApplicationRibbonDiffXml_Click(object sender, RoutedEventArgs e)
        {
            ExecuteActionOnApplicationRibbonAsync(PerformUpdateApplicationRibbonDiffXml);
        }

        private async Task PerformUpdateApplicationRibbonDiffXml()
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.UpdatingApplicationRibbonDiffXml);

            ToggleControls(false, Properties.WindowStatusStrings.UpdatingApplicationRibbonDiffXml);

            var service = await GetService();

            try
            {
                var newText = string.Empty;
                bool? dialogResult = false;

                var title = "Application RibbonDiffXml for entity {0}";

                this.Dispatcher.Invoke(() =>
                {
                    var form = new WindowTextField("Enter " + title, title, string.Empty);

                    dialogResult = form.ShowDialog();

                    newText = form.FieldText;
                });

                if (dialogResult.GetValueOrDefault() == false)
                {
                    ToggleControls(true, Properties.WindowStatusStrings.UpdatingApplicationRibbonDiffXmlCanceled);
                    return;
                }

                ContentCoparerHelper.ClearXsdSchema(newText, out newText);

                UpdateStatus(Properties.WindowStatusStrings.ValidatingApplicationRibbonDiffXml);

                if (!ContentCoparerHelper.TryParseXmlDocument(newText, out var doc))
                {
                    ToggleControls(true, Properties.WindowStatusStrings.TextIsNotValidXml);

                    _iWriteToOutput.ActivateOutputWindow();
                    return;
                }

                bool validateResult = await ValidateXmlDocumentAsync(doc);

                if (!validateResult)
                {
                    ToggleControls(true, Properties.WindowStatusStrings.ValidatingApplicationRibbonDiffXmlFailed);
                    return;
                }




                var repositoryPublisher = new PublisherRepository(service);

                var publisherDefault = await repositoryPublisher.GetDefaultPublisherAsync();

                var repositoryRibbonCustomization = new RibbonCustomizationRepository(service);

                var ribbonCustomization = await repositoryRibbonCustomization.FindApplicationRibbonCustomizationAsync();

                if (publisherDefault != null && ribbonCustomization != null)
                {
                    var solutionUniqueName = string.Format("RibbonDiffXml_{0}", Guid.NewGuid());

                    solutionUniqueName = solutionUniqueName.Replace("-", "_");

                    var solution = new Solution()
                    {
                        UniqueName = solutionUniqueName,
                        FriendlyName = solutionUniqueName,

                        Description = "Temporary solution for exporting RibbonDiffXml.",

                        PublisherId = publisherDefault.ToEntityReference(),

                        Version = "1.0.0.0",
                    };

                    UpdateStatus(Properties.WindowStatusStrings.CreatingNewSolutionFormat1, solutionUniqueName);

                    solution.Id = service.Create(solution);

                    UpdateStatus(Properties.WindowStatusStrings.AddingInSolutionApplicationRibbonFormat1, solutionUniqueName);

                    {
                        var repositorySolutionComponent = new SolutionComponentRepository(service);

                        await repositorySolutionComponent.AddSolutionComponentsAsync(solutionUniqueName, new[] { new SolutionComponent()
                        {
                            ComponentType = new OptionSetValue((int)ComponentType.RibbonCustomization),
                            ObjectId = ribbonCustomization.Id,
                            RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                        }});
                    }

                    UpdateStatus(Properties.WindowStatusStrings.ExportingSolutionAndExtractingApplicationRibbonDiffXmlFormat1, solutionUniqueName);

                    var repository = new ExportSolutionHelper(service);

                    var solutionBodyBinary = await repository.ExportSolutionAndGetBodyBinaryAsync(solutionUniqueName);

                    string ribbonDiffXml = ExportSolutionHelper.GetApplicationRibbonDiffXmlFromSolutionBody(solutionBodyBinary);

                    {
                        string fileName = string.Format("{0}.{1}_{2} Solution Backup at {3}.zip"
                            , service.ConnectionData.Name
                            , solution.UniqueName
                            , solution.Version
                            , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss")
                            );

                        string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                        File.WriteAllBytes(filePath, solutionBodyBinary);

                        this._iWriteToOutput.WriteToOutput("Solution {0} Backup exported to {1}", solution.UniqueName, filePath);

                        this._iWriteToOutput.WriteToOutputFilePathUri(filePath);
                    }

                    if (_commonConfig.SetXmlSchemasDuringExport)
                    {
                        var schemasResources = CommonExportXsdSchemasCommand.ListXsdSchemas.FirstOrDefault(e => string.Equals(e.Item1, "RibbonXml", StringComparison.InvariantCultureIgnoreCase));

                        if (schemasResources != null)
                        {
                            string schemas = ContentCoparerHelper.HandleExportXsdSchemaIntoSchamasFolder(schemasResources.Item2);

                            if (!string.IsNullOrEmpty(schemas))
                            {
                                ribbonDiffXml = ContentCoparerHelper.ReplaceXsdSchema(ribbonDiffXml, schemas);
                            }
                        }
                    }

                    if (_commonConfig.SetIntellisenseContext)
                    {
                        ribbonDiffXml = ContentCoparerHelper.SetRibbonDiffXmlIntellisenseContextEntityName(ribbonDiffXml, string.Empty);
                    }

                    ribbonDiffXml = ContentCoparerHelper.FormatXml(ribbonDiffXml, _commonConfig.ExportRibbonXmlXmlAttributeOnNewLine);

                    {
                        string fileName = EntityFileNameFormatter.GetApplicationRibbonDiffXmlFileName(service.ConnectionData.Name, "BackUp", "xml");
                        string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                        File.WriteAllText(filePath, ribbonDiffXml, new UTF8Encoding(false));

                        this._iWriteToOutput.WriteToOutput("Application RibbonDiffXml BackUp exported to {0}", filePath);

                        this._iWriteToOutput.WriteToOutputFilePathUri(filePath);
                    }

                    solutionBodyBinary = ExportSolutionHelper.ReplaceApplicationRibbonDiffXmlInSolutionBody(solutionBodyBinary, doc.Root);

                    UpdateStatus(Properties.WindowStatusStrings.ImportingSolutionFormat1, solutionUniqueName);

                    {
                        string fileName = string.Format("{0}.{1}_{2} Changed Solution Backup at {3}.zip"
                            , service.ConnectionData.Name
                            , solution.UniqueName
                            , solution.Version
                            , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss")
                            );

                        string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                        File.WriteAllBytes(filePath, solutionBodyBinary);

                        this._iWriteToOutput.WriteToOutput("Changed Solution {0} Backup exported to {1}", solution.UniqueName, filePath);

                        this._iWriteToOutput.WriteToOutputFilePathUri(filePath);
                    }

                    await repository.ImportSolutionAsync(solutionBodyBinary);

                    UpdateStatus(Properties.WindowStatusStrings.DeletingSolutionFormat1, solutionUniqueName);

                    await service.DeleteAsync(solution.LogicalName, solution.Id);

                    UpdateStatus(Properties.WindowStatusStrings.PublishingApplicationRibbon);

                    {
                        var repositoryPublish = new PublishActionsRepository(service);

                        await repositoryPublish.PublishApplicationRibbonAsync();
                    }
                }

                ToggleControls(true, Properties.WindowStatusStrings.UpdatingApplicationRibbonDiffXmlCompleted);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.UpdatingApplicationRibbonDiffXmlFailed);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.UpdatingApplicationRibbonDiffXml);
        }

        private async Task ExecuteActionOnEntityAsync(EntityMetadataListViewItem entityName, Func<EntityMetadataListViewItem, Task> action)
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

            await action(entityName);
        }

        private async Task ExecuteActionOnApplicationRibbonAsync(Func<Task> action)
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

            await action();
        }

        private async void btnPublishApplicationRibbon_Click(object sender, RoutedEventArgs e)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.PublishingApplicationRibbon);

            ToggleControls(false, Properties.WindowStatusStrings.PublishingApplicationRibbon);

            try
            {
                var service = await GetService();

                var repository = new PublishActionsRepository(service);

                await repository.PublishApplicationRibbonAsync();

                ToggleControls(true, Properties.WindowStatusStrings.PublishingApplicationRibbonCompleted);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.PublishingApplicationRibbonFailed);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.PublishingApplicationRibbon);
        }

        private void btnExportEntityRibbon_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionOnEntityAsync(entity, PerformExportEntityRibbon);
        }

        private void btnExportEntityRibbonDiffXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionOnEntityAsync(entity, PerformExportEntityRibbonDiffXml);
        }

        private async Task PerformExportEntityRibbon(EntityMetadataListViewItem entity)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.ExportingRibbonForEntityFormat1, entity.EntityLogicalName);

            var service = await GetService();

            string fileName = EntityFileNameFormatter.GetEntityRibbonFileName(service.ConnectionData.Name, entity.EntityLogicalName);
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            try
            {
                RibbonLocationFilters filter = GetRibbonLocationFilters();

                var repository = new RibbonCustomizationRepository(service);

                await repository.ExportEntityRibbonAsync(entity.EntityLogicalName, filter, filePath, _commonConfig);

                this._iWriteToOutput.WriteToOutput("{0} Ribbon exported to {1}", entity.EntityLogicalName, filePath);

                this._iWriteToOutput.PerformAction(filePath, _commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }

            ToggleControls(true, Properties.WindowStatusStrings.ExportingRibbonForEntityCompletedFormat1, entity.EntityLogicalName);
        }

        private async Task PerformExportEntityRibbonDiffXml(EntityMetadataListViewItem entity)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.ExportingRibbonDiffXmlForEntityFormat1, entity.EntityLogicalName);

            ToggleControls(false, Properties.WindowStatusStrings.ExportingRibbonDiffXmlForEntityFormat1, entity.EntityLogicalName);

            var service = await GetService();

            try
            {
                var repositoryPublisher = new PublisherRepository(service);

                var publisherDefault = await repositoryPublisher.GetDefaultPublisherAsync();

                if (publisherDefault != null)
                {
                    var solutionUniqueName = string.Format("RibbonDiffXml_{0}", Guid.NewGuid());

                    solutionUniqueName = solutionUniqueName.Replace("-", "_");

                    var solution = new Solution()
                    {
                        UniqueName = solutionUniqueName,
                        FriendlyName = solutionUniqueName,

                        Description = "Temporary solution for exporting RibbonDiffXml.",

                        PublisherId = publisherDefault.ToEntityReference(),

                        Version = "1.0.0.0",
                    };

                    UpdateStatus(Properties.WindowStatusStrings.CreatingNewSolutionFormat1, solutionUniqueName);

                    solution.Id = service.Create(solution);

                    UpdateStatus(Properties.WindowStatusStrings.AddingInSolutionEntityFormat2, solutionUniqueName, entity.EntityLogicalName);

                    {
                        var repositorySolutionComponent = new SolutionComponentRepository(service);

                        await repositorySolutionComponent.AddSolutionComponentsAsync(solutionUniqueName, new[] { new SolutionComponent()
                        {
                            ComponentType = new OptionSetValue((int)ComponentType.Entity),
                            ObjectId = entity.EntityMetadata.MetadataId.Value,
                            RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                        }});
                    }

                    UpdateStatus(Properties.WindowStatusStrings.ExportingSolutionAndExtractingRibbonDiffXmlForEntityFormat2, solutionUniqueName, entity.EntityLogicalName);

                    var repository = new ExportSolutionHelper(service);

                    string ribbonDiffXml = await repository.ExportSolutionAndGetRibbonDiffAsync(solutionUniqueName, entity.EntityLogicalName);

                    if (_commonConfig.SetXmlSchemasDuringExport)
                    {
                        var schemasResources = CommonExportXsdSchemasCommand.ListXsdSchemas.FirstOrDefault(e => string.Equals(e.Item1, "RibbonXml", StringComparison.InvariantCultureIgnoreCase));

                        if (schemasResources != null)
                        {
                            string schemas = ContentCoparerHelper.HandleExportXsdSchemaIntoSchamasFolder(schemasResources.Item2);

                            if (!string.IsNullOrEmpty(schemas))
                            {
                                ribbonDiffXml = ContentCoparerHelper.ReplaceXsdSchema(ribbonDiffXml, schemas);
                            }
                        }
                    }

                    if (_commonConfig.SetIntellisenseContext)
                    {
                        ribbonDiffXml = ContentCoparerHelper.SetRibbonDiffXmlIntellisenseContextEntityName(ribbonDiffXml, entity.EntityLogicalName);
                    }

                    ribbonDiffXml = ContentCoparerHelper.FormatXml(ribbonDiffXml, _commonConfig.ExportRibbonXmlXmlAttributeOnNewLine);

                    {
                        string fileName = EntityFileNameFormatter.GetEntityRibbonDiffXmlFileName(service.ConnectionData.Name, entity.EntityLogicalName);
                        string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                        File.WriteAllText(filePath, ribbonDiffXml, new UTF8Encoding(false));

                        this._iWriteToOutput.WriteToOutput("{0} RibbonDiff Xml exported to {1}", entity.EntityLogicalName, filePath);

                        this._iWriteToOutput.PerformAction(filePath, _commonConfig);
                    }

                    UpdateStatus(Properties.WindowStatusStrings.DeletingSolutionFormat1, solutionUniqueName);

                    await service.DeleteAsync(solution.LogicalName, solution.Id);
                }

                ToggleControls(true, Properties.WindowStatusStrings.ExportingRibbonDiffXmlForEntityCompletedFormat1, entity.EntityLogicalName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.ExportingRibbonDiffXmlForEntityFailedFormat1, entity.EntityLogicalName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.ExportingRibbonDiffXmlForEntityFormat1, entity.EntityLogicalName);
        }

        private void mIUpdateEntityRibbonDiffXml_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ExecuteActionOnEntityAsync(entity, PerformUpdateEntityRibbonDiffXml);
        }

        private async Task PerformUpdateEntityRibbonDiffXml(EntityMetadataListViewItem entity)
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            var service = await GetService();

            var repositoryPublisher = new PublisherRepository(service);
            var publisherDefault = await repositoryPublisher.GetDefaultPublisherAsync();

            if (publisherDefault == null)
            {
                ToggleControls(true, Properties.WindowStatusStrings.NotFoundedDefaultPublisher);
                _iWriteToOutput.ActivateOutputWindow();
                return;
            }

            var newText = string.Empty;
            bool? dialogResult = false;

            var title = string.Format("RibbonDiffXml for entity {0}", entity.EntityLogicalName);

            this.Dispatcher.Invoke(() =>
            {
                var form = new WindowTextField("Enter " + title, title, string.Empty);

                dialogResult = form.ShowDialog();

                newText = form.FieldText;
            });

            if (dialogResult.GetValueOrDefault() == false)
            {
                ToggleControls(true, Properties.WindowStatusStrings.UpdatingRibbonDiffXmlForEntityCanceledFormat1, entity.EntityLogicalName);
                _iWriteToOutput.ActivateOutputWindow();
                return;
            }

            ContentCoparerHelper.ClearXsdSchema(newText, out newText);

            UpdateStatus(Properties.WindowStatusStrings.ValidatingRibbonDiffXmlForEntityFormat1, entity.EntityLogicalName);

            if (!ContentCoparerHelper.TryParseXmlDocument(newText, out var doc))
            {
                ToggleControls(true, Properties.WindowStatusStrings.TextIsNotValidXml);
                _iWriteToOutput.ActivateOutputWindow();
                return;
            }

            bool validateResult = await ValidateXmlDocumentAsync(doc);

            if (!validateResult)
            {
                ToggleControls(true, Properties.WindowStatusStrings.ValidatingRibbonDiffXmlForEntityFailedFormat1, entity.EntityLogicalName);
                _iWriteToOutput.ActivateOutputWindow();
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.UpdatingRibbonDiffXmlForEntityFormat1, entity.EntityLogicalName);

            var solutionUniqueName = string.Format("RibbonDiffXml_{0}", Guid.NewGuid());

            solutionUniqueName = solutionUniqueName.Replace("-", "_");

            var solution = new Solution()
            {
                UniqueName = solutionUniqueName,
                FriendlyName = solutionUniqueName,

                Description = "Temporary solution for exporting RibbonDiffXml.",

                PublisherId = publisherDefault.ToEntityReference(),

                Version = "1.0.0.0",
            };

            UpdateStatus(Properties.WindowStatusStrings.CreatingNewSolutionFormat1, solutionUniqueName);

            solution.Id = service.Create(solution);

            UpdateStatus(Properties.WindowStatusStrings.AddingInSolutionEntityFormat2, solutionUniqueName, entity.EntityLogicalName);

            string finalStatus = string.Empty;

            try
            {
                {
                    var repositorySolutionComponent = new SolutionComponentRepository(service);

                    await repositorySolutionComponent.AddSolutionComponentsAsync(solutionUniqueName, new[] { new SolutionComponent()
                        {
                            ComponentType = new OptionSetValue((int)ComponentType.Entity),
                            ObjectId = entity.EntityMetadata.MetadataId.Value,
                            RootComponentBehavior = new OptionSetValue((int)RootComponentBehavior.IncludeSubcomponents),
                        }});
                }

                UpdateStatus(Properties.WindowStatusStrings.ExportingSolutionAndExtractingRibbonDiffXmlForEntityFormat2, solutionUniqueName, entity.EntityLogicalName);

                var repository = new ExportSolutionHelper(service);

                var solutionBodyBinary = await repository.ExportSolutionAndGetBodyBinaryAsync(solutionUniqueName);

                string ribbonDiffXml = ExportSolutionHelper.GetRibbonDiffXmlForEntityFromSolutionBody(entity.EntityLogicalName, solutionBodyBinary);

                {
                    string fileName = string.Format("{0}.{1}_{2} Solution Backup at {3}.zip"
                        , service.ConnectionData.Name
                        , solution.UniqueName
                        , solution.Version
                        , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss")
                        );

                    string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    File.WriteAllBytes(filePath, solutionBodyBinary);

                    this._iWriteToOutput.WriteToOutput("Solution {0} Backup exported to {1}", solution.UniqueName, filePath);

                    this._iWriteToOutput.WriteToOutputFilePathUri(filePath);
                }

                if (_commonConfig.SetXmlSchemasDuringExport)
                {
                    var schemasResources = CommonExportXsdSchemasCommand.ListXsdSchemas.FirstOrDefault(e => string.Equals(e.Item1, "RibbonXml", StringComparison.InvariantCultureIgnoreCase));

                    if (schemasResources != null)
                    {
                        string schemas = ContentCoparerHelper.HandleExportXsdSchemaIntoSchamasFolder(schemasResources.Item2);

                        if (!string.IsNullOrEmpty(schemas))
                        {
                            ribbonDiffXml = ContentCoparerHelper.ReplaceXsdSchema(ribbonDiffXml, schemas);
                        }
                    }
                }

                if (_commonConfig.SetIntellisenseContext)
                {
                    ribbonDiffXml = ContentCoparerHelper.SetRibbonDiffXmlIntellisenseContextEntityName(ribbonDiffXml, entity.EntityLogicalName);
                }

                ribbonDiffXml = ContentCoparerHelper.FormatXml(ribbonDiffXml, _commonConfig.ExportRibbonXmlXmlAttributeOnNewLine);

                {
                    string fileName = EntityFileNameFormatter.GetEntityRibbonDiffXmlFileName(service.ConnectionData.Name, entity.EntityLogicalName, "BackUp", "xml");
                    string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    File.WriteAllText(filePath, ribbonDiffXml, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput("{0} RibbonDiffXml BackUp exported to {0}", entity.EntityLogicalName, filePath);

                    this._iWriteToOutput.WriteToOutputFilePathUri(filePath);
                }

                solutionBodyBinary = ExportSolutionHelper.ReplaceRibbonDiffXmlForEntityInSolutionBody(entity.EntityLogicalName, solutionBodyBinary, doc.Root);

                {
                    string fileName = string.Format("{0}.{1}_{2} Changed Solution Backup at {3}.zip"
                        , service.ConnectionData.Name
                        , solution.UniqueName
                        , solution.Version
                        , DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss")
                        );

                    string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    File.WriteAllBytes(filePath, solutionBodyBinary);

                    this._iWriteToOutput.WriteToOutput("Changed Solution {0} Backup exported to {1}", solution.UniqueName, filePath);

                    this._iWriteToOutput.WriteToOutputFilePathUri(filePath);
                }

                UpdateStatus(Properties.WindowStatusStrings.ImportingSolutionFormat1, solutionUniqueName);

                await repository.ImportSolutionAsync(solutionBodyBinary);

                UpdateStatus(Properties.WindowStatusStrings.PublishingEntitiesFormat1, entity.EntityLogicalName);

                {
                    var repositoryPublish = new PublishActionsRepository(service);

                    await repositoryPublish.PublishEntitiesAsync(new[] { entity.EntityLogicalName });
                }

                finalStatus = string.Format(Properties.WindowStatusStrings.UpdatingRibbonDiffXmlForEntityCompletedFormat1, entity.EntityLogicalName);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                finalStatus = string.Format(Properties.WindowStatusStrings.UpdatingRibbonDiffXmlForEntityFailedFormat1, entity.EntityLogicalName);

                _iWriteToOutput.ActivateOutputWindow();
            }
            finally
            {
                UpdateStatus(Properties.WindowStatusStrings.DeletingSolutionFormat1, solutionUniqueName);
                await service.DeleteAsync(solution.LogicalName, solution.Id);
            }

            ToggleControls(true, finalStatus);
        }

        private Task<bool> ValidateXmlDocumentAsync(XDocument doc)
        {
            return Task.Run(() => ValidateXmlDocument(doc));
        }

        private bool ValidateXmlDocument(XDocument doc)
        {
            XmlSchemaSet schemas = new XmlSchemaSet();

            {
                var schemasResources = CommonExportXsdSchemasCommand.ListXsdSchemas.FirstOrDefault(e => string.Equals(e.Item1, "RibbonXml", StringComparison.InvariantCultureIgnoreCase));

                if (schemasResources != null)
                {
                    foreach (var fileName in schemasResources.Item2)
                    {
                        Uri uri = FileOperations.GetSchemaResourceUri(fileName);
                        StreamResourceInfo info = Application.GetResourceStream(uri);

                        using (StreamReader reader = new StreamReader(info.Stream))
                        {
                            schemas.Add("", XmlReader.Create(reader));
                        }
                    }
                }
            }

            List<ValidationEventArgs> errors = new List<ValidationEventArgs>();

            doc.Validate(schemas, (o, e) =>
            {
                errors.Add(e);
            });

            if (errors.Count > 0)
            {
                _iWriteToOutput.WriteToOutput("RibbonDiff is not valid.");

                foreach (var item in errors)
                {
                    _iWriteToOutput.WriteToOutput(string.Empty);
                    _iWriteToOutput.WriteToOutput(string.Empty);
                    _iWriteToOutput.WriteToOutput("Severity: {0}      Message: {1}", item.Severity, item.Message);
                    _iWriteToOutput.WriteErrorToOutput(item.Exception);
                }

                _iWriteToOutput.ActivateOutputWindow();
            }

            return errors.Count == 0;
        }

        private void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var entity = ((FrameworkElement)e.OriginalSource).DataContext as EntityMetadataListViewItem;

                if (entity != null)
                {
                    ExecuteActionOnEntityAsync(entity, PerformExportEntityRibbon);
                }
            }
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingEntities();
            }

            base.OnKeyDown(e);
        }

        private void mIOpenEntityInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenEntityMetadataInWeb(entity.EntityMetadata.MetadataId.Value);
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
                connectionData.OpenEntityListInWeb(entity.EntityLogicalName);
            }
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
                connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.Entity, entity.EntityMetadata.MetadataId.Value);
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

                await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.Entity, new[] { entity.EntityMetadata.MetadataId.Value }, null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (sender is ContextMenu contextMenu)
            {
                var items = contextMenu.Items.OfType<Control>();

                ConnectionData connectionData = null;

                cmBCurrentConnection.Dispatcher.Invoke(() =>
                {
                    connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
                });

                FillLastSolutionItems(connectionData, items, true, AddIntoCrmSolutionLast_Click, "contMnAddIntoSolutionLast");
            }
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
                , (int)ComponentType.Entity
                , entity.EntityMetadata.MetadataId.Value
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
                , (int)ComponentType.Entity
                , entity.EntityMetadata.MetadataId.Value
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
                ShowExistingEntities();
            }
        }

        private async void btnPublishEntity_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var entityName = entity.EntityLogicalName;

            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.PublishingEntitiesFormat1, entityName);

            ToggleControls(false, Properties.WindowStatusStrings.PublishingEntitiesFormat1, entityName);

            try
            {
                var service = await GetService();

                var repository = new PublishActionsRepository(service);

                await repository.PublishEntitiesAsync(new[] { entityName });

                ToggleControls(true, Properties.WindowStatusStrings.PublishingEntitiesCompletedFormat1, entityName);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.PublishingEntitiesFailedFormat1, entityName);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.PublishingEntitiesFormat1, entityName);
        }

        private void mIApplicationRibbon_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            FillLastSolutionItems(connectionData, new[] { mIAddApplicationIntoLastSolution }, true, AddApplicationRibbonIntoCrmSolutionLast_Click, "mIAddApplicationIntoLastSolution");
        }

        private async void mIApplicationRibbonOpenSolutionsContainingComponentInWindow_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            var repository = new RibbonCustomizationRepository(service);

            var ribbonCustomization = await repository.FindApplicationRibbonCustomizationAsync();

            if (ribbonCustomization != null)
            {
                _commonConfig.Save();

                WindowHelper.OpenExplorerSolutionWindow(
                    _iWriteToOutput
                    , service
                    , _commonConfig
                    , (int)ComponentType.RibbonCustomization
                    , ribbonCustomization.Id
                );
            }
        }

        private async void mIApplicationRibbonOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            var repository = new RibbonCustomizationRepository(service);

            var ribbonCustomization = await repository.FindApplicationRibbonCustomizationAsync();

            if (ribbonCustomization != null)
            {
                service.ConnectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.RibbonCustomization, ribbonCustomization.Id);
            }
        }

        private async void mIApplicationRibbonOpenDependentComponentsInWindow_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            var repository = new RibbonCustomizationRepository(service);

            var ribbonCustomization = await repository.FindApplicationRibbonCustomizationAsync();

            if (ribbonCustomization != null)
            {
                var descriptor = await GetDescriptor();

                WindowHelper.OpenSolutionComponentDependenciesWindow(
                    _iWriteToOutput
                    , service
                    , descriptor
                    , _commonConfig
                    , (int)ComponentType.RibbonCustomization
                    , ribbonCustomization.Id
                    , null
                    );
            }
        }

        private async void AddApplicationRibbonIntoCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddApplicationRibbonIntoSolution(true, null);
        }

        private async void AddApplicationRibbonIntoCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                 && menuItem.Tag != null
                 && menuItem.Tag is string solutionUniqueName
                 )
            {
                await AddApplicationRibbonIntoSolution(false, solutionUniqueName);
            }
        }

        private async Task AddApplicationRibbonIntoSolution(bool withSelect, string solutionUniqueName)
        {
            var service = await GetService();

            var repository = new RibbonCustomizationRepository(service);

            var ribbonCustomization = await repository.FindApplicationRibbonCustomizationAsync();

            if (ribbonCustomization != null)
            {
                _commonConfig.Save();

                var descriptor = await GetDescriptor();

                try
                {
                    this._iWriteToOutput.ActivateOutputWindow();

                    await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.RibbonCustomization, new[] { ribbonCustomization.Id }, null, withSelect);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            }
        }
    }
}