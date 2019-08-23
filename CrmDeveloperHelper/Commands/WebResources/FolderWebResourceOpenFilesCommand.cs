using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FolderWebResourceOpenFilesCommand : AbstractCommand
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
            { PackageIds.FolderWebResourceOpenFilesAllCommandId, OpenFilesType.All, false }
            , { PackageIds.FolderWebResourceOpenFilesAllInTextEditorCommandId, OpenFilesType.All, true }

            , { PackageIds.FolderWebResourceOpenFilesNotEqualByTextCommandId, OpenFilesType.NotEqualByText, false }
            , { PackageIds.FolderWebResourceOpenFilesNotEqualByTextInTextEditorCommandId, OpenFilesType.NotEqualByText, true }

            , { PackageIds.FolderWebResourceOpenFilesEqualByTextCommandId, OpenFilesType.EqualByText, false }
            , { PackageIds.FolderWebResourceOpenFilesEqualByTextInTextEditorCommandId, OpenFilesType.EqualByText, true }

            , { PackageIds.FolderWebResourceOpenFilesNotExistsInCrmWithoutLinkCommandId, OpenFilesType.NotExistsInCrmWithoutLink, false }
            , { PackageIds.FolderWebResourceOpenFilesNotExistsInCrmWithoutLinkInTextEditorCommandId, OpenFilesType.NotExistsInCrmWithoutLink, true }

            , { PackageIds.FolderWebResourceOpenFilesNotExistsInCrmWithLinkCommandId, OpenFilesType.NotExistsInCrmWithLink, false }
            , { PackageIds.FolderWebResourceOpenFilesNotExistsInCrmWithLinkInTextEditorCommandId, OpenFilesType.NotExistsInCrmWithLink, true }

            , { PackageIds.FolderWebResourceOpenFilesWithInsertsCommandId, OpenFilesType.WithInserts, false }
            , { PackageIds.FolderWebResourceOpenFilesWithInsertsInTextEditorCommandId, OpenFilesType.WithInserts, true }

            , { PackageIds.FolderWebResourceOpenFilesWithDeletesCommandId, OpenFilesType.WithDeletes, false }
            , { PackageIds.FolderWebResourceOpenFilesWithDeletesInTextEditorCommandId, OpenFilesType.WithDeletes, true }

            , { PackageIds.FolderWebResourceOpenFilesWithComplexCommandId, OpenFilesType.WithComplex, false }
            , { PackageIds.FolderWebResourceOpenFilesWithComplexInTextEditorCommandId, OpenFilesType.WithComplex, true }

            , { PackageIds.FolderWebResourceOpenFilesWithMirrorCommandId, OpenFilesType.WithMirror, false }
            , { PackageIds.FolderWebResourceOpenFilesWithMirrorInTextEditorCommandId, OpenFilesType.WithMirror, true }

            , { PackageIds.FolderWebResourceOpenFilesWithMirrorInsertsCommandId, OpenFilesType.WithMirrorInserts, false }
            , { PackageIds.FolderWebResourceOpenFilesWithMirrorInsertsInTextEditorCommandId, OpenFilesType.WithMirrorInserts, true }

            , { PackageIds.FolderWebResourceOpenFilesWithMirrorDeletesCommandId, OpenFilesType.WithMirrorDeletes, false }
            , { PackageIds.FolderWebResourceOpenFilesWithMirrorDeletesInTextEditorCommandId, OpenFilesType.WithMirrorDeletes, true }

            , { PackageIds.FolderWebResourceOpenFilesWithMirrorComplexCommandId, OpenFilesType.WithMirrorComplex, false }
            , { PackageIds.FolderWebResourceOpenFilesWithMirrorComplexInTextEditorCommandId, OpenFilesType.WithMirrorComplex, true }
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
                helper.HandleOpenFilesCommand(selectedFiles, _openFilesType, _inTextEditor);
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
