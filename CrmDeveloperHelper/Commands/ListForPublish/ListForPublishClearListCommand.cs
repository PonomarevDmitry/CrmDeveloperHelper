using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.Windows;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.ListForPublish
{
    internal sealed class ListForPublishClearListCommand : AbstractSingleCommand
    {
        private ListForPublishClearListCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.ListForPublishClearListCommandId) { }

        public static ListForPublishClearListCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ListForPublishClearListCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var connectionConfig = Model.ConnectionConfiguration.Get();

            var connectionData = connectionConfig.CurrentConnectionData;

            helper.ShowListForPublish(connectionData);

            if (MessageBox.Show(Properties.MessageBoxStrings.ClearListForPublish, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                helper.ClearListForPublish(connectionData);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusListForPublishWebResourceAny(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.ListForPublishClearListCommand);
        }
    }
}