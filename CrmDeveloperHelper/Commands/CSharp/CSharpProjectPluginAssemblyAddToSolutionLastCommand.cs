using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpProjectPluginAssemblyAddToSolutionLastCommand : AbstractDynamicCommandOnSolutionLast
    {
        private readonly ISourceSelectedProjects _sourceSelectedProjects;

        private CSharpProjectPluginAssemblyAddToSolutionLastCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedProjects sourceSelectedProjects)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedProjects = sourceSelectedProjects;
        }

        public static CSharpProjectPluginAssemblyAddToSolutionLastCommand InstanceCode { get; private set; }

        public static CSharpProjectPluginAssemblyAddToSolutionLastCommand InstanceDocuments { get; private set; }

        public static CSharpProjectPluginAssemblyAddToSolutionLastCommand InstanceFile { get; private set; }

        public static CSharpProjectPluginAssemblyAddToSolutionLastCommand InstanceFolder { get; private set; }

        public static CSharpProjectPluginAssemblyAddToSolutionLastCommand InstanceProject { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedProjects.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedProjects.CreateSource();

            var sourceFile = FileSourceSelectedProjects.CreateSource();

            var sourceFolder = FolderSourceSelectedProjects.CreateSource();

            var sourceProject = ProjectSourceSelectedProjects.CreateSource();

            InstanceCode = new CSharpProjectPluginAssemblyAddToSolutionLastCommand(commandService, PackageIds.guidDynamicCommandSet.CodeCSharpProjectPluginAssemblyAddToSolutionLastCommandId, sourceCode);

            InstanceDocuments = new CSharpProjectPluginAssemblyAddToSolutionLastCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsCSharpProjectPluginAssemblyAddToSolutionLastCommandId, sourceDocuments);

            InstanceFile = new CSharpProjectPluginAssemblyAddToSolutionLastCommand(commandService, PackageIds.guidDynamicCommandSet.FileCSharpProjectPluginAssemblyAddToSolutionLastCommandId, sourceFile);

            InstanceFolder = new CSharpProjectPluginAssemblyAddToSolutionLastCommand(commandService, PackageIds.guidDynamicCommandSet.FolderCSharpProjectPluginAssemblyAddToSolutionLastCommandId, sourceFolder);

            InstanceProject = new CSharpProjectPluginAssemblyAddToSolutionLastCommand(commandService, PackageIds.guidDynamicCommandSet.ProjectPluginAssemblyAddToSolutionLastCommandId, sourceProject);
        }

        protected override void CommandAction(DTEHelper helper, string solutionUniqueName)
        {
            var selectedProjects = _sourceSelectedProjects.GetSelectedProjects(helper).ToList();

            helper.HandlePluginAssemblyAddingToSolutionByProjectCommand(null, selectedProjects.Select(p => p.Name), solutionUniqueName, false);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, string element, OleMenuCommand menuCommand)
        {
            _sourceSelectedProjects.CommandBeforeQueryStatus(applicationObject, menuCommand);
        }
    }
}