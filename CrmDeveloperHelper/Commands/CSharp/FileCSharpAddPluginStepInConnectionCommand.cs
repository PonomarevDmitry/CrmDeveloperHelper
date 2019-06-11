using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FileCSharpAddPluginStepInConnectionCommand : AbstractCommandByConnectionWithoutCurrent
    {
        private FileCSharpAddPluginStepInConnectionCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.FileCSharpAddPluginStepInConnectionCommandId
            )
        {

        }

        public static FileCSharpAddPluginStepInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileCSharpAddPluginStepInConnectionCommand(commandService);
        }

        protected override async void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var projectItem = helper.GetSingleSelectedProjectItemInSolutionExplorer(FileOperations.SupportsCSharpType);

            if (projectItem != null)
            {
                helper.WriteToOutput(null, Properties.OutputStrings.GettingClassFullNameFromFileFormat1, projectItem.FileNames[1]);
                helper.ActivateOutputWindow(null);

                string fileType = await PropertiesHelper.GetTypeFullNameAsync(projectItem);

                helper.HandleAddPluginStep(fileType, connectionData);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpSingle(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerSingleItemContainsProject(applicationObject, menuCommand);
        }
    }
}
