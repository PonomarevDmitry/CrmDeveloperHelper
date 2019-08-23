using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FolderWebResourceAddFilesIntoListForPublishCommand : AbstractCommand
    {
        private readonly OpenFilesType _openFilesType;

        private FolderWebResourceAddFilesIntoListForPublishCommand(
            OleMenuCommandService commandService
            , int idCommand
            , OpenFilesType openFilesType
        ) : base(commandService, idCommand)
        {
            this._openFilesType = openFilesType;
        }

        private static TupleList<int, OpenFilesType> _commandsFolder = new TupleList<int, OpenFilesType>()
        {
              { PackageIds.FolderWebResourceAddIntoPublishListFilesAllCommandId, OpenFilesType.All }
            , { PackageIds.FolderWebResourceAddIntoPublishListFilesNotEqualByTextCommandId, OpenFilesType.NotEqualByText }
            , { PackageIds.FolderWebResourceAddIntoPublishListFilesNotExistsInCrmWithoutLinkCommandId, OpenFilesType.NotExistsInCrmWithoutLink }
            , { PackageIds.FolderWebResourceAddIntoPublishListFilesNotExistsInCrmWithLinkCommandId, OpenFilesType.NotExistsInCrmWithLink }
            , { PackageIds.FolderWebResourceAddIntoPublishListFilesEqualByTextCommandId, OpenFilesType.EqualByText }
            , { PackageIds.FolderWebResourceAddIntoPublishListFilesWithInsertsCommandId, OpenFilesType.WithInserts }
            , { PackageIds.FolderWebResourceAddIntoPublishListFilesWithDeletesCommandId, OpenFilesType.WithDeletes }
            , { PackageIds.FolderWebResourceAddIntoPublishListFilesWithComplexCommandId, OpenFilesType.WithComplex }
            , { PackageIds.FolderWebResourceAddIntoPublishListFilesWithMirrorCommandId, OpenFilesType.WithMirror }
            , { PackageIds.FolderWebResourceAddIntoPublishListFilesWithMirrorInsertsCommandId, OpenFilesType.WithMirrorInserts }
            , { PackageIds.FolderWebResourceAddIntoPublishListFilesWithMirrorDeletesCommandId, OpenFilesType.WithMirrorDeletes }
            , { PackageIds.FolderWebResourceAddIntoPublishListFilesWithMirrorComplexCommandId, OpenFilesType.WithMirrorComplex }
        };

        private static ConcurrentDictionary<OpenFilesType, FolderWebResourceAddFilesIntoListForPublishCommand> _instances = new ConcurrentDictionary<OpenFilesType, FolderWebResourceAddFilesIntoListForPublishCommand>();

        public static FolderWebResourceAddFilesIntoListForPublishCommand Instance(OpenFilesType openFilesType)
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
                var command = new FolderWebResourceAddFilesIntoListForPublishCommand(commandService, item.Item1, item.Item2);

                _instances.TryAdd(item.Item2, command);
            }
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, true).ToList();

            if (selectedFiles.Count > 0)
            {
                helper.HandleAddingIntoPublishListFilesByTypeCommand(selectedFiles, _openFilesType);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(applicationObject, menuCommand);
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceRecursive(applicationObject, menuCommand);
        }
    }
}
