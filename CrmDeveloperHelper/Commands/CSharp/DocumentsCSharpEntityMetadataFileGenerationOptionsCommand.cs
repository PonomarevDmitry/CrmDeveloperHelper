using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class DocumentsCSharpEntityMetadataFileGenerationOptionsCommand : AbstractCommand
    {
        private DocumentsCSharpEntityMetadataFileGenerationOptionsCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidCommandSet.DocumentsCSharpEntityMetadataFileGenerationOptionsCommandId
            )
        {

        }

        public static DocumentsCSharpEntityMetadataFileGenerationOptionsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new DocumentsCSharpEntityMetadataFileGenerationOptionsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleEntityMetadataFileGenerationOptions();
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsCSharp(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CSharpEntityMetadataFileGenerationOptionsCommand);
        }
    }
}
