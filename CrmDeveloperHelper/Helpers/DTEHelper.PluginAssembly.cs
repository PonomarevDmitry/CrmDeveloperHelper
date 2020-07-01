using EnvDTE;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class DTEHelper
    {
        public void HandlePluginAssemblyAddingToSolutionByProjectCommand(ConnectionData connectionData, IEnumerable<string> projectNames, string solutionUniqueName, bool withSelect)
        {
            if (projectNames == null || !projectNames.Any())
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartPluginAssemblyAddingToSolution(conn, commonConfig, projectNames, solutionUniqueName, withSelect));
        }

        public void HandlePluginAssemblyAddingProcessingStepsByProjectCommand(ConnectionData connectionData, IEnumerable<string> projectNames, string solutionUniqueName, bool withSelect)
        {
            if (projectNames == null || !projectNames.Any())
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartPluginAssemblyAddingProcessingStepsToSolution(conn, commonConfig, projectNames, solutionUniqueName, withSelect));
        }

        public void HandlePluginAssemblyComparingWithLocalAssemblyCommand(ConnectionData connectionData, Project project)
        {
            if (project == null || string.IsNullOrEmpty(project.Name))
            {
                return;
            }

            var defaultOutputFilePath = PropertiesHelper.GetOutputFilePath(project);

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartPluginAssemblyComparingWithLocalAssembly(conn, commonConfig, project.Name, defaultOutputFilePath));
        }

        public void HandlePluginAssemblyBuildProjectUpdateCommand(ConnectionData connectionData, bool registerPlugins, IEnumerable<Project> projectList)
        {
            if (projectList == null || !projectList.Any(p => !string.IsNullOrEmpty(p.Name)))
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartPluginAssemblyBuildProjectUpdate(conn, commonConfig, projectList, registerPlugins));
        }

        public void HandleActionOnProjectPluginAssemblyCommand(ConnectionData connectionData, IEnumerable<Project> projectList, ActionOnComponent actionOnComponent)
        {
            if (projectList == null || !projectList.Any(p => !string.IsNullOrEmpty(p.Name)))
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartActionOnPluginAssembly(conn, commonConfig, projectList, actionOnComponent));
        }

        public void HandlePluginAssemblyRegisterCommand(ConnectionData connectionData, List<Project> projectList)
        {
            if (projectList == null || !projectList.Any(p => !string.IsNullOrEmpty(p.Name)))
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartPluginAssemblyRegister(conn, commonConfig, projectList));
        }

        public void HandleOpenPluginAssemblyExplorer()
        {
            HandleOpenPluginAssemblyExplorer(null, null);
        }

        public void HandleOpenPluginAssemblyExplorer(string selection)
        {
            HandleOpenPluginAssemblyExplorer(null, selection);
        }

        public void HandleOpenPluginAssemblyExplorer(ConnectionData connectionData)
        {
            string selection = GetSelectedText();

            HandleOpenPluginAssemblyExplorer(connectionData, selection);
        }

        public void HandleOpenPluginAssemblyExplorer(ConnectionData connectionData, string selection)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartOpenPluginAssemblyExplorer(conn, commonConfig, selection));
        }

        public void HandlePluginAssemblyUpdatingInWindowCommand(ConnectionData connectionData, params Project[] projectList)
        {
            HandlePluginAssemblyUpdatingInWindowCommand(connectionData, projectList.ToList());
        }

        public void HandlePluginAssemblyUpdatingInWindowCommand(ConnectionData connectionData, List<Project> projectList)
        {
            if (projectList == null || !projectList.Any(p => !string.IsNullOrEmpty(p.Name)))
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartPluginAssemblyUpdatingInWindow(conn, commonConfig, projectList));
        }
    }
}
