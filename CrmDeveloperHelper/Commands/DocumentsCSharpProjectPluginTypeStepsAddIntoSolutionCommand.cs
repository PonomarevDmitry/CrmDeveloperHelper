using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class DocumentsCSharpProjectPluginTypeStepsAddIntoSolutionCommand : AbstractCommand
    {
        private DocumentsCSharpProjectPluginTypeStepsAddIntoSolutionCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.DocumentsCSharpProjectPluginTypeStepsAddIntoSolutionCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static DocumentsCSharpProjectPluginTypeStepsAddIntoSolutionCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new DocumentsCSharpProjectPluginTypeStepsAddIntoSolutionCommand(package);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsCSharp(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.DocumentsCSharpProjectPluginTypeStepsAddIntoSolutionCommand);
        }

        private static async void ActionExecute(DTEHelper helper)
        {
            var list = helper.GetOpenedDocumentsAsDocument(FileOperations.SupportsCSharpType);

            var pluginTypeNames = new List<string>();

            helper.ActivateOutputWindow(null);

            foreach (var item in list)
            {
                helper.WriteToOutput(null, Properties.OutputStrings.GettingClassFullNameFromFileFormat1, item?.FullName);
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
    }
}