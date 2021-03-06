﻿using EnvDTE;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
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

        public void HandlePluginAssemblyComparingPluginTypesWithLocalAssemblyCommand(ConnectionData connectionData, IEnumerable<Project> projectList)
        {
            if (projectList == null || !projectList.Any(p => !string.IsNullOrEmpty(p.Name)))
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartPluginAssemblyComparingPluginTypesWithLocalAssembly(conn, commonConfig, projectList));
        }

        public void HandlePluginAssemblyComparingByteArrayLocalAssemblyAndPluginAssemblyCommand(ConnectionData connectionData, IEnumerable<Project> projectList)
        {
            if (projectList == null || !projectList.Any(p => !string.IsNullOrEmpty(p.Name)))
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartComparingByteArrayLocalAssemblyAndPluginAssembly(conn, commonConfig, projectList));
        }

        public void HandlePluginAssemblyBuildProjectUpdateCommand(ConnectionData connectionData, bool registerPlugins, IEnumerable<Project> projectList)
        {
            if (projectList == null || !projectList.Any(p => !string.IsNullOrEmpty(p.Name)))
            {
                return;
            }

            var projectsString = string.Join(Environment.NewLine, projectList.Select(p => p.Name));

            string title = Properties.MessageBoxStrings.ConfirmBuildAndUpdate;

            Func<ConnectionData, string> message = (conn) => string.Format(Properties.MessageBoxStrings.BuildProjectsAndUpdatePluginAssembliesFormat2, projectsString, conn.GetDescriptionColumn());

            GetConnectionConfigConfirmActionAndExecute(connectionData, message, title, (conn, commonConfig) => Controller.StartPluginAssemblyBuildProjectUpdate(conn, commonConfig, projectList, registerPlugins));
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
