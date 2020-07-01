using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpProjectPluginTypeStepsAddToSolutionLastCommand : AbstractDynamicCommandOnSolutionLast
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private CSharpProjectPluginTypeStepsAddToSolutionLastCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static CSharpProjectPluginTypeStepsAddToSolutionLastCommand InstanceCode { get; private set; }

        public static CSharpProjectPluginTypeStepsAddToSolutionLastCommand InstanceDocuments { get; private set; }

        public static CSharpProjectPluginTypeStepsAddToSolutionLastCommand InstanceFile { get; private set; }

        public static CSharpProjectPluginTypeStepsAddToSolutionLastCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new CSharpProjectPluginTypeStepsAddToSolutionLastCommand(commandService, PackageIds.guidDynamicCommandSet.CodeCSharpProjectPluginTypeStepsAddToSolutionLastCommandId, sourceCode);

            InstanceDocuments = new CSharpProjectPluginTypeStepsAddToSolutionLastCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsCSharpProjectPluginTypeStepsAddToSolutionLastCommandId, sourceDocuments);

            InstanceFile = new CSharpProjectPluginTypeStepsAddToSolutionLastCommand(commandService, PackageIds.guidDynamicCommandSet.FileCSharpProjectPluginTypeStepsAddToSolutionLastCommandId, sourceFile);

            InstanceFolder = new CSharpProjectPluginTypeStepsAddToSolutionLastCommand(commandService, PackageIds.guidDynamicCommandSet.FolderCSharpProjectPluginTypeStepsAddToSolutionLastCommandId, sourceFolder);
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