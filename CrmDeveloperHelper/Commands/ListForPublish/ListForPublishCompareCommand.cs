using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.ListForPublish
{
    internal sealed class ListForPublishCompareCommand : AbstractCommand
    {
        private ListForPublishCompareCommand(OleMenuCommandService commandService)
          : base(commandService, PackageIds.guidCommandSet.ListForPublishCompareCommandId) { }

        public static ListForPublishCompareCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ListForPublishCompareCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleFileCompareListForPublishCommand(null, false);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusListForPublishWebResourceAny(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.ListForPublishCompareCommand);
        }
    }
}