using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Commons
{
    internal sealed class CommonExportFormEventsCommand : AbstractSingleCommand
    {
        private CommonExportFormEventsCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CommonExportFormEventsCommandId) { }

        public static CommonExportFormEventsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonExportFormEventsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleExportFormEvents(null);
        }
    }
}
