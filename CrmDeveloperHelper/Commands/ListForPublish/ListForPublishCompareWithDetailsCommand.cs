using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.ListForPublish
{
    internal sealed class ListForPublishCompareWithDetailsCommand : AbstractSingleCommand
    {
        private ListForPublishCompareWithDetailsCommand(OleMenuCommandService commandService)
          : base(commandService, PackageIds.guidCommandSet.ListForPublishCompareWithDetailsCommandId) { }

        public static ListForPublishCompareWithDetailsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ListForPublishCompareWithDetailsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleFileCompareListForPublishCommand(null, true);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusListForPublishWebResourceAny(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.ListForPublishCompareWithDetailsCommand);
        }
    }
}