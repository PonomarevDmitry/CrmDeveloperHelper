using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.ListForPublish
{
    internal sealed class ListForPublishCompareInConnectionGroupCommand : AbstractDynamicCommandByConnectionByGroupWithoutCurrent
    {
        private readonly bool _withDetails;

        private ListForPublishCompareInConnectionGroupCommand(OleMenuCommandService commandService, int baseIdStart, bool withDetails)
            : base(
                commandService
                , baseIdStart
            )
        {
            this._withDetails = withDetails;
        }

        public static ListForPublishCompareInConnectionGroupCommand Instance { get; private set; }

        public static ListForPublishCompareInConnectionGroupCommand InstanceWithDetails { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ListForPublishCompareInConnectionGroupCommand(commandService, PackageIds.ListForPublishCompareInConnectionGroupCommandId, false);

            InstanceWithDetails = new ListForPublishCompareInConnectionGroupCommand(commandService, PackageIds.ListForPublishCompareWithDetailsInConnectionGroupCommandId, true);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleFileCompareListForPublishCommand(connectionData, this._withDetails);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusListForPublishWebResourceAny(applicationObject, menuCommand);
        }
    }
}