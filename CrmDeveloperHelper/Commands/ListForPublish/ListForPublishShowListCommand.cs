using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.ListForPublish
{
    internal sealed class ListForPublishShowListCommand : AbstractSingleCommand
    {
        private ListForPublishShowListCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.ListForPublishShowListCommandId) { }

        public static ListForPublishShowListCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ListForPublishShowListCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var connectionConfig = Model.ConnectionConfiguration.Get();

            var connectionData = connectionConfig.CurrentConnectionData;

            helper.ShowListForPublish(connectionData);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusListForPublishWebResourceAny(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.ListForPublishShowListCommand);
        }
    }
}