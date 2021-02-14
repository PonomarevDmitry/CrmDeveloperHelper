using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpProjectPluginTypeStepsAddToSolutionLastSelectedCommand : AbstractDynamicCommandOnSolutionLastSelected
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private CSharpProjectPluginTypeStepsAddToSolutionLastSelectedCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static CSharpProjectPluginTypeStepsAddToSolutionLastSelectedCommand InstanceCode { get; private set; }

        public static CSharpProjectPluginTypeStepsAddToSolutionLastSelectedCommand InstanceDocuments { get; private set; }

        public static CSharpProjectPluginTypeStepsAddToSolutionLastSelectedCommand InstanceFile { get; private set; }

        public static CSharpProjectPluginTypeStepsAddToSolutionLastSelectedCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new CSharpProjectPluginTypeStepsAddToSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.CodeCSharpProjectPluginTypeStepsAddToSolutionLastSelectedCommandId, sourceCode);

            InstanceDocuments = new CSharpProjectPluginTypeStepsAddToSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.DocumentsCSharpProjectPluginTypeStepsAddToSolutionLastSelectedCommandId, sourceDocuments);

            InstanceFile = new CSharpProjectPluginTypeStepsAddToSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.FileCSharpProjectPluginTypeStepsAddToSolutionLastSelectedCommandId, sourceFile);

            InstanceFolder = new CSharpProjectPluginTypeStepsAddToSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.FolderCSharpProjectPluginTypeStepsAddToSolutionLastSelectedCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper, string solutionUniqueName)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.CSharp).ToList();

            helper.HandlePluginTypeAddingProcessingStepsByProjectCommand(null, solutionUniqueName, false, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, string element, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.CSharp);
        }
    }
}