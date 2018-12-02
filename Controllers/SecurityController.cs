using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public class SecurityController
    {
        private readonly IWriteToOutput _iWriteToOutput = null;

        /// <summary>
        /// Конструктор контроллера
        /// </summary>
        /// <param name="outputWindow"></param>
        public SecurityController(IWriteToOutput outputWindow)
        {
            this._iWriteToOutput = outputWindow;
        }

        #region Открытие Explorers.

        public async Task ExecuteShowingSystemUserExplorer(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.OpeningSystemUsersExplorer);

            try
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

                WindowHelper.OpenSystemUsersExplorer(this._iWriteToOutput, service, commonConfig, null, selection);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.OpeningSystemUsersExplorer);
            }
        }
        
        public async Task ExecuteShowingTeamsExplorer(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.OpeningTeamsExplorer);

            try
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

                WindowHelper.OpenTeamsExplorer(this._iWriteToOutput, service, commonConfig, null, selection);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.OpeningTeamsExplorer);
            }
        }

        public async Task ExecuteShowingSecurityRolesExplorer(string selection, ConnectionData connectionData, CommonConfiguration commonConfig)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(Properties.OperationNames.OpeningSecurityRolesExplorer);

            try
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

                WindowHelper.OpenRolesExplorer(this._iWriteToOutput, service, commonConfig, null, selection);
            }
            catch (Exception xE)
            {
                this._iWriteToOutput.WriteErrorToOutput(xE);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(Properties.OperationNames.OpeningSecurityRolesExplorer);
            }
        }

        #endregion Открытие Explorers.
    }
}