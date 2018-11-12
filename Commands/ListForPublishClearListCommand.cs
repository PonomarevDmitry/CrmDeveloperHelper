using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.Windows;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class ListForPublishClearListCommand : AbstractCommand
    {
        private ListForPublishClearListCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.ListForPublishClearListCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static ListForPublishClearListCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new ListForPublishClearListCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.ShowListForPublish();

            if (MessageBox.Show(Properties.MessageBoxStrings.ClearListForPublish, Properties.MessageBoxStrings.QuestionTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                helper.ClearListForPublish();
            }
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(command, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusListForPublishWebResourceAny(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.ListForPublishClearListCommand);
        }
    }
}