using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal class OutputTraceExportFileCommand : AbstractCommand
    {
        private OutputTraceExportFileCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.OutputTraceExportFileCommandId) { }

        public static OutputTraceExportFileCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputTraceExportFileCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleExportTraceEnableFile();
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusIsConnectionOutput(applicationObject, menuCommand);
        }
    }
}