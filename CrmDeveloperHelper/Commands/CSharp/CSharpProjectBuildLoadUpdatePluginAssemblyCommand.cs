using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpProjectBuildLoadUpdatePluginAssemblyCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedProjects _sourceSelectedProjects;
        private readonly string _commandNameForCorrection;

        private CSharpProjectBuildLoadUpdatePluginAssemblyCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedProjects sourceSelectedProjects, string commandNameForCorrection)
            : base(commandService, idCommand)
        {
            this._sourceSelectedProjects = sourceSelectedProjects;
            this._commandNameForCorrection = commandNameForCorrection;
        }

        public static CSharpProjectBuildLoadUpdatePluginAssemblyCommand InstanceCode { get; private set; }

        public static CSharpProjectBuildLoadUpdatePluginAssemblyCommand InstanceDocuments { get; private set; }

        public static CSharpProjectBuildLoadUpdatePluginAssemblyCommand InstanceFile { get; private set; }

        public static CSharpProjectBuildLoadUpdatePluginAssemblyCommand InstanceFolder { get; private set; }

        public static CSharpProjectBuildLoadUpdatePluginAssemblyCommand InstanceProject { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedProjects.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedProjects.CreateSource();

            var sourceFile = FileSourceSelectedProjects.CreateSource();

            var sourceFolder = FolderSourceSelectedProjects.CreateSource();

            var sourceProject = ProjectSourceSelectedProjects.CreateSource();

            InstanceCode = new CSharpProjectBuildLoadUpdatePluginAssemblyCommand(commandService, PackageIds.guidCommandSet.CodeCSharpProjectBuildLoadUpdatePluginAssemblyCommandId, sourceCode, Properties.CommandNames.CodeCSharpProjectBuildLoadUpdatePluginAssemblyCommand);

            InstanceDocuments = new CSharpProjectBuildLoadUpdatePluginAssemblyCommand(commandService, PackageIds.guidCommandSet.DocumentsCSharpProjectBuildLoadUpdatePluginAssemblyCommandId, sourceDocuments, Properties.CommandNames.DocumentsCSharpProjectBuildLoadUpdatePluginAssemblyCommand);

            InstanceFile = new CSharpProjectBuildLoadUpdatePluginAssemblyCommand(commandService, PackageIds.guidCommandSet.FileCSharpProjectBuildLoadUpdatePluginAssemblyCommandId, sourceFile, Properties.CommandNames.FileCSharpProjectBuildLoadUpdatePluginAssemblyCommand);

            InstanceFolder = new CSharpProjectBuildLoadUpdatePluginAssemblyCommand(commandService, PackageIds.guidCommandSet.FolderCSharpProjectBuildLoadUpdatePluginAssemblyCommandId, sourceFolder, Properties.CommandNames.FolderCSharpProjectBuildLoadUpdatePluginAssemblyCommand);

            InstanceProject = new CSharpProjectBuildLoadUpdatePluginAssemblyCommand(commandService, PackageIds.guidCommandSet.ProjectBuildLoadUpdatePluginAssemblyCommandId, sourceProject, Properties.CommandNames.ProjectBuildLoadUpdatePluginAssemblyCommand);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedProjects = _sourceSelectedProjects.GetSelectedProjects(helper).ToList();

            helper.HandlePluginAssemblyBuildProjectUpdateCommand(null, false, selectedProjects);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedProjects.CommandBeforeQueryStatus(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, this._commandNameForCorrection);
        }
    }
}