using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public class ExplorerController
    {
        private readonly IWriteToOutput _iWriteToOutput = null;

        public ExplorerController(IWriteToOutput iWriteToOutput)
        {
            this._iWriteToOutput = iWriteToOutput;
        }

        #region Скачивание любого веб-ресурса.

        public async Task ExecuteOpeningWebResourceExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            string operation = string.Format(Properties.OperationNames.OpeningWebResourceExplorerFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await OpeningWebResourceExplorer(connectionData, commonConfig, selection);
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

        private async Task OpeningWebResourceExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            WindowHelper.OpenWebResourceExplorerWindow(this._iWriteToOutput, service, commonConfig, selection);
        }

        #endregion Скачивание любого веб-ресурса.

        #region Скачивание любого отчета.

        public async Task ExecuteOpeningReportExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            string operation = string.Format(Properties.OperationNames.OpeningReportExplorerFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await OpeningReportExplorer(connectionData, commonConfig, selection);
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

        private async Task OpeningReportExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            WindowHelper.OpenReportExplorerWindow(this._iWriteToOutput, service, commonConfig, selection);
        }

        #endregion Скачивание любого отчета.

        #region Экспортирование Sitemap Xml.

        public async Task ExecuteOpeningSitemapExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string filter)
        {
            string operation = string.Format(Properties.OperationNames.ExportingSitemapXmlFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await OpeningSitemapExplorer(connectionData, commonConfig, filter);
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

        private async Task OpeningSitemapExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string filter)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            WindowHelper.OpenExportSiteMapWindow(this._iWriteToOutput, service, commonConfig, filter);
        }

        #endregion Экспортирование Sitemap Xml.

        #region Экспортирование ApplicationRibbon.

        public async Task ExecuteOpeningApplicationRibbonExplorer(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.ExportingRibbonXmlFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await OpeningApplicationRibbonExplorer(selection, connectionData, commonConfig);
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

        private async Task OpeningApplicationRibbonExplorer(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            WindowHelper.OpenApplicationRibbonWindow(this._iWriteToOutput, service, commonConfig);
        }

        #endregion Экспортирование Ribbon.

        #region Экспортирование System View (Saved Query) LayoutXml and FethcXml.

        public async Task ExecuteOpeningSystemSavedQueryExplorer(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.ExportingSystemSavedQueryXmlFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await OpeningSystemSavedQueryExplorer(selection, connectionData, commonConfig);
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

        private async Task OpeningSystemSavedQueryExplorer(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            WindowHelper.OpenSavedQueryWindow(this._iWriteToOutput, service, commonConfig, string.Empty, selection);
        }

        #endregion System View (Saved Query) LayoutXml and FethcXml.

        #region Экспортирование System Form FormXml.

        public async Task ExecuteOpeningSystemFormExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection, EnvDTE.SelectedItem selectedItem)
        {
            string operation = string.Format(Properties.OperationNames.ExportingSystemFormXmlFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await OpeningSystemFormExplorer(connectionData, commonConfig, selection, selectedItem);
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

        private async Task OpeningSystemFormExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection, EnvDTE.SelectedItem selectedItem)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            WindowHelper.OpenSystemFormWindow(this._iWriteToOutput, service, commonConfig, string.Empty, selection, selectedItem);
        }

        #endregion Экспортирование System Form FormXml.

        #region Экспортирование System Chart (Saved Query Visualization) Xml.

        public async Task ExecuteOpeningSystemSavedQueryVisualizationExplorer(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.ExportingSystemSavedQueryVisualizationXmlFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await OpeningSystemSavedQueryVisualizationExplorer(selection, connectionData, commonConfig);
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

        private async Task OpeningSystemSavedQueryVisualizationExplorer(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            WindowHelper.OpenSavedQueryVisualizationWindow(this._iWriteToOutput, service, commonConfig, string.Empty, selection);
        }

        #endregion System Chart (Saved Query Visualization) Xml.

        #region Экспортирование Workflow.

        public async Task ExecuteOpeningWorkflowExplorer(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.ExportingWorkflowFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await OpeningWorkflowExplorer(selection, connectionData, commonConfig);
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

        private async Task OpeningWorkflowExplorer(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            WindowHelper.OpenWorkflowWindow(this._iWriteToOutput, service, commonConfig, string.Empty, selection);
        }

        #endregion Экспортирование Workflow.

        #region Экспортирование информации об организации.

        public async Task ExecuteOpeningOrganizationExplorer(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.ExportingOrganizationInformationFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await OpeningOrganizationExplorer(connectionData, commonConfig);
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

        private async Task OpeningOrganizationExplorer(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            WindowHelper.OpenOrganizationExplorer(this._iWriteToOutput, service, commonConfig);
        }

        #endregion Экспортирование информации об организации.

        #region Экспортирование CustomControl.

        public async Task ExecuteOpeningCustomControlExplorer(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.ExportingSystemFormXmlFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await OpeningCustomControlExplorer(selection, connectionData, commonConfig);
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

        private async Task OpeningCustomControlExplorer(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            WindowHelper.OpenCustomControlWindow(this._iWriteToOutput, service, commonConfig, selection);
        }

        #endregion Экспортирование CustomControl.

        #region Trace Reader.

        public async Task ExecuteOpeningTraceReader(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            string operation = string.Format(Properties.OperationNames.TraceReaderFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await OpeningTraceReader(connectionData, commonConfig);
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

        private async Task OpeningTraceReader(ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            WindowHelper.OpenTraceReaderWindow(this._iWriteToOutput, service, commonConfig);
        }

        #endregion Trace Reader.

        #region PluginType Explorer

        public async Task ExecuteOpeningPluginTypeExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            string operation = string.Format(Properties.OperationNames.CreatingPluginTypeDescriptionFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await OpeningPluginTypeExplorer(connectionData, commonConfig, selection);
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

        private async Task OpeningPluginTypeExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            WindowHelper.OpenPluginTypeWindow(
                this._iWriteToOutput
                , service
                , commonConfig
                , selection
            );
        }

        #endregion PluginType Explorer

        #region PluginAssembly Explorer

        public async Task ExecuteOpeningPluginAssemblyExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            string operation = string.Format(Properties.OperationNames.ExportingPluginAssemblyFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await OpeningPluginAssemblyExplorer(connectionData, commonConfig, selection);
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

        private async Task OpeningPluginAssemblyExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            WindowHelper.OpenPluginAssemblyExplorer(
                this._iWriteToOutput
                , service
                , commonConfig
                , selection
            );
        }

        #endregion PluginAssembly Explorer

        #region Plugin Tree

        public async Task ExecuteShowingPluginTree(ConnectionData connectionData, CommonConfiguration commonConfig, string entityFilter, string pluginTypeFilter, string messageFilter)
        {
            string operation = string.Format(Properties.OperationNames.ShowingPluginTreeFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await ShowingPluginTree(connectionData, commonConfig, entityFilter, pluginTypeFilter, messageFilter);
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

        private async Task ShowingPluginTree(ConnectionData connectionData, CommonConfiguration commonConfig, string entityFilter, string pluginTypeFilter, string messageFilter)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            WindowHelper.OpenPluginTreeWindow(this._iWriteToOutput, service, commonConfig, entityFilter, pluginTypeFilter, messageFilter);
        }

        #endregion Plugin Tree

        #region SdkMessage Tree

        public async Task ExecuteShowingSdkMessageTree(ConnectionData connectionData, CommonConfiguration commonConfig, string entityFilter, string messageFilter)
        {
            string operation = string.Format(Properties.OperationNames.ShowingSdkMessageTreeFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await ShowingSdkMessageTree(connectionData, commonConfig, entityFilter, messageFilter);
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

        private async Task ShowingSdkMessageTree(ConnectionData connectionData, CommonConfiguration commonConfig, string entityFilter, string messageFilter)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            WindowHelper.OpenSdkMessageTreeWindow(this._iWriteToOutput, service, commonConfig, entityFilter, messageFilter);
        }

        #endregion SdkMessage Tree

        #region SdkMessageRequest Tree

        public async Task ExecuteShowingSdkMessageRequestTree(ConnectionData connectionData, CommonConfiguration commonConfig, string entityFilter, string messageFilter, EnvDTE.SelectedItem selectedItem)
        {
            string operation = string.Format(Properties.OperationNames.ShowingSdkMessageRequestTreeFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                await ShowingSdkMessageRequestTree(connectionData, commonConfig, entityFilter, messageFilter, selectedItem);
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

        private async Task ShowingSdkMessageRequestTree(ConnectionData connectionData, CommonConfiguration commonConfig, string entityFilter, string messageFilter, EnvDTE.SelectedItem selectedItem)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData, connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            if (service == null)
            {
                _iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionFailedFormat1, connectionData.Name);
                return;
            }

            this._iWriteToOutput.WriteToOutput(connectionData, Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            WindowHelper.OpenSdkMessageRequestTreeWindow(this._iWriteToOutput, service, commonConfig, null, false, selectedItem, entityFilter, messageFilter);
        }

        #endregion SdkMessageRequest Tree
    }
}