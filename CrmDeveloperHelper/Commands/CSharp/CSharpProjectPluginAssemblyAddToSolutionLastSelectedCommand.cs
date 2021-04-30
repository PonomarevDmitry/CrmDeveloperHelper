using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpProjectPluginAssemblyAddToSolutionLastSelectedCommand : AbstractDynamicCommandOnSolutionLastSelected
    {
        private readonly ISourceSelectedProjects _sourceSelectedProjects;

        private CSharpProjectPluginAssemblyAddToSolutionLastSelectedCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedProjects sourceSelectedProjects)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedProjects = sourceSelectedProjects;
        }

        public static CSharpProjectPluginAssemblyAddToSolutionLastSelectedCommand InstanceCode { get; private set; }

        public static CSharpProjectPluginAssemblyAddToSolutionLastSelectedCommand InstanceDocuments { get; private set; }

        public static CSharpProjectPluginAssemblyAddToSolutionLastSelectedCommand InstanceFile { get; private set; }

        public static CSharpProjectPluginAssemblyAddToSolutionLastSelectedCommand InstanceFolder { get; private set; }

        public static CSharpProjectPluginAssemblyAddToSolutionLastSelectedCommand InstanceProject { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedProjects.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedProjects.CreateSource();

            var sourceFile = FileSourceSelectedProjects.CreateSource();

            var sourceFolder = FolderSourceSelectedProjects.CreateSource();

            var sourceProject = ProjectSourceSelectedProjects.CreateSource();

            InstanceCode = new CSharpProjectPluginAssemblyAddToSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.CodeCSharpProjectPluginAssemblyAddToSolutionLastSelectedCommandId, sourceCode);

            InstanceDocuments = new CSharpProjectPluginAssemblyAddToSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.DocumentsCSharpProjectPluginAssemblyAddToSolutionLastSelectedCommandId, sourceDocuments);

            InstanceFile = new CSharpProjectPluginAssemblyAddToSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.FileCSharpProjectPluginAssemblyAddToSolutionLastSelectedCommandId, sourceFile);

            InstanceFolder = new CSharpProjectPluginAssemblyAddToSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.FolderCSharpProjectPluginAssemblyAddToSolutionLastSelectedCommandId, sourceFolder);

            InstanceProject = new CSharpProjectPluginAssemblyAddToSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.ProjectPluginAssemblyAddToSolutionLastSelectedCommandId, sourceProject);
        }

        protected override void CommandAction(DTEHelper helper, string solutionUniqueName)
        {
            var selectedProjects = _sourceSelectedProjects.GetSelectedProjects(helper).ToList();

            var selectedProjectsNames = new List<string>();

            foreach (var project in selectedProjects)
            {
                selectedProjectsNames.Add(PropertiesHelper.GetAssemblyName(project));
            }

            helper.HandlePluginAssemblyAddingToSolutionByProjectCommand(null, selectedProjectsNames, solutionUniqueName, false);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, string element, OleMenuCommand menuCommand)
        {
            _sourceSelectedProjects.CommandBeforeQueryStatus(applicationObject, menuCommand);
        }
    }
}