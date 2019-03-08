using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileCSharpAddPluginStepCommand : AbstractCommand
    {
        private FileCSharpAddPluginStepCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileCSharpAddPluginStepCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FileCSharpAddPluginStepCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileCSharpAddPluginStepCommand(package);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpSingle(command, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerSingleItemContainsProject(command, menuCommand, FileOperations.SupportsCSharpType);
        }

        private static async void ActionExecute(DTEHelper helper)
        {
            try
            {
                EnvDTE.SelectedItem item = helper.GetSingleSelectedItemInSolutionExplorer(FileOperations.SupportsCSharpType);

                if (item != null)
                {
                    helper.WriteToOutput(null, Properties.OutputStrings.GettingClassFullNameFromFileFormat1, item?.ProjectItem?.FileNames[1]);
                    helper.ActivateOutputWindow(null);
                    string fileType = await PropertiesHelper.GetTypeFullNameAsync(item);

                    helper.HandleAddPluginStep(fileType, null);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }
    }
}
