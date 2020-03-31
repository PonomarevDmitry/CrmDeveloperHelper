using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CodeCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private CodeCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommandId
            )
        {

        }

        public static CodeCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeCSharpProjectPluginTypeStepsAddToSolutionInConnectionCommand(commandService);
        }

        protected override async void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsCSharpType);

            helper.WriteToOutput(null, Properties.OutputStrings.GettingClassFullNameFromFileFormat1, document?.FullName);
            helper.ActivateOutputWindow(null);

            string fileType = await PropertiesHelper.GetTypeFullNameAsync(document);

            helper.HandlePluginTypeAddingProcessingStepsByProjectCommand(connectionData, null, true, fileType);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharp(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusActiveDocumentContainingProject(applicationObject, menuCommand);
        }
    }
}
