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

        private readonly Popup _popupAttributeMetadataFilter;
        private readonly AttributeMetadataFilter _attributeMetadataFilter;

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

            InitializeComponent();

            SetInputLanguageEnglish();

            LoadFromConfig();

            _entityMetadataFilter = new EntityMetadataFilter();
            _entityMetadataFilter.CloseClicked += this.entityMetadataFilter_CloseClicked;
            this._popupEntityMetadataFilter = new Popup
            {
                Child = _entityMetadataFilter,

                PlacementTarget = lblFilterEnitity,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
                Focusable = true,
            };
            _popupEntityMetadataFilter.Closed += this.popupEntityMetadataFilter_Closed;

            _attributeMetadataFilter = new AttributeMetadataFilter();
            _attributeMetadataFilter.CloseClicked += this.attributeMetadataFilter_CloseClicked;
            this._popupAttributeMetadataFilter = new Popup
            {
                Child = _attributeMetadataFilter,

                PlacementTarget = lblFilterAttribute,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
                Focusable = true,
            };
            _popupAttributeMetadataFilter.Closed += this.popupAttributeMetadataFilter_Closed;

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

            var task = ShowExistingEntities();
        }

        private void LoadFromConfig()
        {
            WindowSettings winConfig = GetWindowsSettings();

            LoadFormSettings(winConfig);
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

        private async void popupEntityMetadataFilter_Closed(object sender, EventArgs e)
        {
            if (_entityMetadataFilter.FilterChanged)
            {
                await ShowExistingEntities();
            }
        }

        private void entityMetadataFilter_CloseClicked(object sender, EventArgs e)
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

            var connectionData = GetSelectedConnection();

            ToggleControls(connectionData, false, Properties.OutputStrings.LoadingEntities);

            this.Dispatcher.Invoke(() =>
            {
                _itemsSourceEntityList.Clear();
                _itemsSourceAttributeList.Clear();
            });

            IEnumerable<EntityMetadataAuditViewItem> list = Enumerable.Empty<EntityMetadataAuditViewItem>();

            try
            {
                var service = await GetService();

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
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }

            string textName = string.Empty;

            this.Dispatcher.Invoke(() =>
            {
                textName = txtBFilterEnitity.Text.Trim().ToLower();
            });

            list = FilterEntityList(list, textName);

            this.lstVwEntities.Dispatcher.Invoke(() =>
            {
                foreach (var entity in list.OrderBy(s => s.LogicalName))
                {
                    _itemsSourceEntityList.Add(entity);
                }

                if (this.lstVwEntities.Items.Count == 1)
                {
                    this.lstVwEntities.SelectedItem = this.lstVwEntities.Items[0];
                }
                else
                {
                    var entity = list.FirstOrDefault(e => string.Equals(e.LogicalName, textName, StringComparison.InvariantCultureIgnoreCase));

                    if (entity != null)
                    {
                        this.lstVwEntities.SelectedItem = entity;
                    }
                }
            });

            ToggleControls(connectionData, true, Properties.OutputStrings.LoadingEntitiesCompletedFormat1, list.Count());

            await ShowExistingAttributes();
        }

        private IEnumerable<EntityMetadataAuditViewItem> FilterEntityList(IEnumerable<EntityMetadataAuditViewItem> list, string textName)
        {
            list = _entityMetadataFilter.FilterList(list, i => i.EntityMetadata);

            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                if (int.TryParse(textName, out int tempInt))
                {
                    list = list.Where(ent => ent.EntityMetadata.ObjectTypeCode == tempInt);
                }
                else if (Guid.TryParse(textName, out Guid tempGuid))
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

            return list;
        }

        private async Task ShowExistingAttributes()
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            var connectionData = GetSelectedConnection();

            ToggleControls(connectionData, false, Properties.OutputStrings.LoadingAttributes);

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
                    var service = await GetService();

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
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }

            string textName = string.Empty;

            txtBFilterAttribute.Dispatcher.Invoke(() =>
            {
                textName = txtBFilterAttribute.Text.Trim().ToLower();
            });

            list = FilterAttributeList(list, textName);

            this.lstVwAttributes.Dispatcher.Invoke(() =>
            {
                foreach (var entityAttribute in list.OrderBy(s => s.LogicalName))
                {
                    _itemsSourceAttributeList.Add(entityAttribute);
                }

                if (this.lstVwAttributes.Items.Count == 1)
                {
                    this.lstVwAttributes.SelectedItem = this.lstVwAttributes.Items[0];
                }
                else
                {
                    var entityAttribute = list.FirstOrDefault(e => string.Equals(e.LogicalName, textName, StringComparison.InvariantCultureIgnoreCase));

                    if (entityAttribute != null)
                    {
                        this.lstVwAttributes.SelectedItem = entityAttribute;
                    }
                }
            });

            ToggleControls(connectionData, true, Properties.OutputStrings.LoadingAttributesCompletedFormat1, list.Count());
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

        private IEnumerable<AttributeMetadataViewItem> FilterAttributeList(IEnumerable<AttributeMetadataViewItem> list, string textName)
        {
            list = _attributeMetadataFilter.FilterList(list, i => i.AttributeMetadata);

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

            ToggleControl(this.tSProgressBar, cmBCurrentConnection, btnSetCurrentConnection, mISaveChanges, mIClearEntityCacheAndRefresh, mIClearAttributeCacheAndRefresh, mIClearCache);

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

        private async void txtBFilterEnitity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await ShowExistingEntities();
            }
        }

        private async void txtBFilterAttribute_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await ShowExistingAttributes();
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
                    if (cell.Column.IsReadOnly)
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
                    if (cell.Column.IsReadOnly)
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

        private async void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await ShowExistingAttributes();

            UpdateButtonsEnable();
        }

        protected override async Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            await ShowExistingEntities();
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

        private void mIOpenEntityFetchXmlFile_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                this._iWriteToOutput.OpenFetchXmlFile(connectionData, _commonConfig, entity.EntityMetadata.LogicalName);
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
            var entitiesList = GetSelectedEntities()
                .Select(item => item.EntityMetadata.MetadataId.Value);

            if (!entitiesList.Any())
            {
                return;
            }

            await AddEntityMetadataToSolution(
                GetSelectedConnection()
                , entitiesList
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
            var attributeList = GetSelectedAttributes()
                .Select(item => item.AttributeMetadata.MetadataId.Value)
                .ToList();

            if (!attributeList.Any())
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.Attribute, attributeList, null, withSelect);
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

            if (service == null)
            {
                return;
            }

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

            if (service == null)
            {
                return;
            }

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

            if (service == null)
            {
                return;
            }

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

            if (service == null)
            {
                return;
            }

            WindowHelper.OpenExplorerSolutionExplorer(
                _iWriteToOutput
                , service
                , _commonConfig
                , (int)ComponentType.Attribute
                , attribute.AttributeMetadata.MetadataId.Value
                , null
            );
        }

        private async void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

                await ShowExistingEntities();
            }
        }

        #region Clear Cache

        private async void mIClearEntityCacheAndRefresh_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                _cacheEntityMetadata.Remove(connectionData.ConnectionId);

                UpdateButtonsEnable();

                await ShowExistingEntities();
            }
        }

        private async void mIClearAttributeCacheAndRefresh_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                _cacheAttributeMetadata.Remove(connectionData.ConnectionId);

                UpdateButtonsEnable();

                await ShowExistingAttributes();
            }
        }

        private async void mIClearAllConnectionsEntityAndAttributeCacheAndRefresh_Click(object sender, RoutedEventArgs e)
        {
            _cacheEntityMetadata.Clear();
            _cacheAttributeMetadata.Clear();

            UpdateButtonsEnable();

            await ShowExistingEntities();
        }

        private async void mIClearAllConnectionsEntityCacheAndRefresh_Click(object sender, RoutedEventArgs e)
        {
            _cacheEntityMetadata.Clear();

            UpdateButtonsEnable();

            await ShowExistingEntities();
        }

        private async void mIClearAllConnectionsAttributeCacheAndRefresh_Click(object sender, RoutedEventArgs e)
        {
            _cacheAttributeMetadata.Clear();

            UpdateButtonsEnable();

            await ShowExistingAttributes();
        }

        #endregion Clear Cache

        private async void mISaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (!IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.InConnectionSavingChangesFormat1, service.ConnectionData.Name);

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

                    UpdateStatus(service.ConnectionData, Properties.OutputStrings.InConnectionPublishingEntitiesFormat2, service.ConnectionData.Name, entityNamesOrdered);

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

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionSavingChangesCompletedFormat1, service.ConnectionData.Name);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionSavingChangesFailedFormat1, service.ConnectionData.Name);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.SavingChangesFormat1, service.ConnectionData.Name);
        }

        #region Set Attributes Properties

        private void SetItemBoolAttributeFromTag(object sender, Action<AttributeMetadataViewItem, bool> action)
        {
            if (action == null)
            {
                return;
            }

            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is bool value
            )
            {
                ExecuteOnSelectedViewItems<AttributeMetadataViewItem>(lstVwAttributes, item => action(item, value));
            }
        }

        private void mISetAttributesRequiredLevelFromTag_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is AttributeRequiredLevel value
            )
            {
                ExecuteOnSelectedViewItems<AttributeMetadataViewItem>(lstVwAttributes, item => item.RequiredLevel = value);
            }
        }

        private void mISetAttributesIsAuditEnabled_Click(object sender, RoutedEventArgs e)
        {
            SetItemBoolAttributeFromTag(sender, (item, value) => item.IsAuditEnabled = value);
        }

        private void mISetAttributesIsCustomizable_Click(object sender, RoutedEventArgs e)
        {
            SetItemBoolAttributeFromTag(sender, (item, value) => item.IsCustomizable = value);
        }

        private void mISetAttributesIsRenameable_Click(object sender, RoutedEventArgs e)
        {
            SetItemBoolAttributeFromTag(sender, (item, value) => item.IsRenameable = value);
        }

        private void mISetAttributesIsValidForAdvancedFind_Click(object sender, RoutedEventArgs e)
        {
            SetItemBoolAttributeFromTag(sender, (item, value) => item.IsValidForAdvancedFind = value);
        }

        private void mISetAttributesCanModifyAdditionalSettings_Click(object sender, RoutedEventArgs e)
        {
            SetItemBoolAttributeFromTag(sender, (item, value) => item.CanModifyAdditionalSettings = value);
        }

        private void mISetAttributesIsSecured_Click(object sender, RoutedEventArgs e)
        {
            SetItemBoolAttributeFromTag(sender, (item, value) => item.IsSecured = value);
        }

        private void mISetAttributesIsDataSourceSecret_Click(object sender, RoutedEventArgs e)
        {
            SetItemBoolAttributeFromTag(sender, (item, value) => item.IsDataSourceSecret = value);
        }

        private void mISetAttributesIsValidForForm_Click(object sender, RoutedEventArgs e)
        {
            SetItemBoolAttributeFromTag(sender, (item, value) => item.IsValidForForm = value);
        }

        private void mISetAttributesIsRequiredForForm_Click(object sender, RoutedEventArgs e)
        {
            SetItemBoolAttributeFromTag(sender, (item, value) => item.IsRequiredForForm = value);
        }

        private void mISetAttributesIsValidForGrid_Click(object sender, RoutedEventArgs e)
        {
            SetItemBoolAttributeFromTag(sender, (item, value) => item.IsValidForGrid = value);
        }

        private void mISetAttributesIsSortableEnabled_Click(object sender, RoutedEventArgs e)
        {
            SetItemBoolAttributeFromTag(sender, (item, value) => item.IsSortableEnabled = value);
        }

        private void mISetAttributesIsGlobalFilterEnabled_Click(object sender, RoutedEventArgs e)
        {
            SetItemBoolAttributeFromTag(sender, (item, value) => item.IsGlobalFilterEnabled = value);
        }

        #endregion Set Attributes Properties

        private void ExecuteSelectAttributes(bool clearCurrentSelection, Func<AttributeMetadataViewItem, bool> checker)
        {
            ExecuteSelectViewItems<AttributeMetadataViewItem>(lstVwAttributes, clearCurrentSelection, checker);
        }

        private void SelectItemBoolAttributeFromTag(object sender, bool clearCurrentSelection, Func<AttributeMetadataViewItem, bool, bool> checker)
        {
            if (checker == null)
            {
                return;
            }

            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is bool value
            )
            {
                ExecuteSelectAttributes(clearCurrentSelection, item => checker(item, value));
            }
        }

        #region Select Attributes

        private void miSelectAttributeRequiredLevelFromTag_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is AttributeRequiredLevel value
            )
            {
                ExecuteSelectAttributes(false, item => item.RequiredLevel == value);
            }
        }

        private void miSelectAttributeIsAuditEnabled_Click(object sender, RoutedEventArgs e)
        {
            SelectItemBoolAttributeFromTag(sender, false, (item, value) => item.IsAuditEnabled == value);
        }

        private void miSelectAttributeIsCustomizable_Click(object sender, RoutedEventArgs e)
        {
            SelectItemBoolAttributeFromTag(sender, false, (item, value) => item.IsCustomizable == value);
        }

        private void miSelectAttributeIsRenameable_Click(object sender, RoutedEventArgs e)
        {
            SelectItemBoolAttributeFromTag(sender, false, (item, value) => item.IsRenameable == value);
        }

        private void miSelectAttributeIsValidForAdvancedFind_Click(object sender, RoutedEventArgs e)
        {
            SelectItemBoolAttributeFromTag(sender, false, (item, value) => item.IsValidForAdvancedFind == value);
        }

        private void miSelectAttributeCanModifyAdditionalSettings_Click(object sender, RoutedEventArgs e)
        {
            SelectItemBoolAttributeFromTag(sender, false, (item, value) => item.CanModifyAdditionalSettings == value);
        }

        private void miSelectAttributeIsSecured_Click(object sender, RoutedEventArgs e)
        {
            SelectItemBoolAttributeFromTag(sender, false, (item, value) => item.IsSecured == value);
        }

        private void miSelectAttributeIsDataSourceSecret_Click(object sender, RoutedEventArgs e)
        {
            SelectItemBoolAttributeFromTag(sender, false, (item, value) => item.IsDataSourceSecret == value);
        }

        private void miSelectAttributeIsValidForForm_Click(object sender, RoutedEventArgs e)
        {
            SelectItemBoolAttributeFromTag(sender, false, (item, value) => item.IsValidForForm == value);
        }

        private void miSelectAttributeIsRequiredForForm_Click(object sender, RoutedEventArgs e)
        {
            SelectItemBoolAttributeFromTag(sender, false, (item, value) => item.IsRequiredForForm == value);
        }

        private void miSelectAttributeIsValidForGrid_Click(object sender, RoutedEventArgs e)
        {
            SelectItemBoolAttributeFromTag(sender, false, (item, value) => item.IsValidForGrid == value);
        }

        private void miSelectAttributeIsSortableEnabled_Click(object sender, RoutedEventArgs e)
        {
            SelectItemBoolAttributeFromTag(sender, false, (item, value) => item.IsSortableEnabled == value);
        }

        private void miSelectAttributeIsGlobalFilterEnabled_Click(object sender, RoutedEventArgs e)
        {
            SelectItemBoolAttributeFromTag(sender, false, (item, value) => item.IsGlobalFilterEnabled == value);
        }

        private void miSelectCustomAttributes_Click(object sender, RoutedEventArgs e)
        {
            SelectItemBoolAttributeFromTag(sender, false, (item, value) => item.IsCustomAttribute == value);
        }

        private async void miSelectAttributesOnForms_Click(object sender, RoutedEventArgs e)
        {
            await SelectAttributesOnForms(false);
        }

        private async Task SelectAttributesOnForms(bool clearCurrentSelection)
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

            var list = await repository.GetListAsync(entityLogicalName, null, null, new ColumnSet(SystemForm.Schema.Attributes.formxml));

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
                ExecuteSelectAttributes(clearCurrentSelection, item => hashAttributes.Contains(item.LogicalName));
            });
        }

        #endregion Select Attributes

        #region Select Only Attributes

        private void miSelectOnlyAttributeRequiredLevelFromTag_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is AttributeRequiredLevel value
            )
            {
                ExecuteSelectAttributes(true, item => item.RequiredLevel == value);
            }
        }

        private void miSelectOnlyAttributeIsAuditEnabled_Click(object sender, RoutedEventArgs e)
        {
            SelectItemBoolAttributeFromTag(sender, true, (item, value) => item.IsAuditEnabled == value);
        }

        private void miSelectOnlyAttributeIsCustomizable_Click(object sender, RoutedEventArgs e)
        {
            SelectItemBoolAttributeFromTag(sender, true, (item, value) => item.IsCustomizable == value);
        }

        private void miSelectOnlyAttributeIsRenameable_Click(object sender, RoutedEventArgs e)
        {
            SelectItemBoolAttributeFromTag(sender, true, (item, value) => item.IsRenameable == value);
        }

        private void miSelectOnlyAttributeIsValidForAdvancedFind_Click(object sender, RoutedEventArgs e)
        {
            SelectItemBoolAttributeFromTag(sender, true, (item, value) => item.IsValidForAdvancedFind == value);
        }

        private void miSelectOnlyAttributeCanModifyAdditionalSettings_Click(object sender, RoutedEventArgs e)
        {
            SelectItemBoolAttributeFromTag(sender, true, (item, value) => item.CanModifyAdditionalSettings == value);
        }

        private void miSelectOnlyAttributeIsSecured_Click(object sender, RoutedEventArgs e)
        {
            SelectItemBoolAttributeFromTag(sender, true, (item, value) => item.IsSecured == value);
        }

        private void miSelectOnlyAttributeIsDataSourceSecret_Click(object sender, RoutedEventArgs e)
        {
            SelectItemBoolAttributeFromTag(sender, true, (item, value) => item.IsDataSourceSecret == value);
        }

        private void miSelectOnlyAttributeIsValidForForm_Click(object sender, RoutedEventArgs e)
        {
            SelectItemBoolAttributeFromTag(sender, true, (item, value) => item.IsValidForForm == value);
        }

        private void miSelectOnlyAttributeIsRequiredForForm_Click(object sender, RoutedEventArgs e)
        {
            SelectItemBoolAttributeFromTag(sender, true, (item, value) => item.IsRequiredForForm == value);
        }

        private void miSelectOnlyAttributeIsValidForGrid_Click(object sender, RoutedEventArgs e)
        {
            SelectItemBoolAttributeFromTag(sender, true, (item, value) => item.IsValidForGrid == value);
        }

        private void miSelectOnlyAttributeIsSortableEnabled_Click(object sender, RoutedEventArgs e)
        {
            SelectItemBoolAttributeFromTag(sender, true, (item, value) => item.IsSortableEnabled == value);
        }

        private void miSelectOnlyAttributeIsGlobalFilterEnabled_Click(object sender, RoutedEventArgs e)
        {
            SelectItemBoolAttributeFromTag(sender, true, (item, value) => item.IsGlobalFilterEnabled == value);
        }

        private void miSelectOnlyCustomAttributes_Click(object sender, RoutedEventArgs e)
        {
            SelectItemBoolAttributeFromTag(sender, true, (item, value) => item.IsCustomAttribute == value);
        }

        private async void miSelectOnlyAttributesOnForms_Click(object sender, RoutedEventArgs e)
        {
            await SelectAttributesOnForms(true);
        }

        #endregion Select Only Attributes

        #region Select Entities

        private void miSelectCustomEntities_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is bool value
            )
            {
                ExecuteSelectViewItems<EntityMetadataAuditViewItem>(lstVwEntities, false, item => item.IsCustomEntity == value);
            }
        }

        private void miSelectEntityIsAuditEnabled_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is bool value
            )
            {
                ExecuteSelectViewItems<EntityMetadataAuditViewItem>(lstVwEntities, false, item => item.IsAuditEnabled == value);
            }
        }

        #endregion Select Entities

        #region Set Entity Properties

        private void mISetEntitiesIsAuditEnabled_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is bool value
            )
            {
                ExecuteOnSelectedViewItems<EntityMetadataAuditViewItem>(lstVwEntities, item => item.IsAuditEnabled = value);
            }
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

        #region Clipboard Attribute

        private void mIClipboardAttributeCopyName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<AttributeMetadataViewItem>(e, ent => ent.LogicalName);
        }

        private void mIClipboardAttributeCopyDisplayName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<AttributeMetadataViewItem>(e, ent => ent.DisplayName);
        }

        private void mIClipboardAttributeCopyAttributeType_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<AttributeMetadataViewItem>(e, ent => ent.AttributeTypeName);
        }

        private void mIClipboardAttributeCopyAttributeId_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<AttributeMetadataViewItem>(e, ent => ent.AttributeMetadata.MetadataId.ToString());
        }

        #endregion Clipboard Attribute

        #region Clipboard Entity

        private void mIClipboardEntityCopyName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityMetadataAuditViewItem>(e, ent => ent.LogicalName);
        }

        private void mIClipboardEntityCopyDisplayName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityMetadataAuditViewItem>(e, ent => ent.DisplayName);
        }

        private void mIClipboardEntityCopyObjectTypeCode_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityMetadataAuditViewItem>(e, ent => ent.ObjectTypeCode.ToString());
        }

        private void mIClipboardEntityCopyEntityMetadataId_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityMetadataAuditViewItem>(e, ent => ent.EntityMetadata.MetadataId.ToString());
        }

        #endregion Clipboard Entity

        private void btnAttributeMetadataFilter_Click(object sender, RoutedEventArgs e)
        {
            _popupAttributeMetadataFilter.IsOpen = true;
            _popupAttributeMetadataFilter.Child.Focus();
        }

        private async void popupAttributeMetadataFilter_Closed(object sender, EventArgs e)
        {
            if (_attributeMetadataFilter.FilterChanged)
            {
                await ShowExistingAttributes();
            }
        }

        private void attributeMetadataFilter_CloseClicked(object sender, EventArgs e)
        {
            if (_popupAttributeMetadataFilter.IsOpen)
            {
                _popupAttributeMetadataFilter.IsOpen = false;
                this.Focus();
            }
        }
    }
}