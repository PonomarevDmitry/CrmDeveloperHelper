using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public class SecurityController : BaseController<IWriteToOutput>
    {
        /// <summary>
        /// Конструктор контроллера
        /// </summary>
        /// <param name="iWriteToOutput"></param>
        public SecurityController(IWriteToOutput iWriteToOutput)
            : base(iWriteToOutput)
        {
        }

        #region Открытие Explorers.

        public async Task ExecuteShowingSystemUserExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndOpenExplorerAsync(connectionData
                , Properties.OperationNames.OpeningSystemUsersExplorerFormat1
                , (service) => WindowHelper.OpenSystemUsersExplorer(this._iWriteToOutput, service, commonConfig, selection)
            );
        }

        public async Task ExecuteShowingTeamsExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndOpenExplorerAsync(connectionData
                , Properties.OperationNames.OpeningTeamsExplorerFormat1
                , (service) => WindowHelper.OpenTeamsExplorer(this._iWriteToOutput, service, commonConfig, selection)
            );
        }

        public async Task ExecuteShowingSecurityRolesExplorer(ConnectionData connectionData, CommonConfiguration commonConfig, string selection)
        {
            await ConnectAndOpenExplorerAsync(connectionData
                , Properties.OperationNames.OpeningSecurityRolesExplorerFormat1
                , (service) => WindowHelper.OpenRolesExplorer(this._iWriteToOutput, service, commonConfig, selection)
            );
        }

        #endregion Открытие Explorers.
    }
}