using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Commons
{
    internal sealed class CommonFetchXmlOpenEntityFileInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private CommonFetchXmlOpenEntityFileInConnectionCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CommonFetchXmlOpenEntityFileInConnectionCommandId)
        {
        }

        public static CommonFetchXmlOpenEntityFileInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonFetchXmlOpenEntityFileInConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleConnectionOpenFetchXmlFile(connectionData);
        }
    }
}