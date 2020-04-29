using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Views
{
    public static class WindowHelper
    {
        private static void ExecuteInSTAThread(Func<System.Windows.Window> windowGetter)
        {
            ExecuteWithConnectionInSTAThread(null, windowGetter);
        }

        private static void ExecuteWithConnectionInSTAThread(ConnectionData connectionData, Func<System.Windows.Window> windowGetter)
        {
            var worker = new Thread(() =>
            {
                try
                {
                    var form = windowGetter();

                    form.ShowDialog();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.SetApartmentState(ApartmentState.STA);

            worker.Start();
        }

        public static void OpenEntityMetadataExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
        )
        {
            OpenEntityMetadataExplorer(iWriteToOutput, service, commonConfig, null, null, null, false, null);
        }

        public static void OpenEntityMetadataExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntityName
        )
        {
            OpenEntityMetadataExplorer(iWriteToOutput, service, commonConfig, null, filterEntityName, null, false, null);
        }

        public static void OpenEntityMetadataExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntityName
            , EnvDTE.SelectedItem selectedItem
        )
        {
            OpenEntityMetadataExplorer(iWriteToOutput, service, commonConfig, null, filterEntityName, null, false, selectedItem);
        }

        public static void OpenEntityMetadataExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntityName
            , string filePath
            , bool isJavaScript
        )
        {
            OpenEntityMetadataExplorer(iWriteToOutput, service, commonConfig, null, filterEntityName, filePath, isJavaScript, null);
        }

        public static void OpenEntityMetadataExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , IEnumerable<EntityMetadata> entityMetadataList
            , string filterEntityName
        )
        {
            OpenEntityMetadataExplorer(iWriteToOutput, service, commonConfig, entityMetadataList, filterEntityName, null, false, null);
        }

        public static void OpenEntityMetadataExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , IEnumerable<EntityMetadata> entityMetadataList
            , string filterEntityName
            , string filePath
            , bool isJavaScript
            , EnvDTE.SelectedItem selectedItem
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerEntityMetadata
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                    , filterEntityName
                    , entityMetadataList
                    , filePath
                    , isJavaScript
                    , selectedItem
                )
            );
        }

        public static void OpenEntityMetadataFileGenerationOptions(FileGenerationOptions fileGenerationOptions)
        {
            ExecuteInSTAThread(() =>
                new WindowExplorerEntityMetadataOptions(fileGenerationOptions)
            );
        }

        public static void OpenGlobalOptionSetsFileGenerationOptions(FileGenerationOptions fileGenerationOptions)
        {
            ExecuteInSTAThread(() =>
                new WindowExplorerGlobalOptionSetsOptions(fileGenerationOptions)
            );
        }

        public static void OpenFileGenerationOptions(FileGenerationOptions fileGenerationOptions)
        {
            ExecuteInSTAThread(() =>
                new WindowFileGenerationOptions(fileGenerationOptions)
            );
        }

        public static void OpenFileGenerationConfiguration(FileGenerationConfiguration fileGenerationConfiguration)
        {
            ExecuteInSTAThread(() =>
                new WindowFileGenerationConfiguration(fileGenerationConfiguration)
            );
        }

        public static void OpenEntityAttributeExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntityName = null
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerEntityAttribute
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                    , filterEntityName
                )
            );
        }

        public static void OpenEntityKeyExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntityName = null
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerEntityKey
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                    , filterEntityName
                )
            );
        }

        public static void OpenEntityRelationshipOneToManyExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntityName = null
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerEntityRelationshipOneToMany
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                    , filterEntityName
                )
            );
        }

        public static void OpenEntityRelationshipManyToManyExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntityName = null
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerEntityRelationshipManyToMany
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                    , filterEntityName
                )
            );
        }

        public static void OpenEntityPrivilegesExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
        )
        {
            OpenEntityPrivilegesExplorer(iWriteToOutput, service, commonConfig, null, null);
        }

        public static void OpenEntityPrivilegesExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntityName
        )
        {
            OpenEntityPrivilegesExplorer(iWriteToOutput, service, commonConfig, filterEntityName, null);
        }

        public static void OpenEntityPrivilegesExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntityName
            , IEnumerable<EntityMetadata> entityMetadataList
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerEntityPrivileges
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                    , entityMetadataList
                    , filterEntityName
                )
            );
        }

        public static void OpenOtherPrivilegesExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
        )
        {
            OpenOtherPrivilegesExplorer(iWriteToOutput, service, commonConfig, null, null);
        }

        public static void OpenOtherPrivilegesExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filter
        )
        {
            OpenOtherPrivilegesExplorer(iWriteToOutput, service, commonConfig, filter, null);
        }

        public static void OpenOtherPrivilegesExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filter
            , IEnumerable<Privilege> privilegesList
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerOtherPrivileges
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                    , privilegesList
                    , filter
                )
            );
        }

        public static void OpenEntityEditor(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string entityName
            , Guid entityId
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowEntityEditor
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                    , entityName
                    , entityId
                )
            );
        }

        public static void OpenEntityBulkEditor(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string entityName
            , IEnumerable<Guid> entityIds
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowEntityBulkEditor
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                    , entityName
                    , entityIds
                )
            );
        }

        public static void OpenEntityBulkTransfer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , EntityMetadata entityMetadata
            , IEnumerable<Entity> entities
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowEntityBulkTransfer
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                    , entityMetadata
                    , entities
                )
            );
        }

        public static void OpenApplicationRibbonExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerApplicationRibbon
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                )
            );
        }

        public static void OpenSystemFormExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
        )
        {
            OpenSystemFormExplorer(iWriteToOutput, service, commonConfig, null, null, null);
        }

        public static void OpenSystemFormExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntityName
        )
        {
            OpenSystemFormExplorer(iWriteToOutput, service, commonConfig, filterEntityName, null, null);
        }

        public static void OpenSystemFormExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntityName
            , string selection
        )
        {
            OpenSystemFormExplorer(iWriteToOutput, service, commonConfig, filterEntityName, selection, null);
        }

        public static void OpenSystemFormExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntityName
            , string selection
            , EnvDTE.SelectedItem selectedItem
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerSystemForm
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                    , filterEntityName
                    , selectedItem
                    , selection
                )
            );
        }

        public static void OpenSavedQueryExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntityName = null
            , string selection = null
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerSavedQuery
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                    , filterEntityName
                    , selection
                )
            );
        }

        public static void OpenSavedQueryVisualizationExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntityName = null
            , string selection = null
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerSavedQueryVisualization
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                    , filterEntityName
                    , selection
                )
            );
        }

        public static void OpenWorkflowExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntityName = null
            , string selection = null
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerWorkflow
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                    , filterEntityName
                    , selection
                )
            );
        }

        public static void OpenCustomControlExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filter
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerCustomControl
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                    , filter
                )
            );
        }

        public static void OpenTraceReaderExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowTraceReader
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                )
            );
        }

        public static void OpenPluginTree(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string entityFilter = null
            , string pluginTypeFilter = null
            , string messageFilter = null
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowTreePlugin
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                    , entityFilter
                    , pluginTypeFilter
                    , messageFilter
                )
            );
        }

        public static void OpenSdkMessageExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
        )
        {
            OpenSdkMessageExplorer(iWriteToOutput, service, commonConfig, null);
        }

        public static void OpenSdkMessageExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string messageFilter = null
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerSdkMessage
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                    , messageFilter
                )
            );
        }

        public static void OpenSdkMessageTree(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string entityFilter = null
            , string messageFilter = null
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowTreeSdkMessageFilter
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                    , entityFilter
                    , messageFilter
                )
            );
        }

        public static void OpenSdkMessageRequestTree(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
        )
        {
            OpenSdkMessageRequestTree(iWriteToOutput, service, commonConfig, null, false, null, null, null);
        }

        public static void OpenSdkMessageRequestTree(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig

            , string entityFilter
        )
        {
            OpenSdkMessageRequestTree(iWriteToOutput, service, commonConfig, null, false, null, entityFilter, null);
        }

        public static void OpenSdkMessageRequestTree(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig

            , string entityFilter
            , string messageFilter
        )
        {
            OpenSdkMessageRequestTree(iWriteToOutput, service, commonConfig, null, false, null, entityFilter, messageFilter);
        }

        public static void OpenSdkMessageRequestTree(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig

            , string filePath
            , bool isJavaScript
            , EnvDTE.SelectedItem selectedItem

            , string entityFilter
            , string messageFilter
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowTreeSdkMessageRequest
                (
                    iWriteToOutput
                    , service
                    , commonConfig

                    , filePath
                    , isJavaScript
                    , selectedItem

                    , entityFilter
                    , messageFilter
                )
            );
        }

        public static void OpenSystemUsersExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filter
        )
        {
            OpenSystemUsersExplorer(iWriteToOutput, service, commonConfig, filter, null, null);
        }

        public static void OpenSystemUsersExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filter
            , IEnumerable<EntityMetadata> entityMetadataList
            , IEnumerable<Privilege> privileges
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerSystemUser
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                    , entityMetadataList
                    , privileges
                    , filter
                )
            );
        }

        public static void OpenTeamsExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filter
        )
        {
            OpenTeamsExplorer(iWriteToOutput, service, commonConfig, filter, null, null);
        }

        public static void OpenTeamsExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filter
            , IEnumerable<EntityMetadata> entityMetadataList
            , IEnumerable<Privilege> privileges
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerTeam
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                    , entityMetadataList
                    , privileges
                    , filter
                )
            );
        }

        public static void OpenRolesExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
        )
        {
            OpenRolesExplorer(iWriteToOutput, service, commonConfig, null, null, null);
        }

        public static void OpenRolesExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filter
        )
        {
            OpenRolesExplorer(iWriteToOutput, service, commonConfig, filter, null, null);
        }

        public static void OpenRolesExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filter
            , IEnumerable<EntityMetadata> entityMetadataList
            , IEnumerable<Privilege> privileges
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerRole
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                    , entityMetadataList
                    , privileges
                    , filter
                )
            );
        }

        public static void OpenExplorerSolutionExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , int? componentType
            , Guid? objectId
            , EnvDTE.SelectedItem selectedItem
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerSolution
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                    , componentType
                    , objectId
                    , selectedItem
                )
            );
        }

        public static void OpenImportJobExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string selection
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerImportJob
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                    , selection
                )
            );
        }

        public static void OpenSolutionComponentsExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , SolutionComponentDescriptor descriptor
            , CommonConfiguration commonConfig
            , string solutionUniqueName
            , string selection = null
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerSolutionComponents(iWriteToOutput, service, descriptor, commonConfig, solutionUniqueName, selection)
            );
        }

        public static void OpenSolutionComponentDependenciesExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , SolutionComponentDescriptor descriptor
            , CommonConfiguration commonConfig
            , int componentType
            , Guid objectId
            , string selection = null
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerSolutionComponentDependencies
                (
                    iWriteToOutput
                    , service
                    , descriptor
                    , commonConfig
                    , componentType
                    , objectId
                    , selection
                )
            );
        }

        public static void OpenExplorerComponentsExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , SolutionComponentDescriptor descriptor
            , CommonConfiguration commonConfig
            , IEnumerable<SolutionComponent> solutionComponents
            , string solutionUniqueName
            , string header
            , string selection
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerComponents(iWriteToOutput, service, descriptor, commonConfig, solutionComponents, solutionUniqueName, header, selection)
            );
        }

        public static void OpenGlobalOptionSetsExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
        )
        {
            OpenGlobalOptionSetsExplorer(iWriteToOutput, service, commonConfig, null, null, null, null, false, null);
        }

        public static void OpenGlobalOptionSetsExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filter
        )
        {
            OpenGlobalOptionSetsExplorer(iWriteToOutput, service, commonConfig, null, null, filter, null, false, null);
        }

        public static void OpenGlobalOptionSetsExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filter
            , string filterEntityName
        )
        {
            OpenGlobalOptionSetsExplorer(iWriteToOutput, service, commonConfig, null, filterEntityName, filter, null, false, null);
        }

        public static void OpenGlobalOptionSetsExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filter
            , EnvDTE.SelectedItem selectedItem
        )
        {
            OpenGlobalOptionSetsExplorer(iWriteToOutput, service, commonConfig, null, null, filter, null, false, selectedItem);
        }

        public static void OpenGlobalOptionSetsExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filter
            , string filePath
            , bool isJavaScript
        )
        {
            OpenGlobalOptionSetsExplorer(iWriteToOutput, service, commonConfig, null, null, filter, filePath, isJavaScript, null);
        }

        public static void OpenGlobalOptionSetsExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , IEnumerable<OptionSetMetadata> optionSets
            , string filter
        )
        {
            OpenGlobalOptionSetsExplorer(iWriteToOutput, service, commonConfig, optionSets, null, filter, null, false, null);
        }

        public static void OpenGlobalOptionSetsExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , IEnumerable<OptionSetMetadata> optionSets
            , string filterEntityName
            , string filter
            , string filePath
            , bool isJavaScript
            , EnvDTE.SelectedItem selectedItem
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerGlobalOptionSets
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                    , optionSets
                    , filterEntityName
                    , filter
                    , filePath
                    , isJavaScript
                    , selectedItem
                )
            );
        }

        public static void OpenReportExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string selection = null
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerReport
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                    , selection
                )
            );
        }

        public static void OpenExportSiteMapExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
        )
        {
            OpenExportSiteMapExplorer(iWriteToOutput, service, commonConfig, string.Empty);
        }

        public static void OpenExportSiteMapExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filter
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerSiteMap
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                    , filter
                )
            );
        }

        public static void OpenWebResourceExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
        )
        {
            OpenWebResourceExplorer(iWriteToOutput, service, commonConfig, null);
        }

        public static void OpenWebResourceExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string selection
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerWebResource
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                    , selection
                )
            );
        }

        public static void OpenPluginTypeExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string selection = null
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerPluginType
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                    , selection
                )
            );
        }

        public static void OpenPluginAssemblyExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string selection = null
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerPluginAssembly
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                    , selection
                )
            );
        }

        public static void OpenPluginAssemblyUpdateWindow(IWriteToOutput iWriteToOutput, IOrganizationServiceExtented service, PluginAssembly assembly, EnvDTE.Project project)
        {
            string defaultOutputFilePath = null;

            if (project != null)
            {
                defaultOutputFilePath = PropertiesHelper.GetOutputFilePath(project);
            }

            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowPluginAssembly(iWriteToOutput, service, assembly, defaultOutputFilePath, project)
            );
        }

        public static void OpenOrganizationExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerOrganization
                (
                    iWriteToOutput
                    , service
                    , commonConfig
                )
            );
        }

        public static void OpenSolutionImageWindow(
            IWriteToOutput iWriteToOutput
            , ConnectionData connectionData
            , CommonConfiguration commonConfig
        )
        {
            ExecuteWithConnectionInSTAThread(connectionData, () =>
                new WindowSolutionImage
                (
                    iWriteToOutput
                    , commonConfig
                    , connectionData
                    , null
                )
            );
        }

        public static void OpenSolutionDifferenceImageWindow(
            IWriteToOutput iWriteToOutput
            , ConnectionData connectionData
            , CommonConfiguration commonConfig
        )
        {
            ExecuteWithConnectionInSTAThread(connectionData, () =>
                new WindowSolutionDifferenceImage
                (
                    iWriteToOutput
                    , commonConfig
                    , connectionData
                    , null
                )
            );
        }

        public static void OpenOrganizationDifferenceImageWindow(
            IWriteToOutput iWriteToOutput
            , ConnectionData connectionData
            , CommonConfiguration commonConfig
        )
        {
            ExecuteWithConnectionInSTAThread(connectionData, () =>
                new WindowOrganizationDifferenceImage
                (
                    iWriteToOutput
                    , commonConfig
                    , connectionData
                    , null
                )
            );
        }

        public static void OpenOrganizationComparerWindow(
            IWriteToOutput iWriteToOutput
            , ConnectionConfiguration crmConfig
            , CommonConfiguration commonConfig
        )
        {
            OpenOrganizationComparerWindow(iWriteToOutput, crmConfig, commonConfig, null);
        }

        public static void OpenOrganizationComparerWindow(
            IWriteToOutput iWriteToOutput
            , ConnectionConfiguration crmConfig
            , CommonConfiguration commonConfig
            , SolutionImage solutionImage
        )
        {
            ExecuteInSTAThread(() =>
                new WindowOrganizationComparer
                (
                    iWriteToOutput
                    , crmConfig
                    , commonConfig
                    , solutionImage
                )
            );
        }

        public static void OpenOrganizationComparerEntityMetadataWindow(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string entityFilter = null
        )
        {
            ExecuteInSTAThread(() =>
                new WindowOrganizationComparerEntityMetadata
                (
                    iWriteToOutput
                    , commonConfig
                    , connection1
                    , connection2
                    , entityFilter
                )
            );
        }

        public static void OpenOrganizationComparerApplicationRibbonWindow(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
        )
        {
            ExecuteInSTAThread(() =>
                new WindowOrganizationComparerApplicationRibbon
                (
                    iWriteToOutput
                    , commonConfig
                    , connection1
                    , connection2
                )
            );
        }

        public static void OpenOrganizationComparerGlobalOptionSetsWindow(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filter = null
        )
        {
            ExecuteInSTAThread(() =>
                new WindowOrganizationComparerGlobalOptionSets
                (
                    iWriteToOutput
                    , commonConfig
                    , connection1
                    , connection2
                    , filter
                )
            );
        }

        public static void OpenOrganizationComparerSystemFormWindow(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filterEntity = null
            , string filter = null
        )
        {
            ExecuteInSTAThread(() =>
                new WindowOrganizationComparerSystemForm
                (
                    iWriteToOutput
                    , commonConfig
                    , connection1
                    , connection2
                    , filterEntity
                    , filter
                )
            );
        }

        public static void OpenOrganizationComparerSavedQueryWindow(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filterEntity = null
            , string filter = null
        )
        {
            ExecuteInSTAThread(() =>
                new WindowOrganizationComparerSavedQuery
                (
                    iWriteToOutput
                    , commonConfig
                    , connection1
                    , connection2
                    , filterEntity
                    , filter
                )
            );
        }

        public static void OpenOrganizationComparerSavedQueryVisualizationWindow(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filterEntity = null
            , string filter = null
        )
        {
            ExecuteInSTAThread(() =>
                new WindowOrganizationComparerSavedQueryVisualization
                (
                    iWriteToOutput
                    , commonConfig
                    , connection1
                    , connection2
                    , filterEntity
                    , filter
                )
            );
        }

        public static void OpenOrganizationComparerWorkflowWindow(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filterEntity = null
            , string filter = null
        )
        {
            ExecuteInSTAThread(() =>
                new WindowOrganizationComparerWorkflow
                (
                    iWriteToOutput
                    , commonConfig
                    , connection1
                    , connection2
                    , filterEntity
                    , filter
                )
            );
        }

        public static void OpenOrganizationComparerReportWindow(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filter = null
        )
        {
            ExecuteInSTAThread(() =>
                new WindowOrganizationComparerReport
                (
                    iWriteToOutput
                    , commonConfig
                    , connection1
                    , connection2
                    , filter
                )
            );
        }

        public static void OpenOrganizationComparerSiteMapWindow(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
        )
        {
            ExecuteInSTAThread(() =>
                new WindowOrganizationComparerSiteMap
                (
                    iWriteToOutput
                    , commonConfig
                    , connection1
                    , connection2
                )
            );
        }

        public static void OpenOrganizationComparerWebResourcesWindow(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filter = null
        )
        {
            ExecuteInSTAThread(() =>
                new WindowOrganizationComparerWebResources
                (
                    iWriteToOutput
                    , commonConfig
                    , connection1
                    , connection2
                    , filter
                )
            );
        }

        public static void OpenOrganizationComparerPluginAssemblyWindow(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filter = null
        )
        {
            ExecuteInSTAThread(() =>
                new WindowOrganizationComparerPluginAssembly
                (
                    iWriteToOutput
                    , commonConfig
                    , connection1
                    , connection2
                    , filter
                )
            );
        }

        public static void OpenCrmConnectionCard(
            IWriteToOutput iWriteToOutput
            , ConnectionData connectionData
        )
        {
            var worker = new Thread(() =>
            {
                try
                {
                    var form = new WindowCrmConnectionCard(iWriteToOutput, connectionData, connectionData.ConnectionConfiguration.Users);

                    form.ShowDialog();

                    connectionData.Save();
                    connectionData.ConnectionConfiguration.Save();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            worker.SetApartmentState(ApartmentState.STA);

            worker.Start();
        }

        public static bool IsDefinedExplorer(ComponentType componentType)
        {
            switch (componentType)
            {
                case ComponentType.Entity:
                case ComponentType.Attribute:
                case ComponentType.OptionSet:
                case ComponentType.EntityRelationship:
                case ComponentType.EntityKey:

                case ComponentType.Role:
                case ComponentType.Organization:

                case ComponentType.SavedQuery:
                case ComponentType.SavedQueryVisualization:
                case ComponentType.SystemForm:

                case ComponentType.Workflow:
                case ComponentType.Report:
                case ComponentType.WebResource:
                case ComponentType.SiteMap:

                case ComponentType.PluginType:
                case ComponentType.PluginAssembly:

                case ComponentType.SdkMessageProcessingStep:
                case ComponentType.SdkMessageProcessingStepImage:

                case ComponentType.SdkMessage:
                case ComponentType.SdkMessageFilter:

                case ComponentType.SdkMessagePair:
                case ComponentType.SdkMessageRequest:
                case ComponentType.SdkMessageRequestField:
                case ComponentType.SdkMessageResponse:
                case ComponentType.SdkMessageResponseField:

                case ComponentType.RibbonCustomization:

                case ComponentType.CustomControl:

                    return true;
            }

            return false;
        }

        public static void OpenComponentExplorer(
            ComponentType componentType
            , IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string componentName
            , string parameter
        )
        {
            switch (componentType)
            {
                case ComponentType.Entity:
                    OpenEntityMetadataExplorer(iWriteToOutput, service, commonConfig, null, componentName);
                    break;

                case ComponentType.Attribute:
                    OpenEntityAttributeExplorer(iWriteToOutput, service, commonConfig, componentName);
                    break;

                case ComponentType.OptionSet:
                    OpenGlobalOptionSetsExplorer(iWriteToOutput, service, commonConfig, componentName);
                    break;

                case ComponentType.EntityRelationship:
                    if (string.Equals(parameter, typeof(OneToManyRelationshipMetadata).Name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        OpenEntityRelationshipOneToManyExplorer(iWriteToOutput, service, commonConfig, componentName);
                    }
                    else if (string.Equals(parameter, typeof(ManyToManyRelationshipMetadata).Name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        OpenEntityRelationshipManyToManyExplorer(iWriteToOutput, service, commonConfig, componentName);
                    }
                    break;

                case ComponentType.EntityKey:
                    OpenEntityKeyExplorer(iWriteToOutput, service, commonConfig, componentName);
                    break;

                case ComponentType.Role:
                    OpenRolesExplorer(iWriteToOutput, service, commonConfig, componentName, null, null);
                    break;

                case ComponentType.Organization:
                    OpenOrganizationExplorer(iWriteToOutput, service, commonConfig);
                    break;

                case ComponentType.SavedQuery:
                    OpenSavedQueryExplorer(iWriteToOutput, service, commonConfig, null, componentName);
                    break;

                case ComponentType.SavedQueryVisualization:
                    OpenSavedQueryVisualizationExplorer(iWriteToOutput, service, commonConfig, null, componentName);
                    break;

                case ComponentType.SystemForm:
                    OpenSystemFormExplorer(iWriteToOutput, service, commonConfig, null, componentName);
                    break;

                case ComponentType.Workflow:
                    OpenWorkflowExplorer(iWriteToOutput, service, commonConfig, null, componentName);
                    break;

                case ComponentType.Report:
                    OpenReportExplorer(iWriteToOutput, service, commonConfig, componentName);
                    break;

                case ComponentType.WebResource:
                    OpenWebResourceExplorer(iWriteToOutput, service, commonConfig, componentName);
                    break;

                case ComponentType.SiteMap:
                    OpenExportSiteMapExplorer(iWriteToOutput, service, commonConfig);
                    break;

                case ComponentType.PluginType:
                    OpenPluginTypeExplorer(iWriteToOutput, service, commonConfig, componentName);
                    break;

                case ComponentType.PluginAssembly:
                    OpenPluginAssemblyExplorer(iWriteToOutput, service, commonConfig, componentName);
                    break;

                case ComponentType.SdkMessageProcessingStep:
                case ComponentType.SdkMessageProcessingStepImage:
                    OpenPluginTree(iWriteToOutput, service, commonConfig);
                    break;

                case ComponentType.SdkMessage:
                    OpenSdkMessageExplorer(iWriteToOutput, service, commonConfig, componentName);
                    break;

                case ComponentType.SdkMessageFilter:
                    OpenSdkMessageTree(iWriteToOutput, service, commonConfig, null, componentName);
                    break;

                case ComponentType.SdkMessagePair:
                    OpenSdkMessageRequestTree(iWriteToOutput, service, commonConfig, null, componentName);
                    break;

                case ComponentType.SdkMessageRequest:
                case ComponentType.SdkMessageRequestField:
                case ComponentType.SdkMessageResponse:
                case ComponentType.SdkMessageResponseField:
                    OpenSdkMessageRequestTree(iWriteToOutput, service, commonConfig);
                    break;

                case ComponentType.RibbonCustomization:
                    OpenApplicationRibbonExplorer(iWriteToOutput, service, commonConfig);
                    break;

                case ComponentType.CustomControl:
                    OpenCustomControlExplorer(iWriteToOutput, service, commonConfig, componentName);
                    break;
            }
        }
    }
}