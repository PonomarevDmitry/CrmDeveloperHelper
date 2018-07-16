using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
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
    public partial class WindowEntityAttributeExplorer : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private IWriteToOutput _iWriteToOutput;

        private CommonConfiguration _commonConfig;
        private ConnectionConfiguration _connectionConfig;

        private bool _controlsEnabled = true;

        private ObservableCollection<EntityMetadataViewItem> _itemsSourceEntityList;

        private ObservableCollection<AttributeMetadataViewItem> _itemsSourceAttributeList;

        private Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();
        private Dictionary<Guid, SolutionComponentDescriptor> _descriptorCache = new Dictionary<Guid, SolutionComponentDescriptor>();

        private Dictionary<Guid, IEnumerable<EntityMetadataViewItem>> _cacheEntityMetadata = new Dictionary<Guid, IEnumerable<EntityMetadataViewItem>>();

        private Dictionary<Guid, Dictionary<string, IEnumerable<AttributeMetadataViewItem>>> _cacheAttributeMetadata = new Dictionary<Guid, Dictionary<string, IEnumerable<AttributeMetadataViewItem>>>();

        private int _init = 0;

        public WindowEntityAttributeExplorer(
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
                _cacheEntityMetadata[service.ConnectionData.ConnectionId] = allEntities.Select(e => new EntityMetadataViewItem(e)).ToList();
            }

            BindingOperations.EnableCollectionSynchronization(_connectionConfig.Connections, sysObjectConnections);

            InitializeComponent();

            LoadFromConfig();

            txtBFilterEnitity.Text = filterEntity;
            txtBFilterEnitity.SelectionLength = 0;
            txtBFilterEnitity.SelectionStart = txtBFilterEnitity.Text.Length;

            txtBFilterEnitity.Focus();

            _itemsSourceEntityList = new ObservableCollection<EntityMetadataViewItem>();
            lstVwEntities.ItemsSource = _itemsSourceEntityList;

            _itemsSourceAttributeList = new ObservableCollection<AttributeMetadataViewItem>();
            lstVwAttributes.ItemsSource = _itemsSourceAttributeList;

            UpdateButtonsEnable();

            cmBCurrentConnection.ItemsSource = _connectionConfig.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            _init--;

            if (allEntities != null)
            {
                var list = _cacheEntityMetadata[service.ConnectionData.ConnectionId].AsEnumerable();

                list = FilterEntityList(list, filterEntity);

                LoadEntities(list);
            }
            else if (service != null)
            {
                ShowExistingEntities();
            }
        }

        private void LoadFromConfig()
        {
            WindowSettings winConfig = GetWindowsSettings();

            LoadFormSettings(winConfig);
        }

        protected override void LoadConfigurationInternal(WindowSettings winConfig)
        {
            base.LoadConfigurationInternal(winConfig);

            LoadFormSettings(winConfig);
        }

        private const string paramColumnEntityWidth = "ColumnEntityWidth";
        private const string paramColumnAttributeWidth = "ColumnAttributeWidth";

        private void LoadFormSettings(WindowSettings winConfig)
        {
            if (winConfig.DictDouble.ContainsKey(paramColumnEntityWidth)
                && winConfig.DictDouble.ContainsKey(paramColumnAttributeWidth)
                )
            {
                var widthEntity = winConfig.DictDouble[paramColumnEntityWidth];
                var widthAttribute = winConfig.DictDouble[paramColumnAttributeWidth];

                if (widthEntity + widthAttribute > 0)
                {
                    var tempParamerts = widthEntity * 100 / (widthEntity + widthAttribute);
                    var tempFetchText = widthAttribute * 100 / (widthEntity + widthAttribute);

                    columnEntity.Width = new GridLength(tempParamerts, GridUnitType.Star);
                    columnAttribute.Width = new GridLength(tempFetchText, GridUnitType.Star);
                }
            }
        }

        protected override void SaveConfigurationInternal(WindowSettings winConfig)
        {
            base.SaveConfigurationInternal(winConfig);

            winConfig.DictDouble[paramColumnEntityWidth] = columnEntity.Width.Value;
            winConfig.DictDouble[paramColumnAttributeWidth] = columnAttribute.Width.Value;
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

            _itemsSourceEntityList.Clear();
            _itemsSourceAttributeList.Clear();

            IEnumerable<EntityMetadataViewItem> list = Enumerable.Empty<EntityMetadataViewItem>();

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

                        _cacheEntityMetadata.Add(service.ConnectionData.ConnectionId, temp.Select(e => new EntityMetadataViewItem(e)).ToList());
                    }

                    list = _cacheEntityMetadata[service.ConnectionData.ConnectionId];
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }

            string textName = string.Empty;

            txtBFilterEnitity.Dispatcher.Invoke(() =>
            {
                textName = txtBFilterEnitity.Text.Trim().ToLower();
            });

            list = FilterEntityList(list, textName);

            LoadEntities(list);
        }

        private static IEnumerable<EntityMetadataViewItem> FilterEntityList(IEnumerable<EntityMetadataViewItem> list, string textName)
        {
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
                            ent.LogicalName.ToLower().Contains(textName)
                            || (ent.DisplayName != null && ent.EntityMetadata.DisplayName.LocalizedLabels
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

        private void LoadEntities(IEnumerable<EntityMetadataViewItem> results)
        {
            this._iWriteToOutput.WriteToOutput("Found {0} entity(ies).", results.Count());

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

            UpdateStatus(string.Format("{0} entities loaded.", results.Count()));

            ToggleControls(true);

            ShowExistingAttributes();
        }

        private async Task ShowExistingAttributes()
        {
            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false);

            UpdateStatus("Loading attributes...");

            string entityLogicalName = string.Empty;

            this.Dispatcher.Invoke(() =>
            {
                _itemsSourceAttributeList.Clear();

                if (this.lstVwEntities.SelectedItems.Count == 1
                    && this.lstVwEntities.SelectedItems[0] != null
                    && this.lstVwEntities.SelectedItems[0] is EntityMetadataViewItem
                )
                {
                    entityLogicalName = (this.lstVwEntities.SelectedItems[0] as EntityMetadataViewItem).LogicalName;
                }
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
                            _cacheAttributeMetadata.Add(service.ConnectionData.ConnectionId, new Dictionary<string, IEnumerable<AttributeMetadataViewItem>>(StringComparer.InvariantCultureIgnoreCase));
                        }

                        var cacheAttribute = _cacheAttributeMetadata[service.ConnectionData.ConnectionId];

                        if (!cacheAttribute.ContainsKey(entityLogicalName))
                        {
                            var repository = new EntityMetadataRepository(service);

                            var metadata = await repository.GetEntityMetadataAttributesAsync(entityLogicalName, EntityFilters.Attributes);

                            if (metadata != null && metadata.Attributes != null)
                            {
                                cacheAttribute.Add(entityLogicalName, metadata.Attributes.Where(e => string.IsNullOrEmpty(e.AttributeOf)).Select(e => new AttributeMetadataViewItem(e)).ToList());
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
                    this._iWriteToOutput.WriteErrorToOutput(ex);
                }
            }

            string textName = string.Empty;

            txtBFilterAttribute.Dispatcher.Invoke(() =>
            {
                textName = txtBFilterAttribute.Text.Trim().ToLower();
            });

            list = FilterAttributeList(list, textName);

            LoadAttributes(list);
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
                        ent.LogicalName.ToLower().Contains(textName)
                        || (ent.DisplayName != null && ent.AttributeMetadata.DisplayName.LocalizedLabels
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

            return list;
        }

        private void LoadAttributes(IEnumerable<AttributeMetadataViewItem> results)
        {
            this._iWriteToOutput.WriteToOutput("Found {0} attributes.", results.Count());

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

            UpdateStatus(string.Format("{0} attributes loaded.", results.Count()));

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

            ToggleControl(cmBCurrentConnection, enabled);

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
            this.lstVwEntities.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this._controlsEnabled && this.lstVwEntities.SelectedItems.Count > 0;

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

        private void txtBFilterAttribute_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowExistingAttributes();
            }
        }

        private EntityMetadataViewItem GetSelectedEntity()
        {
            EntityMetadataViewItem result = null;

            if (this.lstVwEntities.SelectedItems.Count > 0
                && this.lstVwEntities.SelectedItems[0] != null
                && this.lstVwEntities.SelectedItems[0] is EntityMetadataViewItem
                )
            {
                result = (this.lstVwEntities.SelectedItems[0] as EntityMetadataViewItem);
            }

            return result;
        }

        private List<EntityMetadataViewItem> GetSelectedEntities()
        {
            List<EntityMetadataViewItem> result = this.lstVwEntities.SelectedItems.OfType<EntityMetadataViewItem>().ToList();

            return result;
        }

        private AttributeMetadataViewItem GetSelectedAttribute()
        {
            AttributeMetadataViewItem result = null;

            if (this.lstVwAttributes.SelectedItems.Count > 0
                && this.lstVwAttributes.SelectedItems[0] != null
                && this.lstVwAttributes.SelectedItems[0] is AttributeMetadataViewItem
                )
            {
                result = (this.lstVwAttributes.SelectedItems[0] as AttributeMetadataViewItem);
            }

            return result;
        }

        private List<AttributeMetadataViewItem> GetSelectedAttributes()
        {
            List<AttributeMetadataViewItem> result = this.lstVwAttributes.SelectedItems.OfType<AttributeMetadataViewItem>().ToList();

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
                entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId].Select(i => i.EntityMetadata).ToList();
            }

            WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, entityMetadataList, null);
        }

        private async void btnExportRibbon_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            IEnumerable<EntityMetadata> entityMetadataList = null;

            if (_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId].Select(i => i.EntityMetadata).ToList();
            }

            WindowHelper.OpenEntityRibbonWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, entityMetadataList);
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

            WindowHelper.OpenSystemFormWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
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

        private async void btnAttributesDependentComponent_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            IEnumerable<EntityMetadata> entityMetadataList = null;

            if (_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId].Select(i => i.EntityMetadata).ToList();
            }

            WindowHelper.OpenAttributesDependentComponentWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, entityMetadataList);
        }

        private async void btnPluginTree_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenPluginTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName);
        }

        private async void btnMessageTree_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName);
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

            }
        }

        private async Task ExecuteActionAsync(IEnumerable<string> entityNames, Func<IEnumerable<string>, Task> action)
        {
            if (!_controlsEnabled)
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
            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false);

            var entityNamesOrdered = string.Join(",", entityNames.OrderBy(s => s));

            try
            {
                UpdateStatus(string.Format("Publishing Entity {0}...", entityNames.Count()));

                this._iWriteToOutput.WriteToOutput("Start publishing entities {0} at {1}", entityNamesOrdered, DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

                var service = await GetService();

                var repository = new PublishActionsRepository(service);

                await repository.PublishEntitiesAsync(entityNames);

                this._iWriteToOutput.WriteToOutput("End publishing entity {0} at {1}", entityNamesOrdered, DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

                UpdateStatus(string.Format("Entities {0} published", entityNamesOrdered));
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                UpdateStatus(string.Format("Publish Entity {0} failed", entityNamesOrdered));
            }
            finally
            {
                ToggleControls(true);
            }
        }

        private void lstVwEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowExistingAttributes();

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

        private void mIOpenAttributeInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            var attribute = GetSelectedAttribute();

            if (entity == null || attribute == null)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

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

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenEntityListInWeb(entity.LogicalName);
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
            var entityList = GetSelectedEntities();

            if (entityList == null || !entityList.Any())
            {
                return;
            }

            _commonConfig.Save();

            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData != null)
            {
                var backWorker = new Thread(() =>
                {
                    try
                    {
                        this._iWriteToOutput.ActivateOutputWindow();

                        var contr = new SolutionController(this._iWriteToOutput);

                        contr.ExecuteAddingComponentesIntoSolution(connectionData, _commonConfig, solutionUniqueName, ComponentType.Entity, entityList.Select(item => item.EntityMetadata.MetadataId.Value).ToList(), null, withSelect);
                    }
                    catch (Exception ex)
                    {
                        this._iWriteToOutput.WriteErrorToOutput(ex);
                    }
                });

                backWorker.Start();
            }
        }

        private void AddAttributeIntoCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            AddAttributeIntoSolution(true, null);
        }

        private void AddAttributeIntoCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                AddAttributeIntoSolution(false, solutionUniqueName);
            }
        }

        private void AddAttributeIntoSolution(bool withSelect, string solutionUniqueName)
        {
            var attributeList = GetSelectedAttributes();

            if (attributeList == null || !attributeList.Any())
            {
                return;
            }

            _commonConfig.Save();

            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData != null)
            {
                var backWorker = new Thread(() =>
                {
                    try
                    {
                        this._iWriteToOutput.ActivateOutputWindow();

                        var contr = new SolutionController(this._iWriteToOutput);

                        contr.ExecuteAddingComponentesIntoSolution(connectionData, _commonConfig, solutionUniqueName, ComponentType.Attribute, attributeList.Select(item => item.AttributeMetadata.MetadataId.Value).ToList(), null, withSelect);
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

        private void ContextMenuAttribute_Opened(object sender, RoutedEventArgs e)
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

                            menuItem.Click += AddAttributeIntoCrmSolutionLast_Click;

                            lastSolution.Items.Add(menuItem);
                        }
                    }
                }
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

            WindowHelper.OpenSolutionComponentDependenciesWindow(_iWriteToOutput, service, descriptor, _commonConfig, (int)ComponentType.Entity, entity.EntityMetadata.MetadataId.Value, null);
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

        private void mIAttributeOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            var attribute = GetSelectedAttribute();

            if (attribute == null)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.Attribute, attribute.AttributeMetadata.MetadataId.Value);
            }
        }

        private async void mIAttributeOpenDependentComponentsInWindow_Click(object sender, RoutedEventArgs e)
        {
            var attribute = GetSelectedAttribute();

            if (attribute == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();
            var descriptor = await GetDescriptor();

            WindowHelper.OpenSolutionComponentDependenciesWindow(_iWriteToOutput, service, descriptor, _commonConfig, (int)ComponentType.Attribute, attribute.AttributeMetadata.MetadataId.Value, null);
        }

        private async void mIAttributeOpenSolutionsContainingComponentInWindow_Click(object sender, RoutedEventArgs e)
        {
            var attribute = GetSelectedAttribute();

            if (attribute == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenExplorerSolutionWindow(
                _iWriteToOutput
                , service
                , _commonConfig
                , (int)ComponentType.Attribute
                , attribute.AttributeMetadata.MetadataId.Value
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
                this._itemsSourceEntityList.Clear();
                this._itemsSourceAttributeList.Clear();
            });

            if (!_controlsEnabled)
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

        private void mIClearAttributeCacheAndRefresh_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData != null)
            {
                _cacheAttributeMetadata.Remove(connectionData.ConnectionId);

                UpdateButtonsEnable();

                ShowExistingAttributes();
            }
        }

        private async void mISaveChanges(object sender, RoutedEventArgs e)
        {
            if (!_controlsEnabled)
            {
                return;
            }

            ToggleControls(false);

            UpdateStatus("Saving Changes...");

            this._iWriteToOutput.WriteToOutput("Start saving changes at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

            HashSet<string> listForPublish = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            try
            {
                var service = await GetService();

                if (service != null)
                {
                    var listEntitiesToChange = new List<EntityMetadataViewItem>();
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
                        this._iWriteToOutput.WriteToOutput("Updating Attributes:");

                        foreach (var attribute in listAttributesToChange.OrderBy(a => a.AttributeMetadata.EntityLogicalName).ThenBy(a => a.LogicalName))
                        {
                            this._iWriteToOutput.WriteToOutput("    {0}.{1}", attribute.AttributeMetadata.EntityLogicalName, attribute.LogicalName);

                            listForPublish.Add(attribute.AttributeMetadata.EntityLogicalName);

                            try
                            {
                                await repository.UpdateAttributeMetadataAsync(attribute.AttributeMetadata);
                            }
                            catch (Exception ex)
                            {
                                _iWriteToOutput.WriteErrorToOutput(ex);
                            }
                        }
                    }

                    if (listEntitiesToChange.Any())
                    {
                        this._iWriteToOutput.WriteToOutput("Updating Entities:");

                        foreach (var entityMetadata in listEntitiesToChange.OrderBy(a => a.LogicalName))
                        {
                            this._iWriteToOutput.WriteToOutput("    {0}", entityMetadata.LogicalName);

                            listForPublish.Add(entityMetadata.LogicalName);

                            try
                            {
                                await repository.UpdateEntityMetadataAsync(entityMetadata.EntityMetadata);
                            }
                            catch (Exception ex)
                            {
                                _iWriteToOutput.WriteErrorToOutput(ex);
                            }
                        }
                    }

                    if (listForPublish.Any())
                    {
                        var entityNamesOrdered = string.Join(",", listForPublish.OrderBy(s => s));

                        UpdateStatus(string.Format("Publishing Entity {0}...", listForPublish.Count()));

                        this._iWriteToOutput.WriteToOutput("Start publishing entities {0} at {1}", entityNamesOrdered, DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));

                        var repositoryPublish = new PublishActionsRepository(service);

                        await repositoryPublish.PublishEntitiesAsync(listForPublish);

                        this._iWriteToOutput.WriteToOutput("End publishing entity {0} at {1}", entityNamesOrdered, DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
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
                            _iWriteToOutput.WriteErrorToOutput(ex);
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
                            _iWriteToOutput.WriteErrorToOutput(ex);
                        }
                    }

                    this._iWriteToOutput.WriteToOutput("End saving changes at {0}", DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
                }

                UpdateStatus("Changes saved.");
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                UpdateStatus("Saving Changes failed.");
            }
            finally
            {
                ToggleControls(true);
            }
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
                if (this.lstVwEntities.SelectedItems.Count == 1
                    && this.lstVwEntities.SelectedItems[0] != null
                    && this.lstVwEntities.SelectedItems[0] is EntityMetadataViewItem
                )
                {
                    entityLogicalName = (this.lstVwEntities.SelectedItems[0] as EntityMetadataViewItem).LogicalName;
                }
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

            var list = await repository.GetListAsync(entityLogicalName, new ColumnSet(SystemForm.Schema.Attributes.formxml));

            var hashAttributes = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var form in list)
            {
                if (!string.IsNullOrEmpty(form.FormXml) && ContentCoparerHelper.TryParseXml(form.FormXml, out var doc))
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

        private void ExecuteSelectEntities(Func<EntityMetadataViewItem, bool> checker)
        {
            var list = lstVwEntities.Items.OfType<EntityMetadataViewItem>().Where(i => checker(i)).ToList();

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

        private void ExecuteOnSelectedEntities(Action<EntityMetadataViewItem> action)
        {
            var list = lstVwEntities.SelectedItems.OfType<EntityMetadataViewItem>().ToList();

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
    }
}