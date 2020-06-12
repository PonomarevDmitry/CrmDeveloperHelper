using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class DocumentsCSharpGlobalOptionSetsMetadataFileGenerationOptionsCommand : AbstractSingleCommand
    {
        private DocumentsCSharpGlobalOptionSetsMetadataFileGenerationOptionsCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidCommandSet.DocumentsCSharpGlobalOptionSetsMetadataFileGenerationOptionsCommandId
            )
        {

        }

        public static DocumentsCSharpGlobalOptionSetsMetadataFileGenerationOptionsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new DocumentsCSharpGlobalOptionSetsMetadataFileGenerationOptionsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleGlobalOptionSetsMetadataFileGenerationOptions();
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsCSharp(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CSharpGlobalOptionSetsMetadataFileGenerationOptionsCommand);
        }
    }
}
