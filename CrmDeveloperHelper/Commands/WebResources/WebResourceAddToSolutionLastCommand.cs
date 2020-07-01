using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class WebResourceAddToSolutionLastCommand : AbstractDynamicCommandOnSolutionLast
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private WebResourceAddToSolutionLastCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static WebResourceAddToSolutionLastCommand InstanceCode { get; private set; }

        public static WebResourceAddToSolutionLastCommand InstanceDocuments { get; private set; }

        public static WebResourceAddToSolutionLastCommand InstanceFile { get; private set; }

        public static WebResourceAddToSolutionLastCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new WebResourceAddToSolutionLastCommand(commandService, PackageIds.guidDynamicCommandSet.CodeWebResourceAddToSolutionLastCommandId, sourceCode);

            InstanceDocuments = new WebResourceAddToSolutionLastCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsWebResourceAddToSolutionLastCommandId, sourceDocuments);

            InstanceFile = new WebResourceAddToSolutionLastCommand(commandService, PackageIds.guidDynamicCommandSet.FileWebResourceAddToSolutionLastCommandId, sourceFile);

            InstanceFolder = new WebResourceAddToSolutionLastCommand(commandService, PackageIds.guidDynamicCommandSet.FolderWebResourceAddToSolutionLastCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper, string solutionUniqueName)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResource).ToList();

            helper.HandleWebResourceAddingToSolutionCommand(null, solutionUniqueName, false, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, string element, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResource);
        }
    }
}