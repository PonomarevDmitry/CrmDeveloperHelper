using Microsoft.Crm.Sdk.Messages;
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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowExplorerOtherPrivileges : WindowWithConnectionList
    {
        private readonly ObservableCollection<OtherPrivilegeListViewItem> _itemsSourceOtherPrivileges;

        private readonly ObservableCollection<RoleOtherPrivilegeViewItem> _itemsSourceSecurityRoleList;

        private readonly Dictionary<Guid, IEnumerable<Privilege>> _cachePrivileges = new Dictionary<Guid, IEnumerable<Privilege>>();

        private readonly List<PrivilegeType> _privielgeTypesAll = Enum.GetValues(typeof(PrivilegeType)).OfType<PrivilegeType>().ToList();

        private readonly Dictionary<PrivilegeDepth, MenuItem> _menuItemsSetPrivilegeDepths;

        public WindowExplorerOtherPrivileges(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , IOrganizationServiceExtented service
            , IEnumerable<Privilege> privileges
            , string filter
        ) : base(iWriteToOutput, commonConfig, service)
        {
            this.IncreaseInit();

            InputLanguageManager.SetInputLanguage(this, CultureInfo.CreateSpecificCulture("en-US"));

            if (privileges != null)
            {
                _cachePrivileges[service.ConnectionData.ConnectionId] = privileges;
            }

            InitializeComponent();

            FillRoleEditorLayoutTabs();

            LoadFromConfig();

            txtBOtherPrivilegesFilter.Text = filter;
            txtBOtherPrivilegesFilter.SelectionLength = 0;
            txtBOtherPrivilegesFilter.SelectionStart = txtBOtherPrivilegesFilter.Text.Length;

            txtBOtherPrivilegesFilter.Focus();

            this._menuItemsSetPrivilegeDepths = new Dictionary<PrivilegeDepth, MenuItem>()
            {
                { PrivilegeDepth.Basic, mISetAttributeOtherPrivilegeRightBasic }
                , { PrivilegeDepth.Local, mISetAttributeOtherPrivilegeRightLocal }
                , { PrivilegeDepth.Deep, mISetAttributeOtherPrivilegeRightDeep }
                , { PrivilegeDepth.Global, mISetAttributeOtherPrivilegeRightGlobal }
            };

            lstVwOtherPrivileges.ItemsSource = _itemsSourceOtherPrivileges = new ObservableCollection<OtherPrivilegeListViewItem>();

            _itemsSourceSecurityRoleList = new ObservableCollection<RoleOtherPrivilegeViewItem>();
            lstVwSecurityRoles.ItemsSource = _itemsSourceSecurityRoleList;

            cmBCurrentConnection.ItemsSource = service.ConnectionData.ConnectionConfiguration.Connections;
            cmBCurrentConnection.SelectedItem = service.ConnectionData;

            FillExplorersMenuItems();

            this.DecreaseInit();

            var task = ShowExistingOtherPrivileges();
        }

        private void FillExplorersMenuItems()
        {
            var explorersHelper = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService
                , getOtherPrivilegeName: GetOtherPrivilegeName
                , getOtherPrivilegesList: GetOtherPrivilegesList
            );

            var compareWindowsHelper = new CompareWindowsHelper(_iWriteToOutput, _commonConfig, () => Tuple.Create(GetSelectedConnection(), GetSelectedConnection())
            );

            explorersHelper.FillExplorers(miExplorers);
            compareWindowsHelper.FillCompareWindows(miCompareOrganizations);
        }

        private string GetOtherPrivilegeName()
        {
            var entity = GetSelectedOtherPrivilege();

            return entity?.Name ?? txtBOtherPrivilegesFilter.Text.Trim();
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

        private void FillRoleEditorLayoutTabs()
        {
            var tabs = RoleEditorLayoutTab.GetTabs();

            cmBRoleEditorLayoutTabsPrivileges.Items.Clear();

            cmBRoleEditorLayoutTabsPrivileges.Items.Add("All");

            foreach (var tab in tabs)
            {
                cmBRoleEditorLayoutTabsPrivileges.Items.Add(tab);
            }

            cmBRoleEditorLayoutTabsPrivileges.SelectedIndex = 0;
        }

        protected override void LoadConfigurationInternal(WindowSettings winConfig)
        {
            base.LoadConfigurationInternal(winConfig);

            LoadFormSettings(winConfig);
        }

        private const string paramColumnPrivilegeNameWidth = "ColumnPrivilegeNameWidth";

        private void LoadFormSettings(WindowSettings winConfig)
        {
            if (winConfig.DictDouble.ContainsKey(paramColumnPrivilegeNameWidth))
            {
                columnPrivilegeName.Width = new GridLength(winConfig.DictDouble[paramColumnPrivilegeNameWidth]);
            }
        }

        protected override void SaveConfigurationInternal(WindowSettings winConfig)
        {
            base.SaveConfigurationInternal(winConfig);

            winConfig.DictDouble[paramColumnPrivilegeNameWidth] = columnPrivilegeName.Width.Value;
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

        private async Task ShowExistingOtherPrivileges()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            var service = await GetService();

            ToggleControls(service.ConnectionData, false, Properties.OutputStrings.LoadingOtherPrivileges);

            this.Dispatcher.Invoke(() =>
            {
                _itemsSourceOtherPrivileges.Clear();
                _itemsSourceSecurityRoleList.Clear();
            });

            IEnumerable<OtherPrivilegeListViewItem> listOtherPrivileges = Enumerable.Empty<OtherPrivilegeListViewItem>();

            try
            {
                if (service != null)
                {
                    var otherPrivileges = await GetOtherPrivileges(service);

                    string filterOtherPrivilege = string.Empty;
                    RoleEditorLayoutTab selectedTabPrivileges = null;

                    this.Dispatcher.Invoke(() =>
                    {
                        filterOtherPrivilege = txtBOtherPrivilegesFilter.Text.Trim().ToLower();
                        selectedTabPrivileges = cmBRoleEditorLayoutTabsPrivileges.SelectedItem as RoleEditorLayoutTab;
                    });

                    otherPrivileges = FilterPrivilegeList(otherPrivileges, filterOtherPrivilege, selectedTabPrivileges);

                    if (otherPrivileges.Any())
                    {
                        listOtherPrivileges = otherPrivileges.Select(e => new OtherPrivilegeListViewItem(e));
                    }
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            string textName = string.Empty;

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

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.LoadingOtherPrivilegesCompletedFormat1, listOtherPrivileges.Count());

            await ShowExistingSecurityRoles();
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

        private async Task<IEnumerable<Privilege>> GetOtherPrivileges(IOrganizationServiceExtented service)
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

        private async Task ShowExistingSecurityRoles()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            Privilege privilege = null;

            this.Dispatcher.Invoke(() =>
            {
                foreach (var entity in _itemsSourceSecurityRoleList)
                {
                    entity.PropertyChanged -= rolePrivilege_PropertyChanged;
                    entity.PropertyChanged -= rolePrivilege_PropertyChanged;
                }

                _itemsSourceSecurityRoleList.Clear();

                privilege = GetSelectedOtherPrivilege()?.Privilege;

                foreach (var menuItem in _menuItemsSetPrivilegeDepths.Values)
                {
                    menuItem.IsEnabled = false;
                    menuItem.Visibility = Visibility.Collapsed;
                }
            });

            if (privilege == null)
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

            IEnumerable<RoleOtherPrivilegeViewItem> list = Enumerable.Empty<RoleOtherPrivilegeViewItem>();

            try
            {
                if (service != null)
                {
                    RoleRepository repositoryRole = new RoleRepository(service);

                    var roles = await repositoryRole.GetListAsync(textName
                        , new ColumnSet
                        (
                            Role.Schema.Attributes.name
                            , Role.Schema.Attributes.businessunitid
                            , Role.Schema.Attributes.ismanaged
                            , Role.Schema.Attributes.iscustomizable
                        )
                    );

                    var repository = new RolePrivilegesRepository(service);

                    var task = repository.GetEntityPrivilegesAsync(roles.Select(r => r.RoleId.Value), new[] { privilege.Id });

                    ActivateMenuItemsSetPrivileges(privilege);

                    var listRolePrivilege = await task;

                    list = roles.Select(r => new RoleOtherPrivilegeViewItem(r, listRolePrivilege.Where(rp => rp.RoleId == r.RoleId), privilege)).ToList();
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);
            }

            LoadSecurityRoles(list);

            ToggleControls(service.ConnectionData, true, Properties.OutputStrings.LoadingEntityPrivilegesCompletedFormat1, list.Count());
        }

        private void ActivateMenuItemsSetPrivileges(Privilege privilege)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (privilege.CanBeBasic.GetValueOrDefault())
                {
                    _menuItemsSetPrivilegeDepths[PrivilegeDepth.Basic].IsEnabled = true;
                    _menuItemsSetPrivilegeDepths[PrivilegeDepth.Basic].Visibility = Visibility.Visible;
                }

                if (privilege.CanBeLocal.GetValueOrDefault())
                {
                    _menuItemsSetPrivilegeDepths[PrivilegeDepth.Local].IsEnabled = true;
                    _menuItemsSetPrivilegeDepths[PrivilegeDepth.Local].Visibility = Visibility.Visible;
                }

                if (privilege.CanBeDeep.GetValueOrDefault())
                {
                    _menuItemsSetPrivilegeDepths[PrivilegeDepth.Deep].IsEnabled = true;
                    _menuItemsSetPrivilegeDepths[PrivilegeDepth.Deep].Visibility = Visibility.Visible;
                }

                if (privilege.CanBeGlobal.GetValueOrDefault())
                {
                    _menuItemsSetPrivilegeDepths[PrivilegeDepth.Global].IsEnabled = true;
                    _menuItemsSetPrivilegeDepths[PrivilegeDepth.Global].Visibility = Visibility.Visible;
                }

                menuSetPrivilege.IsEnabled = _menuItemsSetPrivilegeDepths.Values.Any(m => m.IsEnabled);
            });
        }

        private void LoadSecurityRoles(IEnumerable<RoleOtherPrivilegeViewItem> results)
        {
            this.lstVwSecurityRoles.Dispatcher.Invoke(() =>
            {
                foreach (var rolePrivilege in results
                    .OrderBy(s => s.RoleName)
                    .ThenBy(s => s.RoleTemplateName)
                    .ThenBy(s => s.Role.Id)
                )
                {
                    rolePrivilege.PropertyChanged -= rolePrivilege_PropertyChanged;
                    rolePrivilege.PropertyChanged -= rolePrivilege_PropertyChanged;
                    rolePrivilege.PropertyChanged += rolePrivilege_PropertyChanged;

                    _itemsSourceSecurityRoleList.Add(rolePrivilege);
                }

                if (this.lstVwSecurityRoles.Items.Count == 1)
                {
                    this.lstVwSecurityRoles.SelectedItem = this.lstVwSecurityRoles.Items[0];
                }
            });
        }

        private void rolePrivilege_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
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

        protected override void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(connectionData, statusFormat, args);

            ToggleControl(this.tSProgressBar
                , cmBCurrentConnection
                , btnSetCurrentConnection

                , btnRefreshOtherPrivileges
                , btnClearOtherPrivilegesCacheAndRefresh
                , btnRefreshRoles
            );

            ToggleControl(IsControlsEnabled && _menuItemsSetPrivilegeDepths.Values.Any(m => m.IsEnabled)
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

        private async void txtBOtherPrivilegesFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await ShowExistingOtherPrivileges();
            }
        }

        private async void cmBRoleEditorLayoutTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await ShowExistingOtherPrivileges();
        }

        private async void txtBFilterSecurityRole_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await ShowExistingSecurityRoles();
            }
        }

        private RoleOtherPrivilegeViewItem GetSelectedSecurityRole()
        {
            return this.lstVwSecurityRoles.SelectedItems.OfType<RoleOtherPrivilegeViewItem>().Count() == 1
                ? this.lstVwSecurityRoles.SelectedItems.OfType<RoleOtherPrivilegeViewItem>().SingleOrDefault() : null;
        }

        private List<RoleOtherPrivilegeViewItem> GetSelectedSecurityRoles()
        {
            List<RoleOtherPrivilegeViewItem> result = this.lstVwSecurityRoles.SelectedItems.OfType<RoleOtherPrivilegeViewItem>().ToList();

            return result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async Task ExecuteActionAsync(IEnumerable<string> entityNames, Func<IEnumerable<string>, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            await action(entityNames);
        }

        private async void lstVwOtherPrivileges_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await ShowExistingSecurityRoles();
        }

        protected override async Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            await ShowExistingOtherPrivileges();
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

        private void ContextMenuSecurityRole_Opened(object sender, RoutedEventArgs e)
        {
            if (sender is ContextMenu contextMenu)
            {
                var items = contextMenu.Items.OfType<Control>();

                ConnectionData connectionData = GetSelectedConnection();

                FillLastSolutionItems(connectionData, items, true, AddSecurityRoleToCrmSolutionLast_Click, "contMnAddSecurityRoleToSolutionLast");
            }
        }

        private void mIOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            var entity = GetSelectedOtherPrivilege();

            if (entity == null)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                connectionData.OpenSolutionComponentDependentComponentsInWeb(ComponentType.Privilege, entity.Privilege.Id);
            }
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

        private async void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                this._itemsSourceOtherPrivileges?.Clear();
                this._itemsSourceSecurityRoleList?.Clear();
            });

            if (!this.IsControlsEnabled)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                await ShowExistingOtherPrivileges();
            }
        }

        private async void btnClearOtherPrivilegesCacheAndRefresh_Click(object sender, RoutedEventArgs e)
        {
            ConnectionData connectionData = GetSelectedConnection();

            if (connectionData != null)
            {
                _cachePrivileges.Remove(connectionData.ConnectionId);

                await ShowExistingOtherPrivileges();
            }
        }

        private async void btnRefreshOtherPrivileges_Click(object sender, RoutedEventArgs e)
        {
            await ShowExistingOtherPrivileges();
        }

        private async void btnRefreshRoles_Click(object sender, RoutedEventArgs e)
        {
            await ShowExistingSecurityRoles();
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
                        RoleOtherPrivilegeViewItem item = GetItemFromRoutedDataContext<RoleOtherPrivilegeViewItem>(e);

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
                || !(menuItem.DataContext is RoleOtherPrivilegeViewItem role)
                )
            {
                return;
            }

            var service = await GetService();

            IEnumerable<Privilege> otherPrivilegesList = null;

            if (_cachePrivileges.ContainsKey(service.ConnectionData.ConnectionId))
            {
                otherPrivilegesList = _cachePrivileges[service.ConnectionData.ConnectionId];
            }

            WindowHelper.OpenRolesExplorer(_iWriteToOutput, service, _commonConfig, role.RoleName, null, otherPrivilegesList);
        }

        private async void mICreateSecurityRoleBackup_Click(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is MenuItem menuItem))
            {
                return;
            }

            if (menuItem.DataContext == null
                || !(menuItem.DataContext is RoleOtherPrivilegeViewItem role)
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

            string fileName = EntityFileNameFormatter.GetRoleFileName(service.ConnectionData.Name, role.Role.Name, EntityFileNameFormatter.Headers.Backup, FileExtension.xml);
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

            await ShowExistingSecurityRoles();
        }

        private void btnSetCurrentConnection_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentConnection(_iWriteToOutput, GetSelectedConnection());
        }

        private void SetSelectedRolesPrivilege(PrivilegeDepthExtended privilegeDepth)
        {
            Privilege privilege = GetSelectedOtherPrivilege()?.Privilege;

            if (privilege == null)
            {
                return;
            }

            var list = lstVwSecurityRoles.SelectedItems.OfType<RoleOtherPrivilegeViewItem>().ToList();

            if (!list.Any())
            {
                return;
            }

            bool canBePrivilegeDepth = false;

            switch (privilegeDepth)
            {
                case PrivilegeDepthExtended.Basic:
                    canBePrivilegeDepth = privilege.CanBeBasic.GetValueOrDefault();
                    break;

                case PrivilegeDepthExtended.Local:
                    canBePrivilegeDepth = privilege.CanBeLocal.GetValueOrDefault();
                    break;

                case PrivilegeDepthExtended.Deep:
                    canBePrivilegeDepth = privilege.CanBeDeep.GetValueOrDefault();
                    break;

                case PrivilegeDepthExtended.Global:
                    canBePrivilegeDepth = privilege.CanBeGlobal.GetValueOrDefault();
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
                    item.Right = privilegeDepth;
                }
            }
        }

        #region Set Attribute

        private void mISetAttributeOtherPrivilegeRightNone_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeDepthExtended.None);
        }

        private void mISetAttributeOtherPrivilegeRightBasic_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeDepthExtended.Basic);
        }

        private void mISetAttributeOtherPrivilegeRightLocal_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeDepthExtended.Local);
        }

        private void mISetAttributeOtherPrivilegeRightDeep_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeDepthExtended.Deep);
        }

        private void mISetAttributeOtherPrivilegeRightGlobal_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedRolesPrivilege(PrivilegeDepthExtended.Global);
        }

        #endregion Set Attribute

        #region Other Privilege

        private OtherPrivilegeListViewItem GetSelectedOtherPrivilege()
        {
            return this.lstVwOtherPrivileges.SelectedItems.OfType<OtherPrivilegeListViewItem>().Count() == 1
                ? this.lstVwOtherPrivileges.SelectedItems.OfType<OtherPrivilegeListViewItem>().SingleOrDefault() : null;
        }

        private List<OtherPrivilegeListViewItem> GetSelectedOtherPrivileges()
        {
            List<OtherPrivilegeListViewItem> result = this.lstVwOtherPrivileges.SelectedItems.OfType<OtherPrivilegeListViewItem>().ToList();

            return result;
        }

        private void ContextMenuOtherPrivilege_Opened(object sender, RoutedEventArgs e)
        {
            if (sender is ContextMenu contextMenu)
            {
                var items = contextMenu.Items.OfType<Control>();

                ConnectionData connectionData = GetSelectedConnection();

                FillLastSolutionItems(connectionData, items, true, AddOtherPrivilegeToCrmSolutionLast_Click, "contMnAddOtherPrivilegeToSolutionLast");
            }
        }

        private void mIOtherPrivilegeOpenDependentComponentsInWeb_Click(object sender, RoutedEventArgs e)
        {
            var privilege = GetSelectedOtherPrivilege();

            if (privilege == null)
            {
                return;
            }

            ConnectionData connectionData = GetSelectedConnection();

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