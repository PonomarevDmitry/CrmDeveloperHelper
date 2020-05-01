using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public class ExplorersHelper
    {
        private readonly IWriteToOutput _iWriteToOutput;
        private readonly CommonConfiguration _commonConfig;

        private readonly Func<Task<IOrganizationServiceExtented>> GetService;

        private readonly Func<string> _getEntityName;
        private readonly Func<string> _getGlobalOptionSetName;
        private readonly Func<string> _getWorkflowName;
        private readonly Func<string> _getSystemFormName;
        private readonly Func<string> _getSavedQueryName;
        private readonly Func<string> _getSavedQueryVisualizationName;
        private readonly Func<string> _getOtherPrivilegeName;
        private readonly Func<string> _getSiteMapName;
        private readonly Func<string> _getReportName;
        private readonly Func<string> _getWebResourceName;

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
            , Func<Guid, IEnumerable<EntityMetadata>> getEntityMetadataList = null
            , Func<Guid, IEnumerable<Privilege>> getOtherPrivilegesList = null
            , Func<Guid, IEnumerable<OptionSetMetadata>> getGlobalOptionSetMetadataList = null
            , Func<string> getGlobalOptionSetName = null
            , Func<string> getWorkflowName = null
            , Func<string> getSystemFormName = null
            , Func<string> getSavedQueryName = null
            , Func<string> getSavedQueryVisualizationName = null
            , Func<string> getOtherPrivilegeName = null
            , Func<string> getSiteMapName = null
            , Func<string> getReportName = null
            , Func<string> getWebResourceName = null
        ) : this(iWriteToOutput, commonConfig, getService
            , getEntityName
            , getEntityMetadataList
            , getOtherPrivilegesList
            , getGlobalOptionSetMetadataList
            , getGlobalOptionSetName
            , getWorkflowName
            , getSystemFormName
            , getSavedQueryName
            , getSavedQueryVisualizationName
            , getOtherPrivilegeName
            , getSiteMapName
            , getReportName
            , getWebResourceName
        )
        {
            this._selectedItem = selectedItem;
        }

        public ExplorersHelper(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , Func<Task<IOrganizationServiceExtented>> getService
            , Func<string> getEntityName = null
            , Func<Guid, IEnumerable<EntityMetadata>> getEntityMetadataList = null
            , Func<Guid, IEnumerable<Privilege>> getOtherPrivilegesList = null
            , Func<Guid, IEnumerable<OptionSetMetadata>> getGlobalOptionSetMetadataList = null
            , Func<string> getGlobalOptionSetName = null
            , Func<string> getWorkflowName = null
            , Func<string> getSystemFormName = null
            , Func<string> getSavedQueryName = null
            , Func<string> getSavedQueryVisualizationName = null
            , Func<string> getOtherPrivilegeName = null
            , Func<string> getSiteMapName = null
            , Func<string> getReportName = null
            , Func<string> getWebResourceName = null
        )
        {
            this._iWriteToOutput = iWriteToOutput;
            this._commonConfig = commonConfig;
            this.GetService = getService;

            this._getEntityMetadataList = getEntityMetadataList;
            this._getOtherPrivilegesList = getOtherPrivilegesList;
            this._getGlobalOptionSetMetadataList = getGlobalOptionSetMetadataList;

            this._getEntityName = getEntityName;
            this._getGlobalOptionSetName = getGlobalOptionSetName;
            this._getWorkflowName = getWorkflowName;
            this._getSystemFormName = getSystemFormName;
            this._getSavedQueryName = getSavedQueryName;
            this._getSavedQueryVisualizationName = getSavedQueryVisualizationName;
            this._getOtherPrivilegeName = getOtherPrivilegeName;
            this._getSiteMapName = getSiteMapName;
            this._getReportName = getReportName;
            this._getWebResourceName = getWebResourceName;
        }

        private string GetEntityName()
        {
            if (_getEntityName != null)
            {
                return _getEntityName();
            }

            return string.Empty;
        }

        private string GetGlobalOptionSetName()
        {
            if (_getGlobalOptionSetName != null)
            {
                return _getGlobalOptionSetName();
            }

            return string.Empty;
        }

        private string GetWorkflowName()
        {
            if (_getWorkflowName != null)
            {
                return _getWorkflowName();
            }

            return string.Empty;
        }

        private string GetSystemFormName()
        {
            if (_getSystemFormName != null)
            {
                return _getSystemFormName();
            }

            return string.Empty;
        }

        private string GetSavedQueryName()
        {
            if (_getSavedQueryName != null)
            {
                return _getSavedQueryName();
            }

            return string.Empty;
        }

        private string GetSavedQueryVisualizationName()
        {
            if (_getSavedQueryVisualizationName != null)
            {
                return _getSavedQueryVisualizationName();
            }

            return string.Empty;
        }

        private string GetOtherPrivilegeName()
        {
            if (_getOtherPrivilegeName != null)
            {
                return _getOtherPrivilegeName();
            }

            return string.Empty;
        }

        private string GetSiteMapName()
        {
            if (_getSiteMapName != null)
            {
                return _getSiteMapName();
            }

            return string.Empty;
        }

        private string GetReportName()
        {
            if (_getReportName != null)
            {
                return _getReportName();
            }

            return string.Empty;
        }

        private string GetWebResourceName()
        {
            if (_getWebResourceName != null)
            {
                return _getWebResourceName();
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

        public void FillExplorers(MenuItem miExplorers)
        {
            var miCreateMetadataFile = new MenuItem()
            {
                Header = "Entity Metadata",
            };
            miCreateMetadataFile.Click += miCreateMetadataFile_Click;


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

            var miMessageExplorer = new MenuItem()
            {
                Header = "Message Explorer",
            };
            miMessageExplorer.Click += miMessageExplorer_Click;

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



            miExplorers.Items.Add(miCreateMetadataFile);

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
            miExplorers.Items.Add(miPluginTree);
            miExplorers.Items.Add(miMessageExplorer);
            miExplorers.Items.Add(miMessageFilterTree);
            miExplorers.Items.Add(miMessageRequestTree);

            if (_selectedItem != null)
            {
                miExplorers.Items.Add(miMessageRequestTreeSelectedItem);
            }
        }

        #region Кнопки открытия других форм с информация о сущности.

        private async void miCreateMetadataFile_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();

            var service = await GetService();

            IEnumerable<EntityMetadata> entityMetadataList = GetEntityMetadataList(service.ConnectionData.ConnectionId);

            WindowHelper.OpenEntityMetadataExplorer(this._iWriteToOutput, service, _commonConfig, entityMetadataList, entityName);
        }

        private async void miEntityAttributeExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityAttributeExplorer(this._iWriteToOutput, service, _commonConfig, entityName);
        }

        private async void miEntityRelationshipOneToManyExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityRelationshipOneToManyExplorer(this._iWriteToOutput, service, _commonConfig, entityName);
        }

        private async void miEntityRelationshipManyToManyExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityRelationshipManyToManyExplorer(this._iWriteToOutput, service, _commonConfig, entityName);
        }

        private async void miEntityKeyExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenEntityKeyExplorer(this._iWriteToOutput, service, _commonConfig, entityName);
        }

        public async void miEntityPrivilegesExplorer_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();

            _commonConfig.Save();

            var service = await GetService();

            IEnumerable<EntityMetadata> entityMetadataList = GetEntityMetadataList(service.ConnectionData.ConnectionId);

            WindowHelper.OpenEntityPrivilegesExplorer(this._iWriteToOutput, service, _commonConfig, entityName, entityMetadataList);
        }

        private async void miOtherPrivilegesExplorer_Click(object sender, RoutedEventArgs e)
        {
            var otherPrivilegeName = GetOtherPrivilegeName();

            _commonConfig.Save();

            var service = await GetService();

            IEnumerable<Privilege> privilegesList = GetOtherPrivilegesList(service.ConnectionData.ConnectionId);

            WindowHelper.OpenOtherPrivilegesExplorer(this._iWriteToOutput, service, _commonConfig, otherPrivilegeName, privilegesList);
        }

        private async void miSecurityRolesExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            IEnumerable<EntityMetadata> entityMetadataList = GetEntityMetadataList(service.ConnectionData.ConnectionId);

            IEnumerable<Privilege> privilegesList = GetOtherPrivilegesList(service.ConnectionData.ConnectionId);

            WindowHelper.OpenRolesExplorer(this._iWriteToOutput, service, _commonConfig, null, entityMetadataList, privilegesList);
        }

        private async void miGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();
            var optionSetName = GetGlobalOptionSetName();

            var service = await GetService();

            var optionSetMetadataList = GetGlobalOptionSetMetadataList(service.ConnectionData.ConnectionId);

            _commonConfig.Save();

            WindowHelper.OpenGlobalOptionSetsExplorer(
                this._iWriteToOutput
                , service
                , _commonConfig
                , optionSetMetadataList
                , optionSetName
                , entityName
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

            var entityName = GetEntityName();
            var optionSetName = GetGlobalOptionSetName();

            var service = await GetService();

            var optionSetMetadataList = GetGlobalOptionSetMetadataList(service.ConnectionData.ConnectionId);

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
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenApplicationRibbonExplorer(this._iWriteToOutput, service, _commonConfig);
        }

        private async void miSiteMaps_Click(object sender, RoutedEventArgs e)
        {
            var siteMapName = GetSiteMapName();

            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenExportSiteMapExplorer(this._iWriteToOutput, service, _commonConfig, siteMapName);
        }

        private async void miWebResources_Click(object sender, RoutedEventArgs e)
        {
            var webResourceName = GetWebResourceName();

            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenWebResourceExplorer(this._iWriteToOutput, service, _commonConfig, webResourceName);
        }

        private async void miReports_Click(object sender, RoutedEventArgs e)
        {
            var reportName = GetReportName();

            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenReportExplorer(this._iWriteToOutput, service, _commonConfig, reportName);
        }

        private async void miSystemForms_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();
            var systemFormName = GetSystemFormName();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSystemFormExplorer(this._iWriteToOutput, service, _commonConfig, entityName, systemFormName);
        }

        private async void miSystemFormsSelectedItem_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedItem == null)
            {
                return;
            }

            var entityName = GetEntityName();
            var systemFormName = GetSystemFormName();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSystemFormExplorer(this._iWriteToOutput, service, _commonConfig, entityName, systemFormName, _selectedItem);
        }

        private async void miSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();
            var savedQueryName = GetSavedQueryName();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSavedQueryExplorer(this._iWriteToOutput, service, _commonConfig, entityName, savedQueryName);
        }

        private async void miSavedChart_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();
            var chartName = GetSavedQueryVisualizationName();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSavedQueryVisualizationExplorer(this._iWriteToOutput, service, _commonConfig, entityName, chartName);
        }

        private async void miWorkflows_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();
            var workflowName = GetWorkflowName();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenWorkflowExplorer(this._iWriteToOutput, service, _commonConfig, entityName, workflowName);
        }

        private async void miPluginAssemblies_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenPluginAssemblyExplorer(this._iWriteToOutput, service, _commonConfig, null);
        }

        private async void miPluginTypes_Click(object sender, RoutedEventArgs e)
        {
            var service = await GetService();

            _commonConfig.Save();

            WindowHelper.OpenPluginTypeExplorer(this._iWriteToOutput, service, _commonConfig, null);
        }

        private async void miPluginTree_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenPluginTree(this._iWriteToOutput, service, _commonConfig, entityName, string.Empty, string.Empty);
        }

        private async void miMessageExplorer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageExplorer(this._iWriteToOutput, service, _commonConfig, string.Empty);
        }

        private async void miMessageFilterTree_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageFilterTree(this._iWriteToOutput, service, _commonConfig, entityName, string.Empty);
        }

        private async void miMessageRequestTree_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageRequestTree(this._iWriteToOutput, service, _commonConfig, entityName);
        }

        private async void miMessageRequestTreeSelectedItem_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedItem == null)
            {
                return;
            }

            var entityName = GetEntityName();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenSdkMessageRequestTree(this._iWriteToOutput, service, _commonConfig, string.Empty, false, _selectedItem, entityName, string.Empty);
        }

        #endregion Кнопки открытия других форм с информация о сущности.

        public void FillCompareWindows(MenuItem miCompareOrganizations)
        {
            var miOrganizationComparer = new MenuItem()
            {
                Header = "Organization Comparer",
            };
            miOrganizationComparer.Click += miOrganizationComparer_Click;

            var miCompareMetadataFile = new MenuItem()
            {
                Header = "Entity Metadata",
            };
            miCompareMetadataFile.Click += miCompareMetadataFile_Click;

            var miCompareGlobalOptionSets = new MenuItem()
            {
                Header = "Global OptionSets",
            };
            miCompareGlobalOptionSets.Click += miCompareGlobalOptionSets_Click;

            var miCompareApplicationRibbon = new MenuItem()
            {
                Header = "ApplicationRibbon",
            };
            miCompareApplicationRibbon.Click += miCompareApplicationRibbon_Click;

            var miCompareSystemForms = new MenuItem()
            {
                Header = "Forms",
            };
            miCompareSystemForms.Click += miCompareSystemForms_Click;

            var miCompareSavedQuery = new MenuItem()
            {
                Header = "Views",
            };
            miCompareSavedQuery.Click += miCompareSavedQuery_Click;

            var miCompareSavedChart = new MenuItem()
            {
                Header = "Charts",
            };
            miCompareSavedChart.Click += miCompareSavedChart_Click;

            var miCompareWorkflows = new MenuItem()
            {
                Header = "Workflows",
            };
            miCompareWorkflows.Click += miCompareWorkflows_Click;

            //<MenuItem Header="Organization Comparer" Click="miOrganizationComparer_Click"/>
            //<Separator/>
            //<MenuItem Header="Entity Metadata" Click="miCompareMetadataFile_Click"/>
            //<Separator/>
            //<MenuItem Header="Global OptionSets" Click="miCompareGlobalOptionSets_Click"/>
            //<Separator/>
            //<MenuItem Header="ApplicationRibbon" Click="miCompareApplicationRibbon_Click"/>
            //<Separator/>
            //<MenuItem Header="Forms" Click="miCompareSystemForms_Click"/>
            //<MenuItem Header="Views" Click="miCompareSavedQuery_Click"/>
            //<MenuItem Header="Charts" Click="miCompareSavedChart_Click"/>
            //<Separator/>
            //<MenuItem Header="Workflows" Click="miCompareWorkflows_Click"/>

            miCompareOrganizations.Items.Add(miOrganizationComparer);

            miCompareOrganizations.Items.Add(new Separator());
            miCompareOrganizations.Items.Add(miCompareMetadataFile);

            miCompareOrganizations.Items.Add(new Separator());
            miCompareOrganizations.Items.Add(miCompareGlobalOptionSets);

            miCompareOrganizations.Items.Add(new Separator());
            miCompareOrganizations.Items.Add(miCompareApplicationRibbon);

            miCompareOrganizations.Items.Add(new Separator());
            miCompareOrganizations.Items.Add(miCompareSystemForms);
            miCompareOrganizations.Items.Add(miCompareSavedQuery);
            miCompareOrganizations.Items.Add(miCompareSavedChart);

            miCompareOrganizations.Items.Add(new Separator());
            miCompareOrganizations.Items.Add(miCompareWorkflows);
        }

        private async void miOrganizationComparer_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerWindow(this._iWriteToOutput, service.ConnectionData.ConnectionConfiguration, _commonConfig);
        }

        private async void miCompareMetadataFile_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerEntityMetadataWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entityName);
        }

        private async void miCompareApplicationRibbon_Click(object sender, RoutedEventArgs e)
        {
            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerApplicationRibbonWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData);
        }

        private async void miCompareGlobalOptionSets_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();
            var optionSetName = GetGlobalOptionSetName();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerGlobalOptionSetsWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, optionSetName, entityName);
        }

        private async void miCompareSystemForms_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();
            var systemFormName = GetSystemFormName();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSystemFormWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entityName, systemFormName);
        }

        private async void miCompareSavedQuery_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();
            var savedQueryName = GetSavedQueryName();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSavedQueryWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entityName, savedQueryName);
        }

        private async void miCompareSavedChart_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();
            var chartName = GetSavedQueryVisualizationName();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerSavedQueryVisualizationWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entityName, chartName);
        }

        private async void miCompareWorkflows_Click(object sender, RoutedEventArgs e)
        {
            var entityName = GetEntityName();
            var workflowName = GetWorkflowName();

            _commonConfig.Save();

            var service = await GetService();

            WindowHelper.OpenOrganizationComparerWorkflowWindow(this._iWriteToOutput, _commonConfig, service.ConnectionData, service.ConnectionData, entityName, workflowName);
        }
    }
}
