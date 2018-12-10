using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    class AddFilesIntoListForPublishCommand : IServiceProviderOwner
    {
        private readonly Func<DTEHelper, List<SelectedFile>> _listGetter;
        private readonly Package _package;
        private readonly Guid _guidCommandset;
        private readonly int _idCommand;
        private readonly Action<IServiceProviderOwner, OleMenuCommand> _actionBeforeQueryStatus;
        private readonly OpenFilesType _openFilesType;

        public IServiceProvider ServiceProvider => this._package;

        private AddFilesIntoListForPublishCommand(
            Package package
            , Guid guidCommandset
            , int idCommand
            , Func<DTEHelper, List<SelectedFile>> listGetter
            , OpenFilesType openFilesType
            , Action<IServiceProviderOwner, OleMenuCommand> actionBeforeQueryStatus
        )
        {
            this._package = package ?? throw new ArgumentNullException(nameof(package));
            this._actionBeforeQueryStatus = actionBeforeQueryStatus;
            this._guidCommandset = guidCommandset;
            this._idCommand = idCommand;
            this._listGetter = listGetter;
            this._openFilesType = openFilesType;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(this._guidCommandset, this._idCommand);
                var menuItem = new OleMenuCommand(this.MenuItemCallback, menuCommandID);
                if (actionBeforeQueryStatus != null)
                {
                    menuItem.BeforeQueryStatus += MenuItem_BeforeQueryStatus;
                }

                commandService.AddCommand(menuItem);
            }
        }

        private static TupleList<int, OpenFilesType> _commandsDocuments = new TupleList<int, OpenFilesType>()
        {
              { PackageIds.DocumentsAddIntoPublishListFilesAllCommandId, OpenFilesType.All }
            , { PackageIds.DocumentsAddIntoPublishListFilesNotEqualByTextCommandId, OpenFilesType.NotEqualByText }

            , { PackageIds.DocumentsAddIntoPublishListFilesNotExistsInCrmWithoutLinkCommandId, OpenFilesType.NotExistsInCrmWithoutLink }
            , { PackageIds.DocumentsAddIntoPublishListFilesNotExistsInCrmWithLinkCommandId, OpenFilesType.NotExistsInCrmWithLink }

            , { PackageIds.DocumentsAddIntoPublishListFilesEqualByTextCommandId, OpenFilesType.EqualByText }
            , { PackageIds.DocumentsAddIntoPublishListFilesWithInsertsCommandId, OpenFilesType.WithInserts }
            , { PackageIds.DocumentsAddIntoPublishListFilesWithDeletesCommandId, OpenFilesType.WithDeletes }
            , { PackageIds.DocumentsAddIntoPublishListFilesWithComplexCommandId, OpenFilesType.WithComplex }
            , { PackageIds.DocumentsAddIntoPublishListFilesWithMirrorCommandId, OpenFilesType.WithMirror }
            , { PackageIds.DocumentsAddIntoPublishListFilesWithMirrorInsertsCommandId, OpenFilesType.WithMirrorInserts }
            , { PackageIds.DocumentsAddIntoPublishListFilesWithMirrorDeletesCommandId, OpenFilesType.WithMirrorDeletes }
            , { PackageIds.DocumentsAddIntoPublishListFilesWithMirrorComplexCommandId, OpenFilesType.WithMirrorComplex }
        };

        private static TupleList<int, OpenFilesType> _commandsFile = new TupleList<int, OpenFilesType>()
        {
              { PackageIds.FileAddIntoPublishListFilesAllCommandId, OpenFilesType.All }
            , { PackageIds.FileAddIntoPublishListFilesNotEqualByTextCommandId, OpenFilesType.NotEqualByText }
            , { PackageIds.FileAddIntoPublishListFilesNotExistsInCrmWithoutLinkCommandId, OpenFilesType.NotExistsInCrmWithoutLink }
            , { PackageIds.FileAddIntoPublishListFilesNotExistsInCrmWithLinkCommandId, OpenFilesType.NotExistsInCrmWithLink }
            , { PackageIds.FileAddIntoPublishListFilesEqualByTextCommandId, OpenFilesType.EqualByText }
            , { PackageIds.FileAddIntoPublishListFilesWithInsertsCommandId, OpenFilesType.WithInserts }
            , { PackageIds.FileAddIntoPublishListFilesWithDeletesCommandId, OpenFilesType.WithDeletes }
            , { PackageIds.FileAddIntoPublishListFilesWithComplexCommandId, OpenFilesType.WithComplex }
            , { PackageIds.FileAddIntoPublishListFilesWithMirrorCommandId, OpenFilesType.WithMirror }
            , { PackageIds.FileAddIntoPublishListFilesWithMirrorInsertsCommandId, OpenFilesType.WithMirrorInserts }
            , { PackageIds.FileAddIntoPublishListFilesWithMirrorDeletesCommandId, OpenFilesType.WithMirrorDeletes }
            , { PackageIds.FileAddIntoPublishListFilesWithMirrorComplexCommandId, OpenFilesType.WithMirrorComplex }
        };

        private static TupleList<int, OpenFilesType> _commandsFolder = new TupleList<int, OpenFilesType>()
        {
              { PackageIds.FolderAddIntoPublishListFilesAllCommandId, OpenFilesType.All }
            , { PackageIds.FolderAddIntoPublishListFilesNotEqualByTextCommandId, OpenFilesType.NotEqualByText }
            , { PackageIds.FolderAddIntoPublishListFilesNotExistsInCrmWithoutLinkCommandId, OpenFilesType.NotExistsInCrmWithoutLink }
            , { PackageIds.FolderAddIntoPublishListFilesNotExistsInCrmWithLinkCommandId, OpenFilesType.NotExistsInCrmWithLink }
            , { PackageIds.FolderAddIntoPublishListFilesEqualByTextCommandId, OpenFilesType.EqualByText }
            , { PackageIds.FolderAddIntoPublishListFilesWithInsertsCommandId, OpenFilesType.WithInserts }
            , { PackageIds.FolderAddIntoPublishListFilesWithDeletesCommandId, OpenFilesType.WithDeletes }
            , { PackageIds.FolderAddIntoPublishListFilesWithComplexCommandId, OpenFilesType.WithComplex }
            , { PackageIds.FolderAddIntoPublishListFilesWithMirrorCommandId, OpenFilesType.WithMirror }
            , { PackageIds.FolderAddIntoPublishListFilesWithMirrorInsertsCommandId, OpenFilesType.WithMirrorInserts }
            , { PackageIds.FolderAddIntoPublishListFilesWithMirrorDeletesCommandId, OpenFilesType.WithMirrorDeletes }
            , { PackageIds.FolderAddIntoPublishListFilesWithMirrorComplexCommandId, OpenFilesType.WithMirrorComplex }
        };

        private static ConcurrentDictionary<Tuple<Func<DTEHelper, List<SelectedFile>>, OpenFilesType>, AddFilesIntoListForPublishCommand> _instances = new ConcurrentDictionary<Tuple<Func<DTEHelper, List<SelectedFile>>, OpenFilesType>, AddFilesIntoListForPublishCommand>();

        public static AddFilesIntoListForPublishCommand Instance(Func<DTEHelper, List<SelectedFile>> listGetter, OpenFilesType openFilesType)
        {
            var key = Tuple.Create(listGetter, openFilesType);

            if (_instances.ContainsKey(key))
            {
                return _instances[key];
            }

            return null;
        }

        public static void Initialize(Package package)
        {
            foreach (var item in _commandsDocuments)
            {
                var command = new AddFilesIntoListForPublishCommand(package, PackageGuids.guidCommandSet, item.Item1, CommonHandlers.GetOpenedDocuments, item.Item2, ActionBeforeQueryStatusActionBeforeQueryStatusOpenedDocumentsWebResourceConnectionIsNotReadOnly);

                _instances.TryAdd(Tuple.Create((Func<DTEHelper, List<SelectedFile>>)CommonHandlers.GetOpenedDocuments, item.Item2), command);
            }

            foreach (var item in _commandsFile)
            {
                var command = new AddFilesIntoListForPublishCommand(package, PackageGuids.guidCommandSet, item.Item1, CommonHandlers.GetSelectedFiles, item.Item2, ActionBeforeQueryStatusSolutionExplorerWebResourceAnyConnectionIsNotReadOnly);

                _instances.TryAdd(Tuple.Create((Func<DTEHelper, List<SelectedFile>>)CommonHandlers.GetSelectedFiles, item.Item2), command);
            }

            foreach (var item in _commandsFolder)
            {
                var command = new AddFilesIntoListForPublishCommand(package, PackageGuids.guidCommandSet, item.Item1, CommonHandlers.GetSelectedFilesRecursive, item.Item2, ActionBeforeQueryStatusSolutionExplorerWebResourceRecursiveConnectionIsNotReadOnly);

                _instances.TryAdd(Tuple.Create((Func<DTEHelper, List<SelectedFile>>)CommonHandlers.GetSelectedFilesRecursive, item.Item2), command);
            }
        }

        private static void ActionBeforeQueryStatusActionBeforeQueryStatusOpenedDocumentsWebResourceConnectionIsNotReadOnly(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            menuCommand.Enabled = menuCommand.Visible = true;

            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(command, menuCommand);
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsWebResource(command, menuCommand);
        }

        private static void ActionBeforeQueryStatusSolutionExplorerWebResourceAnyConnectionIsNotReadOnly(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            menuCommand.Enabled = menuCommand.Visible = true;

            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(command, menuCommand);
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceAny(command, menuCommand);
        }

        private static void ActionBeforeQueryStatusSolutionExplorerWebResourceRecursiveConnectionIsNotReadOnly(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            menuCommand.Enabled = menuCommand.Visible = true;

            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(command, menuCommand);
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceRecursive(command, menuCommand);
        }

        private void MenuItem_BeforeQueryStatus(object sender, EventArgs e)
        {
            try
            {
                if (sender is OleMenuCommand menuCommand)
                {
                    _actionBeforeQueryStatus?.Invoke(this, menuCommand);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(ex);
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

                var applicationObject = this.ServiceProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
                if (applicationObject == null)
                {
                    return;
                }

                var helper = DTEHelper.Create(applicationObject);

                List<SelectedFile> selectedFiles = _listGetter(helper);

                if (selectedFiles.Count > 0)
                {
                    helper.HandleAddingIntoPublishListFilesByTypeCommand(selectedFiles, _openFilesType);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(ex);
            }
        }
    }
}