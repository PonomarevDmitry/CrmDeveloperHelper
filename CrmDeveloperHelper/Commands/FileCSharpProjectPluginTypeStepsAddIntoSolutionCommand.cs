using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
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

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.FileCSharpProjectPluginTypeStepsAddIntoSolutionCommand);
        }

        private static async void ActionExecute(DTEHelper helper)
        {
            try
            {
                var list = helper.GetListSelectedItemInSolutionExplorer(FileOperations.SupportsCSharpType);

                var pluginTypeNames = new List<string>();

                helper.ActivateOutputWindow(null);

                foreach (var item in list)
                {
                    helper.WriteToOutput(null, Properties.OutputStrings.GettingClassFullNameFromFileFormat1, item?.ProjectItem?.FileNames[1]);
                    var typeName = await PropertiesHelper.GetTypeFullNameAsync(item);

                    if (!string.IsNullOrEmpty(typeName))
                    {
                        pluginTypeNames.Add(typeName);
                    }
                }

                if (pluginTypeNames.Any())
                {
                    helper.HandleAddingPluginAssemblyIntoSolutionByProjectCommand(null, true, pluginTypeNames.ToArray());
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }
    }
}