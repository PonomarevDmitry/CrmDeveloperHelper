using EnvDTE;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class DTEHelper
    {
        public void HandlePluginAssemblyAddingToSolutionByProjectCommand(ConnectionData connectionData, string solutionUniqueName, bool withSelect, params string[] projectNames)
        {
            if (projectNames == null || !projectNames.Any())
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartPluginAssemblyAddingToSolution(conn, commonConfig, solutionUniqueName, projectNames, withSelect));
        }

        public void HandlePluginAssemblyAddingProcessingStepsByProjectCommand(ConnectionData connectionData, string solutionUniqueName, bool withSelect, params string[] projectNames)
        {
            if (projectNames == null || !projectNames.Any())
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartPluginAssemblyAddingProcessingStepsToSolution(conn, commonConfig, solutionUniqueName, projectNames, withSelect));
        }

        public void HandlePluginAssemblyComparingWithLocalAssemblyCommand(ConnectionData connectionData, EnvDTE.Project project)
        {
            if (project == null || string.IsNullOrEmpty(project.Name))
            {
                return;
            }

            var defaultOutputFilePath = PropertiesHelper.GetOutputFilePath(project);

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartPluginAssemblyComparingWithLocalAssembly(conn, commonConfig, project.Name, defaultOutputFilePath));
        }

        public void HandlePluginAssemblyBuildProjectUpdateCommand(ConnectionData connectionData, bool registerPlugins, params EnvDTE.Project[] projectList)
        {
            HandlePluginAssemblyBuildProjectUpdateCommand(connectionData, registerPlugins, projectList.ToList());
        }

        public void HandlePluginAssemblyBuildProjectUpdateCommand(ConnectionData connectionData, bool registerPlugins, List<Project> projectList)
        {
            if (projectList == null || !projectList.Any(p => !string.IsNullOrEmpty(p.Name)))
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartPluginAssemblyBuildProjectUpdate(conn, commonConfig, projectList, registerPlugins));
        }

        public void HandlePluginAssemblyCreateEntityDescriptionCommand(ConnectionData connectionData, List<Project> projectList)
        {
            if (projectList == null || !projectList.Any(p => !string.IsNullOrEmpty(p.Name)))
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartPluginAssemblyCreateEntityDescription(conn, commonConfig, projectList));
        }

        public void HandlePluginAssemblyCreateDescriptionCommand(ConnectionData connectionData, List<Project> projectList)
        {
            if (projectList == null || !projectList.Any(p => !string.IsNullOrEmpty(p.Name)))
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartPluginAssemblyCreateDescription(conn, commonConfig, projectList));
        }

        public void HandlePluginAssemblyProjectOpenCommand(ConnectionData connectionData, List<Project> projectList, ActionOpenComponent actionOpen)
        {
            if (projectList == null || !projectList.Any(p => !string.IsNullOrEmpty(p.Name)))
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartPluginAssemblyOpen(conn, commonConfig, projectList, actionOpen));
        }

        public void HandlePluginAssemblyRegisterCommand(ConnectionData connectionData, List<EnvDTE.Project> projectList)
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

        public void HandlePluginAssemblyUpdatingInWindowCommand(ConnectionData connectionData, params EnvDTE.Project[] projectList)
        {
            HandlePluginAssemblyUpdatingInWindowCommand(connectionData, projectList.ToList());
        }

        public void HandlePluginAssemblyUpdatingInWindowCommand(ConnectionData connectionData, List<EnvDTE.Project> projectList)
        {
            if (projectList == null || !projectList.Any(p => !string.IsNullOrEmpty(p.Name)))
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartPluginAssemblyUpdatingInWindow(conn, commonConfig, projectList));
        }
    }
}
