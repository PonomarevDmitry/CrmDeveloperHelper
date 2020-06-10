using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Linq;


namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class DTEHelper
    {
        public void HandlePluginTypeAddingProcessingStepsByProjectCommand(ConnectionData connectionData, string solutionUniqueName, bool withSelect, params string[] pluginTypeNames)
        {
            if (pluginTypeNames == null || !pluginTypeNames.Any())
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartPluginTypeAddingProcessingStepsToSolution(conn, commonConfig, solutionUniqueName, pluginTypeNames, withSelect));
        }

        public void HandleActionOnPluginTypesCommand(ConnectionData connectionData,  ActionOnComponent actionOnComponent, params string[] pluginTypeNames)
        {
            if (pluginTypeNames == null || !pluginTypeNames.Any())
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartActionOnPluginTypes(conn, commonConfig, pluginTypeNames, actionOnComponent));
        }

        public void HandleOpenPluginTypeExplorer(string selection)
        {
            HandleOpenPluginTypeExplorer(null, selection);
        }

        public void HandleOpenPluginTypeExplorer(ConnectionData connectionData, string selection)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartOpenPluginTypeExplorer(conn, commonConfig, selection));
        }

        public void HandleAddPluginStep(string pluginTypeName, ConnectionData connectionData)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartAddPluginStep(conn, commonConfig, pluginTypeName));
        }
    }
}
