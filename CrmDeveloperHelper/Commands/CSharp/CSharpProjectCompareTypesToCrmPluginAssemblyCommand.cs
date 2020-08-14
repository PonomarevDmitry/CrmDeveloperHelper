using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpProjectCompareTypesToCrmPluginAssemblyCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedProjects _sourceSelectedProjects;
        private readonly string _commandNameForCorrection;

        private CSharpProjectCompareTypesToCrmPluginAssemblyCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedProjects sourceSelectedProjects, string commandNameForCorrection)
            : base(commandService, idCommand)
        {
            this._sourceSelectedProjects = sourceSelectedProjects;
            this._commandNameForCorrection = commandNameForCorrection;
        }

        public static CSharpProjectCompareTypesToCrmPluginAssemblyCommand InstanceCode { get; private set; }

        public static CSharpProjectCompareTypesToCrmPluginAssemblyCommand InstanceDocuments { get; private set; }

        public static CSharpProjectCompareTypesToCrmPluginAssemblyCommand InstanceFile { get; private set; }

        public static CSharpProjectCompareTypesToCrmPluginAssemblyCommand InstanceFolder { get; private set; }

        public static CSharpProjectCompareTypesToCrmPluginAssemblyCommand InstanceProject { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedProjects.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedProjects.CreateSource();

            var sourceFile = FileSourceSelectedProjects.CreateSource();

            var sourceFolder = FolderSourceSelectedProjects.CreateSource();

            var sourceProject = ProjectSourceSelectedProjects.CreateSource();

            InstanceCode = new CSharpProjectCompareTypesToCrmPluginAssemblyCommand(commandService, PackageIds.guidCommandSet.CodeCSharpProjectCompareTypesToCrmPluginAssemblyCommandId, sourceCode, Properties.CommandNames.CodeCSharpProjectCompareTypesToCrmPluginAssemblyCommand);

            InstanceDocuments = new CSharpProjectCompareTypesToCrmPluginAssemblyCommand(commandService, PackageIds.guidCommandSet.DocumentsCSharpProjectCompareTypesToCrmPluginAssemblyCommandId, sourceDocuments, Properties.CommandNames.DocumentsCSharpProjectCompareTypesToCrmPluginAssemblyCommand);

            InstanceFile = new CSharpProjectCompareTypesToCrmPluginAssemblyCommand(commandService, PackageIds.guidCommandSet.FileCSharpProjectCompareTypesToCrmPluginAssemblyCommandId, sourceFile, Properties.CommandNames.FileCSharpProjectCompareTypesToCrmPluginAssemblyCommand);

            InstanceFolder = new CSharpProjectCompareTypesToCrmPluginAssemblyCommand(commandService, PackageIds.guidCommandSet.FolderCSharpProjectCompareTypesToCrmPluginAssemblyCommandId, sourceFolder, Properties.CommandNames.FolderCSharpProjectCompareTypesToCrmPluginAssemblyCommand);

            InstanceProject = new CSharpProjectCompareTypesToCrmPluginAssemblyCommand(commandService, PackageIds.guidCommandSet.ProjectCompareTypesToCrmPluginAssemblyCommandId, sourceProject, Properties.CommandNames.ProjectCompareTypesToCrmPluginAssemblyCommand);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedProjects = _sourceSelectedProjects.GetSelectedProjects(helper).ToList();

            helper.HandlePluginAssemblyComparingPluginTypesWithLocalAssemblyCommand(null, selectedProjects);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedProjects.CommandBeforeQueryStatus(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, this._commandNameForCorrection);
        }
    }
}
