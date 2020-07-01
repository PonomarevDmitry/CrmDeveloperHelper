using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpProjectUpdatePluginAssemblyInConnectionCommand : AbstractDynamicCommandByConnectionAllWithoutCurrent
    {
        private readonly ISourceSelectedProjects _sourceSelectedProjects;

        private CSharpProjectUpdatePluginAssemblyInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedProjects sourceSelectedProjects)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedProjects = sourceSelectedProjects;
        }

        public static CSharpProjectUpdatePluginAssemblyInConnectionCommand InstanceCode { get; private set; }

        public static CSharpProjectUpdatePluginAssemblyInConnectionCommand InstanceDocuments { get; private set; }

        public static CSharpProjectUpdatePluginAssemblyInConnectionCommand InstanceFile { get; private set; }

        public static CSharpProjectUpdatePluginAssemblyInConnectionCommand InstanceFolder { get; private set; }

        public static CSharpProjectUpdatePluginAssemblyInConnectionCommand InstanceProject { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedProjects.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedProjects.CreateSource();

            var sourceFile = FileSourceSelectedProjects.CreateSource();

            var sourceFolder = FolderSourceSelectedProjects.CreateSource();

            var sourceProject = ProjectSourceSelectedProjects.CreateSource();

            InstanceCode = new CSharpProjectUpdatePluginAssemblyInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.CodeCSharpProjectUpdatePluginAssemblyInConnectionCommandId, sourceCode);

            InstanceDocuments = new CSharpProjectUpdatePluginAssemblyInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsCSharpProjectUpdatePluginAssemblyInConnectionCommandId, sourceDocuments);

            InstanceFile = new CSharpProjectUpdatePluginAssemblyInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FileCSharpProjectUpdatePluginAssemblyInConnectionCommandId, sourceFile);

            InstanceFolder = new CSharpProjectUpdatePluginAssemblyInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FolderCSharpProjectUpdatePluginAssemblyInConnectionCommandId, sourceFolder);

            InstanceProject = new CSharpProjectUpdatePluginAssemblyInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.ProjectUpdatePluginAssemblyInConnectionCommandId, sourceProject);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var selectedProjects = _sourceSelectedProjects.GetSelectedProjects(helper).ToList();

            helper.HandlePluginAssemblyUpdatingInWindowCommand(connectionData, selectedProjects);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedProjects.CommandBeforeQueryStatus(applicationObject, menuCommand);
        }
    }
}