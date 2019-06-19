using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal class OutputTraceExportFileCommand : AbstractOutputWindowCommand
    {
        private OutputTraceExportFileCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.OutputTraceExportFileCommandId) { }

        public static OutputTraceExportFileCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputTraceExportFileCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleExportTraceEnableFile();
        }
    }
}