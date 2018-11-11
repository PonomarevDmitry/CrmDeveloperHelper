using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public class ExportSolutionController
    {
        private IWriteToOutput _iWriteToOutput = null;

        /// <summary>
        /// Конструктор контроллера
        /// </summary>
        /// <param name="iWriteToOutput"></param>
        public ExportSolutionController(IWriteToOutput iWriteToOutput)
        {
            this._iWriteToOutput = iWriteToOutput;
        }

        public async Task ExecuteExportingSolution(EnvDTE.SelectedItem selectedItem, string filter, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.ExportingCRMSolution);

            try
            {
                await ExportingSolution(selectedItem, filter, connectionData, commonConfig);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.ExportingCRMSolution);
            }
        }

        private async Task ExportingSolution(EnvDTE.SelectedItem selectedItem, string filter, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            if (connectionData == null)
            {
                this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.NoCurrentCRMConnection);
                return;
            }

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.ConnectingToCRM);;

            this._iWriteToOutput.WriteToOutput(connectionData.GetConnectionDescription());

            // Подключаемся к CRM.
            var service = await QuickConnection.ConnectAsync(connectionData);

            this._iWriteToOutput.WriteToOutput(Properties.OutputStrings.CurrentServiceEndpointFormat, service.CurrentServiceEndpoint);

            WindowHelper.OpenExportSolutionWindow(
                this._iWriteToOutput
                , service
                , commonConfig
                , selectedItem
                , filter
                );
        }
    }
}
