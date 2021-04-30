using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpProjectPluginAssemblyStepsAddToSolutionLastSelectedCommand : AbstractDynamicCommandOnSolutionLastSelected
    {
        private readonly ISourceSelectedProjects _sourceSelectedProjects;

        private CSharpProjectPluginAssemblyStepsAddToSolutionLastSelectedCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedProjects sourceSelectedProjects)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedProjects = sourceSelectedProjects;
        }

        public static CSharpProjectPluginAssemblyStepsAddToSolutionLastSelectedCommand InstanceCode { get; private set; }

        public static CSharpProjectPluginAssemblyStepsAddToSolutionLastSelectedCommand InstanceDocuments { get; private set; }

        public static CSharpProjectPluginAssemblyStepsAddToSolutionLastSelectedCommand InstanceFile { get; private set; }

        public static CSharpProjectPluginAssemblyStepsAddToSolutionLastSelectedCommand InstanceFolder { get; private set; }

        public static CSharpProjectPluginAssemblyStepsAddToSolutionLastSelectedCommand InstanceProject { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedProjects.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedProjects.CreateSource();

            var sourceFile = FileSourceSelectedProjects.CreateSource();

            var sourceFolder = FolderSourceSelectedProjects.CreateSource();

            var sourceProject = ProjectSourceSelectedProjects.CreateSource();

            InstanceCode = new CSharpProjectPluginAssemblyStepsAddToSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.CodeCSharpProjectPluginAssemblyStepsAddToSolutionLastSelectedCommandId, sourceCode);

            InstanceDocuments = new CSharpProjectPluginAssemblyStepsAddToSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.DocumentsCSharpProjectPluginAssemblyStepsAddToSolutionLastSelectedCommandId, sourceDocuments);

            InstanceFile = new CSharpProjectPluginAssemblyStepsAddToSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.FileCSharpProjectPluginAssemblyStepsAddToSolutionLastSelectedCommandId, sourceFile);

            InstanceFolder = new CSharpProjectPluginAssemblyStepsAddToSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.FolderCSharpProjectPluginAssemblyStepsAddToSolutionLastSelectedCommandId, sourceFolder);

            InstanceProject = new CSharpProjectPluginAssemblyStepsAddToSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.ProjectPluginAssemblyStepsAddToSolutionLastSelectedCommandId, sourceProject);
        }

        protected override void CommandAction(DTEHelper helper, string solutionUniqueName)
        {
            var selectedProjects = _sourceSelectedProjects.GetSelectedProjects(helper).ToList();

            var selectedProjectsNames = new List<string>();

            foreach (var project in selectedProjects)
            {
                selectedProjectsNames.Add(PropertiesHelper.GetAssemblyName(project));
            }

            helper.HandlePluginAssemblyAddingProcessingStepsByProjectCommand(null, selectedProjectsNames, solutionUniqueName, false);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, string element, OleMenuCommand menuCommand)
        {
            _sourceSelectedProjects.CommandBeforeQueryStatus(applicationObject, menuCommand);
        }
    }
}