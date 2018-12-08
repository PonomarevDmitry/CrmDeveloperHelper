using Microsoft.Xrm.Sdk;
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
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowSystemUserExplorer : WindowBase
    {
        private readonly object sysObjectConnections = new object();

        private IWriteToOutput _iWriteToOutput;

        private CommonConfiguration _commonConfig;
        private ConnectionConfiguration _connectionConfig;

        private bool _controlsEnabled = true;

        private ObservableCollection<SystemUser> _itemsSourceSystemUsers;

        private ObservableCollection<Team> _itemsSourceTeams;
        private ObservableCollection<Role> _itemsSourceRoles;
        private ObservableCollection<Role> _itemsSourceRolesByTeams;

        private ObservableCollection<EntityPrivilegeViewItem> _itemsSourceEntityPrivileges;

        private Dictionary<Guid, IOrganizationServiceExtented> _connectionCache = new Dictionary<Guid, IOrganizationServiceExtented>();
        private Dictionary<Guid, SolutionComponentDescriptor> _descriptorCache = new Dictionary<Guid, SolutionComponentDescriptor>();

        private Dictionary<Guid, IEnumerable<EntityMetadata>> _cacheEntityMetadata = new Dictionary<Guid, IEnumerable<EntityMetadata>>();

        private int _init = 0;

        public WindowSystemUserExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , IEnumerable<EntityMetadata> entityMetadataList
            , string filterEntity
        )
        {
            _init++;

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;
            this._connectionConfig = service.ConnectionData.ConnectionConfiguration;

            _connectionCache[service.ConnectionData.ConnectionId] = service;
            _descriptorCache[service.ConnectionData.ConnectionId] = new SolutionComponentDescriptor(service, true);

            if (entityMetadataList != null && entityMetadataList.Any(e => e.Privileges != null && e.Privileges.Any()))
            {
                _cacheEntityMetadata[service.ConnectionData.ConnectionId] = entityMetadataList;
            }

            BindingOperations.EnableCollectionSynchronization(_connectionConfig.Connections, sysObjectConnections);

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

            UpdateSystemUsersButtons();

            cmBCurrentConnection.ItemsSource = _connectionConfig.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            _init--;

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
            _connectionConfig.Save();

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

        private async Task ShowExistingSystemUsers()
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.LoadingSystemUsers);

            string textName = string.Empty;

            this.Dispatcher.Invoke(() =>
            {
                _itemsSourceSystemUsers.Clear();

                _itemsSourceTeams.Clear();

                _itemsSourceRoles.Clear();
                _itemsSourceRolesByTeams.Clear();

                _itemsSourceEntityPrivileges.Clear();

                textName = txtBFilterSystemUser.Text.Trim().ToLower();
            });

            IEnumerable<SystemUser> list = Enumerable.Empty<SystemUser>();

            try
            {
                var service = await GetService();

                if (service != null)
                {
                    SystemUserRepository repository = new SystemUserRepository(service);

                    list = await repository.GetListAsync(textName, new ColumnSet(SystemUser.Schema.Attributes.fullname, SystemUser.Schema.Attributes.domainname, SystemUser.Schema.Attributes.businessunitid, SystemUser.Schema.Attributes.isdisabled));
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
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

            ToggleControls(true, Properties.WindowStatusStrings.LoadingSystemUsersCompletedFormat1, list.Count());

            await RefreshSystemUserInfo();
        }

        private async Task ShowSystemUserRoles()
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.LoadingSystemUserSecurityRoles);

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
                var service = await GetService();

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
                this._iWriteToOutput.WriteErrorToOutput(ex);
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

            ToggleControls(true, Properties.WindowStatusStrings.LoadingSystemUserSecurityRolesCompleted);
        }

        private async Task ShowSystemUserTeams()
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.LoadingSystemUserTeams);

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
                var service = await GetService();

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
                this._iWriteToOutput.WriteErrorToOutput(ex);
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

            ToggleControls(true, Properties.WindowStatusStrings.LoadingSystemUserTeamsCompletedFormat1, list.Count());
        }

        private async Task ShowSystemUserEntityPrivileges()
        {
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.WindowStatusStrings.LoadingEntities);

            this.Dispatcher.Invoke(() =>
            {
                _itemsSourceEntityPrivileges.Clear();
            });

            var user = GetSelectedSystemUser();

            IEnumerable<EntityMetadata> entityMetadataList = Enumerable.Empty<EntityMetadata>();

            IEnumerable<EntityPrivilegeViewItem> list = Enumerable.Empty<EntityPrivilegeViewItem>();

            try
            {
                var service = await GetService();

                if (service != null)
                {
                    if (!_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
                    {
                        EntityMetadataRepository repository = new EntityMetadataRepository(service);

                        var temp = await repository.GetEntitiesForEntityAttributeExplorerAsync(EntityFilters.Entity | EntityFilters.Privileges);

                        _cacheEntityMetadata.Add(service.ConnectionData.ConnectionId, temp);
                    }

                    entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId];

                    entityMetadataList = entityMetadataList.Where(e => e.Privileges != null && e.Privileges.Any());

                    if (user != null)
                    {
                        string textName = string.Empty;

                        txtBEntityFilter.Dispatcher.Invoke(() =>
                        {
                            textName = txtBEntityFilter.Text.Trim().ToLower();
                        });

                        entityMetadataList = FilterEntityList(entityMetadataList, textName);

                        if (entityMetadataList.Any())
                        {
                            var repository = new RolePrivilegesRepository(service);

                            var privileges = await repository.GetUserPrivilegesAsync(user.Id);

                            list = entityMetadataList.Select(e => new EntityPrivilegeViewItem(e, privileges));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
            }

            this.lstVwEntityPrivileges.Dispatcher.Invoke(() =>
            {
                foreach (var entity in list.OrderBy(s => s.LogicalName))
                {
                    _itemsSourceEntityPrivileges.Add(entity);
                }

                if (this.lstVwEntityPrivileges.Items.Count == 1)
                {
                    this.lstVwEntityPrivileges.SelectedItem = this.lstVwEntityPrivileges.Items[0];
                }
            });

            ToggleControls(true, Properties.WindowStatusStrings.LoadingEntitiesCompletedFormat1, entityMetadataList.Count());
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

            ToggleControl(enabled, cmBCurrentConnection, btnRefreshEntites, btnRefreshRoles, btnRefreshSystemUsers, btnRefreshTeams, tSProgressBar);

            UpdateSystemUsersButtons();

            UpdateSecurityRolesButtons();

            UpdateTeamsButtons();
        }

        private void ToggleControl(bool enabled, params Control[] controlsArray)
        {
            foreach (var control in controlsArray)
            {
                if (control == null)
                {
                    continue;
                }

                control.Dispatcher.Invoke(() =>
                {
                    if (control is TextBox textBox)
                    {
                        textBox.IsReadOnly = !enabled;
                    }
                    else if (control is ProgressBar progressBar)
                    {
                        progressBar.IsIndeterminate = !enabled;
                    }
                    else
                    {
                        control.IsEnabled = enabled;
                    }
                });
            }
        }

        private void UpdateSystemUsersButtons()
        {
            this.lstVwSystemUsers.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this._controlsEnabled
                        && this.lstVwSystemUsers != null
                        && this.lstVwSystemUsers.SelectedItems.OfType<SystemUser>().Any(u => !u.IsDisabled.GetValueOrDefault());

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
                result = this.lstVwSystemUsers.SelectedItems.OfType<SystemUser>().Where(u => !u.IsDisabled.GetValueOrDefault()).ToList();
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

            WindowHelper.OpenEntityMetadataWindow(this._iWriteToOutput, service, _commonConfig, entity?.LogicalName, entityMetadataList, null);
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

            WindowHelper.OpenEntitySecurityRolesExplorer(this._iWriteToOutput, service, _commonConfig, entityMetadataList, entity?.LogicalName);
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

            WindowHelper.OpenOrganizationComparerGlobalOptionSetsWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entity?.LogicalName);
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
            if (_init > 0 || !_controlsEnabled)
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
            if (_init > 0 || !_controlsEnabled)
            {
                return;
            }

            var service = await GetService();

            var entityNamesOrdered = string.Join(", ", entityNames.OrderBy(s => s));

            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.PublishingEntitiesFormat2, service.ConnectionData.Name, entityNamesOrdered);

            ToggleControls(false, Properties.WindowStatusStrings.PublishingEntitiesFormat2, service.ConnectionData.Name, entityNamesOrdered);

            try
            {
                var repository = new PublishActionsRepository(service);

                await repository.PublishEntitiesAsync(entityNames);

                ToggleControls(true, Properties.WindowStatusStrings.PublishingEntitiesCompletedFormat2, service.ConnectionData.Name, entityNamesOrdered);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);

                ToggleControls(true, Properties.WindowStatusStrings.PublishingEntitiesFailedFormat2, service.ConnectionData.Name, entityNamesOrdered);
            }

            this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.PublishingEntitiesFormat2, service.ConnectionData.Name, entityNamesOrdered);
        }

        private void lstVwSystemUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshSystemUserInfo();
        }

        private async Task RefreshSystemUserInfo()
        {
            try
            {
                await ShowSystemUserRoles();

                await ShowSystemUserTeams();

                await ShowSystemUserEntityPrivileges();
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
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
                connectionData.OpenEntityListInWeb(entity.LogicalName);
            }
        }

        private async void AddEntityIntoCrmSolution_Click(object sender, RoutedEventArgs e)
        {
            await AddEntityIntoSolution(true, null);
        }

        private async void AddEntityIntoCrmSolutionLast_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
               && menuItem.Tag != null
               && menuItem.Tag is string solutionUniqueName
               )
            {
                await AddEntityIntoSolution(false, solutionUniqueName);
            }
        }

        private async Task AddEntityIntoSolution(bool withSelect, string solutionUniqueName)
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
                this._iWriteToOutput.ActivateOutputWindow();

                await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.Entity, entityList.Select(item => item.EntityMetadata.MetadataId.Value).ToList(), null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
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

                FillLastSolutionItems(connectionData, items, true, AddEntityIntoCrmSolutionLast_Click, "contMnAddIntoSolutionLast");
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
            if (_init > 0 || !_controlsEnabled)
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

            if (_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId];
            }

            switch (entity)
            {
                case Role role:
                    WindowHelper.OpenRolesExplorer(_iWriteToOutput, service, _commonConfig, entityMetadataList, role.Name);
                    break;

                case SystemUser user:
                    WindowHelper.OpenSystemUsersExplorer(_iWriteToOutput, service, _commonConfig, entityMetadataList, user.FullName);
                    break;

                case Team team:
                    WindowHelper.OpenTeamsExplorer(_iWriteToOutput, service, _commonConfig, entityMetadataList, team.Name);
                    break;
            }
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
                connectionData.OpenEntityListInWeb(entity.LogicalName);
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

            if (_cacheEntityMetadata.ContainsKey(service.ConnectionData.ConnectionId))
            {
                entityMetadataList = _cacheEntityMetadata[service.ConnectionData.ConnectionId];
            }

            WindowHelper.OpenTeamsExplorer(_iWriteToOutput, service, _commonConfig, entityMetadataList, role.TeamName);
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
                this._iWriteToOutput.ActivateOutputWindow();

                await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.Role, roleList.Select(item => item.Id).ToList(), null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
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
                this._iWriteToOutput.ActivateOutputWindow();

                await SolutionController.AddSolutionComponentsGroupIntoSolution(_iWriteToOutput, service, descriptor, _commonConfig, solutionUniqueName, ComponentType.Role, roleList.Select(item => item.Id).ToList(), null, withSelect);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(ex);
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
                    bool enabled = this._controlsEnabled
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

            Func<string, Task<IEnumerable<Entity>>> getter = null;

            if (userList.Count() == 1)
            {
                var user = userList.First();

                getter = (string filter) => Task.Run(async () => await repository.GetAvailableRolesForUserAsync(filter, user.Id, new ColumnSet(
                                 Role.Schema.Attributes.name
                                 , Role.Schema.Attributes.businessunitid
                                 , Role.Schema.Attributes.ismanaged
                                 , Role.Schema.Attributes.iscustomizable
                                 )) as IEnumerable<Entity>);
            }
            else
            {
                getter = (string filter) => Task.Run(async () => await repository.GetListAsync(filter, new ColumnSet(
                                 Role.Schema.Attributes.name
                                 , Role.Schema.Attributes.businessunitid
                                 , Role.Schema.Attributes.ismanaged
                                 , Role.Schema.Attributes.iscustomizable
                                 )) as IEnumerable<Entity>);
            }

            IEnumerable<DataGridColumn> columns = Helpers.SolutionComponentDescription.Implementation.RoleDescriptionBuilder.GetDataGridColumn();

            var form = new WindowEntitySelect(_iWriteToOutput, service.ConnectionData, Role.EntityLogicalName, getter, columns);

            if (!form.ShowDialog().GetValueOrDefault())
            {
                return;
            }

            if (form.SelectedEntity == null)
            {
                return;
            }

            var role = form.SelectedEntity.ToEntity<Role>();

            string usersName = string.Join(", ", userList.Select(r => string.Format("{0} - {1}", r.DomainName, r.FullName)).OrderBy(s => s));
            string rolesName = role.Name;

            string operationName = string.Format(Properties.OperationNames.AssigningRolesToUsersFormat3, service.ConnectionData.Name, rolesName, usersName);

            _iWriteToOutput.WriteToOutputStartOperation(operationName);

            ToggleControls(false, Properties.WindowStatusStrings.AssigningRolesToUsersFormat3, service.ConnectionData.Name, rolesName, usersName);

            var repositoryRolePrivileges = new RolePrivilegesRepository(service);

            foreach (var user in userList)
            {
                try
                {
                    await repositoryRolePrivileges.AssignRolesToUserAsync(user.Id, new[] { role.Id });
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(ex);
                }
            }

            ToggleControls(true, Properties.WindowStatusStrings.AssigningRolesToUsersCompletedFormat3, service.ConnectionData.Name, rolesName, usersName);

            _iWriteToOutput.WriteToOutputEndOperation(operationName);

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

            _iWriteToOutput.WriteToOutputStartOperation(operationName);

            ToggleControls(false, Properties.WindowStatusStrings.RemovingRolesFromUsersFormat3, service.ConnectionData.Name, rolesName, usersName);

            try
            {
                var repository = new RolePrivilegesRepository(service);

                await repository.RemoveRolesFromUserAsync(user.Id, roleList.Select(r => r.Id));
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);
            }

            ToggleControls(true, Properties.WindowStatusStrings.RemovingRolesFromUsersCompletedFormat3, service.ConnectionData.Name, rolesName, usersName);

            _iWriteToOutput.WriteToOutputEndOperation(operationName);

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

            Func<string, Task<IEnumerable<Entity>>> getter = null;

            if (userList.Count == 1)
            {
                var user = userList.First();

                getter = (string filter) => Task.Run(async () => await repository.GetAvailableTeamsForUserAsync(filter, user.Id, new ColumnSet(
                                Team.Schema.Attributes.name
                                , Team.Schema.Attributes.businessunitid
                                , Team.Schema.Attributes.isdefault
                                )) as IEnumerable<Entity>);
            }
            else
            {
                getter = (string filter) => Task.Run(async () => await repository.GetNotDefaultTeamsAsync(filter, new ColumnSet(
                                Team.Schema.Attributes.name
                                , Team.Schema.Attributes.businessunitid
                                , Team.Schema.Attributes.isdefault
                                )) as IEnumerable<Entity>);
            }

            IEnumerable<DataGridColumn> columns = TeamRepository.GetDataGridColumn();

            var form = new WindowEntitySelect(_iWriteToOutput, service.ConnectionData, Team.EntityLogicalName, getter, columns);

            if (!form.ShowDialog().GetValueOrDefault())
            {
                return;
            }

            if (form.SelectedEntity == null)
            {
                return;
            }

            var team = form.SelectedEntity.ToEntity<Team>();

            string usersName = string.Join(", ", userList.Select(r => string.Format("{0} - {1}", r.DomainName, r.FullName)).OrderBy(s => s));
            string teamsName = team.Name;

            string operationName = string.Format(Properties.OperationNames.AddingUsersToTeamsFormat3, service.ConnectionData.Name, usersName, teamsName);

            _iWriteToOutput.WriteToOutputStartOperation(operationName);

            ToggleControls(false, Properties.WindowStatusStrings.AddingUsersToTeamsFormat3, service.ConnectionData.Name, usersName, teamsName);

            try
            {
                await repository.AddUsersToTeamAsync(form.SelectedEntity.Id, userList.Select(r => r.Id));
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);
            }

            ToggleControls(true, Properties.WindowStatusStrings.AddingUsersToTeamsCompletedFormat3, service.ConnectionData.Name, usersName, teamsName);

            _iWriteToOutput.WriteToOutputEndOperation(operationName);

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

            _iWriteToOutput.WriteToOutputStartOperation(operationName);

            ToggleControls(false, Properties.WindowStatusStrings.RemovingUsersFromTeamsFormat3, service.ConnectionData.Name, usersName, teamsName);

            try
            {
                var repository = new TeamRepository(service);

                await repository.RemoveUserFromTeamsAsync(user.Id, teamList.Select(r => r.Id));
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(ex);
            }

            ToggleControls(true, Properties.WindowStatusStrings.RemovingUsersFromTeamsCompletedFormat3, service.ConnectionData.Name, usersName, teamsName);

            _iWriteToOutput.WriteToOutputEndOperation(operationName);

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
                    bool enabled = this._controlsEnabled
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
    }
}