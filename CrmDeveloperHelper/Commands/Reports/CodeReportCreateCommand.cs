using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Reports
{
    internal sealed class CodeReportCreateCommand : AbstractDynamicCommandByConnectionAll
    {
        private CodeReportCreateCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.CodeReportCreateCommandId
            )
        {

        }

        public static CodeReportCreateCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeReportCreateCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            connectionData.OpenReportCreateInWeb();
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentReport(applicationObject, menuCommand);
        }
    }
}