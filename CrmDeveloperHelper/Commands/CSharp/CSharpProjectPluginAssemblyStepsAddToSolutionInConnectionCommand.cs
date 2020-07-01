using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ISourceSelectedProjects _sourceSelectedProjects;

        private CSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedProjects sourceSelectedProjects)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedProjects = sourceSelectedProjects;
        }

        public static CSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand InstanceCode { get; private set; }

        public static CSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand InstanceDocuments { get; private set; }

        public static CSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand InstanceFile { get; private set; }

        public static CSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand InstanceFolder { get; private set; }

        public static CSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand InstanceProject { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedProjects.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedProjects.CreateSource();

            var sourceFile = FileSourceSelectedProjects.CreateSource();

            var sourceFolder = FolderSourceSelectedProjects.CreateSource();

            var sourceProject = ProjectSourceSelectedProjects.CreateSource();

            InstanceCode = new CSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.CodeCSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommandId, sourceCode);

            InstanceDocuments = new CSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsCSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommandId, sourceDocuments);

            InstanceFile = new CSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FileCSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommandId, sourceFile);

            InstanceFolder = new CSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FolderCSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommandId, sourceFolder);

            InstanceProject = new CSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.ProjectPluginAssemblyStepsAddToSolutionInConnectionCommandId, sourceProject);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var selectedProjects = _sourceSelectedProjects.GetSelectedProjects(helper).ToList();

            helper.HandlePluginAssemblyAddingProcessingStepsByProjectCommand(connectionData, selectedProjects.Select(p => p.Name), null, true);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedProjects.CommandBeforeQueryStatus(applicationObject, menuCommand);
        }
    }
}