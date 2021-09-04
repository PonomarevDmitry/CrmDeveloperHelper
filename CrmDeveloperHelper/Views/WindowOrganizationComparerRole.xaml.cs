using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
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
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public partial class WindowOrganizationComparerRole : WindowWithConnectionList
    {
        private readonly Popup _popupEntityMetadataFilter;
        private readonly EntityMetadataFilter _entityMetadataFilter;

        private readonly ObservableCollection<EntityViewItem> _itemsSourceRoles;

        private readonly List<PrivilegeType> _privielgeTypesAll = Enum.GetValues(typeof(PrivilegeType)).Cast<PrivilegeType>().ToList();

        private readonly ObservableCollection<RoleLinkedEntitiesPrivilegeViewItem> _itemsSourceEntityPrivileges;
        private readonly ObservableCollection<RoleLinkedOtherPrivilegeViewItem> _itemsSourceOtherPrivileges;

        private readonly Dictionary<Guid, IEnumerable<EntityMetadata>> _cacheEntityMetadata = new Dictionary<Guid, IEnumerable<EntityMetadata>>();
        private readonly Dictionary<Guid, IEnumerable<Privilege>> _cachePrivileges = new Dictionary<Guid, IEnumerable<Privilege>>();

        private readonly List<RoleLinkedEntitiesPrivilegeViewItem> _currentRoleEntityPrivileges;
        private readonly List<RoleLinkedOtherPrivilegeViewItem> _currentRoleOtherPrivileges;

        public WindowOrganizationComparerRole(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filter
        ) : base(iWriteToOutput, commonConfig, connection1)
        {
            this.IncreaseInit();

            InitializeComponent();

            SetInputLanguageEnglish();

            this.Resources["ConnectionName1"] = connection1.Name;
            this.Resources["ConnectionName2"] = connection2.Name;

            _entityMetadataFilter = new EntityMetadataFilter();
            _entityMetadataFilter.CloseClicked += this.entityMetadataFilter_CloseClicked;
            this._popupEntityMetadataFilter = new Popup
            {
                Child = _entityMetadataFilter,

                PlacementTarget = lblEntitiesList,
                Placement = PlacementMode.Bottom,
                StaysOpen = false,
                Focusable = true,
            };
            _popupEntityMetadataFilter.Closed += this.popupEntityMetadataFilter_Closed;

            FillRoleEditorLayoutTabs();

            LoadFromConfig();

            if (!string.IsNullOrEmpty(filter))
            {
                txtBFilterRole.Text = filter;
            }

            txtBFilterRole.SelectionLength = 0;
            txtBFilterRole.SelectionStart = txtBFilterRole.Text.Length;

            txtBFilterRole.Focus();

            this.lstVwSecurityRoles.ItemsSource = _itemsSourceRoles = new ObservableCollection<EntityViewItem>();

            lstVwEntityPrivileges.ItemsSource = _itemsSourceEntityPrivileges = new ObservableCollection<RoleLinkedEntitiesPrivilegeViewItem>();
            lstVwOtherPrivileges.ItemsSource = _itemsSourceOtherPrivileges = new ObservableCollection<RoleLinkedOtherPrivilegeViewItem>();

            _currentRoleEntityPrivileges = new List<RoleLinkedEntitiesPrivilegeViewItem>();
            _currentRoleOtherPrivileges = new List<RoleLinkedOtherPrivilegeViewItem>();

            cmBConnection1.ItemsSource = connection1.ConnectionConfiguration.Connections;
            cmBConnection1.SelectedItem = connection1;

            cmBConnection2.ItemsSource = connection1.ConnectionConfiguration.Connections;
            cmBConnection2.SelectedItem = connection2;

            FillExplorersMenuItems();

            this.DecreaseInit();

            var task = ShowExistingSecurityRoles();
        }

        private void FillExplorersMenuItems()
        {
            //var explorersHelper1 = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService1
            //    , getReportName: GetReportName1
            //);

            //var explorersHelper2 = new ExplorersHelper(_iWriteToOutput, _commonConfig, GetService2
            //    , getReportName: GetReportName2
            //);

            //explorersHelper1.FillExplorers(miExplorers1);
            //explorersHelper2.FillExplorers(miExplorers2);

            var compareWindowsHelper = new CompareWindowsHelper(_iWriteToOutput, _commonConfig, () => Tuple.Create(GetConnection1(), GetConnection2())
                , getRoleName: GetRoleName
            );
            compareWindowsHelper.FillCompareWindows(miCompareOrganizations);

            if (this.Resources.Contains("listContextMenu")
                && this.Resources["listContextMenu"] is ContextMenu contextMenu
            )
            {
                var items = contextMenu.Items.OfType<MenuItem>();

                foreach (var item in items)
                {
                    //if (string.Equals(item.Uid, "miExplorers1", StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    explorersHelper1.FillExplorers(item);
                    //}
                    //else if (string.Equals(item.Uid, "miExplorers2", StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    explorersHelper2.FillExplorers(item);
                    //}
                    //else
                    if (string.Equals(item.Uid, "miCompareOrganizations", StringComparison.InvariantCultureIgnoreCase))
                    {
                        compareWindowsHelper.FillCompareWindows(item);
                    }
                }
            }
        }

        private string GetRoleName()
        {
            var roleLink = GetSelectedRoleLink();

            return roleLink?.Name1 ?? txtBFilterRole.Text.Trim();
        }

        //private IEnumerable<EntityMetadata> GetEntityMetadataList(Guid connectionId)
        //{
        //    if (_cacheEntityMetadata.ContainsKey(connectionId))
        //    {
        //        return _cacheEntityMetadata[connectionId];
        //    }

        //    return null;
        //}

        //private IEnumerable<Privilege> GetOtherPrivilegesList(Guid connectionId)
        //{
        //    if (_cachePrivileges.ContainsKey(connectionId))
        //    {
        //        return _cachePrivileges[connectionId];
        //    }

        //    return null;
        //}

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

        private void LoadFromConfig()
        {
            cmBFileAction.DataContext = _commonConfig;
        }

        protected override void LoadConfigurationInternal(WindowSettings winConfig)
        {
            base.LoadConfigurationInternal(winConfig);

            LoadFormSettings(winConfig);
        }

        private const string paramColumnRoleWidth = "ColumnRoleWidth";

        private void LoadFormSettings(WindowSettings winConfig)
        {
            if (winConfig.DictDouble.ContainsKey(paramColumnRoleWidth))
            {
                columnRole.Width = new GridLength(winConfig.DictDouble[paramColumnRoleWidth]);
            }
        }

        protected override void SaveConfigurationInternal(WindowSettings winConfig)
        {
            base.SaveConfigurationInternal(winConfig);

            winConfig.DictDouble[paramColumnRoleWidth] = columnRole.Width.Value;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            _commonConfig.Save();

            BindingOperations.ClearAllBindings(cmBConnection1);
            cmBConnection1.Items.DetachFromSourceCollection();
            cmBConnection1.DataContext = null;
            cmBConnection1.ItemsSource = null;

            BindingOperations.ClearAllBindings(cmBConnection2);
            cmBConnection2.Items.DetachFromSourceCollection();
            cmBConnection2.DataContext = null;
            cmBConnection2.ItemsSource = null;
        }

        private ConnectionData GetConnection1()
        {
            ConnectionData connectionData = null;

            cmBConnection1.Dispatcher.Invoke(() =>
            {
                connectionData = cmBConnection1.SelectedItem as ConnectionData;
            });

            return connectionData;
        }

        private ConnectionData GetConnection2()
        {
            ConnectionData connectionData = null;

            cmBConnection2.Dispatcher.Invoke(() =>
            {
                connectionData = cmBConnection2.SelectedItem as ConnectionData;
            });

            return connectionData;
        }

        private Task<IOrganizationServiceExtented> GetService1()
        {
            return GetOrganizationService(GetConnection1());
        }

        private Task<IOrganizationServiceExtented> GetService2()
        {
            return GetOrganizationService(GetConnection2());
        }

        private async Task ShowExistingSecurityRoles()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.LoadingSecurityRoles);

            var filterRole = string.Empty;

            this.Dispatcher.Invoke(() =>
            {
                this._itemsSourceRoles.Clear();

                this._itemsSourceEntityPrivileges.Clear();
                this._itemsSourceOtherPrivileges.Clear();

                this._currentRoleEntityPrivileges.Clear();
                this._currentRoleOtherPrivileges.Clear();

                filterRole = txtBFilterRole.Text.Trim().ToLower();
            });

            IEnumerable<LinkedEntities<Role>> list = Enumerable.Empty<LinkedEntities<Role>>();

            try
            {
                var service1 = await GetService1();
                var service2 = await GetService2();

                if (service1 != null && service2 != null)
                {
                    var columnSet = new ColumnSet(
                        Role.Schema.Attributes.name
                        , Role.Schema.Attributes.businessunitid
                        , Role.Schema.Attributes.ismanaged
                        , Role.Schema.Attributes.iscustomizable
                        , Role.Schema.Attributes.roletemplateid
                    );

                    var temp = new List<LinkedEntities<Role>>();

                    if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                    {
                        var repository1 = new RoleRepository(service1);
                        var repository2 = new RoleRepository(service2);

                        var task1 = repository1.GetListAsync(filterRole, columnSet);
                        var task2 = repository2.GetListAsync(filterRole, columnSet);

                        var list1 = await task1;
                        var list2 = await task2;

                        foreach (var role1 in list1)
                        {
                            var role2 = list2.FirstOrDefault(c => c.Id == role1.Id);

                            if (role2 == null && role1.RoleTemplateId != null)
                            {
                                role2 = list2.FirstOrDefault(role => role.RoleTemplateId != null && role.RoleTemplateId.Id == role1.RoleTemplateId.Id);
                            }

                            if (role2 != null)
                            {
                                temp.Add(new LinkedEntities<Role>(role1, role2));
                            }
                        }
                    }
                    else
                    {
                        var repository1 = new RoleRepository(service1);

                        var task1 = repository1.GetListAsync(filterRole, columnSet);

                        var list1 = await task1;

                        foreach (var role1 in list1)
                        {
                            temp.Add(new LinkedEntities<Role>(role1, null));
                        }
                    }

                    list = temp;
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }

            this.lstVwSecurityRoles.Dispatcher.Invoke(() =>
            {
                foreach (var link in list.OrderBy(ent => ent.Entity1.Name))
                {
                    var item = new EntityViewItem(link);

                    this._itemsSourceRoles.Add(item);
                }

                if (this.lstVwSecurityRoles.Items.Count == 1)
                {
                    this.lstVwSecurityRoles.SelectedItem = this.lstVwSecurityRoles.Items[0];
                }
            });

            ToggleControls(true, Properties.OutputStrings.LoadingSecurityRolesCompletedFormat1, list.Count());
        }

        private class EntityViewItem
        {
            public string Name1 => Link.Entity1?.Name;

            public string Name2 => Link.Entity2?.Name;

            public string BusinessUnitName { get; private set; }

            public LinkedEntities<Role> Link { get; private set; }

            public bool IsCustomizable1 => Link.Entity1.IsCustomizable.Value;

            public bool IsCustomizable2 => Link.Entity2.IsCustomizable.Value;

            public EntityViewItem(LinkedEntities<Role> link)
            {
                this.Link = link;

                this.BusinessUnitName = Link.Entity1.BusinessUnitId.Name;

                if (Link.Entity1.BusinessUnitParentBusinessUnit == null)
                {
                    this.BusinessUnitName = "Root Organization";
                }
            }
        }

        private async Task ShowRoleEntityPrivileges()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.LoadingEntities);

            this.Dispatcher.Invoke(() =>
            {
                _itemsSourceEntityPrivileges.Clear();
                _itemsSourceOtherPrivileges.Clear();

                _currentRoleEntityPrivileges.Clear();
                _currentRoleOtherPrivileges.Clear();
            });

            var role = GetSelectedRole();

            try
            {
                var service1 = await GetService1();
                var service2 = await GetService2();

                if (service1 != null && service2 != null)
                {
                    if (service1.ConnectionData.ConnectionId != service2.ConnectionData.ConnectionId)
                    {
                        IEnumerable<Privilege> otherPrivileges1 = await GetOtherPrivileges(service1);
                        IEnumerable<Privilege> otherPrivileges2 = await GetOtherPrivileges(service2);

                        IEnumerable<EntityMetadata> entityMetadataList1 = await GetEntityMetadataEnumerable(service1);
                        IEnumerable<EntityMetadata> entityMetadataList2 = await GetEntityMetadataEnumerable(service2);

                        entityMetadataList1 = entityMetadataList1.Where(e => e.Privileges != null && e.Privileges.Any(p => p.PrivilegeType != PrivilegeType.None));
                        entityMetadataList2 = entityMetadataList2.Where(e => e.Privileges != null && e.Privileges.Any(p => p.PrivilegeType != PrivilegeType.None));

                        if (role != null)
                        {
                            if (entityMetadataList1.Any()
                                || entityMetadataList2.Any()
                                || otherPrivileges1.Any()
                                || otherPrivileges2.Any()
                            )
                            {
                                var repository1 = new RolePrivilegesRepository(service1);
                                var repository2 = new RolePrivilegesRepository(service2);

                                IEnumerable<RolePrivilege> rolePrivileges1 = await repository1.GetRolePrivilegesAsync(role.Entity1.Id);
                                IEnumerable<RolePrivilege> rolePrivileges2 = Enumerable.Empty<RolePrivilege>();

                                if (role.Entity2 != null)
                                {
                                    rolePrivileges2 = await repository2.GetRolePrivilegesAsync(role.Entity2.Id);
                                }

                                foreach (var entityMetadata1 in entityMetadataList1)
                                {
                                    var entityMetadata2 = entityMetadataList2.FirstOrDefault(e => string.Equals(e.LogicalName, entityMetadata1.LogicalName, StringComparison.InvariantCultureIgnoreCase));

                                    if (entityMetadata2 == null)
                                    {
                                        continue;
                                    }

                                    _currentRoleEntityPrivileges.Add(new RoleLinkedEntitiesPrivilegeViewItem(
                                        entityMetadata1
                                        , entityMetadata2
                                        , rolePrivileges1
                                        , rolePrivileges2
                                        , (role.Entity1?.IsCustomizable?.Value).GetValueOrDefault()
                                        , (role.Entity2?.IsCustomizable?.Value).GetValueOrDefault()
                                    ));
                                }

                                foreach (var priv1 in otherPrivileges1
                                    .OrderBy(p => p.LinkedEntitiesSorted)
                                    .ThenBy(p => p.Name, PrivilegeNameComparer.Comparer)
                                )
                                {
                                    var priv2 = otherPrivileges2.FirstOrDefault(p => string.Equals(p.Name, priv1.Name, StringComparison.InvariantCultureIgnoreCase));

                                    if (priv2 == null)
                                    {
                                        continue;
                                    }

                                    _currentRoleOtherPrivileges.Add(new RoleLinkedOtherPrivilegeViewItem(
                                        priv1
                                        , priv2
                                        , rolePrivileges1
                                        , rolePrivileges2
                                        , (role.Entity1?.IsCustomizable?.Value).GetValueOrDefault()
                                        , (role.Entity2?.IsCustomizable?.Value).GetValueOrDefault()
                                    ));
                                }
                            }
                        }
                    }
                    else
                    {
                        var otherPrivileges1 = await GetOtherPrivileges(service1);

                        IEnumerable<EntityMetadata> entityMetadataList1 = await GetEntityMetadataEnumerable(service1);

                        entityMetadataList1 = entityMetadataList1.Where(e => e.Privileges != null && e.Privileges.Any(p => p.PrivilegeType != PrivilegeType.None));

                        if (role != null)
                        {
                            if (entityMetadataList1.Any()
                                || otherPrivileges1.Any()
                            )
                            {
                                var repository1 = new RolePrivilegesRepository(service1);

                                IEnumerable<RolePrivilege> rolePrivileges1 = await repository1.GetRolePrivilegesAsync(role.Entity1.Id);

                                foreach (var entityMetadata1 in entityMetadataList1)
                                {
                                    _currentRoleEntityPrivileges.Add(new RoleLinkedEntitiesPrivilegeViewItem(
                                        entityMetadata1
                                        , null
                                        , rolePrivileges1
                                        , Enumerable.Empty<RolePrivilege>()
                                        , (role.Entity1?.IsCustomizable?.Value).GetValueOrDefault()
                                        , (role.Entity2?.IsCustomizable?.Value).GetValueOrDefault()
                                    ));
                                }

                                foreach (var priv1 in otherPrivileges1)
                                {
                                    _currentRoleOtherPrivileges.Add(new RoleLinkedOtherPrivilegeViewItem(
                                        priv1
                                        , null
                                        , rolePrivileges1
                                        , Enumerable.Empty<RolePrivilege>()
                                        , (role.Entity1?.IsCustomizable?.Value).GetValueOrDefault()
                                        , (role.Entity2?.IsCustomizable?.Value).GetValueOrDefault()
                                    ));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }

            ToggleControls(true, Properties.OutputStrings.LoadingEntitiesCompletedFormat1, _currentRoleEntityPrivileges.Count());

            PerformFilterEntityPrivileges();

            PerformFilterOtherPrivileges();
        }

        private void PerformFilterEntityPrivileges()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.FilteringEntities);

            string filterEntity = string.Empty;
            RoleEditorLayoutTab selectedTabEntities = null;

            this.Dispatcher.Invoke(() =>
            {
                _itemsSourceEntityPrivileges.Clear();

                filterEntity = txtBEntityFilter.Text.Trim().ToLower();
                selectedTabEntities = cmBRoleEditorLayoutTabsEntities.SelectedItem as RoleEditorLayoutTab;
            });

            var entityMetadataList = FilterEntityList(_currentRoleEntityPrivileges, filterEntity, selectedTabEntities);

            this.lstVwEntityPrivileges.Dispatcher.Invoke(() =>
            {
                foreach (var entity in entityMetadataList
                    .OrderBy(s => s.IsIntersect1)
                    .ThenBy(s => s.EntityMetadata1.LogicalName)
                )
                {
                    entity.PropertyChanged -= rolePrivilege_PropertyChanged;
                    entity.PropertyChanged -= rolePrivilege_PropertyChanged;
                    entity.PropertyChanged += rolePrivilege_PropertyChanged;

                    _itemsSourceEntityPrivileges.Add(entity);
                }

                if (this.lstVwEntityPrivileges.Items.Count == 1)
                {
                    this.lstVwEntityPrivileges.SelectedItem = this.lstVwEntityPrivileges.Items[0];
                }
            });

            ToggleControls(true, Properties.OutputStrings.FilteringEntitiesCompletedFormat1, entityMetadataList.Count());
        }

        private IEnumerable<RoleLinkedEntitiesPrivilegeViewItem> FilterEntityList(IEnumerable<RoleLinkedEntitiesPrivilegeViewItem> list, string filterEntity, RoleEditorLayoutTab selectedTab)
        {
            list = _entityMetadataFilter.FilterList(list, e => e.EntityMetadata1);

            if (selectedTab != null)
            {
                list = list.Where(e => selectedTab.EntitiesHash.Contains(e.ObjectTypeCode1)
                    || (e.ObjectTypeCode2.HasValue && selectedTab.EntitiesHash.Contains(e.ObjectTypeCode2.Value))
                );
            }

            if (!string.IsNullOrEmpty(filterEntity))
            {
                filterEntity = filterEntity.ToLower();

                if (int.TryParse(filterEntity, out int tempInt))
                {
                    list = list.Where(ent => ent.ObjectTypeCode1 == tempInt || ent.ObjectTypeCode2 == tempInt);
                }
                else if (Guid.TryParse(filterEntity, out Guid tempGuid))
                {
                    list = list.Where(ent => ent.EntityMetadata1.MetadataId == tempGuid || ent.EntityMetadata2?.MetadataId == tempGuid);
                }
                else
                {
                    list = list
                    .Where(ent =>
                        ent.LogicalName.IndexOf(filterEntity, StringComparison.InvariantCultureIgnoreCase) > -1
                        ||
                        (
                            ent.EntityMetadata1.DisplayName != null
                            && ent.EntityMetadata1.DisplayName.LocalizedLabels
                                .Where(l => !string.IsNullOrEmpty(l.Label))
                                .Any(lbl => lbl.Label.IndexOf(filterEntity, StringComparison.InvariantCultureIgnoreCase) > -1)
                        )
                        ||
                        (
                            ent.EntityMetadata2 != null
                            && ent.EntityMetadata2.DisplayName != null
                            && ent.EntityMetadata2.DisplayName.LocalizedLabels
                                .Where(l => !string.IsNullOrEmpty(l.Label))
                                .Any(lbl => lbl.Label.IndexOf(filterEntity, StringComparison.InvariantCultureIgnoreCase) > -1)
                        )
                    );
                }
            }

            return list;
        }

        private void PerformFilterOtherPrivileges()
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.FilteringOtherPrivileges);

            string filterOtherPrivilege = string.Empty;
            RoleEditorLayoutTab selectedTabEntities = null;

            this.Dispatcher.Invoke(() =>
            {
                _itemsSourceOtherPrivileges.Clear();

                filterOtherPrivilege = txtBOtherPrivilegesFilter.Text.Trim().ToLower();
                selectedTabEntities = cmBRoleEditorLayoutTabsPrivileges.SelectedItem as RoleEditorLayoutTab;
            });

            var otherPrivilegesList = FilterPrivilegeList(_currentRoleOtherPrivileges, filterOtherPrivilege, selectedTabEntities);

            this.lstVwOtherPrivileges.Dispatcher.Invoke(() =>
            {
                foreach (var otherPriv in otherPrivilegesList
                    .OrderBy(s => s.EntityLogicalName1)
                    .ThenBy(s => s.EntityLogicalName1)
                    .ThenBy(s => s.Name, PrivilegeNameComparer.Comparer)
                )
                {
                    otherPriv.PropertyChanged -= rolePrivilege_PropertyChanged;
                    otherPriv.PropertyChanged -= rolePrivilege_PropertyChanged;
                    otherPriv.PropertyChanged += rolePrivilege_PropertyChanged;

                    _itemsSourceOtherPrivileges.Add(otherPriv);
                }

                if (this.lstVwOtherPrivileges.Items.Count == 1)
                {
                    this.lstVwOtherPrivileges.SelectedItem = this.lstVwOtherPrivileges.Items[0];
                }
            });

            ToggleControls(true, Properties.OutputStrings.FilteringOtherPrivilegesCompletedFormat1, otherPrivilegesList.Count());
        }

        private static IEnumerable<RoleLinkedOtherPrivilegeViewItem> FilterPrivilegeList(IEnumerable<RoleLinkedOtherPrivilegeViewItem> list, string textName, RoleEditorLayoutTab selectedTab)
        {
            if (selectedTab != null)
            {
                list = list.Where(p => (p.Privilege1 != null && p.Privilege1.PrivilegeId.HasValue && selectedTab.PrivilegesHash.Contains(p.Privilege1.PrivilegeId.Value))
                    || (p.Privilege2 != null && p.Privilege2.PrivilegeId.HasValue && selectedTab.PrivilegesHash.Contains(p.Privilege2.PrivilegeId.Value))
                );
            }

            if (!string.IsNullOrEmpty(textName))
            {
                textName = textName.ToLower();

                if (Guid.TryParse(textName, out Guid tempGuid))
                {
                    list = list.Where(ent => (ent.Privilege1 != null && ent.Privilege1.Id == tempGuid)
                        || (ent.Privilege2 != null && ent.Privilege2.Id == tempGuid)
                    );
                }
                else
                {
                    list = list.Where(ent => ent.Name.IndexOf(textName, StringComparison.InvariantCultureIgnoreCase) != -1);
                }
            }

            return list;
        }

        private LinkedEntities<Role> GetSelectedRole()
        {
            EntityViewItem result = null;

            lstVwSecurityRoles.Dispatcher.Invoke(() =>
            {
                result = this.lstVwSecurityRoles.SelectedItems.Cast<EntityViewItem>().Count() == 1
                    ? this.lstVwSecurityRoles.SelectedItems.Cast<EntityViewItem>().SingleOrDefault() : null;
            });

            return result?.Link;
        }

        private void rolePrivilege_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, nameof(RoleLinkedEntitiesPrivilegeViewItem.IsChanged1), StringComparison.InvariantCultureIgnoreCase))
            {
                UpdateSaveRoleChangesButtons1();
            }
            else if (string.Equals(e.PropertyName, nameof(RoleLinkedEntitiesPrivilegeViewItem.IsChanged2), StringComparison.InvariantCultureIgnoreCase))
            {
                UpdateSaveRoleChangesButtons2();
            }
        }

        private void UpdateSaveRoleChangesButtons1()
        {
            this.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled &&
                        (_itemsSourceEntityPrivileges.Any(e => e.IsChanged1) || _itemsSourceOtherPrivileges.Any(e => e.IsChanged1));

                    UIElement[] list =
                    {
                        btnSaveRoleChanges1
                    };

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

        private void UpdateSaveRoleChangesButtons2()
        {
            this.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled &&
                        (_itemsSourceEntityPrivileges.Any(e => e.IsChanged2) || _itemsSourceOtherPrivileges.Any(e => e.IsChanged2));

                    UIElement[] list =
                    {
                        btnSaveRoleChanges2
                    };

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

        private void UpdateStatus(string format, params object[] args)
        {
            string message = format;

            if (args != null && args.Length > 0)
            {
                message = string.Format(format, args);
            }

            _iWriteToOutput.WriteToOutput(null, message);

            this.stBIStatus.Dispatcher.Invoke(() =>
            {
                this.stBIStatus.Content = message;
            });
        }

        protected override void ToggleControls(ConnectionData connectionData, bool enabled, string statusFormat, params object[] args)
        {
            ToggleControls(enabled, statusFormat, args);
        }

        protected void ToggleControls(bool enabled, string statusFormat, params object[] args)
        {
            this.ChangeInitByEnabled(enabled);

            UpdateStatus(statusFormat, args);

            ToggleControl(this.tSProgressBar, this.cmBConnection1, this.cmBConnection2);

            UpdateButtonsEnable();
        }

        private void UpdateButtonsEnable()
        {
            this.lstVwSecurityRoles.Dispatcher.Invoke(() =>
            {
                try
                {
                    bool enabled = this.IsControlsEnabled && this.lstVwSecurityRoles.SelectedItems.Count > 0;

                    var item = (this.lstVwSecurityRoles.SelectedItems[0] as EntityViewItem);

                    tSDDBShowDifference.IsEnabled = enabled && item.Link.Entity1 != null && item.Link.Entity2 != null;
                    tSDDBConnection1.IsEnabled = enabled && item.Link.Entity1 != null;
                    tSDDBConnection2.IsEnabled = enabled && item.Link.Entity2 != null;
                }
                catch (Exception)
                {
                }
            });
        }

        private async void txtBFilterRole_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await ShowExistingSecurityRoles();
            }
        }

        private EntityViewItem GetSelectedRoleLink()
        {
            return this.lstVwSecurityRoles.SelectedItems.Cast<EntityViewItem>().Count() == 1
                ? this.lstVwSecurityRoles.SelectedItems.Cast<EntityViewItem>().SingleOrDefault() : null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lstVwSecurityRoles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                EntityViewItem item = GetItemFromRoutedDataContext<EntityViewItem>(e);

                if (item != null)
                {
                    ExecuteActionOnLinkedEntities(item.Link, false, PerformShowingDifferenceAllAsync);
                }
            }
        }

        private async void lstVwSecurityRoles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsEnable();

            await RefreshRoleInfo();
        }

        private async Task RefreshRoleInfo()
        {
            try
            {
                await ShowRoleEntityPrivileges();
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
        }

        private void ExecuteActionOnLinkedEntities(LinkedEntities<Role> linked, bool showAllways, Func<LinkedEntities<Role>, bool, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            if (linked.Entity1 == null || linked.Entity2 == null || linked.Entity1 == linked.Entity2)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            action(linked, showAllways);
        }

        private void btnShowDifferenceAll_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedRoleLink();

            if (link == null)
            {
                return;
            }

            ExecuteActionOnLinkedEntities(link.Link, false, PerformShowingDifferenceAllAsync);
        }

        private async Task PerformShowingDifferenceAllAsync(LinkedEntities<Role> linked, bool showAllways)
        {
            //await PerformShowingDifferenceRoleDescriptionAsync(linked, showAllways);

            //await PerformShowingDifferenceEntityDescriptionAsync(linked, showAllways);
        }

        private void mIShowDifferenceRoleDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedRoleLink();

            if (link == null)
            {
                return;
            }

            //ExecuteActionOnLinkedEntities(link.Link, true, PerformShowingDifferenceRoleDescriptionAsync);
        }

        //private async Task PerformShowingDifferenceRoleDescriptionAsync(LinkedEntities<Role> linked, bool showAllways)
        //{
        //    if (!this.IsControlsEnabled)
        //    {
        //        return;
        //    }

        //    ToggleControls(false, Properties.OutputStrings.ShowingDifferenceRoleDescriptionFormat1, linked.Entity1.Name);

        //    var service1 = await GetService1();
        //    var service2 = await GetService2();

        //    if (service1 != null && service2 != null)
        //    {
        //        var handler1 = new RoleDescriptionHandler(service1, service1.ConnectionData.GetConnectionInfo());
        //        var handler2 = new RoleDescriptionHandler(service2, service2.ConnectionData.GetConnectionInfo());

        //        DateTime now = DateTime.Now;

        //        string desc1 = await handler1.CreateDescriptionAsync(linked.Entity1.Id, linked.Entity1.Name, now);
        //        string desc2 = await handler2.CreateDescriptionAsync(linked.Entity2.Id, linked.Entity2.Name, now);

        //        if (showAllways || desc1 != desc2)
        //        {
        //            string filePath1 = await CreateDescriptionFileAsync(service1.ConnectionData, linked.Entity1.Name, "Description", desc1);
        //            string filePath2 = await CreateDescriptionFileAsync(service2.ConnectionData, linked.Entity2.Name, "Description", desc2);

        //            if (File.Exists(filePath1) && File.Exists(filePath2))
        //            {
        //                await this._iWriteToOutput.ProcessStartProgramComparerAsync(service1.ConnectionData, filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2), service2.ConnectionData);
        //            }
        //            else
        //            {
        //                this._iWriteToOutput.PerformAction(service1.ConnectionData, filePath1);

        //                this._iWriteToOutput.PerformAction(service2.ConnectionData, filePath2);
        //            }
        //        }
        //    }

        //    ToggleControls(true, Properties.OutputStrings.ShowingDifferenceRoleDescriptionCompletedFormat1, linked.Entity1.Name);
        //}

        private void mIShowDifferenceEntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedRoleLink();

            if (link == null)
            {
                return;
            }

            ExecuteActionOnLinkedEntities(link.Link, true, PerformShowingDifferenceEntityDescriptionAsync);
        }

        private async Task PerformShowingDifferenceEntityDescriptionAsync(LinkedEntities<Role> linked, bool showAllways)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.ShowingDifferenceEntityDescription);

            try
            {
                var service1 = await GetService1();
                var service2 = await GetService2();

                if (service1 != null && service2 != null)
                {
                    var task1 = service1.RetrieveByQueryAsync<Role>(linked.Entity1.LogicalName, linked.Entity1.Id, ColumnSetInstances.AllColumns);
                    var task2 = service1.RetrieveByQueryAsync<Role>(linked.Entity2.LogicalName, linked.Entity2.Id, ColumnSetInstances.AllColumns);

                    var role1 = await task1;
                    var role2 = await task2;

                    var desc1 = await EntityDescriptionHandler.GetEntityDescriptionAsync(role1);
                    var desc2 = await EntityDescriptionHandler.GetEntityDescriptionAsync(role2);

                    if (showAllways || desc1 != desc2)
                    {
                        string filePath1 = await CreateDescriptionFileAsync(service1.ConnectionData, role1.Name, EntityFileNameFormatter.Headers.EntityDescription, desc1);
                        string filePath2 = await CreateDescriptionFileAsync(service2.ConnectionData, role2.Name, EntityFileNameFormatter.Headers.EntityDescription, desc2);

                        if (File.Exists(filePath1) && File.Exists(filePath2))
                        {
                            await this._iWriteToOutput.ProcessStartProgramComparerAsync(service1.ConnectionData, filePath1, filePath2, Path.GetFileName(filePath1), Path.GetFileName(filePath2), service2.ConnectionData);
                        }
                        else
                        {
                            this._iWriteToOutput.PerformAction(service1.ConnectionData, filePath1);

                            this._iWriteToOutput.PerformAction(service2.ConnectionData, filePath2);
                        }
                    }
                }

                ToggleControls(true, Properties.OutputStrings.ShowingDifferenceEntityDescriptionCompleted);
            }
            catch (Exception ex)
            {
                _iWriteToOutput.WriteErrorToOutput(null, ex);

                ToggleControls(true, Properties.OutputStrings.ShowingDifferenceEntityDescriptionFailed);
            }
        }

        private void ExecuteActionRoleDescription(
            Guid RoleId
            , Func<Task<IOrganizationServiceExtented>> getService
            , Func<Guid, Func<Task<IOrganizationServiceExtented>>, Task> action)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            _commonConfig.CheckFolderForExportExists(_iWriteToOutput);

            action(RoleId, getService);
        }

        private void mIExportRole1EntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedRoleLink();

            if (link == null && link.Link.Entity1 != null)
            {
                return;
            }

            ExecuteActionRoleDescription(link.Link.Entity1.Id, GetService1, PerformExportEntityDescriptionToFileAsync);
        }

        private void mIExportRole2EntityDescription_Click(object sender, RoutedEventArgs e)
        {
            var link = GetSelectedRoleLink();

            if (link == null && link.Link.Entity2 != null)
            {
                return;
            }

            ExecuteActionRoleDescription(link.Link.Entity2.Id, GetService2, PerformExportEntityDescriptionToFileAsync);
        }

        private async Task PerformExportEntityDescriptionToFileAsync(Guid idRole, Func<Task<IOrganizationServiceExtented>> getService)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            ToggleControls(false, Properties.OutputStrings.CreatingEntityDescription);

            var service = await getService();

            if (service != null)
            {
                var role = await service.RetrieveByQueryAsync<Role>(Role.EntityLogicalName, idRole, ColumnSetInstances.AllColumns);

                var description = await EntityDescriptionHandler.GetEntityDescriptionAsync(role, service.ConnectionData);

                string filePath = await CreateDescriptionFileAsync(service.ConnectionData, role.Name, EntityFileNameFormatter.Headers.EntityDescription, description);

                this._iWriteToOutput.PerformAction(service.ConnectionData, filePath);
            }

            ToggleControls(true, Properties.OutputStrings.CreatingEntityDescriptionCompleted);
        }

        private Task<string> CreateDescriptionFileAsync(ConnectionData connectionData, string name, string fieldTitle, string description)
        {
            return Task.Run(() => CreateDescriptionFile(connectionData, name, fieldTitle, description));
        }

        private string CreateDescriptionFile(ConnectionData connectionData, string name, string fieldTitle, string description)
        {
            string fileName = EntityFileNameFormatter.GetRoleFileName(connectionData.Name, name, fieldTitle, FileExtension.txt);
            string filePath = Path.Combine(_commonConfig.FolderForExport, FileOperations.RemoveWrongSymbols(fileName));

            if (!string.IsNullOrEmpty(description))
            {
                try
                {
                    File.WriteAllText(filePath, description, new UTF8Encoding(false));

                    this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.InConnectionEntityFieldExportedToFormat5, connectionData.Name, Role.Schema.EntityLogicalName, name, fieldTitle, filePath);
                }
                catch (Exception ex)
                {
                    this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
                }
            }
            else
            {
                filePath = string.Empty;
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.InConnectionEntityFieldIsEmptyFormat4, connectionData.Name, Role.Schema.EntityLogicalName, name, fieldTitle);
                this._iWriteToOutput.ActivateOutputWindow(connectionData);
            }

            return filePath;
        }

        protected override async Task OnRefreshList(ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            await ShowExistingSecurityRoles();
        }

        private void cmBCurrentConnection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.IsControlsEnabled)
            {
                return;
            }

            this.Dispatcher.Invoke(() =>
            {
                this._itemsSourceRoles?.Clear();

                ConnectionData connection1 = cmBConnection1.SelectedItem as ConnectionData;
                ConnectionData connection2 = cmBConnection2.SelectedItem as ConnectionData;

                if (connection1 != null && connection2 != null)
                {
                    this.Resources["ConnectionName1"] = connection1.Name;
                    this.Resources["ConnectionName2"] = connection2.Name;

                    UpdateButtonsEnable();

                    var task = ShowExistingSecurityRoles();
                }
            });
        }

        private async void btnExplorerRole1_Click(object sender, RoutedEventArgs e)
        {
            var roleLink = GetSelectedRoleLink();

            _commonConfig.Save();

            var service = await GetService1();

            if (service == null)
            {
                return;
            }

            WindowHelper.OpenRolesExplorer(this._iWriteToOutput, service, _commonConfig, roleLink?.Name1 ?? txtBFilterRole.Text);
        }

        private async void btnExplorerRole2_Click(object sender, RoutedEventArgs e)
        {
            var roleLink = GetSelectedRoleLink();

            _commonConfig.Save();

            var service = await GetService2();

            if (service == null)
            {
                return;
            }

            WindowHelper.OpenRolesExplorer(this._iWriteToOutput, service, _commonConfig, roleLink?.Name1 ?? txtBFilterRole.Text);
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (!(sender is ContextMenu contextMenu))
            {
                return;
            }

            EntityViewItem linkedEntityMetadata = GetItemFromRoutedDataContext<EntityViewItem>(e);

            var hasTwoEntities = linkedEntityMetadata != null
                && linkedEntityMetadata.Link != null
                && linkedEntityMetadata.Link.Entity1 != null
                && linkedEntityMetadata.Link.Entity2 != null;

            var hasSecondEntity = linkedEntityMetadata != null
                && linkedEntityMetadata.Link != null
                && linkedEntityMetadata.Link.Entity2 != null;

            var items = contextMenu.Items.OfType<Control>();

            ActivateControls(items, hasTwoEntities, "menuContextDifference");

            ActivateControls(items, hasSecondEntity, "menuContextConnection2");
        }

        private void mISaveRoleChanges1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mISaveRoleChanges2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mIClearEntityCacheAndRefresh_Click(object sender, RoutedEventArgs e)
        {

        }

        #region Set Attribute

        private void mISetAttributeEntityPrivilegeAll1_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is PrivilegeDepthExtended privilegeDepth
            )
            {
                SetSelectedRoleEntityPrivileges1(privilegeDepth);
            }
        }

        private void SetSelectedRoleEntityPrivileges1(PrivilegeDepthExtended privilegeDepth)
        {
            var list = lstVwEntityPrivileges.SelectedItems.Cast<RoleLinkedEntitiesPrivilegeViewItem>().Where(e => e.EntityMetadata1 != null).ToList();

            if (!list.Any())
            {
                return;
            }

            foreach (var privilegeType in _privielgeTypesAll)
            {
                foreach (var item in list)
                {
                    SetSelectedRoleEntityPrivileges1(item, privilegeType, privilegeDepth);
                }
            }
        }

        private void mISetAttributeEntityPrivilegeCreate1_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is PrivilegeDepthExtended privilegeDepth
            )
            {
                SetSelectedRoleEntityPrivileges1(PrivilegeType.Create, privilegeDepth);
            }
        }

        private void mISetAttributeEntityPrivilegeRead1_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is PrivilegeDepthExtended privilegeDepth
            )
            {
                SetSelectedRoleEntityPrivileges1(PrivilegeType.Read, privilegeDepth);
            }
        }

        private void mISetAttributeEntityPrivilegeUpdate1_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is PrivilegeDepthExtended privilegeDepth
            )
            {
                SetSelectedRoleEntityPrivileges1(PrivilegeType.Write, privilegeDepth);
            }
        }

        private void mISetAttributeEntityPrivilegeDelete1_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is PrivilegeDepthExtended privilegeDepth
            )
            {
                SetSelectedRoleEntityPrivileges1(PrivilegeType.Delete, privilegeDepth);
            }
        }

        private void mISetAttributeEntityPrivilegeAppend1_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is PrivilegeDepthExtended privilegeDepth
            )
            {
                SetSelectedRoleEntityPrivileges1(PrivilegeType.Append, privilegeDepth);
            }
        }

        private void mISetAttributeEntityPrivilegeAppendTo1_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is PrivilegeDepthExtended privilegeDepth
            )
            {
                SetSelectedRoleEntityPrivileges1(PrivilegeType.AppendTo, privilegeDepth);
            }
        }

        private void mISetAttributeEntityPrivilegeAssign1_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is PrivilegeDepthExtended privilegeDepth
            )
            {
                SetSelectedRoleEntityPrivileges1(PrivilegeType.Assign, privilegeDepth);
            }
        }

        private void mISetAttributeEntityPrivilegeShare1_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is PrivilegeDepthExtended privilegeDepth
            )
            {
                SetSelectedRoleEntityPrivileges1(PrivilegeType.Share, privilegeDepth);
            }
        }

        private void SetSelectedRoleEntityPrivileges1(PrivilegeType privilegeType, PrivilegeDepthExtended privilegeDepth)
        {
            var list = lstVwEntityPrivileges.SelectedItems.Cast<RoleLinkedEntitiesPrivilegeViewItem>().Where(e => e.EntityMetadata1 != null).ToList();

            if (!list.Any())
            {
                return;
            }

            foreach (var item in list)
            {
                SetSelectedRoleEntityPrivileges1(item, privilegeType, privilegeDepth);
            }
        }

        private static void SetSelectedRoleEntityPrivileges1(RoleLinkedEntitiesPrivilegeViewItem item, PrivilegeType privilegeType, PrivilegeDepthExtended privilegeDepth)
        {
            switch (privilegeType)
            {
                case PrivilegeType.Create:
                    if (item.AvailableCreate1 && item.CreateOptions1.Contains(privilegeDepth))
                    {
                        item.CreateRight1 = privilegeDepth;
                    }
                    break;

                case PrivilegeType.Read:
                    if (item.AvailableRead1 && item.ReadOptions1.Contains(privilegeDepth))
                    {
                        item.ReadRight1 = privilegeDepth;
                    }
                    break;

                case PrivilegeType.Write:
                    if (item.AvailableUpdate1 && item.UpdateOptions1.Contains(privilegeDepth))
                    {
                        item.UpdateRight1 = privilegeDepth;
                    }
                    break;

                case PrivilegeType.Delete:
                    if (item.AvailableDelete1 && item.DeleteOptions1.Contains(privilegeDepth))
                    {
                        item.DeleteRight1 = privilegeDepth;
                    }
                    break;

                case PrivilegeType.Assign:
                    if (item.AvailableAssign1 && item.AssignOptions1.Contains(privilegeDepth))
                    {
                        item.AssignRight1 = privilegeDepth;
                    }
                    break;

                case PrivilegeType.Share:
                    if (item.AvailableShare1 && item.ShareOptions1.Contains(privilegeDepth))
                    {
                        item.ShareRight1 = privilegeDepth;
                    }
                    break;

                case PrivilegeType.Append:
                    if (item.AvailableAppend1 && item.AppendOptions1.Contains(privilegeDepth))
                    {
                        item.AppendRight1 = privilegeDepth;
                    }
                    break;

                case PrivilegeType.AppendTo:
                    if (item.AvailableAppendTo1 && item.AppendToOptions1.Contains(privilegeDepth))
                    {
                        item.AppendToRight1 = privilegeDepth;
                    }
                    break;

                case PrivilegeType.None:
                default:
                    break;
            }
        }

        private void mISetAttributeEntityPrivilegeAll2_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is PrivilegeDepthExtended privilegeDepth
            )
            {
                SetSelectedRoleEntityPrivileges2(privilegeDepth);
            }
        }

        private void SetSelectedRoleEntityPrivileges2(PrivilegeDepthExtended privilegeDepth)
        {
            var list = lstVwEntityPrivileges.SelectedItems.Cast<RoleLinkedEntitiesPrivilegeViewItem>().Where(e => e.EntityMetadata2 != null).ToList();

            if (!list.Any())
            {
                return;
            }

            foreach (var privilegeType in _privielgeTypesAll)
            {
                foreach (var item in list)
                {
                    SetSelectedRoleEntityPrivileges2(item, privilegeType, privilegeDepth);
                }
            }
        }

        private void mISetAttributeEntityPrivilegeCreate2_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is PrivilegeDepthExtended privilegeDepth
            )
            {
                SetSelectedRoleEntityPrivileges2(PrivilegeType.Create, privilegeDepth);
            }
        }

        private void mISetAttributeEntityPrivilegeRead2_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is PrivilegeDepthExtended privilegeDepth
            )
            {
                SetSelectedRoleEntityPrivileges2(PrivilegeType.Read, privilegeDepth);
            }
        }

        private void mISetAttributeEntityPrivilegeUpdate2_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is PrivilegeDepthExtended privilegeDepth
            )
            {
                SetSelectedRoleEntityPrivileges2(PrivilegeType.Write, privilegeDepth);
            }
        }

        private void mISetAttributeEntityPrivilegeDelete2_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is PrivilegeDepthExtended privilegeDepth
            )
            {
                SetSelectedRoleEntityPrivileges2(PrivilegeType.Delete, privilegeDepth);
            }
        }

        private void mISetAttributeEntityPrivilegeAppend2_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is PrivilegeDepthExtended privilegeDepth
            )
            {
                SetSelectedRoleEntityPrivileges2(PrivilegeType.Append, privilegeDepth);
            }
        }

        private void mISetAttributeEntityPrivilegeAppendTo2_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is PrivilegeDepthExtended privilegeDepth
            )
            {
                SetSelectedRoleEntityPrivileges2(PrivilegeType.AppendTo, privilegeDepth);
            }
        }

        private void mISetAttributeEntityPrivilegeAssign2_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is PrivilegeDepthExtended privilegeDepth
            )
            {
                SetSelectedRoleEntityPrivileges2(PrivilegeType.Assign, privilegeDepth);
            }
        }

        private void mISetAttributeEntityPrivilegeShare2_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is PrivilegeDepthExtended privilegeDepth
            )
            {
                SetSelectedRoleEntityPrivileges2(PrivilegeType.Share, privilegeDepth);
            }
        }

        private void SetSelectedRoleEntityPrivileges2(PrivilegeType privilegeType, PrivilegeDepthExtended privilegeDepth)
        {
            var list = lstVwEntityPrivileges.SelectedItems.Cast<RoleLinkedEntitiesPrivilegeViewItem>().Where(e => e.EntityMetadata2 != null).ToList();

            if (!list.Any())
            {
                return;
            }

            foreach (var item in list)
            {
                SetSelectedRoleEntityPrivileges2(item, privilegeType, privilegeDepth);
            }
        }

        private static void SetSelectedRoleEntityPrivileges2(RoleLinkedEntitiesPrivilegeViewItem item, PrivilegeType privilegeType, PrivilegeDepthExtended privilegeDepth)
        {
            switch (privilegeType)
            {
                case PrivilegeType.Create:
                    if (item.AvailableCreate2 && item.CreateOptions2.Contains(privilegeDepth))
                    {
                        item.CreateRight2 = privilegeDepth;
                    }
                    break;

                case PrivilegeType.Read:
                    if (item.AvailableRead2 && item.ReadOptions2.Contains(privilegeDepth))
                    {
                        item.ReadRight2 = privilegeDepth;
                    }
                    break;

                case PrivilegeType.Write:
                    if (item.AvailableUpdate2 && item.UpdateOptions2.Contains(privilegeDepth))
                    {
                        item.UpdateRight2 = privilegeDepth;
                    }
                    break;

                case PrivilegeType.Delete:
                    if (item.AvailableDelete2 && item.DeleteOptions2.Contains(privilegeDepth))
                    {
                        item.DeleteRight2 = privilegeDepth;
                    }
                    break;

                case PrivilegeType.Assign:
                    if (item.AvailableAssign2 && item.AssignOptions2.Contains(privilegeDepth))
                    {
                        item.AssignRight2 = privilegeDepth;
                    }
                    break;

                case PrivilegeType.Share:
                    if (item.AvailableShare2 && item.ShareOptions2.Contains(privilegeDepth))
                    {
                        item.ShareRight2 = privilegeDepth;
                    }
                    break;

                case PrivilegeType.Append:
                    if (item.AvailableAppend2 && item.AppendOptions2.Contains(privilegeDepth))
                    {
                        item.AppendRight2 = privilegeDepth;
                    }
                    break;

                case PrivilegeType.AppendTo:
                    if (item.AvailableAppendTo2 && item.AppendToOptions2.Contains(privilegeDepth))
                    {
                        item.AppendToRight2 = privilegeDepth;
                    }
                    break;

                case PrivilegeType.None:
                default:
                    break;
            }
        }

        private void mISetAttributeOtherPrivilegeRight1_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is PrivilegeDepthExtended privilegeDepth
            )
            {
                SetSelectedRoleOtherPrivileges1(privilegeDepth);
            }
        }

        private void SetSelectedRoleOtherPrivileges1(PrivilegeDepthExtended privilegeDepth)
        {
            var list = lstVwOtherPrivileges.SelectedItems.Cast<RoleLinkedOtherPrivilegeViewItem>().Where(p => p.Privilege1 != null).ToList();

            if (!list.Any())
            {
                return;
            }

            foreach (var item in list)
            {
                if (item.RightOptions1.Contains(privilegeDepth))
                {
                    item.Right1 = privilegeDepth;
                }
            }
        }

        private void mISetAttributeOtherPrivilegeRight2_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem
                && menuItem.Tag != null
                && menuItem.Tag is PrivilegeDepthExtended privilegeDepth
            )
            {
                SetSelectedRoleOtherPrivileges2(privilegeDepth);
            }
        }

        private void SetSelectedRoleOtherPrivileges2(PrivilegeDepthExtended privilegeDepth)
        {
            var list = lstVwOtherPrivileges.SelectedItems.Cast<RoleLinkedOtherPrivilegeViewItem>().Where(p => p.Privilege2 != null).ToList();

            if (!list.Any())
            {
                return;
            }

            foreach (var item in list)
            {
                if (item.RightOptions2.Contains(privilegeDepth))
                {
                    item.Right2 = privilegeDepth;
                }
            }
        }

        #endregion Set Attribute

        private void LstVwEntityPrivileges_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtBEntityFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PerformFilterEntityPrivileges();
            }
        }

        private void txtBOtherPrivilegesFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PerformFilterOtherPrivileges();
            }
        }

        private void cmBRoleEditorLayoutTabsEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PerformFilterEntityPrivileges();
        }

        private void cmBRoleEditorLayoutTabsPrivileges_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PerformFilterOtherPrivileges();
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

        private void btnEntityMetadataFilter_Click(object sender, RoutedEventArgs e)
        {
            _popupEntityMetadataFilter.IsOpen = true;
            _popupEntityMetadataFilter.Child.Focus();
        }

        private void popupEntityMetadataFilter_Closed(object sender, EventArgs e)
        {
            if (_entityMetadataFilter.FilterChanged)
            {
                PerformFilterEntityPrivileges();
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

        private async void btnRefreshEntites_Click(object sender, RoutedEventArgs e)
        {
            await ShowRoleEntityPrivileges();
        }
    }
}