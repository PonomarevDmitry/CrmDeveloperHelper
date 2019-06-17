using Nav.Common.VSPackages.CrmDeveloperHelper.Controllers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Threading;
using System.Windows.Controls;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public static class WindowCrmConnectionHandler
    {
        internal static void InitializeConnectionMenuItems(
            MenuItem miConnection
            , IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , Func<ConnectionData> getSelectedSingleConnection
        )
        {
            #region Check

            {
                //<Separator/>
                //<MenuItem Header="Workflows used Entities" Click="miCheckWorkflowsUsedEntities_Click"/>
                //<MenuItem Header="Workflows used Not Exists Entities" Click="miCheckWorkflowsUsedNotExistsEntities_Click"/>
                //<Separator/>
                //<MenuItem Header="Entities Ownership" Click="miCheckEntitiesOwnership_Click"/>
                //<Separator/>
                //<MenuItem Header="Global OptionSet Duplicates on Entity" Click="miCheckGlobalOptionSetDuplicatesOnEntity_Click"/>
                //<MenuItem Header="Solution ComponentType Enum" Click="miSolutionComponentTypeEnum_Click"/>
                //<MenuItem Header="Create All Dependency Nodes Description" Click="miCreateAllDependencyNodesDescription_Click"/>
                //<Separator/>
                //<MenuItem Header="Check Managed Entities in CRM" Click="miCheckManagedEntitiesInCRM_Click"/>
                //<Separator/>
                //<MenuItem Header="Plugin Steps Duplicates" Click="miCheckPluginStepsDuplicates_Click"/>
                //<MenuItem Header="Plugin Images Duplicates" Click="miCheckPluginImagesDuplicates_Click"/>
                //<Separator/>
                //<MenuItem Header="Plugin Steps Required Components" Click="miCheckPluginStepsRequiredComponents_Click"/>
                //<MenuItem Header="Plugin Images Required Components" Click="miCheckPluginImagesRequiredComponents_Click"/>

                var menuItemCheck = new MenuItem()
                {
                    Header = "Check",
                };

                {
                    var menuItemCheckWorkflowsUsedEntities = new MenuItem()
                    {
                        Header = "Workflows used Entities",
                    };
                    menuItemCheckWorkflowsUsedEntities.Click += (s, e) => miCheckWorkflowsUsedEntities_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemCheckWorkflowsUsedNotExistsEntities = new MenuItem()
                    {
                        Header = "Workflows used Not Exists Entities",
                    };
                    menuItemCheckWorkflowsUsedNotExistsEntities.Click += (s, e) => miCheckWorkflowsUsedNotExistsEntities_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemCheckEntitiesOwnership = new MenuItem()
                    {
                        Header = "Entities Ownership",
                    };
                    menuItemCheckEntitiesOwnership.Click += (s, e) => miCheckEntitiesOwnership_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemCheckGlobalOptionSetDuplicatesOnEntity = new MenuItem()
                    {
                        Header = "Global OptionSet Duplicates on Entity",
                    };
                    menuItemCheckGlobalOptionSetDuplicatesOnEntity.Click += (s, e) => miCheckGlobalOptionSetDuplicatesOnEntity_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemSolutionComponentTypeEnum = new MenuItem()
                    {
                        Header = "Solution ComponentType Enum",
                    };
                    menuItemSolutionComponentTypeEnum.Click += (s, e) => miSolutionComponentTypeEnum_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemCreateAllDependencyNodesDescription = new MenuItem()
                    {
                        Header = "Create All Dependency Nodes Description",
                    };
                    menuItemCreateAllDependencyNodesDescription.Click += (s, e) => miCreateAllDependencyNodesDescription_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemCheckManagedEntitiesInCRM = new MenuItem()
                    {
                        Header = "Check Managed Entities in CRM",
                    };
                    menuItemCheckManagedEntitiesInCRM.Click += (s, e) => miCheckManagedEntitiesInCRM_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);



                    var menuItemCheckPluginStepsDuplicates = new MenuItem()
                    {
                        Header = "Plugin Steps Duplicates",
                    };
                    menuItemCheckPluginStepsDuplicates.Click += (s, e) => miCheckPluginStepsDuplicates_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemCheckPluginImagesDuplicates = new MenuItem()
                    {
                        Header = "Plugin Images Duplicates",
                    };
                    menuItemCheckPluginImagesDuplicates.Click += (s, e) => miCheckPluginImagesDuplicates_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemCheckPluginStepsRequiredComponents = new MenuItem()
                    {
                        Header = "Plugin Steps Required Components",
                    };
                    menuItemCheckPluginStepsRequiredComponents.Click += (s, e) => miCheckPluginStepsRequiredComponents_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemCheckPluginImagesRequiredComponents = new MenuItem()
                    {
                        Header = "Plugin Images Required Components",
                    };
                    menuItemCheckPluginImagesRequiredComponents.Click += (s, e) => miCheckPluginImagesRequiredComponents_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    menuItemCheck.Items.Add(menuItemCheckWorkflowsUsedEntities);
                    menuItemCheck.Items.Add(menuItemCheckWorkflowsUsedNotExistsEntities);
                    menuItemCheck.Items.Add(new Separator());
                    menuItemCheck.Items.Add(menuItemCheckEntitiesOwnership);
                    menuItemCheck.Items.Add(new Separator());
                    menuItemCheck.Items.Add(menuItemCheckGlobalOptionSetDuplicatesOnEntity);
                    menuItemCheck.Items.Add(menuItemSolutionComponentTypeEnum);
                    menuItemCheck.Items.Add(menuItemCreateAllDependencyNodesDescription);
                    menuItemCheck.Items.Add(new Separator());
                    menuItemCheck.Items.Add(menuItemCheckManagedEntitiesInCRM);
                    menuItemCheck.Items.Add(new Separator());
                    menuItemCheck.Items.Add(menuItemCheckPluginStepsDuplicates);
                    menuItemCheck.Items.Add(menuItemCheckPluginImagesDuplicates);
                    menuItemCheck.Items.Add(new Separator());
                    menuItemCheck.Items.Add(menuItemCheckPluginStepsRequiredComponents);
                    menuItemCheck.Items.Add(menuItemCheckPluginImagesRequiredComponents);
                }

                miConnection.Items.Add(menuItemCheck);
            }

            #endregion Check

            #region Find

            {
                //<MenuItem Header="CRM Objects names for prefix" Click="miCheckObjectsNamesForPrefix_Click"/>
                //<MenuItem Header="CRM Objects names for prefix and show dependent components" Click="miCheckObjectsNamesForPrefixAndShowDependentComponents_Click"/>
                //<MenuItem Header="CRM Objects marked to delete and show dependent components" Click="miCheckObjectsMarkedToDeleteAndShowDependentComponents_Click"/>
                //<Separator/>
                //<MenuItem Header="Find Entity Objects by name" Click="miFindEntityObjectsByName_Click"/>
                //<MenuItem Header="Find Entity Objects contains string" Click="miFindEntityObjectsContainsString_Click"/>
                //<Separator/>
                //<MenuItem Header="Find Entity Objects by Id" Click="miFindEntityObjectsById_Click"/>
                //<MenuItem Header="Find Entity Objects by Uniqueidentifier" Click="miFindEntityObjectsByUniqueidentifier_Click"/>
                //<Separator/>
                //<MenuItem Header="Edit Entity Objects by Id" Click="miFindEntityObjectsById_Click"/>

                var menuItemFind = new MenuItem()
                {
                    Header = "Find",
                };

                {
                    var menuItemFindObjectsNamesForPrefix = new MenuItem()
                    {
                        Header = "Find CRM Objects names for prefix",
                    };
                    menuItemFindObjectsNamesForPrefix.Click += (s, e) => miFindObjectsNamesForPrefix_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemFindObjectsNamesForPrefixAndShowDependentComponents = new MenuItem()
                    {
                        Header = "Find CRM Objects names for prefix and show dependent components",
                    };
                    menuItemFindObjectsNamesForPrefixAndShowDependentComponents.Click += (s, e) => miFindObjectsNamesForPrefixAndShowDependentComponents_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemFindObjectsMarkedToDeleteAndShowDependentComponents = new MenuItem()
                    {
                        Header = "Find CRM Objects marked to delete and show dependent components",
                    };
                    menuItemFindObjectsMarkedToDeleteAndShowDependentComponents.Click += (s, e) => miFindObjectsMarkedToDeleteAndShowDependentComponents_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemFindEntityObjectsByName = new MenuItem()
                    {
                        Header = "Find Entity Objects by name",
                    };
                    menuItemFindEntityObjectsByName.Click += (s, e) => miFindEntityObjectsByName_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemFindEntityObjectsContainsString = new MenuItem()
                    {
                        Header = "Find Entity Objects contains string",
                    };
                    menuItemFindEntityObjectsContainsString.Click += (s, e) => miFindEntityObjectsContainsString_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemFindEntityObjectsById = new MenuItem()
                    {
                        Header = "Find Entity Objects by Id",
                    };
                    menuItemFindEntityObjectsById.Click += (s, e) => miFindEntityObjectsById_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemFindEntityObjectsByUniqueidentifier = new MenuItem()
                    {
                        Header = "Find Entity Objects by Uniqueidentifier",
                    };
                    menuItemFindEntityObjectsByUniqueidentifier.Click += (s, e) => miFindEntityObjectsByUniqueidentifier_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemEditEntityObjectsById = new MenuItem()
                    {
                        Header = "Edit Entity Objects by Id",
                    };
                    menuItemEditEntityObjectsById.Click += (s, e) => miEditEntityObjectsById_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);



                    menuItemFind.Items.Add(menuItemFindObjectsNamesForPrefix);
                    menuItemFind.Items.Add(menuItemFindObjectsNamesForPrefixAndShowDependentComponents);
                    menuItemFind.Items.Add(menuItemFindObjectsMarkedToDeleteAndShowDependentComponents);
                    menuItemFind.Items.Add(new Separator());
                    menuItemFind.Items.Add(menuItemFindEntityObjectsByName);
                    menuItemFind.Items.Add(menuItemFindEntityObjectsContainsString);
                    menuItemFind.Items.Add(new Separator());
                    menuItemFind.Items.Add(menuItemFindEntityObjectsById);
                    menuItemFind.Items.Add(menuItemFindEntityObjectsByUniqueidentifier);
                    menuItemFind.Items.Add(new Separator());
                    menuItemFind.Items.Add(menuItemEditEntityObjectsById);
                }

                miConnection.Items.Add(new Separator());
                miConnection.Items.Add(menuItemFind);
            }

            #endregion Find

            #region Plugin Configuration

            {
                //<Separator/>
                //<MenuItem Header="Plugin Configuration">
                //    <MenuItem Header="Create Plugin Configuration"  Click="miExplorerCreatePluginConfiguration_Click"/>
                //    <Separator/>
                //    <MenuItem Header="PluginType Description"  Click="miExplorerPluginConfigurationType_Click"/>
                //    <MenuItem Header="Plugin Assembly Description"  Click="miExplorerPluginConfigurationAssembly_Click"/>
                //    <Separator/>
                //    <MenuItem Header="Plugin Configuration Tree"  Click="miExplorerPluginConfigurationTree_Click"/>
                //    <Separator/>
                //    <MenuItem Header="Plugin Configuration Comparer"  Click="miExplorerPluginConfigurationComparer_Click"/>
                //</MenuItem>

                var menuItemPluginConfiguration = new MenuItem()
                {
                    Header = "Plugin Configuration",
                };

                {
                    var menuItemCreatePluginConfiguration = new MenuItem()
                    {
                        Header = "Create Plugin Configuration",
                    };
                    menuItemCreatePluginConfiguration.Click += (s, e) => miExplorerCreatePluginConfiguration_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemPluginTypeExplorer = new MenuItem()
                    {
                        Header = "PluginType Explorer",
                    };
                    menuItemPluginTypeExplorer.Click += (s, e) => miExplorerPluginConfigurationType_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemPluginAssemblyExplorer = new MenuItem()
                    {
                        Header = "Plugin Assembly Explorer",
                    };
                    menuItemPluginAssemblyExplorer.Click += (s, e) => miExplorerPluginConfigurationAssembly_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemPluginConfigurationTree = new MenuItem()
                    {
                        Header = "Plugin Configuration Tree",
                    };
                    menuItemPluginConfigurationTree.Click += (s, e) => miExplorerPluginConfigurationTree_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemPluginConfigurationComparer = new MenuItem()
                    {
                        Header = "Plugin Configuration Comparer",
                    };
                    menuItemPluginConfigurationComparer.Click += (s, e) => miExplorerPluginConfigurationComparer_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    menuItemPluginConfiguration.Items.Add(menuItemCreatePluginConfiguration);
                    menuItemPluginConfiguration.Items.Add(new Separator());
                    menuItemPluginConfiguration.Items.Add(menuItemPluginTypeExplorer);
                    menuItemPluginConfiguration.Items.Add(menuItemPluginAssemblyExplorer);
                    menuItemPluginConfiguration.Items.Add(new Separator());
                    menuItemPluginConfiguration.Items.Add(menuItemPluginConfigurationTree);
                    menuItemPluginConfiguration.Items.Add(new Separator());
                    menuItemPluginConfiguration.Items.Add(menuItemPluginConfigurationComparer);
                }

                miConnection.Items.Add(new Separator());
                miConnection.Items.Add(menuItemPluginConfiguration);
            }

            #endregion Plugin Configuration

            #region Entity Information

            {
                //<Separator/>
                //<MenuItem Header="Entity Metadata"  Click="miExplorerEntityMetadata_Click"/>

                //<MenuItem Header="Entity Information" >
                //    <MenuItem Header="Entity Attribute Explorer"  Click="miEntityAttributeExplorer_Click"/>
                //    <MenuItem Header="Entity Relationship One-To-Many Explorer" Click="miEntityRelationshipOneToManyExplorer_Click"/>
                //    <MenuItem Header="Entity Relationship Many-To-Many Explorer" Click="miEntityRelationshipManyToManyExplorer_Click"/>
                //    <MenuItem Header="Entity Key Explorer" Click="miEntityKeyExplorer_Click"/>
                //    <Separator/>
                //    <MenuItem Header="Entity Privileges Explorer" Click="miEntityPrivilegesExplorer_Click"/>
                //    <Separator/>
                //    <Separator/>
                //    <MenuItem Header="System Form"  Click="miExplorerSystemForms_Click"/>
                //    <MenuItem Header="System View"  Click="miExplorerSystemViews_Click"/>
                //    <MenuItem Header="System Chart"  Click="miExplorerSystemCharts_Click"/>
                //</MenuItem>

                var menuItemEntityMetadata = new MenuItem()
                {
                    Header = "Entity Metadata",
                };
                menuItemEntityMetadata.Click += (s, e) => miExplorerEntityMetadata_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                var menuItemEntityInformation = new MenuItem()
                {
                    Header = "Entity Information",
                };

                {
                    var menuItemEntityAttributeExplorer = new MenuItem()
                    {
                        Header = "Entity Attribute Explorer",
                    };
                    menuItemEntityAttributeExplorer.Click += (s, e) => miEntityAttributeExplorer_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemEntityRelationshipOneToManyExplorer = new MenuItem()
                    {
                        Header = "Entity Relationship One-To-Many Explorer",
                    };
                    menuItemEntityRelationshipOneToManyExplorer.Click += (s, e) => miEntityRelationshipOneToManyExplorer_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemEntityRelationshipManyToManyExplorer = new MenuItem()
                    {
                        Header = "Entity Relationship Many-To-Many Explorer",
                    };
                    menuItemEntityRelationshipManyToManyExplorer.Click += (s, e) => miEntityRelationshipManyToManyExplorer_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemEntityKeyExplorer = new MenuItem()
                    {
                        Header = "Entity Key Explorer",
                    };
                    menuItemEntityKeyExplorer.Click += (s, e) => miEntityKeyExplorer_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemEntityPrivilegesExplorer = new MenuItem()
                    {
                        Header = "Entity Privileges Explorer",
                    };
                    menuItemEntityPrivilegesExplorer.Click += (s, e) => miEntityPrivilegesExplorer_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemSystemForm = new MenuItem()
                    {
                        Header = "System Form",
                    };
                    menuItemSystemForm.Click += (s, e) => miExplorerSystemForms_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemSystemView = new MenuItem()
                    {
                        Header = "System View",
                    };
                    menuItemSystemView.Click += (s, e) => miExplorerSystemViews_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemSystemChart = new MenuItem()
                    {
                        Header = "System Chart",
                    };
                    menuItemSystemChart.Click += (s, e) => miExplorerSystemCharts_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);


                    menuItemEntityInformation.Items.Add(menuItemEntityAttributeExplorer);
                    menuItemEntityInformation.Items.Add(menuItemEntityRelationshipOneToManyExplorer);
                    menuItemEntityInformation.Items.Add(menuItemEntityRelationshipManyToManyExplorer);
                    menuItemEntityInformation.Items.Add(menuItemEntityKeyExplorer);
                    menuItemEntityInformation.Items.Add(new Separator());
                    menuItemEntityInformation.Items.Add(menuItemEntityPrivilegesExplorer);
                    menuItemEntityInformation.Items.Add(new Separator());
                    menuItemEntityInformation.Items.Add(menuItemSystemForm);
                    menuItemEntityInformation.Items.Add(menuItemSystemView);
                    menuItemEntityInformation.Items.Add(menuItemSystemChart);
                }

                miConnection.Items.Add(new Separator());
                miConnection.Items.Add(menuItemEntityMetadata);
                miConnection.Items.Add(menuItemEntityInformation);
            }

            #endregion Entity Information

            #region Plugin Information

            {
                //<MenuItem Header="Plugin Information" >
                //    <MenuItem Header="PluginType Explorer"  Click="miExplorerPluginTypeDescription_Click"/>
                //    <MenuItem Header="Plugin Assembly Explorer"  Click="miExplorerPluginAssemblyDescription_Click"/>
                //    <Separator/>
                //    <MenuItem Header="Plugin Tree"  Click="miExplorerPluginTree_Click"/>
                //    <MenuItem Header="Message Tree" Click="miExplorerSdkMessageTree_Click" />
                //    <MenuItem Header="Message Request Tree" Click="miExplorerSdkMessageRequestTree_Click" />
                //</MenuItem>

                var menuItemPluginInformation = new MenuItem()
                {
                    Header = "Plugin Information",
                };

                {
                    var menuItemPluginTypeExplorer = new MenuItem()
                    {
                        Header = "PluginType Explorer",
                    };
                    menuItemPluginTypeExplorer.Click += (s, e) => miExplorerPluginTypeExplorer_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemPluginAssemblyExplorer = new MenuItem()
                    {
                        Header = "Plugin Assembly Explorer",
                    };
                    menuItemPluginAssemblyExplorer.Click += (s, e) => miPluginAssemblyExplorer_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemPluginTree = new MenuItem()
                    {
                        Header = "Plugin Tree",
                    };
                    menuItemPluginTree.Click += (s, e) => miExplorerPluginTree_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemMessageTree = new MenuItem()
                    {
                        Header = "Message Tree",
                    };
                    menuItemMessageTree.Click += (s, e) => miExplorerSdkMessageTree_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemMessageRequestTree = new MenuItem()
                    {
                        Header = "Message Request Tree",
                    };
                    menuItemMessageRequestTree.Click += (s, e) => miExplorerSdkMessageRequestTree_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    menuItemPluginInformation.Items.Add(menuItemPluginTypeExplorer);
                    menuItemPluginInformation.Items.Add(menuItemPluginAssemblyExplorer);
                    menuItemPluginInformation.Items.Add(new Separator());
                    menuItemPluginInformation.Items.Add(menuItemPluginTree);
                    menuItemPluginInformation.Items.Add(menuItemMessageTree);
                    menuItemPluginInformation.Items.Add(menuItemMessageRequestTree);
                }

                miConnection.Items.Add(menuItemPluginInformation);
            }

            #endregion Plugin Information

            #region Security

            {
                //<Separator/>
                //<MenuItem Header="Security" >
                //    <MenuItem Header="Entity Privileges Explorer" Click="miEntityPrivilegesExplorer_Click"/>
                //    <MenuItem Header="SystemUsers Explorer"  Click="miSystemUsersExplorer_Click"/>
                //    <MenuItem Header="Teams Explorer" Click="miTeamsExplorer_Click" />
                //    <MenuItem Header="SecurityRoles Explorer" Click="miSecurityRolesExplorer_Click" />
                //</MenuItem>

                var menuItemSecurity = new MenuItem()
                {
                    Header = "Security",
                };

                {
                    var menuItemEntityPrivilegesExplorer = new MenuItem()
                    {
                        Header = "Entity Privileges Explorer",
                    };
                    menuItemEntityPrivilegesExplorer.Click += (s, e) => miEntityPrivilegesExplorer_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemSystemUsersExplorer = new MenuItem()
                    {
                        Header = "SystemUsers Explorer",
                    };
                    menuItemSystemUsersExplorer.Click += (s, e) => miSystemUsersExplorer_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemTeamsExplorer = new MenuItem()
                    {
                        Header = "Teams Explorer",
                    };
                    menuItemTeamsExplorer.Click += (s, e) => miTeamsExplorer_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemSecurityRolesExplorer = new MenuItem()
                    {
                        Header = "SecurityRoles Explorer",
                    };
                    menuItemSecurityRolesExplorer.Click += (s, e) => miSecurityRolesExplorer_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    menuItemSecurity.Items.Add(menuItemEntityPrivilegesExplorer);
                    menuItemSecurity.Items.Add(new Separator());
                    menuItemSecurity.Items.Add(menuItemSystemUsersExplorer);
                    menuItemSecurity.Items.Add(menuItemTeamsExplorer);
                    menuItemSecurity.Items.Add(menuItemSecurityRolesExplorer);
                }

                miConnection.Items.Add(new Separator());
                miConnection.Items.Add(menuItemSecurity);
            }

            #endregion Security

            #region Explorers

            {
                //<MenuItem Header="Organization" Click="miExplorerOrganization_Click" />

                //<Separator/>
                //<MenuItem Header="ApplicationRibbon"  Click="miExplorerApplicationRibbon_Click"/>

                //<Separator/>
                //<MenuItem Header="Global Option Sets"  Click="miExplorerGlobalOptionSets_Click"/>

                //<Separator/>
                //<MenuItem Header="SiteMap"  Click="miExplorerSitemap_Click"/>
                //<MenuItem Header="System Forms Events"  Click="miExplorerSystemFormsEvents_Click"/>

                //<Separator/>
                //<MenuItem Header="WebResources"  Click="miExplorerWebResources_Click"/>
                //<MenuItem Header="Workflows"  Click="miExplorerWorkflows_Click"/>
                //<MenuItem Header="Reports"  Click="miExplorerReports_Click"/>
                //<MenuItem Header="Custom Controls"  Click="miExplorerCustomControls_Click"/>

                var menuItemExplorers = new MenuItem()
                {
                    Header = "Explorers",
                };

                {
                    var menuItemOrganization = new MenuItem()
                    {
                        Header = "Organization",
                    };
                    menuItemOrganization.Click += (s, e) => miExplorerOrganization_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemApplicationRibbon = new MenuItem()
                    {
                        Header = "ApplicationRibbon",
                    };
                    menuItemApplicationRibbon.Click += (s, e) => miExplorerApplicationRibbon_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemGlobalOptionSets = new MenuItem()
                    {
                        Header = "Global Option Sets",
                    };
                    menuItemGlobalOptionSets.Click += (s, e) => miExplorerGlobalOptionSets_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemSiteMap = new MenuItem()
                    {
                        Header = "SiteMap",
                    };
                    menuItemSiteMap.Click += (s, e) => miExplorerSitemap_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemSystemFormsEvents = new MenuItem()
                    {
                        Header = "System Forms Events",
                    };
                    menuItemSystemFormsEvents.Click += (s, e) => miExplorerSystemFormsEvents_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemWebResources = new MenuItem()
                    {
                        Header = "WebResources",
                    };
                    menuItemWebResources.Click += (s, e) => miExplorerWebResources_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemWorkflows = new MenuItem()
                    {
                        Header = "Workflows",
                    };
                    menuItemWorkflows.Click += (s, e) => miExplorerWorkflows_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemReports = new MenuItem()
                    {
                        Header = "Reports",
                    };
                    menuItemReports.Click += (s, e) => miExplorerReports_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemCustomControls = new MenuItem()
                    {
                        Header = "Custom Controls",
                    };
                    menuItemCustomControls.Click += (s, e) => miExplorerCustomControls_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    menuItemExplorers.Items.Add(menuItemOrganization);

                    menuItemExplorers.Items.Add(new Separator());
                    menuItemExplorers.Items.Add(menuItemApplicationRibbon);

                    menuItemExplorers.Items.Add(new Separator());
                    menuItemExplorers.Items.Add(menuItemGlobalOptionSets);

                    menuItemExplorers.Items.Add(new Separator());
                    menuItemExplorers.Items.Add(menuItemSiteMap);
                    menuItemExplorers.Items.Add(menuItemSystemFormsEvents);

                    menuItemExplorers.Items.Add(new Separator());
                    menuItemExplorers.Items.Add(menuItemWebResources);
                    menuItemExplorers.Items.Add(menuItemWorkflows);
                    menuItemExplorers.Items.Add(menuItemReports);
                    menuItemExplorers.Items.Add(menuItemCustomControls);
                }

                miConnection.Items.Add(new Separator());
                miConnection.Items.Add(menuItemExplorers);
            }

            #endregion Explorers

            #region Solutions

            {
                //<Separator/>
                //<MenuItem Header="Solution Explorer"  Click="miExplorerSolutionComponents_Click"/>
                //<MenuItem Header="Open SolutionImage Window"  Click="miOpenSolutionImage_Click"/>
                //<MenuItem Header="Open SolutionDifferenceImage Window"  Click="miOpenSolutionDifferenceImage_Click"/>

                var menuItemSolutions = new MenuItem()
                {
                    Header = "Solutions",
                };

                {
                    var menuItemSolutionExplorer = new MenuItem()
                    {
                        Header = "Solution Explorer",
                    };
                    menuItemSolutionExplorer.Click += (s, e) => miExplorerSolutionComponents_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemOpenSolutionImageWindow = new MenuItem()
                    {
                        Header = "Open SolutionImage Window",
                    };
                    menuItemOpenSolutionImageWindow.Click += (s, e) => miOpenSolutionImage_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    var menuItemOpenSolutionDifferenceImage = new MenuItem()
                    {
                        Header = "Open SolutionDifferenceImage Window",
                    };
                    menuItemOpenSolutionDifferenceImage.Click += (s, e) => miOpenSolutionDifferenceImage_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                    menuItemSolutions.Items.Add(menuItemSolutionExplorer);

                    menuItemSolutions.Items.Add(new Separator());
                    menuItemSolutions.Items.Add(menuItemOpenSolutionImageWindow);

                    menuItemSolutions.Items.Add(new Separator());
                    menuItemSolutions.Items.Add(menuItemOpenSolutionDifferenceImage);
                }

                miConnection.Items.Add(new Separator());
                miConnection.Items.Add(menuItemSolutions);
            }

            #endregion Solutions

            #region Organization Difference

            {
                //<MenuItem Header="Open Organization Difference Image"  Click="miOpenOrganizationDifferenceImage_Click"/>

                var menuItemOpenOrganizationDifference = new MenuItem()
                {
                    Header = "Open Organization Difference Image",
                };
                menuItemOpenOrganizationDifference.Click += (s, e) => miOpenOrganizationDifferenceImage_Click(iWriteToOutput, commonConfig, getSelectedSingleConnection);

                miConnection.Items.Add(new Separator());
                miConnection.Items.Add(menuItemOpenOrganizationDifference);
            }

            #endregion Organization Difference
        }

        #region Кнопки одной среды.

        private static void miExplorerSitemap_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        ExplorerController contr = new ExplorerController(iWriteToOutput);

                        contr.ExecuteOpeningSitemapExplorer(connection, commonConfig, string.Empty);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miExplorerGlobalOptionSets_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        var contr = new EntityMetadataController(iWriteToOutput);

                        contr.ExecuteCreatingFileWithGlobalOptionSets(connection, commonConfig, null, null);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miExplorerSystemFormsEvents_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        ExportXmlController contr = new ExportXmlController(iWriteToOutput);

                        contr.ExecuteExportingFormsEvents(connection, commonConfig);

                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miExplorerEntityMetadata_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        var contr = new EntityMetadataController(iWriteToOutput);

                        contr.ExecuteCreatingFileWithEntityMetadata(string.Empty, null, connection, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miEntityAttributeExplorer_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        var contr = new EntityMetadataController(iWriteToOutput);

                        contr.ExecuteOpeningEntityAttributeExplorer(string.Empty, connection, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miEntityRelationshipOneToManyExplorer_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        var contr = new EntityMetadataController(iWriteToOutput);

                        contr.ExecuteOpeningEntityRelationshipOneToManyExplorer(string.Empty, connection, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miEntityRelationshipManyToManyExplorer_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        var contr = new EntityMetadataController(iWriteToOutput);

                        contr.ExecuteOpeningEntityRelationshipManyToManyExplorer(string.Empty, connection, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miEntityKeyExplorer_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        var contr = new EntityMetadataController(iWriteToOutput);

                        contr.ExecuteOpeningEntityKeyExplorer(string.Empty, connection, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miEntityPrivilegesExplorer_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        var contr = new EntityMetadataController(iWriteToOutput);

                        contr.ExecuteOpeningEntityPrivilegesExplorer(string.Empty, connection, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miSystemUsersExplorer_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        var contr = new SecurityController(iWriteToOutput);

                        contr.ExecuteShowingSystemUserExplorer(string.Empty, connection, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miTeamsExplorer_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        var contr = new SecurityController(iWriteToOutput);

                        contr.ExecuteShowingTeamsExplorer(string.Empty, connection, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miSecurityRolesExplorer_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        var contr = new SecurityController(iWriteToOutput);

                        contr.ExecuteShowingSecurityRolesExplorer(string.Empty, connection, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miExplorerApplicationRibbon_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        ExplorerController contr = new ExplorerController(iWriteToOutput);

                        contr.ExecuteOpeningApplicationRibbonExplorer(string.Empty, connection, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miExplorerWorkflows_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        ExplorerController contr = new ExplorerController(iWriteToOutput);

                        contr.ExecuteOpeningWorkflowExplorer(string.Empty, connection, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miExplorerSystemForms_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        ExplorerController contr = new ExplorerController(iWriteToOutput);

                        contr.ExecuteOpeningSystemFormExplorer(connection, commonConfig, string.Empty, null);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miExplorerSystemViews_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        ExplorerController contr = new ExplorerController(iWriteToOutput);

                        contr.ExecuteOpeningSystemSavedQueryExplorer(string.Empty, connection, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miExplorerSystemCharts_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        ExplorerController contr = new ExplorerController(iWriteToOutput);

                        contr.ExecuteOpeningSystemSavedQueryVisualizationExplorer(string.Empty, connection, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miExplorerWebResources_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        ExplorerController contr = new ExplorerController(iWriteToOutput);

                        contr.ExecuteOpeningWebResourceExplorer(connection, commonConfig, string.Empty);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miExplorerReports_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        ExplorerController contr = new ExplorerController(iWriteToOutput);

                        contr.ExecuteOpeningReportExplorer(connection, commonConfig, string.Empty);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miExplorerCustomControls_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        ExplorerController contr = new ExplorerController(iWriteToOutput);

                        contr.ExecuteOpeningCustomControlExplorer(string.Empty, connection, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miExplorerSolutionComponents_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        SolutionController contr = new SolutionController(iWriteToOutput);

                        contr.ExecuteOpeningSolutionExlorerWindow(null, connection, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miPluginAssemblyExplorer_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        var contr = new ExplorerController(iWriteToOutput);

                        contr.ExecuteOpeningPluginAssemblyExplorer(connection, commonConfig, string.Empty);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miExplorerPluginTypeExplorer_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        var contr = new ExplorerController(iWriteToOutput);

                        contr.ExecuteOpeningPluginTypeExplorer(connection, commonConfig, string.Empty);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miExplorerPluginTree_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        var contr = new ExplorerController(iWriteToOutput);

                        contr.ExecuteShowingPluginTree(connection, commonConfig, string.Empty, string.Empty, string.Empty);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miExplorerSdkMessageTree_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        var contr = new ExplorerController(iWriteToOutput);

                        contr.ExecuteShowingSdkMessageTree(connection, commonConfig, string.Empty, string.Empty);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miExplorerSdkMessageRequestTree_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        var contr = new ExplorerController(iWriteToOutput);

                        contr.ExecuteShowingSdkMessageRequestTree(connection, commonConfig, null, null, null);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miExplorerCreatePluginConfiguration_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                var form = new WindowPluginConfiguration(commonConfig, true);

                if (form.ShowDialog().GetValueOrDefault())
                {
                    commonConfig.Save();

                    var backWorker = new Thread(() =>
                    {
                        try
                        {
                            ExportPluginConfigurationController contr = new ExportPluginConfigurationController(iWriteToOutput);

                            contr.ExecuteExportingPluginConfigurationXml(connection, commonConfig);
                        }
                        catch (Exception ex)
                        {
                            iWriteToOutput.WriteErrorToOutput(null, ex);
                        }
                    });
                    backWorker.Start();
                }
            }
        }

        private static void miExplorerPluginConfigurationAssembly_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                try
                {
                    PluginConfigurationController contr = new PluginConfigurationController(iWriteToOutput);

                    contr.ExecuteShowingPluginConfigurationAssemblyDescription(commonConfig, string.Empty);
                }
                catch (Exception ex)
                {
                    iWriteToOutput.WriteErrorToOutput(null, ex);
                }
            }
        }

        private static void miExplorerPluginConfigurationType_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                try
                {
                    PluginConfigurationController contr = new PluginConfigurationController(iWriteToOutput);

                    contr.ExecuteShowingPluginConfigurationTypeDescription(commonConfig, string.Empty);
                }
                catch (Exception ex)
                {
                    iWriteToOutput.WriteErrorToOutput(null, ex);
                }
            }
        }

        private static void miExplorerPluginConfigurationTree_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                try
                {
                    PluginConfigurationController contr = new PluginConfigurationController(iWriteToOutput);

                    contr.ExecuteShowingPluginConfigurationTree(connection, commonConfig, string.Empty);
                }
                catch (Exception ex)
                {
                    iWriteToOutput.WriteErrorToOutput(null, ex);
                }
            }
        }

        private static void miExplorerPluginConfigurationComparer_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                try
                {
                    PluginConfigurationController contr = new PluginConfigurationController(iWriteToOutput);

                    contr.ExecuteShowingPluginConfigurationComparer(commonConfig, string.Empty);
                }
                catch (Exception ex)
                {
                    iWriteToOutput.WriteErrorToOutput(null, ex);
                }
            }
        }

        private static void miOpenSolutionImage_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        SolutionController contr = new SolutionController(iWriteToOutput);

                        contr.ExecuteOpeningSolutionImageWindow(connection, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miOpenSolutionDifferenceImage_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        SolutionController contr = new SolutionController(iWriteToOutput);

                        contr.ExecuteOpeningSolutionDifferenceImageWindow(connection, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miExplorerOrganization_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        var contr = new ExplorerController(iWriteToOutput);

                        contr.ExecuteOpeningOrganizationExplorer(connection, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miOpenOrganizationDifferenceImage_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        var contr = new SolutionController(iWriteToOutput);

                        contr.ExecuteOpeningOrganizationDifferenceImageWindow(connection, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        #endregion Кнопки одной среды.

        #region Кнопки Check.

        private static void miFindObjectsNamesForPrefix_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                var dialog = new WindowSelectPrefix("Select Entity Name Prefix", "Entity Name Prefix");

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    commonConfig.Save();

                    var backWorker = new Thread(() =>
                    {
                        try
                        {
                            var contr = new CheckController(iWriteToOutput);

                            contr.ExecuteCheckingEntitiesNames(connection, commonConfig, dialog.Prefix);
                        }
                        catch (Exception ex)
                        {
                            iWriteToOutput.WriteErrorToOutput(null, ex);
                        }
                    });

                    backWorker.Start();
                }
            }
        }

        private static void miFindObjectsNamesForPrefixAndShowDependentComponents_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                var dialog = new WindowSelectPrefix("Select Entity Name Prefix", "Entity Name Prefix");

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    commonConfig.Save();

                    var backWorker = new Thread(() =>
                    {
                        try
                        {
                            var contr = new CheckController(iWriteToOutput);

                            contr.ExecuteCheckingEntitiesNamesAndShowDependentComponents(connection, commonConfig, dialog.Prefix);
                        }
                        catch (Exception ex)
                        {
                            iWriteToOutput.WriteErrorToOutput(null, ex);
                        }
                    });

                    backWorker.Start();
                }
            }
        }

        private static void miFindObjectsMarkedToDeleteAndShowDependentComponents_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                var dialog = new WindowSelectPrefix("Select mark to delete", "Mark to delete");

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    commonConfig.Save();

                    var backWorker = new Thread(() =>
                    {
                        try
                        {
                            var contr = new CheckController(iWriteToOutput);

                            contr.ExecuteCheckingMarkedToDelete(connection, commonConfig, dialog.Prefix);
                        }
                        catch (Exception ex)
                        {
                            iWriteToOutput.WriteErrorToOutput(null, ex);
                        }
                    });

                    backWorker.Start();
                }
            }
        }

        private static void miCheckEntitiesOwnership_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        var contr = new CheckController(iWriteToOutput);

                        contr.ExecuteCheckingEntitiesOwnership(connection, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miCheckGlobalOptionSetDuplicatesOnEntity_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        var contr = new CheckController(iWriteToOutput);

                        contr.ExecuteCheckingGlobalOptionSetDuplicates(connection, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miSolutionComponentTypeEnum_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        var contr = new CheckController(iWriteToOutput);

                        contr.ExecuteCheckingComponentTypeEnum(connection, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miCreateAllDependencyNodesDescription_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        var contr = new CheckController(iWriteToOutput);

                        contr.ExecuteCreatingAllDependencyNodesDescription(connection, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miCheckPluginStepsDuplicates_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        var contr = new CheckPluginController(iWriteToOutput);

                        contr.ExecuteCheckingPluginSteps(connection, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miCheckPluginImagesDuplicates_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        var contr = new CheckPluginController(iWriteToOutput);

                        contr.ExecuteCheckingPluginImages(connection, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miCheckPluginStepsRequiredComponents_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        var contr = new CheckPluginController(iWriteToOutput);

                        contr.ExecuteCheckingPluginStepsRequiredComponents(connection, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miCheckPluginImagesRequiredComponents_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        var contr = new CheckPluginController(iWriteToOutput);

                        contr.ExecuteCheckingPluginImagesRequiredComponents(connection, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        private static void miCheckWorkflowsUsedEntities_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                var dialog = new WindowSelectFolderForExport(connection, commonConfig.FolderForExport, commonConfig.DefaultFileAction);

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    var connectionData = dialog.GetConnectionData();

                    if (connectionData != null)
                    {
                        commonConfig.Save();

                        var backWorker = new Thread(() =>
                        {
                            try
                            {
                                var contr = new CheckController(iWriteToOutput);

                                contr.ExecuteCheckingWorkflowsUsedEntities(connectionData, commonConfig);
                            }
                            catch (Exception ex)
                            {
                                iWriteToOutput.WriteErrorToOutput(null, ex);
                            }
                        });

                        backWorker.Start();
                    }
                }
            }
        }

        private static void miCheckWorkflowsUsedNotExistsEntities_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                var dialog = new WindowSelectFolderForExport(connection, commonConfig.FolderForExport, commonConfig.DefaultFileAction);

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    var connectionData = dialog.GetConnectionData();

                    if (connectionData != null)
                    {
                        commonConfig.Save();

                        var backWorker = new Thread(() =>
                        {
                            try
                            {
                                var contr = new CheckController(iWriteToOutput);

                                contr.ExecuteCheckingWorkflowsNotExistingUsedEntities(connectionData, commonConfig);
                            }
                            catch (Exception ex)
                            {
                                iWriteToOutput.WriteErrorToOutput(null, ex);
                            }
                        });

                        backWorker.Start();
                    }
                }
            }
        }

        private static void miFindEntityObjectsByName_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                var dialog = new WindowSelectPrefix("Select Element Name", "Element Name");

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    commonConfig.Save();

                    var backWorker = new Thread(() =>
                    {
                        try
                        {
                            var contr = new CheckController(iWriteToOutput);

                            contr.ExecuteFindEntityElementsByName(connection, commonConfig, dialog.Prefix);
                        }
                        catch (Exception ex)
                        {
                            iWriteToOutput.WriteErrorToOutput(null, ex);
                        }
                    });

                    backWorker.Start();
                }
            }
        }

        private static void miFindEntityObjectsContainsString_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                var dialog = new WindowSelectPrefix("Select String for contain", "String for contain");

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    commonConfig.Save();

                    var backWorker = new Thread(() =>
                    {
                        try
                        {
                            var contr = new CheckController(iWriteToOutput);

                            contr.ExecuteFindEntityElementsContainsString(connection, commonConfig, dialog.Prefix);
                        }
                        catch (Exception ex)
                        {
                            iWriteToOutput.WriteErrorToOutput(null, ex);
                        }
                    });

                    backWorker.Start();
                }
            }
        }

        private static void miFindEntityObjectsById_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                var dialog = new WindowSelectEntityIdToFind(commonConfig, connection, string.Format("Find Entity in {0} by Id", connection.Name));

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    string entityName = dialog.EntityTypeName;
                    int? entityTypeCode = dialog.EntityTypeCode;
                    Guid entityId = dialog.EntityId;

                    var connectionData = dialog.GetConnectionData();

                    if (connectionData != null)
                    {
                        commonConfig.Save();

                        var backWorker = new Thread(() =>
                        {
                            try
                            {
                                var contr = new CheckController(iWriteToOutput);

                                contr.ExecuteFindEntityById(connectionData, commonConfig, entityName, entityTypeCode, entityId);
                            }
                            catch (Exception ex)
                            {
                                iWriteToOutput.WriteErrorToOutput(null, ex);
                            }
                        });

                        backWorker.Start();
                    }
                }
            }
        }

        private static void miFindEntityObjectsByUniqueidentifier_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                var dialog = new WindowSelectEntityIdToFind(commonConfig, connection, string.Format("Find Entity in {0} by Uniqueidentifier", connection.Name));

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    string entityName = dialog.EntityTypeName;
                    int? entityTypeCode = dialog.EntityTypeCode;
                    Guid entityId = dialog.EntityId;

                    var connectionData = dialog.GetConnectionData();

                    if (connectionData != null)
                    {
                        commonConfig.Save();

                        var backWorker = new Thread(() =>
                        {
                            try
                            {
                                var contr = new CheckController(iWriteToOutput);

                                contr.ExecuteFindEntityByUniqueidentifier(connectionData, commonConfig, entityName, entityTypeCode, entityId);
                            }
                            catch (Exception ex)
                            {
                                iWriteToOutput.WriteErrorToOutput(null, ex);
                            }
                        });

                        backWorker.Start();
                    }
                }
            }
        }

        private static void miEditEntityObjectsById_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                var dialog = new WindowSelectEntityIdToFind(commonConfig, connection, string.Format("Edit Entity in {0} by Id", connection.Name));

                if (dialog.ShowDialog().GetValueOrDefault())
                {
                    string entityName = dialog.EntityTypeName;
                    int? entityTypeCode = dialog.EntityTypeCode;
                    Guid entityId = dialog.EntityId;

                    var connectionData = dialog.GetConnectionData();

                    if (connectionData != null)
                    {
                        commonConfig.Save();

                        var backWorker = new Thread(() =>
                        {
                            try
                            {
                                var contr = new CheckController(iWriteToOutput);

                                contr.ExecuteEditEntityById(connectionData, commonConfig, entityName, entityTypeCode, entityId);
                            }
                            catch (Exception ex)
                            {
                                iWriteToOutput.WriteErrorToOutput(null, ex);
                            }
                        });

                        backWorker.Start();
                    }
                }
            }
        }

        private static void miCheckManagedEntitiesInCRM_Click(IWriteToOutput iWriteToOutput, CommonConfiguration commonConfig, Func<ConnectionData> getSelectedSingleConnection)
        {
            var connection = getSelectedSingleConnection();

            if (connection != null)
            {
                commonConfig.Save();

                var backWorker = new Thread(() =>
                {
                    try
                    {
                        var contr = new CheckManagedEntitiesController(iWriteToOutput);

                        contr.ExecuteCheckingManagedEntities(connection, commonConfig);
                    }
                    catch (Exception ex)
                    {
                        iWriteToOutput.WriteErrorToOutput(null, ex);
                    }
                });
                backWorker.Start();
            }
        }

        #endregion Кнопки Check.
    }
}
