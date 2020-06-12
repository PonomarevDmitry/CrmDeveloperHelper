using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class DocumentsWebResourceMultiDifferenceCommand : AbstractSingleCommand
    {
        private readonly OpenFilesType _openFilesType;

        private DocumentsWebResourceMultiDifferenceCommand(
            OleMenuCommandService commandService
            , int idCommand
            , OpenFilesType openFilesType
        ) : base(commandService, idCommand)
        {
            this._openFilesType = openFilesType;
        }

        private static TupleList<int, OpenFilesType> _commandsDocuments = new TupleList<int, OpenFilesType>()
        {
              { PackageIds.guidCommandSet.DocumentsWebResourceMultiDifferenceFilesNotEqualByTextCommandId, OpenFilesType.NotEqualByText }
            , { PackageIds.guidCommandSet.DocumentsWebResourceMultiDifferenceFilesNotExistsInCrmWithLinkCommandId, OpenFilesType.NotExistsInCrmWithLink }
            , { PackageIds.guidCommandSet.DocumentsWebResourceMultiDifferenceFilesEqualByTextCommandId, OpenFilesType.EqualByText }
            , { PackageIds.guidCommandSet.DocumentsWebResourceMultiDifferenceFilesWithInsertsCommandId, OpenFilesType.WithInserts }
            , { PackageIds.guidCommandSet.DocumentsWebResourceMultiDifferenceFilesWithDeletesCommandId, OpenFilesType.WithDeletes }
            , { PackageIds.guidCommandSet.DocumentsWebResourceMultiDifferenceFilesWithComplexCommandId, OpenFilesType.WithComplex }
            , { PackageIds.guidCommandSet.DocumentsWebResourceMultiDifferenceFilesWithMirrorCommandId, OpenFilesType.WithMirror }
            , { PackageIds.guidCommandSet.DocumentsWebResourceMultiDifferenceFilesWithMirrorInsertsCommandId, OpenFilesType.WithMirrorInserts }
            , { PackageIds.guidCommandSet.DocumentsWebResourceMultiDifferenceFilesWithMirrorDeletesCommandId, OpenFilesType.WithMirrorDeletes }
            , { PackageIds.guidCommandSet.DocumentsWebResourceMultiDifferenceFilesWithMirrorComplexCommandId, OpenFilesType.WithMirrorComplex }
        };

        private static ConcurrentDictionary<OpenFilesType, DocumentsWebResourceMultiDifferenceCommand> _instances = new ConcurrentDictionary<OpenFilesType, DocumentsWebResourceMultiDifferenceCommand>();

        public static DocumentsWebResourceMultiDifferenceCommand Instance(OpenFilesType openFilesType)
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
                var command = new DocumentsWebResourceMultiDifferenceCommand(commandService, item.Item1, item.Item2);

                _instances.TryAdd(item.Item2, command);
            }
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedDocuments(FileOperations.SupportsWebResourceTextType).ToList();

            if (selectedFiles.Count > 0)
            {
                helper.HandleWebResourceMultiDifferenceFiles(selectedFiles, _openFilesType);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsWebResourceText(applicationObject, menuCommand);
        }
    }
}
