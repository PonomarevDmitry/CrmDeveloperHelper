using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Commons
{
    internal class CommonTraceExportFileCommand : AbstractSingleCommand
    {
        private CommonTraceExportFileCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CommonTraceExportFileCommandId) { }

        public static CommonTraceExportFileCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonTraceExportFileCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleExportTraceEnableFile();
        }
    }
}