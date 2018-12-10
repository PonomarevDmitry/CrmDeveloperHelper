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