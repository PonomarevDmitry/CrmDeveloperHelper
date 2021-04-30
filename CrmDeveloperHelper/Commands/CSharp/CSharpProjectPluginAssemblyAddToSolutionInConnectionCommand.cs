using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpProjectPluginAssemblyAddToSolutionInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ISourceSelectedProjects _sourceSelectedProjects;

        private CSharpProjectPluginAssemblyAddToSolutionInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedProjects sourceSelectedProjects)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedProjects = sourceSelectedProjects;
        }

        public static CSharpProjectPluginAssemblyAddToSolutionInConnectionCommand InstanceCode { get; private set; }

        public static CSharpProjectPluginAssemblyAddToSolutionInConnectionCommand InstanceDocuments { get; private set; }

        public static CSharpProjectPluginAssemblyAddToSolutionInConnectionCommand InstanceFile { get; private set; }

        public static CSharpProjectPluginAssemblyAddToSolutionInConnectionCommand InstanceFolder { get; private set; }

        public static CSharpProjectPluginAssemblyAddToSolutionInConnectionCommand InstanceProject { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedProjects.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedProjects.CreateSource();

            var sourceFile = FileSourceSelectedProjects.CreateSource();

            var sourceFolder = FolderSourceSelectedProjects.CreateSource();

            var sourceProject = ProjectSourceSelectedProjects.CreateSource();

            InstanceCode = new CSharpProjectPluginAssemblyAddToSolutionInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.CodeCSharpProjectPluginAssemblyAddToSolutionInConnectionCommandId, sourceCode);

            InstanceDocuments = new CSharpProjectPluginAssemblyAddToSolutionInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsCSharpProjectPluginAssemblyAddToSolutionInConnectionCommandId, sourceDocuments);

            InstanceFile = new CSharpProjectPluginAssemblyAddToSolutionInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FileCSharpProjectPluginAssemblyAddToSolutionInConnectionCommandId, sourceFile);

            InstanceFolder = new CSharpProjectPluginAssemblyAddToSolutionInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FolderCSharpProjectPluginAssemblyAddToSolutionInConnectionCommandId, sourceFolder);

            InstanceProject = new CSharpProjectPluginAssemblyAddToSolutionInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.ProjectPluginAssemblyAddToSolutionInConnectionCommandId, sourceProject);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var selectedProjects = _sourceSelectedProjects.GetSelectedProjects(helper).ToList();

            var selectedProjectsNames = new List<string>();

            foreach (var project in selectedProjects)
            {
                selectedProjectsNames.Add(PropertiesHelper.GetAssemblyName(project));
            }

            helper.HandlePluginAssemblyAddingToSolutionByProjectCommand(connectionData, selectedProjectsNames, null, true);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedProjects.CommandBeforeQueryStatus(applicationObject, menuCommand);
        }
    }
}