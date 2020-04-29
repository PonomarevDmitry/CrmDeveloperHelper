using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription;
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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExplorerEntityPrivileges : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private readonly Popup _popupEntityMetadataFilter;
        private readonly EntityMetadataFilter _entityMetadataFilter;

        private readonly IWriteToOutput _iWriteToOutput;

        private readonly CommonConfiguration _commonConfig;

        private readonly ObservableCollection<EntityMetadataListViewItem> _itemsSourceEntityList;

        private readonly ObservableCollection<RoleEntityPrivilegesViewItem> _itemsSourceSecurityRoleList;

        private readonly Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();

        private readonly Dictionary<Guid, IEnumerable<EntityMetadata>> _cacheEntityMetadata = new Dictionary<Guid, IEnumerable<EntityMetadata>>();

        private readonly Dictionary<PrivilegeType, MenuItem> _menuItemsSetPrivileges;
        private readonly Dictionary<PrivilegeDepth, MenuItem> _menuItemsSetPrivilegesDepthsAll;
        private readonly Dictionary<PrivilegeType, Dictionary<PrivilegeDepth, MenuItem>> _menuItemsSetPrivilegesDepths;

        public WindowExplorerEntityPrivileges(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , IEnumerable<EntityMetadata> entityMetadataList
            , string filterEntity
        )
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;

            _connectionCache[service.ConnectionData.ConnectionId] = service;

            if (entityMetadataList != null && entityMetadataList.Any(e => e.Privileges != null && e.Privileges.Any()))
            {
                _cacheEntityMetadata[service.ConnectionData.ConnectionId] = entityMetadataList;
            }

            BindingOperations.EnableCollectionSynchronization(service.ConnectionData.ConnectionConfiguration.Connections, sysObjectConnections);

            InitializeComponent();

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

            FillRoleEditorLayoutTabs();

            LoadFromConfig();

            txtBFilterEnitity.Text = filterEntity;
            txtBFilterEnitity.SelectionLength = 0;
            txtBFilterEnitity.SelectionStart = txtBFilterEnitity.Text.Length;

            txtBFilterEnitity.Focus();

            _itemsSourceEntityList = new ObservableCollection<EntityMetadataListViewItem>();
            lstVwEntities.ItemsSource = _itemsSourceEntityList;

            _itemsSourceSecurityRoleList = new ObservableCollection<RoleEntityPrivilegesViewItem>();
            lstVwSecurityRoles.ItemsSource = _itemsSourceSecurityRoleList;

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            this._menuItemsSetPrivilegesDepthsAll = new Dictionary<PrivilegeDepth, MenuItem>()
            {
                { PrivilegeDepth.Basic, mISetAttributeAllBasic }
                , { PrivilegeDepth.Local, mISetAttributeAllLocal }
                , { PrivilegeDepth.Deep, mISetAttributeAllDeep }
                , { PrivilegeDepth.Global, mISetAttributeAllGlobal }
            };

            this._menuItemsSetPrivilegesDepths = new Dictionary<PrivilegeType, Dictionary<PrivilegeDepth, MenuItem>>()
            {
                {
                    PrivilegeType.Create, new Dictionary<PrivilegeDepth, MenuItem>()
                    {
                        { PrivilegeDepth.Basic, mISetAttributeCreateBasic }
                        , { PrivilegeDepth.Local, mISetAttributeCreateLocal }
                        , { PrivilegeDepth.Deep, mISetAttributeCreateDeep }
                        , { PrivilegeDepth.Global, mISetAttributeCreateGlobal }
                    }
                },

                {
                    PrivilegeType.Read, new Dictionary<PrivilegeDepth, MenuItem>()
                    {
                        { PrivilegeDepth.Basic, mISetAttributeReadBasic }
                        , { PrivilegeDepth.Local, mISetAttributeReadLocal }
                        , { PrivilegeDepth.Deep, mISetAttributeReadDeep }
                        , { PrivilegeDepth.Global, mISetAttributeReadGlobal }
                    }
                },

                {
                    PrivilegeType.Write, new Dictionary<PrivilegeDepth, MenuItem>()
                    {
                        { PrivilegeDepth.Basic, mISetAttributeUpdateBasic }
                        , { PrivilegeDepth.Local, mISetAttributeUpdateLocal }
                        , { PrivilegeDepth.Deep, mISetAttributeUpdateDeep }
                        , { PrivilegeDepth.Global, mISetAttributeUpdateGlobal }
                    }
                },

                {
                    PrivilegeType.Delete, new Dictionary<PrivilegeDepth, MenuItem>()
                    {
                        { PrivilegeDepth.Basic, mISetAttributeDeleteBasic }
                        , { PrivilegeDepth.Local, mISetAttributeDeleteLocal }
                        , { PrivilegeDepth.Deep, mISetAttributeDeleteDeep }
                        , { PrivilegeDepth.Global, mISetAttributeDeleteGlobal }
                    }
                },

                {
                    PrivilegeType.Append, new Dictionary<PrivilegeDepth, MenuItem>()
                    {
                        { PrivilegeDepth.Basic, mISetAttributeAppendBasic }
                        , { PrivilegeDepth.Local, mISetAttributeAppendLocal }
                        , { PrivilegeDepth.Deep, mISetAttributeAppendDeep }
                        , { PrivilegeDepth.Global, mISetAttributeAppendGlobal }
                    }
                },

                {
                    PrivilegeType.AppendTo, new Dictionary<PrivilegeDepth, MenuItem>()
                    {
                        { PrivilegeDepth.Basic, mISetAttributeAppendToBasic }
                        , { PrivilegeDepth.Local, mISetAttributeAppendToLocal }
                        , { PrivilegeDepth.Deep, mISetAttributeAppendToDeep }
                        , { PrivilegeDepth.Global, mISetAttributeAppendToGlobal }
                    }
                },

                {
                    PrivilegeType.Assign, new Dictionary<PrivilegeDepth, MenuItem>()
                    {
                        { PrivilegeDepth.Basic, mISetAttributeAssignBasic }
                        , { PrivilegeDepth.Local, mISetAttributeAssignLocal }
                        , { PrivilegeDepth.Deep, mISetAttributeAssignDeep }
                        , { PrivilegeDepth.Global, mISetAttributeAssignGlobal }
                    }
                },

                {
                    PrivilegeType.Share, new Dictionary<PrivilegeDepth, MenuItem>()
                    {
                        { PrivilegeDepth.Basic, mISetAttributeShareBasic }
                        , { PrivilegeDepth.Local, mISetAttributeShareLocal }
                        , { PrivilegeDepth.Deep, mISetAttributeShareDeep }
                        , { PrivilegeDepth.Global, mISetAttributeShareGlobal }
                    }
                },
            };

            this._menuItemsSetPrivileges = new Dictionary<PrivilegeType, MenuItem>()
            {
                { PrivilegeType.Create, mISetPrivilegeCreate }

                , { PrivilegeType.Read, mISetPrivilegeRead }

                , { PrivilegeType.Write, mISetPrivilegeUpdate }

                , { PrivilegeType.Delete, mISetPrivilegeDelete }

                , { PrivilegeType.Append, mISetPrivilegeAppend }

                , { PrivilegeType.AppendTo, mISetPrivilegeAppendTo }

                , { PrivilegeType.Assign, mISetPrivilegeAssign }

                , { PrivilegeType.Share, mISetPrivilegeShare }
            };

            this.DecreaseInit();

            ShowExistingEntities();
        }

        private void LoadFromConfig()
        {
            WindowSettings winConfig = GetWindowsSettings();

            LoadFormSettings(winConfig);
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

            try
            {
                var service = await QuickConnection.ConnectAndWriteToOutputAsync(_iWriteToOutput, connectionData);

                if (service != null)
                {
                    _connectionCache[connectionData.ConnectionId] = service;
                }

                return service;
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

        private async Task ShowExistingEntities()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.LoadingEntities);

            _itemsSourceEntityList.Clear();
            _itemsSourceSecurityRoleList.Clear();

            IEnumerable<EntityMetadataListViewItem> list = Enumerable.Empty<EntityMetadataListViewItem>();

            try
            {
                if (service != null)
                {
                    if (!_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
                    {
                        EntityMetadataRepository repository = new EntityMetadataRepository(service);

                        var temp = await repository.GetEntitiesDisplayNameWithPrivilegesAsync();

                        _cacheEntityMetadata.Add(service.ConnectionData.ConnectionId, temp);
                    }

                    list = _cacheEntityMetadata[service.ConnectionData.ConnectionId].Select(e => new EntityMetadataListViewItem(e)).ToList();
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

            ShowEntitySecurityRoles();
        }

        private IEnumerable<EntityMetadataListViewItem> FilterEntityList(IEnumerable<EntityMetadataListViewItem> list, string textName, RoleEditorLayoutTab selectedTab)
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

        private void LoadEntities(IEnumerable<EntityMetadataListViewItem> results)
        {
            this.lstVwEntities.Dispatcher.Invoke(() =>
            {
                foreach (var entity in results
                    .OrderBy(s => s.IsIntersect)
                    .ThenBy(s => s.LogicalName)
                )
                {
                    _itemsSourceEntityList.Add(entity);
                }

                if (this.lstVwEntities.Items.Count == 1)
                {
                    this.lstVwEntities.SelectedItem = this.lstVwEntities.Items[0];
                }
            });
        }

        private async Task ShowEntitySecurityRoles()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            EntityMetadata entityMetadata = null;

            this.Dispatcher.Invoke(() =>
            {
                foreach (var entity in _itemsSourceSecurityRoleList)
                {
                    entity.PropertyChanged -= Entity_PropertyChanged;
                    entity.PropertyChanged -= Entity_PropertyChanged;
                }

                _itemsSourceSecurityRoleList.Clear();

                entityMetadata = GetSelectedEntity()?.EntityMetadata;

                foreach (var menuItem in _menuItemsSetPrivileges.Values)
                {
                    menuItem.IsEnabled = false;
                    menuItem.Visibility = Visibility.Collapsed;
                }

                foreach (var menuItem in _menuItemsSetPrivilegesDepthsAll.Values)
                {
                    menuItem.IsEnabled = false;
                    menuItem.Visibility = Visibility.Collapsed;
                }

                foreach (var privDic in _menuItemsSetPrivilegesDepths.Values)
                {
                    foreach (var menuItem in privDic.Values)
                    {
                        menuItem.IsEnabled = false;
                        menuItem.Visibility = Visibility.Collapsed;
                    }
                }
            });

            if (entityMetadata == null)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.LoadingEntityPrivileges);

            string textName = string.Empty;

            txtBFilterSecurityRole.Dispatcher.Invoke(() =>
            {
                textName = txtBFilterSecurityRole.Text.Trim().ToLower();
            });

            IEnumerable<RoleEntityPrivilegesViewItem> list = Enumerable.Empty<RoleEntityPrivilegesViewItem>();

            try
            {
                if (service != null)
                {
                    if (entityMetadata.Privileges != null && entityMetadata.Privileges.Any())
                    {
                        RoleRepository repositoryRole = new RoleRepository(service);

                        var roles = await repositoryRole.GetListAsync(textName,
                        new ColumnSet(
                            Role.Schema.Attributes.name
                            , Role.Schema.Attributes.businessunitid
                            , Role.Schema.Attributes.ismanaged
                            , Role.Schema.Attributes.iscustomizable
                        ));

                        var repository = new RolePrivilegesRepository(service);

                        var task = repository.GetEntityPrivilegesAsync(roles.Select(r => r.RoleId.Value), entityMetadata.Privileges?.Select(p => p.PrivilegeId));

                        ActivateMenuItemsSetPrivileges(entityMetadata.Privileges);

                        var listRolePrivilege = await task;

                        list = roles.Select(r => new RoleEntityPrivilegesViewItem(r, listRolePrivilege.Where(rp => rp.RoleId == r.RoleId), entityMetadata.Privileges)).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            LoadSecurityRoles(list);

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.LoadingEntityPrivilegesCompletedFormat1, list.Count());
        }

        private void ActivateMenuItemsSetPrivileges(SecurityPrivilegeMetadata[] privileges)
        {
            this.Dispatcher.Invoke(() =>
            {
                foreach (var priv in privileges)
                {
                    if (_menuItemsSetPrivileges.ContainsKey(priv.PrivilegeType))
                    {
                        _menuItemsSetPrivileges[priv.PrivilegeType].IsEnabled = true;
                        _menuItemsSetPrivileges[priv.PrivilegeType].Visibility = Visibility.Visible;
                    }

                    if (priv.CanBeBasic)
                    {
                        _menuItemsSetPrivilegesDepthsAll[PrivilegeDepth.Basic].IsEnabled = true;
                        _menuItemsSetPrivilegesDepthsAll[PrivilegeDepth.Basic].Visibility = Visibility.Visible;
                    }

                    if (priv.CanBeLocal)
                    {
                        _menuItemsSetPrivilegesDepthsAll[PrivilegeDepth.Local].IsEnabled = true;
                        _menuItemsSetPrivilegesDepthsAll[PrivilegeDepth.Local].Visibility = Visibility.Visible;
                    }

                    if (priv.CanBeDeep)
                    {
                        _menuItemsSetPrivilegesDepthsAll[PrivilegeDepth.Deep].IsEnabled = true;
                        _menuItemsSetPrivilegesDepthsAll[PrivilegeDepth.Deep].Visibility = Visibility.Visible;
                    }

                    if (priv.CanBeGlobal)
                    {
                        _menuItemsSetPrivilegesDepthsAll[PrivilegeDepth.Global].IsEnabled = true;
                        _menuItemsSetPrivilegesDepthsAll[PrivilegeDepth.Global].Visibility = Visibility.Visible;
                    }

                    if (_menuItemsSetPrivilegesDepths.ContainsKey(priv.PrivilegeType))
                    {
                        var dict = _menuItemsSetPrivilegesDepths[priv.PrivilegeType];

                        dict[PrivilegeDepth.Basic].IsEnabled = priv.CanBeBasic;
                        dict[PrivilegeDepth.Local].IsEnabled = priv.CanBeLocal;
                        dict[PrivilegeDepth.Deep].IsEnabled = priv.CanBeDeep;
                        dict[PrivilegeDepth.Global].IsEnabled = priv.CanBeGlobal;

                        foreach (var menuItem in dict.Values)
                        {
                            menuItem.Visibility = menuItem.IsEnabled ? Visibility.Visible : Visibility.Collapsed;
                        }
                    }
                }

                menuSetPrivilege.IsEnabled = _menuItemsSetPrivileges.Values.Any(m => m.IsEnabled);
            });
        }

        private void LoadSecurityRoles(IEnumerable<RoleEntityPrivilegesViewItem> results)
        {
            this.lstVwSecurityRoles.Dispatcher.Invoke(() =>
            {
                foreach (var entity in results
                    .OrderBy(s => s.RoleName)
                    .ThenBy(s => s.RoleTemplateName)
                    .ThenBy(s => s.Role.Id)
                )
                {
                    entity.PropertyChanged -= Entity_PropertyChanged;
                    entity.PropertyChanged -= Entity_PropertyChanged;
                    entity.PropertyChanged += Entity_PropertyChanged;

                    _itemsSourceSecurityRoleList.Add(entity);
                }

                if (this.lstVwSecurityRoles.Items.Count == 1)
                {
                    this.lstVwSecurityRoles.SelectedItem = this.lstVwSecurityRoles.Items[0];
                }
            });
        }

        private void Entity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "IsChanged", StringComparison.InvariantCultureIgnoreCase))
            {
                UpdateRoleButtons();
            }
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

            ToggleControl(this.tSProgressBar
                , cmBCurrentConnection
                , btnSetCurrentConnection

                , btnRefreshEntites
                , btnClearEntityCacheAndRefresh
                , btnRefreshRoles
            );

            ToggleControl(IsControlsEnabled && _menuItemsSetPrivileges.Values.Any(m => m.IsEnabled)
                , menuSetPrivilege
            );

            UpdateRoleButtons();
        }

        private void UpdateRoleButtons()
        {
            this.lstVwSecurityRoles.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled
                        && this._itemsSourceSecurityRoleList != null
                        && this._itemsSourceSecurityRoleList.Any(e => e.IsChanged)
                        ;

                    UIElement[] list = { btnSaveRoleChanges };

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

        private void txtBFilterSecurityRole_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowEntitySecurityRoles();
            }
        }

        private EntityMetadataListViewItem GetSelectedEntity()
        {
            return this.lstVwEntities.SelectedItems.OfType<EntityMetadataListViewItem>().Count() == 1
                ? this.lstVwEntities.SelectedItems.OfType<EntityMetadataListViewItem>().SingleOrDefault() : null;
        }

        private List<EntityMetadataListViewItem> GetSelectedEntities()
        {
            List<EntityMetadataListViewItem> result = this.lstVwEntities.SelectedItems.OfType<EntityMetadataListViewItem>().ToList();

            return result;
        }

        private RoleEntityPrivilegesViewItem GetSelectedSecurityRole()
        {
            return this.lstVwSecurityRoles.SelectedItems.OfType<RoleEntityPrivilegesViewItem>().Count() == 1
                ? this.lstVwSecurityRoles.SelectedItems.OfType<RoleEntityPrivilegesViewItem>().SingleOrDefault() : null;
        }

        private List<RoleEntityPrivilegesViewItem> GetSelectedSecurityRoles()
        {
            List<RoleEntityPrivilegesViewItem> result = this.lstVwSecurityRoles.SelectedItems.OfType<RoleEntityPrivilegesViewItem>().ToList();

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
                entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId].ToList();
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

        private async void btnEntityRelationshipOneToManyExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityRelationshipOneToManyExplorer(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName);
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

        private async void miOtherPrivilegesExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOtherPrivilegesExplorer(this._iWriteToOutput, service, _commonConfig);
        }

        private async void btnExportApplicationRibbon_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenApplicationRibbonExplorer(this._iWriteToOutput, service, _commonConfig);
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

            WindowHelper.OpenSystemFormExplorer(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName);
        }

        private async void btnSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSavedQueryExplorer(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnSavedChart_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSavedQueryVisualizationExplorer(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnWorkflows_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenWorkflowExplorer(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnPluginTree_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenPluginTree(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty, string.Empty);
        }

        private async void btnMessageTree_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageTree(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
        }

        private async void btnMessageRequestTree_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageRequestTree(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName);
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
                EntityMetadataListViewItem item = GetItemFromRoutedDataContext<EntityMetadataListViewItem>(e);

                if (item != null)
                {
                    ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

                    if (connectionData != null)
                    {
                        connectionData.OpenEntityMetadataInWeb(item.EntityMetadata.MetadataId.Value);
                    }
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
            ShowEntitySecurityRoles();
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

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenEntityMetadataInWeb(entity.EntityMetadata.MetadataId.Value);
            }
        }

        private async void mIOpenSecurityRoleInWeb_Click(object sender, RoutedEventArgs e)
        {
            var role = GetSelectedSecurityRole();

            if (role == null)
            {
                return;
            }

            var service = await GetService();

            service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.Role, role.Role.RoleId.Value);
        }

        private void mIOpenSecurityRoleListInWeb_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenEntityInstanceListInWeb(Role.EntityLogicalName);
            }
        }

        private void mICopyEntityInstanceIdToClipboard_Click(object sender, RoutedEventArgs e)
        {
            var role = GetSelectedSecurityRole();

            if (role == null)
            {
                return;
            }

            ClipboardHelper.SetText(role.Role.Id.ToString());
        }

        private void mICopyEntityInstanceUrlToClipboard_Click(object sender, RoutedEventArgs e)
        {
            var role = GetSelectedSecurityRole();

            if (role == null)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                var url = connectionData.GetEntityInstanceUrl(role.Role.LogicalName, role.Role.Id);

                ClipboardHelper.SetText(url);
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

        private async void AddSecurityRoleToCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddSecurityRoleToSolution(true, null);
        }

        private async void AddSecurityRoleToCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddSecurityRoleToSolution(false, solutionUniqueName);
            }
        }

        private async Task AddSecurityRoleToSolution(bool withSelect, string solutionUniqueName)
        {
            var roleList = GetSelectedSecurityRoles();

            if (roleList == null || !roleList.Any())
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.Role, roleList.Select(item => item.Role.RoleId.Value).ToList(), null, withSelect);
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

        private void ContextMenuSecurityRole_Opened(object sender, RoutedEventArgs e)
        {
            if (sender is ContextMenu contextMenu)
            {
                var items = contextMenu.Items.OfType<Control>();

                ConnectionData connectionData = null;

                cmBCurrentConnection.Dispatcher.Invoke(() =>
                {
                    connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
                });

                FillLastSolutionItems(connectionData, items, true, AddSecurityRoleToCrmSolutionLast_Click, "contMnAddToSolutionLast");
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

        private void mISecurityRoleOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            var role = GetSelectedSecurityRole();

            if (role == null)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.Role, role.Role.RoleId.Value);
            }
        }

        private async void mISecurityRoleOpenDependentComponentsInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var role = GetSelectedSecurityRole();

            if (role == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSolutionComponentDependenciesExplorer(_iWriteToOutput, service, null, _commonConfig, (int)ComponentType.Role, role.Role.RoleId.Value, null);
        }

        private async void mISecurityRoleOpenSolutionsContainingComponentInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var role = GetSelectedSecurityRole();

            if (role == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenExplorerSolutionExplorer(
                _iWriteToOutput
                , service
                , _commonConfig
                , (int)ComponentType.Role
                , role.Role.RoleId.Value
                , null
            );
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                this._itemsSourceEntityList?.Clear();
                this._itemsSourceSecurityRoleList?.Clear();
            });

            if (!this.IsControlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                ShowExistingEntities();
            }
        }

        private void btnClearEntityCacheAndRefresh_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = null;

            cmBCurrentConnection.Dispatcher.Invoke(() =>
            {
                connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
            });

            if (connectionData != null)
            {
                _cacheEntityMetadata.Remove(connectionData.ConnectionId);

                ShowExistingEntities();
            }
        }

        private void btnRefreshEntites_Click(object sender, RoutedEventArgs e)
        {
            ShowExistingEntities();
        }

        private void btnRefreshRoles_Click(object sender, RoutedEventArgs e)
        {
            ShowEntitySecurityRoles();
        }

        private void lstVwSecurityRoles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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
                    if (cell.Column == colBusinessUnit
                        || cell.Column == colRoleName
                        || cell.Column == colRoleTemplate
                        )
                    {
                        RoleEntityPrivilegesViewItem item = GetItemFromRoutedDataContext<RoleEntityPrivilegesViewItem>(e);

                        if (item != null)
                        {
                            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

                            if (connectionData != null)
                            {
                                connectionData.OpenEntityInstanceInWeb(item.Role.LogicalName, item.Role.Id);
                            }
                        }
                    }
                }
            }
        }

        private async void mIOpenSecurityRoleExplorer_Click(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is MenuItem menuItem))
            {
                return;
            }

            if (menuItem.DataContext == null
                || !(menuItem.DataContext is RoleEntityPrivilegesViewItem role)
                )
            {
                return;
            }

            var service = await GetService();

            IEnumerable<EntityMetadata> entityMetadataList = null;

            if (_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId];
            }

            WindowHelper.OpenRolesExplorer(_iWriteToOutput, service, _commonConfig, role.RoleName, entityMetadataList, null);
        }

        private async void mICreateSecurityRoleBackup_Click(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is MenuItem menuItem))
            {
                return;
            }

            if (menuItem.DataContext == null
                || !(menuItem.DataContext is RoleEntityPrivilegesViewItem role)
                )
            {
                return;
            }

            var service = await GetService();

            string operationName = string.Format(Properties.OperationNames.CreatingRoleBackupFormat2, service.ConnectionData.Name, role.Role.Name);

            _iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operationName);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.CreatingRoleBackupFormat2, service.ConnectionData.Name, role.Role.Name);

            var repositoryRolePrivileges = new RolePrivilegesRepository(service);
            var repositoryPrivileges = new PrivilegeRepository(service);

            var rolePrivileges = await repositoryRolePrivileges.GetRolePrivilegesAsync(role.Role.Id);
            var privileges = await repositoryPrivileges.GetListForRoleAsync(role.Role.Id);

            var temp = new List<Model.Backup.RolePrivilege>();

            foreach (var rolePriv in rolePrivileges)
            {
                var priv = privileges.FirstOrDefault(p => p.Id == rolePriv.PrivilegeId);

                if (priv != null)
                {
                    temp.Add(new Model.Backup.RolePrivilege()
                    {
                        Name = priv.Name,
                        Level = rolePriv.Depth,
                    });
                }
            }

            Model.Backup.Role roleBackup = new Model.Backup.Role()
            {
                Id = role.Role.Id,
                TemplateId = role.Role.RoleTemplateId?.Id,
                Name = role.Role.Name,
            };

            roleBackup.RolePrivileges.AddRange(temp.OrderBy(p => p.Name));

            string fileName = EntityFileNameFormatter.GetRoleFileName(service.ConnectionData.Name, role.Role.Name, EntityFileNameFormatter.Headers.Backup, "xml");
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            await roleBackup.SaveAsync(filePath);

            _iWriteToOutput.WriteToOutput(service.ConnectionData
                , Properties.OutputStrings.ExportedRoleBackupForConnectionFormat3
                , service.ConnectionData.Name
                , role.Role.Name
                , filePath
            );

            _iWriteToOutput.PerformAction(service.ConnectionData, filePath);

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.CreatingRoleBackupCompletedFormat2, service.ConnectionData.Name, role.Role.Name);

            _iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operationName);
        }

        private async void mISaveRoleChanges_Click(object sender, RoutedEventArgs e)
        {
            var changedRoles = _itemsSourceSecurityRoleList?.Where(en => en.IsChanged).ToList();

            if (changedRoles == null || !changedRoles.Any())
            {
                return;
            }

            string rolesName = string.Join(", ", changedRoles.Select(r => r.Role.Name).OrderBy(s => s));

            string message = string.Format(Properties.MessageBoxStrings.SaveChangesToRolesFormat1, rolesName);

            if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }

            var service = await GetService();

            string operationName = string.Format(Properties.OperationNames.SavingChangesInRolesFormat1, service.ConnectionData.Name);

            _iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operationName);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.SavingChangesInRolesFormat1, service.ConnectionData.Name);

            var rolePrivileges = new RolePrivilegesRepository(service);

            foreach (var role in changedRoles)
            {
                try
                {
                    List<RolePrivilege> privilegesAdd = role.GetAddRolePrivilege();
                    List<RolePrivilege> privilegesRemove = role.GetRemoveRolePrivilege();

                    _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.SavingChangesInRoleFormat1, role.RoleName);
                    _iWriteToOutput.WriteToOutputEntityInstance(service.ConnectionData, role.Role);

                    await rolePrivileges.ModifyRolePrivilegesAsync(role.Role.Id, privilegesAdd, privilegesRemove);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                }
            }

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.SavingChangesInRolesCompletedFormat1, service.ConnectionData.Name);

            _iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operationName);

            ShowEntitySecurityRoles();
        }

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, cmBCurrentConnection.SelectedItem as ConnectionData);
        }

        private void SetSelectedRolesPrivilege(PrivilegeDepthExtended privilegeDepth)
        {
            EntityMetadata entityMetadata = GetSelectedEntity()?.EntityMetadata;

            if (entityMetadata == null
                || entityMetadata.Privileges == null
                || !entityMetadata.Privileges.Any()
            )
            {
                return;
            }

            var list = lstVwSecurityRoles.SelectedItems.OfType<RoleEntityPrivilegesViewItem>().ToList();

            if (!list.Any())
            {
                return;
            }

            foreach (var privilege in entityMetadata.Privileges)
            {
                bool canBePrivilegeDepth = false;

                switch (privilegeDepth)
                {
                    case PrivilegeDepthExtended.Basic:
                        canBePrivilegeDepth = privilege.CanBeBasic;
                        break;

                    case PrivilegeDepthExtended.Local:
                        canBePrivilegeDepth = privilege.CanBeLocal;
                        break;

                    case PrivilegeDepthExtended.Deep:
                        canBePrivilegeDepth = privilege.CanBeDeep;
                        break;

                    case PrivilegeDepthExtended.Global:
                        canBePrivilegeDepth = privilege.CanBeGlobal;
                        break;

                    case PrivilegeDepthExtended.None:
                        canBePrivilegeDepth = true;
                        break;

                    default:
                        break;
                }

                if (canBePrivilegeDepth)
                {
                    foreach (var item in list)
                    {
                        switch (privilege.PrivilegeType)
                        {
                            case PrivilegeType.Create:
                                item.CreateRight = privilegeDepth;
                                break;
                            case PrivilegeType.Read:
                                item.ReadRight = privilegeDepth;
                                break;
                            case PrivilegeType.Write:
                                item.UpdateRight = privilegeDepth;
                                break;
                            case PrivilegeType.Delete:
                                item.DeleteRight = privilegeDepth;
                                break;
                            case PrivilegeType.Assign:
                                item.AssignRight = privilegeDepth;
                                break;
                            case PrivilegeType.Share:
                                item.ShareRight = privilegeDepth;
                                break;
                            case PrivilegeType.Append:
                                item.AppendRight = privilegeDepth;
                                break;
                            case PrivilegeType.AppendTo:
                                item.AppendToRight = privilegeDepth;
                                break;

                            case PrivilegeType.None:
                            default:
                                break;
                        }
                    }
                }
            }
        }

        private void SetSelectedRolesPrivilege(PrivilegeType privilegeType, PrivilegeDepthExtended privilegeDepth)
        {
            EntityMetadata entityMetadata = GetSelectedEntity()?.EntityMetadata;

            if (entityMetadata == null
                || entityMetadata.Privileges == null
                || !entityMetadata.Privileges.Any(p => p.PrivilegeType == privilegeType)
            )
            {
                return;
            }

            var privilege = entityMetadata.Privileges.First(p => p.PrivilegeType == privilegeType);

            switch (privilegeDepth)
            {
                case PrivilegeDepthExtended.Basic:
                    if (!privilege.CanBeBasic)
                    {
                        return;
                    }
                    break;

                case PrivilegeDepthExtended.Local:
                    if (!privilege.CanBeLocal)
                    {
                        return;
                    }
                    break;
                case PrivilegeDepthExtended.Deep:
                    if (!privilege.CanBeDeep)
                    {
                        return;
                    }
                    break;
                case PrivilegeDepthExtended.Global:
                    if (!privilege.CanBeGlobal)
                    {
                        return;
                    }
                    break;

                case PrivilegeDepthExtended.None:
                default:
                    break;
            }

            var list = lstVwSecurityRoles.SelectedItems.OfType<RoleEntityPrivilegesViewItem>().ToList();

            if (!list.Any())
            {
                return;
            }

            foreach (var item in list)
            {
                switch (privilegeType)
                {
                    case PrivilegeType.Create:
                        item.CreateRight = privilegeDepth;
                        break;
                    case PrivilegeType.Read:
                        item.ReadRight = privilegeDepth;
                        break;
                    case PrivilegeType.Write:
                        item.UpdateRight = privilegeDepth;
                        break;
                    case PrivilegeType.Delete:
                        item.DeleteRight = privilegeDepth;
                        break;
                    case PrivilegeType.Assign:
                        item.AssignRight = privilegeDepth;
                        break;
                    case PrivilegeType.Share:
                        item.ShareRight = privilegeDepth;
                        break;
                    case PrivilegeType.Append:
                        item.AppendRight = privilegeDepth;
                        break;
                    case PrivilegeType.AppendTo:
                        item.AppendToRight = privilegeDepth;
                        break;

                    case PrivilegeType.None:
                    default:
                        break;
                }
            }
        }

        #region Set Attribute

        private void mISetAttributeAllNone_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeDepthExtended.None);
        }

        private void mISetAttributeAllBasic_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeDepthExtended.Basic);
        }

        private void mISetAttributeAllLocal_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeDepthExtended.Local);
        }

        private void mISetAttributeAllDeep_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeDepthExtended.Deep);
        }

        private void mISetAttributeAllGlobal_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeDepthExtended.Global);
        }

        private void mISetAttributeCreateNone_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Create, PrivilegeDepthExtended.None);
        }

        private void mISetAttributeCreateBasic_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Create, PrivilegeDepthExtended.Basic);
        }

        private void mISetAttributeCreateLocal_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Create, PrivilegeDepthExtended.Local);
        }

        private void mISetAttributeCreateDeep_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Create, PrivilegeDepthExtended.Deep);
        }

        private void mISetAttributeCreateGlobal_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Create, PrivilegeDepthExtended.Global);
        }

        private void mISetAttributeReadNone_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Read, PrivilegeDepthExtended.None);
        }

        private void mISetAttributeReadBasic_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Read, PrivilegeDepthExtended.Basic);
        }

        private void mISetAttributeReadLocal_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Read, PrivilegeDepthExtended.Local);
        }

        private void mISetAttributeReadDeep_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Read, PrivilegeDepthExtended.Deep);
        }

        private void mISetAttributeReadGlobal_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Read, PrivilegeDepthExtended.Global);
        }

        private void mISetAttributeUpdateNone_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Write, PrivilegeDepthExtended.None);
        }

        private void mISetAttributeUpdateBasic_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Write, PrivilegeDepthExtended.Basic);
        }

        private void mISetAttributeUpdateLocal_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Write, PrivilegeDepthExtended.Local);
        }

        private void mISetAttributeUpdateDeep_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Write, PrivilegeDepthExtended.Deep);
        }

        private void mISetAttributeUpdateGlobal_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Write, PrivilegeDepthExtended.Global);
        }

        private void mISetAttributeDeleteNone_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Delete, PrivilegeDepthExtended.None);
        }

        private void mISetAttributeDeleteBasic_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Delete, PrivilegeDepthExtended.Basic);
        }

        private void mISetAttributeDeleteLocal_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Delete, PrivilegeDepthExtended.Local);
        }

        private void mISetAttributeDeleteDeep_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Delete, PrivilegeDepthExtended.Deep);
        }

        private void mISetAttributeDeleteGlobal_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Delete, PrivilegeDepthExtended.Global);
        }

        private void mISetAttributeAppendNone_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Append, PrivilegeDepthExtended.None);
        }

        private void mISetAttributeAppendBasic_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Append, PrivilegeDepthExtended.Basic);
        }

        private void mISetAttributeAppendLocal_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Append, PrivilegeDepthExtended.Local);
        }

        private void mISetAttributeAppendDeep_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Append, PrivilegeDepthExtended.Deep);
        }

        private void mISetAttributeAppendGlobal_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Append, PrivilegeDepthExtended.Global);
        }

        private void mISetAttributeAppendToNone_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.AppendTo, PrivilegeDepthExtended.None);
        }

        private void mISetAttributeAppendToBasic_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.AppendTo, PrivilegeDepthExtended.Basic);
        }

        private void mISetAttributeAppendToLocal_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.AppendTo, PrivilegeDepthExtended.Local);
        }

        private void mISetAttributeAppendToDeep_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.AppendTo, PrivilegeDepthExtended.Deep);
        }

        private void mISetAttributeAppendToGlobal_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.AppendTo, PrivilegeDepthExtended.Global);
        }

        private void mISetAttributeAssignNone_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Assign, PrivilegeDepthExtended.None);
        }

        private void mISetAttributeAssignBasic_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Assign, PrivilegeDepthExtended.Basic);
        }

        private void mISetAttributeAssignLocal_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Assign, PrivilegeDepthExtended.Local);
        }

        private void mISetAttributeAssignDeep_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Assign, PrivilegeDepthExtended.Deep);
        }

        private void mISetAttributeAssignGlobal_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Assign, PrivilegeDepthExtended.Global);
        }

        private void mISetAttributeShareNone_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Share, PrivilegeDepthExtended.None);
        }

        private void mISetAttributeShareBasic_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Share, PrivilegeDepthExtended.Basic);
        }

        private void mISetAttributeShareLocal_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Share, PrivilegeDepthExtended.Local);
        }

        private void mISetAttributeShareDeep_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Share, PrivilegeDepthExtended.Deep);
        }

        private void mISetAttributeShareGlobal_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeType.Share, PrivilegeDepthExtended.Global);
        }

        #endregion Set Attribute

        private void hyperlinkPublishEntity_Click(object sender, RoutedEventArgs e)
        {
            EntityMetadataListViewItem entity = GetItemFromRoutedDataContext<EntityMetadataListViewItem>(e);

            if (entity == null)
            {
                return;
            }

            ExecuteActionAsync(new[] { entity.LogicalName }, PublishEntityAsync);
        }
    }
}