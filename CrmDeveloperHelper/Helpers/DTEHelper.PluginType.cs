using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public partial class DTEHelper
    {
        public void HandlePluginTypeAddingProcessingStepsByProjectCommand(ConnectionData connectionData, string solutionUniqueName, bool withSelect, IEnumerable<SelectedFile> selectedFiles)
        {
            if (!selectedFiles.Any())
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => HandlePluginTypeAddingProcessingStepsByProjectCommandInternal(conn, commonConfig, selectedFiles, solutionUniqueName, withSelect));
        }

        private void HandlePluginTypeAddingProcessingStepsByProjectCommandInternal(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<SelectedFile> selectedFiles, string solutionUniqueName, bool withSelect)
        {
            if (!selectedFiles.Any())
            {
                return;
            }

            this.ActivateOutputWindow(connectionData);

            this.WriteToOutput(connectionData, Properties.OutputStrings.GettingClassTypeFullNameFromFilesFormat, selectedFiles.Count().ToString());

            List<string> pluginTypeNames = new List<string>();

            var table = new FormatTextTableHandler();
            table.SetHeader("File", "Type.FullName");

            foreach (var selectedFile in selectedFiles.OrderBy(f => f.FilePath))
            {
                string pluginType = CSharpCodeHelper.GetClassInFileBySyntaxTree(selectedFile.FilePath);

                table.AddLine(selectedFile.FilePath, pluginType);

                pluginTypeNames.Add(pluginType);
            }

            StringBuilder stringBuilder = new StringBuilder();

            table.GetFormatedLines(false).ForEach(s => stringBuilder.AppendLine(s));

            this.WriteToOutput(connectionData, stringBuilder.ToString());

            Controller.StartPluginTypeAddingProcessingStepsToSolution(connectionData, commonConfig, pluginTypeNames, solutionUniqueName, withSelect);
        }

        public void HandleActionOnPluginTypesCommand(ConnectionData connectionData, IEnumerable<SelectedFile> selectedFiles, ActionOnComponent actionOnComponent, string fieldName, string fieldTitle)
        {
            if (!selectedFiles.Any())
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => HandleActionOnPluginTypesCommandInternal(conn, commonConfig, selectedFiles, actionOnComponent, fieldName, fieldTitle));
        }

        private void HandleActionOnPluginTypesCommandInternal(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<SelectedFile> selectedFiles, ActionOnComponent actionOnComponent, string fieldName, string fieldTitle)
        {
            if (!selectedFiles.Any())
            {
                return;
            }

            this.ActivateOutputWindow(connectionData);

            this.WriteToOutput(connectionData, Properties.OutputStrings.GettingClassTypeFullNameFromFilesFormat, selectedFiles.Count().ToString());

            List<string> pluginTypeNames = new List<string>();

            var table = new FormatTextTableHandler();
            table.SetHeader("File", "Type.FullName");

            foreach (var selectedFile in selectedFiles.OrderBy(f => f.FileName))
            {
                string pluginType = CSharpCodeHelper.GetClassInFileBySyntaxTree(selectedFile.FilePath);

                table.AddLine(selectedFile.FilePath, pluginType);

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

        public void HandleOpenPluginTypeExplorer(ConnectionData connectionData, SelectedFile selectedFile)
        {
            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => HandleGetPluginTypeOpenPluginTypeExplorerInternal(conn, commonConfig, selectedFile));
        }

        private void HandleGetPluginTypeOpenPluginTypeExplorerInternal(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string pluginType = string.Empty;

            if (selectedFile != null)
            {
                pluginType = CSharpCodeHelper.GetClassInFileBySyntaxTree(selectedFile.FilePath);

                this.WriteToOutput(connectionData, Properties.OutputStrings.GettingClassTypeFullNameFromFileFormat2, selectedFile.FilePath, pluginType);
                this.ActivateOutputWindow(connectionData);
            }

            Controller.StartOpenPluginTypeExplorer(connectionData, commonConfig, pluginType);
        }

        public void HandleAddPluginStep(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => HandleAddPluginStepInternal(conn, commonConfig, selectedFile));
        }

        private void HandleAddPluginStepInternal(ConnectionData connectionData, CommonConfiguration commonConfig, SelectedFile selectedFile)
        {
            string pluginType = CSharpCodeHelper.GetClassInFileBySyntaxTree(selectedFile.FilePath);

            this.WriteToOutput(connectionData, Properties.OutputStrings.GettingClassTypeFullNameFromFileFormat2, selectedFile.FilePath, pluginType);
            this.ActivateOutputWindow(connectionData);

            this.Controller.StartAddPluginStep(connectionData, commonConfig, pluginType);
        }

        public void HandlePluginTypeCustomWorkflowActivityInfoGetCurrentCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartPluginTypeCustomWorkflowActivityInfoGetCurrent(conn, commonConfig, selectedFile));
        }

        public void HandlePluginTypeCustomWorkflowActivityInfoShowDifferenceCommand(ConnectionData connectionData, SelectedFile selectedFile)
        {
            if (selectedFile == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartPluginTypeCustomWorkflowActivityInfoDifference(conn, commonConfig, selectedFile));
        }

        public void HandlePluginTypeCustomWorkflowActivityInfoShowDifferenceCommand(ConnectionData connectionData, XDocument doc, string filePath)
        {
            if (doc == null)
            {
                return;
            }

            GetConnectionConfigAndExecute(connectionData, (conn, commonConfig) => Controller.StartPluginTypeCustomWorkflowActivityInfoDifference(conn, commonConfig, doc, filePath));
        }
    }
}