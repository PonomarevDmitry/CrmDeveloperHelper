using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public class ExplorersHelper : BaseExplorersHelper
    {
        private readonly Func<string> _getOtherPrivilegeName;
        private readonly Func<string> _getMessageName;
        private readonly Func<string> _getPluginTypeName;

        private readonly Func<Task<IOrganizationServiceExtented>> GetService;

        private readonly Func<Guid, IEnumerable<EntityMetadata>> _getEntityMetadataList;
        private readonly Func<Guid, IEnumerable<Privilege>> _getOtherPrivilegesList;
        private readonly Func<Guid, IEnumerable<OptionSetMetadata>> _getGlobalOptionSetMetadataList;

        private readonly EnvDTE.SelectedItem _selectedItem = null;

        public ExplorersHelper(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , Func<Task<IOrganizationServiceExtented>> getService
            , EnvDTE.SelectedItem selectedItem
            , Func<string> getEntityName = null
            , Func<string> getGlobalOptionSetName = null
            , Func<string> getWorkflowName = null
            , Func<string> getSystemFormName = null
            , Func<string> getSavedQueryName = null
            , Func<string> getSavedQueryVisualizationName = null
            , Func<string> getSiteMapName = null
            , Func<string> getReportName = null
            , Func<string> getWebResourceName = null
            , Func<string> getPluginAssemblyName = null
            , Func<string> getOtherPrivilegeName = null
            , Func<string> getMessageName = null
            , Func<string> getPluginTypeName = null
            , Func<Guid, IEnumerable<EntityMetadata>> getEntityMetadataList = null
            , Func<Guid, IEnumerable<Privilege>> getOtherPrivilegesList = null
            , Func<Guid, IEnumerable<OptionSetMetadata>> getGlobalOptionSetMetadataList = null
        ) : this(iWriteToOutput, commonConfig, getService
            , getEntityName
            , getGlobalOptionSetName
            , getWorkflowName
            , getSystemFormName
            , getSavedQueryName
            , getSavedQueryVisualizationName
            , getSiteMapName
            , getReportName
            , getWebResourceName
            , getPluginAssemblyName
            , getOtherPrivilegeName
            , getMessageName
            , getPluginTypeName
            , getEntityMetadataList
            , getOtherPrivilegesList
            , getGlobalOptionSetMetadataList
        )
        {
            this._selectedItem = selectedItem;
        }

        public ExplorersHelper(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , Func<Task<IOrganizationServiceExtented>> getService
            , Func<string> getEntityName = null
            , Func<string> getGlobalOptionSetName = null
            , Func<string> getWorkflowName = null
            , Func<string> getSystemFormName = null
            , Func<string> getSavedQueryName = null
            , Func<string> getSavedQueryVisualizationName = null
            , Func<string> getSiteMapName = null
            , Func<string> getReportName = null
            , Func<string> getWebResourceName = null
            , Func<string> getPluginAssemblyName = null
            , Func<string> getOtherPrivilegeName = null
            , Func<string> getMessageName = null
            , Func<string> getPluginTypeName = null
            , Func<Guid, IEnumerable<EntityMetadata>> getEntityMetadataList = null
            , Func<Guid, IEnumerable<Privilege>> getOtherPrivilegesList = null
            , Func<Guid, IEnumerable<OptionSetMetadata>> getGlobalOptionSetMetadataList = null
        ) : base(iWriteToOutput, commonConfig
            , getEntityName
            , getGlobalOptionSetName
            , getWorkflowName
            , getSystemFormName
            , getSavedQueryName
            , getSavedQueryVisualizationName
            , getSiteMapName
            , getReportName
            , getWebResourceName
            , getPluginAssemblyName
        )
        {
            this.GetService = getService;

            this._getOtherPrivilegeName = getOtherPrivilegeName;
            this._getMessageName = getMessageName;
            this._getPluginTypeName = getPluginTypeName;

            this._getEntityMetadataList = getEntityMetadataList;
            this._getOtherPrivilegesList = getOtherPrivilegesList;
            this._getGlobalOptionSetMetadataList = getGlobalOptionSetMetadataList;
        }

        protected string GetOtherPrivilegeName()
        {
            if (_getOtherPrivilegeName != null)
            {
                return _getOtherPrivilegeName();
            }

            return string.Empty;
        }

        protected string GetMessageName()
        {
            if (_getMessageName != null)
            {
                return _getMessageName();
            }

            return string.Empty;
        }

        protected string GetPluginTypeName()
        {
            if (_getPluginTypeName != null)
            {
                return _getPluginTypeName();
            }

            return string.Empty;
        }

        private IEnumerable<EntityMetadata> GetEntityMetadataList(Guid connectionId)
        {
            if (_getEntityMetadataList != null)
            {
                return _getEntityMetadataList(connectionId);
            }

            return null;
        }

        private IEnumerable<OptionSetMetadata> GetGlobalOptionSetMetadataList(Guid connectionId)
        {
            if (_getGlobalOptionSetMetadataList != null)
            {
                return _getGlobalOptionSetMetadataList(connectionId);
            }

            return null;
        }

        private IEnumerable<Privilege> GetOtherPrivilegesList(Guid connectionId)
        {
            if (_getOtherPrivilegesList != null)
            {
                return _getOtherPrivilegesList(connectionId);
            }

            return null;
        }

        public void FillExplorers(ContextMenu contextMenu, params string[] uidList)
        {
            if (uidList == null || uidList.Length == 0)
            {
                return;
            }

            var items = contextMenu.Items.OfType<Control>();

            FillExplorers(items, uidList);
        }

        public void FillExplorers(IEnumerable<Control> items, params string[] uidList)
        {
            if (uidList == null || uidList.Length == 0)
            {
                return;
            }

            HashSet<string> hash = new HashSet<string>(uidList, StringComparer.InvariantCultureIgnoreCase);

            IEnumerable<MenuItem> source = WindowBase.GetMenuItems(items);

            foreach (var item in source)
            {
                if (hash.Contains(item.Uid))
                {
                    FillExplorers(item);
                }
            }
        }

        public void FillExplorers(MenuItem miExplorers)
        {
            var miEntityMetadataExplorer = new MenuItem()
            {
                Header = "Entity Metadata",
            };
            miEntityMetadataExplorer.Click += miEntityMetadataExplorer_Click;

            var miEntityMetadataExplorerSelectedItem = new MenuItem()
            {
                Header = "Entity Metadata in Addition Mode",
            };
            miEntityMetadataExplorerSelectedItem.Click += MiEntityMetadataExplorerSelectedItem_Click;


            var miEntityAttributeExplorer = new MenuItem()
            {
                Header = "Entity Attribute Explorer",
            };
            miEntityAttributeExplorer.Click += miEntityAttributeExplorer_Click;

            var miEntityRelationshipOneToManyExplorer = new MenuItem()
            {
                Header = "Entity Relationship One-To-Many Explorer",
            };
            miEntityRelationshipOneToManyExplorer.Click += miEntityRelationshipOneToManyExplorer_Click;

            var miEntityRelationshipManyToManyExplorer = new MenuItem()
            {
                Header = "Entity Relationship Many-To-Many Explorer",
            };
            miEntityRelationshipManyToManyExplorer.Click += miEntityRelationshipManyToManyExplorer_Click;

            var miEntityKeyExplorer = new MenuItem()
            {
                Header = "Entity Key Explorer",
            };
            miEntityKeyExplorer.Click += miEntityKeyExplorer_Click;

            var miEntityPrivilegesExplorer = new MenuItem()
            {
                Header = "Entity Privileges Explorer",
            };
            miEntityPrivilegesExplorer.Click += miEntityPrivilegesExplorer_Click;

            var miOtherPrivilegesExplorer = new MenuItem()
            {
                Header = "Other Privileges Explorer",
            };
            miOtherPrivilegesExplorer.Click += miOtherPrivilegesExplorer_Click;

            var miSecurityRolesExplorer = new MenuItem()
            {
                Header = "SecurityRoles Explorer",
            };
            miSecurityRolesExplorer.Click += miSecurityRolesExplorer_Click;




            var miGlobalOptionSets = new MenuItem()
            {
                Header = "Global OptionSets",
            };
            miGlobalOptionSets.Click += miGlobalOptionSets_Click;

            var miGlobalOptionSetsSelectedItem = new MenuItem()
            {
                Header = "Global OptionSets in Addition Mode",
            };
            miGlobalOptionSetsSelectedItem.Click += miGlobalOptionSetsSelectedItem_Click;

            var miSystemForms = new MenuItem()
            {
                Header = "Forms",
            };
            miSystemForms.Click += miSystemForms_Click;

            var miSystemFormsSelectedItem = new MenuItem()
            {
                Header = "Forms in Addition",
            };
            miSystemFormsSelectedItem.Click += miSystemFormsSelectedItem_Click;

            var miSavedQuery = new MenuItem()
            {
                Header = "Views",
            };
            miSavedQuery.Click += miSavedQuery_Click;

            var miSavedChart = new MenuItem()
            {
                Header = "Charts",
            };
            miSavedChart.Click += miSavedChart_Click;

            var miExportApplicationRibbon = new MenuItem()
            {
                Header = "ApplicationRibbon",
            };
            miExportApplicationRibbon.Click += miExportApplicationRibbon_Click;

            var miSiteMaps = new MenuItem()
            {
                Header = "SiteMaps",
            };
            miSiteMaps.Click += miSiteMaps_Click;

            var miWebResources = new MenuItem()
            {
                Header = "WebResources",
            };
            miWebResources.Click += miWebResources_Click;

            var miReports = new MenuItem()
            {
                Header = "Reports",
            };
            miReports.Click += miReports_Click;

            var miWorkflows = new MenuItem()
            {
                Header = "Workflows",
            };
            miWorkflows.Click += miWorkflows_Click;

            var miPluginTypes = new MenuItem()
            {
                Header = "Plugin Types",
            };
            miPluginTypes.Click += miPluginTypes_Click;

            var miPluginAssemblies = new MenuItem()
            {
                Header = "Plugin Assemblies",
            };
            miPluginAssemblies.Click += miPluginAssemblies_Click;

            var miPluginTree = new MenuItem()
            {
                Header = "Plugin Tree",
            };
            miPluginTree.Click += miPluginTree_Click;

            var miMessageProcessingStepExplorer = new MenuItem()
            {
                Header = "Plugin Steps Explorer",
            };
            miMessageProcessingStepExplorer.Click += miMessageProcessingStepExplorer_Click;

            var miMessageExplorer = new MenuItem()
            {
                Header = "Message Explorer",
            };
            miMessageExplorer.Click += miMessageExplorer_Click;

            var miMessageFilterExplorer = new MenuItem()
            {
                Header = "Message Filter Explorer",
            };
            miMessageFilterExplorer.Click += miMessageFilterExplorer_Click;

            var miMessageFilterTree = new MenuItem()
            {
                Header = "Message Filter Tree",
            };
            miMessageFilterTree.Click += miMessageFilterTree_Click;

            var miMessageRequestTree = new MenuItem()
            {
                Header = "Message Request Tree",
            };
            miMessageRequestTree.Click += miMessageRequestTree_Click;

            var miMessageRequestTreeSelectedItem = new MenuItem()
            {
                Header = "Message Request Tree in Addition",
            };
            miMessageRequestTreeSelectedItem.Click += miMessageRequestTreeSelectedItem_Click;

            //<MenuItem Header="Entity Metadata" Click="miCreateMetadataFile_Click" />

            //<Separator/>
            //<MenuItem Header="Entity Attribute Explorer" Click="miEntityAttributeExplorer_Click"/>
            //<MenuItem Header="Entity Relationship One-To-Many Explorer" Click="miEntityRelationshipOneToManyExplorer_Click"/>
            //<MenuItem Header="Entity Relationship Many-To-Many Explorer" Click="miEntityRelationshipManyToManyExplorer_Click"/>
            //<MenuItem Header="Entity Key Explorer" Click="miEntityKeyExplorer_Click"/>
            //<Separator/>
            //<MenuItem Header="Entity Privileges Explorer" Click="miEntityPrivilegesExplorer_Click"/>
            //<Separator/>
            //<MenuItem Header="Other Privileges Explorer" Click="miOtherPrivilegesExplorer_Click"/>
            //<Separator/>
            //<MenuItem Header="SecurityRoles Explorer" Click="miSecurityRolesExplorer_Click"/>

            //<Separator/>
            //<MenuItem Header="Global OptionSets" Click="miGlobalOptionSets_Click"/>
            //<MenuItem Header="Global OptionSets in Addition Mode" Click="miGlobalOptionSetsSelectedItem_Click"/>
            //<Separator/>
            //<MenuItem Header="Forms" Click="miSystemForms_Click" />
            //<MenuItem Header="Forms in Addition Mode" Click="miSystemFormsSelectedItem_Click"/>
            //<MenuItem Header="Views" Click="miSavedQuery_Click" />
            //<MenuItem Header="Charts" Click="miSavedChart_Click" />
            //<Separator/>
            //<MenuItem Header="ApplicationRibbon" Click="miExportApplicationRibbon_Click"/>
            //<Separator/>
            //<MenuItem Header="SiteMap" Click="btnSiteMap_Click"/>
            //<Separator/>
            //<MenuItem Header="WebResources" Click="btnWebResources_Click"/>
            //<Separator/>
            //<MenuItem Header="Report" Click="btnExportReport_Click"/>
            //<Separator/>
            //<MenuItem Header="Workflows" Click="miWorkflows_Click" />
            //<Separator/>
            //<MenuItem Header="Plugin Type" Click="btnPluginType_Click"/>
            //<MenuItem Header="Plugin Assembly" Click="btnPluginAssembly_Click"/>
            //<Separator/>
            //<MenuItem Header="Plugin Tree" Click="miPluginTree_Click" />
            //<MenuItem Header="Message Explorer" Click="miMessageExplorer_Click" />
            //<MenuItem Header="Message Filter Tree" Click="miMessageFilterTree_Click"/>
            //<MenuItem Header="Message Request Tree" Click="miMessageRequestTree_Click"/>



            miExplorers.Items.Add(miEntityMetadataExplorer);
            if (_selectedItem != null)
            {
                miExplorers.Items.Add(miEntityMetadataExplorerSelectedItem);
            }

            miExplorers.Items.Add(new Separator());
            miExplorers.Items.Add(miEntityAttributeExplorer);
            miExplorers.Items.Add(miEntityRelationshipOneToManyExplorer);
            miExplorers.Items.Add(miEntityRelationshipManyToManyExplorer);
            miExplorers.Items.Add(miEntityKeyExplorer);

            miExplorers.Items.Add(new Separator());
            miExplorers.Items.Add(miEntityPrivilegesExplorer);

            miExplorers.Items.Add(new Separator());
            miExplorers.Items.Add(miOtherPrivilegesExplorer);

            miExplorers.Items.Add(new Separator());
            miExplorers.Items.Add(miSecurityRolesExplorer);

            miExplorers.Items.Add(new Separator());
            miExplorers.Items.Add(miGlobalOptionSets);
            if (_selectedItem != null)
            {
                miExplorers.Items.Add(miGlobalOptionSetsSelectedItem);
            }

            miExplorers.Items.Add(new Separator());
            miExplorers.Items.Add(miSystemForms);
            if (_selectedItem != null)
            {
                miExplorers.Items.Add(miSystemFormsSelectedItem);
            }

            miExplorers.Items.Add(miSavedQuery);
            miExplorers.Items.Add(miSavedChart);

            miExplorers.Items.Add(new Separator());
            miExplorers.Items.Add(miExportApplicationRibbon);

            miExplorers.Items.Add(new Separator());
            miExplorers.Items.Add(miSiteMaps);

            miExplorers.Items.Add(new Separator());
            miExplorers.Items.Add(miWebResources);

            miExplorers.Items.Add(new Separator());
            miExplorers.Items.Add(miReports);

            miExplorers.Items.Add(new Separator());
            miExplorers.Items.Add(miWorkflows);

            miExplorers.Items.Add(new Separator());
            miExplorers.Items.Add(miPluginTypes);
            miExplorers.Items.Add(miPluginAssemblies);

            miExplorers.Items.Add(new Separator());
            miExplorers.Items.Add(miMessageProcessingStepExplorer);
            miExplorers.Items.Add(miMessageExplorer);
            miExplorers.Items.Add(miMessageFilterExplorer);

            miExplorers.Items.Add(new Separator());
            miExplorers.Items.Add(miPluginTree);
            miExplorers.Items.Add(miMessageFilterTree);
            miExplorers.Items.Add(miMessageRequestTree);

            if (_selectedItem != null)
            {
                miExplorers.Items.Add(miMessageRequestTreeSelectedItem);
            }
        }

        #region Кнопки открытия других форм с информация о сущности.

        public async void miEntityMetadataExplorer_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var entityName = GetEntityName();

            IEnumerable<EntityMetadata> entityMetadataList = GetEntityMetadataList(service.ConnectionData.ConnectionId);

            WindowHelper.OpenEntityMetadataExplorer(this._iWriteToOutput, service, _commonConfig, entityMetadataList, entityName);
        }

        public async void MiEntityMetadataExplorerSelectedItem_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedItem == null)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var entityName = GetEntityName();

            IEnumerable<EntityMetadata> entityMetadataList = GetEntityMetadataList(service.ConnectionData.ConnectionId);

            WindowHelper.OpenEntityMetadataExplorer(
                this._iWriteToOutput
                , service
                , _commonConfig
                , entityMetadataList
                , entityName
                , _selectedItem
            );
        }

        private async void miEntityAttributeExplorer_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var entityName = GetEntityName();

            _commonConfig.Save();

            WindowHelper.OpenEntityAttributeExplorer(this._iWriteToOutput, service, _commonConfig, entityName);
        }

        private async void miEntityRelationshipOneToManyExplorer_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var entityName = GetEntityName();

            _commonConfig.Save();

            WindowHelper.OpenEntityRelationshipOneToManyExplorer(this._iWriteToOutput, service, _commonConfig, entityName);
        }

        private async void miEntityRelationshipManyToManyExplorer_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var entityName = GetEntityName();

            _commonConfig.Save();

            WindowHelper.OpenEntityRelationshipManyToManyExplorer(this._iWriteToOutput, service, _commonConfig, entityName);
        }

        private async void miEntityKeyExplorer_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var entityName = GetEntityName();

            _commonConfig.Save();

            WindowHelper.OpenEntityKeyExplorer(this._iWriteToOutput, service, _commonConfig, entityName);
        }

        public async void miEntityPrivilegesExplorer_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            IEnumerable<EntityMetadata> entityMetadataList = GetEntityMetadataList(service.ConnectionData.ConnectionId);

            var entityName = GetEntityName();

            _commonConfig.Save();

            WindowHelper.OpenEntityPrivilegesExplorer(this._iWriteToOutput, service, _commonConfig, entityName, entityMetadataList);
        }

        public async void miOtherPrivilegesExplorer_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            IEnumerable<Privilege> privilegesList = GetOtherPrivilegesList(service.ConnectionData.ConnectionId);

            var otherPrivilegeName = GetOtherPrivilegeName();

            _commonConfig.Save();

            WindowHelper.OpenOtherPrivilegesExplorer(this._iWriteToOutput, service, _commonConfig, otherPrivilegeName, privilegesList);
        }

        private async void miSecurityRolesExplorer_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            IEnumerable<EntityMetadata> entityMetadataList = GetEntityMetadataList(service.ConnectionData.ConnectionId);

            IEnumerable<Privilege> privilegesList = GetOtherPrivilegesList(service.ConnectionData.ConnectionId);

            _commonConfig.Save();

            WindowHelper.OpenRolesExplorer(this._iWriteToOutput, service, _commonConfig, null, entityMetadataList, privilegesList);
        }

        private async void miGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var optionSetMetadataList = GetGlobalOptionSetMetadataList(service.ConnectionData.ConnectionId);

            var entityName = GetEntityName();
            var optionSetName = GetGlobalOptionSetName();

            _commonConfig.Save();

            WindowHelper.OpenGlobalOptionSetsExplorer(
                this._iWriteToOutput
                , service
                , _commonConfig
                , optionSetMetadataList
                , entityName
                , optionSetName
                , null
                , false
                , null
            );
        }

        private async void miGlobalOptionSetsSelectedItem_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedItem == null)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var optionSetMetadataList = GetGlobalOptionSetMetadataList(service.ConnectionData.ConnectionId);

            var entityName = GetEntityName();
            var optionSetName = GetGlobalOptionSetName();

            _commonConfig.Save();

            WindowHelper.OpenGlobalOptionSetsExplorer(
                this._iWriteToOutput
                , service
                , _commonConfig
                , optionSetMetadataList
                , entityName
                , optionSetName
                , string.Empty
                , false
                , _selectedItem
            );
        }

        private async void miExportApplicationRibbon_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            _commonConfig.Save();

            WindowHelper.OpenApplicationRibbonExplorer(this._iWriteToOutput, service, _commonConfig);
        }

        private async void miSiteMaps_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var siteMapName = GetSiteMapName();

            _commonConfig.Save();

            WindowHelper.OpenExportSiteMapExplorer(this._iWriteToOutput, service, _commonConfig, siteMapName);
        }

        private async void miWebResources_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var webResourceName = GetWebResourceName();

            _commonConfig.Save();

            WindowHelper.OpenWebResourceExplorer(this._iWriteToOutput, service, _commonConfig, webResourceName);
        }

        private async void miReports_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var reportName = GetReportName();

            _commonConfig.Save();

            WindowHelper.OpenReportExplorer(this._iWriteToOutput, service, _commonConfig, reportName);
        }

        private async void miSystemForms_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var entityName = GetEntityName();
            var systemFormName = GetSystemFormName();

            _commonConfig.Save();

            WindowHelper.OpenSystemFormExplorer(this._iWriteToOutput, service, _commonConfig, entityName, systemFormName);
        }

        private async void miSystemFormsSelectedItem_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedItem == null)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var entityName = GetEntityName();
            var systemFormName = GetSystemFormName();

            _commonConfig.Save();

            WindowHelper.OpenSystemFormExplorer(this._iWriteToOutput, service, _commonConfig, entityName, systemFormName, _selectedItem);
        }

        private async void miSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var entityName = GetEntityName();
            var savedQueryName = GetSavedQueryName();

            _commonConfig.Save();

            WindowHelper.OpenSavedQueryExplorer(this._iWriteToOutput, service, _commonConfig, entityName, savedQueryName);
        }

        private async void miSavedChart_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var entityName = GetEntityName();
            var chartName = GetSavedQueryVisualizationName();

            _commonConfig.Save();

            WindowHelper.OpenSavedQueryVisualizationExplorer(this._iWriteToOutput, service, _commonConfig, entityName, chartName);
        }

        private async void miWorkflows_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var entityName = GetEntityName();
            var workflowName = GetWorkflowName();

            _commonConfig.Save();

            WindowHelper.OpenWorkflowExplorer(this._iWriteToOutput, service, _commonConfig, entityName, workflowName);
        }

        public async void miPluginAssemblies_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var pluginAssemblyName = GetPluginAssemblyName();

            _commonConfig.Save();

            WindowHelper.OpenPluginAssemblyExplorer(this._iWriteToOutput, service, _commonConfig, pluginAssemblyName);
        }

        public async void miPluginTypes_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var pluginTypeName = GetPluginTypeName();

            _commonConfig.Save();

            WindowHelper.OpenPluginTypeExplorer(this._iWriteToOutput, service, _commonConfig, pluginTypeName);
        }

        public async void miPluginTree_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var entityName = GetEntityName();
            var pluginTypeName = GetPluginTypeName();
            var messageName = GetMessageName();

            _commonConfig.Save();

            WindowHelper.OpenPluginTree(this._iWriteToOutput, service, _commonConfig, entityName, pluginTypeName, messageName);
        }

        public async void miMessageProcessingStepExplorer_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var entityName = GetEntityName();
            var pluginTypeName = GetPluginTypeName();
            var messageName = GetMessageName();

            _commonConfig.Save();

            WindowHelper.OpenSdkMessageProcessingStepExplorer(this._iWriteToOutput, service, _commonConfig, entityName, pluginTypeName, messageName);
        }

        public async void miMessageExplorer_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var messageName = GetMessageName();

            _commonConfig.Save();

            WindowHelper.OpenSdkMessageExplorer(this._iWriteToOutput, service, _commonConfig, messageName);
        }

        public async void miMessageFilterExplorer_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var entityName = GetEntityName();
            var messageName = GetMessageName();

            _commonConfig.Save();

            WindowHelper.OpenSdkMessageFilterExplorer(this._iWriteToOutput, service, _commonConfig, entityName, messageName);
        }

        public async void miMessageFilterTree_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var entityName = GetEntityName();
            var messageName = GetMessageName();

            _commonConfig.Save();

            WindowHelper.OpenSdkMessageFilterTree(this._iWriteToOutput, service, _commonConfig, entityName, messageName);
        }

        public async void miMessageRequestTree_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var entityName = GetEntityName();
            var messageName = GetMessageName();

            _commonConfig.Save();

            WindowHelper.OpenSdkMessageRequestTree(this._iWriteToOutput, service, _commonConfig, entityName, messageName);
        }

        public async void miMessageRequestTreeSelectedItem_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedItem == null)
            {
                return;
            }

            var service = await GetService();

            if (service == null)
            {
                return;
            }

            var entityName = GetEntityName();
            var messageName = GetMessageName();

            _commonConfig.Save();

            WindowHelper.OpenSdkMessageRequestTree(this._iWriteToOutput, service, _commonConfig, string.Empty, false, _selectedItem, entityName, messageName);
        }

        #endregion Кнопки открытия других форм с информация о сущности.
    }
}
