using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class DTEHelper
    {
        public void HandlePluginTypeAddingProcessingStepsByProjectCommand(ConnectionData connectionData, string solutionUniqueName, bool withSelect, EnvDTE.Document document)
        {
            if (document == null || string.IsNullOrEmpty(document.FullName))
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => HandlePluginTypeAddingProcessingStepsByProjectCommandInternal(conn, commonConfig, solutionUniqueName, withSelect, document.FullName));
        }

        public void HandlePluginTypeAddingProcessingStepsByProjectCommand(ConnectionData connectionData, string solutionUniqueName, bool withSelect, IEnumerable<EnvDTE.Document> documentColl)
        {
            if (documentColl == null || !documentColl.Any(d => !string.IsNullOrEmpty(d.FullName)))
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => HandlePluginTypeAddingProcessingStepsByProjectCommandInternal(conn, commonConfig, solutionUniqueName, withSelect, documentColl.Where(d => !string.IsNullOrEmpty(d.FullName)).Select(d => d.FullName)));
        }

        public void HandlePluginTypeAddingProcessingStepsByProjectCommand(ConnectionData connectionData, string solutionUniqueName, bool withSelect, EnvDTE.ProjectItem projectItem)
        {
            if (projectItem == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => HandlePluginTypeAddingProcessingStepsByProjectCommandInternal(conn, commonConfig, solutionUniqueName, withSelect, projectItem.FileNames[1]));
        }

        public void HandlePluginTypeAddingProcessingStepsByProjectCommand(ConnectionData connectionData, string solutionUniqueName, bool withSelect, IEnumerable<EnvDTE.ProjectItem> projectItemColl)
        {
            if (projectItemColl == null || !projectItemColl.Any(i => !string.IsNullOrEmpty(i.FileNames[1])))
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => HandlePluginTypeAddingProcessingStepsByProjectCommandInternal(conn, commonConfig, solutionUniqueName, withSelect, projectItemColl.Where(i => !string.IsNullOrEmpty(i.FileNames[1])).Select(i => i.FileNames[1])));
        }

        private void HandlePluginTypeAddingProcessingStepsByProjectCommandInternal(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, bool withSelect, string filePath)
        {
            string pluginType = CSharpCodeHelper.GetClassInFileBySyntaxTree(filePath);

            this.WriteToOutput(connectionData, Properties.OutputStrings.GettingClassTypeFullNameFromFileFormat2, filePath, pluginType);
            this.ActivateOutputWindow(connectionData);

            Controller.StartPluginTypeAddingProcessingStepsToSolution(connectionData, commonConfig, solutionUniqueName, new[] { pluginType }, withSelect);
        }

        private void HandlePluginTypeAddingProcessingStepsByProjectCommandInternal(ConnectionData connectionData, CommonConfiguration commonConfig, string solutionUniqueName, bool withSelect, IEnumerable<string> filePathColl)
        {
            var list = filePathColl.OrderBy(s => s).ToList();

            if (!list.Any())
            {
                return;
            }

            this.ActivateOutputWindow(connectionData);

            this.WriteToOutput(connectionData, Properties.OutputStrings.GettingClassTypeFullNameFromFilesFormat, list.Count.ToString());

            List<string> pluginTypeNames = new List<string>();

            var table = new FormatTextTableHandler();
            table.SetHeader("File", "Type.FullName");

            foreach (var filePath in list)
            {
                string pluginType = CSharpCodeHelper.GetClassInFileBySyntaxTree(filePath);

                table.AddLine(filePath, pluginType);

                pluginTypeNames.Add(pluginType);
            }

            StringBuilder stringBuilder = new StringBuilder();

            table.GetFormatedLines(false).ForEach(s => stringBuilder.AppendLine(s));

            this.WriteToOutput(connectionData, stringBuilder.ToString());

            Controller.StartPluginTypeAddingProcessingStepsToSolution(connectionData, commonConfig, solutionUniqueName, pluginTypeNames, withSelect);
        }

        public void HandleActionOnPluginTypesCommand(ConnectionData connectionData, ActionOnComponent actionOnComponent, string[] pluginTypeNames)
        {
            HandleActionOnPluginTypesCommand(connectionData, actionOnComponent, string.Empty, string.Empty, pluginTypeNames);
        }

        public void HandleActionOnPluginTypesCommand(ConnectionData connectionData, ActionOnComponent actionOnComponent, string fieldName, string fieldTitle, string[] pluginTypeNames)
        {
            if (pluginTypeNames == null || !pluginTypeNames.Any())
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartActionOnPluginTypes(conn, commonConfig, pluginTypeNames, actionOnComponent, fieldName, fieldTitle));
        }

        public void HandleActionOnPluginTypesCommand(ConnectionData connectionData, ActionOnComponent actionOnComponent, IEnumerable<EnvDTE.ProjectItem> projectItemColl)
        {
            HandleActionOnPluginTypesCommand(connectionData, actionOnComponent, string.Empty, string.Empty, projectItemColl);
        }

        public void HandleActionOnPluginTypesCommand(ConnectionData connectionData, ActionOnComponent actionOnComponent, string fieldName, string fieldTitle, IEnumerable<EnvDTE.ProjectItem> projectItemColl)
        {
            if (projectItemColl == null || !projectItemColl.Any(i => !string.IsNullOrEmpty(i.FileNames[1])))
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => HandleActionOnPluginTypesCommandInternal(conn, commonConfig, actionOnComponent, fieldName, fieldTitle, projectItemColl.Where(i => !string.IsNullOrEmpty(i.FileNames[1])).Select(i => i.FileNames[1])));
        }

        public void HandleActionOnPluginTypesCommand(ConnectionData connectionData, ActionOnComponent actionOnComponent, EnvDTE.Document document)
        {
            HandleActionOnPluginTypesCommand(connectionData, actionOnComponent, string.Empty, string.Empty, document);
        }

        public void HandleActionOnPluginTypesCommand(ConnectionData connectionData, ActionOnComponent actionOnComponent, string fieldName, string fieldTitle, EnvDTE.Document document)
        {
            if (document == null || string.IsNullOrEmpty(document.FullName))
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => HandleActionOnPluginTypesCommandInternal(conn, commonConfig, actionOnComponent, fieldName, fieldTitle, document.FullName));
        }

        private void HandleActionOnPluginTypesCommandInternal(ConnectionData connectionData, CommonConfiguration commonConfig, ActionOnComponent actionOnComponent, string fieldName, string fieldTitle, string filePath)
        {
            string pluginType = CSharpCodeHelper.GetClassInFileBySyntaxTree(filePath);

            this.WriteToOutput(connectionData, Properties.OutputStrings.GettingClassTypeFullNameFromFileFormat2, filePath, pluginType);
            this.ActivateOutputWindow(connectionData);

            Controller.StartActionOnPluginTypes(connectionData, commonConfig, new[] { pluginType }, actionOnComponent, fieldName, fieldTitle);
        }

        public void HandleActionOnPluginTypesCommand(ConnectionData connectionData, ActionOnComponent actionOnComponent, IEnumerable<EnvDTE.Document> documentColl)
        {
            HandleActionOnPluginTypesCommand(connectionData, actionOnComponent, string.Empty, string.Empty, documentColl);
        }

        public void HandleActionOnPluginTypesCommand(ConnectionData connectionData, ActionOnComponent actionOnComponent, string fieldName, string fieldTitle, IEnumerable<EnvDTE.Document> documentColl)
        {
            if (documentColl == null || !documentColl.Any(d => !string.IsNullOrEmpty(d.FullName)))
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => HandleActionOnPluginTypesCommandInternal(conn, commonConfig, actionOnComponent, fieldName, fieldTitle, documentColl.Where(d => !string.IsNullOrEmpty(d.FullName)).Select(d => d.FullName)));
        }

        private void HandleActionOnPluginTypesCommandInternal(ConnectionData connectionData, CommonConfiguration commonConfig, ActionOnComponent actionOnComponent, string fieldName, string fieldTitle, IEnumerable<string> filePathColl)
        {
            var list = filePathColl.OrderBy(s => s).ToList();

            if (!list.Any())
            {
                return;
            }

            this.ActivateOutputWindow(connectionData);

            this.WriteToOutput(connectionData, Properties.OutputStrings.GettingClassTypeFullNameFromFilesFormat, list.Count.ToString());

            List<string> pluginTypeNames = new List<string>();

            var table = new FormatTextTableHandler();
            table.SetHeader("File", "Type.FullName");

            foreach (var filePath in list)
            {
                string pluginType = CSharpCodeHelper.GetClassInFileBySyntaxTree(filePath);

                table.AddLine(filePath, pluginType);

                pluginTypeNames.Add(pluginType);
            }

            StringBuilder stringBuilder = new StringBuilder();

            table.GetFormatedLines(false).ForEach(s => stringBuilder.AppendLine(s));

            this.WriteToOutput(connectionData, stringBuilder.ToString());

            Controller.StartActionOnPluginTypes(connectionData, commonConfig, pluginTypeNames, actionOnComponent, fieldName, fieldTitle);
        }

        public void HandleOpenPluginTypeExplorer(string selection)
        {
            HandleOpenPluginTypeExplorer(null, selection);
        }

        public void HandleOpenPluginTypeExplorer(ConnectionData connectionData, string selection)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartOpenPluginTypeExplorer(conn, commonConfig, selection));
        }

        public void HandleOpenPluginTypeExplorer(ConnectionData connectionData, EnvDTE.Document document)
        {
            if (document == null || string.IsNullOrEmpty(document.FullName))
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => HandleGetPluginTypeOpenPluginTypeExplorerInternal(conn, commonConfig, document.FullName));
        }

        public void HandleOpenPluginTypeExplorer(ConnectionData connectionData, EnvDTE.ProjectItem projectItem)
        {
            if (projectItem == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => HandleGetPluginTypeOpenPluginTypeExplorerInternal(conn, commonConfig, projectItem.FileNames[1]));
        }

        private void HandleGetPluginTypeOpenPluginTypeExplorerInternal(ConnectionData connectionData, CommonConfiguration commonConfig, string filePath)
        {
            string pluginType = CSharpCodeHelper.GetClassInFileBySyntaxTree(filePath);

            this.WriteToOutput(connectionData, Properties.OutputStrings.GettingClassTypeFullNameFromFileFormat2, filePath, pluginType);
            this.ActivateOutputWindow(connectionData);

            this.HandleOpenPluginTypeExplorer(connectionData, pluginType);
        }

        public void HandleAddPluginStep(string pluginTypeName, ConnectionData connectionData)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartAddPluginStep(conn, commonConfig, pluginTypeName));
        }

        public void HandleAddPluginStep(ConnectionData connectionData, EnvDTE.Document document)
        {
            if (document == null || string.IsNullOrEmpty(document.FullName))
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => HandleAddPluginStepInternal(conn, commonConfig, document.FullName));
        }

        public void HandleAddPluginStep(ConnectionData connectionData, EnvDTE.ProjectItem projectItem)
        {
            if (projectItem == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => HandleAddPluginStepInternal(conn, commonConfig, projectItem.FileNames[1]));
        }

        private void HandleAddPluginStepInternal(ConnectionData connectionData, CommonConfiguration commonConfig, string filePath)
        {
            string pluginType = CSharpCodeHelper.GetClassInFileBySyntaxTree(filePath);

            this.WriteToOutput(connectionData, Properties.OutputStrings.GettingClassTypeFullNameFromFileFormat2, filePath, pluginType);
            this.ActivateOutputWindow(connectionData);

            this.Controller.StartAddPluginStep(connectionData, commonConfig, pluginType);
        }
    }
}
