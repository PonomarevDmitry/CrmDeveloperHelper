using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Commons
{
    internal sealed class OpenWebResourcesInSolutionLastSelectedCommand : AbstractDynamicCommandOnSolutionLastSelected
    {
        private readonly bool _inTextEditor;
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private OpenWebResourcesInSolutionLastSelectedCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles, bool inTextEditor)
            : base(commandService, baseIdStart)
        {
            this._inTextEditor = inTextEditor;
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static OpenWebResourcesInSolutionLastSelectedCommand InstanceCode { get; private set; }

        public static OpenWebResourcesInSolutionLastSelectedCommand InstanceDocuments { get; private set; }

        public static OpenWebResourcesInSolutionLastSelectedCommand InstanceFile { get; private set; }

        public static OpenWebResourcesInSolutionLastSelectedCommand InstanceFolder { get; private set; }

        public static OpenWebResourcesInSolutionLastSelectedCommand InstanceCommon { get; private set; }



        public static OpenWebResourcesInSolutionLastSelectedCommand InstanceCodeInTextEditor { get; private set; }

        public static OpenWebResourcesInSolutionLastSelectedCommand InstanceDocumentsInTextEditor { get; private set; }

        public static OpenWebResourcesInSolutionLastSelectedCommand InstanceFileInTextEditor { get; private set; }

        public static OpenWebResourcesInSolutionLastSelectedCommand InstanceFolderInTextEditor { get; private set; }

        public static OpenWebResourcesInSolutionLastSelectedCommand InstanceCommonInTextEditor { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceCommon = new OpenWebResourcesInSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.CommonOpenWebResourcesInSolutionLastSelectedCommandId, null, false);

            InstanceCommonInTextEditor = new OpenWebResourcesInSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.CommonOpenWebResourcesInSolutionLastSelectedInTextEditorCommandId, null, true);


            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new OpenWebResourcesInSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.CodeWebResourceOpenWebResourcesInSolutionLastSelectedCommandId, sourceCode, false);

            InstanceDocuments = new OpenWebResourcesInSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.DocumentsWebResourceOpenWebResourcesInSolutionLastSelectedCommandId, sourceDocuments, false);

            InstanceFile = new OpenWebResourcesInSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.FileWebResourceOpenWebResourcesInSolutionLastSelectedCommandId, sourceFile, false);

            InstanceFolder = new OpenWebResourcesInSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.FolderWebResourceOpenWebResourcesInSolutionLastSelectedCommandId, sourceFolder, false);



            InstanceCodeInTextEditor = new OpenWebResourcesInSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.CodeWebResourceOpenWebResourcesInSolutionLastSelectedInTextEditorCommandId, sourceCode, true);

            InstanceDocumentsInTextEditor = new OpenWebResourcesInSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.DocumentsWebResourceOpenWebResourcesInSolutionLastSelectedInTextEditorCommandId, sourceDocuments, true);

            InstanceFileInTextEditor = new OpenWebResourcesInSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.FileWebResourceOpenWebResourcesInSolutionLastSelectedInTextEditorCommandId, sourceFile, true);

            InstanceFolderInTextEditor = new OpenWebResourcesInSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.FolderWebResourceOpenWebResourcesInSolutionLastSelectedInTextEditorCommandId, sourceFolder, true);
        }

        protected override void CommandAction(DTEHelper helper, string solutionUniqueName)
        {
            var connectionConfig = ConnectionConfiguration.Get();

            if (connectionConfig.CurrentConnectionData != null)
            {
                helper.HandleSolutionOpenWebResourcesInLastSelected(connectionConfig.CurrentConnectionData, solutionUniqueName, this._inTextEditor);
            }
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, string solutionUniqueName, OleMenuCommand menuCommand)
        {
            if (_inTextEditor)
            {
                CommonHandlers.ActionBeforeQueryStatusTextEditorProgramExists(applicationObject, menuCommand);
            }

            if (_sourceSelectedFiles != null)
            {
                _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResource);
            }
        }
    }
}
