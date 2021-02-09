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
            var thread = new Thread(() =>
            {
                try
                {
                    var form = windowGetter();

                    form.ShowDialog();

                    form.Close();
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToOutput(connectionData, ex);
                }
            });

            thread.SetApartmentState(ApartmentState.STA);

            thread.Start();
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

        public static void OpenJavaScriptFileGenerationOptions(FileGenerationOptions fileGenerationOptions)
        {
            ExecuteInSTAThread(() =>
                new WindowFileGenerationJavaScriptOptions(fileGenerationOptions)
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
                    , commonConfig
                    , service
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
                    , commonConfig
                    , service
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
                    , commonConfig
                    , service
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
                    , commonConfig
                    , service
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
                    , commonConfig
                    , service
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
                    , commonConfig
                    , service
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
        )
        {
            OpenEntityEditorInternal(iWriteToOutput, service, commonConfig, entityName, null, null);
        }

        public static void OpenEntityEditor(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string entityName
            , Entity entity
        )
        {
            OpenEntityEditorInternal(iWriteToOutput, service, commonConfig, entityName, null, entity);
        }

        public static void OpenEntityEditor(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string entityName
            , Guid entityId
        )
        {
            OpenEntityEditorInternal(iWriteToOutput, service, commonConfig, entityName, entityId, null);
        }

        private static void OpenEntityEditorInternal(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string entityName
            , Guid? entityId
            , Entity entity
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowEntityEditor
                (
                    iWriteToOutput
                    , commonConfig
                    , service
                    , entityName
                    , entityId
                    , entity
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
                    , commonConfig
                    , service
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
                    , commonConfig
                    , service
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
                    , commonConfig
                    , service
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
                    , commonConfig
                    , service
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
                    , commonConfig
                    , service
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
                    , commonConfig
                    , service
                    , filterEntityName
                    , selection
                )
            );
        }

        public static void OpenWorkflowExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
        )
        {
            OpenWorkflowExplorer(iWriteToOutput, service, commonConfig, null, null, null);
        }

        public static void OpenWorkflowExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntityName
        )
        {
            OpenWorkflowExplorer(iWriteToOutput, service, commonConfig, filterEntityName, null, null);
        }

        public static void OpenWorkflowExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntityName
            , string selection
        )
        {
            OpenWorkflowExplorer(iWriteToOutput, service, commonConfig, filterEntityName, selection, null);
        }

        public static void OpenWorkflowExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string filterEntityName
            , string selection
            , IEnumerable<Guid> selectedWorkflows
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerWorkflow
                (
                    iWriteToOutput
                    , commonConfig
                    , service
                    , filterEntityName
                    , selection
                    , selectedWorkflows
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
                    , commonConfig
                    , service
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
                    , commonConfig
                    , service
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
                    , commonConfig
                    , service
                    , entityFilter
                    , pluginTypeFilter
                    , messageFilter
                )
            );
        }

        public static void OpenSdkMessageProcessingStepExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string entityFilter = null
            , string pluginTypeFilter = null
            , string messageFilter = null
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerSdkMessageProcessingStep
                (
                    iWriteToOutput
                    , commonConfig
                    , service
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
            , string messageFilter
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerSdkMessage
                (
                    iWriteToOutput
                    , commonConfig
                    , service
                    , messageFilter
                )
            );
        }

        public static void OpenSdkMessageFilterExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
        )
        {
            OpenSdkMessageFilterExplorer(iWriteToOutput, service, commonConfig, null, null);
        }

        public static void OpenSdkMessageFilterExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , string entityFilter
            , string messageFilter
        )
        {
            ExecuteWithConnectionInSTAThread(service.ConnectionData, () =>
                new WindowExplorerSdkMessageFilter
                (
                    iWriteToOutput
                    , commonConfig
                    , service
                    , entityFilter
                    , messageFilter
                )
            );
        }

        public static void OpenSdkMessageFilterTree(
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
                    , commonConfig
                    , service
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
                    , commonConfig
                    , service

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
                    , commonConfig
                    , service
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
                    , commonConfig
                    , service
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
                    , commonConfig
                    , service
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
                    , commonConfig
                    , service
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
                    , commonConfig
                    , service
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
                new WindowExplorerSolutionComponents(iWriteToOutput, commonConfig, service, descriptor, solutionUniqueName, selection)
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
                new WindowExplorerComponents(iWriteToOutput, commonConfig, service, descriptor, solutionComponents, solutionUniqueName, header, selection)
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
                    , commonConfig
                    , service
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
                    , commonConfig
                    , service
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
                    , commonConfig
                    , service
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
                    , commonConfig
                    , service
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
                    , commonConfig
                    , service
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
                    , commonConfig
                    , service
                    , selection
                )
            );
        }

        public static void OpenPluginAssemblyUpdateWindow(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , PluginAssembly assembly
            , EnvDTE.Project project
        )
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
                    , commonConfig
                    , service
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

        #region Compare Organizations

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
            , string filterEntityName = null
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
                    , filterEntityName
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
            , string filter = null
        )
        {
            ExecuteInSTAThread(() =>
                new WindowOrganizationComparerSiteMap
                (
                    iWriteToOutput
                    , commonConfig
                    , connection1
                    , connection2
                    , filter
                )
            );
        }

        public static void OpenOrganizationComparerWebResourcesWindow(
            IWriteToOutput iWriteToOutput
            , CommonConfiguration commonConfig
            , ConnectionData connection1
            , ConnectionData connection2
            , string filter
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

        #endregion Compare Organizations

        public static void OpenCrmConnectionCard(
            IWriteToOutput iWriteToOutput
            , ConnectionData connectionData
        )
        {
            var thread = new Thread(() =>
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

            thread.SetApartmentState(ApartmentState.STA);

            thread.Start();
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

        public static bool HasExplorer(int? componentType)
        {
            if (!SolutionComponent.IsDefinedComponentType(componentType))
            {
                return false;
            }

            return WindowHelper.IsDefinedExplorer((ComponentType)componentType);
        }

        public static void OpenComponentExplorer(
            IWriteToOutput iWriteToOutput
            , IOrganizationServiceExtented service
            , CommonConfiguration commonConfig
            , SolutionComponentDescriptor descriptor
            , ComponentType componentType
            , Guid objectId
        )
        {
            switch (componentType)
            {
                case ComponentType.Entity:
                    {
                        var entityMetadata = descriptor.MetadataSource.GetEntityMetadata(objectId);

                        OpenEntityMetadataExplorer(iWriteToOutput, service, commonConfig, entityMetadata.LogicalName);
                    }
                    break;

                case ComponentType.Attribute:
                    {
                        var attributeMetadata = descriptor.MetadataSource.GetAttributeMetadata(objectId);

                        OpenEntityAttributeExplorer(iWriteToOutput, service, commonConfig, attributeMetadata.EntityLogicalName);
                    }
                    break;

                case ComponentType.OptionSet:
                    {
                        var globalOptionSetName = descriptor.GetName(new SolutionComponent()
                        {
                            ComponentTypeEnum = Entities.GlobalOptionSets.componenttype.Option_Set_9,
                            ObjectId = objectId,
                        });

                        OpenGlobalOptionSetsExplorer(iWriteToOutput, service, commonConfig, globalOptionSetName);
                    }
                    break;

                case ComponentType.EntityRelationship:
                    {
                        var relationMetadata = descriptor.MetadataSource.GetRelationshipMetadata(objectId);

                        if (relationMetadata is OneToManyRelationshipMetadata oneToManyRelationshipMetadata)
                        {
                            OpenEntityRelationshipOneToManyExplorer(iWriteToOutput, service, commonConfig, oneToManyRelationshipMetadata.ReferencedEntity);
                        }
                        else if (relationMetadata is ManyToManyRelationshipMetadata manyToManyRelationshipMetadata)
                        {
                            OpenEntityRelationshipManyToManyExplorer(iWriteToOutput, service, commonConfig, manyToManyRelationshipMetadata.Entity1LogicalName);
                        }
                    }
                    break;

                case ComponentType.EntityKey:
                    {
                        var entityKeyMetatadata = descriptor.MetadataSource.GetEntityKeyMetadata(objectId);

                        OpenEntityKeyExplorer(iWriteToOutput, service, commonConfig, entityKeyMetatadata.EntityLogicalName);
                    }
                    break;

                case ComponentType.Role:
                    {
                        var role = descriptor.GetEntity<Role>((int)ComponentType.Role, objectId);

                        OpenRolesExplorer(iWriteToOutput, service, commonConfig, role.Name, null, null);
                    }
                    break;

                case ComponentType.Organization:
                    OpenOrganizationExplorer(iWriteToOutput, service, commonConfig);
                    break;

                case ComponentType.SavedQuery:
                    {
                        var savedQuery = descriptor.GetEntity<SavedQuery>((int)ComponentType.SavedQuery, objectId);

                        OpenSavedQueryExplorer(iWriteToOutput, service, commonConfig, savedQuery.ReturnedTypeCode, savedQuery.Id.ToString());
                    }
                    break;

                case ComponentType.SavedQueryVisualization:
                    {
                        var savedQueryVisualization = descriptor.GetEntity<SavedQueryVisualization>((int)ComponentType.SavedQueryVisualization, objectId);

                        OpenSavedQueryVisualizationExplorer(iWriteToOutput, service, commonConfig, savedQueryVisualization.PrimaryEntityTypeCode, savedQueryVisualization.Id.ToString());
                    }
                    break;

                case ComponentType.SystemForm:
                    {
                        var systemForm = descriptor.GetEntity<SystemForm>((int)ComponentType.SystemForm, objectId);

                        OpenSystemFormExplorer(iWriteToOutput, service, commonConfig, systemForm.ObjectTypeCode, systemForm.Id.ToString());
                    }
                    break;

                case ComponentType.Workflow:
                    {
                        var workflow = descriptor.GetEntity<Workflow>((int)ComponentType.Workflow, objectId);

                        OpenWorkflowExplorer(iWriteToOutput, service, commonConfig, workflow.PrimaryEntity, workflow.Id.ToString());
                    }
                    break;

                case ComponentType.Report:
                    {
                        var report = descriptor.GetEntity<Report>((int)ComponentType.Report, objectId);

                        OpenReportExplorer(iWriteToOutput, service, commonConfig, report.Id.ToString());
                    }
                    break;

                case ComponentType.WebResource:
                    {
                        var webResource = descriptor.GetEntity<WebResource>((int)ComponentType.WebResource, objectId);

                        OpenWebResourceExplorer(iWriteToOutput, service, commonConfig, webResource.Name);
                    }
                    break;

                case ComponentType.SiteMap:
                    OpenExportSiteMapExplorer(iWriteToOutput, service, commonConfig);
                    break;

                case ComponentType.PluginType:
                    {
                        var pluginType = descriptor.GetEntity<PluginType>((int)ComponentType.PluginType, objectId);

                        OpenPluginTypeExplorer(iWriteToOutput, service, commonConfig, pluginType.TypeName);
                    }
                    break;

                case ComponentType.PluginAssembly:
                    {
                        var pluginAssembly = descriptor.GetEntity<PluginAssembly>((int)ComponentType.PluginAssembly, objectId);

                        OpenPluginAssemblyExplorer(iWriteToOutput, service, commonConfig, pluginAssembly.Name);
                    }
                    break;

                case ComponentType.SdkMessageProcessingStep:
                case ComponentType.SdkMessageProcessingStepImage:
                    OpenPluginTree(iWriteToOutput, service, commonConfig);
                    break;

                case ComponentType.SdkMessage:
                    {
                        var sdkMessage = descriptor.GetEntity<SdkMessage>((int)ComponentType.SdkMessage, objectId);

                        OpenSdkMessageExplorer(iWriteToOutput, service, commonConfig, sdkMessage.Name);
                    }
                    break;

                case ComponentType.SdkMessageFilter:
                    {
                        var sdkMessageFilter = descriptor.GetEntity<SdkMessageFilter>((int)ComponentType.SdkMessageFilter, objectId);

                        OpenSdkMessageFilterExplorer(iWriteToOutput, service, commonConfig, sdkMessageFilter.PrimaryObjectTypeCode, sdkMessageFilter.SdkMessageId?.Name ?? sdkMessageFilter.MessageName);
                    }
                    break;

                case ComponentType.SdkMessagePair:
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
                    {
                        var customControl = descriptor.GetEntity<CustomControl>((int)ComponentType.CustomControl, objectId);

                        OpenCustomControlExplorer(iWriteToOutput, service, commonConfig, customControl.Id.ToString());
                    }
                    break;
            }
        }
    }
}