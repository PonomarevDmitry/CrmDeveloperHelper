using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CodeCSharpProjectPluginTypeStepsAddToSolutionLastCommand : AbstractDynamicCommandAddObjectToSolutionLast
    {
        private CodeCSharpProjectPluginTypeStepsAddToSolutionLastCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.CodeCSharpProjectPluginTypeStepsAddToSolutionLastCommandId
                , ActionExecuteAsync
                , ActionBeforeQueryStatus
            )
        {

        }

        public static CodeCSharpProjectPluginTypeStepsAddToSolutionLastCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeCSharpProjectPluginTypeStepsAddToSolutionLastCommand(commandService);
        }

        private static async void ActionExecuteAsync(DTEHelper helper, ConnectionData connectionData, string solutionUniqueName)
        {
            var document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsCSharpType);

            helper.WriteToOutput(null, Properties.OutputStrings.GettingClassFullNameFromFileFormat1, document?.FullName);
            helper.ActivateOutputWindow(null);

            string fileType = await PropertiesHelper.GetTypeFullNameAsync(document);

            helper.HandleAddingPluginTypeProcessingStepsByProjectCommand(null, solutionUniqueName, false, fileType);
        }

        private static void ActionBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharp(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusActiveDocumentContainingProject(applicationObject, menuCommand);
        }
    }
}