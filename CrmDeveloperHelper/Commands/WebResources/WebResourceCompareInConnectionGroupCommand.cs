using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.SourcesSelectedFiles;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class WebResourceCompareInConnectionGroupCommand : AbstractDynamicCommandByConnectionByGroupWithoutCurrent
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;
        private readonly bool _withDetails;

        private WebResourceCompareInConnectionGroupCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles, bool withDetails)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
            this._withDetails = withDetails;
        }

        public static WebResourceCompareInConnectionGroupCommand InstanceDocuments { get; private set; }

        public static WebResourceCompareInConnectionGroupCommand InstanceFile { get; private set; }

        public static WebResourceCompareInConnectionGroupCommand InstanceFolder { get; private set; }

        public static WebResourceCompareInConnectionGroupCommand InstanceWithDetailsCode { get; private set; }

        public static WebResourceCompareInConnectionGroupCommand InstanceWithDetailsDocuments { get; private set; }

        public static WebResourceCompareInConnectionGroupCommand InstanceWithDetailsFile { get; private set; }

        public static WebResourceCompareInConnectionGroupCommand InstanceWithDetailsFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceDocuments = new WebResourceCompareInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsWebResourceCompareInConnectionGroupCommandId, sourceDocuments, false);

            InstanceFile = new WebResourceCompareInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.FileWebResourceCompareInConnectionGroupCommandId, sourceFile, false);

            InstanceFolder = new WebResourceCompareInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.FolderWebResourceCompareInConnectionGroupCommandId, sourceFolder, false);

            InstanceWithDetailsCode = new WebResourceCompareInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.CodeWebResourceCompareWithDetailsInConnectionGroupCommandId, sourceCode, true);

            InstanceWithDetailsDocuments = new WebResourceCompareInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsWebResourceCompareWithDetailsInConnectionGroupCommandId, sourceDocuments, true);

            InstanceWithDetailsFile = new WebResourceCompareInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.FileWebResourceCompareWithDetailsInConnectionGroupCommandId, sourceFile, true);

            InstanceWithDetailsFolder = new WebResourceCompareInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.FolderWebResourceCompareWithDetailsInConnectionGroupCommandId, sourceFolder, true);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = null;

            if (this._withDetails)
            {
                selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResourceText).ToList();
            }
            else
            {
                selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResource).ToList();
            }

            if (selectedFiles != null)
            {
                helper.HandleWebResourceCompareCommand(connectionData, selectedFiles, this._withDetails);
            }
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            if (this._withDetails)
            {
                _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResourceText);
            }
            else
            {
                _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResource);
            }
        }
    }
}
