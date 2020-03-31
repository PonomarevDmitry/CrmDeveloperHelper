using EnvDTE;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class DTEHelper
    {
        public void HandleSolutionOpenLastSelected(ConnectionData connectionData, string solutionUniqueName, ActionOpenComponent action)
        {
            if (string.IsNullOrEmpty(solutionUniqueName))
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartSolutionOpening(conn, commonConfig, solutionUniqueName, action));
        }

        public string GetLastSolutionUniqueName()
        {
            ConnectionConfiguration connectionConfig = Model.ConnectionConfiguration.Get();

            return connectionConfig?.CurrentConnectionData?.LastSelectedSolutionsUniqueName?.FirstOrDefault();
        }

        public void HandleOpenSolutionExplorerWindow(ConnectionData connectionData)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartOpenSolutionExplorerWindow(conn, commonConfig, null));
        }

        public void HandleSolutionAddFileToFolder()
        {
            SelectedItem selectedItem = GetSelectedProjectItem();

            if (selectedItem == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(null, (conn, commonConfig) => Controller.StartOpenSolutionExplorerWindow(conn, commonConfig, selectedItem));
        }
    }
}
