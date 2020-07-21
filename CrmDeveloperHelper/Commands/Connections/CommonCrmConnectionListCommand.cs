using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Connections
{
    internal sealed class CommonCrmConnectionListCommand : AbstractSingleCommand
    {
        private CommonCrmConnectionListCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CommonCrmConnectionListCommandId)
        {
        }

        public static CommonCrmConnectionListCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonCrmConnectionListCommand(commandService);
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