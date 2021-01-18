using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal abstract class CodeLinkedGlobalOptionSetPublishInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly SelectedFileType _selectedFileType;

        protected CodeLinkedGlobalOptionSetPublishInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, SelectedFileType selectedFileType)
            : base(commandService, baseIdStart)
        {
            this._selectedFileType = selectedFileType;
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            if (helper.TryGetLinkedGlobalOptionSetName(_selectedFileType.GetCheckerFunction(), out string optionSetName))
            {
                helper.HandlePublishGlobalOptionSetCommand(connectionData, optionSetName);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
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
