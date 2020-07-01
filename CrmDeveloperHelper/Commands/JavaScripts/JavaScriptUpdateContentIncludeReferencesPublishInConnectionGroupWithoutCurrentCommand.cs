using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts
{
    internal sealed class JavaScriptUpdateContentIncludeReferencesPublishInConnectionGroupWithoutCurrentCommand : AbstractDynamicCommandByConnectionByGroupWithoutCurrent
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private JavaScriptUpdateContentIncludeReferencesPublishInConnectionGroupWithoutCurrentCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static JavaScriptUpdateContentIncludeReferencesPublishInConnectionGroupWithoutCurrentCommand InstanceCode { get; private set; }

        public static JavaScriptUpdateContentIncludeReferencesPublishInConnectionGroupWithoutCurrentCommand InstanceFile { get; private set; }

        public static JavaScriptUpdateContentIncludeReferencesPublishInConnectionGroupWithoutCurrentCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new JavaScriptUpdateContentIncludeReferencesPublishInConnectionGroupWithoutCurrentCommand(commandService, PackageIds.guidDynamicCommandSet.CodeJavaScriptUpdateContentIncludeReferencesPublishInConnectionGroupCommandId, sourceCode);

            InstanceFile = new JavaScriptUpdateContentIncludeReferencesPublishInConnectionGroupWithoutCurrentCommand(commandService, PackageIds.guidDynamicCommandSet.FileJavaScriptUpdateContentIncludeReferencesPublishInConnectionGroupCommandId, sourceFile);

            InstanceFolder = new JavaScriptUpdateContentIncludeReferencesPublishInConnectionGroupWithoutCurrentCommand(commandService, PackageIds.guidDynamicCommandSet.FolderJavaScriptUpdateContentIncludeReferencesPublishInConnectionGroupCommandId, sourceFolder);
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