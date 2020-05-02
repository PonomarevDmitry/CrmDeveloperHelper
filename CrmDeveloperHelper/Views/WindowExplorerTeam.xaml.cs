using Microsoft.Xrm.Sdk;
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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExplorerTeam : WindowWithConnectionList
    {
        private string _tabSpacer = "    ";

        private readonly Popup _popupEntityMetadataFilter;
        private readonly EntityMetadataFilter _entityMetadataFilter;

        private readonly ObservableCollection<SystemUser> _itemsSourceSystemUsers;

        private readonly ObservableCollection<Team> _itemsSourceTeams;
        private readonly ObservableCollection<Role> _itemsSourceRoles;

        private readonly ObservableCollection<EntityPrivilegeViewItem> _itemsSourceEntityPrivileges;
        private readonly ObservableCollection<OtherPrivilegeViewItem> _itemsSourceOtherPrivileges;

        private readonly Dictionary<Guid, SolutionComponentDescriptor> _descriptorCache = new Dictionary<Guid, SolutionComponentDescriptor>();

        private readonly Dictionary<Guid, IEnumerable<EntityMetadata>> _cacheEntityMetadata = new Dictionary<Guid, IEnumerable<EntityMetadata>>();
        private readonly Dictionary<Guid, IEnumerable<Privilege>> _cachePrivileges = new Dictionary<Guid, IEnumerable<Privilege>>();

        public WindowExplorerTeam(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
            , IEnumerable<EntityMetadata> entityMetadataList
            , IEnumerable<Privilege> privileges
            , string filterEntity
        ) : base(iWriteToOutput, commonConfig, service)
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            if (entityMetadataList != null && entityMetadataList.Any(e => e.Privileges != null && e.Privileges.Any()))
            {
                _cacheEntityMetadata[service.ConnectionData.ConnectionId] = entityMetadataList;
            }

            if (privileges != null)
            {
                _cachePrivileges[service.ConnectionData.ConnectionId] = privileges;
            }

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

            cmBTeamType.ItemsSource = new EnumBindingSourceExtension(typeof(Team.Schema.OptionSets.teamtype?)).ProvideValue(null) as IEnumerable;

            FillRoleEditorLayoutTabs();
            FillIsDefaultComboBox();

            LoadFromConfig();

            txtBFilterTeams.Text = filterEntity;
            txtBFilterTeams.SelectionLength = 0;
            txtBFilterTeams.SelectionStart = txtBFilterTeams.Text.Length;

            txtBFilterTeams.Focus();

            lstVwSystemUsers.ItemsSource = _itemsSourceSystemUsers = new ObservableCollection<SystemUser>();

            lstVwTeams.ItemsSource = _itemsSourceTeams = new ObservableCollection<Team>();
            lstVwSecurityRoles.ItemsSource = _itemsSourceRoles = new ObservableCollection<Role>();
            lstVwEntityPrivileges.ItemsSource = _itemsSourceEntityPrivileges = new ObservableCollection<EntityPrivilegeViewItem>();
            lstVwOtherPrivileges.ItemsSource = _itemsSourceOtherPrivileges = new ObservableCollection<OtherPrivilegeViewItem>();

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            FillExplorersMenuItems();

            this.DecreaseInit();

            ShowExistingTeams();
        }

        private void FillExplorersMenuItems()
        {
            var explorersHelper = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService
                , getEntityName: GetEntityName
                , getOtherPrivilegeName: GetOtherPrivilegeName
                , getEntityMetadataList: GetEntityMetadataList
                , getOtherPrivilegesList: GetOtherPrivilegesList
            );

            var compareWindowsHelper = new CompareWindowsHelper(_iWriteToOutput, _commonConfig, () => Tuple.Create(GetSelectedConnection(), GetSelectedConnection())
                , getEntityName: GetEntityName
            );

            explorersHelper.FillExplorers(miExplorers);
            compareWindowsHelper.FillCompareWindows(miCompareOrganizations);

            if (this.Resources.Contains("listContextMenuEntityPrivileges")
                && this.Resources["listContextMenuEntityPrivileges"] is ContextMenu listContextMenuEntityPrivileges
            )
            {
                explorersHelper.FillExplorers(listContextMenuEntityPrivileges, nameof(miExplorers));

                compareWindowsHelper.FillCompareWindows(listContextMenuEntityPrivileges, nameof(miCompareOrganizations));

                var items = listContextMenuEntityPrivileges.Items.OfType<MenuItem>();

                foreach (var item in items)
                {
                    if (string.Equals(item.Uid, "miEntityPrivilegesExplorer", StringComparison.InvariantCultureIgnoreCase))
                    {
                        item.Click += explorersHelper.miEntityPrivilegesExplorer_Click;
                    }
                }
            }

            if (this.Resources.Contains("listContextMenuOtherPrivileges")
                && this.Resources["listContextMenuOtherPrivileges"] is ContextMenu listContextMenuOtherPrivileges
            )
            {
                var items = listContextMenuOtherPrivileges.Items.OfType<MenuItem>();

                foreach (var item in items)
                {
                    if (string.Equals(item.Uid, "mIOpenOtherPrivilegeExplorer", StringComparison.InvariantCultureIgnoreCase))
                    {
                        item.Click += explorersHelper.miOtherPrivilegesExplorer_Click;
                    }
                }
            }
        }

        private string GetEntityName()
        {
            var entity = GetSelectedEntity();

            return entity?.LogicalName;
        }

        private string GetOtherPrivilegeName()
        {
            var privilege = GetSelectedOtherPrivilege();

            return privilege?.Name;
        }

        private IEnumerable<EntityMetadata> GetEntityMetadataList(Guid connectionId)
        {
            if (_cacheEntityMetadata.ContainsKey(connectionId))
            {
                return _cacheEntityMetadata[connectionId];
            }

            return null;
        }

        private IEnumerable<Privilege> GetOtherPrivilegesList(Guid connectionId)
        {
            if (_cachePrivileges.ContainsKey(connectionId))
            {
                return _cachePrivileges[connectionId];
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

        private void _popupEntityMetadataFilter_Closed(object sender, EventArgs e)
        {
            if (_entityMetadataFilter.FilterChanged)
            {
                ShowTeamEntityPrivileges();
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
            var tabs = RoleEditorLayoutTab.GetTabs();

            cmBRoleEditorLayoutTabsEntities.Items.Clear();

            cmBRoleEditorLayoutTabsEntities.Items.Add("All");

            foreach (var tab in tabs)
            {
                cmBRoleEditorLayoutTabsEntities.Items.Add(tab);
            }

            cmBRoleEditorLayoutTabsEntities.SelectedIndex = 0;

            cmBRoleEditorLayoutTabsPrivileges.Items.Clear();

            cmBRoleEditorLayoutTabsPrivileges.Items.Add("All");

            foreach (var tab in tabs)
            {
                cmBRoleEditorLayoutTabsPrivileges.Items.Add(tab);
            }

            cmBRoleEditorLayoutTabsPrivileges.SelectedIndex = 0;
        }

        private void FillIsDefaultComboBox()
        {
            cmBIsDefault.Items.Clear();
            cmBIsDefault.Items.Add(string.Empty);
            cmBIsDefault.Items.Add(false);
            cmBIsDefault.Items.Add(true);
        }

        protected override void LoadConfigurationInternal(WindowSettings winConfig)
        {
            base.LoadConfigurationInternal(winConfig);

            LoadFormSettings(winConfig);
        }

        private const string paramColumnSystemUserWidth = "ColumnSystemUserWidth";

        private void LoadFormSettings(WindowSettings winConfig)
        {
            if (winConfig.DictDouble.ContainsKey(paramColumnSystemUserWidth))
            {
                columnSystemUser.Width = new GridLength(winConfig.DictDouble[paramColumnSystemUserWidth]);
            }
        }

        protected override void SaveConfigurationInternal(WindowSettings winConfig)
        {
            base.SaveConfigurationInternal(winConfig);

            winConfig.DictDouble[paramColumnSystemUserWidth] = columnSystemUser.Width.Value;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            BindingOperations.ClearAllBindings(cmBCurrentConnection);

            cmBCurrentConnection.Items.DetachFromSourceCollection();

            cmBCurrentConnection.DataContext = null;
            cmBCurrentConnection.ItemsSource = null;
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

        private SolutionComponentDescriptor GetDescriptor(IOrganizationServiceExtented service)
        {
            if (service != null)
            {
                if (!_descriptorCache.ContainsKey(service.ConnectionData.ConnectionId))
                {
                    _descriptorCache[service.ConnectionData.ConnectionId] = new SolutionComponentDescriptor(service);
                }

                _descriptorCache[service.ConnectionData.ConnectionId].SetSettings(_commonConfig);

                return _descriptorCache[service.ConnectionData.ConnectionId];
            }

            return null;
        }

        private async Task ShowTeamSystemUsers()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.LoadingTeamSystemUsers);

            string textName = string.Empty;

            this.Dispatcher.Invoke(() =>
            {
                _itemsSourceSystemUsers.Clear();

                textName = txtBFilterSystemUser.Text.Trim().ToLower();
            });

            IEnumerable<SystemUser> list = Enumerable.Empty<SystemUser>();

            var team = GetSelectedTeam();

            try
            {
                if (service != null && team != null)
                {
                    SystemUserRepository repository = new SystemUserRepository(service);

                    list = await repository.GetUsersByTeamAsync(team.Id, textName
                        , new ColumnSet(
                            SystemUser.Schema.Attributes.fullname
                            , SystemUser.Schema.Attributes.domainname
                            , SystemUser.Schema.Attributes.businessunitid
                            , SystemUser.Schema.Attributes.isdisabled
                            ));
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            this.lstVwSystemUsers.Dispatcher.Invoke(() =>
            {
                foreach (var entity in list.OrderBy(s => s.FullName))
                {
                    _itemsSourceSystemUsers.Add(entity);
                }

                if (this.lstVwSystemUsers.Items.Count == 1)
                {
                    this.lstVwSystemUsers.SelectedItem = this.lstVwSystemUsers.Items[0];
                }
            });

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.LoadingTeamSystemUsersCompletedFormat1, list.Count());
        }

        private async Task ShowTeamRoles()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.LoadingTeamSecurityRoles);

            string filterRole = string.Empty;

            this.Dispatcher.Invoke(() =>
            {
                _itemsSourceRoles.Clear();

                filterRole = txtBFilterRole.Text.Trim().ToLower();
            });

            var team = GetSelectedTeam();

            IEnumerable<Role> list = Enumerable.Empty<Role>();

            try
            {
                if (service != null && team != null && team.TeamType?.Value != (int)Team.Schema.OptionSets.teamtype.Access_1)
                {
                    RoleRepository repository = new RoleRepository(service);

                    list = await repository.GetTeamRolesAsync(team.Id, filterRole
                        , new ColumnSet(
                                Role.Schema.Attributes.name
                                , Role.Schema.Attributes.businessunitid
                                , Role.Schema.Attributes.ismanaged
                                , Role.Schema.Attributes.iscustomizable
                                ));
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            this.lstVwSecurityRoles.Dispatcher.Invoke(() =>
            {
                foreach (var entity in list.OrderBy(s => s.Name))
                {
                    _itemsSourceRoles.Add(entity);
                }

                if (this.lstVwSecurityRoles.Items.Count == 1)
                {
                    this.lstVwSecurityRoles.SelectedItem = this.lstVwSecurityRoles.Items[0];
                }
            });

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.LoadingTeamSecurityRolesCompletedFormat1, list.Count());
        }

        private async Task ShowExistingTeams()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.LoadingTeams);

            string filterTeam = string.Empty;
            Team.Schema.OptionSets.teamtype? teamType = null;
            bool? isDefault = null;

            this.Dispatcher.Invoke(() =>
            {
                _itemsSourceTeams.Clear();

                _itemsSourceSystemUsers.Clear();
                _itemsSourceRoles.Clear();
                _itemsSourceEntityPrivileges.Clear();
                _itemsSourceOtherPrivileges.Clear();

                filterTeam = txtBFilterTeams.Text.Trim().ToLower();

                {
                    if (cmBTeamType.SelectedItem is Team.Schema.OptionSets.teamtype comboBoxItem)
                    {
                        teamType = comboBoxItem;
                    }
                }

                if (cmBIsDefault.SelectedItem is bool value)
                {
                    isDefault = value;
                }
            });

            IEnumerable<Team> list = Enumerable.Empty<Team>();

            try
            {
                if (service != null)
                {
                    TeamRepository repository = new TeamRepository(service);

                    list = await repository.GetListAsync(filterTeam
                        , isDefault
                        , teamType
                        , new ColumnSet
                        (
                            Team.Schema.Attributes.name
                            , Team.Schema.Attributes.businessunitid
                            , Team.Schema.Attributes.teamtype
                            , Team.Schema.Attributes.regardingobjectid
                            , Team.Schema.Attributes.teamtemplateid
                            , Team.Schema.Attributes.isdefault
                        )
                    );
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            this.lstVwTeams.Dispatcher.Invoke(() =>
            {
                foreach (var entity in list
                     .OrderBy(s => s.TeamType?.Value)
                    .ThenBy(s => s.RegardingObjectId?.LogicalName)
                    .ThenBy(s => s.TeamTemplateName)
                    .ThenBy(s => s.Name)
                )
                {
                    _itemsSourceTeams.Add(entity);
                }

                if (this.lstVwTeams.Items.Count == 1)
                {
                    this.lstVwTeams.SelectedItem = this.lstVwTeams.Items[0];
                }
            });

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.LoadingTeamsCompletedFormat1, list.Count());

            await RefreshTeamInfo();
        }

        private async Task ShowTeamEntityPrivileges()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.LoadingEntities);

            this.Dispatcher.Invoke(() =>
            {
                _itemsSourceEntityPrivileges.Clear();
                _itemsSourceOtherPrivileges.Clear();
            });

            var team = GetSelectedTeam();

            IEnumerable<EntityMetadata> entityMetadataList = Enumerable.Empty<EntityMetadata>();

            IEnumerable<EntityPrivilegeViewItem> listEntityPrivileges = Enumerable.Empty<EntityPrivilegeViewItem>();

            IEnumerable<OtherPrivilegeViewItem> listOtherPrivileges = Enumerable.Empty<OtherPrivilegeViewItem>();

            try
            {
                if (service != null)
                {
                    var otherPrivileges = await GetPrivileges(service);
                    entityMetadataList = await GetEntityMetadataEnumerable(service);

                    entityMetadataList = entityMetadataList.Where(e => e.Privileges != null && e.Privileges.Any(p => p.PrivilegeType != PrivilegeType.None));

                    if (team != null && team.TeamType?.Value != (int)Team.Schema.OptionSets.teamtype.Access_1)
                    {
                        string filterEntity = string.Empty;
                        string filterOtherPrivilege = string.Empty;
                        RoleEditorLayoutTab selectedTabEntities = null;
                        RoleEditorLayoutTab selectedTabPrivileges = null;

                        this.Dispatcher.Invoke(() =>
                        {
                            filterEntity = txtBEntityFilter.Text.Trim().ToLower();
                            filterOtherPrivilege = txtBOtherPrivilegesFilter.Text.Trim().ToLower();

                            selectedTabEntities = cmBRoleEditorLayoutTabsEntities.SelectedItem as RoleEditorLayoutTab;
                            selectedTabPrivileges = cmBRoleEditorLayoutTabsPrivileges.SelectedItem as RoleEditorLayoutTab;
                        });

                        entityMetadataList = FilterEntityList(entityMetadataList, filterEntity, selectedTabEntities);
                        otherPrivileges = FilterPrivilegeList(otherPrivileges, filterOtherPrivilege, selectedTabPrivileges);

                        if (entityMetadataList.Any() || otherPrivileges.Any())
                        {
                            var repository = new RolePrivilegesRepository(service);

                            var userPrivileges = await repository.GetTeamPrivilegesAsync(team.Id);

                            listEntityPrivileges = entityMetadataList.Select(e => new EntityPrivilegeViewItem(e, userPrivileges));

                            listOtherPrivileges = otherPrivileges.Select(e => new OtherPrivilegeViewItem(e, userPrivileges));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            this.lstVwEntityPrivileges.Dispatcher.Invoke(() =>
            {
                foreach (var entity in listEntityPrivileges
                    .OrderBy(s => s.IsIntersect)
                    .ThenBy(s => s.LogicalName)
                )
                {
                    _itemsSourceEntityPrivileges.Add(entity);
                }

                if (this.lstVwEntityPrivileges.Items.Count == 1)
                {
                    this.lstVwEntityPrivileges.SelectedItem = this.lstVwEntityPrivileges.Items[0];
                }
            });

            this.lstVwOtherPrivileges.Dispatcher.Invoke(() =>
            {
                foreach (var otherPriv in listOtherPrivileges
                    .OrderBy(s => s.EntityLogicalName)
                    .ThenBy(s => s.Name, PrivilegeNameComparer.Comparer)
                )
                {
                    _itemsSourceOtherPrivileges.Add(otherPriv);
                }

                if (this.lstVwOtherPrivileges.Items.Count == 1)
                {
                    this.lstVwOtherPrivileges.SelectedItem = this.lstVwOtherPrivileges.Items[0];
                }
            });

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.LoadingEntitiesCompletedFormat1, entityMetadataList.Count());
        }

        private async Task<IEnumerable<Privilege>> GetPrivileges(IOrganizationServiceExtented service)
        {
            if (!_cachePrivileges.ContainsKey(service.ConnectionData.ConnectionId))
            {
                PrivilegeRepository repository = new PrivilegeRepository(service);

                var temp = await repository.GetListOtherPrivilegeAsync(new ColumnSet(
                    Privilege.Schema.Attributes.privilegeid
                    , Privilege.Schema.Attributes.name
                    , Privilege.Schema.Attributes.accessright

                    , Privilege.Schema.Attributes.canbebasic
                    , Privilege.Schema.Attributes.canbelocal
                    , Privilege.Schema.Attributes.canbedeep
                    , Privilege.Schema.Attributes.canbeglobal

                    , Privilege.Schema.Attributes.canbeentityreference
                    , Privilege.Schema.Attributes.canbeparententityreference
                ));

                _cachePrivileges.Add(service.ConnectionData.ConnectionId, temp);
            }

            return _cachePrivileges[service.ConnectionData.ConnectionId];
        }

        private async Task<IEnumerable<EntityMetadata>> GetEntityMetadataEnumerable(IOrganizationServiceExtented service)
        {
            if (!_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                EntityMetadataRepository repository = new EntityMetadataRepository(service);

                var temp = await repository.GetEntitiesDisplayNameWithPrivilegesAsync();

                _cacheEntityMetadata.Add(service.ConnectionData.ConnectionId, temp);
            }

            return _cacheEntityMetadata[service.ConnectionData.ConnectionId];
        }

        private IEnumerable<EntityMetadata> FilterEntityList(IEnumerable<EntityMetadata> list, string textName, RoleEditorLayoutTab selectedTab)
        {
            list = _entityMetadataFilter.FilterList(list);

            if (selectedTab != null)
            {
                list = list.Where(e => e.ObjectTypeCode.HasValue && selectedTab.EntitiesHash.Contains(e.ObjectTypeCode.Value));
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
                            ent.LogicalName.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1
                            ||
                            (
                                ent.DisplayName != null
                                && ent.DisplayName.LocalizedLabels
                                    .Where(l => !string.IsNullOrEmpty(l.Label))
                                    .Any(lbl => lbl.Label.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) > -1)
                            )
                        );
                    }
                }
            }

            return list;
        }

        private static IEnumerable<Privilege> FilterPrivilegeList(IEnumerable<Privilege> list, string textName, RoleEditorLayoutTab selectedTab)
        {
            if (selectedTab != null)
            {
                list = list.Where(p => p.PrivilegeId.HasValue && selectedTab.PrivilegesHash.Contains(p.PrivilegeId.Value));
            }

            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                if (Guid.TryParse(textName, out Guid tempGuid))
                {
                    list = list.Where(ent => ent.Id == tempGuid);
                }
                else
                {
                    list = list.Where(ent => ent.Name.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) != -1);
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

        protected override void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(connectionData, statusFormat, args);

            ToggleControl(this.tSProgressBar, cmBCurrentConnection, btnSetCurrentConnection, btnRefreshEntites, btnRefreshRoles, btnRefreshSystemUsers, btnRefreshTeams, tSProgressBar);

            UpdateSecurityRolesButtons();

            UpdateSystemUsersButtons();

            UpdateTeamButtons();
        }

        private void txtBFilterSystemUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowTeamSystemUsers();
            }
        }

        private void txtBEntityFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowTeamEntityPrivileges();
            }
        }

        private void cmBRoleEditorLayoutTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowTeamEntityPrivileges();
        }

        private void txtBFilterTeams_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowExistingTeams();
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowExistingTeams();
        }

        private void txtBFilterRole_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowTeamRoles();
            }
        }

        private Team GetSelectedTeam()
        {
            Team result = null;

            lstVwTeams.Dispatcher.Invoke(() =>
            {
                result = this.lstVwTeams.SelectedItems.OfType<Team>().Count() == 1
                    ? this.lstVwTeams.SelectedItems.OfType<Team>().SingleOrDefault() : null;
            });

            return result;
        }

        private List<Team> GetSelectedTeams()
        {
            List<Team> result = null;

            lstVwTeams.Dispatcher.Invoke(() =>
            {
                result = this.lstVwTeams.SelectedItems.OfType<Team>().ToList();
            });

            return result;
        }

        private EntityPrivilegeViewItem GetSelectedEntity()
        {
            EntityPrivilegeViewItem result = null;

            lstVwEntityPrivileges.Dispatcher.Invoke(() =>
            {
                result = this.lstVwEntityPrivileges.SelectedItems.OfType<EntityPrivilegeViewItem>().Count() == 1
                     ? this.lstVwEntityPrivileges.SelectedItems.OfType<EntityPrivilegeViewItem>().SingleOrDefault() : null;
            });

            return result;
        }

        private List<EntityPrivilegeViewItem> GetSelectedEntities()
        {
            List<EntityPrivilegeViewItem> result = null;

            lstVwEntityPrivileges.Dispatcher.Invoke(() =>
            {
                result = this.lstVwEntityPrivileges.SelectedItems.OfType<EntityPrivilegeViewItem>().ToList();
            });

            return result;
        }

        private List<Role> GetSelectedRoles()
        {
            List<Role> result = null;

            lstVwSecurityRoles.Dispatcher.Invoke(() =>
            {
                result = this.lstVwSecurityRoles.SelectedItems.OfType<Role>().ToList();
            });

            return result;
        }

        private List<SystemUser> GetSelectedSystemUsers()
        {
            List<SystemUser> result = null;

            lstVwSystemUsers.Dispatcher.Invoke(() =>
            {
                result = this.lstVwSystemUsers.SelectedItems.OfType<SystemUser>().ToList();
            });

            return result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lstVwEntity_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Entity entity = GetItemFromRoutedDataContext<Entity>(e);

                if (entity != null)
                {
                    ConnectionData connectionData = GetSelectedConnection();

                    if (connectionData != null)
                    {
                        connectionData.OpenEntityInstanceInWeb(entity.LogicalName, entity.Id);
                    }
                }
            }
        }

        private void LstVwEntityPrivileges_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                EntityPrivilegeViewItem item = GetItemFromRoutedDataContext<EntityPrivilegeViewItem>(e);

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

            var entityNamesOrdered = string.Join(", ", entityNames.OrderBy(s => s));

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

        private void lstVwTeam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshTeamInfo();
        }

        private void lstVwSecurityRoles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSecurityRolesButtons();
        }

        private void UpdateSecurityRolesButtons()
        {
            this.lstVwSecurityRoles.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwSecurityRoles != null && this.lstVwSecurityRoles.SelectedItems.Count > 0;

                    UIElement[] list = { btnRemoveRoleFromTeam };

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

        private void lstVwSystemUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSystemUsersButtons();
        }

        private void UpdateSystemUsersButtons()
        {
            this.lstVwSystemUsers.Dispatcher.Invoke(() =>
            {
                try
                {
                    var team = GetSelectedTeam();

                    bool enabled = this.IsControlsEnabled
                        && team != null
                        && !team.IsDefault.GetValueOrDefault()
                        && this.lstVwSystemUsers != null
                        && this.lstVwSystemUsers.SelectedItems.Count > 0;

                    UIElement[] list = { btnRemoveUserFromTeam };

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

        private void UpdateTeamButtons()
        {
            this.lstVwTeams.Dispatcher.Invoke(() =>
            {
                try
                {
                    {
                        bool enabled = this.IsControlsEnabled
                            && this.lstVwTeams != null
                            && this.lstVwTeams.SelectedItems.Count > 0
                            ;

                        UIElement[] list = { btnAssignRoleToTeam };

                        foreach (var button in list)
                        {
                            button.IsEnabled = enabled;
                        }
                    }

                    {
                        bool enabled = this.IsControlsEnabled
                            && this.lstVwTeams != null
                            && this.lstVwTeams.SelectedItems.OfType<Team>().Any(r => !r.IsDefault.GetValueOrDefault())
                            ;

                        UIElement[] list = { btnAddUserToTeam };

                        foreach (var button in list)
                        {
                            button.IsEnabled = enabled;
                        }
                    }
                }
                catch (Exception)
                {
                }
            });
        }

        private async Task RefreshTeamInfo()
        {
            var service = await GetService();

            try
            {
                await ShowTeamRoles();

                await ShowTeamSystemUsers();

                await ShowTeamEntityPrivileges();
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        protected override void OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            ShowExistingTeams();
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
            await AddEntityToSolution(true, null, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0);
        }

        private async void AddToCrmSolutionDoNotIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            await AddEntityToSolution(true, null, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Do_not_include_subcomponents_1);
        }

        private async void AddToCrmSolutionIncludeAsShellOnly_Click(object sender, RoutedEventArgs e)
        {
            await AddEntityToSolution(true, null, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_As_Shell_Only_2);
        }

        private async void AddToCrmSolutionLastIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddEntityToSolution(false, solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_Subcomponents_0);
            }
        }

        private async void AddToCrmSolutionLastDoNotIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddEntityToSolution(false, solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Do_not_include_subcomponents_1);
            }
        }

        private async void AddToCrmSolutionLastIncludeAsShellOnly_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddEntityToSolution(false, solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior.Include_As_Shell_Only_2);
            }
        }

        private async Task AddEntityToSolution(bool withSelect, string solutionUniqueName, SolutionComponent.Schema.OptionSets.rootcomponentbehavior rootComponentBehavior)
        {
            var entityList = GetSelectedEntities();

            if (entityList == null || !entityList.Any())
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();
            var descriptor = GetDescriptor(service);

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.Entity, entityList.Select(item => item.EntityMetadata.MetadataId.Value).ToList(), rootComponentBehavior, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        private void ContextMenuEntityPrivileges_Opened(object sender, RoutedEventArgs e)
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
            var descriptor = GetDescriptor(service);

            WindowHelper.OpenSolutionComponentDependenciesExplorer(_iWriteToOutput, service, descriptor, _commonConfig, (int)ComponentType.Entity, entity.EntityMetadata.MetadataId.Value, null);
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

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                ShowExistingTeams();
            }
        }

        private void mIClearEntityCacheAndRefresh_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                _cacheEntityMetadata.Remove(connectionData.ConnectionId);
                _cachePrivileges.Remove(connectionData.ConnectionId);

                RefreshTeamInfo();
            }
        }

        private void mIOpenEntityInstanceInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is MenuItem menuItem))
            {
                return;
            }

            if (menuItem.DataContext == null
                || !(menuItem.DataContext is Entity entity)
                )
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                connectionData.OpenEntityInstanceInWeb(entity.LogicalName, entity.Id);
            }
        }

        private void mICopyEntityInstanceIdToClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is MenuItem menuItem))
            {
                return;
            }

            if (menuItem.DataContext == null
                || !(menuItem.DataContext is Entity entity)
                )
            {
                return;
            }

            ClipboardHelper.SetText(entity.Id.ToString());
        }

        private void mICopyEntityInstanceUrlToClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is MenuItem menuItem))
            {
                return;
            }

            if (menuItem.DataContext == null
                || !(menuItem.DataContext is Entity entity)
                )
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                var url = connectionData.GetEntityInstanceUrl(entity.LogicalName, entity.Id);

                ClipboardHelper.SetText(url);
            }
        }

        private async void mIOpenEntityExplorer_Click(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is MenuItem menuItem))
            {
                return;
            }

            if (menuItem.DataContext == null
                || !(menuItem.DataContext is Entity entity)
                )
            {
                return;
            }

            var service = await GetService();

            IEnumerable<EntityMetadata> entityMetadataList = GetEntityMetadataList(service.ConnectionData.ConnectionId);
            IEnumerable<Privilege> privileges = GetOtherPrivilegesList(service.ConnectionData.ConnectionId);

            switch (entity)
            {
                case Role role:
                    WindowHelper.OpenRolesExplorer(_iWriteToOutput, service, _commonConfig, role.Name, entityMetadataList, privileges);
                    break;

                case SystemUser user:
                    WindowHelper.OpenSystemUsersExplorer(_iWriteToOutput, service, _commonConfig, user.FullName, entityMetadataList, privileges);
                    break;

                case Team team:
                    WindowHelper.OpenTeamsExplorer(_iWriteToOutput, service, _commonConfig, team.Name, entityMetadataList, privileges);
                    break;
            }
        }

        private async void mICreateEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is MenuItem menuItem))
            {
                return;
            }

            if (menuItem.DataContext == null
                || !(menuItem.DataContext is Entity entity)
                )
            {
                return;
            }

            var service = await GetService();

            var entityFull = service.RetrieveByQuery<Entity>(entity.LogicalName, entity.Id, new ColumnSet(true));

            string fileName = EntityFileNameFormatter.GetEntityName(service.ConnectionData.Name, entityFull, EntityFileNameFormatter.Headers.EntityDescription, "txt");
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, entityFull, null, service.ConnectionData);

            _iWriteToOutput.WriteToOutput(service.ConnectionData
                , Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                , service.ConnectionData.Name
                , entityFull.LogicalName
                , filePath
            );

            _iWriteToOutput.PerformAction(service.ConnectionData, filePath);
        }

        private void mIOpenEntityInstanceListInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is MenuItem menuItem))
            {
                return;
            }

            if (menuItem.DataContext == null
                || !(menuItem.DataContext is Entity entity)
                )
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                connectionData.OpenEntityInstanceListInWeb(entity.LogicalName);
            }
        }

        private void ContextMenuRole_Opened(object sender, RoutedEventArgs e)
        {
            if (sender is ContextMenu contextMenu)
            {
                var items = contextMenu.Items.OfType<Control>();

                ConnectionData connectionData = GetSelectedConnection();

                FillLastSolutionItems(connectionData, items, true, AddRoleToCrmSolutionLast_Click, "contMnAddToSolutionLast");
            }
        }

        private async void AddRoleToCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddRoleToSolution(true, null);
        }

        private async void AddRoleToCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddRoleToSolution(false, solutionUniqueName);
            }
        }

        private async Task AddRoleToSolution(bool withSelect, string solutionUniqueName)
        {
            var roleList = GetSelectedRoles();

            if (roleList == null || !roleList.Any())
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();
            var descriptor = GetDescriptor(service);

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.Role, roleList.Select(item => item.Id).ToList(), null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        private async void btnAssignRoleToTeam_Click(object sender, RoutedEventArgs e)
        {
            var teamList = (IEnumerable<Team>)GetSelectedTeams();

            if (teamList == null)
            {
                return;
            }

            teamList = teamList.Where(t => t.TeamType?.Value == (int)Team.Schema.OptionSets.teamtype.Owner_0 && !t.IsDefault.GetValueOrDefault());

            if (!teamList.Any())
            {
                return;
            }

            var service = await GetService();

            var repository = new RoleRepository(service);

            Func<string, Task<IEnumerable<Role>>> getter = null;

            if (teamList.Count() == 1)
            {
                var team = teamList.First();

                getter = (string filter) => repository.GetAvailableRolesForTeamAsync(filter, team.Id, new ColumnSet(
                                 Role.Schema.Attributes.name
                                 , Role.Schema.Attributes.businessunitid
                                 , Role.Schema.Attributes.ismanaged
                                 , Role.Schema.Attributes.iscustomizable
                                 ));
            }
            else
            {
                getter = (string filter) => repository.GetListAsync(filter, new ColumnSet(
                                 Role.Schema.Attributes.name
                                 , Role.Schema.Attributes.businessunitid
                                 , Role.Schema.Attributes.ismanaged
                                 , Role.Schema.Attributes.iscustomizable
                                 ));
            }

            IEnumerable<DataGridColumn> columns = Helpers.SolutionComponentDescription.Implementation.RoleDescriptionBuilder.GetDataGridColumn();

            var form = new WindowEntitySelect<Role>(_iWriteToOutput, service.ConnectionData, Role.EntityLogicalName, getter, columns);

            if (!form.ShowDialog().GetValueOrDefault())
            {
                return;
            }

            if (form.SelectedEntity == null)
            {
                return;
            }

            var role = form.SelectedEntity;

            string rolesName = role.Name;
            string teamsName = string.Join(", ", teamList.Select(r => r.Name).OrderBy(s => s));

            string operationName = string.Format(Properties.OperationNames.AssigningRolesToTeamsFormat3, service.ConnectionData.Name, rolesName, teamsName);

            _iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operationName);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.AssigningRolesToTeamsFormat3, service.ConnectionData.Name, rolesName, teamsName);

            var repositoryRolePrivileges = new RolePrivilegesRepository(service);

            foreach (var team in teamList)
            {
                try
                {
                    await repositoryRolePrivileges.AssignRolesToTeamAsync(team.Id, new[] { role.Id });
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                }
            }

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.AssigningRolesToTeamsCompletedFormat3, service.ConnectionData.Name, rolesName, teamsName);

            _iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operationName);

            RefreshTeamInfo();
        }

        private async void btnRemoveRoleFromTeam_Click(object sender, RoutedEventArgs e)
        {
            var team = GetSelectedTeam();

            var roleList = GetSelectedRoles();

            if (team == null || roleList == null || !roleList.Any())
            {
                return;
            }

            string rolesName = string.Join(", ", roleList.Select(u => u.Name).OrderBy(s => s));
            string teamsName = team.Name;

            string message = string.Format(Properties.MessageBoxStrings.AreYouSureRemoveRolesFromTeamsFormat2, rolesName, teamsName);

            if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }

            var service = await GetService();

            string operationName = string.Format(Properties.OperationNames.RemovingRolesFromTeamsFormat3, service.ConnectionData.Name, rolesName, teamsName);

            _iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operationName);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.RemovingRolesFromTeamsFormat3, service.ConnectionData.Name, rolesName, teamsName);

            try
            {
                var repository = new RolePrivilegesRepository(service);

                await repository.RemoveRolesFromTeamAsync(team.Id, roleList.Select(r => r.Id));
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.RemovingRolesFromTeamsCompletedFormat3, service.ConnectionData.Name, rolesName, teamsName);

            _iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operationName);

            RefreshTeamInfo();
        }

        private async void btnAddUserToTeam_Click(object sender, RoutedEventArgs e)
        {
            var teamList = (IEnumerable<Team>)GetSelectedTeams();

            if (teamList == null)
            {
                return;
            }

            teamList = teamList.Where(t => t.TeamType?.Value == (int)Team.Schema.OptionSets.teamtype.Owner_0 && !t.IsDefault.GetValueOrDefault());

            if (!teamList.Any())
            {
                return;
            }

            var service = await GetService();

            var repository = new SystemUserRepository(service);

            Func<string, Task<IEnumerable<SystemUser>>> getter = null;

            if (teamList.Count() == 1)
            {
                var team = teamList.First();

                getter = (string filter) => repository.GetAvailableUsersForTeamAsync(filter, team.Id, new ColumnSet(
                                SystemUser.Schema.Attributes.domainname
                                , SystemUser.Schema.Attributes.fullname
                                , SystemUser.Schema.Attributes.businessunitid
                                , SystemUser.Schema.Attributes.isdisabled
                                ));
            }
            else
            {
                getter = (string filter) => repository.GetUsersAsync(filter, new ColumnSet(
                                SystemUser.Schema.Attributes.domainname
                                , SystemUser.Schema.Attributes.fullname
                                , SystemUser.Schema.Attributes.businessunitid
                                , SystemUser.Schema.Attributes.isdisabled
                                ));
            }

            IEnumerable<DataGridColumn> columns = SystemUserRepository.GetDataGridColumn();

            var form = new WindowEntitySelect<SystemUser>(_iWriteToOutput, service.ConnectionData, SystemUser.EntityLogicalName, getter, columns);

            if (!form.ShowDialog().GetValueOrDefault())
            {
                return;
            }

            if (form.SelectedEntity == null)
            {
                return;
            }

            var user = form.SelectedEntity;

            string usersName = string.Format("{0} - {1}", user.DomainName, user.FullName);
            string teamsName = string.Join(", ", teamList.Select(r => r.Name).OrderBy(s => s));

            string operationName = string.Format(Properties.OperationNames.AddingUsersToTeamsFormat3, service.ConnectionData.Name, usersName, teamsName);

            _iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operationName);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.AddingUsersToTeamsFormat3, service.ConnectionData.Name, usersName, teamsName);

            try
            {
                var repositoryTeam = new TeamRepository(service);

                await repositoryTeam.AddUserFromTeamsAsync(form.SelectedEntity.Id, teamList.Select(r => r.Id));
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.AddingUsersToTeamsCompletedFormat3, service.ConnectionData.Name, usersName, teamsName);

            _iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operationName);

            RefreshTeamInfo();
        }

        private async void btnRemoveUserFromTeam_Click(object sender, RoutedEventArgs e)
        {
            var team = GetSelectedTeam();

            var userList = GetSelectedSystemUsers();

            if (team == null || team.IsDefault.GetValueOrDefault() || userList == null || !userList.Any())
            {
                return;
            }

            string usersName = string.Join(", ", userList.Select(r => string.Format("{0} - {1}", r.DomainName, r.FullName)).OrderBy(s => s));
            string teamsName = team.Name;

            string message = string.Format(Properties.MessageBoxStrings.AreYouSureRemoveUsersFromTeamsFormat2, usersName, teamsName);

            if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }

            var service = await GetService();

            string operationName = string.Format(Properties.OperationNames.RemovingUsersFromTeamsFormat3, service.ConnectionData.Name, usersName, teamsName);

            _iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operationName);

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.RemovingUsersFromTeamsFormat3, service.ConnectionData.Name, usersName, teamsName);

            try
            {
                var repository = new TeamRepository(service);

                await repository.RemoveUsersFromTeamAsync(team.Id, userList.Select(r => r.Id));
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.RemovingUsersFromTeamsCompletedFormat3, service.ConnectionData.Name, usersName, teamsName);

            _iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operationName);

            RefreshTeamInfo();
        }

        private void btnRefreshSystemUsers_Click(object sender, RoutedEventArgs e)
        {
            ShowTeamSystemUsers();
        }

        private void btnRefreshRoles_Click(object sender, RoutedEventArgs e)
        {
            ShowTeamRoles();
        }

        private void btnRefreshTeams_Click(object sender, RoutedEventArgs e)
        {
            ShowExistingTeams();
        }

        private void btnRefreshEntites_Click(object sender, RoutedEventArgs e)
        {
            ShowTeamEntityPrivileges();
        }

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, GetSelectedConnection());
        }

        private async void mIRolePrivilegesWithRolePrivileges_Click(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is MenuItem menuItem))
            {
                return;
            }

            if (menuItem.DataContext == null
                || !(menuItem.DataContext is Entity entity)
            )
            {
                return;
            }

            var team1 = entity.ToEntity<Team>();

            var service = await GetService();

            var repositoryRole = new RoleRepository(service);

            Func<string, Task<IEnumerable<Role>>> getter = (string filter) => repositoryRole.GetListAsync(filter
                , new ColumnSet(
                    Role.Schema.Attributes.name
                    , Role.Schema.Attributes.businessunitid
                    , Role.Schema.Attributes.ismanaged
                    , Role.Schema.Attributes.iscustomizable
                )
            );

            IEnumerable<DataGridColumn> columns = Helpers.SolutionComponentDescription.Implementation.RoleDescriptionBuilder.GetDataGridColumn();

            var form = new WindowEntitySelect<Role>(_iWriteToOutput, service.ConnectionData, Role.EntityLogicalName, getter, columns);

            if (!form.ShowDialog().GetValueOrDefault())
            {
                return;
            }

            if (form.SelectedEntity == null)
            {
                return;
            }

            var role2 = form.SelectedEntity;

            string name1 = string.Format("Team {0}", team1.Name);
            string name2 = string.Format("Role {0}", role2.Name);

            StringBuilder content = new StringBuilder();

            content.AppendLine(Properties.OutputStrings.ConnectingToCRM);
            content.AppendLine(service.ConnectionData.GetConnectionDescription());
            content.AppendFormat(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint).AppendLine();

            string operation = string.Format(Properties.OperationNames.ComparingEntitiesPrivilegesFormat3, service.ConnectionData.Name, name1, name2);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operation));

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ComparingEntitiesPrivilegesFormat2, name1, name2);

            var repositoryRolePrivileges = new RolePrivilegesRepository(service);

            var teamPrivileges1 = await repositoryRolePrivileges.GetTeamPrivilegesAsync(team1.Id);
            var rolePrivileges2 = await repositoryRolePrivileges.GetRolePrivilegesAsync(role2.Id);

            var hashPrivileges = new HashSet<Guid>(teamPrivileges1.Select(p => p.PrivilegeId).Union(rolePrivileges2.Select(p => p.PrivilegeId)));

            var repositoryPrivilege = new PrivilegeRepository(service);
            var privileges = await repositoryPrivilege.GetListByIdsAsync(hashPrivileges);

            var comparer = new RolePrivilegeComparerHelper(_tabSpacer, name1, name2);

            content.AppendLine();

            var difference = comparer.CompareRolePrivileges(teamPrivileges1, rolePrivileges2, privileges, PrivilegeNameComparer.Comparer);

            difference.ForEach(s => content.AppendLine(s));

            content.AppendLine();

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ComparingEntitiesPrivilegesCompletedFormat2, name1, name2);

            content.AppendLine(_iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operation));

            string fileName = EntityFileNameFormatter.ComparingRolePrivilegesInEntitiesFileName(service.ConnectionData.Name, name1, name2);

            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            _iWriteToOutput.PerformAction(service.ConnectionData, filePath);
        }

        private async void mIRolePrivilegesWithUserPrivileges_Click(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is MenuItem menuItem))
            {
                return;
            }

            if (menuItem.DataContext == null
                || !(menuItem.DataContext is Entity entity)
            )
            {
                return;
            }

            var team1 = entity.ToEntity<Team>();

            var service = await GetService();

            var repositoryUser = new SystemUserRepository(service);

            Func<string, Task<IEnumerable<SystemUser>>> getter = (string filter) => repositoryUser.GetUsersAsync(filter, new ColumnSet(
                                SystemUser.Schema.Attributes.domainname
                                , SystemUser.Schema.Attributes.fullname
                                , SystemUser.Schema.Attributes.businessunitid
                                , SystemUser.Schema.Attributes.isdisabled
            ));

            IEnumerable<DataGridColumn> columns = SystemUserRepository.GetDataGridColumn();

            var form = new WindowEntitySelect<SystemUser>(_iWriteToOutput, service.ConnectionData, SystemUser.EntityLogicalName, getter, columns);

            if (!form.ShowDialog().GetValueOrDefault())
            {
                return;
            }

            if (form.SelectedEntity == null)
            {
                return;
            }

            var user2 = form.SelectedEntity;

            string name1 = string.Format("Team {0}", team1.Name);
            string name2 = string.Format("User {0}", user2.FullName);

            StringBuilder content = new StringBuilder();

            content.AppendLine(Properties.OutputStrings.ConnectingToCRM);
            content.AppendLine(service.ConnectionData.GetConnectionDescription());
            content.AppendFormat(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint).AppendLine();

            string operation = string.Format(Properties.OperationNames.ComparingEntitiesPrivilegesFormat3, service.ConnectionData.Name, name1, name2);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operation));

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ComparingEntitiesPrivilegesFormat2, name1, name2);

            var repositoryRolePrivileges = new RolePrivilegesRepository(service);

            var teamPrivileges1 = await repositoryRolePrivileges.GetTeamPrivilegesAsync(team1.Id);
            var userPrivileges2 = await repositoryRolePrivileges.GetUserPrivilegesAsync(user2.Id);

            var hashPrivileges = new HashSet<Guid>(teamPrivileges1.Select(p => p.PrivilegeId).Union(userPrivileges2.Select(p => p.PrivilegeId)));

            var repositoryPrivilege = new PrivilegeRepository(service);
            var privileges = await repositoryPrivilege.GetListByIdsAsync(hashPrivileges);

            var comparer = new RolePrivilegeComparerHelper(_tabSpacer, name1, name2);

            content.AppendLine();

            var difference = comparer.CompareRolePrivileges(teamPrivileges1, userPrivileges2, privileges, PrivilegeNameComparer.Comparer);

            difference.ForEach(s => content.AppendLine(s));

            content.AppendLine();

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ComparingEntitiesPrivilegesCompletedFormat2, name1, name2);

            content.AppendLine(_iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operation));

            string fileName = EntityFileNameFormatter.ComparingRolePrivilegesInEntitiesFileName(service.ConnectionData.Name, name1, name2);

            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            _iWriteToOutput.PerformAction(service.ConnectionData, filePath);
        }

        private async void mIRolePrivilegesWithTeamPrivileges_Click(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is MenuItem menuItem))
            {
                return;
            }

            if (menuItem.DataContext == null
                || !(menuItem.DataContext is Entity entity)
            )
            {
                return;
            }

            var team1 = entity.ToEntity<Team>();

            var service = await GetService();

            var repositoryTeam = new TeamRepository(service);

            Func<string, Task<IEnumerable<Team>>> getter = (string filter) => repositoryTeam.GetOwnerTeamsNotAnotherAsync(filter
                , team1.Id
                , new ColumnSet(
                    Team.Schema.Attributes.name
                    , Team.Schema.Attributes.businessunitid
                    , Team.Schema.Attributes.isdefault
            ));

            IEnumerable<DataGridColumn> columns = TeamRepository.GetDataGridColumnOwner();

            var form = new WindowEntitySelect<Team>(_iWriteToOutput, service.ConnectionData, Team.EntityLogicalName, getter, columns);

            if (!form.ShowDialog().GetValueOrDefault())
            {
                return;
            }

            if (form.SelectedEntity == null)
            {
                return;
            }

            var team2 = form.SelectedEntity;

            string name1 = string.Format("Team {0}", team1.Name);
            string name2 = string.Format("Team {0}", team2.Name);

            StringBuilder content = new StringBuilder();

            content.AppendLine(Properties.OutputStrings.ConnectingToCRM);
            content.AppendLine(service.ConnectionData.GetConnectionDescription());
            content.AppendFormat(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint).AppendLine();

            string operation = string.Format(Properties.OperationNames.ComparingEntitiesPrivilegesFormat3, service.ConnectionData.Name, name1, name2);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operation));

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.ComparingEntitiesPrivilegesFormat2, name1, name2);

            var repositoryRolePrivileges = new RolePrivilegesRepository(service);

            var teamPrivileges1 = await repositoryRolePrivileges.GetTeamPrivilegesAsync(team1.Id);
            var teamPrivileges2 = await repositoryRolePrivileges.GetTeamPrivilegesAsync(team2.Id);

            var hashPrivileges = new HashSet<Guid>(teamPrivileges1.Select(p => p.PrivilegeId).Union(teamPrivileges2.Select(p => p.PrivilegeId)));

            var repositoryPrivilege = new PrivilegeRepository(service);
            var privileges = await repositoryPrivilege.GetListByIdsAsync(hashPrivileges);

            var comparer = new RolePrivilegeComparerHelper(_tabSpacer, name1, name2);

            content.AppendLine();

            var difference = comparer.CompareRolePrivileges(teamPrivileges1, teamPrivileges2, privileges, PrivilegeNameComparer.Comparer);

            difference.ForEach(s => content.AppendLine(s));

            content.AppendLine();

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.ComparingEntitiesPrivilegesCompletedFormat2, name1, name2);

            content.AppendLine(_iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operation));

            string fileName = EntityFileNameFormatter.ComparingRolePrivilegesInEntitiesFileName(service.ConnectionData.Name, name1, name2);

            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            _iWriteToOutput.PerformAction(service.ConnectionData, filePath);
        }

        #region Other Privilege

        private OtherPrivilegeViewItem GetSelectedOtherPrivilege()
        {
            return this.lstVwOtherPrivileges.SelectedItems.OfType<OtherPrivilegeViewItem>().Count() == 1
                ? this.lstVwOtherPrivileges.SelectedItems.OfType<OtherPrivilegeViewItem>().SingleOrDefault() : null;
        }

        private List<OtherPrivilegeViewItem> GetSelectedOtherPrivileges()
        {
            var result = this.lstVwOtherPrivileges.SelectedItems.OfType<OtherPrivilegeViewItem>().ToList();

            return result;
        }

        private void ContextMenuOtherPrivilege_Opened(object sender, RoutedEventArgs e)
        {
            if (!(sender is ContextMenu contextMenu))
            {
                return;
            }

            var items = contextMenu.Items.OfType<Control>();

            ConnectionData connectionData = GetSelectedConnection();

            FillLastSolutionItems(connectionData, items, true, AddOtherPrivilegeToCrmSolutionLast_Click, "contMnAddOtherPrivilegeToSolutionLast");
        }

        private void mIOtherPrivilegeOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            var privilege = GetSelectedOtherPrivilege();

            if (privilege == null)
            {
                return;
            }

            var connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.Privilege, privilege.Privilege.Id);
            }
        }

        private async void mIOtherPrivilegeOpenDependentComponentsInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var privilege = GetSelectedOtherPrivilege();

            if (privilege == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSolutionComponentDependenciesExplorer(_iWriteToOutput, service, null, _commonConfig, (int)ComponentType.Privilege, privilege.Privilege.Id, null);
        }

        private async void mIOtherPrivilegeOpenSolutionsContainingComponentInExplorer_Click(object sender, RoutedEventArgs e)
        {
            var privilege = GetSelectedOtherPrivilege();

            if (privilege == null)
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenExplorerSolutionExplorer(
                _iWriteToOutput
                , service
                , _commonConfig
                , (int)ComponentType.Privilege
                , privilege.Privilege.Id
                , null
            );
        }

        private async void AddOtherPrivilegeToCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddOtherPrivilegeToSolution(true, null);
        }

        private async void AddOtherPrivilegeToCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddOtherPrivilegeToSolution(false, solutionUniqueName);
            }
        }

        private async Task AddOtherPrivilegeToSolution(bool withSelect, string solutionUniqueName)
        {
            var otherPrivilegesList = GetSelectedOtherPrivileges();

            if (otherPrivilegesList == null || !otherPrivilegesList.Any())
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupToSolution(_iWriteToOutput, service, null, _commonConfig, solutionUniqueName, ComponentType.Privilege, otherPrivilegesList.Select(item => item.Privilege.Id).ToList(), null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        #endregion Other Privilege
    }
}