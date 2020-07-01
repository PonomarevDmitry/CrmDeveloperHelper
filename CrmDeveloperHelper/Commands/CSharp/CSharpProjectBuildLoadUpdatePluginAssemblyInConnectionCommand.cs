using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpProjectBuildLoadUpdatePluginAssemblyInConnectionCommand : AbstractDynamicCommandByConnectionAllWithoutCurrent
    {
        private readonly ISourceSelectedProjects _sourceSelectedProjects;

        private CSharpProjectBuildLoadUpdatePluginAssemblyInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedProjects sourceSelectedProjects)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedProjects = sourceSelectedProjects;
        }

        public static CSharpProjectBuildLoadUpdatePluginAssemblyInConnectionCommand InstanceCode { get; private set; }

        public static CSharpProjectBuildLoadUpdatePluginAssemblyInConnectionCommand InstanceDocuments { get; private set; }

        public static CSharpProjectBuildLoadUpdatePluginAssemblyInConnectionCommand InstanceFile { get; private set; }

        public static CSharpProjectBuildLoadUpdatePluginAssemblyInConnectionCommand InstanceFolder { get; private set; }

        public static CSharpProjectBuildLoadUpdatePluginAssemblyInConnectionCommand InstanceProject { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedProjects.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedProjects.CreateSource();

            var sourceFile = FileSourceSelectedProjects.CreateSource();

            var sourceFolder = FolderSourceSelectedProjects.CreateSource();

            var sourceProject = ProjectSourceSelectedProjects.CreateSource();

            InstanceCode = new CSharpProjectBuildLoadUpdatePluginAssemblyInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.CodeCSharpProjectBuildLoadUpdatePluginAssemblyInConnectionCommandId, sourceCode);

            InstanceDocuments = new CSharpProjectBuildLoadUpdatePluginAssemblyInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsCSharpProjectBuildLoadUpdatePluginAssemblyInConnectionCommandId, sourceDocuments);

            InstanceFile = new CSharpProjectBuildLoadUpdatePluginAssemblyInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FileCSharpProjectBuildLoadUpdatePluginAssemblyInConnectionCommandId, sourceFile);

            InstanceFolder = new CSharpProjectBuildLoadUpdatePluginAssemblyInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FolderCSharpProjectBuildLoadUpdatePluginAssemblyInConnectionCommandId, sourceFolder);

            InstanceProject = new CSharpProjectBuildLoadUpdatePluginAssemblyInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.ProjectBuildLoadUpdatePluginAssemblyInConnectionCommandId, sourceProject);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var selectedProjects = _sourceSelectedProjects.GetSelectedProjects(helper).ToList();

            helper.HandlePluginAssemblyBuildProjectUpdateCommand(connectionData, false, selectedProjects);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedProjects.CommandBeforeQueryStatus(applicationObject, menuCommand);
        }
    }
}