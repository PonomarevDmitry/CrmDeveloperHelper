using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal abstract class CodeLinkedGlobalOptionSetExplorerCommand : AbstractSingleCommand
    {
        private readonly SelectedFileType _selectedFileType;

        protected CodeLinkedGlobalOptionSetExplorerCommand(OleMenuCommandService commandService, int idCommand, SelectedFileType selectedFileType)
            : base(commandService, idCommand)
        {
            this._selectedFileType = selectedFileType;
        }

        protected override void CommandAction(DTEHelper helper)
        {
            if (helper.TryGetLinkedGlobalOptionSetName(_selectedFileType.GetCheckerFunction(), out string optionSetName))
            {
                helper.HandleExportGlobalOptionSets(optionSetName);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            if (_selectedFileType == SelectedFileType.WebResourceJavaScriptHasLinkedGlobalOptionSet)
            {
                CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScriptHasLinkedGlobalOptionSetName(applicationObject, menuCommand);
            }
            else if (_selectedFileType == SelectedFileType.CSharpHasLinkedGlobalOptionSet)
            {
                CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharpHasLinkedGlobalOptionSetName(applicationObject, menuCommand);
            }

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeLinkedGlobalOptionSetExplorerCommand);
        }
    }
}
