using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FileCSharpPluginTreeCommand : AbstractCommand
    {
        private FileCSharpPluginTreeCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.FileCSharpPluginTreeCommandId) { }

        public static FileCSharpPluginTreeCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileCSharpPluginTreeCommand(commandService);
        }

        private async void CommandAction(DTEHelper helper)
        {
            try
            {
                var projectItem = helper.GetSingleSelectedProjectItemInSolutionExplorer(FileOperations.SupportsCSharpType);

                if (projectItem != null)
                {
                    helper.WriteToOutput(null, Properties.OutputStrings.GettingClassFullNameFromFileFormat1, projectItem.FileNames[1]);
                    helper.ActivateOutputWindow(null);

                    string fileType = await PropertiesHelper.GetTypeFullNameAsync(projectItem);

                    helper.HandleOpenPluginTree(string.Empty, fileType, string.Empty);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpSingle(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerSingleItemContainsProject(applicationObject, menuCommand);
        }
    }
}
