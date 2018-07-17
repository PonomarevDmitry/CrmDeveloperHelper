using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

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
            _descriptorCache[service.ConnectionData.ConnectionId] = new SolutionComponentDescriptor(_iWriteToOutput, service, true);

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
                    _iWriteToOutput.WriteToOutput("Connection to CRM.");
                    _iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());
                    var service = await QuickConnection.ConnectAsync(connectionData);
                    _iWriteToOutput.WriteToOutput("Current Service Endpoint: {0}", service.CurrentServiceEndpoint);

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

                    _descriptorCache[connectionData.ConnectionId] = new SolutionComponentDescriptor(_iWriteToOutput, service, true);
                }

                return _descriptorCache[connectionData.ConnectionId];
            }

            return null;
        }

        private async Task ShowExistingEntities()
        {
            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false);

            UpdateStatus("Loading entities...");

            this._itemsSource.Clear();

            IEnumerable<EntityMetadata> list = Enumerable.Empty<EntityMetadata>();

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

            list = FilterList(list, textName);

            LoadEntities(list);
        }

        private static IEnumerable<EntityMetadata> FilterList(IEnumerable<EntityMetadata> list, string textName)
        {
            list = list.Where(e => !e.IsIntersect.GetValueOrDefault());

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
            this._iWriteToOutput.WriteToOutput("Found {0} entity(ies)", results.Count());

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

            UpdateStatus(string.Format("{0} entities loaded", results.Count()));

            ToggleControls(true);
        }

        private void UpdateStatus(string msg)
        {
            this.statusBar.Dispatcher.Invoke(() =>
            {
                this.tSSLStatusMessage.Content = msg;
            });
        }

        private void ToggleControls(bool enabled)
        {
            this._controlsEnabled = enabled;

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
            EntityMetadataListViewItem result = null;

            if (this.lstVwEntities.SelectedItems.Count == 1
                && this.lstVwEntities.SelectedItems[0] != null
                && this.lstVwEntities.SelectedItems[0] is EntityMetadataListViewItem
                )
            {
                result = (this.lstVwEntities.SelectedItems[0] as EntityMetadataListViewItem);
            }

            return result;
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

        private async void btnGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService();
            var descriptor = await GetDescriptor();

            var entityMetadata = descriptor.GetEntityMetadata(entity.EntityMetadata.MetadataId.Value);

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

            WindowHelper.OpenPluginTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName);
        }

        private async void btnMessageTree_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName);
        }

        private async void btnMessageRequestTree_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageRequestTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.EntityLogicalName);
        }

        private async void btnOrganizationComparer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerWindow(this._iWriteToOutput, service.ConnectionData.ConnectionConfiguration, _commonConfig);
        }

        private async void btnCompareMetadataFile_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerEntityMetadataWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }

        private async void btnCompareRibbon_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerRibbonWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }

        private async void btnCompareGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerGlobalOptionSetsWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
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
            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false);

            var service = await GetService();

            string fileName = EntityFileNameFormatter.GetApplicationRibbonFileName(service.ConnectionData.Name);
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            try
            {
                RibbonLocationFilters filter = GetRibbonLocationFilters();

                var repository = new RibbonCustomizationRepository(service);

                await repository.ExportApplicationRibbon(filter, filePath, _commonConfig);

                this._iWriteToOutput.WriteToOutput("Application Ribbon Xml exported to {0}", filePath);

                this._iWriteToOutput.PerformAction(filePath, _commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }

            ToggleControls(true);
        }

        private void btnExportApplicationRibbonDiffXml_Click(object sender, RoutedEventArgs e)
        {
            ExecuteActionOnApplicationRibbonAsync(PerformExportApplicationRibbonDiffXml);
        }

        private async Task PerformExportApplicationRibbonDiffXml()
        {
            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false);

            var service = await GetService();

            try
            {
                var repositoryPublisher = new PublisherRepository(service);

                var publisherDefault = await repositoryPublisher.GetDefaultPublisherAsync();

                var repositoryRibbonCustomization = new RibbonCustomizationRepository(service);

                var ribbonCustomization = await repositoryRibbonCustomization.FindApplicationRibbonCustomizationAsync();

                if (publisherDefault != null && ribbonCustomization != null)
                {
                    var uniqueName = string.Format("RibbonDiffXml_{0}", Guid.NewGuid());

                    uniqueName = uniqueName.Replace("-", "_");

                    var solution = new Solution()
                    {
                        UniqueName = uniqueName,
                        FriendlyName = uniqueName,

                        Description = "Temporary solution for exporting RibbonDiffXml.",

                        PublisherId = publisherDefault.ToEntityReference(),

                        Version = "1.0.0.0",
                    };

                    _iWriteToOutput.WriteToOutput("Creating new solution {0}.", uniqueName);

                    UpdateStatus("Creating new solution...");

                    solution.Id = service.Create(solution);

                    _iWriteToOutput.WriteToOutput("Adding in solution {0} ApplicationRibbon.", uniqueName);

                    UpdateStatus("Adding in solution ApplicationRibbon...");

                    {
                        var repositorySolutionComponent = new SolutionComponentRepository(service);

                        await repositorySolutionComponent.AddSolutionComponentsAsync(uniqueName, new[] { new SolutionComponent()
                        {
                            ComponentType = new OptionSetValue((int)ComponentType.RibbonCustomization),
                            ObjectId = ribbonCustomization.Id,
                        }});
                    }

                    _iWriteToOutput.WriteToOutput("Exporting solution {0} and extracting ApplicationRibbonDiffXml.", uniqueName);

                    UpdateStatus("Exporting solution and extracting ApplicationRibbonDiffXml...");

                    var repository = new ExportSolutionHelper(service);

                    string ribbonDiffXml = await repository.ExportSolutionAndGetApplicationRibbonDiffAsync(uniqueName);

                    ribbonDiffXml = ContentCoparerHelper.FormatXml(ribbonDiffXml, _commonConfig.ExportRibbonXmlXmlAttributeOnNewLine);

                    string fileName = EntityFileNameFormatter.GetApplicationRibbonDiffXmlFileName(service.ConnectionData.Name);
                    string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    File.WriteAllText(filePath, ribbonDiffXml);

                    _iWriteToOutput.WriteToOutput("Deleting solution {0}.", uniqueName);

                    UpdateStatus("Deleting solution...");

                    service.Delete(solution.LogicalName, solution.Id);

                    this._iWriteToOutput.WriteToOutput("ApplicationRibbonDiffXml Xml exported to {0}", filePath);

                    this._iWriteToOutput.PerformAction(filePath, _commonConfig);

                    UpdateStatus("Export ApplicationRibbonDiffXml completed.");
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                UpdateStatus("Export ApplicationRibbonDiffXml failed.");
            }

            ToggleControls(true);
        }

        private async Task ExecuteActionOnEntityAsync(EntityMetadataListViewItem entityName, Func<EntityMetadataListViewItem, Task> action)
        {
            string folder = txtBFolder.Text.Trim();

            if (!_controlsEnabled)
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

            if (!_controlsEnabled)
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
            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false);

            UpdateStatus("Publishing Application Ribbon...");

            this._iWriteToOutput.WriteToOutput("Start publishing Application Ribbon at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            try
            {
                var service = await GetService();

                var repository = new PublishActionsRepository(service);

                await repository.PublishApplicationRibbonAsync();

                this._iWriteToOutput.WriteToOutput("End publishing Application Ribbon at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

                UpdateStatus("Application Ribbon published.");
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                UpdateStatus("Publish Application Ribbon failed.");
            }
            finally
            {
                ToggleControls(true);
            }
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
            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false);

            var service = await GetService();

            string fileName = EntityFileNameFormatter.GetEntityRibbonFileName(service.ConnectionData.Name, entity.EntityLogicalName);
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            try
            {
                RibbonLocationFilters filter = GetRibbonLocationFilters();

                var repository = new RibbonCustomizationRepository(service);

                await repository.ExportEntityRibbon(entity.EntityLogicalName, filter, filePath, _commonConfig);

                this._iWriteToOutput.WriteToOutput("Ribbon Xml exported to {0}", filePath);

                this._iWriteToOutput.PerformAction(filePath, _commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }

            ToggleControls(true);
        }

        private async Task PerformExportEntityRibbonDiffXml(EntityMetadataListViewItem entity)
        {
            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false);

            var service = await GetService();

            try
            {
                var repositoryPublisher = new PublisherRepository(service);

                var publisherDefault = await repositoryPublisher.GetDefaultPublisherAsync();

                if (publisherDefault != null)
                {
                    var uniqueName = string.Format("RibbonDiffXml_{0}", Guid.NewGuid());

                    uniqueName = uniqueName.Replace("-", "_");

                    var solution = new Solution()
                    {
                        UniqueName = uniqueName,
                        FriendlyName = uniqueName,

                        Description = "Temporary solution for exporting RibbonDiffXml.",

                        PublisherId = publisherDefault.ToEntityReference(),

                        Version = "1.0.0.0",
                    };

                    _iWriteToOutput.WriteToOutput("Creating new solution {0}.", uniqueName);

                    UpdateStatus("Creating new solution...");

                    solution.Id = service.Create(solution);

                    _iWriteToOutput.WriteToOutput("Adding in solution {0} entity {1}.", uniqueName, entity.EntityLogicalName);

                    UpdateStatus(string.Format("Adding in solution entity {0}...", entity.EntityLogicalName));

                    {
                        var repositorySolutionComponent = new SolutionComponentRepository(service);

                        await repositorySolutionComponent.AddSolutionComponentsAsync(uniqueName, new[] { new SolutionComponent()
                        {
                            ComponentType = new OptionSetValue((int)ComponentType.Entity),
                            ObjectId = entity.EntityMetadata.MetadataId.Value,
                        }});
                    }

                    _iWriteToOutput.WriteToOutput("Exporting solution {0} and extracting RibbonDiffXml for {1}.", uniqueName, entity.EntityLogicalName);

                    UpdateStatus(string.Format("Exporting solution and extracting RibbonDiffXml for {0}...", entity.EntityLogicalName));

                    var repository = new ExportSolutionHelper(service);

                    string ribbonDiffXml = await repository.ExportSolutionAndGetRibbonDiffAsync(uniqueName, entity.EntityLogicalName);

                    ribbonDiffXml = ContentCoparerHelper.FormatXml(ribbonDiffXml, _commonConfig.ExportRibbonXmlXmlAttributeOnNewLine);

                    string fileName = EntityFileNameFormatter.GetEntityRibbonDiffXmlFileName(service.ConnectionData.Name, entity.EntityLogicalName);
                    string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

                    File.WriteAllText(filePath, ribbonDiffXml);

                    _iWriteToOutput.WriteToOutput("Deleting solution {0}.", uniqueName);

                    UpdateStatus("Deleting solution...");

                    service.Delete(solution.LogicalName, solution.Id);

                    this._iWriteToOutput.WriteToOutput("RibbonDiff Xml exported to {0}", filePath);

                    this._iWriteToOutput.PerformAction(filePath, _commonConfig);

                    UpdateStatus("Export RibbonDiffXml completed.");
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);

                UpdateStatus("Export RibbonDiffXml failed.");
            }

            ToggleControls(true);
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

        private void AddIntoCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            AddIntoSolution(true, null);
        }

        private void AddIntoCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                AddIntoSolution(false, solutionUniqueName);
            }
        }

        private void AddIntoSolution(bool withSelect, string solutionUniqueName)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData != null)
            {
                _commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        this._iWriteToOutput.ActivateOutputWindow();

                        var contr = new SolutionController(this._iWriteToOutput);

                        contr.ExecuteAddingComponentesIntoSolution(connectionData, _commonConfig, solutionUniqueName, ComponentType.Entity, new[] { entity.EntityMetadata.MetadataId.Value }, null, withSelect);
                    }
                    catch (Exception ex)
                    {
                        this._iWriteToOutput.WriteErrorToOutput(ex);
                    }
                });

                backWorker.Start();
            }
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (sender is ContextMenu contextMenu)
            {
                var items = contextMenu.Items.OfType<MenuItem>();

                ConnectionData connectionData = null;

                cmBCurrentConnection.Dispatcher.Invoke(() =>
                {
                    connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
                });

                var lastSolution = items.FirstOrDefault(i => string.Equals(i.Uid, "contMnAddIntoSolutionLast", StringComparison.InvariantCultureIgnoreCase));

                if (lastSolution != null)
                {
                    lastSolution.Items.Clear();

                    lastSolution.IsEnabled = false;
                    lastSolution.Visibility = Visibility.Collapsed;

                    if (connectionData != null
                        && connectionData.LastSelectedSolutionsUniqueName != null
                        && connectionData.LastSelectedSolutionsUniqueName.Any()
                        )
                    {
                        lastSolution.IsEnabled = true;
                        lastSolution.Visibility = Visibility.Visible;

                        foreach (var uniqueName in connectionData.LastSelectedSolutionsUniqueName)
                        {
                            var menuItem = new MenuItem()
                            {
                                Header = uniqueName.Replace("_", "__"),
                                Tag = uniqueName,
                            };

                            menuItem.Click += AddIntoCrmSolutionLast_Click;

                            lastSolution.Items.Add(menuItem);
                        }
                    }
                }
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
            if (_init > 0)
            {
                return;
            }

            this.Dispatcher.Invoke(() =>
            {
                this._itemsSource.Clear();
            });

            if (!_controlsEnabled)
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

            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false);

            UpdateStatus(string.Format("Publishing Entity {0}...", entityName));

            this._iWriteToOutput.WriteToOutput("Start publishing entity {0} at {1}", entityName, DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            try
            {
                var service = await GetService();

                var repository = new PublishActionsRepository(service);

                await repository.PublishEntitiesAsync(new[] { entityName });

                this._iWriteToOutput.WriteToOutput("End publishing entity {0} at {1}", entityName, DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

                UpdateStatus(string.Format("Entity {0} published", entityName));
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                UpdateStatus(string.Format("Publish Entity {0} failed", entityName));
            }
            finally
            {
                ToggleControls(true);
            }
        }

        private void mIApplicationRibbon_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            mIAddApplicationIntoLastSolution.Items.Clear();

            mIAddApplicationIntoLastSolution.IsEnabled = false;
            mIAddApplicationIntoLastSolution.Visibility = Visibility.Collapsed;

            if (connectionData != null
                && connectionData.LastSelectedSolutionsUniqueName != null
                && connectionData.LastSelectedSolutionsUniqueName.Any()
                )
            {
                mIAddApplicationIntoLastSolution.IsEnabled = true;
                mIAddApplicationIntoLastSolution.Visibility = Visibility.Visible;

                foreach (var uniqueName in connectionData.LastSelectedSolutionsUniqueName)
                {
                    var menuItem = new MenuItem()
                    {
                        Header = uniqueName.Replace("_", "__"),
                        Tag = uniqueName,
                    };

                    menuItem.Click += AddApplicationRibbonIntoCrmSolutionLast_Click;

                    mIAddApplicationIntoLastSolution.Items.Add(menuItem);
                }
            }
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

        private void AddApplicationRibbonIntoCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            AddApplicationRibbonIntoSolution(true, null);
        }

        private void AddApplicationRibbonIntoCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                 && menuItem.Tag != null
                 && menuItem.Tag is string solutionUniqueName
                 )
            {
                AddApplicationRibbonIntoSolution(false, solutionUniqueName);
            }
        }

        private async void AddApplicationRibbonIntoSolution(bool withSelect, string solutionUniqueName)
        {
            var service = await GetService();

            var repository = new RibbonCustomizationRepository(service);

            var ribbonCustomization = await repository.FindApplicationRibbonCustomizationAsync();

            if (ribbonCustomization != null)
            {
                _commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        this._iWriteToOutput.ActivateOutputWindow();

                        var contr = new SolutionController(this._iWriteToOutput);

                        contr.ExecuteAddingComponentesIntoSolution(service.ConnectionData, _commonConfig, solutionUniqueName, ComponentType.RibbonCustomization, new[] { ribbonCustomization.Id }, null, withSelect);
                    }
                    catch (Exception ex)
                    {
                        this._iWriteToOutput.WriteErrorToOutput(ex);
                    }
                });

                backWorker.Start();
            }
        }
    }
}