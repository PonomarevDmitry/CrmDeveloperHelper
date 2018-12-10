using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Collections.Concurrent;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class OpenFilesCommand : IServiceProviderOwner
    {
        private readonly Func<DTEHelper, List<SelectedFile>> _listGetter;
        private readonly Package _package;
        private readonly Guid _guidCommandset;
        private readonly int _idCommand;
        private readonly Action<IServiceProviderOwner, OleMenuCommand> _actionBeforeQueryStatus;
        private readonly bool _inTextEditor;
        private readonly OpenFilesType _openFilesType;

        public IServiceProvider ServiceProvider => this._package;

        private OpenFilesCommand(
            Package package
            , Guid guidCommandset
            , int idCommand
            , Func<DTEHelper, List<SelectedFile>> listGetter
            , OpenFilesType openFilesType
            , Action<IServiceProviderOwner, OleMenuCommand> actionBeforeQueryStatus
            , bool inTextEditor
        )
        {
            this._package = package ?? throw new ArgumentNullException(nameof(package));
            this._actionBeforeQueryStatus = actionBeforeQueryStatus;
            this._guidCommandset = guidCommandset;
            this._idCommand = idCommand;
            this._listGetter = listGetter;
            this._openFilesType = openFilesType;
            this._inTextEditor = inTextEditor;

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

        private static TupleList<int, OpenFilesType, bool> _commandsListforPublish = new TupleList<int, OpenFilesType, bool>()
        {
            { PackageIds.ListForPublishOpenFilesAllCommandId, OpenFilesType.All, false }
            , { PackageIds.ListForPublishOpenFilesAllInTextEditorCommandId, OpenFilesType.All, true }

            , { PackageIds.ListForPublishOpenFilesNotEqualByTextCommandId, OpenFilesType.NotEqualByText, false }
            , { PackageIds.ListForPublishOpenFilesNotEqualByTextInTextEditorCommandId, OpenFilesType.NotEqualByText, true }

            , { PackageIds.ListForPublishOpenFilesEqualByTextCommandId, OpenFilesType.EqualByText, false }
            , { PackageIds.ListForPublishOpenFilesEqualByTextInTextEditorCommandId, OpenFilesType.EqualByText, true }

            , { PackageIds.ListForPublishOpenFilesNotExistsInCrmWithoutLinkCommandId, OpenFilesType.NotExistsInCrmWithoutLink, false }
            , { PackageIds.ListForPublishOpenFilesNotExistsInCrmWithoutLinkInTextEditorCommandId, OpenFilesType.NotExistsInCrmWithoutLink, true }

            , { PackageIds.ListForPublishOpenFilesNotExistsInCrmWithLinkCommandId, OpenFilesType.NotExistsInCrmWithLink, false }
            , { PackageIds.ListForPublishOpenFilesNotExistsInCrmWithLinkInTextEditorCommandId, OpenFilesType.NotExistsInCrmWithLink, true }

            , { PackageIds.ListForPublishOpenFilesWithInsertsCommandId, OpenFilesType.WithInserts, false }
            , { PackageIds.ListForPublishOpenFilesWithInsertsInTextEditorCommandId, OpenFilesType.WithInserts, true }

            , { PackageIds.ListForPublishOpenFilesWithDeletesCommandId, OpenFilesType.WithDeletes, false }
            , { PackageIds.ListForPublishOpenFilesWithDeletesInTextEditorCommandId, OpenFilesType.WithDeletes, true }

            , { PackageIds.ListForPublishOpenFilesWithComplexCommandId, OpenFilesType.WithComplex, false }
            , { PackageIds.ListForPublishOpenFilesWithComplexInTextEditorCommandId, OpenFilesType.WithComplex, true }

            , { PackageIds.ListForPublishOpenFilesWithMirrorCommandId, OpenFilesType.WithMirror, false }
            , { PackageIds.ListForPublishOpenFilesWithMirrorInTextEditorCommandId, OpenFilesType.WithMirror, true }

            , { PackageIds.ListForPublishOpenFilesWithMirrorInsertsCommandId, OpenFilesType.WithMirrorInserts, false }
            , { PackageIds.ListForPublishOpenFilesWithMirrorInsertsInTextEditorCommandId, OpenFilesType.WithMirrorInserts, true }

            , { PackageIds.ListForPublishOpenFilesWithMirrorDeletesCommandId, OpenFilesType.WithMirrorDeletes, false }
            , { PackageIds.ListForPublishOpenFilesWithMirrorDeletesInTextEditorCommandId, OpenFilesType.WithMirrorDeletes, true }

            , { PackageIds.ListForPublishOpenFilesWithMirrorComplexCommandId, OpenFilesType.WithMirrorComplex, false }
            , { PackageIds.ListForPublishOpenFilesWithMirrorComplexInTextEditorCommandId, OpenFilesType.WithMirrorComplex, true }
        };

        private static TupleList<int, OpenFilesType, bool> _commandsFile = new TupleList<int, OpenFilesType, bool>()
        {
            { PackageIds.FileOpenFilesAllCommandId, OpenFilesType.All, false }
            , { PackageIds.FileOpenFilesAllInTextEditorCommandId, OpenFilesType.All, true }

            , { PackageIds.FileOpenFilesNotEqualByTextCommandId, OpenFilesType.NotEqualByText, false }
            , { PackageIds.FileOpenFilesNotEqualByTextInTextEditorCommandId, OpenFilesType.NotEqualByText, true }

            , { PackageIds.FileOpenFilesEqualByTextCommandId, OpenFilesType.EqualByText, false }
            , { PackageIds.FileOpenFilesEqualByTextInTextEditorCommandId, OpenFilesType.EqualByText, true }

            , { PackageIds.FileOpenFilesNotExistsInCrmWithoutLinkCommandId, OpenFilesType.NotExistsInCrmWithoutLink, false }
            , { PackageIds.FileOpenFilesNotExistsInCrmWithoutLinkInTextEditorCommandId, OpenFilesType.NotExistsInCrmWithoutLink, true }

            , { PackageIds.FileOpenFilesNotExistsInCrmWithLinkCommandId, OpenFilesType.NotExistsInCrmWithLink, false }
            , { PackageIds.FileOpenFilesNotExistsInCrmWithLinkInTextEditorCommandId, OpenFilesType.NotExistsInCrmWithLink, true }

            , { PackageIds.FileOpenFilesWithInsertsCommandId, OpenFilesType.WithInserts, false }
            , { PackageIds.FileOpenFilesWithInsertsInTextEditorCommandId, OpenFilesType.WithInserts, true }

            , { PackageIds.FileOpenFilesWithDeletesCommandId, OpenFilesType.WithDeletes, false }
            , { PackageIds.FileOpenFilesWithDeletesInTextEditorCommandId, OpenFilesType.WithDeletes, true }

            , { PackageIds.FileOpenFilesWithComplexCommandId, OpenFilesType.WithComplex, false }
            , { PackageIds.FileOpenFilesWithComplexInTextEditorCommandId, OpenFilesType.WithComplex, true }

            , { PackageIds.FileOpenFilesWithMirrorCommandId, OpenFilesType.WithMirror, false }
            , { PackageIds.FileOpenFilesWithMirrorInTextEditorCommandId, OpenFilesType.WithMirror, true }

            , { PackageIds.FileOpenFilesWithMirrorInsertsCommandId, OpenFilesType.WithMirrorInserts, false }
            , { PackageIds.FileOpenFilesWithMirrorInsertsInTextEditorCommandId, OpenFilesType.WithMirrorInserts, true }

            , { PackageIds.FileOpenFilesWithMirrorDeletesCommandId, OpenFilesType.WithMirrorDeletes, false }
            , { PackageIds.FileOpenFilesWithMirrorDeletesInTextEditorCommandId, OpenFilesType.WithMirrorDeletes, true }

            , { PackageIds.FileOpenFilesWithMirrorComplexCommandId, OpenFilesType.WithMirrorComplex, false }
            , { PackageIds.FileOpenFilesWithMirrorComplexInTextEditorCommandId, OpenFilesType.WithMirrorComplex, true }
        };

        private static TupleList<int, OpenFilesType, bool> _commandsFolder = new TupleList<int, OpenFilesType, bool>()
        {
            { PackageIds.FolderOpenFilesAllCommandId, OpenFilesType.All, false }
            , { PackageIds.FolderOpenFilesAllInTextEditorCommandId, OpenFilesType.All, true }

            , { PackageIds.FolderOpenFilesNotEqualByTextCommandId, OpenFilesType.NotEqualByText, false }
            , { PackageIds.FolderOpenFilesNotEqualByTextInTextEditorCommandId, OpenFilesType.NotEqualByText, true }

            , { PackageIds.FolderOpenFilesEqualByTextCommandId, OpenFilesType.EqualByText, false }
            , { PackageIds.FolderOpenFilesEqualByTextInTextEditorCommandId, OpenFilesType.EqualByText, true }

            , { PackageIds.FolderOpenFilesNotExistsInCrmWithoutLinkCommandId, OpenFilesType.NotExistsInCrmWithoutLink, false }
            , { PackageIds.FolderOpenFilesNotExistsInCrmWithoutLinkInTextEditorCommandId, OpenFilesType.NotExistsInCrmWithoutLink, true }

            , { PackageIds.FolderOpenFilesNotExistsInCrmWithLinkCommandId, OpenFilesType.NotExistsInCrmWithLink, false }
            , { PackageIds.FolderOpenFilesNotExistsInCrmWithLinkInTextEditorCommandId, OpenFilesType.NotExistsInCrmWithLink, true }

            , { PackageIds.FolderOpenFilesWithInsertsCommandId, OpenFilesType.WithInserts, false }
            , { PackageIds.FolderOpenFilesWithInsertsInTextEditorCommandId, OpenFilesType.WithInserts, true }

            , { PackageIds.FolderOpenFilesWithDeletesCommandId, OpenFilesType.WithDeletes, false }
            , { PackageIds.FolderOpenFilesWithDeletesInTextEditorCommandId, OpenFilesType.WithDeletes, true }

            , { PackageIds.FolderOpenFilesWithComplexCommandId, OpenFilesType.WithComplex, false }
            , { PackageIds.FolderOpenFilesWithComplexInTextEditorCommandId, OpenFilesType.WithComplex, true }

            , { PackageIds.FolderOpenFilesWithMirrorCommandId, OpenFilesType.WithMirror, false }
            , { PackageIds.FolderOpenFilesWithMirrorInTextEditorCommandId, OpenFilesType.WithMirror, true }

            , { PackageIds.FolderOpenFilesWithMirrorInsertsCommandId, OpenFilesType.WithMirrorInserts, false }
            , { PackageIds.FolderOpenFilesWithMirrorInsertsInTextEditorCommandId, OpenFilesType.WithMirrorInserts, true }

            , { PackageIds.FolderOpenFilesWithMirrorDeletesCommandId, OpenFilesType.WithMirrorDeletes, false }
            , { PackageIds.FolderOpenFilesWithMirrorDeletesInTextEditorCommandId, OpenFilesType.WithMirrorDeletes, true }

            , { PackageIds.FolderOpenFilesWithMirrorComplexCommandId, OpenFilesType.WithMirrorComplex, false }
            , { PackageIds.FolderOpenFilesWithMirrorComplexInTextEditorCommandId, OpenFilesType.WithMirrorComplex, true }
        };

        private static ConcurrentDictionary<Tuple<Func<DTEHelper, List<SelectedFile>>, OpenFilesType, bool>, OpenFilesCommand> _instances = new ConcurrentDictionary<Tuple<Func<DTEHelper, List<SelectedFile>>, OpenFilesType, bool>, OpenFilesCommand>();

        public static OpenFilesCommand Instance(Func<DTEHelper, List<SelectedFile>> listGetter, OpenFilesType openFilesType, bool inTextEditor)
        {
            var key = Tuple.Create(listGetter, openFilesType, inTextEditor);

            if (_instances.ContainsKey(key))
            {
                return _instances[key];
            }

            return null;
        }

        private static void ActionBeforeQueryStatusListForPublishWebResourceTextAnyInTextEditor(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            menuCommand.Enabled = menuCommand.Visible = true;

            CommonHandlers.ActionBeforeQueryStatusTextEditorProgramExists(command, menuCommand);
            CommonHandlers.ActionBeforeQueryStatusListForPublishWebResourceTextAny(command, menuCommand);
        }

        private static void ActionBeforeQueryStatusSolutionExplorerWebResourceTextAnyInTextEditor(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            menuCommand.Enabled = menuCommand.Visible = true;

            CommonHandlers.ActionBeforeQueryStatusTextEditorProgramExists(command, menuCommand);
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextAny(command, menuCommand);
        }

        private static void ActionBeforeQueryStatusSolutionExplorerWebResourceTextRecursiveInTextEditor(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            menuCommand.Enabled = menuCommand.Visible = true;

            CommonHandlers.ActionBeforeQueryStatusTextEditorProgramExists(command, menuCommand);
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextRecursive(command, menuCommand);
        }

        internal static void ActionBeforeQueryStatusListForPublishWebResourceTextAny(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            menuCommand.Enabled = menuCommand.Visible = true;

            CommonHandlers.ActionBeforeQueryStatusListForPublishWebResourceTextAny(command, menuCommand);
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerWebResourceTextAny(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            menuCommand.Enabled = menuCommand.Visible = true;

            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextAny(command, menuCommand);
        }

        internal static void ActionBeforeQueryStatusSolutionExplorerWebResourceTextRecursive(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            menuCommand.Enabled = menuCommand.Visible = true;

            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextRecursive(command, menuCommand);
        }

        public static void Initialize(Package package)
        {
            foreach (var item in _commandsListforPublish)
            {
                Action<IServiceProviderOwner, OleMenuCommand> actionBeforeQueryStatus = null;

                if (item.Item3)
                {
                    actionBeforeQueryStatus = ActionBeforeQueryStatusListForPublishWebResourceTextAnyInTextEditor;
                }
                else
                {
                    actionBeforeQueryStatus = ActionBeforeQueryStatusListForPublishWebResourceTextAny;
                }

                var command = new OpenFilesCommand(package, PackageGuids.guidCommandSet, item.Item1, CommonHandlers.GetSelectedFilesInListForPublish, item.Item2, actionBeforeQueryStatus, item.Item3);

                _instances.TryAdd(Tuple.Create<Func<DTEHelper, List<SelectedFile>>, OpenFilesType, bool>(CommonHandlers.GetSelectedFilesInListForPublish, item.Item2, item.Item3), command);
            }

            foreach (var item in _commandsFile)
            {
                Action<IServiceProviderOwner, OleMenuCommand> actionBeforeQueryStatus = null;

                if (item.Item3)
                {
                    actionBeforeQueryStatus = ActionBeforeQueryStatusSolutionExplorerWebResourceTextAnyInTextEditor;
                }
                else
                {
                    actionBeforeQueryStatus = ActionBeforeQueryStatusSolutionExplorerWebResourceTextAny;
                }

                var command = new OpenFilesCommand(package, PackageGuids.guidCommandSet, item.Item1, CommonHandlers.GetSelectedFiles, item.Item2, actionBeforeQueryStatus, item.Item3);

                _instances.TryAdd(Tuple.Create<Func<DTEHelper, List<SelectedFile>>, OpenFilesType, bool>(CommonHandlers.GetSelectedFiles, item.Item2, item.Item3), command);
            }

            foreach (var item in _commandsFolder)
            {
                Action<IServiceProviderOwner, OleMenuCommand> actionBeforeQueryStatus = null;

                if (item.Item3)
                {
                    actionBeforeQueryStatus = ActionBeforeQueryStatusSolutionExplorerWebResourceTextRecursiveInTextEditor;
                }
                else
                {
                    actionBeforeQueryStatus = ActionBeforeQueryStatusSolutionExplorerWebResourceTextRecursive;
                }

                var command = new OpenFilesCommand(package, PackageGuids.guidCommandSet, item.Item1, CommonHandlers.GetSelectedFilesRecursive, item.Item2, actionBeforeQueryStatus, item.Item3);

                _instances.TryAdd(Tuple.Create<Func<DTEHelper, List<SelectedFile>>, OpenFilesType, bool>(CommonHandlers.GetSelectedFilesRecursive, item.Item2, item.Item3), command);
            }
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
                    helper.HandleOpenFilesCommand(selectedFiles, _openFilesType, _inTextEditor);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(ex);
            }
        }
    }
}
