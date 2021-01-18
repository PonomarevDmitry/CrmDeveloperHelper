using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal abstract class CodeLinkedGlobalOptionSetAddToSolutionLastCommand : AbstractDynamicCommandOnSolutionLast
    {
        private readonly SelectedFileType _selectedFileType;

        protected CodeLinkedGlobalOptionSetAddToSolutionLastCommand(OleMenuCommandService commandService, int baseIdStart, SelectedFileType selectedFileType)
            : base(commandService, baseIdStart)
        {
            this._selectedFileType = selectedFileType;
        }

        protected override void CommandAction(DTEHelper helper, string solutionUniqueName)
        {
            if (helper.TryGetLinkedGlobalOptionSetName(_selectedFileType.GetCheckerFunction(), out string optionSetName))
            {
                helper.HandleAddingGlobalOptionSetToSolutionCommand(null, solutionUniqueName, false, new[] { optionSetName });
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, string solutionUniqueName, OleMenuCommand menuCommand)
        {
            if (_selectedFileType == SelectedFileType.WebResourceJavaScriptHasLinkedGlobalOptionSet)
            {
                CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScriptHasLinkedGlobalOptionSetName(applicationObject, menuCommand);
            }
            else if (_selectedFileType == SelectedFileType.CSharpHasLinkedGlobalOptionSet)
            {
                CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharpHasLinkedGlobalOptionSetName(applicationObject, menuCommand);
            }
        }
    }
}
