using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FileWebResourceMultiDifferenceCommand : AbstractSingleCommand
    {
        private readonly OpenFilesType _openFilesType;

        private FileWebResourceMultiDifferenceCommand(
            OleMenuCommandService commandService
            , int idCommand
            , OpenFilesType openFilesType
        ) : base(commandService, idCommand)
        {
            this._openFilesType = openFilesType;
        }

        private static TupleList<int, OpenFilesType> _commandsFile = new TupleList<int, OpenFilesType>()
        {
              { PackageIds.guidCommandSet.FileWebResourceMultiDifferenceFilesNotEqualByTextCommandId, OpenFilesType.NotEqualByText }
            , { PackageIds.guidCommandSet.FileWebResourceMultiDifferenceFilesNotExistsInCrmWithLinkCommandId, OpenFilesType.NotExistsInCrmWithLink }
            , { PackageIds.guidCommandSet.FileWebResourceMultiDifferenceFilesEqualByTextCommandId, OpenFilesType.EqualByText }
            , { PackageIds.guidCommandSet.FileWebResourceMultiDifferenceFilesWithInsertsCommandId, OpenFilesType.WithInserts }
            , { PackageIds.guidCommandSet.FileWebResourceMultiDifferenceFilesWithDeletesCommandId, OpenFilesType.WithDeletes }
            , { PackageIds.guidCommandSet.FileWebResourceMultiDifferenceFilesWithComplexCommandId, OpenFilesType.WithComplex }
            , { PackageIds.guidCommandSet.FileWebResourceMultiDifferenceFilesWithMirrorCommandId, OpenFilesType.WithMirror }
            , { PackageIds.guidCommandSet.FileWebResourceMultiDifferenceFilesWithMirrorInsertsCommandId, OpenFilesType.WithMirrorInserts }
            , { PackageIds.guidCommandSet.FileWebResourceMultiDifferenceFilesWithMirrorDeletesCommandId, OpenFilesType.WithMirrorDeletes }
            , { PackageIds.guidCommandSet.FileWebResourceMultiDifferenceFilesWithMirrorComplexCommandId, OpenFilesType.WithMirrorComplex }
        };

        private static ConcurrentDictionary<OpenFilesType, FileWebResourceMultiDifferenceCommand> _instances = new ConcurrentDictionary<OpenFilesType, FileWebResourceMultiDifferenceCommand>();

        public static FileWebResourceMultiDifferenceCommand Instance(OpenFilesType openFilesType)
        {
            if (_instances.ContainsKey(openFilesType))
            {
                return _instances[openFilesType];
            }

            return null;
        }

        public static void Initialize(OleMenuCommandService commandService)
        {
            foreach (var item in _commandsFile)
            {
                var command = new FileWebResourceMultiDifferenceCommand(commandService, item.Item1, item.Item2);

                _instances.TryAdd(item.Item2, command);
            }
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, false).ToList();

            if (selectedFiles.Count > 0)
            {
                helper.HandleWebResourceMultiDifferenceFiles(selectedFiles, _openFilesType);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextAny(applicationObject, menuCommand);
        }
    }
}
