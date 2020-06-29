using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.SourcesSelectedFiles;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class WebResourceCheckEncodingCompareFilesCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;
        private readonly bool _withDetails;

        private WebResourceCheckEncodingCompareFilesCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles, bool withDetails)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
            this._withDetails = withDetails;
        }

        public static WebResourceCheckEncodingCompareFilesCommand InstanceDocuments { get; private set; }

        public static WebResourceCheckEncodingCompareFilesCommand InstanceFile { get; private set; }

        public static WebResourceCheckEncodingCompareFilesCommand InstanceFolder { get; private set; }

        public static WebResourceCheckEncodingCompareFilesCommand InstanceWithDetailsDocuments { get; private set; }

        public static WebResourceCheckEncodingCompareFilesCommand InstanceWithDetailsFile { get; private set; }

        public static WebResourceCheckEncodingCompareFilesCommand InstanceWithDetailsFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceDocuments = new WebResourceCheckEncodingCompareFilesCommand(commandService, PackageIds.guidCommandSet.DocumentsWebResourceCheckEncodingCompareFilesCommandId, sourceDocuments, false);

            InstanceFile = new WebResourceCheckEncodingCompareFilesCommand(commandService, PackageIds.guidCommandSet.FileWebResourceCheckEncodingCompareFilesCommandId, sourceFile, false);

            InstanceFolder = new WebResourceCheckEncodingCompareFilesCommand(commandService, PackageIds.guidCommandSet.FolderWebResourceCheckEncodingCompareFilesCommandId, sourceFolder, false);

            InstanceWithDetailsDocuments = new WebResourceCheckEncodingCompareFilesCommand(commandService, PackageIds.guidCommandSet.DocumentsWebResourceCheckEncodingCompareWithDetailsFilesCommandId, sourceDocuments, true);

            InstanceWithDetailsFile = new WebResourceCheckEncodingCompareFilesCommand(commandService, PackageIds.guidCommandSet.FileWebResourceCheckEncodingCompareWithDetailsFilesCommandId, sourceFile, true);

            InstanceWithDetailsFolder = new WebResourceCheckEncodingCompareFilesCommand(commandService, PackageIds.guidCommandSet.FolderWebResourceCheckEncodingCompareWithDetailsFilesCommandId, sourceFolder, true);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResourceText).ToList();

            helper.HandleWebResourceCompareFilesWithoutUTF8EncodingCommand(selectedFiles, this._withDetails);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResourceText);
        }
    }
}