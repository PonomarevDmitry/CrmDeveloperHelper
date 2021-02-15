using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.ListForPublish
{
    internal sealed class ListForPublishOpenFilesCommand : AbstractDynamicCommandByOpenFilesType
    {
        private readonly bool _inTextEditor;

        private ListForPublishOpenFilesCommand(
            OleMenuCommandService commandService
            , int baseIdStart
            , IList<OpenFilesType> sourceOpenTypes
            , bool inTextEditor
        ) : base(commandService, baseIdStart, sourceOpenTypes)
        {
            this._inTextEditor = inTextEditor;
        }

        public static ListForPublishOpenFilesCommand InstanceFileOrdinal { get; private set; }

        public static ListForPublishOpenFilesCommand InstanceFileChanges { get; private set; }

        public static ListForPublishOpenFilesCommand InstanceFileMirror { get; private set; }

        public static ListForPublishOpenFilesCommand InstanceInTextEditorFileOrdinal { get; private set; }

        public static ListForPublishOpenFilesCommand InstanceInTextEditorFileChanges { get; private set; }

        public static ListForPublishOpenFilesCommand InstanceInTextEditorFileMirror { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceFileOrdinal = new ListForPublishOpenFilesCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.ListForPublishOpenFilesByTypeOrdinalCommandId, _typesOrdinal, false);
            InstanceFileChanges = new ListForPublishOpenFilesCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.ListForPublishOpenFilesByTypeWithChangesCommandId, _typesChanges, false);
            InstanceFileMirror = new ListForPublishOpenFilesCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.ListForPublishOpenFilesByTypeWithMirrorCommandId, _typesMirror, false);

            InstanceInTextEditorFileOrdinal = new ListForPublishOpenFilesCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.ListForPublishOpenFilesByTypeInTextEditorOrdinalCommandId, _typesOrdinal, true);
            InstanceInTextEditorFileChanges = new ListForPublishOpenFilesCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.ListForPublishOpenFilesByTypeInTextEditorWithChangesCommandId, _typesChanges, true);
            InstanceInTextEditorFileMirror = new ListForPublishOpenFilesCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.ListForPublishOpenFilesByTypeInTextEditorWithMirrorCommandId, _typesMirror, true);
        }

        protected override void CommandAction(DTEHelper helper, OpenFilesType openFilesType)
        {
            var selectedFiles = helper.GetSelectedFilesFromListForPublish(FileOperations.SupportsWebResourceTextType).ToList();

            var crmConfig = ConnectionConfiguration.Get();

            var connectionData = crmConfig.CurrentConnectionData;

            if (selectedFiles.Any())
            {
                helper.ShowListForPublish(connectionData);

                helper.HandleWebResourceOpenFilesCommand(selectedFiles, openFilesType, _inTextEditor);
            }
            else
            {
                helper.WriteToOutput(connectionData, Properties.OutputStrings.PublishListIsEmpty);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OpenFilesType openFilesType, OleMenuCommand menuCommand)
        {
            if (_inTextEditor)
            {
                CommonHandlers.ActionBeforeQueryStatusTextEditorProgramExists(applicationObject, menuCommand);
            }

            CommonHandlers.ActionBeforeQueryStatusListForPublishWebResourceTextAny(applicationObject, menuCommand);
        }
    }
}