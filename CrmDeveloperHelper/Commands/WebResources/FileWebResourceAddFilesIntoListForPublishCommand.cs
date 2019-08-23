using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FileWebResourceAddFilesIntoListForPublishCommand : AbstractCommand
    {
        private readonly OpenFilesType _openFilesType;

        private FileWebResourceAddFilesIntoListForPublishCommand(
            OleMenuCommandService commandService
            , int idCommand
            , OpenFilesType openFilesType
        ) : base(commandService, idCommand)
        {
            this._openFilesType = openFilesType;
        }

        private static TupleList<int, OpenFilesType> _commandsFile = new TupleList<int, OpenFilesType>()
        {
              { PackageIds.guidCommandSet.FileWebResourceAddIntoPublishListFilesAllCommandId, OpenFilesType.All }
            , { PackageIds.guidCommandSet.FileWebResourceAddIntoPublishListFilesNotEqualByTextCommandId, OpenFilesType.NotEqualByText }
            , { PackageIds.guidCommandSet.FileWebResourceAddIntoPublishListFilesNotExistsInCrmWithoutLinkCommandId, OpenFilesType.NotExistsInCrmWithoutLink }
            , { PackageIds.guidCommandSet.FileWebResourceAddIntoPublishListFilesNotExistsInCrmWithLinkCommandId, OpenFilesType.NotExistsInCrmWithLink }
            , { PackageIds.guidCommandSet.FileWebResourceAddIntoPublishListFilesEqualByTextCommandId, OpenFilesType.EqualByText }
            , { PackageIds.guidCommandSet.FileWebResourceAddIntoPublishListFilesWithInsertsCommandId, OpenFilesType.WithInserts }
            , { PackageIds.guidCommandSet.FileWebResourceAddIntoPublishListFilesWithDeletesCommandId, OpenFilesType.WithDeletes }
            , { PackageIds.guidCommandSet.FileWebResourceAddIntoPublishListFilesWithComplexCommandId, OpenFilesType.WithComplex }
            , { PackageIds.guidCommandSet.FileWebResourceAddIntoPublishListFilesWithMirrorCommandId, OpenFilesType.WithMirror }
            , { PackageIds.guidCommandSet.FileWebResourceAddIntoPublishListFilesWithMirrorInsertsCommandId, OpenFilesType.WithMirrorInserts }
            , { PackageIds.guidCommandSet.FileWebResourceAddIntoPublishListFilesWithMirrorDeletesCommandId, OpenFilesType.WithMirrorDeletes }
            , { PackageIds.guidCommandSet.FileWebResourceAddIntoPublishListFilesWithMirrorComplexCommandId, OpenFilesType.WithMirrorComplex }
        };

        private static ConcurrentDictionary<OpenFilesType, FileWebResourceAddFilesIntoListForPublishCommand> _instances = new ConcurrentDictionary<OpenFilesType, FileWebResourceAddFilesIntoListForPublishCommand>();

        public static FileWebResourceAddFilesIntoListForPublishCommand Instance(OpenFilesType openFilesType)
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
                var command = new FileWebResourceAddFilesIntoListForPublishCommand(commandService, item.Item1, item.Item2);

                _instances.TryAdd(item.Item2, command);
            }
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, false).ToList();

            if (selectedFiles.Count > 0)
            {
                helper.HandleAddingIntoPublishListFilesByTypeCommand(selectedFiles, _openFilesType);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(applicationObject, menuCommand);
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceAny(applicationObject, menuCommand);
        }
    }
}
