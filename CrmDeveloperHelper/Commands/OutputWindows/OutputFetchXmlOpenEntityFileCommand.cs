using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal sealed class OutputFetchXmlOpenEntityFileCommand : AbstractOutputWindowCommand
    {
        private OutputFetchXmlOpenEntityFileCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.OutputFetchXmlOpenEntityFileCommandId)
        {
        }

        public static OutputFetchXmlOpenEntityFileCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputFetchXmlOpenEntityFileCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleConnectionOpenFetchXmlFile(connectionData);
        }
    }
}