﻿using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class MultiDifferenceCommand
    {
        private readonly Func<DTEHelper, IEnumerable<SelectedFile>> _listGetter;
        private readonly Guid _guidCommandset;
        private readonly int _idCommand;
        private readonly Action<EnvDTE80.DTE2, OleMenuCommand> _actionBeforeQueryStatus;
        private readonly OpenFilesType _openFilesType;

        private MultiDifferenceCommand(
            OleMenuCommandService commandService
            , Guid guidCommandset
            , int idCommand
            , Func<DTEHelper, IEnumerable<SelectedFile>> listGetter
            , OpenFilesType openFilesType
            , Action<EnvDTE80.DTE2, OleMenuCommand> actionBeforeQueryStatus
        )
        {
            this._actionBeforeQueryStatus = actionBeforeQueryStatus;
            this._guidCommandset = guidCommandset;
            this._idCommand = idCommand;
            this._listGetter = listGetter;
            this._openFilesType = openFilesType;

            var menuCommandID = new CommandID(this._guidCommandset, this._idCommand);
            var menuItem = new OleMenuCommand(this.MenuItemCallback, menuCommandID);
            if (actionBeforeQueryStatus != null)
            {
                menuItem.BeforeQueryStatus += MenuItem_BeforeQueryStatus;
            }

            commandService.AddCommand(menuItem);
        }

        private static TupleList<int, OpenFilesType> _commandsListforPublish = new TupleList<int, OpenFilesType>()
        {
              { PackageIds.ListForPublishMultiDifferenceFilesNotEqualByTextCommandId, OpenFilesType.NotEqualByText }
            , { PackageIds.ListForPublishMultiDifferenceFilesNotExistsInCrmWithLinkCommandId, OpenFilesType.NotExistsInCrmWithLink }
            , { PackageIds.ListForPublishMultiDifferenceFilesEqualByTextCommandId, OpenFilesType.EqualByText }
            , { PackageIds.ListForPublishMultiDifferenceFilesWithInsertsCommandId, OpenFilesType.WithInserts }
            , { PackageIds.ListForPublishMultiDifferenceFilesWithDeletesCommandId, OpenFilesType.WithDeletes }
            , { PackageIds.ListForPublishMultiDifferenceFilesWithComplexCommandId, OpenFilesType.WithComplex }
            , { PackageIds.ListForPublishMultiDifferenceFilesWithMirrorCommandId, OpenFilesType.WithMirror }
            , { PackageIds.ListForPublishMultiDifferenceFilesWithMirrorInsertsCommandId, OpenFilesType.WithMirrorInserts }
            , { PackageIds.ListForPublishMultiDifferenceFilesWithMirrorDeletesCommandId, OpenFilesType.WithMirrorDeletes }
            , { PackageIds.ListForPublishMultiDifferenceFilesWithMirrorComplexCommandId, OpenFilesType.WithMirrorComplex }
        };

        private static TupleList<int, OpenFilesType> _commandsDocuments = new TupleList<int, OpenFilesType>()
        {
              { PackageIds.DocumentsMultiDifferenceFilesNotEqualByTextCommandId, OpenFilesType.NotEqualByText }
            , { PackageIds.DocumentsMultiDifferenceFilesNotExistsInCrmWithLinkCommandId, OpenFilesType.NotExistsInCrmWithLink }
            , { PackageIds.DocumentsMultiDifferenceFilesEqualByTextCommandId, OpenFilesType.EqualByText }
            , { PackageIds.DocumentsMultiDifferenceFilesWithInsertsCommandId, OpenFilesType.WithInserts }
            , { PackageIds.DocumentsMultiDifferenceFilesWithDeletesCommandId, OpenFilesType.WithDeletes }
            , { PackageIds.DocumentsMultiDifferenceFilesWithComplexCommandId, OpenFilesType.WithComplex }
            , { PackageIds.DocumentsMultiDifferenceFilesWithMirrorCommandId, OpenFilesType.WithMirror }
            , { PackageIds.DocumentsMultiDifferenceFilesWithMirrorInsertsCommandId, OpenFilesType.WithMirrorInserts }
            , { PackageIds.DocumentsMultiDifferenceFilesWithMirrorDeletesCommandId, OpenFilesType.WithMirrorDeletes }
            , { PackageIds.DocumentsMultiDifferenceFilesWithMirrorComplexCommandId, OpenFilesType.WithMirrorComplex }
        };

        private static TupleList<int, OpenFilesType> _commandsFile = new TupleList<int, OpenFilesType>()
        {
              { PackageIds.FileMultiDifferenceFilesNotEqualByTextCommandId, OpenFilesType.NotEqualByText }
            , { PackageIds.FileMultiDifferenceFilesNotExistsInCrmWithLinkCommandId, OpenFilesType.NotExistsInCrmWithLink }
            , { PackageIds.FileMultiDifferenceFilesEqualByTextCommandId, OpenFilesType.EqualByText }
            , { PackageIds.FileMultiDifferenceFilesWithInsertsCommandId, OpenFilesType.WithInserts }
            , { PackageIds.FileMultiDifferenceFilesWithDeletesCommandId, OpenFilesType.WithDeletes }
            , { PackageIds.FileMultiDifferenceFilesWithComplexCommandId, OpenFilesType.WithComplex }
            , { PackageIds.FileMultiDifferenceFilesWithMirrorCommandId, OpenFilesType.WithMirror }
            , { PackageIds.FileMultiDifferenceFilesWithMirrorInsertsCommandId, OpenFilesType.WithMirrorInserts }
            , { PackageIds.FileMultiDifferenceFilesWithMirrorDeletesCommandId, OpenFilesType.WithMirrorDeletes }
            , { PackageIds.FileMultiDifferenceFilesWithMirrorComplexCommandId, OpenFilesType.WithMirrorComplex }
        };

        private static TupleList<int, OpenFilesType> _commandsFolder = new TupleList<int, OpenFilesType>()
        {
              { PackageIds.FolderMultiDifferenceFilesNotEqualByTextCommandId, OpenFilesType.NotEqualByText }
            , { PackageIds.FolderMultiDifferenceFilesNotExistsInCrmWithLinkCommandId, OpenFilesType.NotExistsInCrmWithLink }
            , { PackageIds.FolderMultiDifferenceFilesEqualByTextCommandId, OpenFilesType.EqualByText }
            , { PackageIds.FolderMultiDifferenceFilesWithInsertsCommandId, OpenFilesType.WithInserts }
            , { PackageIds.FolderMultiDifferenceFilesWithDeletesCommandId, OpenFilesType.WithDeletes }
            , { PackageIds.FolderMultiDifferenceFilesWithComplexCommandId, OpenFilesType.WithComplex }
            , { PackageIds.FolderMultiDifferenceFilesWithMirrorCommandId, OpenFilesType.WithMirror }
            , { PackageIds.FolderMultiDifferenceFilesWithMirrorInsertsCommandId, OpenFilesType.WithMirrorInserts }
            , { PackageIds.FolderMultiDifferenceFilesWithMirrorDeletesCommandId, OpenFilesType.WithMirrorDeletes }
            , { PackageIds.FolderMultiDifferenceFilesWithMirrorComplexCommandId, OpenFilesType.WithMirrorComplex }
        };

        private static ConcurrentDictionary<Tuple<Func<DTEHelper, IEnumerable<SelectedFile>>, OpenFilesType>, MultiDifferenceCommand> _instances = new ConcurrentDictionary<Tuple<Func<DTEHelper, IEnumerable<SelectedFile>>, OpenFilesType>, MultiDifferenceCommand>();

        public static MultiDifferenceCommand Instance(Func<DTEHelper, IEnumerable<SelectedFile>> listGetter, OpenFilesType openFilesType)
        {
            var key = Tuple.Create(listGetter, openFilesType);

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
                var command = new MultiDifferenceCommand(commandService, PackageGuids.guidCommandSet, item.Item1, CommonHandlers.GetSelectedFilesInListForPublish, item.Item2, ActionBeforeQueryStatusListForPublishWebResourceTextAnyDifferenceProgramm);

                _instances.TryAdd(Tuple.Create((Func<DTEHelper, IEnumerable<SelectedFile>>)CommonHandlers.GetSelectedFilesInListForPublish, item.Item2), command);
            }

            foreach (var item in _commandsDocuments)
            {
                var command = new MultiDifferenceCommand(commandService, PackageGuids.guidCommandSet, item.Item1, CommonHandlers.GetOpenedDocuments, item.Item2, ActionBeforeQueryStatusOpenedDocumentsWebResourceTextDifferenceProgramm);

                _instances.TryAdd(Tuple.Create((Func<DTEHelper, IEnumerable<SelectedFile>>)CommonHandlers.GetOpenedDocuments, item.Item2), command);
            }

            foreach (var item in _commandsFile)
            {
                var command = new MultiDifferenceCommand(commandService, PackageGuids.guidCommandSet, item.Item1, CommonHandlers.GetSelectedFiles, item.Item2, ActionBeforeQueryStatusSolutionExplorerWebResourceTextAnyDifferenceProgramm);

                _instances.TryAdd(Tuple.Create((Func<DTEHelper, IEnumerable<SelectedFile>>)CommonHandlers.GetSelectedFiles, item.Item2), command);
            }

            foreach (var item in _commandsFolder)
            {
                var command = new MultiDifferenceCommand(commandService, PackageGuids.guidCommandSet, item.Item1, CommonHandlers.GetSelectedFilesRecursive, item.Item2, ActionBeforeQueryStatusSolutionExplorerWebResourceTextRecursiveDifferenceProgramm);

                _instances.TryAdd(Tuple.Create((Func<DTEHelper, IEnumerable<SelectedFile>>)CommonHandlers.GetSelectedFilesRecursive, item.Item2), command);
            }
        }

        private static void ActionBeforeQueryStatusListForPublishWebResourceTextAnyDifferenceProgramm(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusListForPublishWebResourceTextAny(applicationObject, menuCommand);
        }

        private static void ActionBeforeQueryStatusOpenedDocumentsWebResourceTextDifferenceProgramm(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsWebResourceText(applicationObject, menuCommand);
        }

        private static void ActionBeforeQueryStatusSolutionExplorerWebResourceTextAnyDifferenceProgramm(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextAny(applicationObject, menuCommand);
        }

        private static void ActionBeforeQueryStatusSolutionExplorerWebResourceTextRecursiveDifferenceProgramm(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextRecursive(applicationObject, menuCommand);
        }

        private void MenuItem_BeforeQueryStatus(object sender, EventArgs e)
        {
            try
            {
                if (sender is OleMenuCommand menuCommand)
                {

                    menuCommand.Enabled = menuCommand.Visible = false;

                    var applicationObject = CrmDeveloperHelperPackage.Singleton.ApplicationObject;
                    if (applicationObject == null)
                    {
                        return;
                    }

                    menuCommand.Enabled = menuCommand.Visible = true;

                    _actionBeforeQueryStatus?.Invoke(applicationObject, menuCommand);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            try
            {
                OleMenuCommand menuCommand = sender as OleMenuCommand;
                if (menuCommand == null)
                {
                    return;
                }

                var applicationObject = CrmDeveloperHelperPackage.Singleton.ApplicationObject;
                if (applicationObject == null)
                {
                    return;
                }

                var helper = DTEHelper.Create(applicationObject);

                List<SelectedFile> selectedFiles = _listGetter(helper).ToList();

                if (selectedFiles.Count > 0)
                {
                    helper.HandleMultiDifferenceFiles(selectedFiles, _openFilesType);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }
    }
}
