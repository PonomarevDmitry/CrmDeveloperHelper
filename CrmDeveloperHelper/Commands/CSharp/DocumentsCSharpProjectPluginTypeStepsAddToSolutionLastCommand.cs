using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class DocumentsCSharpProjectPluginTypeStepsAddToSolutionLastCommand : AbstractAddObjectToSolutionLastCommand
    {
        private DocumentsCSharpProjectPluginTypeStepsAddToSolutionLastCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.DocumentsCSharpProjectPluginTypeStepsAddToSolutionLastCommandId
                , ActionExecuteAsync
                , CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsCSharp
            )
        {

        }

        public static DocumentsCSharpProjectPluginTypeStepsAddToSolutionLastCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new DocumentsCSharpProjectPluginTypeStepsAddToSolutionLastCommand(commandService);
        }

        private static async void ActionExecuteAsync(DTEHelper helper, ConnectionData connectionData, string solutionUniqueName)
        {
            var list = helper.GetOpenedDocumentsAsDocument(FileOperations.SupportsCSharpType).ToList();

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
                helper.HandleAddingPluginTypeProcessingStepsByProjectCommand(null, solutionUniqueName, false, pluginTypeNames.ToArray());
            }
        }
    }
}