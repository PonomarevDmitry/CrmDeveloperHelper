using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpPluginAssemblyExplorerCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedProjects _sourceSelectedProjects;

        private CSharpPluginAssemblyExplorerCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedProjects sourceSelectedProjects)
            : base(commandService, idCommand)
        {
            this._sourceSelectedProjects = sourceSelectedProjects;
        }

        public static CSharpPluginAssemblyExplorerCommand InstanceCode { get; private set; }

        public static CSharpPluginAssemblyExplorerCommand InstanceFile { get; private set; }

        public static CSharpPluginAssemblyExplorerCommand InstanceProject { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedProjects.CreateSource();

            var sourceFile = FileSourceSelectedProjectSingle.CreateSource();

            var sourceProject = ProjectSourceSelectedProjectSingle.CreateSource();

            InstanceCode = new CSharpPluginAssemblyExplorerCommand(commandService, PackageIds.guidCommandSet.CodeCSharpPluginAssemblyExplorerCommandId, sourceCode);

            InstanceFile = new CSharpPluginAssemblyExplorerCommand(commandService, PackageIds.guidCommandSet.FileCSharpPluginAssemblyExplorerCommandId, sourceFile);

            InstanceProject = new CSharpPluginAssemblyExplorerCommand(commandService, PackageIds.guidCommandSet.ProjectPluginAssemblyExplorerCommandId, sourceProject);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedProjects = _sourceSelectedProjects.GetSelectedProjects(helper).ToList();

            helper.HandleOpenPluginAssemblyExplorer(selectedProjects.FirstOrDefault()?.Name);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedProjects.CommandBeforeQueryStatus(applicationObject, menuCommand);
        }
    }
}