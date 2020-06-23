using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.ListForPublish
{
    internal sealed class ListForPublishMultiDifferenceCommand : AbstractSingleCommand
    {
        private readonly OpenFilesType _openFilesType;

        private ListForPublishMultiDifferenceCommand(
            OleMenuCommandService commandService
            , int idCommand
            , OpenFilesType openFilesType
        ) : base(commandService, idCommand)
        {
            this._openFilesType = openFilesType;
        }

        private static TupleList<int, OpenFilesType> _commandsListforPublish = new TupleList<int, OpenFilesType>()
        {
              { PackageIds.guidCommandSet.ListForPublishMultiDifferenceFilesNotEqualByTextCommandId, OpenFilesType.NotEqualByText }
            , { PackageIds.guidCommandSet.ListForPublishMultiDifferenceFilesNotExistsInCrmWithLinkCommandId, OpenFilesType.NotExistsInCrmWithLink }
            , { PackageIds.guidCommandSet.ListForPublishMultiDifferenceFilesEqualByTextCommandId, OpenFilesType.EqualByText }
            , { PackageIds.guidCommandSet.ListForPublishMultiDifferenceFilesWithInsertsCommandId, OpenFilesType.WithInserts }
            , { PackageIds.guidCommandSet.ListForPublishMultiDifferenceFilesWithDeletesCommandId, OpenFilesType.WithDeletes }
            , { PackageIds.guidCommandSet.ListForPublishMultiDifferenceFilesWithComplexCommandId, OpenFilesType.WithComplexChanges }
            , { PackageIds.guidCommandSet.ListForPublishMultiDifferenceFilesWithMirrorCommandId, OpenFilesType.WithMirrorChanges }
            , { PackageIds.guidCommandSet.ListForPublishMultiDifferenceFilesWithMirrorInsertsCommandId, OpenFilesType.WithMirrorInserts }
            , { PackageIds.guidCommandSet.ListForPublishMultiDifferenceFilesWithMirrorDeletesCommandId, OpenFilesType.WithMirrorDeletes }
            , { PackageIds.guidCommandSet.ListForPublishMultiDifferenceFilesWithMirrorComplexCommandId, OpenFilesType.WithMirrorComplexChanges }
        };

        private static ConcurrentDictionary<OpenFilesType, ListForPublishMultiDifferenceCommand> _instances = new ConcurrentDictionary<OpenFilesType, ListForPublishMultiDifferenceCommand>();

        public static ListForPublishMultiDifferenceCommand Instance(OpenFilesType openFilesType)
        {
            if (_instances.ContainsKey(openFilesType))
            {
                return _instances[openFilesType];
            }

            return null;
        }

        public static void Initialize(OleMenuCommandService commandService)
        {
            foreach (var item in _commandsListforPublish)
            {
                var command = new ListForPublishMultiDifferenceCommand(commandService, item.Item1, item.Item2);

                _instances.TryAdd(item.Item2, command);
            }
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedFiles = helper.GetSelectedFilesFromListForPublish().ToList();

            if (selectedFiles.Any())
            {
                helper.ShowListForPublish(null);

                helper.HandleWebResourceMultiDifferenceFiles(selectedFiles, _openFilesType);
            }
            else
            {
                helper.WriteToOutput(null, Properties.OutputStrings.PublishListIsEmpty);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusListForPublishWebResourceTextAny(applicationObject, menuCommand);
        }
    }
}
