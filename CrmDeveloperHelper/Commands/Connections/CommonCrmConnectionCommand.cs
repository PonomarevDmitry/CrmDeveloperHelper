using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Connections
{
    internal sealed class CommonCrmConnectionCommand : AbstractSingleCommand
    {
        private CommonCrmConnectionCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CommonCrmConnectionCommandId) { }

        public static CommonCrmConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonCrmConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleConnectionOpenList();
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CommonCrmConnectionCommand);
        }
    }
}