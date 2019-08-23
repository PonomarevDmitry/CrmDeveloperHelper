using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.ListForPublish
{
    internal sealed class ListForPublishOpenFilesCommand : AbstractCommand
    {
        private readonly bool _inTextEditor;
        private readonly OpenFilesType _openFilesType;

        private ListForPublishOpenFilesCommand(
            OleMenuCommandService commandService
            , int idCommand
            , OpenFilesType openFilesType
            , bool inTextEditor
        ) : base(commandService, idCommand)
        {
            this._openFilesType = openFilesType;
            this._inTextEditor = inTextEditor;
        }

        private static TupleList<int, OpenFilesType, bool> _commandsListforPublish = new TupleList<int, OpenFilesType, bool>()
        {
            { PackageIds.guidCommandSet.ListForPublishOpenFilesAllCommandId, OpenFilesType.All, false }
            , { PackageIds.guidCommandSet.ListForPublishOpenFilesAllInTextEditorCommandId, OpenFilesType.All, true }

            , { PackageIds.guidCommandSet.ListForPublishOpenFilesNotEqualByTextCommandId, OpenFilesType.NotEqualByText, false }
            , { PackageIds.guidCommandSet.ListForPublishOpenFilesNotEqualByTextInTextEditorCommandId, OpenFilesType.NotEqualByText, true }

            , { PackageIds.guidCommandSet.ListForPublishOpenFilesEqualByTextCommandId, OpenFilesType.EqualByText, false }
            , { PackageIds.guidCommandSet.ListForPublishOpenFilesEqualByTextInTextEditorCommandId, OpenFilesType.EqualByText, true }

            , { PackageIds.guidCommandSet.ListForPublishOpenFilesNotExistsInCrmWithoutLinkCommandId, OpenFilesType.NotExistsInCrmWithoutLink, false }
            , { PackageIds.guidCommandSet.ListForPublishOpenFilesNotExistsInCrmWithoutLinkInTextEditorCommandId, OpenFilesType.NotExistsInCrmWithoutLink, true }

            , { PackageIds.guidCommandSet.ListForPublishOpenFilesNotExistsInCrmWithLinkCommandId, OpenFilesType.NotExistsInCrmWithLink, false }
            , { PackageIds.guidCommandSet.ListForPublishOpenFilesNotExistsInCrmWithLinkInTextEditorCommandId, OpenFilesType.NotExistsInCrmWithLink, true }

            , { PackageIds.guidCommandSet.ListForPublishOpenFilesWithInsertsCommandId, OpenFilesType.WithInserts, false }
            , { PackageIds.guidCommandSet.ListForPublishOpenFilesWithInsertsInTextEditorCommandId, OpenFilesType.WithInserts, true }

            , { PackageIds.guidCommandSet.ListForPublishOpenFilesWithDeletesCommandId, OpenFilesType.WithDeletes, false }
            , { PackageIds.guidCommandSet.ListForPublishOpenFilesWithDeletesInTextEditorCommandId, OpenFilesType.WithDeletes, true }

            , { PackageIds.guidCommandSet.ListForPublishOpenFilesWithComplexCommandId, OpenFilesType.WithComplex, false }
            , { PackageIds.guidCommandSet.ListForPublishOpenFilesWithComplexInTextEditorCommandId, OpenFilesType.WithComplex, true }

            , { PackageIds.guidCommandSet.ListForPublishOpenFilesWithMirrorCommandId, OpenFilesType.WithMirror, false }
            , { PackageIds.guidCommandSet.ListForPublishOpenFilesWithMirrorInTextEditorCommandId, OpenFilesType.WithMirror, true }

            , { PackageIds.guidCommandSet.ListForPublishOpenFilesWithMirrorInsertsCommandId, OpenFilesType.WithMirrorInserts, false }
            , { PackageIds.guidCommandSet.ListForPublishOpenFilesWithMirrorInsertsInTextEditorCommandId, OpenFilesType.WithMirrorInserts, true }

            , { PackageIds.guidCommandSet.ListForPublishOpenFilesWithMirrorDeletesCommandId, OpenFilesType.WithMirrorDeletes, false }
            , { PackageIds.guidCommandSet.ListForPublishOpenFilesWithMirrorDeletesInTextEditorCommandId, OpenFilesType.WithMirrorDeletes, true }

            , { PackageIds.guidCommandSet.ListForPublishOpenFilesWithMirrorComplexCommandId, OpenFilesType.WithMirrorComplex, false }
            , { PackageIds.guidCommandSet.ListForPublishOpenFilesWithMirrorComplexInTextEditorCommandId, OpenFilesType.WithMirrorComplex, true }
        };

        private static ConcurrentDictionary<Tuple<OpenFilesType, bool>, ListForPublishOpenFilesCommand> _instances = new ConcurrentDictionary<Tuple<OpenFilesType, bool>, ListForPublishOpenFilesCommand>();

        public static ListForPublishOpenFilesCommand Instance(OpenFilesType openFilesType, bool inTextEditor)
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
            foreach (var item in _commandsListforPublish)
            {
                var command = new ListForPublishOpenFilesCommand(commandService, item.Item1, item.Item2, item.Item3);

                _instances.TryAdd(Tuple.Create(item.Item2, item.Item3), command);
            }
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = CommonHandlers.GetSelectedFilesInListForPublish(helper).ToList();

            if (selectedFiles.Count > 0)
            {
                helper.HandleOpenFilesCommand(selectedFiles, _openFilesType, _inTextEditor);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusListForPublishWebResourceTextAny(applicationObject, menuCommand);

            if (_inTextEditor)
            {
                CommonHandlers.ActionBeforeQueryStatusTextEditorProgramExists(applicationObject, menuCommand);
            }
        }
    }
}
