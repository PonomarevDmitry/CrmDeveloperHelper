using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileCSharpProjectPluginTypeStepsAddIntoSolutionCommand : AbstractCommand
    {
        private FileCSharpProjectPluginTypeStepsAddIntoSolutionCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileCSharpProjectPluginTypeStepsAddIntoSolutionCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FileCSharpProjectPluginTypeStepsAddIntoSolutionCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileCSharpProjectPluginTypeStepsAddIntoSolutionCommand(package);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerAnyItemContainsProject(command, menuCommand, FileOperations.SupportsCSharpType);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, "Add Steps for PluginType into Crm Solution");
        }

        private static async void ActionExecute(DTEHelper helper)
        {
            var list = helper.GetListSelectedItemInSolutionExplorer(FileOperations.SupportsCSharpType);

            var files = new List<string>();

            foreach (var item in list)
            {
                var typeNam = await PropertiesHelper.GetTypeFullNameAsync(item);

                if (!string.IsNullOrEmpty(typeNam))
                {
                    files.Add(typeNam);
                }
            }

            if (files.Any())
            {
                helper.HandleAddingPluginAssemblyIntoSolutionByProjectCommand(null, true, files.ToArray());
            }
        }
    }
}