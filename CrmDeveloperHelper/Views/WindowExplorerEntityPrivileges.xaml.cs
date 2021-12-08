using Microsoft.Crm.Sdk.Messages;
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class WindowExplorerEntityPrivileges : WindowWithConnectionList
    {
        private readonly Popup _popupEntityMetadataFilter;
        private readonly EntityMetadataFilter _entityMetadataFilter;

        private readonly ObservableCollection<EntityMetadataListViewItem> _itemsSourceEntityList;

        private readonly ObservableCollection<EntityRolePrivilegeViewItem> _itemsSourceSecurityRoleList;

        private readonly Dictionary<Guid, IEnumerable<EntityMetadata>> _cacheEntityMetadata = new Dictionary<Guid, IEnumerable<EntityMetadata>>();

        private readonly Dictionary<PrivilegeType, MenuItem> _menuItemsSetPrivileges;
        private readonly Dictionary<PrivilegeDepth, MenuItem> _menuItemsSetPrivilegesDepthsAll;
        private readonly Dictionary<PrivilegeType, Dictionary<PrivilegeDepth, MenuItem>> _menuItemsSetPrivilegesDepths;

        public WindowExplorerEntityPrivileges(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
            , IEnumerable<EntityMetadata> entityMetadataList
            , string filterEntity
        ) : base(iWriteToOutput, commonConfig, service)
        {
            this.IncreaseInit();

            InitializeComponent();

            SetInputLanguageEnglish();

            if (entityMetadataList != null && entityMetadataList.Any(e => e.Privileges != null && e.Privileges.Any()))
            {
                _cacheEntityMetadata[service.ConnectionData.ConnectionId] = entityMetadataList;
            }

            _entityMetadataFilter = new EntityMetadataFilter();
            _entityMetadataFilter.CloseClicked += this.entityMetadataFilter_CloseClicked;
            this._popupEntityMetadataFilter = new Popup
            {
                Child = _entityMetadataFilter,

                PlacementTarget = lblFilterEntity,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
                Focusable = true,
            };
            _popupEntityMetadataFilter.Closed += this.popupEntityMetadataFilter_Closed;

            LoadFromConfig();

            txtBFilterEnitity.Text = filterEntity;
            txtBFilterEnitity.SelectionLength = 0;
            txtBFilterEnitity.SelectionStart = txtBFilterEnitity.Text.Length;

            txtBFilterEnitity.Focus();

            _itemsSourceEntityList = new ObservableCollection<EntityMetadataListViewItem>();
            lstVwEntities.ItemsSource = _itemsSourceEntityList;

            _itemsSourceSecurityRoleList = new ObservableCollection<EntityRolePrivilegeViewItem>();
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

            FillExplorersMenuItems();

            this.DecreaseInit();

            var task = ShowExistingEntities();
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
                return _cacheEntityMetadata[connectionId];
            }

            return null;
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

        private async Task ShowExistingEntities()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var connectionData = GetSelectedConnection();

            ToggleControls(connectionData, false, Properties.OutputStrings.LoadingEntities);

            this.Dispatcher.Invoke(() =>
            {
                _itemsSourceEntityList.Clear();
                _itemsSourceSecurityRoleList.Clear();
            });

            IEnumerable<EntityMetadataListViewItem> list = Enumerable.Empty<EntityMetadataListViewItem>();

            try
            {
                var service = await GetService();

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
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }

            string filterEntity = string.Empty;

            this.Dispatcher.Invoke(() =>
            {
                filterEntity = txtBFilterEnitity.Text.Trim().ToLower();
            });

            list = FilterEntityList(list, filterEntity);

            this.lstVwEntities.Dispatcher.Invoke(() =>
            {
                foreach (var entity in list
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
                else
                {
                    var entity = list.FirstOrDefault(e => string.Equals(e.LogicalName, filterEntity, StringComparison.InvariantCultureIgnoreCase));

                    if (entity != null)
                    {
                        this.lstVwEntities.SelectedItem = entity;
                    }
                }
            });

            ToggleControls(connectionData, true, Properties.OutputStrings.LoadingEntitiesCompletedFormat1, list.Count());

            await ShowEntitySecurityRoles();
        }

        private IEnumerable<EntityMetadataListViewItem> FilterEntityList(IEnumerable<EntityMetadataListViewItem> list, string textName)
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
                    list = list.Where(ent =>
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

            return list;
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

            var connectionData = GetSelectedConnection();

            ToggleControls(connectionData, false, Properties.OutputStrings.LoadingEntityPrivileges);

            string filterRole = string.Empty;

            txtBFilterSecurityRole.Dispatcher.Invoke(() =>
            {
                filterRole = txtBFilterSecurityRole.Text.Trim().ToLower();
            });

            IEnumerable<EntityRolePrivilegeViewItem> list = Enumerable.Empty<EntityRolePrivilegeViewItem>();

            try
            {
                var service = await GetService();

                if (service != null)
                {
                    if (entityMetadata.Privileges != null && entityMetadata.Privileges.Any())
                    {
                        var repositoryRole = new RoleRepository(service);

                        IEnumerable<Role> roles = await repositoryRole.GetListAsync(filterRole, new ColumnSet(
                            Role.Schema.Attributes.name
                            , Role.Schema.Attributes.businessunitid
                            , Role.Schema.Attributes.ismanaged
                            , Role.Schema.Attributes.roletemplateid
                            , Role.Schema.Attributes.iscustomizable
                        ));

                        var repository = new RolePrivilegesRepository(service);

                        var task = repository.GetEntityPrivilegesAsync(roles.Select(r => r.RoleId.Value), entityMetadata.Privileges?.Select(p => p.PrivilegeId));

                        ActivateMenuItemsSetPrivileges(entityMetadata.Privileges);

                        var listRolePrivilege = await task;

                        list = roles.Select(r => new EntityRolePrivilegeViewItem(r, listRolePrivilege.Where(rp => rp.RoleId == r.RoleId), entityMetadata.Privileges)).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }

            this.lstVwSecurityRoles.Dispatcher.Invoke(() =>
            {
                foreach (var entity in list
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
                else
                {
                    var role = list.FirstOrDefault(e => string.Equals(e.RoleName, filterRole, StringComparison.InvariantCultureIgnoreCase));

                    if (role != null)
                    {
                        this.lstVwSecurityRoles.SelectedItem = role;
                    }
                }
            });

            ToggleControls(connectionData, true, Properties.OutputStrings.LoadingEntityPrivilegesCompletedFormat1, list.Count());
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

        private void Entity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, nameof(SinglePrivilegeViewItem.IsChanged), StringComparison.InvariantCultureIgnoreCase))
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

        protected override void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
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

        private async void txtBFilterEnitity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await ShowExistingEntities();
            }
        }

        private async void txtBFilterSecurityRole_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await ShowEntitySecurityRoles();
            }
        }

        private EntityMetadataListViewItem GetSelectedEntity()
        {
            return this.lstVwEntities.SelectedItems.Cast<EntityMetadataListViewItem>().Count() == 1
                ? this.lstVwEntities.SelectedItems.Cast<EntityMetadataListViewItem>().SingleOrDefault() : null;
        }

        private List<EntityMetadataListViewItem> GetSelectedEntities()
        {
            List<EntityMetadataListViewItem> result = this.lstVwEntities.SelectedItems.Cast<EntityMetadataListViewItem>().ToList();

            return result;
        }

        private EntityRolePrivilegeViewItem GetSelectedSecurityRole()
        {
            return this.lstVwSecurityRoles.SelectedItems.Cast<EntityRolePrivilegeViewItem>().Count() == 1
                ? this.lstVwSecurityRoles.SelectedItems.Cast<EntityRolePrivilegeViewItem>().SingleOrDefault() : null;
        }

        private List<EntityRolePrivilegeViewItem> GetSelectedSecurityRoles()
        {
            List<EntityRolePrivilegeViewItem> result = this.lstVwSecurityRoles.SelectedItems.Cast<EntityRolePrivilegeViewItem>().ToList();

            return result;
        }

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
                    ConnectionData connectionData = GetSelectedConnection();

                    if (connectionData != null)
                    {
                        connectionData.OpenEntityMetadataInWeb(item.EntityMetadata.MetadataId.Value);
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
            await ShowEntitySecurityRoles();
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

        private async void mIOpenSecurityRoleInWeb_Click(object sender, RoutedEventArgs e)
        {
            var role = GetSelectedSecurityRole();

            if (role == null)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            service.UrlGenerator.OpenSolutionComponentInWeb(ComponentType.Role, role.Role.RoleId.Value);
        }

        private void mIOpenSecurityRoleListInWeb_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = GetSelectedConnection();

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

            ConnectionData connectionData = GetSelectedConnection();

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
            var roleList = GetSelectedSecurityRoles()
                .Select(item => item.Role.RoleId.Value)
                .ToList();

            if (!roleList.Any())
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            _commonConfig.Save();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.Role, roleList, null, withSelect);
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

        private void ContextMenuSecurityRole_Opened(object sender, RoutedEventArgs e)
        {
            if (sender is ContextMenu contextMenu)
            {
                var items = contextMenu.Items.OfType<Control>();

                ConnectionData connectionData = GetSelectedConnection();

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

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenSolutionComponentDependenciesExplorer(_iWriteToOutput, service, null, _commonConfig, (int)ComponentType.Entity, entity.EntityMetadata.MetadataId.Value, null);
        }

        private async void mIOpenSolutionsContainingComponentInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            if (entity == null)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            _commonConfig.Save();

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

            ConnectionData connectionData = GetSelectedConnection();

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

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenSolutionComponentDependenciesExplorer(_iWriteToOutput, service, null, _commonConfig, (int)ComponentType.Role, role.Role.RoleId.Value, null);
        }

        private async void mISecurityRoleOpenSolutionsContainingComponentInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var role = GetSelectedSecurityRole();

            if (role == null)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenExplorerSolutionExplorer(
                _iWriteToOutput
                , service
                , _commonConfig
                , (int)ComponentType.Role
                , role.Role.RoleId.Value
                , null
            );
        }

        private async void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                await ShowExistingEntities();
            }
        }

        private async void btnClearEntityCacheAndRefresh_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                _cacheEntityMetadata.Remove(connectionData.ConnectionId);

                await ShowExistingEntities();
            }
        }

        private async void btnRefreshEntites_Click(object sender, RoutedEventArgs e)
        {
            await ShowExistingEntities();
        }

        private async void btnRefreshRoles_Click(object sender, RoutedEventArgs e)
        {
            await ShowEntitySecurityRoles();
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
                        EntityRolePrivilegeViewItem item = GetItemFromRoutedDataContext<EntityRolePrivilegeViewItem>(e);

                        if (item != null)
                        {
                            ConnectionData connectionData = GetSelectedConnection();

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
                || !(menuItem.DataContext is EntityRolePrivilegeViewItem role)
            )
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

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
                || !(menuItem.DataContext is EntityRolePrivilegeViewItem role)
            )
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            string operationName = string.Format(Properties.OperationNames.CreatingRoleBackupFormat2, service.ConnectionData.Name, role.Role.Name);

            _iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operationName);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.InConnectionCreatingRoleBackupFormat2, service.ConnectionData.Name, role.Role.Name);

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

            var roleBackup = new Model.Backup.Role()
            {
                Id = role.Role.Id,
                TemplateId = role.Role.RoleTemplateId?.Id,
                Name = role.Role.Name,
            };

            roleBackup.RolePrivileges.AddRange(temp.OrderBy(p => p.Name));

            string fileName = EntityFileNameFormatter.GetRoleFileName(service.ConnectionData.Name, role.Role.Name, EntityFileNameFormatter.Headers.Backup, FileExtension.xml);
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            await roleBackup.SaveAsync(filePath);

            _iWriteToOutput.WriteToOutput(service.ConnectionData
                , Properties.OutputStrings.InConnectionExportedRoleBackupFormat3
                , service.ConnectionData.Name
                , role.Role.Name
                , filePath
            );

            _iWriteToOutput.PerformAction(service.ConnectionData, filePath);

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionCreatingRoleBackupCompletedFormat2, service.ConnectionData.Name, role.Role.Name);

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

            if (service == null)
            {
                return;
            }

            string operationName = string.Format(Properties.OperationNames.SavingChangesInRolesFormat1, service.ConnectionData.Name);

            _iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operationName);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.InConnectionSavingChangesInRolesFormat1, service.ConnectionData.Name);

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

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.InConnectionSavingChangesInRolesCompletedFormat1, service.ConnectionData.Name);

            _iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operationName);

            await ShowEntitySecurityRoles();
        }

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, GetSelectedConnection());
        }

        #region Set Attribute

        private void mISetAttributeAll_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is PrivilegeDepthExtended privilegeDepth
            )
            {
                SetSelectedRolesPrivilege(privilegeDepth);
            }
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

            var list = lstVwSecurityRoles.SelectedItems.Cast<EntityRolePrivilegeViewItem>().ToList();

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

        private void mISetAttributeCreate_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is PrivilegeDepthExtended privilegeDepth
            )
            {
                SetSelectedRolesPrivilege(PrivilegeType.Create, privilegeDepth);
            }
        }

        private void mISetAttributeRead_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is PrivilegeDepthExtended privilegeDepth
            )
            {
                SetSelectedRolesPrivilege(PrivilegeType.Read, privilegeDepth);
            }
        }

        private void mISetAttributeUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is PrivilegeDepthExtended privilegeDepth
            )
            {
                SetSelectedRolesPrivilege(PrivilegeType.Write, privilegeDepth);
            }
        }

        private void mISetAttributeDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is PrivilegeDepthExtended privilegeDepth
            )
            {
                SetSelectedRolesPrivilege(PrivilegeType.Delete, privilegeDepth);
            }
        }

        private void mISetAttributeAppend_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is PrivilegeDepthExtended privilegeDepth
            )
            {
                SetSelectedRolesPrivilege(PrivilegeType.Append, privilegeDepth);
            }
        }

        private void mISetAttributeAppendTo_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is PrivilegeDepthExtended privilegeDepth
            )
            {
                SetSelectedRolesPrivilege(PrivilegeType.AppendTo, privilegeDepth);
            }
        }

        private void mISetAttributeAssign_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is PrivilegeDepthExtended privilegeDepth
            )
            {
                SetSelectedRolesPrivilege(PrivilegeType.Assign, privilegeDepth);
            }
        }

        private void mISetAttributeShare_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is PrivilegeDepthExtended privilegeDepth
            )
            {
                SetSelectedRolesPrivilege(PrivilegeType.Share, privilegeDepth);
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

            var list = lstVwSecurityRoles.SelectedItems.Cast<EntityRolePrivilegeViewItem>().ToList();

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

        #endregion Set Attribute

        private async void hyperlinkPublishEntity_Click(object sender, RoutedEventArgs e)
        {
            EntityMetadataListViewItem entity = GetItemFromRoutedDataContext<EntityMetadataListViewItem>(e);

            if (entity == null)
            {
                return;
            }

            await PublishEntityAsync(GetSelectedConnection(), new[] { entity.LogicalName });
        }

        #region Clipboard Entity

        private void mIClipboardEntityCopyName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityMetadataListViewItem>(e, ent => ent.LogicalName);
        }

        private void mIClipboardEntityCopyDisplayName_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityMetadataListViewItem>(e, ent => ent.DisplayName);
        }

        private void mIClipboardEntityCopyObjectTypeCode_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityMetadataListViewItem>(e, ent => ent.ObjectTypeCode.ToString());
        }

        private void mIClipboardEntityCopyEntityMetadataId_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityMetadataListViewItem>(e, ent => ent.EntityMetadata.MetadataId.ToString());
        }

        #endregion Clipboard Entity

        #region Role Context Menu

        private void mICopyRoleNameToClipboard_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityRolePrivilegeViewItem>(e, ent => ent.Role.Name);
        }

        private void mICopyRoleTemplateNameToClipboard_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityRolePrivilegeViewItem>(e, ent => ent.RoleTemplateName);
        }

        private void mICopyRoleTemplateIdToClipboard_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityRolePrivilegeViewItem>(e, ent => ent.Role.RoleTemplateId?.Id.ToString());
        }

        private void mICopyRoleBusinessUnitNameToClipboard_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityRolePrivilegeViewItem>(e, ent => ent.Role.BusinessUnitId?.Name);
        }

        private void mICopyRoleBusinessUnitIdToClipboard_Click(object sender, RoutedEventArgs e)
        {
            GetEntityViewItemAndCopyToClipboard<EntityRolePrivilegeViewItem>(e, ent => ent.Role.BusinessUnitId?.Id.ToString());
        }

        #endregion Role Context Menu
    }
}