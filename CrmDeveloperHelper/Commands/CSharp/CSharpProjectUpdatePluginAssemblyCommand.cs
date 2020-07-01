using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpProjectUpdatePluginAssemblyCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedProjects _sourceSelectedProjects;
        private readonly string _commandNameForCorrection;

        private CSharpProjectUpdatePluginAssemblyCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedProjects sourceSelectedProjects, string commandNameForCorrection)
            : base(commandService, idCommand)
        {
            this._sourceSelectedProjects = sourceSelectedProjects;
            this._commandNameForCorrection = commandNameForCorrection;
        }

        public static CSharpProjectUpdatePluginAssemblyCommand InstanceCode { get; private set; }

        public static CSharpProjectUpdatePluginAssemblyCommand InstanceDocuments { get; private set; }

        public static CSharpProjectUpdatePluginAssemblyCommand InstanceFile { get; private set; }

        public static CSharpProjectUpdatePluginAssemblyCommand InstanceFolder { get; private set; }

        public static CSharpProjectUpdatePluginAssemblyCommand InstanceProject { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedProjects.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedProjects.CreateSource();

            var sourceFile = FileSourceSelectedProjects.CreateSource();

            var sourceFolder = FolderSourceSelectedProjects.CreateSource();

            var sourceProject = ProjectSourceSelectedProjects.CreateSource();

            InstanceCode = new CSharpProjectUpdatePluginAssemblyCommand(commandService, PackageIds.guidCommandSet.CodeCSharpProjectUpdatePluginAssemblyCommandId, sourceCode, Properties.CommandNames.CodeCSharpProjectUpdatePluginAssemblyCommand);

            InstanceDocuments = new CSharpProjectUpdatePluginAssemblyCommand(commandService, PackageIds.guidCommandSet.DocumentsCSharpProjectUpdatePluginAssemblyCommandId, sourceDocuments, Properties.CommandNames.DocumentsCSharpProjectUpdatePluginAssemblyCommand);

            InstanceFile = new CSharpProjectUpdatePluginAssemblyCommand(commandService, PackageIds.guidCommandSet.FileCSharpProjectUpdatePluginAssemblyCommandId, sourceFile, Properties.CommandNames.FileCSharpProjectUpdatePluginAssemblyCommand);

            InstanceFolder = new CSharpProjectUpdatePluginAssemblyCommand(commandService, PackageIds.guidCommandSet.FolderCSharpProjectUpdatePluginAssemblyCommandId, sourceFolder, Properties.CommandNames.FolderCSharpProjectUpdatePluginAssemblyCommand);

            InstanceProject = new CSharpProjectUpdatePluginAssemblyCommand(commandService, PackageIds.guidCommandSet.ProjectUpdatePluginAssemblyCommandId, sourceProject, Properties.CommandNames.ProjectUpdatePluginAssemblyCommand);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedProjects = _sourceSelectedProjects.GetSelectedProjects(helper).ToList();

            helper.HandlePluginAssemblyUpdatingInWindowCommand(null, selectedProjects);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedProjects.CommandBeforeQueryStatus(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, this._commandNameForCorrection);
        }
    }
}