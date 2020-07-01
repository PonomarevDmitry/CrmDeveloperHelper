using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommand : AbstractDynamicCommandByConnectionAllWithoutCurrent
    {
        private readonly ISourceSelectedProjects _sourceSelectedProjects;

        private CSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedProjects sourceSelectedProjects)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedProjects = sourceSelectedProjects;
        }

        public static CSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommand InstanceCode { get; private set; }

        public static CSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommand InstanceDocuments { get; private set; }

        public static CSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommand InstanceFile { get; private set; }

        public static CSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommand InstanceFolder { get; private set; }

        public static CSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommand InstanceProject { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedProjects.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedProjects.CreateSource();

            var sourceFile = FileSourceSelectedProjects.CreateSource();

            var sourceFolder = FolderSourceSelectedProjects.CreateSource();

            var sourceProject = ProjectSourceSelectedProjects.CreateSource();

            InstanceCode = new CSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.CodeCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommandId, sourceCode);

            InstanceDocuments = new CSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommandId, sourceDocuments);

            InstanceFile = new CSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FileCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommandId, sourceFile);

            InstanceFolder = new CSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FolderCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommandId, sourceFolder);

            InstanceProject = new CSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.ProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommandId, sourceProject);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var selectedProjects = _sourceSelectedProjects.GetSelectedProjects(helper).ToList();

            helper.HandlePluginAssemblyBuildProjectUpdateCommand(connectionData, true, selectedProjects);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedProjects.CommandBeforeQueryStatus(applicationObject, menuCommand);
        }
    }
}