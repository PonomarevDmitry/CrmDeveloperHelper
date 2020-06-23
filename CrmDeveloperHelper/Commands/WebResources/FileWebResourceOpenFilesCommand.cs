using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FileWebResourceOpenFilesCommand : AbstractSingleCommand
    {
        private readonly OpenFilesType _openFilesType;
        private readonly bool _inTextEditor;

        private FileWebResourceOpenFilesCommand(
            OleMenuCommandService commandService
            , int idCommand
            , OpenFilesType openFilesType
            , bool inTextEditor
        ) : base(commandService, idCommand)
        {
            this._openFilesType = openFilesType;
            this._inTextEditor = inTextEditor;
        }

        private static TupleList<int, OpenFilesType, bool> _commandsFile = new TupleList<int, OpenFilesType, bool>()
        {
            { PackageIds.guidCommandSet.FileWebResourceOpenFilesAllCommandId, OpenFilesType.All, false }
            , { PackageIds.guidCommandSet.FileWebResourceOpenFilesAllInTextEditorCommandId, OpenFilesType.All, true }

            , { PackageIds.guidCommandSet.FileWebResourceOpenFilesNotEqualByTextCommandId, OpenFilesType.NotEqualByText, false }
            , { PackageIds.guidCommandSet.FileWebResourceOpenFilesNotEqualByTextInTextEditorCommandId, OpenFilesType.NotEqualByText, true }

            , { PackageIds.guidCommandSet.FileWebResourceOpenFilesEqualByTextCommandId, OpenFilesType.EqualByText, false }
            , { PackageIds.guidCommandSet.FileWebResourceOpenFilesEqualByTextInTextEditorCommandId, OpenFilesType.EqualByText, true }

            , { PackageIds.guidCommandSet.FileWebResourceOpenFilesNotExistsInCrmWithoutLinkCommandId, OpenFilesType.NotExistsInCrmWithoutLink, false }
            , { PackageIds.guidCommandSet.FileWebResourceOpenFilesNotExistsInCrmWithoutLinkInTextEditorCommandId, OpenFilesType.NotExistsInCrmWithoutLink, true }

            , { PackageIds.guidCommandSet.FileWebResourceOpenFilesNotExistsInCrmWithLinkCommandId, OpenFilesType.NotExistsInCrmWithLink, false }
            , { PackageIds.guidCommandSet.FileWebResourceOpenFilesNotExistsInCrmWithLinkInTextEditorCommandId, OpenFilesType.NotExistsInCrmWithLink, true }

            , { PackageIds.guidCommandSet.FileWebResourceOpenFilesWithInsertsCommandId, OpenFilesType.WithInserts, false }
            , { PackageIds.guidCommandSet.FileWebResourceOpenFilesWithInsertsInTextEditorCommandId, OpenFilesType.WithInserts, true }

            , { PackageIds.guidCommandSet.FileWebResourceOpenFilesWithDeletesCommandId, OpenFilesType.WithDeletes, false }
            , { PackageIds.guidCommandSet.FileWebResourceOpenFilesWithDeletesInTextEditorCommandId, OpenFilesType.WithDeletes, true }

            , { PackageIds.guidCommandSet.FileWebResourceOpenFilesWithComplexCommandId, OpenFilesType.WithComplexChanges, false }
            , { PackageIds.guidCommandSet.FileWebResourceOpenFilesWithComplexInTextEditorCommandId, OpenFilesType.WithComplexChanges, true }

            , { PackageIds.guidCommandSet.FileWebResourceOpenFilesWithMirrorCommandId, OpenFilesType.WithMirrorChanges, false }
            , { PackageIds.guidCommandSet.FileWebResourceOpenFilesWithMirrorInTextEditorCommandId, OpenFilesType.WithMirrorChanges, true }

            , { PackageIds.guidCommandSet.FileWebResourceOpenFilesWithMirrorInsertsCommandId, OpenFilesType.WithMirrorInserts, false }
            , { PackageIds.guidCommandSet.FileWebResourceOpenFilesWithMirrorInsertsInTextEditorCommandId, OpenFilesType.WithMirrorInserts, true }

            , { PackageIds.guidCommandSet.FileWebResourceOpenFilesWithMirrorDeletesCommandId, OpenFilesType.WithMirrorDeletes, false }
            , { PackageIds.guidCommandSet.FileWebResourceOpenFilesWithMirrorDeletesInTextEditorCommandId, OpenFilesType.WithMirrorDeletes, true }

            , { PackageIds.guidCommandSet.FileWebResourceOpenFilesWithMirrorComplexCommandId, OpenFilesType.WithMirrorComplexChanges, false }
            , { PackageIds.guidCommandSet.FileWebResourceOpenFilesWithMirrorComplexInTextEditorCommandId, OpenFilesType.WithMirrorComplexChanges, true }
        };

        private static ConcurrentDictionary<Tuple<OpenFilesType, bool>, FileWebResourceOpenFilesCommand> _instances = new ConcurrentDictionary<Tuple<OpenFilesType, bool>, FileWebResourceOpenFilesCommand>();

        public static FileWebResourceOpenFilesCommand Instance(OpenFilesType openFilesType, bool inTextEditor)
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
            foreach (var item in _commandsFile)
            {
                var command = new FileWebResourceOpenFilesCommand(commandService, item.Item1, item.Item2, item.Item3);

                _instances.TryAdd(Tuple.Create(item.Item2, item.Item3), command);
            }
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, false).ToList();

            if (selectedFiles.Count > 0)
            {
                helper.HandleWebResourceOpenFilesCommand(selectedFiles, _openFilesType, _inTextEditor);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextAny(applicationObject, menuCommand);

            if (_inTextEditor)
            {
                CommonHandlers.ActionBeforeQueryStatusTextEditorProgramExists(applicationObject, menuCommand);
            }
        }
    }
}
