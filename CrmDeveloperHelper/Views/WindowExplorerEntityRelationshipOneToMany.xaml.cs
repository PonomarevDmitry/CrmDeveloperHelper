using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExplorerEntityRelationshipOneToMany : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private readonly IWriteToOutput _iWriteToOutput;

        private readonly CommonConfiguration _commonConfig;

        private readonly ObservableCollection<EntityMetadataViewItem> _itemsSourceEntityList;

        private readonly ObservableCollection<OneToManyRelationshipMetadataViewItem> _itemsSourceEntityRelationshipList;

        private readonly Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();

        private readonly Dictionary<Guid, IEnumerable<EntityMetadata>> _cacheEntityMetadata = new Dictionary<Guid, IEnumerable<EntityMetadata>>();

        private readonly Dictionary<Guid, ConcurrentDictionary<string, Task>> _cacheMetadataTask = new Dictionary<Guid, ConcurrentDictionary<string, Task>>();

        private readonly Dictionary<Guid, ConcurrentDictionary<string, IEnumerable<OneToManyRelationshipMetadataViewItem>>> _cacheEntityOneToMany = new Dictionary<Guid, ConcurrentDictionary<string, IEnumerable<OneToManyRelationshipMetadataViewItem>>>();

        private readonly Dictionary<Guid, ConcurrentDictionary<string, IEnumerable<OneToManyRelationshipMetadataViewItem>>> _cacheEntityManyToOne = new Dictionary<Guid, ConcurrentDictionary<string, IEnumerable<OneToManyRelationshipMetadataViewItem>>>();

        public WindowExplorerEntityRelationshipOneToMany(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntity
        )
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;

            _connectionCache[service.ConnectionData.ConnectionId] = service;

            BindingOperations.EnableCollectionSynchronization(service.ConnectionData.ConnectionConfiguration.Connections, sysObjectConnections);

            InitializeComponent();

            FillRoleEditorLayoutTabs();

            LoadFromConfig();

            txtBFilterEnitity.Text = filterEntity;
            txtBFilterEnitity.SelectionLength = 0;
            txtBFilterEnitity.SelectionStart = txtBFilterEnitity.Text.Length;

            txtBFilterEnitity.Focus();

            _itemsSourceEntityList = new ObservableCollection<EntityMetadataViewItem>();
            lstVwEntities.ItemsSource = _itemsSourceEntityList;

            _itemsSourceEntityRelationshipList = new ObservableCollection<OneToManyRelationshipMetadataViewItem>();
            lstVwEntityRelationships.ItemsSource = _itemsSourceEntityRelationshipList;

            UpdateButtonsEnable();

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            this.DecreaseInit();

            ShowExistingEntities();
        }

        private void LoadFromConfig()
        {
            WindowSettings winConfig = GetWindowsSettings();

            LoadFormSettings(winConfig);
        }

        private void FillRoleEditorLayoutTabs()
        {
            cmBRoleEditorLayoutTabs.Items.Clear();

            cmBRoleEditorLayoutTabs.Items.Add("All");

            var tabs = RoleEditorLayoutTab.GetTabs();

            foreach (var tab in tabs)
            {
                cmBRoleEditorLayoutTabs.Items.Add(tab);
            }

            cmBRoleEditorLayoutTabs.SelectedIndex = 0;
        }

        protected override void LoadConfigurationInternal(WindowSettings winConfig)
        {
            base.LoadConfigurationInternal(winConfig);

            LoadFormSettings(winConfig);
        }

        private const string paramColumnEntityWidth = "ColumnEntityWidth";
        private const string paramRelationType = "RelationType";

        private void LoadFormSettings(WindowSettings winConfig)
        {
            if (winConfig.DictDouble.ContainsKey(paramColumnEntityWidth))
            {
                columnEntity.Width = new GridLength(winConfig.DictDouble[paramColumnEntityWidth]);
            }

            {
                var categoryValue = winConfig.GetValueInt(paramRelationType);

                if (categoryValue.HasValue && 0 <= categoryValue && categoryValue < cmBRelationType.Items.Count)
                {
                    cmBRelationType.SelectedIndex = categoryValue.Value;
                }
            }
        }

        protected override void SaveConfigurationInternal(WindowSettings winConfig)
        {
            base.SaveConfigurationInternal(winConfig);

            winConfig.DictDouble[paramColumnEntityWidth] = columnEntity.Width.Value;
            winConfig.DictInt[paramRelationType] = cmBRelationType.SelectedIndex;
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

        private async Task<IOrganizationServiceExtented> GetService()
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

            if (_connectionCache.ContainsKey(connectionData.ConnectionId))
            {
                return _connectionCache[connectionData.ConnectionId];
            }

            ToggleControls(connectionData, false, string.Empty);

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
                ToggleControls(connectionData, true, string.Empty);
            }

            return null;
        }

        private enum RelationType
        {
            All = 0,
            OneToMany = 1,
            ManyToOne = 2,
        }

        private RelationType GetRelationType()
        {
            var result = RelationType.All;

            cmBRelationType.Dispatcher.Invoke(() =>
            {
                if (cmBRelationType.SelectedIndex != -1)
                {
                    result = (RelationType)cmBRelationType.SelectedIndex;
                }
            });

            return result;
        }

        private async Task ShowExistingEntities()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.LoadingEntities);

            _itemsSourceEntityList.Clear();
            _itemsSourceEntityRelationshipList.Clear();

            IEnumerable<EntityMetadataViewItem> list = Enumerable.Empty<EntityMetadataViewItem>();

            try
            {
                if (service != null)
                {
                    if (!_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
                    {
                        EntityMetadataRepository repository = new EntityMetadataRepository(service);

                        var temp = await repository.GetEntitiesForEntityAttributeExplorerAsync(EntityFilters.Entity);

                        _cacheEntityMetadata.Add(service.ConnectionData.ConnectionId, temp);
                    }

                    list = _cacheEntityMetadata[service.ConnectionData.ConnectionId].Select(e => new EntityMetadataViewItem(e)).ToList();
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            string textName = string.Empty;
            RoleEditorLayoutTab selectedTab = null;

            this.Dispatcher.Invoke(() =>
            {
                textName = txtBFilterEnitity.Text.Trim().ToLower();
                selectedTab = cmBRoleEditorLayoutTabs.SelectedItem as RoleEditorLayoutTab;
            });

            list = FilterEntityList(list, textName, selectedTab);

            LoadEntities(list);

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.LoadingEntitiesCompletedFormat1, list.Count());

            ShowExistingEntityRelationships();
        }

        private static IEnumerable<EntityMetadataViewItem> FilterEntityList(IEnumerable<EntityMetadataViewItem> list, string textName, RoleEditorLayoutTab selectedTab)
        {
            if (selectedTab != null)
            {
                list = list.Where(ent => ent.EntityMetadata.ObjectTypeCode.HasValue && selectedTab.EntitiesHash.Contains(ent.EntityMetadata.ObjectTypeCode.Value));
            }

            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                if (int.TryParse(textName, out int tempInt))
                {
                    list = list.Where(ent => ent.EntityMetadata.ObjectTypeCode == tempInt);
                }
                else
                {
                    if (Guid.TryParse(textName, out Guid tempGuid))
                    {
                        list = list.Where(ent => ent.EntityMetadata.MetadataId == tempGuid);
                    }
                    else
                    {
                        list = list
                        .Where(ent =>
                            ent.LogicalName.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                            ||
                            (
                                ent.DisplayName != null
                                && ent.EntityMetadata.DisplayName.LocalizedLabels
                                    .Where(l => !string.IsNullOrEmpty(l.Label))
                                    .Any(lbl => lbl.Label.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1)
                            )

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

        private void LoadEntities(IEnumerable<EntityMetadataViewItem> results)
        {
            this.lstVwEntities.Dispatcher.Invoke(() =>
            {
                foreach (var entity in results.OrderBy(s => s.LogicalName))
                {
                    _itemsSourceEntityList.Add(entity);
                }

                if (this.lstVwEntities.Items.Count == 1)
                {
                    this.lstVwEntities.SelectedItem = this.lstVwEntities.Items[0];
                }
            });
        }

        private async Task ShowExistingEntityRelationships()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.LoadingOneToManyRelationships);

            string entityLogicalName = string.Empty;

            this.Dispatcher.Invoke(() =>
            {
                _itemsSourceEntityRelationshipList.Clear();

                entityLogicalName = GetSelectedEntity()?.LogicalName;
            });

            IEnumerable<OneToManyRelationshipMetadataViewItem> list = Enumerable.Empty<OneToManyRelationshipMetadataViewItem>();

            if (!string.IsNullOrEmpty(entityLogicalName))
            {
                try
                {
                    if (service != null)
                    {
                        if (!_cacheEntityOneToMany.ContainsKey(service.ConnectionData.ConnectionId))
                        {
                            _cacheEntityOneToMany.Add(service.ConnectionData.ConnectionId, new ConcurrentDictionary<string, IEnumerable<OneToManyRelationshipMetadataViewItem>>(StringComparer.InvariantCultureIgnoreCase));
                        }

                        if (!_cacheEntityManyToOne.ContainsKey(service.ConnectionData.ConnectionId))
                        {
                            _cacheEntityManyToOne.Add(service.ConnectionData.ConnectionId, new ConcurrentDictionary<string, IEnumerable<OneToManyRelationshipMetadataViewItem>>(StringComparer.InvariantCultureIgnoreCase));
                        }

                        if (!_cacheMetadataTask.ContainsKey(service.ConnectionData.ConnectionId))
                        {
                            _cacheMetadataTask.Add(service.ConnectionData.ConnectionId, new ConcurrentDictionary<string, Task>(StringComparer.InvariantCultureIgnoreCase));
                        }

                        var connectionEntityOneToMany = _cacheEntityOneToMany[service.ConnectionData.ConnectionId];
                        var connectionEntityManyToOne = _cacheEntityManyToOne[service.ConnectionData.ConnectionId];
                        var cacheMetadataTask = _cacheMetadataTask[service.ConnectionData.ConnectionId];

                        if (!connectionEntityOneToMany.ContainsKey(entityLogicalName)
                            || !connectionEntityManyToOne.ContainsKey(entityLogicalName)
                            )
                        {
                            if (cacheMetadataTask.ContainsKey(entityLogicalName))
                            {
                                if (cacheMetadataTask.TryGetValue(entityLogicalName, out var task))
                                {
                                    await task;
                                }
                            }
                            else
                            {
                                var task = GetEntityMetadataInformationAsync(service, entityLogicalName, connectionEntityOneToMany, connectionEntityManyToOne, cacheMetadataTask);

                                cacheMetadataTask.TryAdd(entityLogicalName, task);

                                await task;
                            }
                        }

                        HashSet<Guid> hashGuid = new HashSet<Guid>();
                        var coll = new List<OneToManyRelationshipMetadataViewItem>();

                        var relType = GetRelationType();

                        if (relType == RelationType.All || relType == RelationType.OneToMany)
                        {
                            foreach (var item in connectionEntityOneToMany[entityLogicalName])
                            {
                                if (hashGuid.Add(item.OneToManyRelationshipMetadata.MetadataId.Value))
                                {
                                    coll.Add(item);
                                }
                            }
                        }

                        if (relType == RelationType.All || relType == RelationType.ManyToOne)
                        {
                            foreach (var item in connectionEntityManyToOne[entityLogicalName])
                            {
                                if (hashGuid.Add(item.OneToManyRelationshipMetadata.MetadataId.Value))
                                {
                                    coll.Add(item);
                                }
                            }
                        }

                        list = coll;
                    }
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                }
            }

            string textName = string.Empty;

            txtBFilterEntityRelationship.Dispatcher.Invoke(() =>
            {
                textName = txtBFilterEntityRelationship.Text.Trim().ToLower();
            });

            list = FilterEntityRelationshipList(list, textName);

            LoadEntityRelationships(list);

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.LoadingOneToManyRelationshipsCompletedFormat1, list.Count());
        }

        private static async Task GetEntityMetadataInformationAsync(IOrganizationServiceExtented service
            , string entityLogicalName
            , ConcurrentDictionary<string, IEnumerable<OneToManyRelationshipMetadataViewItem>> connectionEntityOneToMany
            , ConcurrentDictionary<string, IEnumerable<OneToManyRelationshipMetadataViewItem>> connectionEntityManyToOne
            , ConcurrentDictionary<string, Task> cacheMetadataTask
        )
        {
            var repository = new EntityMetadataRepository(service);

            var metadata = await repository.GetEntityMetadataAttributesAsync(entityLogicalName, EntityFilters.Relationships);

            if (metadata != null)
            {
                if (metadata.OneToManyRelationships != null && !connectionEntityOneToMany.ContainsKey(entityLogicalName))
                {
                    connectionEntityOneToMany.TryAdd(entityLogicalName, metadata.OneToManyRelationships.Select(e => new OneToManyRelationshipMetadataViewItem(e)).ToList());
                }

                if (metadata.ManyToOneRelationships != null && !connectionEntityManyToOne.ContainsKey(entityLogicalName))
                {
                    connectionEntityManyToOne.TryAdd(entityLogicalName, metadata.ManyToOneRelationships.Select(e => new OneToManyRelationshipMetadataViewItem(e)).ToList());
                }
            }

            if (cacheMetadataTask.ContainsKey(entityLogicalName))
            {
                cacheMetadataTask.TryRemove(entityLogicalName, out _);
            }
        }

        private static IEnumerable<OneToManyRelationshipMetadataViewItem> FilterEntityRelationshipList(IEnumerable<OneToManyRelationshipMetadataViewItem> list, string textName)
        {
            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                if (Guid.TryParse(textName, out Guid tempGuid))
                {
                    list = list.Where(ent => ent.OneToManyRelationshipMetadata.MetadataId == tempGuid);
                }
                else
                {
                    list = list
                    .Where(ent =>
                        ent.SchemaName.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                        ||
                        (
                            ent.OneToManyRelationshipMetadata.AssociatedMenuConfiguration != null
                            && ent.OneToManyRelationshipMetadata.AssociatedMenuConfiguration.Label.LocalizedLabels
                                .Where(l => !string.IsNullOrEmpty(l.Label))
                                .Any(lbl => lbl.Label.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1)
                        )

                    //|| (ent.Description != null && ent.Description.LocalizedLabels
                    //    .Where(l => !string.IsNullOrEmpty(l.Label))
                    //    .Any(lbl => lbl.Label.ToLower().Contains(textName)))

                    //|| (ent.DisplayCollectionName != null && ent.DisplayCollectionName.LocalizedLabels
                    //    .Where(l => !string.IsNullOrEmpty(l.Label))
                    //    .Any(lbl => lbl.Label.ToLower().Contains(textName)))
                    );
                }
            }

            return list;
        }

        private void LoadEntityRelationships(IEnumerable<OneToManyRelationshipMetadataViewItem> results)
        {
            this.lstVwEntityRelationships.Dispatcher.Invoke(() =>
            {
                foreach (var entity in results
                    .OrderBy(s => s.ReferencedEntity)
                    .ThenBy(s => s.ReferencedAttribute)
                    .ThenBy(s => s.ReferencingEntity)
                    .ThenBy(s => s.ReferencingAttribute)
                    .ThenBy(s => s.SchemaName)
                )
                {
                    _itemsSourceEntityRelationshipList.Add(entity);
                }

                if (this.lstVwEntityRelationships.Items.Count == 1)
                {
                    this.lstVwEntityRelationships.SelectedItem = this.lstVwEntityRelationships.Items[0];
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

        private void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(connectionData, statusFormat, args);

            ToggleControl(this.tSProgressBar, cmBCurrentConnection, btnSetCurrentConnection, cmBRelationType, mIClearEntityCacheAndRefresh, mIClearEntityRelationshipCacheAndRefresh);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwEntities.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwEntities != null && this.lstVwEntities.SelectedItems.Count > 0;

                    UIElement[] list = { };

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

        private void cmBRoleEditorLayoutTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowExistingEntities();
        }

        private void txtBFilterEntityRelationship_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowExistingEntityRelationships();
            }
        }

        private EntityMetadataViewItem GetSelectedEntity()
        {
            return this.lstVwEntities.SelectedItems.OfType<EntityMetadataViewItem>().Count() == 1
                ? this.lstVwEntities.SelectedItems.OfType<EntityMetadataViewItem>().SingleOrDefault() : null;
        }

        private List<EntityMetadataViewItem> GetSelectedEntities()
        {
            List<EntityMetadataViewItem> result = this.lstVwEntities.SelectedItems.OfType<EntityMetadataViewItem>().ToList();

            return result;
        }

        private OneToManyRelationshipMetadataViewItem GetSelectedEntityRelationship()
        {
            return this.lstVwEntityRelationships.SelectedItems.OfType<OneToManyRelationshipMetadataViewItem>().Count() == 1
                ? this.lstVwEntityRelationships.SelectedItems.OfType<OneToManyRelationshipMetadataViewItem>().SingleOrDefault() : null;
        }

        private List<OneToManyRelationshipMetadataViewItem> GetSelectedEntityRelationships()
        {
            List<OneToManyRelationshipMetadataViewItem> result = this.lstVwEntityRelationships.SelectedItems.OfType<OneToManyRelationshipMetadataViewItem>().ToList();

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

            WindowHelper.OpenEntityMetadataExplorer(this._iWriteToOutput, service, _commonConfig, entityMetadataList, entity?.LogicalName);
        }

        private async void btnEntityAttributeExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityAttributeExplorer(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName);
        }

        private async void btnEntityRelationshipManyToManyExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityRelationshipManyToManyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName);
        }

        private async void btnEntityKeyExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityKeyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName);
        }

        private async void miEntityPrivilegesExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            IEnumerable<EntityMetadata> entityMetadataList = null;

            if (_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId];
            }

            WindowHelper.OpenEntityPrivilegesExplorer(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, entityMetadataList);
        }

        private async void miSecurityRolesExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            IEnumerable<EntityMetadata> entityMetadataList = null;

            if (_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId];
            }

            WindowHelper.OpenRolesExplorer(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnExportApplicationRibbon_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenApplicationRibbonWindow(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenGlobalOptionSetsExplorer(
                this._iWriteToOutput
                , service
                , _commonConfig
                , string.Empty
                , entity.LogicalName
            );
        }

        private async void btnSystemForms_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSystemFormWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName);
        }

        private async void btnSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSavedQueryWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnSavedChart_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSavedQueryVisualizationWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnWorkflows_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenWorkflowWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnPluginTree_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenPluginTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty, string.Empty);
        }

        private async void btnMessageTree_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnMessageRequestTree_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageRequestTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName);
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

            WindowHelper.OpenOrganizationComparerEntityMetadataWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.LogicalName);
        }

        private async void btnCompareApplicationRibbons_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerApplicationRibbonWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
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

            WindowHelper.OpenOrganizationComparerSystemFormWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.LogicalName);
        }

        private async void btnCompareSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSavedQueryWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.LogicalName);
        }

        private async void btnCompareSavedChart_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSavedQueryVisualizationWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.LogicalName);
        }

        private async void btnCompareWorkflows_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerWorkflowWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.LogicalName);
        }

        #endregion Кнопки открытия других форм с информация о сущности.

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                EntityMetadataViewItem entity = GetItemFromRoutedDataContext<EntityMetadataViewItem>(e);
                ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

                if (connectionData != null && entity != null)
                {
                    connectionData.OpenEntityMetadataInWeb(entity.EntityMetadata.MetadataId.Value);
                }
            }
        }

        private void lstVwEntityRelationships_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

                var entity = GetSelectedEntity();

                OneToManyRelationshipMetadataViewItem entityRelationship = GetItemFromRoutedDataContext<OneToManyRelationshipMetadataViewItem>(e);

                if (connectionData != null
                    && entity != null
                    && entityRelationship != null
                )
                {
                    connectionData.OpenRelationshipMetadataInWeb(entity.EntityMetadata.MetadataId.Value, entityRelationship.OneToManyRelationshipMetadata.MetadataId.Value);
                }
            }
        }

        private async Task ExecuteActionAsync(IEnumerable<string> entityNames, Func<IEnumerable<string>, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            await action(entityNames);
        }

        private void btnPublishEntity_Click(object sender, RoutedEventArgs e)
        {
            var entityList = GetSelectedEntities();

            if (entityList == null || !entityList.Any())
            {
                return;
            }

            ExecuteActionAsync(entityList.Select(item => item.LogicalName).ToList(), PublishEntityAsync);
        }

        private async Task PublishEntityAsync(IEnumerable<string> entityNames)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            var entityNamesOrdered = string.Join(",", entityNames.OrderBy(s => s));

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.PublishingEntitiesFormat2, service.ConnectionData.Name, entityNamesOrdered);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.PublishingEntitiesFormat2, service.ConnectionData.Name, entityNamesOrdered);

            try
            {
                var repository = new PublishActionsRepository(service);

                await repository.PublishEntitiesAsync(entityNames);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.PublishingEntitiesCompletedFormat2, service.ConnectionData.Name, entityNamesOrdered);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.PublishingEntitiesFailedFormat2, service.ConnectionData.Name, entityNamesOrdered);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.PublishingEntitiesFormat2, service.ConnectionData.Name, entityNamesOrdered);
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowExistingEntityRelationships();

            UpdateButtonsEnable();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingEntities();

                return;
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

        private void mIOpenEntityRelationshipInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            var entityRelationship = GetSelectedEntityRelationship();

            if (entity == null || entityRelationship == null)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenRelationshipMetadataInWeb(entity.EntityMetadata.MetadataId.Value, entityRelationship.OneToManyRelationshipMetadata.MetadataId.Value);
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
                connectionData.OpenEntityInstanceListInWeb(entity.LogicalName);
            }
        }

        private async void AddToCrmSolutionIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            await AddToSolution(true, null, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0);
        }

        private async void AddToCrmSolutionDoNotIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            await AddToSolution(true, null, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Do_not_include_subcomponents_1);
        }

        private async void AddToCrmSolutionIncludeAsShellOnly_Click(object sender, RoutedEventArgs e)
        {
            await AddToSolution(true, null, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_As_Shell_Only_2);
        }

        private async void AddToCrmSolutionLastIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddToSolution(false, solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0);
            }
        }

        private async void AddToCrmSolutionLastDoNotIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddToSolution(false, solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Do_not_include_subcomponents_1);
            }
        }

        private async void AddToCrmSolutionLastIncludeAsShellOnly_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddToSolution(false, solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_As_Shell_Only_2);
            }
        }

        private async Task AddToSolution(bool withSelect, string solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior rootComponentBehavior)
        {
            var entityList = GetSelectedEntities();

            if (entityList == null || !entityList.Any())
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.Entity, entityList.Select(item => item.EntityMetadata.MetadataId.Value).ToList(), rootComponentBehavior, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        private async void AddEntityRelationshipToCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddEntityRelationshipToSolution(true, null);
        }

        private async void AddEntityRelationshipToCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddEntityRelationshipToSolution(false, solutionUniqueName);
            }
        }

        private async Task AddEntityRelationshipToSolution(bool withSelect, string solutionUniqueName)
        {
            var entityRelationshipList = GetSelectedEntityRelationships();

            if (entityRelationshipList == null || !entityRelationshipList.Any())
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.EntityRelationship, entityRelationshipList.Select(item => item.OneToManyRelationshipMetadata.MetadataId.Value).ToList(), null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
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

                FillLastSolutionItems(connectionData, items, true, AddToCrmSolutionLastIncludeSubcomponents_Click, "contMnAddToSolutionLastIncludeSubcomponents");

                FillLastSolutionItems(connectionData, items, true, AddToCrmSolutionLastDoNotIncludeSubcomponents_Click, "contMnAddToSolutionLastDoNotIncludeSubcomponents");

                FillLastSolutionItems(connectionData, items, true, AddToCrmSolutionLastIncludeAsShellOnly_Click, "contMnAddToSolutionLastIncludeAsShellOnly");

                ActivateControls(items, connectionData.LastSelectedSolutionsUniqueName != null && connectionData.LastSelectedSolutionsUniqueName.Any(), "contMnAddToSolutionLast");
            }
        }

        private void ContextMenuEntityRelationship_Opened(object sender, RoutedEventArgs e)
        {
            if (sender is ContextMenu contextMenu)
            {
                var items = contextMenu.Items.OfType<Control>();

                ConnectionData connectionData = null;

                cmBCurrentConnection.Dispatcher.Invoke(() =>
                {
                    connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
                });

                FillLastSolutionItems(connectionData, items, true, AddEntityRelationshipToCrmSolutionLast_Click, "contMnAddToSolutionLast");
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

        private async void mIOpenDependentComponentsInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSolutionComponentDependenciesWindow(_iWriteToOutput, service, null, _commonConfig, (int)ComponentType.Entity, entity.EntityMetadata.MetadataId.Value, null);
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

            WindowHelper.OpenExplorerSolutionWindow(
                _iWriteToOutput
                , service
                , _commonConfig
                , (int)ComponentType.Entity
                , entity.EntityMetadata.MetadataId.Value
                , null
            );
        }

        private void mIEntityRelationshipOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entityRelationship = GetSelectedEntityRelationship();

            if (entityRelationship == null)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.EntityRelationship, entityRelationship.OneToManyRelationshipMetadata.MetadataId.Value);
            }
        }

        private async void mIEntityRelationshipOpenDependentComponentsInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entityRelationship = GetSelectedEntityRelationship();

            if (entityRelationship == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSolutionComponentDependenciesWindow(_iWriteToOutput, service, null, _commonConfig, (int)ComponentType.EntityRelationship, entityRelationship.OneToManyRelationshipMetadata.MetadataId.Value, null);
        }

        private async void mIEntityRelationshipOpenSolutionsContainingComponentInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entityRelationship = GetSelectedEntityRelationship();

            if (entityRelationship == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenExplorerSolutionWindow(
                _iWriteToOutput
                , service
                , _commonConfig
                , (int)ComponentType.EntityRelationship
                , entityRelationship.OneToManyRelationshipMetadata.MetadataId.Value
                , null
            );
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                this._itemsSourceEntityList?.Clear();
                this._itemsSourceEntityRelationshipList?.Clear();
            });

            if (!this.IsControlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                UpdateButtonsEnable();

                ShowExistingEntities();
            }
        }

        private void mIClearEntityCacheAndRefresh_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData != null)
            {
                _cacheEntityMetadata.Remove(connectionData.ConnectionId);

                UpdateButtonsEnable();

                ShowExistingEntities();
            }
        }

        private void mIClearEntityRelationshipCacheAndRefresh_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData != null)
            {
                _cacheEntityOneToMany.Remove(connectionData.ConnectionId);

                UpdateButtonsEnable();

                ShowExistingEntityRelationships();
            }
        }

        private void cmBRelationType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            UpdateButtonsEnable();

            ShowExistingEntityRelationships();
        }

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, cmBCurrentConnection.SelectedItem as ConnectionData);
        }

        private void hyperlinkPublishEntity_Click(object sender, RoutedEventArgs e)
        {
            EntityMetadataViewItem entity = GetItemFromRoutedDataContext<EntityMetadataViewItem>(e);

            if (entity == null)
            {
                return;
            }

            ExecuteActionAsync(new[] { entity.LogicalName }, PublishEntityAsync);
        }
    }
}