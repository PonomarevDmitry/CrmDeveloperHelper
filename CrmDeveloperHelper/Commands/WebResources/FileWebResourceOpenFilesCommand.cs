using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FileWebResourceOpenFilesCommand : AbstractCommand
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
            { PackageIds.FileWebResourceOpenFilesAllCommandId, OpenFilesType.All, false }
            , { PackageIds.FileWebResourceOpenFilesAllInTextEditorCommandId, OpenFilesType.All, true }

            , { PackageIds.FileWebResourceOpenFilesNotEqualByTextCommandId, OpenFilesType.NotEqualByText, false }
            , { PackageIds.FileWebResourceOpenFilesNotEqualByTextInTextEditorCommandId, OpenFilesType.NotEqualByText, true }

            , { PackageIds.FileWebResourceOpenFilesEqualByTextCommandId, OpenFilesType.EqualByText, false }
            , { PackageIds.FileWebResourceOpenFilesEqualByTextInTextEditorCommandId, OpenFilesType.EqualByText, true }

            , { PackageIds.FileWebResourceOpenFilesNotExistsInCrmWithoutLinkCommandId, OpenFilesType.NotExistsInCrmWithoutLink, false }
            , { PackageIds.FileWebResourceOpenFilesNotExistsInCrmWithoutLinkInTextEditorCommandId, OpenFilesType.NotExistsInCrmWithoutLink, true }

            , { PackageIds.FileWebResourceOpenFilesNotExistsInCrmWithLinkCommandId, OpenFilesType.NotExistsInCrmWithLink, false }
            , { PackageIds.FileWebResourceOpenFilesNotExistsInCrmWithLinkInTextEditorCommandId, OpenFilesType.NotExistsInCrmWithLink, true }

            , { PackageIds.FileWebResourceOpenFilesWithInsertsCommandId, OpenFilesType.WithInserts, false }
            , { PackageIds.FileWebResourceOpenFilesWithInsertsInTextEditorCommandId, OpenFilesType.WithInserts, true }

            , { PackageIds.FileWebResourceOpenFilesWithDeletesCommandId, OpenFilesType.WithDeletes, false }
            , { PackageIds.FileWebResourceOpenFilesWithDeletesInTextEditorCommandId, OpenFilesType.WithDeletes, true }

            , { PackageIds.FileWebResourceOpenFilesWithComplexCommandId, OpenFilesType.WithComplex, false }
            , { PackageIds.FileWebResourceOpenFilesWithComplexInTextEditorCommandId, OpenFilesType.WithComplex, true }

            , { PackageIds.FileWebResourceOpenFilesWithMirrorCommandId, OpenFilesType.WithMirror, false }
            , { PackageIds.FileWebResourceOpenFilesWithMirrorInTextEditorCommandId, OpenFilesType.WithMirror, true }

            , { PackageIds.FileWebResourceOpenFilesWithMirrorInsertsCommandId, OpenFilesType.WithMirrorInserts, false }
            , { PackageIds.FileWebResourceOpenFilesWithMirrorInsertsInTextEditorCommandId, OpenFilesType.WithMirrorInserts, true }

            , { PackageIds.FileWebResourceOpenFilesWithMirrorDeletesCommandId, OpenFilesType.WithMirrorDeletes, false }
            , { PackageIds.FileWebResourceOpenFilesWithMirrorDeletesInTextEditorCommandId, OpenFilesType.WithMirrorDeletes, true }

            , { PackageIds.FileWebResourceOpenFilesWithMirrorComplexCommandId, OpenFilesType.WithMirrorComplex, false }
            , { PackageIds.FileWebResourceOpenFilesWithMirrorComplexInTextEditorCommandId, OpenFilesType.WithMirrorComplex, true }
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
                helper.HandleOpenFilesCommand(selectedFiles, _openFilesType, _inTextEditor);
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
