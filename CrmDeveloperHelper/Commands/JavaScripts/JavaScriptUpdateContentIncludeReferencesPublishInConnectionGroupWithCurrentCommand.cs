using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts
{
    internal sealed class JavaScriptUpdateContentIncludeReferencesPublishInConnectionGroupWithCurrentCommand : AbstractDynamicCommandByConnectionByGroupWithCurrent
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private JavaScriptUpdateContentIncludeReferencesPublishInConnectionGroupWithCurrentCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static JavaScriptUpdateContentIncludeReferencesPublishInConnectionGroupWithCurrentCommand InstanceDocuments { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            InstanceDocuments = new JavaScriptUpdateContentIncludeReferencesPublishInConnectionGroupWithCurrentCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsJavaScriptUpdateContentIncludeReferencesPublishInConnectionGroupCommandId, sourceDocuments);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResourceJavaScript).ToList();

            helper.HandleUpdateContentIncludeReferencesToDependencyXmlCommand(connectionData, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResourceJavaScript);
        }
    }
}