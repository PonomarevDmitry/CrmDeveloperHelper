using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public class ExplorerController : BaseController<IWriteToOutput>
    {
        public ExplorerController(IWriteToOutput iWriteToOutput)
            : base(iWriteToOutput)
        {
        }

        #region Открытие Entity Explorers.

        public async Task ExecuteOpeningEntityMetadataExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection, EnvDTE.SelectedItem selectedItem)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.OpeningEntityMetadataExplorerFormat1
                , (service) => WindowHelper.OpenEntityMetadataExplorer(this._iWriteToOutput, service, commonConfig, selection, selectedItem)
            );
        }

        public async Task ExecuteOpeningEntityAttributeExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.OpeningEntityAttributeExplorerFormat1
                , (service) => WindowHelper.OpenEntityAttributeExplorer(this._iWriteToOutput, service, commonConfig, selection)
            );
        }

        public async Task ExecuteOpeningEntityKeyExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.OpeningEntityKeyExplorerFormat1
                , (service) => WindowHelper.OpenEntityKeyExplorer(this._iWriteToOutput, service, commonConfig, selection)
            );
        }

        public async Task ExecuteOpeningEntityRelationshipOneToManyExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.OpeningEntityRelationshipOneToManyExplorerFormat1
                , (service) => WindowHelper.OpenEntityRelationshipOneToManyExplorer(this._iWriteToOutput, service, commonConfig, selection)
            );
        }

        public async Task ExecuteOpeningEntityRelationshipManyToManyExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.OpeningEntityRelationshipManyToManyExplorerFormat1
                , (service) => WindowHelper.OpenEntityRelationshipManyToManyExplorer(this._iWriteToOutput, service, commonConfig, selection)
            );
        }

        public async Task ExecuteOpeningEntityPrivilegesExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.OpeningEntityPrivilegesExplorerFormat1
                , (service) => WindowHelper.OpenEntityPrivilegesExplorer(this._iWriteToOutput, service, commonConfig, selection)
            );
        }

        public async Task ExecuteOpeningOtherPrivilegesExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.OpeningOtherPrivilegesExplorerFormat1
                , (service) => WindowHelper.OpenOtherPrivilegesExplorer(this._iWriteToOutput, service, commonConfig, selection)
            );
        }

        #endregion Открытие Entity Explorers.

        #region Открытие Global OptionSet Explorer.

        public async Task ExecuteCreatingFileWithGlobalOptionSets(ConnectionData connectionData, CommonConfiguration commonConfig, string selection, EnvDTE.SelectedItem selectedItem)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.OpeningGlobalOptionSetsExplorerFormat1
                , (service) => WindowHelper.OpenGlobalOptionSetsExplorer(this._iWriteToOutput, service, commonConfig, selection, selectedItem)
            );
        }

        #endregion Открытие Global OptionSet Explorer.

        #region Opening Entity Metadata File Generation Options

        public void ExecuteOpeningEntityMetadataFileGenerationOptions()
        {
            GetFileGenerationOptionsAndOpenExplorer(
                Properties.OperationNames.OpeningEntityMetadataFileGenerationOptions
                , WindowHelper.OpenEntityMetadataFileGenerationOptions
            );
        }

        #endregion Opening Entity Metadata File Generation Options

        #region Opening Entity Metadata File Generation Options

        public void ExecuteOpeningJavaScriptFileGenerationOptions()
        {
            GetFileGenerationOptionsAndOpenExplorer(
                Properties.OperationNames.OpeningJavaScriptFileGenerationOptions
                , WindowHelper.OpenJavaScriptFileGenerationOptions
            );
        }

        #endregion Opening Entity Metadata File Generation Options

        #region Opening Global OptionSets Metadata File Generation Options

        public void ExecuteOpeningGlobalOptionSetsMetadataFileGenerationOptions()
        {
            GetFileGenerationOptionsAndOpenExplorer(
                Properties.OperationNames.OpeningGlobalOptionSetsFileGenerationOptions
                , WindowHelper.OpenGlobalOptionSetsFileGenerationOptions
            );
        }

        #endregion Opening Global OptionSets Metadata File Generation Options

        public void ExecuteOpeningFileGenerationOptions()
        {
            GetFileGenerationOptionsAndOpenExplorer(
                Properties.OperationNames.OpeningFileGenerationOptions
                , WindowHelper.OpenFileGenerationOptions
            );
        }

        public void ExecuteOpeningFileGenerationConfiguration()
        {
            GetFileGenerationConfigurationAndOpenExplorer(
                Properties.OperationNames.OpeningFileGenerationConfiguration
                , WindowHelper.OpenFileGenerationConfiguration
            );
        }

        #region Скачивание любого веб-ресурса.

        public async Task ExecuteOpeningWebResourceExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.OpeningWebResourceExplorerFormat1
                , (service) => WindowHelper.OpenWebResourceExplorer(this._iWriteToOutput, service, commonConfig, selection)
            );
        }

        #endregion Скачивание любого веб-ресурса.

        #region Скачивание любого отчета.

        public async Task ExecuteOpeningReportExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.OpeningReportExplorerFormat1
                , (service) => WindowHelper.OpenReportExplorer(this._iWriteToOutput, service, commonConfig, selection)
            );
        }

        #endregion Скачивание любого отчета.

        #region Экспортирование SiteMap Xml.

        public async Task ExecuteOpeningSiteMapExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string filter)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.ExportingSiteMapXmlFormat1
                , (service) => WindowHelper.OpenExportSiteMapExplorer(this._iWriteToOutput, service, commonConfig, filter)
            );
        }

        #endregion Экспортирование SiteMap Xml.

        #region Экспортирование ApplicationRibbon.

        public async Task ExecuteOpeningApplicationRibbonExplorer(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.ExportingRibbonXmlFormat1
                , (service) => WindowHelper.OpenApplicationRibbonExplorer(this._iWriteToOutput, service, commonConfig)
            );
        }

        #endregion Экспортирование Ribbon.

        #region Экспортирование System View (Saved Query) LayoutXml and FethcXml.

        public async Task ExecuteOpeningSystemSavedQueryExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.ExportingSystemSavedQueryXmlFormat1
                , (service) => WindowHelper.OpenSavedQueryExplorer(this._iWriteToOutput, service, commonConfig, string.Empty, selection)
            );
        }

        #endregion System View (Saved Query) LayoutXml and FethcXml.

        #region Экспортирование System Form FormXml.

        public async Task ExecuteOpeningSystemFormExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string entityName, string selection, EnvDTE.SelectedItem selectedItem)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.ExportingSystemFormXmlFormat1
                , (service) => WindowHelper.OpenSystemFormExplorer(this._iWriteToOutput, service, commonConfig, entityName, selection, selectedItem)
            );
        }

        #endregion Экспортирование System Form FormXml.

        #region Экспортирование System Chart (Saved Query Visualization) Xml.

        public async Task ExecuteOpeningSystemSavedQueryVisualizationExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.ExportingSystemSavedQueryVisualizationXmlFormat1
                , (service) => WindowHelper.OpenSavedQueryVisualizationExplorer(this._iWriteToOutput, service, commonConfig, string.Empty, selection)
            );
        }

        #endregion System Chart (Saved Query Visualization) Xml.

        #region Экспортирование Workflow.

        public async Task ExecuteOpeningWorkflowExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.ExportingWorkflowFormat1
                , (service) => WindowHelper.OpenWorkflowExplorer(this._iWriteToOutput, service, commonConfig, string.Empty, selection)
            );
        }

        #endregion Экспортирование Workflow.

        #region Экспортирование информации об организации.

        public async Task ExecuteOpeningOrganizationExplorer(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.ExportingOrganizationInformationFormat1
                , (service) => WindowHelper.OpenOrganizationExplorer(this._iWriteToOutput, service, commonConfig)
            );
        }

        #endregion Экспортирование информации об организации.

        #region Экспортирование CustomControl.

        public async Task ExecuteOpeningCustomControlExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.ExportingSystemFormXmlFormat1
                , (service) => WindowHelper.OpenCustomControlExplorer(this._iWriteToOutput, service, commonConfig, selection)
            );
        }

        #endregion Экспортирование CustomControl.

        #region Trace Reader.

        public async Task ExecuteOpeningTraceReader(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.TraceReaderFormat1
                , (service) => WindowHelper.OpenTraceReaderExplorer(this._iWriteToOutput, service, commonConfig)
            );
        }

        #endregion Trace Reader.

        #region PluginType Explorer

        public async Task ExecuteOpeningPluginTypeExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.CreatingPluginTypeDescriptionFormat1
                , (service) => WindowHelper.OpenPluginTypeExplorer(this._iWriteToOutput, service, commonConfig, selection)
            );
        }

        #endregion PluginType Explorer

        #region PluginAssembly Explorer

        public async Task ExecuteOpeningPluginAssemblyExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.ExportingPluginAssemblyFormat1
                , (service) => WindowHelper.OpenPluginAssemblyExplorer(this._iWriteToOutput, service, commonConfig, selection)
            );
        }

        #endregion PluginAssembly Explorer

        #region Plugin Tree

        public async Task ExecuteShowingPluginTree(ConnectionData connectionData, CommonConfiguration commonConfig, string entityFilter, string pluginTypeFilter, string messageFilter)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.ShowingPluginTreeFormat1
                , (service) => WindowHelper.OpenPluginTree(this._iWriteToOutput, service, commonConfig, entityFilter, pluginTypeFilter, messageFilter)
            );
        }

        #endregion Plugin Tree

        #region Plugin Steps Explorer

        public async Task ExecuteShowingPluginStepsExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string entityFilter, string pluginTypeFilter, string messageFilter)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.ShowingPluginStepsExplorerFormat1
                , (service) => WindowHelper.OpenSdkMessageProcessingStepExplorer(this._iWriteToOutput, service, commonConfig, entityFilter, pluginTypeFilter, messageFilter)
            );
        }

        #endregion Plugin Steps Explorer

        #region SdkMessage Explorer

        public async Task ExecuteShowingSdkMessageExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string messageFilter)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.ShowingSdkMessageExplorerFormat1
                , (service) => WindowHelper.OpenSdkMessageExplorer(this._iWriteToOutput, service, commonConfig, messageFilter)
            );
        }

        #endregion SdkMessage Explorer

        #region SdkMessageFilter Explorer

        public async Task ExecuteShowingSdkMessageFilterExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string messageFilter)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.ShowingSdkMessageFilterExplorerFormat1
                , (service) => WindowHelper.OpenSdkMessageFilterExplorer(this._iWriteToOutput, service, commonConfig, null, messageFilter)
            );
        }

        #endregion SdkMessageFilter Explorer

        #region SdkMessageFilter Tree

        public async Task ExecuteShowingSdkMessageFilterTree(ConnectionData connectionData, CommonConfiguration commonConfig, string entityFilter, string messageFilter)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.ShowingSdkMessageFilterTreeFormat1
                , (service) => WindowHelper.OpenSdkMessageFilterTree(this._iWriteToOutput, service, commonConfig, entityFilter, messageFilter)
            );
        }

        #endregion SdkMessageFilter Tree

        #region SdkMessageRequest Tree

        public async Task ExecuteShowingSdkMessageRequestTree(ConnectionData connectionData, CommonConfiguration commonConfig, string entityFilter, string messageFilter, EnvDTE.SelectedItem selectedItem)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.ShowingSdkMessageRequestTreeFormat1
                , (service) => WindowHelper.OpenSdkMessageRequestTree(this._iWriteToOutput, service, commonConfig, null, false, selectedItem, entityFilter, messageFilter)
            );
        }

        #endregion SdkMessageRequest Tree

        #region Solution Explorer

        public async Task ExecuteOpeningSolutionExlorerWindow(ConnectionData connectionData, CommonConfiguration commonConfig, EnvDTE.SelectedItem selectedItem)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.SolutionExplorerFormat1
                , (service) => WindowHelper.OpenExplorerSolutionExplorer(this._iWriteToOutput, service, commonConfig, null, null, selectedItem)
            );
        }

        #endregion Solution Explorer

        #region ImportJob Explorer

        public async Task ExecuteOpeningImportJobExlorerWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.ImportJobExplorerFormat1
                , (service) => WindowHelper.OpenImportJobExplorer(this._iWriteToOutput, service, commonConfig, null)
            );
        }

        #endregion ImportJob Explorer

        #region SolutionImage Window

        public void ExecuteOpeningSolutionImageWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.ShowingSolutionImageWindowFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                OpeningSolutionImageWindow(connectionData, commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        private void OpeningSolutionImageWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            WindowHelper.OpenSolutionImageWindow(this._iWriteToOutput, connectionData, commonConfig);
        }

        #endregion SolutionImage Window

        #region SolutionDifferenceImage Window

        public void ExecuteOpeningSolutionDifferenceImageWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, Properties.OperationNames.ShowingSolutionDifferenceImageWindow);

            try
            {
                OpeningSolutionDifferenceImageWindow(connectionData, commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, Properties.OperationNames.ShowingSolutionDifferenceImageWindow);
            }
        }

        private void OpeningSolutionDifferenceImageWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            WindowHelper.OpenSolutionDifferenceImageWindow(this._iWriteToOutput, connectionData, commonConfig);
        }

        #endregion SolutionDifferenceImage Window

        #region OrganizationDifferenceImage Window

        public void ExecuteOpeningOrganizationDifferenceImageWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, Properties.OperationNames.ShowingOrganizationDifferenceImageWindow);

            try
            {
                OpeningOrganizationDifferenceImageWindow(connectionData, commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, Properties.OperationNames.ShowingOrganizationDifferenceImageWindow);
            }
        }

        private void OpeningOrganizationDifferenceImageWindow(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            WindowHelper.OpenOrganizationDifferenceImageWindow(this._iWriteToOutput, connectionData, commonConfig);
        }

        #endregion OrganizationDifferenceImage Window

        #region Organization Comparer

        public void ExecuteOrganizationComparer(ConnectionConfiguration crmConfig, CommonConfiguration commonConfig)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(null, Properties.OperationNames.ShowingOrganizationComparer);

            try
            {
                WindowHelper.OpenOrganizationComparerWindow(this._iWriteToOutput, crmConfig, commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(null, Properties.OperationNames.ShowingOrganizationComparer);
            }
        }

        #endregion Organization Comparer
    }
}
