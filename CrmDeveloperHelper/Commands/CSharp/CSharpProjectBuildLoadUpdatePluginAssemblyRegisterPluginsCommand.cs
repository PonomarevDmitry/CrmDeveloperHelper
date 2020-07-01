using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedProjects _sourceSelectedProjects;
        private readonly string _commandNameForCorrection;

        private CSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedProjects sourceSelectedProjects, string commandNameForCorrection)
            : base(commandService, idCommand)
        {
            this._sourceSelectedProjects = sourceSelectedProjects;
            this._commandNameForCorrection = commandNameForCorrection;
        }

        public static CSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand InstanceCode { get; private set; }

        public static CSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand InstanceDocuments { get; private set; }

        public static CSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand InstanceFile { get; private set; }

        public static CSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand InstanceFolder { get; private set; }

        public static CSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand InstanceProject { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedProjects.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedProjects.CreateSource();

            var sourceFile = FileSourceSelectedProjects.CreateSource();

            var sourceFolder = FolderSourceSelectedProjects.CreateSource();

            var sourceProject = ProjectSourceSelectedProjects.CreateSource();

            InstanceCode = new CSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand(commandService, PackageIds.guidCommandSet.CodeCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommandId, sourceCode, Properties.CommandNames.CodeCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand);

            InstanceDocuments = new CSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand(commandService, PackageIds.guidCommandSet.DocumentsCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommandId, sourceDocuments, Properties.CommandNames.DocumentsCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand);

            InstanceFile = new CSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand(commandService, PackageIds.guidCommandSet.FileCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommandId, sourceFile, Properties.CommandNames.FileCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand);

            InstanceFolder = new CSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand(commandService, PackageIds.guidCommandSet.FolderCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommandId, sourceFolder, Properties.CommandNames.FolderCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand);

            InstanceProject = new CSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand(commandService, PackageIds.guidCommandSet.ProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommandId, sourceProject, Properties.CommandNames.ProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedProjects = _sourceSelectedProjects.GetSelectedProjects(helper).ToList();

            helper.HandlePluginAssemblyBuildProjectUpdateCommand(null, true, selectedProjects);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedProjects.CommandBeforeQueryStatus(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, this._commandNameForCorrection);
        }
    }
}