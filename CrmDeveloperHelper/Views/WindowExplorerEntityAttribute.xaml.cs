using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.UserControls;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExplorerEntityAttribute : WindowWithConnectionList
    {
        private readonly Popup _popupEntityMetadataFilter;
        private readonly EntityMetadataFilter _entityMetadataFilter;

        private readonly ObservableCollection<EntityMetadataAuditViewItem> _itemsSourceEntityList;

        private readonly ObservableCollection<AttributeMetadataViewItem> _itemsSourceAttributeList;

        private readonly Dictionary<Guid, IEnumerable<EntityMetadataAuditViewItem>> _cacheEntityMetadata = new Dictionary<Guid, IEnumerable<EntityMetadataAuditViewItem>>();

        private readonly Dictionary<Guid, ConcurrentDictionary<string, Task>> _cacheMetadataTask = new Dictionary<Guid, ConcurrentDictionary<string, Task>>();

        private readonly Dictionary<Guid, ConcurrentDictionary<string, IEnumerable<AttributeMetadataViewItem>>> _cacheAttributeMetadata = new Dictionary<Guid, ConcurrentDictionary<string, IEnumerable<AttributeMetadataViewItem>>>();

        public WindowExplorerEntityAttribute(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
            , string filterEntity
        ) : base(iWriteToOutput, commonConfig, service)
        {
            IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            InitializeComponent();

            FillRoleEditorLayoutTabs();

            LoadFromConfig();

            _entityMetadataFilter = new EntityMetadataFilter();
            _entityMetadataFilter.CloseClicked += this._entityMetadataFilter_CloseClicked;
            this._popupEntityMetadataFilter = new Popup
            {
                Child = _entityMetadataFilter,

                PlacementTarget = lblEntitiesList,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
                Focusable = true,
            };
            _popupEntityMetadataFilter.Closed += this._popupEntityMetadataFilter_Closed;

            txtBFilterEnitity.Text = filterEntity;
            txtBFilterEnitity.SelectionLength = 0;
            txtBFilterEnitity.SelectionStart = txtBFilterEnitity.Text.Length;

            txtBFilterEnitity.Focus();

            _itemsSourceEntityList = new ObservableCollection<EntityMetadataAuditViewItem>();
            lstVwEntities.ItemsSource = _itemsSourceEntityList;

            _itemsSourceAttributeList = new ObservableCollection<AttributeMetadataViewItem>();
            lstVwAttributes.ItemsSource = _itemsSourceAttributeList;

            UpdateButtonsEnable();

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            FillExplorersMenuItems();

            DecreaseInit();

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

        private void FillExplorersMenuItems()
        {
            var explorersHelper = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService
                , getEntityName: GetEntityName
                , getEntityMetadataList: GetEntityMetadataList
            );

            var compareWindowsHelper = new CompareWindowsHelper(_iWriteToOutput, _commonConfig, () => Tuple.Create(GetSelectedConnection(), GetSelectedConnection())
                , getEntityName: GetEntityName
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

        private string GetEntityName()
        {
            var entity = GetSelectedEntity();

            return entity?.LogicalName;
        }

        private IEnumerable<EntityMetadata> GetEntityMetadataList(Guid connectionId)
        {
            if (_cacheEntityMetadata.ContainsKey(connectionId))
            {
                return _cacheEntityMetadata[connectionId].Select(i => i.EntityMetadata).ToList();
            }

            return null;
        }

        protected override void LoadConfigurationInternal(WindowSettings winConfig)
        {
            base.LoadConfigurationInternal(winConfig);

            LoadFormSettings(winConfig);
        }

        private const string paramColumnEntityWidth = "ColumnEntityWidth";

        private void LoadFormSettings(WindowSettings winConfig)
        {
            if (winConfig.DictDouble.ContainsKey(paramColumnEntityWidth))
            {
                columnEntity.Width = new GridLength(winConfig.DictDouble[paramColumnEntityWidth]);
            }
        }

        protected override void SaveConfigurationInternal(WindowSettings winConfig)
        {
            base.SaveConfigurationInternal(winConfig);

            winConfig.DictDouble[paramColumnEntityWidth] = columnEntity.Width.Value;
        }

        protected override void OnClosed(EventArgs e)
        {
            BindingOperations.ClearAllBindings(cmBCurrentConnection);

            cmBCurrentConnection.Items.DetachFromSourceCollection();

            cmBCurrentConnection.DataContext = null;
            cmBCurrentConnection.ItemsSource = null;

            base.OnClosed(e);
        }

        private void btnEntityMetadataFilter_Click(object sender, RoutedEventArgs e)
        {
            _popupEntityMetadataFilter.IsOpen = true;
            _popupEntityMetadataFilter.Child.Focus();
        }

        private void _popupEntityMetadataFilter_Closed(object sender, EventArgs e)
        {
            if (_entityMetadataFilter.FilterChanged)
            {
                ShowExistingEntities();
            }
        }

        private void _entityMetadataFilter_CloseClicked(object sender, EventArgs e)
        {
            if (_popupEntityMetadataFilter.IsOpen)
            {
                _popupEntityMetadataFilter.IsOpen = false;
                this.Focus();
            }
        }

        private async Task ShowExistingEntities()
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.LoadingEntities);

            _itemsSourceEntityList.Clear();
            _itemsSourceAttributeList.Clear();

            IEnumerable<EntityMetadataAuditViewItem> list = Enumerable.Empty<EntityMetadataAuditViewItem>();

            try
            {
                if (service != null)
                {
                    if (!_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
                    {
                        EntityMetadataRepository repository = new EntityMetadataRepository(service);

                        var task = repository.GetEntitiesForEntityAttributeExplorerAsync(EntityFilters.Entity);

                        var temp = await task;

                        _cacheEntityMetadata.Add(service.ConnectionData.ConnectionId, temp.Select(e => new EntityMetadataAuditViewItem(e)).ToList());
                    }

                    list = _cacheEntityMetadata[service.ConnectionData.ConnectionId];
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

            ShowExistingAttributes();
        }

        private IEnumerable<EntityMetadataAuditViewItem> FilterEntityList(IEnumerable<EntityMetadataAuditViewItem> list, string textName, RoleEditorLayoutTab selectedTab)
        {
            list = _entityMetadataFilter.FilterList(list, i => i.EntityMetadata);

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
                            ent.LogicalName.ToLower().IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
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

        private void LoadEntities(IEnumerable<EntityMetadataAuditViewItem> results)
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

        private async Task ShowExistingAttributes()
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.LoadingAttributes);

            string entityLogicalName = string.Empty;

            this.Dispatcher.Invoke(() =>
            {
                _itemsSourceAttributeList.Clear();

                entityLogicalName = GetSelectedEntity()?.LogicalName;
            });

            IEnumerable<AttributeMetadataViewItem> list = Enumerable.Empty<AttributeMetadataViewItem>();

            if (!string.IsNullOrEmpty(entityLogicalName))
            {
                try
                {
                    if (service != null)
                    {
                        if (!_cacheAttributeMetadata.ContainsKey(service.ConnectionData.ConnectionId))
                        {
                            _cacheAttributeMetadata.Add(service.ConnectionData.ConnectionId, new ConcurrentDictionary<string, IEnumerable<AttributeMetadataViewItem>>(StringComparer.InvariantCultureIgnoreCase));
                        }

                        if (!_cacheMetadataTask.ContainsKey(service.ConnectionData.ConnectionId))
                        {
                            _cacheMetadataTask.Add(service.ConnectionData.ConnectionId, new ConcurrentDictionary<string, Task>(StringComparer.InvariantCultureIgnoreCase));
                        }

                        var cacheAttribute = _cacheAttributeMetadata[service.ConnectionData.ConnectionId];
                        var cacheMetadataTask = _cacheMetadataTask[service.ConnectionData.ConnectionId];

                        if (!cacheAttribute.ContainsKey(entityLogicalName))
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
                                var task = GetEntityMetadataInformationAsync(service, entityLogicalName, cacheAttribute, cacheMetadataTask);

                                cacheMetadataTask.TryAdd(entityLogicalName, task);

                                await task;
                            }
                        }

                        if (cacheAttribute.ContainsKey(entityLogicalName))
                        {
                            list = cacheAttribute[entityLogicalName];
                        }
                    }
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                }
            }

            string textName = string.Empty;

            txtBFilterAttribute.Dispatcher.Invoke(() =>
            {
                textName = txtBFilterAttribute.Text.Trim().ToLower();
            });

            list = FilterAttributeList(list, textName);

            LoadAttributes(list);

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.LoadingAttributesCompletedFormat1, list.Count());
        }

        private static async Task GetEntityMetadataInformationAsync(IOrganizationServiceExtented service, string entityLogicalName, ConcurrentDictionary<string, IEnumerable<AttributeMetadataViewItem>> cacheAttribute, ConcurrentDictionary<string, Task> cacheMetadataTask)
        {
            var repository = new EntityMetadataRepository(service);

            var metadata = await repository.GetEntityMetadataAttributesAsync(entityLogicalName, EntityFilters.Attributes);

            if (metadata != null && metadata.Attributes != null)
            {
                if (!cacheAttribute.ContainsKey(entityLogicalName))
                {
                    cacheAttribute.TryAdd(entityLogicalName, metadata.Attributes.Where(e => string.IsNullOrEmpty(e.AttributeOf)).Select(e => new AttributeMetadataViewItem(e)).ToList());
                }
            }

            if (cacheMetadataTask.ContainsKey(entityLogicalName))
            {
                cacheMetadataTask.TryRemove(entityLogicalName, out _);
            }
        }

        private static IEnumerable<AttributeMetadataViewItem> FilterAttributeList(IEnumerable<AttributeMetadataViewItem> list, string textName)
        {
            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                if (Guid.TryParse(textName, out Guid tempGuid))
                {
                    list = list.Where(ent => ent.AttributeMetadata.MetadataId == tempGuid);
                }
                else
                {
                    list = list
                    .Where(ent =>
                        ent.LogicalName.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                        ||
                        (
                            ent.AttributeMetadata.DisplayName != null
                            && ent.AttributeMetadata.DisplayName.LocalizedLabels != null
                            && ent.AttributeMetadata.DisplayName.LocalizedLabels
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

        private void LoadAttributes(IEnumerable<AttributeMetadataViewItem> results)
        {
            this.lstVwAttributes.Dispatcher.Invoke(() =>
            {
                foreach (var entity in results.OrderBy(s => s.LogicalName))
                {
                    _itemsSourceAttributeList.Add(entity);
                }

                if (this.lstVwAttributes.Items.Count == 1)
                {
                    this.lstVwAttributes.SelectedItem = this.lstVwAttributes.Items[0];
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

        protected override void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(connectionData, statusFormat, args);

            ToggleControl(this.tSProgressBar, cmBCurrentConnection, btnSetCurrentConnection, mISaveChanges, mIClearEntityCacheAndRefresh, mIClearAttributeCacheAndRefresh);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwEntities.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwEntities.SelectedItems.Count > 0;

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

        private void txtBFilterAttribute_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowExistingAttributes();
            }
        }

        private EntityMetadataAuditViewItem GetSelectedEntity()
        {
            return this.lstVwEntities.SelectedItems.OfType<EntityMetadataAuditViewItem>().Count() == 1
                ? this.lstVwEntities.SelectedItems.OfType<EntityMetadataAuditViewItem>().SingleOrDefault() : null;
        }

        private List<EntityMetadataAuditViewItem> GetSelectedEntities()
        {
            List<EntityMetadataAuditViewItem> result = this.lstVwEntities.SelectedItems.OfType<EntityMetadataAuditViewItem>().ToList();

            return result;
        }

        private AttributeMetadataViewItem GetSelectedAttribute()
        {
            return this.lstVwAttributes.SelectedItems.OfType<AttributeMetadataViewItem>().Count() == 1
                ? this.lstVwAttributes.SelectedItems.OfType<AttributeMetadataViewItem>().SingleOrDefault() : null;
        }

        private List<AttributeMetadataViewItem> GetSelectedAttributes()
        {
            List<AttributeMetadataViewItem> result = this.lstVwAttributes.SelectedItems.OfType<AttributeMetadataViewItem>().ToList();

            return result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lstVwEntities_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
            {
                return;
            }

            IInputElement element = e.MouseDevice.DirectlyOver;
            if (element != null
                && element is FrameworkElement frameworkElement
            )
            {
                if (frameworkElement.Parent is DataGridCell cell)
                {
                    if (cell.Column == colEntityName
                        || cell.Column == colEntityDisplayName
                    )
                    {
                        EntityMetadataAuditViewItem entity = GetItemFromRoutedDataContext<EntityMetadataAuditViewItem>(e);
                        ConnectionData connectionData = GetSelectedConnection();

                        if (connectionData != null && entity != null)
                        {
                            connectionData.OpenEntityMetadataInWeb(entity.EntityMetadata.MetadataId.Value);
                        }
                    }
                }
            }
        }

        private void lstVwAttributes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
            {
                return;
            }

            IInputElement element = e.MouseDevice.DirectlyOver;
            if (element != null
                && element is FrameworkElement frameworkElement
            )
            {
                if (frameworkElement.Parent is DataGridCell cell)
                {
                    if (cell.Column == colAttributeName
                        || cell.Column == colAttributeDisplayName
                        || cell.Column == colAttributeType
                    )
                    {
                        ConnectionData connectionData = GetSelectedConnection();

                        var entity = GetSelectedEntity();

                        AttributeMetadataViewItem attribute = GetItemFromRoutedDataContext<AttributeMetadataViewItem>(e);

                        if (connectionData != null
                            && entity != null
                            && attribute != null
                        )
                        {
                            connectionData.OpenAttributeMetadataInWeb(entity.EntityMetadata.MetadataId.Value, attribute.AttributeMetadata.MetadataId.Value);
                        }
                    }
                }
            }
        }

        private async void btnPublishEntity_Click(object sender, RoutedEventArgs e)
        {
            var entityList = GetSelectedEntities();

            if (entityList == null || !entityList.Any())
            {
                return;
            }

            await PublishEntityAsync(GetSelectedConnection(), entityList.Select(item => item.LogicalName).ToList());
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowExistingAttributes();

            UpdateButtonsEnable();
        }

        protected override void OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            ShowExistingEntities();
        }

        private void mIOpenEntityInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                connectionData.OpenEntityMetadataInWeb(entity.EntityMetadata.MetadataId.Value);
            }
        }

        private void mIOpenAttributeInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            var attribute = GetSelectedAttribute();

            if (entity == null || attribute == null)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                connectionData.OpenAttributeMetadataInWeb(entity.EntityMetadata.MetadataId.Value, attribute.AttributeMetadata.MetadataId.Value);
            }
        }

        private void mIOpenEntityListInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

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

            await AddEntityMetadataToSolution(
                GetSelectedConnection()
                , entityList.Select(item => item.EntityMetadata.MetadataId.Value)
                , withSelect
                , solutionUniqueName
                , rootComponentBehavior
            );
        }

        private async void AddAttributeToCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddAttributeToSolution(true, null);
        }

        private async void AddAttributeToCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddAttributeToSolution(false, solutionUniqueName);
            }
        }

        private async Task AddAttributeToSolution(bool withSelect, string solutionUniqueName)
        {
            var attributeList = GetSelectedAttributes();

            if (attributeList == null || !attributeList.Any())
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.Attribute, attributeList.Select(item => item.AttributeMetadata.MetadataId.Value).ToList(), null, withSelect);
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

                ConnectionData connectionData = GetSelectedConnection();

                FillLastSolutionItems(connectionData, items, true, AddToCrmSolutionLastIncludeSubcomponents_Click, "contMnAddToSolutionLastIncludeSubcomponents");

                FillLastSolutionItems(connectionData, items, true, AddToCrmSolutionLastDoNotIncludeSubcomponents_Click, "contMnAddToSolutionLastDoNotIncludeSubcomponents");

                FillLastSolutionItems(connectionData, items, true, AddToCrmSolutionLastIncludeAsShellOnly_Click, "contMnAddToSolutionLastIncludeAsShellOnly");

                ActivateControls(items, connectionData.LastSelectedSolutionsUniqueName != null && connectionData.LastSelectedSolutionsUniqueName.Any(), "contMnAddToSolutionLast");
            }
        }

        private void ContextMenuAttribute_Opened(object sender, RoutedEventArgs e)
        {
            if (sender is ContextMenu contextMenu)
            {
                var items = contextMenu.Items.OfType<Control>();

                ConnectionData connectionData = GetSelectedConnection();

                FillLastSolutionItems(connectionData, items, true, AddAttributeToCrmSolutionLast_Click, "contMnAddToSolutionLast");
            }
        }

        private void mIOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

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

            WindowHelper.OpenSolutionComponentDependenciesExplorer(_iWriteToOutput, service, null, _commonConfig, (int)ComponentType.Entity, entity.EntityMetadata.MetadataId.Value, null);
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
                , (int)ComponentType.Entity
                , entity.EntityMetadata.MetadataId.Value
                , null
            );
        }

        private void mIAttributeOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            var attribute = GetSelectedAttribute();

            if (attribute == null)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.Attribute, attribute.AttributeMetadata.MetadataId.Value);
            }
        }

        private async void mIAttributeOpenDependentComponentsInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var attribute = GetSelectedAttribute();

            if (attribute == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSolutionComponentDependenciesExplorer(_iWriteToOutput, service, null, _commonConfig, (int)ComponentType.Attribute, attribute.AttributeMetadata.MetadataId.Value, null);
        }

        private async void mIAttributeOpenSolutionsContainingComponentInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var attribute = GetSelectedAttribute();

            if (attribute == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenExplorerSolutionExplorer(
                _iWriteToOutput
                , service
                , _commonConfig
                , (int)ComponentType.Attribute
                , attribute.AttributeMetadata.MetadataId.Value
                , null
            );
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                this._itemsSourceEntityList?.Clear();
                this._itemsSourceAttributeList?.Clear();
            });

            if (!IsControlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                UpdateButtonsEnable();

                ShowExistingEntities();
            }
        }

        private void mIClearEntityCacheAndRefresh_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                _cacheEntityMetadata.Remove(connectionData.ConnectionId);

                UpdateButtonsEnable();

                ShowExistingEntities();
            }
        }

        private void mIClearAttributeCacheAndRefresh_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                _cacheAttributeMetadata.Remove(connectionData.ConnectionId);

                UpdateButtonsEnable();

                ShowExistingAttributes();
            }
        }

        private async void mISaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.SavingChangesFormat1, service.ConnectionData.Name);

            HashSet<string> listForPublish = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.SavingChangesFormat1, service.ConnectionData.Name);

            try
            {
                var listEntitiesToChange = new List<EntityMetadataAuditViewItem>();
                var listAttributesToChange = new List<AttributeMetadataViewItem>();

                if (_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
                {
                    listEntitiesToChange.AddRange(_cacheEntityMetadata[service.ConnectionData.ConnectionId].Where(item => item.IsChanged));
                }

                if (_cacheAttributeMetadata.ContainsKey(service.ConnectionData.ConnectionId))
                {
                    var dict = _cacheAttributeMetadata[service.ConnectionData.ConnectionId];

                    listAttributesToChange.AddRange(dict.Values.SelectMany(item => item.Where(a => a.IsChanged)));
                }

                var repository = new EntityMetadataRepository(service);

                if (listAttributesToChange.Any())
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.UpdatingAttributes);

                    foreach (var attribute in listAttributesToChange.OrderBy(a => a.AttributeMetadata.EntityLogicalName).ThenBy(a => a.LogicalName))
                    {
                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, "    {0}.{1}", attribute.AttributeMetadata.EntityLogicalName, attribute.LogicalName);

                        listForPublish.Add(attribute.AttributeMetadata.EntityLogicalName);

                        try
                        {
                            await repository.UpdateAttributeMetadataAsync(attribute.AttributeMetadata);
                        }
                        catch (Exception ex)
                        {
                            _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                        }
                    }
                }

                if (listEntitiesToChange.Any())
                {
                    this._iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.UpdatingEntities);

                    foreach (var entityMetadata in listEntitiesToChange.OrderBy(a => a.LogicalName))
                    {
                        this._iWriteToOutput.WriteToOutput(service.ConnectionData, "    {0}", entityMetadata.LogicalName);

                        listForPublish.Add(entityMetadata.LogicalName);

                        try
                        {
                            await repository.UpdateEntityMetadataAsync(entityMetadata.EntityMetadata);
                        }
                        catch (Exception ex)
                        {
                            _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                        }
                    }
                }

                if (listForPublish.Any())
                {
                    var entityNamesOrdered = string.Join(",", listForPublish.OrderBy(s => s));

                    UpdateStatus(service.ConnectionData, Properties.OutputStrings.PublishingEntitiesFormat2, service.ConnectionData.Name, entityNamesOrdered);

                    var repositoryPublish = new PublishActionsRepository(service);

                    await repositoryPublish.PublishEntitiesAsync(listForPublish);
                }

                foreach (var attribute in listAttributesToChange.OrderBy(a => a.AttributeMetadata.EntityLogicalName).ThenBy(a => a.LogicalName))
                {
                    try
                    {
                        var metadata = await repository.GetAttributeMetadataAsync(attribute.AttributeMetadata.MetadataId.Value);

                        attribute.LoadMetadata(metadata);
                    }
                    catch (Exception ex)
                    {
                        _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    }
                }

                foreach (var entityMetadata in listEntitiesToChange.OrderBy(a => a.LogicalName))
                {
                    try
                    {
                        var metadata = await repository.GetEntityMetadataAttributesAsync(entityMetadata.EntityMetadata.MetadataId.Value, EntityFilters.Entity);

                        entityMetadata.LoadMetadata(metadata);
                    }
                    catch (Exception ex)
                    {
                        _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                    }
                }

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.SavingChangesCompletedFormat1, service.ConnectionData.Name);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.SavingChangesFailedFormat1, service.ConnectionData.Name);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.SavingChangesFormat1, service.ConnectionData.Name);
        }

        #region Set Attributes Properties

        private void ExecuteOnSelectedAttributes(Action<AttributeMetadataViewItem> action)
        {
            var list = lstVwAttributes.SelectedItems.OfType<AttributeMetadataViewItem>().ToList();

            foreach (var item in list)
            {
                action?.Invoke(item);
            }
        }

        private void mISetAttributesRequiredLevelToNone_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedAttributes(item => item.RequiredLevel = AttributeRequiredLevel.None);
        }

        private void mISetAttributesRequiredLevelToRecommended_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedAttributes(item => item.RequiredLevel = AttributeRequiredLevel.Recommended);
        }

        private void mISetAttributesRequiredLevelToApplicationRequired_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedAttributes(item => item.RequiredLevel = AttributeRequiredLevel.ApplicationRequired);
        }

        private void mISetAttributesRequiredLevelToSystemRequired_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedAttributes(item => item.RequiredLevel = AttributeRequiredLevel.SystemRequired);
        }

        private void mISetAttributesIsAuditEnabledToFalse_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedAttributes(item => item.IsAuditEnabled = false);
        }

        private void mISetAttributesIsAuditEnabledToTrue_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedAttributes(item => item.IsAuditEnabled = true);
        }

        private void mISetAttributesIsCustomizableToFalse_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedAttributes(item => item.IsCustomizable = false);
        }

        private void mISetAttributesIsCustomizableToTrue_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedAttributes(item => item.IsCustomizable = true);
        }

        private void mISetAttributesIsRenameableToFalse_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedAttributes(item => item.IsRenameable = false);
        }

        private void mISetAttributesIsRenameableToTrue_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedAttributes(item => item.IsRenameable = true);
        }

        private void mISetAttributesIsValidForAdvancedFindToFalse_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedAttributes(item => item.IsValidForAdvancedFind = false);
        }

        private void mISetAttributesIsValidForAdvancedFindToTrue_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedAttributes(item => item.IsValidForAdvancedFind = true);
        }

        private void mISetAttributesCanModifyAdditionalSettingsToFalse_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedAttributes(item => item.CanModifyAdditionalSettings = false);
        }

        private void mISetAttributesCanModifyAdditionalSettingsToTrue_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedAttributes(item => item.CanModifyAdditionalSettings = true);
        }

        private void mISetAttributesIsSecuredToFalse_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedAttributes(item => item.IsSecured = false);
        }

        private void mISetAttributesIsSecuredToTrue_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedAttributes(item => item.IsSecured = true);
        }

        private void mISetAttributesIsDataSourceSecretToFalse_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedAttributes(item => item.IsDataSourceSecret = false);
        }

        private void mISetAttributesIsDataSourceSecretToTrue_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedAttributes(item => item.IsDataSourceSecret = true);
        }

        private void mISetAttributesIsValidForFormToFalse_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedAttributes(item => item.IsValidForForm = false);
        }

        private void mISetAttributesIsValidForFormToTrue_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedAttributes(item => item.IsValidForForm = true);
        }

        private void mISetAttributesIsRequiredForFormToFalse_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedAttributes(item => item.IsRequiredForForm = false);
        }

        private void mISetAttributesIsRequiredForFormToTrue_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedAttributes(item => item.IsRequiredForForm = true);
        }

        private void mISetAttributesIsValidForGridToFalse_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedAttributes(item => item.IsValidForGrid = false);
        }

        private void mISetAttributesIsValidForGridToTrue_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedAttributes(item => item.IsValidForGrid = true);
        }

        private void mISetAttributesIsSortableEnabledToFalse_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedAttributes(item => item.IsSortableEnabled = false);
        }

        private void mISetAttributesIsSortableEnabledToTrue_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedAttributes(item => item.IsSortableEnabled = true);
        }

        private void mISetAttributesIsGlobalFilterEnabledToFalse_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedAttributes(item => item.IsGlobalFilterEnabled = false);
        }

        private void mISetAttributesIsGlobalFilterEnabledToTrue_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedAttributes(item => item.IsGlobalFilterEnabled = true);
        }

        #endregion Set Attributes Properties

        #region Select Attributes

        private void ExecuteSelectAttributes(Func<AttributeMetadataViewItem, bool> checker)
        {
            var list = lstVwAttributes.Items.OfType<AttributeMetadataViewItem>().Where(i => checker(i)).ToList();

            foreach (var item in list)
            {
                lstVwAttributes.SelectedItems.Add(item);
            }
        }

        private void miSelectAttributeRequiredLevelWithNone_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectAttributes(item => item.RequiredLevel == AttributeRequiredLevel.None);
        }

        private void miSelectAttributeRequiredLevelWithRecommended_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectAttributes(item => item.RequiredLevel == AttributeRequiredLevel.Recommended);
        }

        private void miSelectAttributeRequiredLevelWithApplicationRequired_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectAttributes(item => item.RequiredLevel == AttributeRequiredLevel.ApplicationRequired);
        }

        private void miSelectAttributeRequiredLevelWithSystemRequired_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectAttributes(item => item.RequiredLevel == AttributeRequiredLevel.SystemRequired);
        }

        private void miSelectAttributeIsAuditEnabledWithFalse_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectAttributes(item => item.IsAuditEnabled == false);
        }

        private void miSelectAttributeIsAuditEnabledWithTrue_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectAttributes(item => item.IsAuditEnabled == true);
        }

        private void miSelectAttributeIsCustomizableWithFalse_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectAttributes(item => item.IsCustomizable == false);
        }

        private void miSelectAttributeIsCustomizableWithTrue_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectAttributes(item => item.IsCustomizable == true);
        }

        private void miSelectAttributeIsRenameableWithFalse_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectAttributes(item => item.IsRenameable == false);
        }

        private void miSelectAttributeIsRenameableWithTrue_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectAttributes(item => item.IsRenameable == true);
        }

        private void miSelectAttributeIsValidForAdvancedFindWithFalse_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectAttributes(item => item.IsValidForAdvancedFind == false);
        }

        private void miSelectAttributeIsValidForAdvancedFindWithTrue_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectAttributes(item => item.IsValidForAdvancedFind == true);
        }

        private void miSelectAttributeCanModifyAdditionalSettingsWithFalse_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectAttributes(item => item.CanModifyAdditionalSettings == false);
        }

        private void miSelectAttributeCanModifyAdditionalSettingsWithTrue_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectAttributes(item => item.CanModifyAdditionalSettings == true);
        }

        private void miSelectAttributeIsSecuredWithFalse_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectAttributes(item => item.IsSecured == false);
        }

        private void miSelectAttributeIsSecuredWithTrue_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectAttributes(item => item.IsSecured == true);
        }

        private void miSelectAttributeIsDataSourceSecretWithFalse_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectAttributes(item => item.IsDataSourceSecret == false);
        }

        private void miSelectAttributeIsDataSourceSecretWithTrue_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectAttributes(item => item.IsDataSourceSecret == true);
        }

        private void miSelectAttributeIsValidForFormWithFalse_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectAttributes(item => item.IsValidForForm == false);
        }

        private void miSelectAttributeIsValidForFormWithTrue_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectAttributes(item => item.IsValidForForm == true);
        }

        private void miSelectAttributeIsRequiredForFormWithFalse_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectAttributes(item => item.IsRequiredForForm == false);
        }

        private void miSelectAttributeIsRequiredForFormWithTrue_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectAttributes(item => item.IsRequiredForForm == true);
        }

        private void miSelectAttributeIsValidForGridWithFalse_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectAttributes(item => item.IsValidForGrid == false);
        }

        private void miSelectAttributeIsValidForGridWithTrue_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectAttributes(item => item.IsValidForGrid == true);
        }

        private void miSelectAttributeIsSortableEnabledWithFalse_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectAttributes(item => item.IsSortableEnabled == false);
        }

        private void miSelectAttributeIsSortableEnabledWithTrue_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectAttributes(item => item.IsSortableEnabled == true);
        }

        private void miSelectAttributeIsGlobalFilterEnabledWithFalse_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectAttributes(item => item.IsGlobalFilterEnabled == false);
        }

        private void miSelectAttributeIsGlobalFilterEnabledWithTrue_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectAttributes(item => item.IsGlobalFilterEnabled == true);
        }

        private void miSelectCustomAttributes_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectAttributes(item => item.AttributeMetadata.IsCustomAttribute.GetValueOrDefault() == true);
        }

        private async void miSelectAttributesOnForms_Click(object sender, RoutedEventArgs e)
        {
            string entityLogicalName = string.Empty;

            this.Dispatcher.Invoke(() =>
            {
                entityLogicalName = GetSelectedEntity()?.LogicalName;
            });

            if (string.IsNullOrEmpty(entityLogicalName))
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var repository = new SystemFormRepository(service);

            var list = await repository.GetListAsync(entityLogicalName, null, new ColumnSet(SystemForm.Schema.Attributes.formxml));

            var hashAttributes = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var form in list)
            {
                if (!string.IsNullOrEmpty(form.FormXml) && ContentComparerHelper.TryParseXml(form.FormXml, out var doc))
                {
                    var elements = doc.DescendantsAndSelf("control");

                    foreach (var control in elements)
                    {
                        var attrField = control.Attribute("datafieldname");

                        if (attrField != null
                            && !string.IsNullOrEmpty(attrField.Value)
                            )
                        {
                            hashAttributes.Add(attrField.Value);
                        }
                    }
                }
            }

            lstVwAttributes.Dispatcher.Invoke(() =>
            {
                ExecuteSelectAttributes(item => hashAttributes.Contains(item.LogicalName));
            });
        }

        #endregion Select Attributes

        #region Select Entities

        private void ExecuteSelectEntities(Func<EntityMetadataAuditViewItem, bool> checker)
        {
            var list = lstVwEntities.Items.OfType<EntityMetadataAuditViewItem>().Where(i => checker(i)).ToList();

            foreach (var item in list)
            {
                lstVwEntities.SelectedItems.Add(item);
            }
        }

        private void miSelectCustomEntities_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectEntities(item => item.EntityMetadata.IsCustomEntity.GetValueOrDefault() == true);
        }

        private void miSelectEntityIsAuditEnabledWithFalse_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectEntities(item => item.IsAuditEnabled == false);
        }

        private void miSelectEntityIsAuditEnabledWithTrue_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectEntities(item => item.IsAuditEnabled == true);
        }

        #endregion Select Entities

        #region Set Entity Properties

        private void ExecuteOnSelectedEntities(Action<EntityMetadataAuditViewItem> action)
        {
            var list = lstVwEntities.SelectedItems.OfType<EntityMetadataAuditViewItem>().ToList();

            foreach (var item in list)
            {
                action?.Invoke(item);
            }
        }

        private void mISetEntitiesIsAuditEnabledToFalse_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedEntities(item => item.IsAuditEnabled = false);
        }

        private void mISetEntitiesIsAuditEnabledToTrue_Click(object sender, RoutedEventArgs e)
        {
            ExecuteOnSelectedEntities(item => item.IsAuditEnabled = true);
        }

        #endregion Set Entity Properties

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, GetSelectedConnection());
        }

        private async void hyperlinkPublishEntity_Click(object sender, RoutedEventArgs e)
        {
            EntityMetadataAuditViewItem entity = GetItemFromRoutedDataContext<EntityMetadataAuditViewItem>(e);

            if (entity == null)
            {
                return;
            }

            await PublishEntityAsync(GetSelectedConnection(), new[] { entity.LogicalName });
        }
    }
}