using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FolderWebResourceOpenFilesCommand : AbstractSingleCommand
    {
        private readonly bool _inTextEditor;
        private readonly OpenFilesType _openFilesType;

        private FolderWebResourceOpenFilesCommand(
            OleMenuCommandService commandService
            , int idCommand
            , OpenFilesType openFilesType
            , bool inTextEditor
        ) : base(
            commandService
            , idCommand
        )
        {
            this._openFilesType = openFilesType;
            this._inTextEditor = inTextEditor;
        }

        private static TupleList<int, OpenFilesType, bool> _commandsFolder = new TupleList<int, OpenFilesType, bool>()
        {
            { PackageIds.guidCommandSet.FolderWebResourceOpenFilesAllCommandId, OpenFilesType.All, false }
            , { PackageIds.guidCommandSet.FolderWebResourceOpenFilesAllInTextEditorCommandId, OpenFilesType.All, true }

            , { PackageIds.guidCommandSet.FolderWebResourceOpenFilesNotEqualByTextCommandId, OpenFilesType.NotEqualByText, false }
            , { PackageIds.guidCommandSet.FolderWebResourceOpenFilesNotEqualByTextInTextEditorCommandId, OpenFilesType.NotEqualByText, true }

            , { PackageIds.guidCommandSet.FolderWebResourceOpenFilesEqualByTextCommandId, OpenFilesType.EqualByText, false }
            , { PackageIds.guidCommandSet.FolderWebResourceOpenFilesEqualByTextInTextEditorCommandId, OpenFilesType.EqualByText, true }

            , { PackageIds.guidCommandSet.FolderWebResourceOpenFilesNotExistsInCrmWithoutLinkCommandId, OpenFilesType.NotExistsInCrmWithoutLink, false }
            , { PackageIds.guidCommandSet.FolderWebResourceOpenFilesNotExistsInCrmWithoutLinkInTextEditorCommandId, OpenFilesType.NotExistsInCrmWithoutLink, true }

            , { PackageIds.guidCommandSet.FolderWebResourceOpenFilesNotExistsInCrmWithLinkCommandId, OpenFilesType.NotExistsInCrmWithLink, false }
            , { PackageIds.guidCommandSet.FolderWebResourceOpenFilesNotExistsInCrmWithLinkInTextEditorCommandId, OpenFilesType.NotExistsInCrmWithLink, true }

            , { PackageIds.guidCommandSet.FolderWebResourceOpenFilesWithInsertsCommandId, OpenFilesType.WithInserts, false }
            , { PackageIds.guidCommandSet.FolderWebResourceOpenFilesWithInsertsInTextEditorCommandId, OpenFilesType.WithInserts, true }

            , { PackageIds.guidCommandSet.FolderWebResourceOpenFilesWithDeletesCommandId, OpenFilesType.WithDeletes, false }
            , { PackageIds.guidCommandSet.FolderWebResourceOpenFilesWithDeletesInTextEditorCommandId, OpenFilesType.WithDeletes, true }

            , { PackageIds.guidCommandSet.FolderWebResourceOpenFilesWithComplexCommandId, OpenFilesType.WithComplexChanges, false }
            , { PackageIds.guidCommandSet.FolderWebResourceOpenFilesWithComplexInTextEditorCommandId, OpenFilesType.WithComplexChanges, true }

            , { PackageIds.guidCommandSet.FolderWebResourceOpenFilesWithMirrorCommandId, OpenFilesType.WithMirrorChanges, false }
            , { PackageIds.guidCommandSet.FolderWebResourceOpenFilesWithMirrorInTextEditorCommandId, OpenFilesType.WithMirrorChanges, true }

            , { PackageIds.guidCommandSet.FolderWebResourceOpenFilesWithMirrorInsertsCommandId, OpenFilesType.WithMirrorInserts, false }
            , { PackageIds.guidCommandSet.FolderWebResourceOpenFilesWithMirrorInsertsInTextEditorCommandId, OpenFilesType.WithMirrorInserts, true }

            , { PackageIds.guidCommandSet.FolderWebResourceOpenFilesWithMirrorDeletesCommandId, OpenFilesType.WithMirrorDeletes, false }
            , { PackageIds.guidCommandSet.FolderWebResourceOpenFilesWithMirrorDeletesInTextEditorCommandId, OpenFilesType.WithMirrorDeletes, true }

            , { PackageIds.guidCommandSet.FolderWebResourceOpenFilesWithMirrorComplexCommandId, OpenFilesType.WithMirrorComplexChanges, false }
            , { PackageIds.guidCommandSet.FolderWebResourceOpenFilesWithMirrorComplexInTextEditorCommandId, OpenFilesType.WithMirrorComplexChanges, true }
        };

        private static ConcurrentDictionary<Tuple<OpenFilesType, bool>, FolderWebResourceOpenFilesCommand> _instances = new ConcurrentDictionary<Tuple<OpenFilesType, bool>, FolderWebResourceOpenFilesCommand>();

        public static FolderWebResourceOpenFilesCommand Instance(OpenFilesType openFilesType, bool inTextEditor)
        {
            var key = Tuple.Create(openFilesType, inTextEditor);

            if (_instances.ContainsKey(key))
            {
                return _instances[key];
            }

            return null;
        }

        public static void Initialize(OleMenuCommandService commandService)
        {
            foreach (var item in _commandsFolder)
            {
                var command = new FolderWebResourceOpenFilesCommand(commandService, item.Item1, item.Item2, item.Item3);

                _instances.TryAdd(Tuple.Create(item.Item2, item.Item3), command);
            }
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, true).ToList();

            if (selectedFiles.Count > 0)
            {
                helper.HandleWebResourceOpenFilesCommand(selectedFiles, _openFilesType, _inTextEditor);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextRecursive(applicationObject, menuCommand);

            if (_inTextEditor)
            {
                CommonHandlers.ActionBeforeQueryStatusTextEditorProgramExists(applicationObject, menuCommand);
            }
        }
    }
}
