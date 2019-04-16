using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.SolutionComponentDescription;
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

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowSystemUserExplorer : WindowBase
    {
        private string _tabSpacer = "    ";

        private readonly object sysObjectConnections = new object();

        private readonly IWriteToOutput _iWriteToOutput;

        private readonly CommonConfiguration _commonConfig;

        private readonly ObservableCollection<SystemUser> _itemsSourceSystemUsers;

        private readonly ObservableCollection<Team> _itemsSourceTeams;
        private readonly ObservableCollection<Role> _itemsSourceRoles;
        private readonly ObservableCollection<Role> _itemsSourceRolesByTeams;

        private readonly ObservableCollection<EntityPrivilegeViewItem> _itemsSourceEntityPrivileges;
        private readonly ObservableCollection<OtherPrivilegeViewItem> _itemsSourceOtherPrivileges;

        private readonly Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();
        private readonly Dictionary<Guid, SolutionComponentDescriptor> _descriptorCache = new Dictionary<Guid, SolutionComponentDescriptor>();

        private readonly Dictionary<Guid, IEnumerable<EntityMetadata>> _cacheEntityMetadata = new Dictionary<Guid, IEnumerable<EntityMetadata>>();
        private readonly Dictionary<Guid, IEnumerable<Privilege>> _cachePrivileges = new Dictionary<Guid, IEnumerable<Privilege>>();

        public WindowSystemUserExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , IEnumerable<EntityMetadata> entityMetadataList
            , IEnumerable<Privilege> privileges
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

            if (privileges != null)
            {
                _cachePrivileges[service.ConnectionData.ConnectionId] = privileges;
            }

            BindingOperations.EnableCollectionSynchronization(service.ConnectionData.ConnectionConfiguration.Connections, sysObjectConnections);

            InitializeComponent();

            LoadFromConfig();

            txtBFilterSystemUser.Text = filterEntity;
            txtBFilterSystemUser.SelectionLength = 0;
            txtBFilterSystemUser.SelectionStart = txtBFilterSystemUser.Text.Length;

            txtBFilterSystemUser.Focus();

            lstVwSystemUsers.ItemsSource = _itemsSourceSystemUsers = new ObservableCollection<SystemUser>();

            lstVwTeams.ItemsSource = _itemsSourceTeams = new ObservableCollection<Team>();
            lstVwSecurityRoles.ItemsSource = _itemsSourceRoles = new ObservableCollection<Role>();
            lstVwSecurityRolesByTeams.ItemsSource = _itemsSourceRolesByTeams = new ObservableCollection<Role>();
            lstVwEntityPrivileges.ItemsSource = _itemsSourceEntityPrivileges = new ObservableCollection<EntityPrivilegeViewItem>();
            lstVwOtherPrivileges.ItemsSource = _itemsSourceOtherPrivileges = new ObservableCollection<OtherPrivilegeViewItem>();

            UpdateSystemUsersButtons();

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            this.DecreaseInit();

            ShowExistingSystemUsers();
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

            if (connectionData != null)
            {
                if (!_connectionCache.ContainsKey(connectionData.ConnectionId))
                {
                    ToggleControls(connectionData, false, string.Empty);

                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);
                    _iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());
                    var service = await QuickConnection.ConnectAsync(connectionData);
                    _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

                    _connectionCache[connectionData.ConnectionId] = service;

                    ToggleControls(connectionData, true, string.Empty);
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

                    _descriptorCache[connectionData.ConnectionId] = new SolutionComponentDescriptor(service);
                }

                _descriptorCache[connectionData.ConnectionId].SetSettings(_commonConfig);

                return _descriptorCache[connectionData.ConnectionId];
            }

            return null;
        }

        private async Task ShowExistingSystemUsers()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.LoadingSystemUsers);

            string textName = string.Empty;

            this.Dispatcher.Invoke(() =>
            {
                _itemsSourceSystemUsers.Clear();

                _itemsSourceTeams.Clear();

                _itemsSourceRoles.Clear();
                _itemsSourceRolesByTeams.Clear();

                _itemsSourceEntityPrivileges.Clear();
                _itemsSourceOtherPrivileges.Clear();

                textName = txtBFilterSystemUser.Text.Trim().ToLower();
            });

            IEnumerable<SystemUser> list = Enumerable.Empty<SystemUser>();

            try
            {
                if (service != null)
                {
                    SystemUserRepository repository = new SystemUserRepository(service);

                    list = await repository.GetListAsync(textName, new ColumnSet(SystemUser.Schema.Attributes.fullname, SystemUser.Schema.Attributes.domainname, SystemUser.Schema.Attributes.businessunitid, SystemUser.Schema.Attributes.isdisabled));
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            this.lstVwSystemUsers.Dispatcher.Invoke(() =>
            {
                foreach (var entity in list.OrderBy(s => s.FullName).ThenBy(s => s.DomainName))
                {
                    _itemsSourceSystemUsers.Add(entity);
                }

                if (this.lstVwSystemUsers.Items.Count == 1)
                {
                    this.lstVwSystemUsers.SelectedItem = this.lstVwSystemUsers.Items[0];
                }
            });

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.LoadingSystemUsersCompletedFormat1, list.Count());

            await RefreshSystemUserInfo();
        }

        private async Task ShowSystemUserRoles()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.LoadingSystemUserSecurityRoles);

            string filterRole = string.Empty;

            this.Dispatcher.Invoke(() =>
            {
                _itemsSourceRoles.Clear();
                _itemsSourceRolesByTeams.Clear();

                filterRole = txtBFilterRole.Text.Trim().ToLower();
            });

            var user = GetSelectedSystemUser();

            IEnumerable<Role> list = Enumerable.Empty<Role>();
            IEnumerable<Role> listByTeams = Enumerable.Empty<Role>();

            try
            {
                if (service != null && user != null)
                {
                    RoleRepository repository = new RoleRepository(service);

                    list = await repository.GetUserRolesAsync(user.Id, filterRole
                        , new ColumnSet(
                                Role.Schema.Attributes.name
                                , Role.Schema.Attributes.businessunitid
                                , Role.Schema.Attributes.ismanaged
                                , Role.Schema.Attributes.iscustomizable
                                ));

                    listByTeams = await repository.GetUserRolesByTeamsAsync(user.Id, filterRole
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
                foreach (var entity in list.OrderBy(s => s.Name).ThenBy(s => s.BusinessUnitId?.Name))
                {
                    _itemsSourceRoles.Add(entity);
                }

                if (this.lstVwSecurityRoles.Items.Count == 1)
                {
                    this.lstVwSecurityRoles.SelectedItem = this.lstVwSecurityRoles.Items[0];
                }
            });

            this.lstVwSecurityRolesByTeams.Dispatcher.Invoke(() =>
            {
                foreach (var entity in listByTeams.OrderBy(s => s.Name).ThenBy(s => s.BusinessUnitId?.Name))
                {
                    _itemsSourceRolesByTeams.Add(entity);
                }

                if (this.lstVwSecurityRolesByTeams.Items.Count == 1)
                {
                    this.lstVwSecurityRolesByTeams.SelectedItem = this.lstVwSecurityRolesByTeams.Items[0];
                }
            });

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.LoadingSystemUserSecurityRolesCompleted);
        }

        private async Task ShowSystemUserTeams()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.LoadingSystemUserTeams);

            string filterTeam = string.Empty;

            this.Dispatcher.Invoke(() =>
            {
                _itemsSourceTeams.Clear();

                filterTeam = txtBFilterTeams.Text.Trim().ToLower();
            });

            var user = GetSelectedSystemUser();

            IEnumerable<Team> list = Enumerable.Empty<Team>();

            try
            {
                if (service != null && user != null)
                {
                    TeamRepository repository = new TeamRepository(service);

                    list = await repository.GetUserTeamsAsync(user.Id, filterTeam
                        , new ColumnSet(
                                Team.Schema.Attributes.name
                                , Team.Schema.Attributes.businessunitid
                                , Team.Schema.Attributes.teamtype
                                , Team.Schema.Attributes.regardingobjectid
                                , Team.Schema.Attributes.teamtemplateid
                                , Team.Schema.Attributes.isdefault
                                ));
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

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.LoadingSystemUserTeamsCompletedFormat1, list.Count());
        }

        private async Task ShowSystemUserEntityPrivileges()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.LoadingEntities);

            this.Dispatcher.Invoke(() =>
            {
                _itemsSourceEntityPrivileges.Clear();
                _itemsSourceOtherPrivileges.Clear();
            });

            var user = GetSelectedSystemUser();

            IEnumerable<EntityMetadata> entityMetadataList = Enumerable.Empty<EntityMetadata>();

            IEnumerable<EntityPrivilegeViewItem> listEntityPrivileges = Enumerable.Empty<EntityPrivilegeViewItem>();

            IEnumerable<OtherPrivilegeViewItem> listOtherPrivileges = Enumerable.Empty<OtherPrivilegeViewItem>();

            try
            {
                if (service != null)
                {
                    var taskEntityMetadata = GetEntityMetadataEnumerable(service);
                    var taskPrivileges = GetPrivileges(service);

                    entityMetadataList = await taskEntityMetadata;
                    var otherPrivileges = await taskPrivileges;

                    entityMetadataList = entityMetadataList.Where(e => e.Privileges != null && e.Privileges.Any(p => p.PrivilegeType != PrivilegeType.None));

                    if (user != null)
                    {
                        string filterEntity = string.Empty;
                        string filterOtherPrivilege = string.Empty;

                        this.Dispatcher.Invoke(() =>
                        {
                            filterEntity = txtBEntityFilter.Text.Trim().ToLower();
                            filterOtherPrivilege = txtBOtherPrivilegesFilter.Text.Trim().ToLower();
                        });

                        entityMetadataList = FilterEntityList(entityMetadataList, filterEntity);
                        otherPrivileges = FilterPrivilegeList(otherPrivileges, filterOtherPrivilege);

                        if (entityMetadataList.Any() || otherPrivileges.Any())
                        {
                            var repository = new RolePrivilegesRepository(service);

                            var userPrivileges = await repository.GetUserPrivilegesAsync(user.Id);

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
                foreach (var entity in listEntityPrivileges.OrderBy(s => s.LogicalName))
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
                foreach (var otherPriv in listOtherPrivileges.OrderBy(s => s.EntityLogicalName).ThenBy(s => s.Name, new PrivilegeNameComparer()))
                {
                    _itemsSourceOtherPrivileges.Add(otherPriv);
                }

                if (this.lstVwOtherPrivileges.Items.Count == 1)
                {
                    this.lstVwOtherPrivileges.SelectedItem = this.lstVwOtherPrivileges.Items[0];
                }
            });

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.LoadingEntitiesCompletedFormat1, entityMetadataList.Count());
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

        private static IEnumerable<EntityMetadata> FilterEntityList(IEnumerable<EntityMetadata> list, string textName)
        {
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
                        );
                    }
                }
            }

            return list;
        }

        private static IEnumerable<Privilege> FilterPrivilegeList(IEnumerable<Privilege> list, string textName)
        {
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

        private void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(connectionData, statusFormat, args);

            ToggleControl(this.tSProgressBar, cmBCurrentConnection, btnSetCurrentConnection, btnRefreshEntites, btnRefreshRoles, btnRefreshSystemUsers, btnRefreshTeams, tSProgressBar);

            UpdateSystemUsersButtons();

            UpdateSecurityRolesButtons();

            UpdateTeamsButtons();
        }

        private void UpdateSystemUsersButtons()
        {
            this.lstVwSystemUsers.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled
                        && this.lstVwSystemUsers != null
                        && this.lstVwSystemUsers.SelectedItems.OfType<SystemUser>().Any();

                    UIElement[] list = { btnAssignRoleToUser, btnAddUserToTeam };

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

        private void txtBFilterSystemUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowExistingSystemUsers();
            }
        }

        private void txtBEntityFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowSystemUserEntityPrivileges();
            }
        }

        private void txtBFilterTeams_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowSystemUserTeams();
            }
        }

        private void txtBFilterRole_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ShowSystemUserRoles();
            }
        }

        private SystemUser GetSelectedSystemUser()
        {
            SystemUser result = null;

            lstVwSystemUsers.Dispatcher.Invoke(() =>
            {
                result = this.lstVwSystemUsers.SelectedItems.OfType<SystemUser>().Count() == 1
                    ? this.lstVwSystemUsers.SelectedItems.OfType<SystemUser>().SingleOrDefault() : null;
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

        private List<Team> GetSelectedTeams()
        {
            List<Team> result = null;

            lstVwTeams.Dispatcher.Invoke(() =>
            {
                result = this.lstVwTeams.SelectedItems.OfType<Team>().Where(u => !u.IsDefault.GetValueOrDefault()).ToList();
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

        private List<Role> GetSelectedRolesByTeam()
        {
            List<Role> result = null;

            lstVwSecurityRolesByTeams.Dispatcher.Invoke(() =>
            {
                result = this.lstVwSecurityRolesByTeams.SelectedItems.OfType<Role>().ToList();
            });

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

            WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, service, _commonConfig, entityMetadataList, entity?.LogicalName, null);
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

        private async void miEntitySecurityRolesExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedEntity();

            _commonConfig.Save();

            var service = await GetService();

            IEnumerable<EntityMetadata> entityMetadataList = null;

            if (_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId];
            }

            WindowHelper.OpenEntitySecurityRolesExplorer(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, entityMetadataList);
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
            var source = new SolutionComponentMetadataSource(service);

            var entityMetadata = source.GetEntityMetadata(entity.EntityMetadata.MetadataId.Value);

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

            WindowHelper.OpenSdkMessageRequestTreeWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, string.Empty);
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

        private void lstVwEntity_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var entity = ((FrameworkElement)e.OriginalSource).DataContext as Entity;

                if (entity != null)
                {
                    ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

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
                var item = ((FrameworkElement)e.OriginalSource).DataContext as EntityPrivilegeViewItem;

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

            var entityNamesOrdered = string.Join(", ", entityNames.OrderBy(s => s));

            this._iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, Properties.OperationNames.PublishingEntitiesFormat2, service.ConnectionData.Name, entityNamesOrdered);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.PublishingEntitiesFormat2, service.ConnectionData.Name, entityNamesOrdered);

            try
            {
                var repository = new PublishActionsRepository(service);

                await repository.PublishEntitiesAsync(entityNames);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.PublishingEntitiesCompletedFormat2, service.ConnectionData.Name, entityNamesOrdered);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.PublishingEntitiesFailedFormat2, service.ConnectionData.Name, entityNamesOrdered);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, Properties.OperationNames.PublishingEntitiesFormat2, service.ConnectionData.Name, entityNamesOrdered);
        }

        private void lstVwSystemUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshSystemUserInfo();
        }

        private async Task RefreshSystemUserInfo()
        {
            var service = await GetService();

            try
            {
                await ShowSystemUserRoles();

                await ShowSystemUserTeams();

                await ShowSystemUserEntityPrivileges();
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                ShowExistingSystemUsers();

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

        private async void AddIntoCrmSolutionIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            await AddEntityIntoSolution(true, null, RootComponentBehavior.IncludeSubcomponents);
        }

        private async void AddIntoCrmSolutionDoNotIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            await AddEntityIntoSolution(true, null, RootComponentBehavior.DoNotIncludeSubcomponents);
        }

        private async void AddIntoCrmSolutionIncludeAsShellOnly_Click(object sender, RoutedEventArgs e)
        {
            await AddEntityIntoSolution(true, null, RootComponentBehavior.IncludeAsShellOnly);
        }

        private async void AddIntoCrmSolutionLastIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddEntityIntoSolution(false, solutionUniqueName, RootComponentBehavior.IncludeSubcomponents);
            }
        }

        private async void AddIntoCrmSolutionLastDoNotIncludeSubcomponents_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddEntityIntoSolution(false, solutionUniqueName, RootComponentBehavior.DoNotIncludeSubcomponents);
            }
        }

        private async void AddIntoCrmSolutionLastIncludeAsShellOnly_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddEntityIntoSolution(false, solutionUniqueName, RootComponentBehavior.IncludeAsShellOnly);
            }
        }

        private async Task AddEntityIntoSolution(bool withSelect, string solutionUniqueName, RootComponentBehavior rootComponentBehavior)
        {
            var entityList = GetSelectedEntities();

            if (entityList == null || !entityList.Any())
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();
            var descriptor = await GetDescriptor();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.Entity, entityList.Select(item => item.EntityMetadata.MetadataId.Value).ToList(), rootComponentBehavior, withSelect);
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

                ConnectionData connectionData = null;

                cmBCurrentConnection.Dispatcher.Invoke(() =>
                {
                    connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
                });

                FillLastSolutionItems(connectionData, items, true, AddIntoCrmSolutionLastIncludeSubcomponents_Click, "contMnAddIntoSolutionLastIncludeSubcomponents");

                FillLastSolutionItems(connectionData, items, true, AddIntoCrmSolutionLastDoNotIncludeSubcomponents_Click, "contMnAddIntoSolutionLastDoNotIncludeSubcomponents");

                FillLastSolutionItems(connectionData, items, true, AddIntoCrmSolutionLastIncludeAsShellOnly_Click, "contMnAddIntoSolutionLastIncludeAsShellOnly");

                ActivateControls(items, connectionData.LastSelectedSolutionsUniqueName != null && connectionData.LastSelectedSolutionsUniqueName.Any(), "contMnAddIntoSolutionLast");
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
                , null
            );
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                ShowExistingSystemUsers();
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
                _cachePrivileges.Remove(connectionData.ConnectionId);

                RefreshSystemUserInfo();
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

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

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

            Clipboard.SetText(entity.Id.ToString());
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

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                var url = connectionData.GetEntityInstanceUrl(entity.LogicalName, entity.Id);

                Clipboard.SetText(url);
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

            IEnumerable<EntityMetadata> entityMetadataList = null;
            IEnumerable<Privilege> privileges = null;

            if (_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId];
            }

            if (_cachePrivileges.ContainsKey(service.ConnectionData.ConnectionId))
            {
                privileges = _cachePrivileges[service.ConnectionData.ConnectionId];
            }

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

            _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                   , service.ConnectionData.Name
                   , entityFull.LogicalName
                   , filePath);

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

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenEntityInstanceListInWeb(entity.LogicalName);
            }
        }

        private void mIOpenRoleTeamInWeb_Click(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is MenuItem menuItem))
            {
                return;
            }

            if (menuItem.DataContext == null
                || !(menuItem.DataContext is Role role)
                || !role.TeamId.HasValue
                )
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                connectionData.OpenEntityInstanceInWeb(Team.EntityLogicalName, role.TeamId.Value);
            }
        }

        private void mICopyRoleTeamIdToClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is MenuItem menuItem))
            {
                return;
            }

            if (menuItem.DataContext == null
                || !(menuItem.DataContext is Role role)
                || !role.TeamId.HasValue
                )
            {
                return;
            }

            Clipboard.SetText(role.TeamId.Value.ToString());
        }

        private void mICopyRoleTeamUrlToClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is MenuItem menuItem))
            {
                return;
            }

            if (menuItem.DataContext == null
                || !(menuItem.DataContext is Role role)
                || !role.TeamId.HasValue
                )
            {
                return;
            }

            ConnectionData connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;

            if (connectionData != null)
            {
                var url = connectionData.GetEntityInstanceUrl(Team.EntityLogicalName, role.TeamId.Value);

                Clipboard.SetText(url);
            }
        }

        private async void mIOpenRoleTeamExplorer_Click(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is MenuItem menuItem))
            {
                return;
            }

            if (menuItem.DataContext == null
                || !(menuItem.DataContext is Role role)
                || !role.TeamId.HasValue
                )
            {
                return;
            }

            var service = await GetService();

            IEnumerable<EntityMetadata> entityMetadataList = null;
            IEnumerable<Privilege> privileges = null;

            if (_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId];
            }

            if (_cachePrivileges.ContainsKey(service.ConnectionData.ConnectionId))
            {
                privileges = _cachePrivileges[service.ConnectionData.ConnectionId];
            }

            WindowHelper.OpenTeamsExplorer(_iWriteToOutput, service, _commonConfig, role.TeamName, entityMetadataList, privileges);
        }

        private async void mICreateRoleTeamEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is MenuItem menuItem))
            {
                return;
            }

            if (menuItem.DataContext == null
                || !(menuItem.DataContext is Role role)
                || !role.TeamId.HasValue
                )
            {
                return;
            }

            var service = await GetService();

            var entityFull = service.RetrieveByQuery<Entity>(Role.EntityLogicalName, role.TeamId.Value, new ColumnSet(true));

            string fileName = EntityFileNameFormatter.GetEntityName(service.ConnectionData.Name, entityFull, EntityFileNameFormatter.Headers.EntityDescription, "txt");
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            await EntityDescriptionHandler.ExportEntityDescriptionAsync(filePath, entityFull, null, service.ConnectionData);

            _iWriteToOutput.WriteToOutput(service.ConnectionData, Properties.OutputStrings.ExportedEntityDescriptionForConnectionFormat3
                   , service.ConnectionData.Name
                   , entityFull.LogicalName
                   , filePath);

            DTEHelper.Singleton?.PerformAction(service.ConnectionData, filePath);
        }

        private void ContextMenuRole_Opened(object sender, RoutedEventArgs e)
        {
            if (sender is ContextMenu contextMenu)
            {
                var items = contextMenu.Items.OfType<Control>();

                ConnectionData connectionData = null;

                cmBCurrentConnection.Dispatcher.Invoke(() =>
                {
                    connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
                });

                FillLastSolutionItems(connectionData, items, true, AddRoleIntoCrmSolutionLast_Click, "contMnAddIntoSolutionLast");
            }
        }

        private async void AddRoleIntoCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddRoleIntoSolution(true, null);
        }

        private async void AddRoleIntoCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddRoleIntoSolution(false, solutionUniqueName);
            }
        }

        private async Task AddRoleIntoSolution(bool withSelect, string solutionUniqueName)
        {
            var roleList = GetSelectedRoles();

            if (roleList == null || !roleList.Any())
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();
            var descriptor = await GetDescriptor();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.Role, roleList.Select(item => item.Id).ToList(), null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
        }

        private void ContextMenuRoleByTeam_Opened(object sender, RoutedEventArgs e)
        {
            if (sender is ContextMenu contextMenu)
            {
                var items = contextMenu.Items.OfType<Control>();

                ConnectionData connectionData = null;

                cmBCurrentConnection.Dispatcher.Invoke(() =>
                {
                    connectionData = cmBCurrentConnection.SelectedItem as ConnectionData;
                });

                FillLastSolutionItems(connectionData, items, true, AddRoleByTeamIntoCrmSolutionLast_Click, "contMnAddIntoSolutionLast");
            }
        }

        private async void AddRoleByTeamIntoCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddRoleByTeamIntoSolution(true, null);
        }

        private async void AddRoleByTeamIntoCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddRoleByTeamIntoSolution(false, solutionUniqueName);
            }
        }

        private async Task AddRoleByTeamIntoSolution(bool withSelect, string solutionUniqueName)
        {
            var roleList = GetSelectedRolesByTeam();

            if (roleList == null || !roleList.Any())
            {
                return;
            }

            _commonConfig.Save();

            var service = await GetService();
            var descriptor = await GetDescriptor();

            try
            {
                this._iWriteToOutput.ActivateOutputWindow(service.ConnectionData);

                await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.Role, roleList.Select(item => item.Id).ToList(), null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }
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
                    bool enabled = this.IsControlsEnabled
                        && this.lstVwSecurityRoles != null
                        && this.lstVwSecurityRoles.SelectedItems.Count > 0;

                    UIElement[] list = { btnRemoveRoleFromUser };

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

        private async void btnAssignRoleToUser_Click(object sender, RoutedEventArgs e)
        {
            var userList = GetSelectedSystemUsers();

            if (userList == null || !userList.Any())
            {
                return;
            }

            var service = await GetService();

            var repository = new RoleRepository(service);

            Func<string, Task<IEnumerable<Role>>> getter = null;

            if (userList.Count() == 1)
            {
                var user = userList.First();

                getter = (string filter) => repository.GetAvailableRolesForUserAsync(filter, user.Id, new ColumnSet(
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

            string usersName = string.Join(", ", userList.Select(r => string.Format("{0} - {1}", r.DomainName, r.FullName)).OrderBy(s => s));
            string rolesName = role.Name;

            string operationName = string.Format(Properties.OperationNames.AssigningRolesToUsersFormat3, service.ConnectionData.Name, rolesName, usersName);

            _iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operationName);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.AssigningRolesToUsersFormat3, service.ConnectionData.Name, rolesName, usersName);

            var repositoryRolePrivileges = new RolePrivilegesRepository(service);

            foreach (var user in userList)
            {
                try
                {
                    await repositoryRolePrivileges.AssignRolesToUserAsync(user.Id, new[] { role.Id });
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
                }
            }

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.AssigningRolesToUsersCompletedFormat3, service.ConnectionData.Name, rolesName, usersName);

            _iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operationName);

            RefreshSystemUserInfo();
        }

        private async void btnRemoveRoleFromUser_Click(object sender, RoutedEventArgs e)
        {
            var user = GetSelectedSystemUser();

            var roleList = GetSelectedRoles();

            if (user == null || roleList == null || !roleList.Any())
            {
                return;
            }

            string rolesName = string.Join(", ", roleList.Select(r => r.Name).OrderBy(s => s));
            string usersName = string.Format("{0} - {1}", user.DomainName, user.FullName);

            string message = string.Format(Properties.MessageBoxStrings.AreYouSureRemoveRolesFromUsersFormat2, rolesName, usersName);

            if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }

            var service = await GetService();

            string operationName = string.Format(Properties.OperationNames.RemovingRolesFromUsersFormat3, service.ConnectionData.Name, rolesName, usersName);

            _iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operationName);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.RemovingRolesFromUsersFormat3, service.ConnectionData.Name, rolesName, usersName);

            try
            {
                var repository = new RolePrivilegesRepository(service);

                await repository.RemoveRolesFromUserAsync(user.Id, roleList.Select(r => r.Id));
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.RemovingRolesFromUsersCompletedFormat3, service.ConnectionData.Name, rolesName, usersName);

            _iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operationName);

            RefreshSystemUserInfo();
        }

        private async void btnAddUserToTeam_Click(object sender, RoutedEventArgs e)
        {
            var userList = GetSelectedSystemUsers();

            if (userList == null || !userList.Any())
            {
                return;
            }

            var service = await GetService();

            var repository = new TeamRepository(service);

            Func<string, Task<IEnumerable<Team>>> getter = null;

            if (userList.Count == 1)
            {
                var user = userList.First();

                getter = (string filter) => repository.GetAvailableTeamsForUserAsync(filter, user.Id, new ColumnSet(
                                Team.Schema.Attributes.name
                                , Team.Schema.Attributes.businessunitid
                                , Team.Schema.Attributes.isdefault
                                ));
            }
            else
            {
                getter = (string filter) => repository.GetNotDefaultTeamsAsync(filter, new ColumnSet(
                                Team.Schema.Attributes.name
                                , Team.Schema.Attributes.businessunitid
                                , Team.Schema.Attributes.isdefault
                                ));
            }

            IEnumerable<DataGridColumn> columns = TeamRepository.GetDataGridColumn();

            var form = new WindowEntitySelect<Team>(_iWriteToOutput, service.ConnectionData, Team.EntityLogicalName, getter, columns);

            if (!form.ShowDialog().GetValueOrDefault())
            {
                return;
            }

            if (form.SelectedEntity == null)
            {
                return;
            }

            var team = form.SelectedEntity;

            string usersName = string.Join(", ", userList.Select(r => string.Format("{0} - {1}", r.DomainName, r.FullName)).OrderBy(s => s));
            string teamsName = team.Name;

            string operationName = string.Format(Properties.OperationNames.AddingUsersToTeamsFormat3, service.ConnectionData.Name, usersName, teamsName);

            _iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operationName);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.AddingUsersToTeamsFormat3, service.ConnectionData.Name, usersName, teamsName);

            try
            {
                await repository.AddUsersToTeamAsync(form.SelectedEntity.Id, userList.Select(r => r.Id));
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.AddingUsersToTeamsCompletedFormat3, service.ConnectionData.Name, usersName, teamsName);

            _iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operationName);

            RefreshSystemUserInfo();
        }

        private async void btnRemoveUserFromTeam_Click(object sender, RoutedEventArgs e)
        {
            var user = GetSelectedSystemUser();

            var teamList = GetSelectedTeams();

            if (user == null || teamList == null || !teamList.Any())
            {
                return;
            }

            string usersName = string.Format("{0} - {1}", user.DomainName, user.FullName);
            string teamsName = string.Join(", ", teamList.Select(r => r.Name).OrderBy(s => s));

            string message = string.Format(Properties.MessageBoxStrings.AreYouSureRemoveUsersFromTeamsFormat2, usersName, teamsName);

            if (MessageBox.Show(message, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }

            var service = await GetService();

            string operationName = string.Format(Properties.OperationNames.RemovingUsersFromTeamsFormat3, service.ConnectionData.Name, usersName, teamsName);

            _iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operationName);

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.RemovingUsersFromTeamsFormat3, service.ConnectionData.Name, usersName, teamsName);

            try
            {
                var repository = new TeamRepository(service);

                await repository.RemoveUserFromTeamsAsync(user.Id, teamList.Select(r => r.Id));
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.RemovingUsersFromTeamsCompletedFormat3, service.ConnectionData.Name, usersName, teamsName);

            _iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operationName);

            RefreshSystemUserInfo();
        }

        private void LstVwTeams_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTeamsButtons();
        }

        private void UpdateTeamsButtons()
        {
            this.lstVwTeams.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled
                        && this.lstVwTeams != null
                        && this.lstVwTeams.SelectedItems.OfType<Team>().Any(t => !t.IsDefault.GetValueOrDefault());

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

        private void btnRefreshSystemUsers_Click(object sender, RoutedEventArgs e)
        {
            ShowExistingSystemUsers();
        }

        private void btnRefreshRoles_Click(object sender, RoutedEventArgs e)
        {
            ShowSystemUserRoles();
        }

        private void btnRefreshTeams_Click(object sender, RoutedEventArgs e)
        {
            ShowSystemUserTeams();
        }

        private void btnRefreshEntites_Click(object sender, RoutedEventArgs e)
        {
            ShowSystemUserEntityPrivileges();
        }

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, cmBCurrentConnection.SelectedItem as ConnectionData);
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

            var user1 = entity.ToEntity<SystemUser>();

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

            string name1 = string.Format("User {0}", user1.FullName);
            string name2 = string.Format("Role {0}", role2.Name);

            StringBuilder content = new StringBuilder();

            var privilegeComparer = new PrivilegeNameComparer();

            content.AppendLine(Properties.OutputStrings.ConnectingToCRM);
            content.AppendLine(service.ConnectionData.GetConnectionDescription());
            content.AppendFormat(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint).AppendLine();

            string operation = string.Format(Properties.OperationNames.ComparingEntitiesPrivilegesFormat3, service.ConnectionData.Name, name1, name2);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operation));

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.ComparingEntitiesPrivilegesFormat2, name1, name2);

            var repositoryRolePrivileges = new RolePrivilegesRepository(service);

            var userPrivileges1 = await repositoryRolePrivileges.GetUserPrivilegesAsync(user1.Id);
            var rolePrivileges2 = await repositoryRolePrivileges.GetRolePrivilegesAsync(role2.Id);

            var hashPrivileges = new HashSet<Guid>(userPrivileges1.Select(p => p.PrivilegeId).Union(rolePrivileges2.Select(p => p.PrivilegeId)));

            var repositoryPrivilege = new PrivilegeRepository(service);
            var privileges = await repositoryPrivilege.GetListByIdsAsync(hashPrivileges);

            var comparer = new RolePrivilegeComparerHelper(_tabSpacer, name1, name2);

            content.AppendLine();

            var difference = comparer.CompareRolePrivileges(userPrivileges1, rolePrivileges2, privileges, new PrivilegeNameComparer());

            difference.ForEach(s => content.AppendLine(s));

            content.AppendLine();

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ComparingEntitiesPrivilegesCompletedFormat2, name1, name2);

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

            var user1 = entity.ToEntity<SystemUser>();

            var service = await GetService();

            var repositoryUser = new SystemUserRepository(service);

            Func<string, Task<IEnumerable<SystemUser>>> getter = (string filter) => repositoryUser.GetUsersNotAnotherAsync(filter
                , user1.Id
                , new ColumnSet(
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

            string name1 = string.Format("User {0}", user1.FullName);
            string name2 = string.Format("User {0}", user2.FullName);

            StringBuilder content = new StringBuilder();

            var privilegeComparer = new PrivilegeNameComparer();

            content.AppendLine(Properties.OutputStrings.ConnectingToCRM);
            content.AppendLine(service.ConnectionData.GetConnectionDescription());
            content.AppendFormat(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint).AppendLine();

            string operation = string.Format(Properties.OperationNames.ComparingEntitiesPrivilegesFormat3, service.ConnectionData.Name, name1, name2);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operation));

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.ComparingEntitiesPrivilegesFormat2, name1, name2);

            var repositoryRolePrivileges = new RolePrivilegesRepository(service);

            var rolePrivileges1 = await repositoryRolePrivileges.GetUserPrivilegesAsync(user1.Id);
            var userPrivileges2 = await repositoryRolePrivileges.GetUserPrivilegesAsync(user2.Id);

            var hashPrivileges = new HashSet<Guid>(rolePrivileges1.Select(p => p.PrivilegeId).Union(userPrivileges2.Select(p => p.PrivilegeId)));

            var repositoryPrivilege = new PrivilegeRepository(service);
            var privileges = await repositoryPrivilege.GetListByIdsAsync(hashPrivileges);

            var comparer = new RolePrivilegeComparerHelper(_tabSpacer, name1, name2);

            content.AppendLine();

            var difference = comparer.CompareRolePrivileges(rolePrivileges1, userPrivileges2, privileges, new PrivilegeNameComparer());

            difference.ForEach(s => content.AppendLine(s));

            content.AppendLine();

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ComparingEntitiesPrivilegesCompletedFormat2, name1, name2);

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

            var user1 = entity.ToEntity<SystemUser>();

            var service = await GetService();

            var repositoryTeam = new TeamRepository(service);

            Func<string, Task<IEnumerable<Team>>> getter = (string filter) => repositoryTeam.GetOwnerTeamsAsync(filter, new ColumnSet(
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

            string name1 = string.Format("User {0}", user1.FullName);
            string name2 = string.Format("Team {0}", team2.Name);

            StringBuilder content = new StringBuilder();

            var privilegeComparer = new PrivilegeNameComparer();

            content.AppendLine(Properties.OutputStrings.ConnectingToCRM);
            content.AppendLine(service.ConnectionData.GetConnectionDescription());
            content.AppendFormat(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint).AppendLine();

            string operation = string.Format(Properties.OperationNames.ComparingEntitiesPrivilegesFormat3, service.ConnectionData.Name, name1, name2);

            content.AppendLine(_iWriteToOutput.WriteToOutputStartOperation(service.ConnectionData, operation));

            ToggleControls(service.ConnectionData, false, Properties.WindowStatusStrings.ComparingEntitiesPrivilegesFormat2, name1, name2);

            var repositoryRolePrivileges = new RolePrivilegesRepository(service);

            var rolePrivileges1 = await repositoryRolePrivileges.GetUserPrivilegesAsync(user1.Id);
            var teamPrivileges2 = await repositoryRolePrivileges.GetTeamPrivilegesAsync(team2.Id);

            var hashPrivileges = new HashSet<Guid>(rolePrivileges1.Select(p => p.PrivilegeId).Union(teamPrivileges2.Select(p => p.PrivilegeId)));

            var repositoryPrivilege = new PrivilegeRepository(service);
            var privileges = await repositoryPrivilege.GetListByIdsAsync(hashPrivileges);

            var comparer = new RolePrivilegeComparerHelper(_tabSpacer, name1, name2);

            content.AppendLine();

            var difference = comparer.CompareRolePrivileges(rolePrivileges1, teamPrivileges2, privileges, new PrivilegeNameComparer());

            difference.ForEach(s => content.AppendLine(s));

            content.AppendLine();

            ToggleControls(service.ConnectionData, true, Properties.WindowStatusStrings.ComparingEntitiesPrivilegesCompletedFormat2, name1, name2);

            content.AppendLine(_iWriteToOutput.WriteToOutputEndOperation(service.ConnectionData, operation));

            string fileName = EntityFileNameFormatter.ComparingRolePrivilegesInEntitiesFileName(service.ConnectionData.Name, name1, name2);

            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            File.WriteAllText(filePath, content.ToString(), new UTF8Encoding(false));

            _iWriteToOutput.PerformAction(service.ConnectionData, filePath);
        }
    }
}