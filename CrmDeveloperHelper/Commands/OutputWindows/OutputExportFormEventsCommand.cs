using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal sealed class OutputExportFormEventsCommand : AbstractOutputWindowCommand
    {
        private OutputExportFormEventsCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.OutputExportFormEventsCommandId) { }

        public static OutputExportFormEventsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputExportFormEventsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleExportFormEvents(connectionData);
        }
    }
}