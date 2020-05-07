using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FileCSharpAddPluginStepCommand : AbstractCommand
    {
        private FileCSharpAddPluginStepCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.FileCSharpAddPluginStepCommandId)
        {
        }

        public static FileCSharpAddPluginStepCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileCSharpAddPluginStepCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            System.Threading.Tasks.Task.WaitAll(ExecuteAsync(helper));
        }

        private static async System.Threading.Tasks.Task ExecuteAsync(DTEHelper helper)
        {
            try
            {
                var projectItem = helper.GetSingleSelectedProjectItemInSolutionExplorer(FileOperations.SupportsCSharpType);

                if (projectItem != null)
                {
                    helper.WriteToOutput(null, Properties.OutputStrings.GettingClassFullNameFromFileFormat1, projectItem.FileNames[1]);
                    helper.ActivateOutputWindow(null);

                    string fileType = await PropertiesHelper.GetTypeFullNameAsync(projectItem);

                    helper.HandleAddPluginStep(fileType, null);
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