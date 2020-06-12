using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class DocumentsWebResourceAddFilesIntoListForPublishCommand : AbstractSingleCommand
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
              { PackageIds.guidCommandSet.DocumentsWebResourceAddFilesIntoListForPublishAllCommandId, OpenFilesType.All }
            , { PackageIds.guidCommandSet.DocumentsWebResourceAddFilesIntoListForPublishNotEqualByTextCommandId, OpenFilesType.NotEqualByText }

            , { PackageIds.guidCommandSet.DocumentsWebResourceAddFilesIntoListForPublishNotExistsInCrmWithoutLinkCommandId, OpenFilesType.NotExistsInCrmWithoutLink }
            , { PackageIds.guidCommandSet.DocumentsWebResourceAddFilesIntoListForPublishNotExistsInCrmWithLinkCommandId, OpenFilesType.NotExistsInCrmWithLink }

            , { PackageIds.guidCommandSet.DocumentsWebResourceAddFilesIntoListForPublishEqualByTextCommandId, OpenFilesType.EqualByText }
            , { PackageIds.guidCommandSet.DocumentsWebResourceAddFilesIntoListForPublishWithInsertsCommandId, OpenFilesType.WithInserts }
            , { PackageIds.guidCommandSet.DocumentsWebResourceAddFilesIntoListForPublishWithDeletesCommandId, OpenFilesType.WithDeletes }
            , { PackageIds.guidCommandSet.DocumentsWebResourceAddFilesIntoListForPublishWithComplexCommandId, OpenFilesType.WithComplex }
            , { PackageIds.guidCommandSet.DocumentsWebResourceAddFilesIntoListForPublishWithMirrorCommandId, OpenFilesType.WithMirror }
            , { PackageIds.guidCommandSet.DocumentsWebResourceAddFilesIntoListForPublishWithMirrorInsertsCommandId, OpenFilesType.WithMirrorInserts }
            , { PackageIds.guidCommandSet.DocumentsWebResourceAddFilesIntoListForPublishWithMirrorDeletesCommandId, OpenFilesType.WithMirrorDeletes }
            , { PackageIds.guidCommandSet.DocumentsWebResourceAddFilesIntoListForPublishWithMirrorComplexCommandId, OpenFilesType.WithMirrorComplex }
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
