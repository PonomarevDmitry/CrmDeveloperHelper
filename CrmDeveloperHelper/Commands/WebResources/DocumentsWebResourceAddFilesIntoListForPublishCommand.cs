using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class DocumentsWebResourceAddFilesIntoListForPublishCommand : AbstractCommand
    {
        private readonly OpenFilesType _openFilesType;

        private DocumentsWebResourceAddFilesIntoListForPublishCommand(
            OleMenuCommandService commandService
            , int idCommand
            , OpenFilesType openFilesType
        ) : base(commandService, idCommand)
        {
            this._openFilesType = openFilesType;
        }

        private static TupleList<int, OpenFilesType> _commandsDocuments = new TupleList<int, OpenFilesType>()
        {
              { PackageIds.DocumentsWebResourceAddFilesIntoListForPublishAllCommandId, OpenFilesType.All }
            , { PackageIds.DocumentsWebResourceAddFilesIntoListForPublishNotEqualByTextCommandId, OpenFilesType.NotEqualByText }

            , { PackageIds.DocumentsWebResourceAddFilesIntoListForPublishNotExistsInCrmWithoutLinkCommandId, OpenFilesType.NotExistsInCrmWithoutLink }
            , { PackageIds.DocumentsWebResourceAddFilesIntoListForPublishNotExistsInCrmWithLinkCommandId, OpenFilesType.NotExistsInCrmWithLink }

            , { PackageIds.DocumentsWebResourceAddFilesIntoListForPublishEqualByTextCommandId, OpenFilesType.EqualByText }
            , { PackageIds.DocumentsWebResourceAddFilesIntoListForPublishWithInsertsCommandId, OpenFilesType.WithInserts }
            , { PackageIds.DocumentsWebResourceAddFilesIntoListForPublishWithDeletesCommandId, OpenFilesType.WithDeletes }
            , { PackageIds.DocumentsWebResourceAddFilesIntoListForPublishWithComplexCommandId, OpenFilesType.WithComplex }
            , { PackageIds.DocumentsWebResourceAddFilesIntoListForPublishWithMirrorCommandId, OpenFilesType.WithMirror }
            , { PackageIds.DocumentsWebResourceAddFilesIntoListForPublishWithMirrorInsertsCommandId, OpenFilesType.WithMirrorInserts }
            , { PackageIds.DocumentsWebResourceAddFilesIntoListForPublishWithMirrorDeletesCommandId, OpenFilesType.WithMirrorDeletes }
            , { PackageIds.DocumentsWebResourceAddFilesIntoListForPublishWithMirrorComplexCommandId, OpenFilesType.WithMirrorComplex }
        };

        private static ConcurrentDictionary<OpenFilesType, DocumentsWebResourceAddFilesIntoListForPublishCommand> _instances = new ConcurrentDictionary<OpenFilesType, DocumentsWebResourceAddFilesIntoListForPublishCommand>();

        public static DocumentsWebResourceAddFilesIntoListForPublishCommand Instance(OpenFilesType openFilesType)
        {
            if (_instances.ContainsKey(openFilesType))
            {
                return _instances[openFilesType];
            }

            return null;
        }

        public static void Initialize(OleMenuCommandService commandService)
        {
            foreach (var item in _commandsDocuments)
            {
                var command = new DocumentsWebResourceAddFilesIntoListForPublishCommand(commandService, item.Item1, item.Item2);

                _instances.TryAdd(item.Item2, command);
            }
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = CommonHandlers.GetOpenedDocuments(helper).ToList();

            if (selectedFiles.Count > 0)
            {
                helper.HandleAddingIntoPublishListFilesByTypeCommand(selectedFiles, _openFilesType);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(applicationObject, menuCommand);
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsWebResource(applicationObject, menuCommand);
        }
    }
}
