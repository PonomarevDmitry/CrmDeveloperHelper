using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FolderWebResourceMultiDifferenceCommand : AbstractCommand
    {
        private readonly OpenFilesType _openFilesType;

        private FolderWebResourceMultiDifferenceCommand(
            OleMenuCommandService commandService
            , int idCommand
            , OpenFilesType openFilesType
        ) : base(commandService, idCommand)
        {
            this._openFilesType = openFilesType;
        }

        private static TupleList<int, OpenFilesType> _commandsFolder = new TupleList<int, OpenFilesType>()
        {
              { PackageIds.guidCommandSet.FolderWebResourceMultiDifferenceFilesNotEqualByTextCommandId, OpenFilesType.NotEqualByText }
            , { PackageIds.guidCommandSet.FolderWebResourceMultiDifferenceFilesNotExistsInCrmWithLinkCommandId, OpenFilesType.NotExistsInCrmWithLink }
            , { PackageIds.guidCommandSet.FolderWebResourceMultiDifferenceFilesEqualByTextCommandId, OpenFilesType.EqualByText }
            , { PackageIds.guidCommandSet.FolderWebResourceMultiDifferenceFilesWithInsertsCommandId, OpenFilesType.WithInserts }
            , { PackageIds.guidCommandSet.FolderWebResourceMultiDifferenceFilesWithDeletesCommandId, OpenFilesType.WithDeletes }
            , { PackageIds.guidCommandSet.FolderWebResourceMultiDifferenceFilesWithComplexCommandId, OpenFilesType.WithComplex }
            , { PackageIds.guidCommandSet.FolderWebResourceMultiDifferenceFilesWithMirrorCommandId, OpenFilesType.WithMirror }
            , { PackageIds.guidCommandSet.FolderWebResourceMultiDifferenceFilesWithMirrorInsertsCommandId, OpenFilesType.WithMirrorInserts }
            , { PackageIds.guidCommandSet.FolderWebResourceMultiDifferenceFilesWithMirrorDeletesCommandId, OpenFilesType.WithMirrorDeletes }
            , { PackageIds.guidCommandSet.FolderWebResourceMultiDifferenceFilesWithMirrorComplexCommandId, OpenFilesType.WithMirrorComplex }
        };

        private static ConcurrentDictionary<OpenFilesType, FolderWebResourceMultiDifferenceCommand> _instances = new ConcurrentDictionary<OpenFilesType, FolderWebResourceMultiDifferenceCommand>();

        public static FolderWebResourceMultiDifferenceCommand Instance(OpenFilesType openFilesType)
        {
            if (_instances.ContainsKey(openFilesType))
            {
                return _instances[openFilesType];
            }

            return null;
        }

        public static void Initialize(OleMenuCommandService commandService)
        {
            foreach (var item in _commandsFolder)
            {
                var command = new FolderWebResourceMultiDifferenceCommand(commandService, item.Item1, item.Item2);

                _instances.TryAdd(item.Item2, command);
            }
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, true).ToList();

            if (selectedFiles.Count > 0)
            {
                helper.HandleWebResourceMultiDifferenceFiles(selectedFiles, _openFilesType);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextRecursive(applicationObject, menuCommand);
        }
    }
}
