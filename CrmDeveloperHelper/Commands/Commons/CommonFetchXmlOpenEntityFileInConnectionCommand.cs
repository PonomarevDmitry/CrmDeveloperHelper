using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Commons
{
    internal sealed class CommonFetchXmlOpenEntityFileInConnectionCommand : AbstractSingleCommand
    {
        private CommonFetchXmlOpenEntityFileInConnectionCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CommonFetchXmlOpenEntityFileInConnectionCommandId)
        {
        }

        public static CommonFetchXmlOpenEntityFileInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonFetchXmlOpenEntityFileInConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleConnectionOpenFetchXmlFile();
        }
    }
}
