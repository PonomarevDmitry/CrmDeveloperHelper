using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpProjectPluginAssemblyStepsAddToSolutionLastCommand : AbstractDynamicCommandOnSolutionLast
    {
        private readonly ISourceSelectedProjects _sourceSelectedProjects;

        private CSharpProjectPluginAssemblyStepsAddToSolutionLastCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedProjects sourceSelectedProjects)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedProjects = sourceSelectedProjects;
        }

        public static CSharpProjectPluginAssemblyStepsAddToSolutionLastCommand InstanceCode { get; private set; }

        public static CSharpProjectPluginAssemblyStepsAddToSolutionLastCommand InstanceDocuments { get; private set; }

        public static CSharpProjectPluginAssemblyStepsAddToSolutionLastCommand InstanceFile { get; private set; }

        public static CSharpProjectPluginAssemblyStepsAddToSolutionLastCommand InstanceFolder { get; private set; }

        public static CSharpProjectPluginAssemblyStepsAddToSolutionLastCommand InstanceProject { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedProjects.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedProjects.CreateSource();

            var sourceFile = FileSourceSelectedProjects.CreateSource();

            var sourceFolder = FolderSourceSelectedProjects.CreateSource();

            var sourceProject = ProjectSourceSelectedProjects.CreateSource();

            InstanceCode = new CSharpProjectPluginAssemblyStepsAddToSolutionLastCommand(commandService, PackageIds.guidDynamicCommandSet.CodeCSharpProjectPluginAssemblyStepsAddToSolutionLastCommandId, sourceCode);

            InstanceDocuments = new CSharpProjectPluginAssemblyStepsAddToSolutionLastCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsCSharpProjectPluginAssemblyStepsAddToSolutionLastCommandId, sourceDocuments);

            InstanceFile = new CSharpProjectPluginAssemblyStepsAddToSolutionLastCommand(commandService, PackageIds.guidDynamicCommandSet.FileCSharpProjectPluginAssemblyStepsAddToSolutionLastCommandId, sourceFile);

            InstanceFolder = new CSharpProjectPluginAssemblyStepsAddToSolutionLastCommand(commandService, PackageIds.guidDynamicCommandSet.FolderCSharpProjectPluginAssemblyStepsAddToSolutionLastCommandId, sourceFolder);

            InstanceProject = new CSharpProjectPluginAssemblyStepsAddToSolutionLastCommand(commandService, PackageIds.guidDynamicCommandSet.ProjectPluginAssemblyStepsAddToSolutionLastCommandId, sourceProject);
        }

        protected override void CommandAction(DTEHelper helper, string solutionUniqueName)
        {
            var selectedProjects = _sourceSelectedProjects.GetSelectedProjects(helper).ToList();

            helper.HandlePluginAssemblyAddingProcessingStepsByProjectCommand(null, selectedProjects.Select(p => p.Name), solutionUniqueName, false);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, string element, OleMenuCommand menuCommand)
        {
            _sourceSelectedProjects.CommandBeforeQueryStatus(applicationObject, menuCommand);
        }
    }
}