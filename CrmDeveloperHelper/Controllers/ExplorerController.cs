using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
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
            await ConnectAndOpenExplorerAsync(connectionData
                , Properties.OperationNames.OpeningEntityMetadataExplorerFormat1
                , (service) => WindowHelper.OpenEntityMetadataExplorer(this._iWriteToOutput, service, commonConfig, selection, selectedItem)
            );
        }

        public async Task ExecuteOpeningEntityAttributeExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndOpenExplorerAsync(connectionData
                , Properties.OperationNames.OpeningEntityAttributeExplorerFormat1
                , (service) => WindowHelper.OpenEntityAttributeExplorer(this._iWriteToOutput, service, commonConfig, selection)
            );
        }

        public async Task ExecuteOpeningEntityKeyExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndOpenExplorerAsync(connectionData
                , Properties.OperationNames.OpeningEntityKeyExplorerFormat1
                , (service) => WindowHelper.OpenEntityKeyExplorer(this._iWriteToOutput, service, commonConfig, selection)
            );
        }

        public async Task ExecuteOpeningEntityRelationshipOneToManyExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndOpenExplorerAsync(connectionData
                , Properties.OperationNames.OpeningEntityRelationshipOneToManyFormat1
                , (service) => WindowHelper.OpenEntityRelationshipOneToManyExplorer(this._iWriteToOutput, service, commonConfig, selection)
            );
        }

        public async Task ExecuteOpeningEntityRelationshipManyToManyExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndOpenExplorerAsync(connectionData
                , Properties.OperationNames.OpeningEntityRelationshipManyToManyFormat1
                , (service) => WindowHelper.OpenEntityRelationshipManyToManyExplorer(this._iWriteToOutput, service, commonConfig, selection)
            );
        }

        public async Task ExecuteOpeningEntityPrivilegesExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndOpenExplorerAsync(connectionData
                , Properties.OperationNames.OpeningEntityPrivilegesExplorerFormat1
                , (service) => WindowHelper.OpenEntityPrivilegesExplorer(this._iWriteToOutput, service, commonConfig, selection)
            );
        }

        public async Task ExecuteOpeningOtherPrivilegesExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndOpenExplorerAsync(connectionData
                , Properties.OperationNames.OpeningOtherPrivilegesExplorerFormat1
                , (service) => WindowHelper.OpenOtherPrivilegesExplorer(this._iWriteToOutput, service, commonConfig, selection)
            );
        }

        #endregion Открытие Entity Explorers.

        #region Открытие Global OptionSet Explorer.

        public async Task ExecuteCreatingFileWithGlobalOptionSets(ConnectionData connectionData, CommonConfiguration commonConfig, string selection, EnvDTE.SelectedItem selectedItem)
        {
            await ConnectAndOpenExplorerAsync(connectionData
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

        #region Opening Global OptionSets Metadata File Generation Options

        public void ExecuteOpeningGlobalOptionSetsMetadataFileGenerationOptions()
        {
            GetFileGenerationOptionsAndOpenExplorer(
                Properties.OperationNames.OpeningGlobalOptionSetsFileGenerationOptions
                , WindowHelper.OpenGlobalOptionSetsFileGenerationOptions
            );
        }

        #endregion Opening Global OptionSets Metadata File Generation Options

        #region Скачивание любого веб-ресурса.

        public async Task ExecuteOpeningWebResourceExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndOpenExplorerAsync(connectionData
                , Properties.OperationNames.OpeningWebResourceExplorerFormat1
                , (service) => WindowHelper.OpenWebResourceExplorer(this._iWriteToOutput, service, commonConfig, selection)
            );
        }

        #endregion Скачивание любого веб-ресурса.

        #region Скачивание любого отчета.

        public async Task ExecuteOpeningReportExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndOpenExplorerAsync(connectionData
                , Properties.OperationNames.OpeningReportExplorerFormat1
                , (service) => WindowHelper.OpenReportExplorer(this._iWriteToOutput, service, commonConfig, selection)
            );
        }

        #endregion Скачивание любого отчета.

        #region Экспортирование SiteMap Xml.

        public async Task ExecuteOpeningSiteMapExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string filter)
        {
            await ConnectAndOpenExplorerAsync(connectionData
                , Properties.OperationNames.ExportingSiteMapXmlFormat1
                , (service) => WindowHelper.OpenExportSiteMapExplorer(this._iWriteToOutput, service, commonConfig, filter)
            );
        }

        #endregion Экспортирование SiteMap Xml.

        #region Экспортирование ApplicationRibbon.

        public async Task ExecuteOpeningApplicationRibbonExplorer(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            await ConnectAndOpenExplorerAsync(connectionData
                , Properties.OperationNames.ExportingRibbonXmlFormat1
                , (service) => WindowHelper.OpenApplicationRibbonExplorer(this._iWriteToOutput, service, commonConfig)
            );
        }

        #endregion Экспортирование Ribbon.

        #region Экспортирование System View (Saved Query) LayoutXml and FethcXml.

        public async Task ExecuteOpeningSystemSavedQueryExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndOpenExplorerAsync(connectionData
                , Properties.OperationNames.ExportingSystemSavedQueryXmlFormat1
                , (service) => WindowHelper.OpenSavedQueryExplorer(this._iWriteToOutput, service, commonConfig, string.Empty, selection)
            );
        }

        #endregion System View (Saved Query) LayoutXml and FethcXml.

        #region Экспортирование System Form FormXml.

        public async Task ExecuteOpeningSystemFormExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection, EnvDTE.SelectedItem selectedItem)
        {
            await ConnectAndOpenExplorerAsync(connectionData
                , Properties.OperationNames.ExportingSystemFormXmlFormat1
                , (service) => WindowHelper.OpenSystemFormExplorer(this._iWriteToOutput, service, commonConfig, string.Empty, selection, selectedItem)
            );
        }

        #endregion Экспортирование System Form FormXml.

        #region Экспортирование System Chart (Saved Query Visualization) Xml.

        public async Task ExecuteOpeningSystemSavedQueryVisualizationExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndOpenExplorerAsync(connectionData
                , Properties.OperationNames.ExportingSystemSavedQueryVisualizationXmlFormat1
                , (service) => WindowHelper.OpenSavedQueryVisualizationExplorer(this._iWriteToOutput, service, commonConfig, string.Empty, selection)
            );
        }

        #endregion System Chart (Saved Query Visualization) Xml.

        #region Экспортирование Workflow.

        public async Task ExecuteOpeningWorkflowExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndOpenExplorerAsync(connectionData
                , Properties.OperationNames.ExportingWorkflowFormat1
                , (service) => WindowHelper.OpenWorkflowExplorer(this._iWriteToOutput, service, commonConfig, string.Empty, selection)
            );
        }

        #endregion Экспортирование Workflow.

        #region Экспортирование информации об организации.

        public async Task ExecuteOpeningOrganizationExplorer(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            await ConnectAndOpenExplorerAsync(connectionData
                , Properties.OperationNames.ExportingOrganizationInformationFormat1
                , (service) => WindowHelper.OpenOrganizationExplorer(this._iWriteToOutput, service, commonConfig)
            );
        }

        #endregion Экспортирование информации об организации.

        #region Экспортирование CustomControl.

        public async Task ExecuteOpeningCustomControlExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndOpenExplorerAsync(connectionData
                , Properties.OperationNames.ExportingSystemFormXmlFormat1
                , (service) => WindowHelper.OpenCustomControlExplorer(this._iWriteToOutput, service, commonConfig, selection)
            );
        }

        #endregion Экспортирование CustomControl.

        #region Trace Reader.

        public async Task ExecuteOpeningTraceReader(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            await ConnectAndOpenExplorerAsync(connectionData
                , Properties.OperationNames.TraceReaderFormat1
                , (service) => WindowHelper.OpenTraceReaderExplorer(this._iWriteToOutput, service, commonConfig)
            );
        }

        #endregion Trace Reader.

        #region PluginType Explorer

        public async Task ExecuteOpeningPluginTypeExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndOpenExplorerAsync(connectionData
                , Properties.OperationNames.CreatingPluginTypeDescriptionFormat1
                , (service) => WindowHelper.OpenPluginTypeExplorer(this._iWriteToOutput, service, commonConfig, selection)
            );
        }

        #endregion PluginType Explorer

        #region PluginAssembly Explorer

        public async Task ExecuteOpeningPluginAssemblyExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndOpenExplorerAsync(connectionData
                , Properties.OperationNames.ExportingPluginAssemblyFormat1
                , (service) => WindowHelper.OpenPluginAssemblyExplorer(this._iWriteToOutput, service, commonConfig, selection)
            );
        }

        #endregion PluginAssembly Explorer

        #region Plugin Tree

        public async Task ExecuteShowingPluginTree(ConnectionData connectionData, CommonConfiguration commonConfig, string entityFilter, string pluginTypeFilter, string messageFilter)
        {
            await ConnectAndOpenExplorerAsync(connectionData
                , Properties.OperationNames.ShowingPluginTreeFormat1
                , (service) => WindowHelper.OpenPluginTreeExplorer(this._iWriteToOutput, service, commonConfig, entityFilter, pluginTypeFilter, messageFilter)
            );
        }

        #endregion Plugin Tree

        #region SdkMessage Tree

        public async Task ExecuteShowingSdkMessageTree(ConnectionData connectionData, CommonConfiguration commonConfig, string entityFilter, string messageFilter)
        {
            await ConnectAndOpenExplorerAsync(connectionData
                , Properties.OperationNames.ShowingSdkMessageTreeFormat1
                , (service) => WindowHelper.OpenSdkMessageTreeExplorer(this._iWriteToOutput, service, commonConfig, entityFilter, messageFilter)
            );
        }

        #endregion SdkMessage Tree

        #region SdkMessageRequest Tree

        public async Task ExecuteShowingSdkMessageRequestTree(ConnectionData connectionData, CommonConfiguration commonConfig, string entityFilter, string messageFilter, EnvDTE.SelectedItem selectedItem)
        {
            await ConnectAndOpenExplorerAsync(connectionData
                , Properties.OperationNames.ShowingSdkMessageRequestTreeFormat1
                , (service) => WindowHelper.OpenSdkMessageRequestTreeExplorer(this._iWriteToOutput, service, commonConfig, null, false, selectedItem, entityFilter, messageFilter)
            );
        }

        #endregion SdkMessageRequest Tree
    }
}