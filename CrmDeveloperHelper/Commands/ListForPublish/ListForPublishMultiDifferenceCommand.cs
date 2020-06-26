using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.ListForPublish
{
    internal sealed class ListForPublishMultiDifferenceCommand : AbstractDynamicCommandByOpenFilesType
    {
        private ListForPublishMultiDifferenceCommand(
            OleMenuCommandService commandService
            , int baseIdStart
            , IList<OpenFilesType> sourceOpenTypes
        ) : base(commandService, baseIdStart, sourceOpenTypes)
        {
        }

        public static ListForPublishMultiDifferenceCommand InstanceExistsOrHasLink { get; private set; }

        public static ListForPublishMultiDifferenceCommand InstanceChanges { get; private set; }

        public static ListForPublishMultiDifferenceCommand InstanceMirror { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceExistsOrHasLink = new ListForPublishMultiDifferenceCommand(commandService, PackageIds.guidDynamicCommandSet.ListForPublishMultiDifferenceFilesExistsOrHasLinkCommandId, _typesExistsOrHasLink);
            InstanceChanges = new ListForPublishMultiDifferenceCommand(commandService, PackageIds.guidDynamicCommandSet.ListForPublishMultiDifferenceFilesWithChangesCommandId, _typesChanges);
            InstanceMirror = new ListForPublishMultiDifferenceCommand(commandService, PackageIds.guidDynamicCommandSet.ListForPublishMultiDifferenceFilesWithMirrorCommandId, _typesMirror);
        }

        protected override void CommandAction(DTEHelper helper, OpenFilesType openFilesType)
        {
            var selectedFiles = helper.GetSelectedFilesFromListForPublish(FileOperations.SupportsWebResourceTextType).ToList();

            var crmConfig = ConnectionConfiguration.Get();

            var connectionData = crmConfig.CurrentConnectionData;

            if (selectedFiles.Any())
            {
                helper.ShowListForPublish(connectionData);

                helper.HandleWebResourceMultiDifferenceFiles(selectedFiles, openFilesType);
            }
            else
            {
                helper.WriteToOutput(connectionData, Properties.OutputStrings.PublishListIsEmpty);
            }
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, OpenFilesType element, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusListForPublishWebResourceTextAny(applicationObject, menuCommand);
        }
    }
}