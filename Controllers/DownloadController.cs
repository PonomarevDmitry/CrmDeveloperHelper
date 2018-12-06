using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public class DownloadController
    {
        private IWriteToOutput _iWriteToOutput = null;

        public DownloadController(IWriteToOutput iWriteToOutput)
        {
            this._iWriteToOutput = iWriteToOutput;
        }

        #region Скачивание любого веб-ресурса.

        public async Task ExecuteDownloadCustomWebResources(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            string operation = string.Format(Properties.OperationNames.DownloadingWebResourceFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await DownloadCustomWebResources(connectionData, commonConfig, selection);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task DownloadCustomWebResources(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            WindowHelper.OpenExportWebResourcesWindow(this._iWriteToOutput, service, commonConfig, selection);
        }

        #endregion Скачивание любого веб-ресурса.

        #region Скачивание любого отчета.

        public async Task ExecuteDownloadCustomReport(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            string operation = string.Format(Properties.OperationNames.DownloadingReportFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(operation);

            try
            {
                await DownloadCustomReport(connectionData, commonConfig, selection);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(operation);
            }
        }

        private async Task DownloadCustomReport(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat1, service.CurrentServiceEndpoint);

            WindowHelper.OpenExportReportWindow(this._iWriteToOutput, service, commonConfig, selection);
        }

        #endregion Скачивание любого отчета.
    }
}
